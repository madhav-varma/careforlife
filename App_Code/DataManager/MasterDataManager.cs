using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MasterDataManager
/// </summary>
public class MasterDataManager
{
    public List<OptionModel> GetAvailableSpecialities(string is_rare = "n")
    {
        var options = new List<OptionModel>();

        var query = "select speciality_id, speciality_name from speciality_master where is_active='y' and is_rare='" + is_rare + "' order by speciality_name";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                options.Add(new OptionModel() { Id = r["speciality_id"].ToString(), Value = r["speciality_name"].ToString() });
            }
        }
        return options;
    }
    public List<OptionModel> GetAvailableEducationSpecialities()
    {
        var options = new List<OptionModel>();

        var query = "select speciality_id, speciality_name from edu_speciality_master where is_active='y' order by speciality_name";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                options.Add(new OptionModel() { Id = r["speciality_id"].ToString(), Value = r["speciality_name"].ToString() });
            }
        }
        return options;
    }


    public List<OptionModel> GetAvailableCities()
    {
        var options = new List<OptionModel>();

        var query = "select city_id, city_name from city_master where is_active='y' order by city_name";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                options.Add(new OptionModel() { Id = r["city_id"].ToString(), Value = r["city_name"].ToString() });
            }
        }
        return options;
    }

    public bool ValidateUser(string username, string password)
    {
        var query = "SELECT count(*) as count FROM user_master where user_type=1 and email_id='" + username + "' and password='" + password + "'";
        var count = new DataAccessManager().ExecuteScalar(query);

        if (count != null)
            return int.Parse(count.ToString()) > 0;
        else
            return false;
    }

    public UserModel GetUser(string username)
    {
        var user = new UserModel();

        var query = "SELECT u.* FROM user_master u where u.email_id='" + username + "'";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(UserModel).GetProperties();

            foreach (var prop in props)
            {
                var ignore = Attribute.IsDefined(prop, typeof(IgnoreUpdate));
                if (!ignore)
                {
                    var attribute = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single();
                    string displayName = attribute.DisplayName;

                    var value = r[displayName];
                    var pType = prop.PropertyType;
                    object v = null;
                    if (pType == typeof(bool))
                    {
                        v = value.ToString() == "y";
                    }
                    else
                    {
                        v = value == null ? null : value;
                    }

                    var variable = prop.SetMethod;

                    if (!r.IsNull(displayName))
                    {
                        variable.Invoke(user, new object[] { v });
                    }

                }
            }
            return user;
        }
        return null;
    }

    public MasterDataManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}