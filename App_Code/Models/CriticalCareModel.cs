﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CriticalCareModel
/// </summary>
/// 
[DisplayName("critical_care_master")]
public class CriticalCareModel
{

    [IgnoreInsert]
    [DisplayName("critical_care_id")]
    public int Id { get; set; }
    [DisplayName("hospital_name")]
    public string Name { get; set; }
    [IgnoreSelect]
    [IgnoreUpdate]
    [DisplayName("img_url")]
    public string Images { get; set; }
    [DisplayName("city_id")]
    public int City { get; set; }
    [DisplayName("available_specialities")]
    public string Specialities { get; set; }
    [DisplayName("available_services")]
    public string Services { get; set; }
    [DisplayName("email_id")]
    public string Email { get; set; }
    [DisplayName("contact_no")]
    public string Mobile { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [DisplayName("city_name")]
    public string CityName { get; set; }
    [DisplayName("address")]
    public string Address { get; set; }
    [DisplayName("is_active")]
    public bool IsActive { get; set; }
    [DisplayName("created_on")]
    public DateTime Created { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [IgnoreSelect]
    public string Link { get; set; }

    public CriticalCareModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}