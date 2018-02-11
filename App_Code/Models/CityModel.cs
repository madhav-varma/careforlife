using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CityModel
/// </summary>
[DisplayName("city_master")]
public class CityModel
{
    [IgnoreInsert]
    [DisplayName("city_id")]
    public int Id { get; set; }
    [DisplayName("city_name")]
    public string Name { get; set; }        
    [DisplayName("state_id")]
    public int StateId { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [DisplayName("state_name")]
    public string StateName { get; set; }
    [DisplayName("doc_count")]
    public string DoctorCount { get; set; }
    [DisplayName("is_active")]
    public bool IsActive { get; set; }
    [DisplayName("created_on")]
    public DateTime Created { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [IgnoreSelect]
    public string Link { get; set; }
    public CityModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}