using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CampManager
/// </summary>
public class CampManager
{
    public List<CampModel> GetAllCamps()
    {
        var camps = new List<CampModel>();
        var query = "select * from " + TABLE_NAME + " where is_active='y' order by created desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var camp = new CampModel();
                var props = typeof(CampModel).GetProperties();

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
                            variable.Invoke(camp, new object[] { v });
                        }

                    }
                }
                camps.Add(camp);
            }
        }
        return camps;
    }

    public CampModel GetCampById(string id)
    {
        var camp = new CampModel();
        var query = "select * from " + TABLE_NAME + " where is_active='y' and " + TABLE_ID + " ='" + id + "' order by created desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(CampModel).GetProperties();

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
                        variable.Invoke(camp, new object[] { v });
                    }

                }
            }
        }
        return camp;
    }

    public string GetCampImagesById(string id)
    {
        var query = "select img_url from " + TABLE_NAME + " where is_active='y' and " + TABLE_ID + " ='" + id + "' order by created desc";
        var url = new DataAccessManager().ExecuteScalar(query);

        return url.ToString();
    }

    public bool UpdateCampImagesById(string id, string fileNames)
    {
        var query = "update " + TABLE_NAME + " set img_url = '" + fileNames + "' where " + TABLE_ID + "  = '" + id + "'";
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);
        return res;
    }

    public bool DeleteCamp(string id)
    {
        var query = string.Format("delete from {0} where {1} = '{2}'", TABLE_NAME, TABLE_ID, id);
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public PagedList<CampModel> GetAllCampsPaginated(int skip, int take, string order, string where)
    {
        var camps = new List<CampModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by cp.created desc " : order;
        var query = "select cp.*, cm.city_name from " + TABLE_NAME + " cp, city_master cm where cp.city_id = cm.city_id and  cp.is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var camp = new CampModel();
                var props = typeof(CampModel).GetProperties();

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
                            variable.Invoke(camp, new object[] { v });
                        }

                    }
                }
                camps.Add(camp);
            }
        }
        query = "select count(*) from " + TABLE_NAME + " cp, city_master cm where cp.city_id = cm.city_id and  cp.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<CampModel>() { TotalCount = count, Data = camps };
        return result;
    }   
    public CampManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private string TABLE_NAME = "camp_master";
    private string TABLE_ID = "id";
}