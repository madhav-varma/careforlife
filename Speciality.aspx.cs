using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Speciality : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionManager.ValidateSession(Session, Response);
        var user = (UserModel)Session["user"];

        this.Master.UsernameHead = user.FullName;
        this.Master.UsernameDD = user.FullName;
    }
    protected void SubmitSpeciality(object sender, EventArgs e)
    {
        try
        {
            var speciality = new SpecialityModel();

            speciality.Name = name.Value;
            speciality.IsRare = chk_rare.Checked;
            speciality.IsActive = true;

            if (Request.Files.Count > 0)
            {
                var files = new List<string>();
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFile f = Request.Files[i];
                    if (f.ContentLength > 0)
                    {
                        var extension = Path.GetExtension(f.FileName);
                        var fileName = Guid.NewGuid().ToString() + "." + extension;
                        files.Add(fileName);

                        string pathToSave_100 = HttpContext.Current.Server.MapPath("~/photo/" + fileName);
                        f.SaveAs(pathToSave_100);
                    }
                }
                if (files.Count > 0)
                    speciality.Image = string.Join(" ", files);
            }

            speciality.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);
            var sqlQuery = new Helper().GetInsertQuery<SpecialityModel>(speciality);
            if (!string.IsNullOrWhiteSpace(speciality_id.Value))
            {
                speciality.Id = int.Parse(speciality_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<SpecialityModel>(speciality);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {
                Response.Redirect("Speciality", true);
            }
        }
        catch (Exception ex)
        {
            action.Value = "Failed To Add Speciality!";
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetSpecialities(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(speciality_name))", "LOWER(TRIM(is_rare))" };
        // Initialization.    
        DataTableData<SpecialityModel> result = new DataTableData<SpecialityModel>();
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

            var specialities = new SpecialityManager().GetAllSpecialitiesPaginated(startRec, pageSize, c_order, c_search);

            var specialityList = specialities.Data;
            foreach (var speciality in specialityList)
            {
                speciality.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-spl' data-id='" + speciality.Id + "'>Edit</a><a href='javascript:void(0);' class='add-spl-images' data-id='" + speciality.Id + "'>Add Images</a><a href='javascript:void(0);' style='margin-left:10px' class='delete-spl' data-id='" + speciality.Id + "'>Delete</a>";
            }

            int recFilter = specialities.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = specialities.TotalCount;
            result.recordsFiltered = specialities.TotalCount;
            result.data = specialityList;
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
    public static object GetSpecialityById(string id)
    {
        var speciality = new SpecialityManager().GetSpecialityById(id);
        return speciality;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetImageById(string id)
    {
        var files = new List<FileInfoModel>();
        var specialityImage = new SpecialityManager().GetSpecialityImageById(id);

        var response = new JsonResponse() { IsSuccess = true, Message = "Files found successfully.", Data = files };

        if (!string.IsNullOrEmpty(specialityImage))
        {
            var images = specialityImage.Split(' ');
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
    public static object DeleteSpecialityById(string id)
    {
        var resp = new JsonResponse() { IsSuccess = true, Message = "Speciality Deleted Successfully." };
        var canDelete = new SpecialityManager().CanDelete(id);
        if (!canDelete)
        {
            resp.IsSuccess = false;
            resp.Message = "There are some records linked to this speciality, so you cannot delete this speciality without deleting them.";
        }
        else
        {
            resp.IsSuccess = new SpecialityManager().DeleteSpeciality(id);
        }

        return resp;
    }
}