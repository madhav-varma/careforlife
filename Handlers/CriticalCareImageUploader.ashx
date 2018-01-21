<%@ WebHandler Language="C#" Class="CriticalCareImageUploader" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

public class CriticalCareImageUploader : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {

        var response = new JsonResponse() { IsSuccess = false, Message = "Files Not uploaded Successfully" };
        context.Response.ContentType = "text/plain";

        var id = context.Request.Form["id"];
        var fnames = context.Request.Form["fnames"];

        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(fnames))
        {
            id = id.Split(',')[0];
            fnames = fnames.Split(',')[0];
            var criticalCareImages = new CriticalCareManager().GetCriticalCareImagesById(id);

            var images = criticalCareImages.Split(' ');
            var imagesToDelete = new List<string>();

            var imagesToAdd = new List<string>();
            foreach (var img in images)
            {
                if (!fnames.Contains(img))
                    imagesToDelete.Add(img);
            }

            foreach (var fname in fnames.Split(' '))
            {
                var img = images.ToList().FirstOrDefault(x => fname.Contains(x));
                if (string.IsNullOrEmpty(img))
                    imagesToAdd.Add(fname);
                else
                    imagesToAdd.Add(img);
            }

            if (imagesToDelete.Count > 0)
            {
                //TODO: delete images from server.
            }

            var uploaded = new CriticalCareManager().UpdateCriticalCareImagesById(id, string.Join(" ", imagesToAdd));
            if (uploaded)
            {
                try
                {
                    foreach (string s in context.Request.Files)
                    {
                        HttpPostedFile file = context.Request.Files[s];
                        var fileName = fnames.Split(' ').ToList().First(x => x.Contains(file.FileName.Replace(" ","")));

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            string pathToSave_100 = HttpContext.Current.Server.MapPath("~/photo/" + fileName);
                            file.SaveAs(pathToSave_100);
                        }
                    }
                    response.IsSuccess = true;
                    response.Message = "Files Uploaded Successfully";
                }
                catch (Exception e)
                {
                    response.Message = e.Message;
                }
            }
        }
        if (response.IsSuccess)
            context.Response.Write(new JavaScriptSerializer().Serialize(response));
        else
        {
            throw new Exception(response.Message);
        }
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}