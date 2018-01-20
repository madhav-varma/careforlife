using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IgnoreInsert
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Property |
                       System.AttributeTargets.Struct)
]
public class IgnoreInsert : System.Attribute
{
    public IgnoreInsert()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}