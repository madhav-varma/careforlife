using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CityManager
/// </summary>
public class CityManager
{
    public CityModel GetCityById(string id)
    {
        var city = new CityModel();
        var query = "select * from " + TABLE_NAME + " where is_active='y' and " + TABLE_ID + "='" + id + "' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(CityModel).GetProperties();

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
                        variable.Invoke(city, new object[] { v });
                    }

                }
            }
        }
        return city;
    }

    public bool DeleteCity(string id)
    {
        var query = string.Format("delete from {0} where {1} = '{2}'", TABLE_NAME, TABLE_ID, id);
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public bool CanDelete(string id)
    {
        var result = false;

        var query = "SELECT count(*) " +
            "FROM city_master cm left outer join state_master sm on sm.state_id = cm.state_id " +
            "left outer join blood_bank_master bbm on cm.city_id = bbm.city_id  and bbm.is_active = 'y' " +
            "left outer join camp_master cmpm on cm.city_id = cmpm.city_id  and cmpm.is_active = 'y' " +
            "left outer join critical_care_master ccm on cm.city_id = ccm.city_id  and ccm.is_active = 'y' " +
            "left outer join path_lab_master plm on cm.city_id = plm.city_id and plm.is_active = 'y' " +
            "left outer join rare_speciality_master rsm on cm.city_id = rsm.city_id and rsm.is_active = 'y' " +
            "left outer join doctor_master dm on cm.city_id = dm.city_id and dm.is_active = 'y' " +
            "left outer join medical_facility_master mfm on cm.city_id = mfm.city_id and mfm.is_active = 'y' " +
            "where cm.is_active = 'y' and sm.is_active = 'y' and cm.city_id='" + id + "' " +
            "and (cmpm.title is not null or bbm.blood_bank_name is not null or ccm.hospital_name is not null or plm.lab_name is not null or" +
            " rsm.hospital_name is not null or dm.doctor_name is not null or mfm.hospital_name is not null)";

        var res = new DataAccessManager().ExecuteScalar(query);

        if (res != null && !string.IsNullOrEmpty(res.ToString()))
        {
            result = int.Parse(res.ToString()) <= 0;
        }
        return result;
    }

    public PagedList<CityModel> GetAllCitiesPaginated(int skip, int take, string order, string where)
    {
        var cities = new List<CityModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by cm.created_on desc " : order;
        var query = "select cm.*, sm.state_name from  " + TABLE_NAME + "  cm, state_master sm where cm.state_id = sm.state_id and  cm.is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var city = new CityModel();
                var props = typeof(CityModel).GetProperties();

                foreach (var prop in props)
                {
                    var ignore = Attribute.IsDefined(prop, typeof(IgnoreSelect));
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
                            variable.Invoke(city, new object[] { v });
                        }

                    }
                }
                cities.Add(city);
            }
        }
        query = "select count(*) from  " + TABLE_NAME + "  cm, state_master sm where sm.state_id = cm.state_id and cm.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<CityModel>() { TotalCount = count, Data = cities };
        return result;
    }

    public CityManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private string TABLE_NAME = "city_master";
    private string TABLE_ID = "city_id";


}