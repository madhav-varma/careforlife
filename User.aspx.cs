using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionManager.ValidateSession(Session, Response);
        var user = (UserModel)Session["user"];

        this.Master.UsernameHead = user.FullName;
        this.Master.UsernameDD = user.FullName;
    }



    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetUsers(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(usr.full_name))", "LOWER(TRIM(usr.email_id))", "LOWER(TRIM(usr.mobile_number))", "LOWER(TRIM(usr.profession))", "LOWER(TRIM(usr.city))", "LOWER(TRIM(usr.state))" };
        // Initialization.    
        DataTableData<UserModel> result = new DataTableData<UserModel>();
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
            var users = new UserManager().GetAllUsersPaginated(startRec, pageSize, c_order, c_search);

            var userList = users.Data;
            foreach (var user in userList)
            {
                var enableClass = user.IsDoctor ? "hidden" : "";
                var disableClass = user.IsDoctor ? "" : "hidden";
                user.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='enable-lns " + enableClass + "' data-id='" + user.Email + "'>Enable</a><a href='javascript:void(0);' class='disable-lns " + disableClass + "' data-id='" + user.Email + "'>Disable</a>";
            }

            int recFilter = users.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = users.TotalCount;
            result.recordsFiltered = users.TotalCount;
            result.data = userList;
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
    public static object GetUserById(string id)
    {
        var user = new UserManager().GetUserById(id);
        return user;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object EnableDisableLearning(bool is_doctor, string id)
    {
        //var isDoctor = is_doctor == "yes";
        var resp = new UserManager().EnableDisableLearningSharing(id, is_doctor);
        return resp;
    }
}