using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

/// <summary>
/// Summary description for SessionManager
/// </summary>
public class SessionManager
{
    public static void ValidateSession(HttpSessionState Session, HttpResponse Response)
    {
        var user = (UserModel)Session["user"];

        if (user == null)
        {
            Response.Redirect("Login.aspx");
        }
    }
    public static void StartSession(HttpSessionState Session, HttpResponse Response, string username)
    {
        var user = new MasterDataManager().GetUser(username);
        Session["user"] = user;

        Response.Redirect("Doctor.aspx");
    }
    public static void StopSession(HttpSessionState Session, HttpResponse Response)
    {
        Session["user"] = null;
        Session.Abandon();

        Response.Redirect("Login.aspx");
    }
    public SessionManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}