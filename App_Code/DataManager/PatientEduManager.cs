using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PatientEduManager
/// </summary>
public class PatientEduManager
{
    public List<PatientEducationModel> GetAllPatientEducations()
    {
        var patientEdus = new List<PatientEducationModel>();
        var query = "select * from " + TABLE_NAME + " where is_active='y' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var patientEdu = new PatientEducationModel();
                var props = typeof(PatientEducationModel).GetProperties();

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
                            variable.Invoke(patientEdu, new object[] { v });
                        }

                    }
                }
                patientEdus.Add(patientEdu);
            }
        }
        return patientEdus;
    }

    public PatientEducationModel GetPatientEducationById(string id)
    {
        var patientEdu = new PatientEducationModel();
        var query = "select * from " + TABLE_NAME + " where is_active='y' and " + TABLE_ID + "='" + id + "' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(PatientEducationModel).GetProperties();

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
                        variable.Invoke(patientEdu, new object[] { v });
                    }

                }
            }
        }
        return patientEdu;
    }

    public string GetPatientEducationImagesById(string id)
    {
        var query = "select img_url from " + TABLE_NAME + " where is_active='y' and" + TABLE_ID + "='" + id + "' order by created_on desc";
        var url = new DataAccessManager().ExecuteScalar(query);

        return url.ToString();
    }

    public bool UpdatePatientEducationImagesById(string id, string fileNames)
    {
        var query = "update " + TABLE_NAME + " set img_url = '" + fileNames + "' where" + TABLE_ID + " = '" + id + "'";
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);
        return res;
    }

    public bool DeletePatientEducation(string id)
    {
        var query = string.Format("delete from {0} where {1} = '{2}'", TABLE_NAME, TABLE_ID, id);
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public PagedList<PatientEducationModel> GetAllPatientEducationsPaginated(int skip, int take, string order, string where)
    {
        var patientEdus = new List<PatientEducationModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by pe.created_on desc " : order;
        var query = "select pe.*, sm.speciality_name from  " + TABLE_NAME + "  pe, edu_speciality_master sm where pe.speciality_id = sm.speciality_id and  pe.is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var patientEdu = new PatientEducationModel();
                var props = typeof(PatientEducationModel).GetProperties();

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
                            variable.Invoke(patientEdu, new object[] { v });
                        }

                    }
                }
                patientEdus.Add(patientEdu);
            }
        }
        query = "select count(*) from  " + TABLE_NAME + "  pe, edu_speciality_master sm where sm.speciality_id = pe.speciality_id and  pe.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<PatientEducationModel>() { TotalCount = count, Data = patientEdus };
        return result;
    }

    public PatientEduManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private string TABLE_NAME = "patient_edu_master";
    private string TABLE_ID = "video_id";
}