using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Doctor
/// </summary>
[DisplayName("doctor_master")]
public class DoctorModel
{
    [IgnoreInsert]
    [DisplayName("doctor_id")]
    public int Id { get; set; }
    [DisplayName("doctor_name")]
    public string Name { get; set; }
    [DisplayName("tagline")]
    public string Tagline { get; set; }
    [IgnoreUpdate]
    [IgnoreSelect]
    [DisplayName("img_url")]
    public string Images { get; set; }
    [DisplayName("degree")]
    public string Degree { get; set; }
    [DisplayName("experience")]
    public string Experience { get; set; }
    [DisplayName("email")]
    public string Email { get; set; }
    [DisplayName("contact_no")]
    public string Mobile { get; set; }
    [DisplayName("speciality_id")]
    public int Speciality { get; set; }
    [DisplayName("city_id")]
    public int City { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [DisplayName("speciality_name")]
    public string SpecialityName { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [DisplayName("city_name")]
    public string CityName { get; set; }
    [DisplayName("timings")]
    public string Timing { get; set; }
    [DisplayName("address")]
    public string Address { get; set; }
    [DisplayName("is_special")]
    public bool IsSpecial { get; set; }
    [DisplayName("is_rare")]
    public bool IsRare { get; set; }
    [DisplayName("services")]
    public string Services { get; set; }
    [DisplayName("created_on")]
    public DateTime Created { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [IgnoreSelect]
    public string Link { get; set; }

    public DoctorModel()
    {

    }
}