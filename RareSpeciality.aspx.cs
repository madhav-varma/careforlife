using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RareSpeciality : System.Web.UI.Page
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

    protected void SubmitRareSpeciality(object sender, EventArgs e)
    {
        try
        {
            var rare = new RareSpecialityModel();

            rare.City = int.Parse(city.Value);
            rare.IsActive = true;
            rare.Mobile = mobile.Value;

            var specialitiesKeys = Request.Form.AllKeys.Where(x => x.Contains("service")).ToList();
            var specialities = new List<string>();
            foreach (var key in specialitiesKeys)
            {
                var i = key.Replace("service", "");
                specialities.Add(Request.Form["service" + i]);
            }
            rare.Specialities = string.Join("\n", specialities);

            var locations = new List<string>();
            var hospitalKeys = Request.Form.AllKeys.Where(x => x.Contains("hospital")).ToList();
            foreach (var key in hospitalKeys)
            {
                var i = key.Replace("hospital", "");
                var hospital = Request.Form["hospital" + i];
                var address = Request.Form["address" + i];
                var from = Request.Form["timingFrom" + i];
                var to = Request.Form["timingTo" + i];

                var timing = "{" + string.Format("\"hospital\":\"{0}\", \"Address\":\"{1}\", \"timing\":\"{2} - {3}\"", hospital, address, from, to) + "}";
                locations.Add(timing);
            }

            rare.Address = "[" + string.Join(",", locations) + "]";
       
            rare.Name = name.Value;

            rare.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);



            var sqlQuery = new Helper().GetInsertQuery<RareSpecialityModel>(rare);
            if (!string.IsNullOrWhiteSpace(rare_speciality_id.Value))
            {
                rare.Id = int.Parse(rare_speciality_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<RareSpecialityModel>(rare);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {
                msg = "RareSpeciality Added Successfully!";
                Response.Redirect("RareSpeciality", true);
            }
        }
        catch (Exception ex)
        {
            msg = "Failed To Add RareSpeciality!";
            action.Value = msg;
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetRares(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(rm.hospital_name))", "LOWER(TRIM(rm.email_id))", "dm.mobile", "LOWER(TRIM(cm.city_name))" };
        // Initialization.    
        DataTableData<RareSpecialityModel> result = new DataTableData<RareSpecialityModel>();
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

            var rares = new RareSpecialityManager().GetAllRaresPaginated(startRec, pageSize, c_order, c_search);

            var rarelist = rares.Data;
            foreach (var rare in rarelist)
            {
                rare.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-rare' data-id='" + rare.Id + "'>Edit</a><a href='javascript:void(0);' class='add-rare-images' data-id='" + rare.Id + "'>Add Images</a>";
            }

            int recFilter = rares.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = rares.TotalCount;
            result.recordsFiltered = rares.TotalCount;
            result.data = rarelist;
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
    public static object GetRareSpecialityById(string id)
    {
        var rare = new RareSpecialityManager().GetRareSpecialityById(id);
        return rare;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetImagesById(string id)
    {
        var files = new List<FileInfoModel>();
        var rareImages = new RareSpecialityManager().GetRareSpecialityImagesById(id);

        var response = new JsonResponse() { IsSuccess = false, Message = "Files found successfully.", Data = files };

        if (!string.IsNullOrEmpty(rareImages))
        {
            var images = rareImages.Split(' ');
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
}