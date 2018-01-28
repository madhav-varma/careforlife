using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MedicalFacility : System.Web.UI.Page
{
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
    protected void SubmitMedicalFacility(object sender, EventArgs e)
    {
        try
        {
            var medicalFacility = new MedicalFacilityModel();

            medicalFacility.City = int.Parse(city.Value);
            medicalFacility.Address = address.Value;
            medicalFacility.Email = email.Value;
            medicalFacility.IsActive = true;
            medicalFacility.Mobile = mobile.Value;
            medicalFacility.Name = name.Value;
            medicalFacility.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);            

            var sqlQuery = new Helper().GetInsertQuery<MedicalFacilityModel>(medicalFacility);
            if (!string.IsNullOrWhiteSpace(facility_id.Value))
            {
                medicalFacility.Id = int.Parse(facility_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<MedicalFacilityModel>(medicalFacility);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {
                Response.Redirect("MedicalFacility", true);
            }
        }
        catch (Exception ex)
        {
            action.Value = "Failed To Add Medical Facility!";
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetMedicalFacilities(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(mf.hospital_name))", "LOWER(TRIM(mf.address))", "LOWER(TRIM(mf.email_id))", "LOWER(TRIM(mf.contact_no))", "LOWER(TRIM(cm.city_name))" };
        // Initialization.    
        DataTableData<MedicalFacilityModel> result = new DataTableData<MedicalFacilityModel>();
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

            var medicalFacilities = new MedicalFacilityManager().GetAllMedicalFacilitiesPaginated(startRec, pageSize, c_order, c_search);

            var medicalFacilityList = medicalFacilities.Data;
            foreach (var medicalFacility in medicalFacilityList)
            {
                medicalFacility.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-mf' data-id='" + medicalFacility.Id + "'>Edit</a><a href='javascript:void(0);' class='add-mf-images' data-id='" + medicalFacility.Id + "'>Add Images</a><a href='javascript:void(0);' style='margin-left:10px' class='delete-mf' data-id='" + medicalFacility.Id + "'>Delete</a>";
            }

            int recFilter = medicalFacilities.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = medicalFacilities.TotalCount;
            result.recordsFiltered = medicalFacilities.TotalCount;
            result.data = medicalFacilityList;
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
    public static object GetMedicalFacilityById(string id)
    {
        var mf = new MedicalFacilityManager().GetMedicalFacilityById(id);
        return mf;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetImagesById(string id)
    {
        var files = new List<FileInfoModel>();
        var medicalFacilityImages = new MedicalFacilityManager().GetMedicalFacilityImagesById(id);

        var response = new JsonResponse() { IsSuccess = true, Message = "Files found successfully.", Data= files };

        if (!string.IsNullOrEmpty(medicalFacilityImages))
        {
            var images = medicalFacilityImages.Split(' ');
            try
            {
                foreach (var item in images)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string absFile = HttpContext.Current.Server.MapPath("/photo/" + item);
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object DeleteMedicalFacilityById(string id)
    {
        var resp = new MedicalFacilityManager().DeleteMedicalFacility(id);
        return resp;
    }
}