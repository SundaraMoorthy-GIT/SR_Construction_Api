using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Genuine_API.Common
{
    public class dbFunctions
    {



        public static void Logs(string Error, string Created_by)
        {

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = "insert into errorlog(Created_by,Created_Date,Details,Error) Values(@Created_by,@Created_Date,@Details,@Error)";
                com.Parameters.Add("@Created_by", SqlDbType.VarChar).Value = Created_by;
                com.Parameters.Add("@Created_Date", SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                com.Parameters.Add("@Details", SqlDbType.VarChar).Value = "";
                com.Parameters.Add("@Error", SqlDbType.NVarChar).Value = Error;

                com.ExecuteNonQuery();
            }
            catch { }
        }


    }
}