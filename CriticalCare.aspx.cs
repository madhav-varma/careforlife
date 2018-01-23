﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CriticalCare : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionManager.ValidateSession(Session, Response);
        var user = (UserModel)Session["user"];

        var cities = new MasterDataManager().GetAvailableCities();
        foreach (var c in cities)
        {
            city.Items.Add(new ListItem(c.Value, c.Id));
        }
    }

    protected void SubmitCriticalCare(object sender, EventArgs e)
    {
        try
        {
            var criticalCare = new CriticalCareModel();

            criticalCare.City = int.Parse(city.Value);
            criticalCare.Address = address.Value;
            criticalCare.Email = email.Value;
            criticalCare.IsActive = true;
            criticalCare.Mobile = mobile.Value;
            criticalCare.Name = name.Value;
            criticalCare.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            var servicesKeys = Request.Form.AllKeys.Where(x => x.Contains("service")).ToList();
            var services = new List<string>();
            foreach (var key in servicesKeys)
            {
                var i = key.Replace("service", "");
                services.Add(Request.Form["service" + i]);
            }
            criticalCare.Services = string.Join("\n", services);

            var specialityKeys = Request.Form.AllKeys.Where(x => x.Contains("speciality")).ToList();
            var specialities = new List<string>();
            foreach (var key in specialityKeys)
            {
                var i = key.Replace("speciality", "");
                specialities.Add(Request.Form["speciality" + i]);
            }
            criticalCare.Specialities = string.Join("\n", specialities);

            var sqlQuery = new Helper().GetInsertQuery<CriticalCareModel>(criticalCare);
            if (!string.IsNullOrWhiteSpace(cc_id.Value))
            {
                criticalCare.Id = int.Parse(cc_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<CriticalCareModel>(criticalCare);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {
                Response.Redirect("CriticalCare", true);
            }
        }
        catch (Exception ex)
        {
            action.Value = "Failed To Add Doctor!";
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetCriticalCares(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(cc.hospital_name))", "LOWER(TRIM(cc.email_id))", "LOWER(TRIM(cc.contact_no))", "LOWER(TRIM(cm.city_name))" };
        // Initialization.    
        DataTableData<CriticalCareModel> result = new DataTableData<CriticalCareModel>();
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

            var criticalCares = new CriticalCareManager().GetAllCriticalCaresPaginated(startRec, pageSize, c_order, c_search);

            var criticalCareList = criticalCares.Data;
            foreach (var criticalCare in criticalCareList)
            {
                criticalCare.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-cc' data-id='" + criticalCare.Id + "'>Edit</a><a href='javascript:void(0);' class='add-cc-images' data-id='" + criticalCare.Id + "'>Add Images</a>";
            }

            int recFilter = criticalCares.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = criticalCares.TotalCount;
            result.recordsFiltered = criticalCares.TotalCount;
            result.data = criticalCareList;
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
    public static object GetCriticalCareById(string id)
    {
        var cc = new CriticalCareManager().GetCriticalCareById(id);
        return cc;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetImagesById(string id)
    {
        var files = new List<FileInfoModel>();
        var criticalCareImages = new CriticalCareManager().GetCriticalCareImagesById(id);

        var response = new JsonResponse() { IsSuccess = true, Message = "Files found successfully.", Data = files };

        if (!string.IsNullOrEmpty(criticalCareImages))
        {
            var images = criticalCareImages.Split(' ');
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