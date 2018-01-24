using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BloodBank : System.Web.UI.Page
{
    public string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionManager.ValidateSession(Session, Response);
        var user = (UserModel)Session["user"];

        this.Master.UsernameHead = user.FullName;
        this.Master.UsernameDD = user.FullName;

        var cities = new MasterDataManager().GetAvailableCities();
        foreach (var c in cities)
        {
            city.Items.Add(new ListItem(c.Value, c.Id));
        }
    }

    protected void SubmitBloodBank(object sender, EventArgs e)
    {
        try
        {
            var bloodBank = new BloodBankModel();

            bloodBank.City = int.Parse(city.Value);
            bloodBank.Email = email.Value;
            bloodBank.Mobile = mobile.Value;
            bloodBank.Address = address.Value;
            bloodBank.Timing = timingFrom.Value + " to " + timingTo.Value;
            bloodBank.OpeningYear = opening_year.Value;

            bloodBank.Name = blood_bank_name.Value;
            bloodBank.IsActive = true;
            bloodBank.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            var sqlQuery = new Helper().GetInsertQuery<BloodBankModel>(bloodBank);
            if (!string.IsNullOrWhiteSpace(blood_bank_id.Value))
            {
                bloodBank.Id = int.Parse(blood_bank_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<BloodBankModel>(bloodBank);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {
                msg = "BloodBank Added Successfully!";
                Response.Redirect("BloodBank", true);
            }
        }
        catch (Exception ex)
        {
            msg = "Failed To Add BloodBank!";
            action.Value = msg;
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetBloodBanks(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(bm.blood_bank_name))", "LOWER(TRIM(bm.email_id))", "LOWER(TRIM(bm.contact_no))", "LOWER(TRIM(bm.year_of_opening))", "LOWER(TRIM(bm.timings))", "LOWER(TRIM(cm.city_name))" };
        // Initialization.    
        DataTableData<BloodBankModel> result = new DataTableData<BloodBankModel>();
        try
        {
            // Initialization.                
            string draw = model.draw.ToString();
            int startRec = model.start;
            int pageSize = model.length;

            var c_order = "";
            foreach (var o in model.order)
            {
                var columnName = cols[o.column];
                c_order += string.IsNullOrWhiteSpace(c_order) ? columnName + " " + o.dir : ", " + columnName + " " + o.dir;

            }
            if (!string.IsNullOrWhiteSpace(c_order))
            {
                c_order = " order by " + c_order;
            }

            var c_search = "";
            foreach (var s in model.columns)
            {
                if (!string.IsNullOrWhiteSpace(s.search.value) && s.searchable)
                {
                    var i = model.columns.IndexOf(s);
                    var columnName = cols[i];
                    c_search += i == 1 ? " and " + columnName + " like '%" + s.search.value.Trim().ToLower() + "%'" : " and " + columnName + " like '%" + s.search.value + "%'";
                }
            }
            var bloodBanks = new BloodBankManager().GetAllBloodBanksPaginated(startRec, pageSize, c_order, c_search);

            var bloodBankList = bloodBanks.Data;
            foreach (var bloodBank in bloodBankList)
            {
                bloodBank.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-bloodbank' data-id='" + bloodBank.Id + "'>Edit</a><a href='javascript:void(0);' class='add-bloodbank-images' data-id='" + bloodBank.Id + "'>Add Images</a>";
            }

            int recFilter = bloodBanks.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = bloodBanks.TotalCount;
            result.recordsFiltered = bloodBanks.TotalCount;
            result.data = bloodBankList;
        }
        catch (Exception ex)
        {
            // Info    
            Console.Write(ex);
        }
        // Return info.    
        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetBloodBankById(string id)
    {
        var bloodBank = new BloodBankManager().GetBloodBankById(id);
        return bloodBank;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetImagesById(string id)
    {
        var files = new List<FileInfoModel>();
        var bloodBank = new BloodBankManager().GetBloodBankImagesById(id);

        var response = new JsonResponse() { IsSuccess = true, Message = "Files found successfully.", Data = files };

        if (!string.IsNullOrEmpty(bloodBank))
        {
            try
            {
                var images = bloodBank.Split(' ');
                foreach (var item in images)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string absFile = HttpContext.Current.Server.MapPath("/photo/" + item);
                        //var f = File.Open(absFile, FileMode.Open);
                        var fs = new FileStream(absFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        using (var sr = new StreamReader(fs))
                        {
                            var size = fs.Length;
                            files.Add(new FileInfoModel()
                            {
                                Name = item,
                                Size = size.ToString(),
                                Type = "image"
                            });
                        }
                    }
                }                
                response.Data = files;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }
        }
        return response;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object UploadImagesById(string id)
    {
        var files = new List<FileInfoModel>();
        var bloodBank = new BloodBankManager().GetBloodBankImagesById(id);

        if (!string.IsNullOrEmpty(bloodBank))
        {
            var images = bloodBank.Split(' ');
            foreach (var item in images)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string absFile = HttpContext.Current.Server.MapPath("/photo/" + item);
                    //var f = File.Open(absFile, FileMode.Open);
                    var fs = new FileStream(absFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (var sr = new StreamReader(fs))
                    {
                        var size = fs.Length;
                        files.Add(new FileInfoModel()
                        {
                            Name = item,
                            Size = size.ToString(),
                            Type = "image"
                        });
                    }

                }
            }


        }

        return files;
    }

}