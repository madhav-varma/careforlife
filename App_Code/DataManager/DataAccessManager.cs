using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DataAccessManager
/// </summary>
public class DataAccessManager
{
    public DataRowCollection ExecuteSelectQuery(string query)
    {
        using (MySqlConnection con = C4LDatabase.GetConnection())
        {
            var cmd = new MySqlCommand();

            try
            {
                cmd.Connection = con;
                cmd.CommandText = query;

                cmd.CommandType = System.Data.CommandType.Text;

                DataSet ds = new DataSet();
                MySqlDataAdapter ad = new MySqlDataAdapter();
                ad.SelectCommand = cmd;

                con.Open();
                ad.Fill(ds);

                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows;
                }
            }
            catch (Exception ex)
            {
                //Logger.Error(ex.Message);
                //Logger.Error(ex.StackTrace);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        return null;
    }
    public bool ExecuteInsertUpdateQuery(string query)
    {
        using (MySqlConnection con = C4LDatabase.GetConnection())
        {
            var cmd = new MySqlCommand();

            try
            {
                cmd.Connection = con;
                cmd.CommandText = query;

                cmd.CommandType = System.Data.CommandType.Text;
                con.Open();

                int result = cmd.ExecuteNonQuery();
                return result == 1;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex.Message);
                //Logger.Error(ex.StackTrace);
                return false;
            }
            finally
            {
                cmd.Dispose();
            }
        }        
    }
    public object ExecuteScalar(string query)
    {
        using (MySqlConnection con = C4LDatabase.GetConnection())
        {
            var cmd = new MySqlCommand();

            try
            {
                cmd.Connection = con;
                cmd.CommandText = query;
                cmd.CommandType = System.Data.CommandType.Text;

                con.Open();
                var result = cmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                //Logger.Error(ex.Message);
                //Logger.Error(ex.StackTrace);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        return null;
    }
    public DataAccessManager()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}