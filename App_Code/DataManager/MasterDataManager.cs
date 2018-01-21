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
   
    public MasterDataManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}