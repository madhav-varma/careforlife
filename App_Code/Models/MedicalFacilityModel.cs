﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MedicalFacilityModel
/// </summary>
/// 
[DisplayName("medical_facility_master")]
public class MedicalFacilityModel
{

    [IgnoreInsert]
    [DisplayName("medical_facility_id")]
    public int Id { get; set; }
    [DisplayName("hospital_name")]
    public string Name { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [DisplayName("img_url")]
    public string ImgUrl { get; set; }
    [DisplayName("city_id")]
    public int City { get; set; }
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
    public bool IsSpecial { get; set; }
    [DisplayName("created_on")]
    public DateTime Created { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [IgnoreSelect]
    public string Link { get; set; }

    public MedicalFacilityModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}