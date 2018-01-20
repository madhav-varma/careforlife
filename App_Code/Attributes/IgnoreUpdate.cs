using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IgnoreUpdate
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Property |  
                       System.AttributeTargets.Struct)  
]  
public class IgnoreUpdate : System.Attribute
{
    public IgnoreUpdate()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}