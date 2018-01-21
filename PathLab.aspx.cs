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
            doc.Mobile = telephone.Value;

            var servicesKeys = Request.Form.AllKeys.Where(x => x.Contains("services")).ToList();
            var services = new List<string>();
            foreach (var key in servicesKeys)
            {
                services.Add(Request.Form[key]);
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
            doc.Services = string.Join(" ", services);

            doc.Speciality = int.Parse(speciality.Value);
            doc.Tagline = tagline.Value;
            doc.Name = name.Value;

            doc.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            var insertQuery = new Helper().GetInsertQuery<DoctorModel>(doc);


            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(insertQuery);
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
        var cols = new List<string>() { "a.modified", "LOWER(TRIM(CONCAT(a.last_name, ' ', a.first_name, ' ', a.middle_name)))", "a.email", "a.mobile", "a.aadhar_card", "t.status" };
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
                c_order = string.IsNullOrWhiteSpace(c_order) ? columnName + " " + o.dir : ", " + columnName + " " + o.dir;

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
                    c_search = i == 1 ? " and " + columnName + " like '%" + s.search.value.Trim().ToLower() + "%'" : " and " + columnName + " like '%" + s.search.value + "%'";
                }
            }
            var q = "select t.*, a.aadhar_card, a.id, a.email, a.first_name, a.middle_name, a.last_name, a.alt_fname, a.alt_mname, a.alt_lname, a.mobile, a.modified, a.post_applied from applicant a, transaction_details t where a.id=t.applicant_id " + c_search + c_order + " limit " + pageSize + " offset " + startRec;
            var countq = "select count(*) from applicant a, transaction_details t where a.id=t.applicant_id " + c_search + c_order;

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
        var doc = new DoctorManager().GetDoctorImagesById(id);

        if (!string.IsNullOrEmpty(doc))
        {
            var images = doc.Split(' ');
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

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object UploadImagesById(string id)
    {
        var files = new List<FileInfoModel>();
        var doc = new DoctorManager().GetDoctorImagesById(id);

        if (!string.IsNullOrEmpty(doc))
        {
            var images = doc.Split(' ');
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