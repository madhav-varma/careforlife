using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CampModel
/// </summary>
[DisplayName("camp_master")]
public class CampModel
{
    [IgnoreInsert]
    [DisplayName("id")]
    public int Id { get; set; }
    [DisplayName("title")]
    public string Name { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [DisplayName("img_url")]
    public string ImgUrl { get; set; }
    [DisplayName("city_id")]
    public int City { get; set; }
    [DisplayName("description")]
    public string Description { get; set; }
    [DisplayName("timing")]
    public string Timing { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [DisplayName("city_name")]
    public string CityName { get; set; }
    [DisplayName("address")]
    public string Address { get; set; }
    [DisplayName("organizer")]
    public string Organizer { get; set; }
    [DisplayName("is_active")]
    public bool IsActive { get; set; }
    [DisplayName("desc_1")]
    public string Description1 { get; set; }
    [DisplayName("desc_2")]
    public string Description2 { get; set; }
    [DisplayName("created")]
    public DateTime Created { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [IgnoreSelect]
    public string Link { get; set; }
    public CampModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}