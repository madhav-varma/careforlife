using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BloodBankManager
/// </summary>
public class BloodBankManager
{
    public List<BloodBankModel> GetAllBloodBank()
    {
        var bloodBanks = new List<BloodBankModel>();
        var query = "select * from blood_bank_master where is_active='y' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var bloodBank = new BloodBankModel();
                var props = typeof(BloodBankModel).GetProperties();

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
                            variable.Invoke(bloodBank, new object[] { v });
                        }

                    }
                }
                bloodBanks.Add(bloodBank);
            }
        }
        return bloodBanks;
    }

    public BloodBankModel GetBloodBankById(string id)
    {
        var bloodBank = new BloodBankModel();
        var query = "select * from blood_bank_master where is_active='y' and blood_bank_id='" + id + "' order by created_on desc";
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            var r = rows[0];

            var props = typeof(BloodBankModel).GetProperties();

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
                        variable.Invoke(bloodBank, new object[] { v });
                    }

                }
            }
        }
        return bloodBank;
    }

    public string GetBloodBankImagesById(string id)
    {
        var query = "select img_url from blood_bank_master where is_active='y' and blood_bank_id='" + id + "' order by created_on desc";
        var url = new DataAccessManager().ExecuteScalar(query);

        return url.ToString();
    }

    public bool UpdateBloodBankImagesById(string id, string fileNames)
    {
        var query = "update blood_bank_master set img_url = '" + fileNames + "' where blood_bank_id = '" + id + "'";
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);
        return res;
    }

    public bool DeleteBloodBank(string id)
    {
        var query = string.Format("delete from {0} where {1} = '{2}'", TABLE_NAME, TABLE_ID, id);
        var res = new DataAccessManager().ExecuteInsertUpdateQuery(query);

        return res;
    }

    public PagedList<BloodBankModel> GetAllBloodBanksPaginated(int skip, int take, string order, string where)
    {
        var bloodBanks = new List<BloodBankModel>();
        var orderBy = string.IsNullOrEmpty(order) ? " order by bm.created_on desc " : order;
        var query = "select bm.*, cm.city_name from blood_bank_master bm, city_master cm where bm.city_id = cm.city_id and  bm.is_active='y' " + where + orderBy + " limit " + take + " offset " + skip;
        var rows = new DataAccessManager().ExecuteSelectQuery(query);
        if (rows != null && rows.Count > 0)
        {
            foreach (DataRow r in rows)
            {
                var bloodBank = new BloodBankModel();
                var props = typeof(BloodBankModel).GetProperties();

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
                            variable.Invoke(bloodBank, new object[] { v });
                        }

                    }
                }
                bloodBanks.Add(bloodBank);
            }
        }
        query = "select count(*) from blood_bank_master bm, city_master cm where bm.city_id = cm.city_id and  bm.is_active='y' " + where;
        var totalCount = new DataAccessManager().ExecuteScalar(query);
        var count = totalCount != null ? int.Parse(totalCount.ToString()) : 0;

        var result = new PagedList<BloodBankModel>() { TotalCount = count, Data = bloodBanks };
        return result;
    }

    public BloodBankManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private string TABLE_NAME = "blood_bank_master";
    private string TABLE_ID = "blood_bank_id";
}