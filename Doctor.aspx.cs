using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Doctor : System.Web.UI.Page
{
    public string msg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionManager.ValidateSession(Session, Response);
        var user = (UserModel)Session["user"];

        this.Master.UsernameHead = user.FullName;
        this.Master.UsernameDD = user.FullName;

        var specialities = new MasterDataManager().GetAvailableSpecialities();
        foreach (var sp in specialities)
        {
            speciality.Items.Add(new ListItem(sp.Value, sp.Id));
        }
        var cities = new MasterDataManager().GetAvailableCities();
        foreach (var c in cities)
        {
            city.Items.Add(new ListItem(c.Value, c.Id));
        }
    }

    protected void SubmitDoctor(object sender, EventArgs e)
    {
        try
        {
            var doc = new DoctorModel();

            doc.City = int.Parse(city.Value);
            doc.Degree = degree.Value;
            doc.Experience = experience.Value;
            doc.IsSpecial = true;
            doc.Mobile = mobile.Value;

            var servicesKeys = Request.Form.AllKeys.Where(x => x.Contains("service")).ToList();
            var services = new List<string>();
            foreach (var key in servicesKeys)
            {
                var i = key.Replace("service", "");
                services.Add(Request.Form["service" + i]);
            }
            doc.Services = string.Join("\n", services);

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
            doc.Timing = "[" + string.Join(",", locations) + "]";

            doc.Speciality = int.Parse(speciality.Value);
            doc.Tagline = tagline.Value;
            doc.Name = name.Value;

            doc.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);



            var sqlQuery = new Helper().GetInsertQuery<DoctorModel>(doc);
            if (!string.IsNullOrWhiteSpace(doctor_id.Value)) {
                doc.Id = int.Parse(doctor_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<DoctorModel>(doc);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {
                msg = "Doctor Added Successfully!";
                Response.Redirect("Doctor", true);
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
    public static object GetDoctors(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(dm.doctor_name))", "LOWER(TRIM(dm.tagline))", "LOWER(TRIM(dm.degree))", "dm.experience", "dm.mobile", "LOWER(TRIM(sm.speciality_name))", "LOWER(TRIM(cm.city_name))" };
        // Initialization.    
        DataTableData<DoctorModel> result = new DataTableData<DoctorModel>();
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

            var docs = new DoctorManager().GetAllDoctorsPaginated(startRec, pageSize, c_order, c_search);

            var doclist = docs.Data;
            foreach (var doc in doclist)
            {
                doc.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-doc' data-id='" + doc.Id + "'>Edit</a><a href='javascript:void(0);' class='add-doc-images' data-id='" + doc.Id + "'>Add Images</a>";
            }

            int recFilter = docs.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = docs.TotalCount;
            result.recordsFiltered = docs.TotalCount;
            result.data = doclist;
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
    public static object GetDoctorById(string id)
    {
        var doc = new DoctorManager().GetDoctorById(id);
        return doc;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetImagesById(string id)
    {
        var files = new List<FileInfoModel>();
        var docImages = new DoctorManager().GetDoctorImagesById(id);

        var response = new JsonResponse() { IsSuccess = true, Message= "Files found successfully.", Data = files };

        if (!string.IsNullOrEmpty(docImages))
        {
            var images = docImages.Split(' ');
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
            catch(Exception e)
            {
                response.IsSuccess = false;
                response.Message = e.Message;
            }
        }
        return response;
    }
}