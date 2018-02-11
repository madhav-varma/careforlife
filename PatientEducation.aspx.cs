using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PatientEducation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionManager.ValidateSession(Session, Response);
        var user = (UserModel)Session["user"];

        this.Master.UsernameHead = user.FullName;
        this.Master.UsernameDD = user.FullName;

        var specialities = new MasterDataManager().GetAvailableSpecialities();
        foreach (var s in specialities)
        {
            speciality.Items.Add(new ListItem(s.Value, s.Id));
        }
    }

    protected void SubmitPatientVideo(object sender, EventArgs e)
    {
        try
        {
            var patientVideo = new PatientEducationModel();

            patientVideo.Name = video_name.Value;
            patientVideo.Url = video_url.Value;
            patientVideo.Speciality = int.Parse(speciality.Value);

            patientVideo.IsActive = true;
            patientVideo.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            var sqlQuery = new Helper().GetInsertQuery<PatientEducationModel>(patientVideo);
            if (!string.IsNullOrWhiteSpace(video_id.Value))
            {
                patientVideo.Id = int.Parse(video_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<PatientEducationModel>(patientVideo);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {         
                Response.Redirect("PatientEducation", true);
            }
        }
        catch (Exception ex)
        {            
            action.Value = "Failed To Add Video!";
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetVideos(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(pe.video_name))", "LOWER(TRIM(pe.video_url))", "LOWER(TRIM(sm.speciality_name))" };
        // Initialization.    
        DataTableData<PatientEducationModel> result = new DataTableData<PatientEducationModel>();
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
            var patientVideos = new PatientEduManager().GetAllPatientEducationsPaginated(startRec, pageSize, c_order, c_search);

            var patientVideoList = patientVideos.Data;
            foreach (var patientVideo in patientVideoList)
            {
                patientVideo.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-pe' data-id='" + patientVideo.Id + "'>Edit</a><a href='javascript:void(0);' style='margin-left:10px' class='delete-pe' data-id='" + patientVideo.Id + "'>Delete</a>";
            }

            int recFilter = patientVideos.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = patientVideos.TotalCount;
            result.recordsFiltered = patientVideos.TotalCount;
            result.data = patientVideoList;
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
    public static object GetVideoById(string id)
    {
        var patientVideo = new PatientEduManager().GetPatientEducationById(id);
        return patientVideo;
    }
    

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object DeleteVideoById(string id)
    {
        var resp = new PatientEduManager().DeletePatientEducation(id);
        return resp;
    }
}