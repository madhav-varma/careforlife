using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for IDataModel
/// </summary>
public interface IDataModel
{
    Guid Id { get; set; }
    Guid Created { get; set; }
    Guid IsActive { get; set; }
}