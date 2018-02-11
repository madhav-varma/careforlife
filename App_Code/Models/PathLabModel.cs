using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PathLabModel
/// </summary
/// 
[DisplayName("path_lab_master")]
public class PathLabModel
{

    [IgnoreInsert]
    [DisplayName("path_lab_id")]
    public int Id { get; set; }
    [DisplayName("lab_name")]
    public string Name { get; set; }
    [IgnoreSelect]
    [IgnoreUpdate]
    [DisplayName("img_url")]
    public string Images { get; set; }
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
    [DisplayName("timings")]
    public string Timing { get; set; }
    [DisplayName("address")]
    public string Address { get; set; }
    [DisplayName("is_active")]
    public bool IsActive { get; set; }
    [DisplayName("year_of_opening")]
    public string OpeningYear { get; set; }
    [DisplayName("created_on")]
    public DateTime Created { get; set; }
    [IgnoreInsert]
    [IgnoreUpdate]
    [IgnoreSelect]
    public string Link { get; set; }

    public PathLabModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}