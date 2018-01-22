using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PathLab : System.Web.UI.Page
{
    public string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        var cities = new MasterDataManager().GetAvailableCities();
        foreach (var c in cities)
        {
            city.Items.Add(new ListItem(c.Value, c.Id));
        }
    }
    protected void SubmitPathLab(object sender, EventArgs e)
    {
        try
        {
            var pathLab = new PathLabModel();

            pathLab.City = int.Parse(city.Value);
            pathLab.Email = email.Value;
            pathLab.Mobile = mobile.Value;
            pathLab.Address = address.Value;
            pathLab.Timing = timingFrom.Value + " to " + timingTo.Value;
            pathLab.OpeningYear = opening_year.Value;

            pathLab.Name = lab_name.Value;
            pathLab.IsActive = true;
            pathLab.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            var sqlQuery = new Helper().GetInsertQuery<PathLabModel>(pathLab);
            if (!string.IsNullOrWhiteSpace(path_lab_id.Value))
            {
                pathLab.Id = int.Parse(path_lab_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<PathLabModel>(pathLab);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {
                msg = "PathLab Added Successfully!";
                Response.Redirect("PathLab", true);
            }
        }
        catch (Exception ex)
        {
            msg = "Failed To Add Doctor!";
            action.Value = msg;
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetPathLabs(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(pm.lab_name))", "LOWER(TRIM(pm.email_id))", "LOWER(TRIM(pm.contact_no))", "LOWER(TRIM(pm.year_of_opening))", "LOWER(TRIM(pm.timings))", "LOWER(TRIM(cm.city_name))" };
        // Initialization.    
        DataTableData<PathLabModel> result = new DataTableData<PathLabModel>();
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
            var pathLabs = new PathLabManager().GetAllPathLabPaginated(startRec, pageSize, c_order, c_search);

            var pathLabList = pathLabs.Data;
            foreach (var pathLab in pathLabList)
            {
                pathLab.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-pathlab' data-id='" + pathLab.Id + "'>Edit</a><a href='javascript:void(0);' class='add-pathlab-images' data-id='" + pathLab.Id + "'>Add Images</a>";
            }

            int recFilter = pathLabs.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = pathLabs.TotalCount;
            result.recordsFiltered = pathLabs.TotalCount;
            result.data = pathLabList;
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
    public static object GetPathLabById(string id)
    {
        var pathLab = new PathLabManager().GetPathLabById(id);
        return pathLab;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetImagesById(string id)
    {
        var files = new List<FileInfoModel>();
        var pathLab = new PathLabManager().GetPathLabImagesById(id);

        var response = new JsonResponse() { IsSuccess = false, Message = "Error while getting images." };

        if (!string.IsNullOrEmpty(pathLab))
        {
            try
            {
                var images = pathLab.Split(' ');
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
                response.IsSuccess = true;
                response.Message = "Files found successfully";
                response.Data = files;
            }
            catch (Exception e)
            {
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
        var pathLab = new PathLabManager().GetPathLabImagesById(id);

        if (!string.IsNullOrEmpty(pathLab))
        {
            var images = pathLab.Split(' ');
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