using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Camp : System.Web.UI.Page
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

    protected void SubmitCamp(object sender, EventArgs e)
    {
        try
        {
            var camp = new CampModel();

            camp.City = int.Parse(city.Value);
            camp.Organizer = organizer.Value;
            camp.Name = camp_title.Value;
            camp.Address = address.Value;
            camp.Timing = timingFrom.Value + " - " + timingTo.Value;
            camp.Description = description.Value;
            camp.Description1 = description1.Value;
            camp.Description2 = description2.Value;

            if (Request.Files.Count > 0)
            {
                var files = new List<string>();
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFile f = Request.Files[i];
                    var extension = Path.GetExtension(f.FileName);
                    var fileName = Guid.NewGuid().ToString() + "." + extension;
                    files.Add(fileName);

                    string pathToSave_100 = HttpContext.Current.Server.MapPath("~/photo/" + fileName);
                    f.SaveAs(pathToSave_100);
                }
                camp.Images = string.Join(" ", files);
            }


            camp.IsActive = true;
            camp.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            var sqlQuery = new Helper().GetInsertQuery<CampModel>(camp);
            if (!string.IsNullOrWhiteSpace(camp_id.Value))
            {
                camp.Id = int.Parse(camp_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<CampModel>(camp);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {                
                Response.Redirect("Camp", true);
            }
        }
        catch (Exception ex)
        {            
            action.Value = "Failed To Add Camp!";
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetCamps(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(cp.title))", "LOWER(TRIM(cp.description))", "LOWER(TRIM(cp.address))", "LOWER(TRIM(cp.timing))", "LOWER(TRIM(cm.city_name))" };
        // Initialization.    
        DataTableData<CampModel> result = new DataTableData<CampModel>();
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
            var camps = new CampManager().GetAllCampsPaginated(startRec, pageSize, c_order, c_search);

            var campList = camps.Data;
            foreach (var camp in campList)
            {
                camp.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-cp' data-id='" + camp.Id + "'>Edit</a><a href='javascript:void(0);' class='add-cp-images' data-id='" + camp.Id + "'>Add Images</a><a href='javascript:void(0);' style='margin-left:10px' class='delete-cp' data-id='" + camp.Id + "'>Delete</a>";
            }

            int recFilter = camps.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = camps.TotalCount;
            result.recordsFiltered = camps.TotalCount;
            result.data = campList;
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
    public static object GetCampById(string id)
    {
        var camp = new CampManager().GetCampById(id);
        return camp;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetImagesById(string id)
    {
        var files = new List<FileInfoModel>();
        var campImages = new CampManager().GetCampImagesById(id);

        var response = new JsonResponse() { IsSuccess = true, Message = "Files found successfully.", Data = files };

        if (!string.IsNullOrEmpty(campImages))
        {
            try
            {
                var images = campImages.Split(' ');
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
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object DeleteCampById(string id)
    {
        var resp = new CampManager().DeleteCamp(id);
        return resp;
    }
}