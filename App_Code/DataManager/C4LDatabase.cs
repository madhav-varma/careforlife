using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for C4LDatabase
/// </summary>
public class C4LDatabase
{
    public static MySqlConnection GetConnection()
    {
        return new MySqlConnection(ConfigurationManager.ConnectionStrings["local"].ConnectionString);
    }
    public C4LDatabase()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}