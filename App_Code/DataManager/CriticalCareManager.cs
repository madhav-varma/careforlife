using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

/// <summary>
/// Summary description for CriticalCareManager
/// </summary>
public class CriticalCareManager
{
    public List<CriticalCareModel> GetAllCriticalCares()
    {
        var critical_cares = new List<CriticalCareModel>();

        var query = "select * from critical_care_master where is_active='y' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var critical_care = new CriticalCareModel();
                var props = typeof(CriticalCareModel).GetProperties();

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
                            variable.Invoke(critical_care, new object[] { v });
                        }

                    }
                }
                critical_cares.Add(critical_care);
            }
        }
        return critical_cares;
    }

    public CriticalCareModel GetCriticalCareById(string id)
    {
        var critical_care = new CriticalCareModel();

        var query = "select * from critical_care_master where is_active='y' and critical_care_id='" + id + "' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(CriticalCareModel).GetProperties();

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
                        variable.Invoke(critical_care, new object[] { v });
                    }

                }
            }
        }
        return critical_care;
    }

    public string GetCriticalCareImagesById(string id)
    {
        var query = "select img_url from critical_care_master where is_active='y' and critical_care_id='" + id + "' order by created_on desc";
        var url = new DataAccessManager().ExecuteScalar(query);

        return url.ToString();
    }

    public bool UpdateCriticalCareImagesById(string id, string fileNames)
    {
        var query = "update critical_care_master set img_url = '" + fileNames + "' where critical_care_id = '" + id + "'";
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public PagedList<CriticalCareModel> GetAllCriticalCaresPaginated(int skip, int take, string order, string where)
    {
        var criticalCares = new List<CriticalCareModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by cc.created_on desc " : order;
        var query = "select cc.*, cm.city_name from critical_care_master cc, city_master cm where cc.city_id = cm.city_id and cc.is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var criticalCare = new CriticalCareModel();
                var props = typeof(CriticalCareModel).GetProperties();

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
                            variable.Invoke(criticalCare, new object[] { v });
                        }

                    }
                }
                criticalCares.Add(criticalCare);
            }
        }
        query = "select count(*) from critical_care_master cc, city_master cm where cc.city_id = cm.city_id and cc.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<CriticalCareModel>() { TotalCount = count, Data = criticalCares };
        return result;
    }

    public CriticalCareManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}