using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SpecialityModel
/// </summary>
[DisplayName("speciality_master")]
public class SpecialityModel
{
    [IgnoreInsert]
    [DisplayName("speciality_id")]
    public int Id { get; set; }
    [DisplayName("speciality_name")]
    public string Name { get; set; }
    [IgnoreUpdate]
    [IgnoreSelect]
    [DisplayName("img_url")]
    public string Image { get; set; }
    [DisplayName("is_rare")]
    public string IsRareString { get; set; }
    [DisplayName("is_rare")]
    public bool IsRare { get; set; }
    [DisplayName("is_active")]
    public bool IsActive { get; set; }
    [DisplayName("created_on")]
    public DateTime Created { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [IgnoreSelect]
    public string Link { get; set; }
    public SpecialityModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}