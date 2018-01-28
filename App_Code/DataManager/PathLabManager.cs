using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PathLabManager
/// </summary>
public class PathLabManager
{
    public List<PathLabModel> GetAllPathLab()
    {
        var pathlabs = new List<PathLabModel>();
        var query = "select * from path_lab_master where is_active='y' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var pathlab = new PathLabModel();
                var props = typeof(PathLabModel).GetProperties();

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
                            variable.Invoke(pathlab, new object[] { v });
                        }

                    }
                }
                pathlabs.Add(pathlab);
            }
        }
        return pathlabs;

    }

    public PathLabModel GetPathLabById(string id)
    {
        var pathlab = new PathLabModel();
        var query = "select * from path_lab_master where is_active='y' and path_lab_id='" + id + "' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(PathLabModel).GetProperties();

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
                        variable.Invoke(pathlab, new object[] { v });
                    }

                }
            }
        }
        return pathlab;
    }

    public string GetPathLabImagesById(string id)
    {
        var query = "select img_url from path_lab_master where is_active='y' and path_lab_id='" + id + "' order by created_on desc";
        var url = new DataAccessManager().ExecuteScalar(query);

        return url.ToString();
    }

    public bool UpdatePathLabImageById(string id, string fileNames)
    {
        var query = "update path_lab_master set img_url = '" + fileNames + "' where path_lab_id = '" + id + "'";
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public bool DeletePathLab(string id)
    {
        var query = string.Format("delete from {0} where {1} = '{2}'", TABLE_NAME, TABLE_ID, id);
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public PagedList<PathLabModel> GetAllPathLabPaginated(int skip, int take, string order, string where)
    {
        var pathlabs = new List<PathLabModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by pm.created_on desc " : order;
        var query = "select pm.*, cm.city_name from path_lab_master pm, city_master cm where pm.city_id = cm.city_id and  pm.is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var pathlab = new PathLabModel();
                var props = typeof(PathLabModel).GetProperties();

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
                            variable.Invoke(pathlab, new object[] { v });
                        }

                    }
                }
                pathlabs.Add(pathlab);
            }
        }
        query = "select count(*) from path_lab_master pm, city_master cm where pm.city_id = cm.city_id and  pm.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<PathLabModel>() { TotalCount = count, Data = pathlabs };
        return result;

    }

    public PathLabManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private string TABLE_NAME = "path_lab_master";
    private string TABLE_ID = "path_lab_id";
}