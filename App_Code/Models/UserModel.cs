using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserModel
/// </summary>
[DisplayName("user_master")]
public class UserModel
{
    [DisplayName("email_id")]
    public string Email { get; set; }
    [DisplayName("full_name")]
    public string FullName { get; set; }
    [DisplayName("mobile_number")]
    public string Mobile { get; set; }
    [DisplayName("profession")]
    public string Profession { get; set; }
    [DisplayName("city")]
    public string City { get; set; }
    [DisplayName("state")]
    public string State { get; set; }
    [DisplayName("is_active")]
    public string IsActive { get; set; }
    [DisplayName("created_on")]
    public DateTime Created { get; set; }
    public UserModel()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}