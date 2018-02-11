using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SpecialityManager
/// </summary>
public class SpecialityManager
{
    public SpecialityModel GetSpecialityById(string id)
    {
        var speciality = new SpecialityModel();
        var query = "select * from " + TABLE_NAME + " where is_active='y' and " + TABLE_ID + " ='" + id + "' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(SpecialityModel).GetProperties();

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
                        variable.Invoke(speciality, new object[] { v });
                    }

                }
            }
        }
        return speciality;
    }

    public string GetSpecialityImageById(string id)
    {
        var query = "select img_url from " + TABLE_NAME + " where is_active='y' and " + TABLE_ID + " ='" + id + "' order by created_on desc";
        var url = new DataAccessManager().ExecuteScalar(query);

        return url.ToString();
    }

    public bool UpdateSpecialityImageById(string id, string fileName)
    {
        var query = "update " + TABLE_NAME + " set img_url = '" + fileName + "' where " + TABLE_ID + "  = '" + id + "'";
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);
        return res;
    }

    public bool CanDelete(string id)
    {
        var result = false;

        var query = "SELECT count(*) " +
            "FROM speciality_master sm " +
            "left outer join doctor_master dm on sm.speciality_id = dm.speciality_id and dm.is_active = 'y' " +
            "left outer join patient_edu_master pem on sm.speciality_id = pem.speciality_id and pem.is_active = 'y' " +
            "where sm.is_active = 'y' and sm.speciality_id='" + id + "' " +
            "and (dm.doctor_name is not null or pem.video_name is not null)";

        var res = new DataAccessManager().ExecuteScalar(query);

        if (res != null && !string.IsNullOrEmpty(res.ToString()))
        {
            result = int.Parse(res.ToString()) <= 0;
        }
        return result;
    }

    public bool DeleteSpeciality(string id)
    {
        var query = string.Format("delete from {0} where {1} = '{2}'", TABLE_NAME, TABLE_ID, id);
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public PagedList<SpecialityModel> GetAllSpecialitiesPaginated(int skip, int take, string order, string where)
    {
        var specialities = new List<SpecialityModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by created_on desc " : order;
        var query = "select * from " + TABLE_NAME + " where is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var speciality = new SpecialityModel();
                var props = typeof(SpecialityModel).GetProperties();

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
                            variable.Invoke(speciality, new object[] { v });
                        }

                    }
                }
                specialities.Add(speciality);
            }
        }
        query = "select count(*) from " + TABLE_NAME + " sm where sm.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<SpecialityModel>() { TotalCount = count, Data = specialities };
        return result;
    }
    public SpecialityManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private string TABLE_NAME = "speciality_master";
    private string TABLE_ID = "speciality_id";
}