using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for JsonResponse
/// </summary>
public class JsonResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public JsonResponse()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}