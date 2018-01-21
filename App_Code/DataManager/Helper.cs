using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Helper
/// </summary>
public class Helper
{
    public string GetInsertQuery<T>(T entity)
    {
        var columns = new List<string>();
        var values = new List<string>();

        var en = entity.GetType();
        var tableName = en.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single().DisplayName;

        var props = en.GetProperties();

        foreach (var prop in props)
        {
            var ignore = Attribute.IsDefined(prop, typeof(IgnoreInsert));

            if (!ignore)
            {
                var attribute = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single();
                string displayName = attribute.DisplayName;
                //if (prop.Name != "Id")
                //{
                var variable = prop.GetMethod;
                var val = variable.Invoke(entity, null);
                if (val != null)
                {
                    columns.Add(displayName);

                    var pType = val.GetType();
                    if (pType == typeof(bool))
                    {
                        values.Add((bool)val ? "'y'" : "'n'");
                    }
                    else if (pType == typeof(DateTime))
                    {
                        values.Add("'" + ((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss") + "'");
                    }
                    else
                    {
                        values.Add(val != null ? "'" + val.ToString() + "'" : "''");
                    }
                }
                //}
            }
        }

        var insertQuery = string.Format("insert into {0} ({1}) values ({2})", tableName, string.Join(",", columns), string.Join(",", values));
        return insertQuery;
    }
    public string GetUpdateQuery<T>(T entity)
    {
        var columns = new List<string>();

        var en = entity.GetType();
        var tableName = en.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single().DisplayName;

        var props = en.GetProperties();
        var id = "";
        var db_id = "";



        foreach (var prop in props)
        {
            var ignore = Attribute.IsDefined(prop, typeof(IgnoreUpdate));

            if (!ignore)
            {
                var attribute = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().Single();
                string displayName = attribute.DisplayName;
                if (prop.Name == "Id")
                {
                    var variable = prop.GetMethod;
                    id = variable.Invoke(entity, null).ToString();
                    db_id = displayName;
                }
                else
                {
                    var variable = prop.GetMethod;
                    var val = variable.Invoke(entity, null);

                    if (val != null)
                    {
                        var value = "";
                        var pType = val.GetType();
                        if (pType == typeof(bool))
                        {
                            value = (bool)val ? "'y'" : "'n'";
                        }
                        else if (pType == typeof(DateTime))
                        {
                            value = "'" + ((DateTime)val).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        }
                        else
                        {
                            value = val != null ? "'" + val.ToString() + "'" : "''";
                        }
                        columns.Add(string.Format("{0}={1}", displayName, value));
                    }
                }
            }
        }

        var insertQuery = string.Format("update {0} set {1} where {2}='{3}'", tableName, string.Join(",", columns), db_id, id);
        return insertQuery;
    }
    public Helper()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}