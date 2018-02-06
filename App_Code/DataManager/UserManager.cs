using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserManager
/// </summary>
public class UserManager
{
    public bool EnableDisableLearningSharing(string id, bool isDoctor)
    {
        var lns = isDoctor ? 'y' : 'n';
        var query = "update " + TABLE_NAME + " set is_doctor = '" + lns + "' where " + TABLE_ID + " = '" + id + "'";
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }
    public PagedList<UserModel> GetAllUsersPaginated(int skip, int take, string order, string where)
    {
        var users = new List<UserModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by usr.created_on desc " : order;
        var query = "select usr.* from " + TABLE_NAME + " usr where usr.is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var user = new UserModel();
                var props = typeof(UserModel).GetProperties();

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
                            variable.Invoke(user, new object[] { v });
                        }

                    }
                }
                users.Add(user);
            }
        }
        query = "select count(*) from " + TABLE_NAME + " usr where usr.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<UserModel>() { TotalCount = count, Data = users };
        return result;
    }
    public UserModel GetUserById(string id)
    {
        var user = new UserModel();
        var query = "select * from " + TABLE_NAME + " where is_active='y' and " + TABLE_ID + " ='" + id + "' order by created_on desc";
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
                        variable.Invoke(user, new object[] { v });
                    }

                }
            }
        }
        return user;
    }
    public UserManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private string TABLE_NAME = "user_master";
    private string TABLE_ID = "email_id";
}