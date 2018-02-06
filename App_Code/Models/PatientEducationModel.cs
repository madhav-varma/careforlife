using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PatientEducationModel
/// </summary>
[DisplayName("patient_edu_master")]
public class PatientEducationModel
{
    [IgnoreInsert]
    [DisplayName("video_id")]
    public int Id { get; set; }
    [DisplayName("video_name")]
    public string Name { get; set; }
    [DisplayName("video_url")]
    public string Url { get; set; }
    [DisplayName("created_on")]
    public DateTime Created { get; set; }
    [DisplayName("is_active")]
    public bool IsActive { get; set; }
    [DisplayName("speciality_id")]
    public int Speciality { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [DisplayName("speciality_name")]
    public string SpecialityName { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [IgnoreSelect]
    public string Link { get; set; }

    public PatientEducationModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}