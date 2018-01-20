using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PagedList
/// </summary>
public class PagedList<T>
{
    public int TotalCount { get; set; }
    public List<T> Data { get; set; }
}