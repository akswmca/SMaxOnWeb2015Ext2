using System;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Sand64.Cryptography;


namespace WebAPI.Controllers
{
    internal class DALDatabase 
    {
        SqlConnection theCon;
        SqlConnection theConReadOnly;
        SqlDatabase db = null;
        SqlDatabase dbReadOnly = null;
        
        public DALDatabase()
        {

            String strCS = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
            String strCSRO = ConfigurationManager.ConnectionStrings["DBConnectionStringReadOnly"].ToString();

            strCS = Decryptor.DecryptString(strCS, "3w8motherw4fdcj7");
            strCSRO = Decryptor.DecryptString(strCSRO, "3w8motherw4fdcj7");


            db = new SqlDatabase(strCS);
            dbReadOnly = new SqlDatabase(strCSRO);

            theCon = new SqlConnection(strCS);
            theConReadOnly = new SqlConnection(strCSRO);

            //db = new SqlDatabase(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
            //dbReadOnly = new SqlDatabase(ConfigurationManager.ConnectionStrings["DBConnectionStringReadOnly"].ToString());

            //theCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
            //theConReadOnly = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionStringReadOnly"].ToString());
            //WebAPI.App_Code.DALDatabase a = new WebAPI.App_Code.DALDatabase();            
        }

        private static void PrepareCommand(SqlCommand cmd, SqlParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    SqlParameter parm = (SqlParameter)cmdParms[i];
                    cmd.Parameters.Add(parm);
                }
            }
        }

        

















    }
}