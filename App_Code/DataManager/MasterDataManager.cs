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
    public List<OptionModel> GetAvailableSpecialities()
    {
        var options = new List<OptionModel>();

        var query = "select speciality_id, speciality_name from speciality_master where is_active='y' order by speciality_name";
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
    public List<DoctorModel> GetAllDoctors()
    {
        var docs = new List<DoctorModel>();

        var query = "select * from doctor_master where is_active='y' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var doc = new DoctorModel();
                var props = typeof(DoctorModel).GetProperties();

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
                            variable.Invoke(doc, new object[] { v });
                        }

                    }
                }
                docs.Add(doc);
            }
        }
        return docs;
    }
    public DoctorModel GetDoctorById(string id)
    {
        var doc = new DoctorModel();

        var query = "select * from doctor_master where is_active='y' and doctor_id='" + id + "' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(DoctorModel).GetProperties();

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
                        variable.Invoke(doc, new object[] { v });
                    }

                }
            }
        }
        return doc;
    }
    public string GetDoctorImagesById(string id)
    {
        var query = "select img_url from doctor_master where is_active='y' and doctor_id='" + id + "' order by created_on desc";
        var url = new DataAccessManager().ExecuteScalar(query);
        
        return url.ToString();
    }
    public PagedList<DoctorModel> GetAllDoctorsPaginated(int skip, int take)
    {
        var docs = new List<DoctorModel>();

        var query = "select dm.*, sm.speciality_name, cm.city_name from doctor_master dm, city_master cm, speciality_master sm where dm.city_id = cm.city_id and sm.speciality_id=dm.speciality_id and  dm.is_active='y' order by dm.created_on desc limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var doc = new DoctorModel();
                var props = typeof(DoctorModel).GetProperties();

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
                            variable.Invoke(doc, new object[] { v });
                        }

                    }
                }
                docs.Add(doc);
            }
        }
        query = "select count(*) from doctor_master where is_active='y'";
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<DoctorModel>() { TotalCount = count, Data = docs };
        return result;
    }
    public MasterDataManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}