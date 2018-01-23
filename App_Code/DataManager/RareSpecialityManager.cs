using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RareSpecialityManager
/// </summary>
public class RareSpecialityManager
{

    public List<RareSpecialityModel> GetAllRares()
    {
        var rares = new List<RareSpecialityModel>();

        var query = "select * from rare_speciality_master where is_active='y' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var rare = new RareSpecialityModel();
                var props = typeof(RareSpecialityModel).GetProperties();

                foreach (var prop in props)
                {
                    var ignore = Attribute.IsDefined(prop, typeof(IgnoreInsert));
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
                            variable.Invoke(rare, new object[] { v });
                        }

                    }
                }
                rares.Add(rare);
            }
        }
        return rares;
    }

    public RareSpecialityModel GetRareSpecialityById(string id)
    {
        var rare = new RareSpecialityModel();

        var query = "select * from rare_speciaity_master where is_active='y' and rare_speciality_id='" + id + "' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(RareSpecialityModel).GetProperties();

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
                        variable.Invoke(rare, new object[] { v });
                    }

                }
            }
        }
        return rare;
    }

    public string GetRareSpecialityImagesById(string id)
    {
        var query = "select img_url from rare_speciality_master where is_active='y' and rare_speciality_id='" + id + "' order by created_on desc";
        var url = new DataAccessManager().ExecuteScalar(query);

        return url.ToString();
    }

    public bool UpdateRareSpecialityImagesById(string id, string fileNames)
    {
        var query = "update rare_speciality_master set img_url = '" + fileNames + "' where rare_speciality_id = '" + id + "'";
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public PagedList<RareSpecialityModel> GetAllRaresPaginated(int skip, int take, string order, string where)
    {
        var rares = new List<RareSpecialityModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by rm.created_on desc " : order;
        var query = "select rm.*, cm.city_name from rare_speciality_master rm, city_master cm where rm.city_id = cm.city_id and  rm.is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var rare = new RareSpecialityModel();
                var props = typeof(RareSpecialityModel).GetProperties();

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
                            variable.Invoke(rare, new object[] { v });
                        }

                    }
                }
                rares.Add(rare);
            }
        }
        query = "select count(*) from rare_speciality_master rm, city_master cm where rm.city_id = cm.city_id and  rm.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<RareSpecialityModel>() { TotalCount = count, Data = rares };
        return result;
    }

    public RareSpecialityManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}