using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MedicalFacilityManager
/// </summary>
public class MedicalFacilityManager
{
    public List<MedicalFacilityModel> GetAllMedicalFacilities()
    {
        var medicalFacilities = new List<MedicalFacilityModel>();

        var query = "select * from medical_facility_master where is_active='y' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var medicalFacility = new MedicalFacilityModel();
                var props = typeof(MedicalFacilityModel).GetProperties();

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
                            variable.Invoke(medicalFacility, new object[] { v });
                        }

                    }
                }
                medicalFacilities.Add(medicalFacility);
            }
        }
        return medicalFacilities;
    }

    public MedicalFacilityModel GetMedicalFacilityById(string id)
    {
        var medicalFacility = new MedicalFacilityModel();

        var query = "select * from medical_facility_master where is_active='y' and medical_facility_id='" + id + "' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(MedicalFacilityModel).GetProperties();

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
                        variable.Invoke(medicalFacility, new object[] { v });
                    }

                }
            }
        }
        return medicalFacility;
    }

    public string GetMedicalFacilityImagesById(string id)
    {
        var query = "select img_url from medical_facility_master where is_active='y' and medical_facility_id='" + id + "' order by created_on desc";
        var url = new DataAccessManager().ExecuteScalar(query);

        return url.ToString();
    }

    public bool DeleteMedicalFacility(string id)
    {
        var query = string.Format("delete from {0} where {1} = '{2}'", TABLE_NAME, TABLE_ID, id);
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public bool UpdateMedicalFacilityImagesById(string id, string fileNames)
    {
        var query = "update medical_facility_master set img_url = '" + fileNames + "' where medical_facility_id = '" + id + "'";
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public PagedList<MedicalFacilityModel> GetAllMedicalFacilitiesPaginated(int skip, int take, string order, string where)
    {
        var medicalFacilities = new List<MedicalFacilityModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by mf.created_on desc " : order;
        var query = "select mf.*, cm.city_name from medical_facility_master mf, city_master cm where mf.city_id = cm.city_id and mf.is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var medicalFacility = new MedicalFacilityModel();
                var props = typeof(MedicalFacilityModel).GetProperties();

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
                            variable.Invoke(medicalFacility, new object[] { v });
                        }

                    }
                }
                medicalFacilities.Add(medicalFacility);
            }
        }
        query = "select count(*) from medical_facility_master mf, city_master cm where mf.city_id = cm.city_id and mf.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<MedicalFacilityModel>() { TotalCount = count, Data = medicalFacilities };
        return result;
    }

    public MedicalFacilityManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private string TABLE_NAME = "medical_facility_master";
    private string TABLE_ID = "medical_facility_id";
}