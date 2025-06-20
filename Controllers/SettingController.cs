using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Web.Configuration;

namespace API.Controllers
{
   
    public class SettingController : ApiController
    {

        [HttpGet]
        public string get_User_Name(string Company)
        {

            string Query = "select UM_ID as User_ID,UM_User_Name as User_Name from user_master  where UM_Status='A' and UM_Company=" + Company.Replace("_", "");
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpGet]
        public string get_Branch_Name(string Company)
        {

            string Query = "select ID as value,Branch_Name as label from Branch_Details  where Status='A' ";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpGet]
        public string get_server_Location()
        {

            string uploadPath = "~/Image/";
            return System.Web.HttpContext.Current.Server.MapPath(uploadPath);

        }




        [HttpGet]
        public string get_Files()
        {

            try
            {
                string[] filePaths = Directory.GetFiles(@"C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\");

                string s = "";
                for (int i = 0; i < filePaths.Length; i++)
                {
                    s += filePaths[i];
                }
                return s;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }




        [HttpGet]
        public string Get_User_Area(string Rights,string User, string Company)
        {
            string Query = "";
            if (Rights.ToLower().ToString() == "admin")
            {

                Query = "select x.*,(select [RG_vCode]  from Reference_Group where RG_iID=RGV_IRG_ID ) as Ref_ID ,RGV_vDesciption as label, RGV_IID as  [value] " +
                         " from  ReferenceGroup_Value x  " +
                         " where  (select [RG_vCode]  from Reference_Group where RG_iID=RGV_IRG_ID ) ='Area' ";

            }
            else
            {
                Query = "select x.*,(select [RG_vCode]  from Reference_Group where RG_iID=RGV_IRG_ID ) as Ref_ID ,RGV_vDesciption as label, RGV_IID as  [value] " +
                              " from  ReferenceGroup_Value x  " +
                              " left outer join User_Area on x.RGV_iID=Area_ID " +
                              " where  (select [RG_vCode]  from Reference_Group where RG_iID=RGV_IRG_ID ) ='Area'  and User_ID=" + User;
            }
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            if (dt.Rows.Count > 0)
            {
                string data = GITAPI.dbFunctions.GetJSONString(dt);
                return data;
            }
            else
            {
                return "[]";
            }

        }


        [Authorize]
        [HttpGet]
        public string get_ModuleName1(string Company)
        {

            string Query = "select distinct Module as label,Module as value from Menu_Master";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }


        [HttpGet]
        public string get_Notification(string topic,string Company)
        {

            string Query = "select * from Notification_Tables"+Company +"  where topic='Buhari_Hotel' order  by ID " ;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }




        [Authorize]
        [HttpGet]
        public string get_ModuleName(string Company)
        {

            string Query = "select distinct Module as label,Module as value from Menu_Master";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }



        [HttpGet]
        public string get_Branch_Details(string Company = "0")
        {

            string condi = "";

            string Query = "select * from Branch_Details where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string Delete_Branch_Details(string ID, string UserName, string Company)
        {
            string q = "UPDATE Branch_Details SET Status='D',Delete_Date='" + GITAPI.dbFunctions.getdate() + "',Delete_By='" + UserName + "'  where ID=" + ID;
            GITAPI.dbFunctions.getTable(q);
            return "True";
        }


        [HttpPost]
        public string Post_Branch_Master(JObject jsonData)
        {
            dynamic json = jsonData;
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");
            string ID = isnull(json.ID, "0");
            string User = isnull(json.Created_by, "");
            string ColumnPerfix = isnull(json.ColumnPerfix, "");
            string Query = "";
            Boolean isIDn = false;

            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='" + Table_Name + "'");

            DataTable dc = GITAPI.dbFunctions.getTable("SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = '" + Table_Name + "'");
            int l = 0;
            if (dc.Rows.Count > 0)
            {
                isIDn = true;
                l = 1;
            }

            int auto = 0;
            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {

                    auto = 0;

                    if (isIDn == false)
                    {
                        DataTable dd = GITAPI.dbFunctions.getTable("select isnull(max(" + dt.Rows[0]["Column_Name"].ToString() + "),0)+1 from " + Table_Name);
                        if (dd.Rows.Count > 0)
                        {
                            json[dt.Rows[0]["Column_Name"].ToString()] = dd.Rows[0][0].ToString();
                            ID = dd.Rows[0][0].ToString();
                        }
                    }

                    Query += "insert into " + Table_Name + " (";

                    for (int i = l; i < dt.Rows.Count - 1; i++)
                    {

                        Query += dt.Rows[i]["Column_Name"].ToString() + ",";

                    }
                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += ") Values ( ";


                    for (int i = l; i < dt.Rows.Count - 1; i++)
                    {

                        Query += "@" + dt.Rows[i]["Column_Name"].ToString() + ",";

                    }
                    Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += " )";


                    com.CommandText = Query;


                }
                else
                {
                    auto = 1;
                    Query = "";
                    Query += "update  " + Table_Name + " Set ";

                    for (int i = l; i < dt.Rows.Count - 1; i++)
                    {
                        string col = dt.Rows[i]["Column_Name"].ToString();
                        if (col.ToLower().Equals(ColumnPerfix + "created_date"))
                        { }
                        else if (col.ToLower().Equals(ColumnPerfix + "created_by"))
                        { }
                        else
                        {
                            Query += dt.Rows[i]["Column_Name"].ToString() + "=@" + dt.Rows[i]["Column_Name"].ToString() + ", ";
                        }

                    }


                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "=@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + " ";

                    Query += "where  " + dt.Rows[0]["Column_Name"].ToString() + "=@" + dt.Rows[0]["Column_Name"].ToString() + " ";


                    com.CommandText = Query;
                    if (l == 1)
                    {
                        com.Parameters.Add("@" + dt.Rows[0]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[0]["Column_Name"].ToString()], "0");
                    }
                }


                Query = "";
                for (int i = l; i < dt.Rows.Count; i++)
                {

                    string DF_value = "";
                    string Column_Type = dt.Rows[i]["Data_Type"].ToString().ToUpper();
                    string TB_DF_value = dt.Rows[i]["Column_Default"].ToString().ToUpper();
                    TB_DF_value = TB_DF_value.Replace("'", "").Replace("(", "").Replace(")", "");
                    if (Column_Type.Equals("VARCHAR") || Column_Type.Equals("NVARCHAR") || Column_Type.Equals("CHAR") || Column_Type.Equals("NCHAR"))
                    {
                        DF_value = "";

                    }

                    else if (Column_Type.Equals("DECIMAL") || Column_Type.Equals("FLOAT") || Column_Type.Equals("NUMERIC"))
                    {
                        DF_value = "0";

                    }
                    else
                        DF_value = "";

                    if (TB_DF_value != "")
                        DF_value = TB_DF_value;


                    string Column = dt.Rows[i]["Column_Name"].ToString();
                    string TempQ = "";
                    if (Column.ToLower().Equals(ColumnPerfix + "id"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = ID;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "created_date") && auto == 0)
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = "A";
                    }

                    else if (Column.ToLower().Equals(ColumnPerfix + "created_by") && auto == 0)
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "company"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Company.Replace("_", "");
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "del_by") && auto == 1)
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "del_date") && auto == 1)
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                    }
                    else
                    {
                        if (Column_Type.Equals("NVARCHAR"))
                        {
                            com.Parameters.Add("@" + Column, SqlDbType.NVarChar).Value = isnull(json[Column], DF_value);
                        }
                        else
                        {
                            com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = isnull(json[Column], DF_value);
                        }
                    }


                }
                com.ExecuteNonQuery();
                com.Connection.Close();

                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string isnull(dynamic data, string d1)
        {

            if (string.IsNullOrEmpty(Convert.ToString(data)))
            {
                return d1;
            }
            else
            {
                return data;
            }
        }


        [HttpPost]
        public string insert_PDF_File_Setting(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string Page_Size = isnull(json.Page_Size, "");
            string Page_Orientation = isnull(json.Page_Orientation, "");
            string Compnay_Name = isnull(json.Compnay_Name, "");
            string Compnay_Address = isnull(json.Compnay_Address, "");
            string Generated_By = isnull(json.Generated_By, "");
            string Generated_Date = isnull(json.Generated_Date, "");
            string Generated_Time = isnull(json.Generated_Time, "");
            string Page_No = isnull(json.Page_No, "");
            string Style = isnull(json.Style, "");
            string Font_Family = isnull(json.Font_Family, "");
            string Font_Size = isnull(json.Font_Size, "");
            string Margins_Top = isnull(json.Margins_Top, "");
            string Margins_Left = isnull(json.Margins_Left, "");
            string Margins_Right = isnull(json.Margins_Right, "");
            string Margins_Bottom = isnull(json.Margins_Bottom, "");
            string Update_by = isnull(json.Update_by, "");
            string Update_Date = isnull(json.Update_Date, "");
            string Status = isnull(json.Status, "");
            string Company = isnull(json.Company, "");

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {
                    com.CommandText = "insert into PDF_File_Setting (Page_Size,Page_Orientation,Compnay_Name,Compnay_Address,Generated_By,Generated_Date,Generated_Time,Page_No,Style,Font_Family,Font_Size,Margins_Top,Margins_Left,Margins_Right,Margins_Bottom,Update_by,Update_Date,Status) Values ( @Page_Size,@Page_Orientation,@Compnay_Name,@Compnay_Address,@Generated_By,@Generated_Date,@Generated_Time,@Page_No,@Style,@Font_Family,@Font_Size,@Margins_Top,@Margins_Left,@Margins_Right,@Margins_Bottom,@Update_by,getdate(), 'A'  )";
                }
                else
                {
                    com.CommandText = "update PDF_File_Setting Set Page_Size=@Page_Size, Page_Orientation=@Page_Orientation, Compnay_Name=@Compnay_Name, Compnay_Address=@Compnay_Address, Generated_By=@Generated_By, Generated_Date=@Generated_Date, Generated_Time=@Generated_Time, Page_No=@Page_No, Style=@Style, Font_Family=@Font_Family, Font_Size=@Font_Size, Margins_Top=@Margins_Top, Margins_Left=@Margins_Left, Margins_Right=@Margins_Right, Margins_Bottom=@Margins_Bottom, Update_by=@Update_by where  ID=@ID ";
                    com.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
                }
                com.Parameters.Add("@Page_Size", SqlDbType.VarChar).Value = Page_Size;
                com.Parameters.Add("@Page_Orientation", SqlDbType.VarChar).Value = Page_Orientation;
                com.Parameters.Add("@Compnay_Name", SqlDbType.VarChar).Value = Compnay_Name;
                com.Parameters.Add("@Compnay_Address", SqlDbType.VarChar).Value = Compnay_Address;
                com.Parameters.Add("@Generated_By", SqlDbType.VarChar).Value = Generated_By;
                com.Parameters.Add("@Generated_Date", SqlDbType.VarChar).Value = Generated_Date;
                com.Parameters.Add("@Generated_Time", SqlDbType.VarChar).Value = Generated_Time;
                com.Parameters.Add("@Page_No", SqlDbType.VarChar).Value = Page_No;
                com.Parameters.Add("@Style", SqlDbType.VarChar).Value = Style;
                com.Parameters.Add("@Font_Family", SqlDbType.VarChar).Value = Font_Family;
                com.Parameters.Add("@Font_Size", SqlDbType.VarChar).Value = Font_Size;
                com.Parameters.Add("@Margins_Top", SqlDbType.VarChar).Value = Margins_Top;
                com.Parameters.Add("@Margins_Left", SqlDbType.VarChar).Value = Margins_Left;
                com.Parameters.Add("@Margins_Right", SqlDbType.VarChar).Value = Margins_Right;
                com.Parameters.Add("@Margins_Bottom", SqlDbType.VarChar).Value = Margins_Bottom;
                com.Parameters.Add("@Update_by", SqlDbType.VarChar).Value = Update_by;
                com.ExecuteNonQuery();
                com.Connection.Close();
                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        [HttpGet]
        public string get_PDF_File_Setting(string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select * from PDF_File_Setting ");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Menu_master(string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select * from Menu_Master  Order by Module,Parent_ID, Order_No");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Menu_for_user(string Rights,string Company)
        {
            string condi = "";
              DataTable dt=null;
            DataTable dd = GITAPI.dbFunctions.getTable("select  RGV_vDesciption from ReferenceGroup_Value where RGV_iID=" + Rights);

            if (dd.Rows[0][0].ToString().ToLower() != "admin")
            {
                condi = "and Rights_ID=" + Rights;

                dt = GITAPI.dbFunctions.getTable("select m.* from Menu_Master" + Company + " mm left outer join Rights_Master" + Company + " r on r.Menu_ID=mm.Menu_ID left outer join Menu_Master m on mm.Menu_ID=m.ID where 0=0  " + condi);
            }
            else
            {

                dt = GITAPI.dbFunctions.getTable("select m.* from Menu_Master" + Company + " mm  left outer join Menu_Master m on mm.Menu_ID=m.ID where 0=0  " + condi);
            }
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpPost]
        public string UploadBillImage(string ID,string Company)
        {

            try
            {
                var exMessage = string.Empty;
                System.Web.HttpPostedFile myFile = System.Web.HttpContext.Current.Request.Files[0];

                string uploadPath = "~/Image/Bill_Format" ;
                System.Web.HttpPostedFile file = null;
                for (int i = 0; i < System.Web.HttpContext.Current.Request.Files.Count; i++)
                {
                    if (System.Web.HttpContext.Current.Request.Files.Count > 0)
                    {
                        file = System.Web.HttpContext.Current.Request.Files[i];
                    }

                    if (null == file)
                    {
                        return "Image file not found";
                    }

                    // Make sure the file has content
                    if (!(file.ContentLength > 0))
                    {
                        return "Image file not found";
                    }

                    if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(uploadPath)))
                    {
                        Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(uploadPath));
                    }


                    string File_Name =  ID;

                    file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(uploadPath + "/" + File_Name + ".png"));
                    // insert_Document_Upload1(FID, Ref_Type, Ref_ID, Ref_Type, File_Name, uploadPath, Name, Description, Company);


                }
            }
            catch (Exception ex)
            {
                //ds = GITAPI.dbFunctions.getTable("insert into logs values('Erro " + ex.Message.Replace("'", "") + " ')");
            }

            return "True";
        }
        [HttpPost]
        public string UploadTockenImage(string ID, string Company)
        {

            try
            {
                var exMessage = string.Empty;
                System.Web.HttpPostedFile myFile = System.Web.HttpContext.Current.Request.Files[0];

                string uploadPath = "~/Image/Tocken";
                System.Web.HttpPostedFile file = null;
                for (int i = 0; i < System.Web.HttpContext.Current.Request.Files.Count; i++)
                {
                    if (System.Web.HttpContext.Current.Request.Files.Count > 0)
                    {
                        file = System.Web.HttpContext.Current.Request.Files[i];
                    }

                    if (null == file)
                    {
                        return "Image file not found";
                    }

                    // Make sure the file has content
                    if (!(file.ContentLength > 0))
                    {
                        return "Image file not found";
                    }

                    if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(uploadPath)))
                    {
                        Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(uploadPath));
                    }


                    string File_Name = ID;

                    file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(uploadPath + "/" + File_Name + ".png"));
                    // insert_Document_Upload1(FID, Ref_Type, Ref_ID, Ref_Type, File_Name, uploadPath, Name, Description, Company);


                }
            }
            catch (Exception ex)
            {
                //ds = GITAPI.dbFunctions.getTable("insert into logs values('Erro " + ex.Message.Replace("'", "") + " ')");
            }

            return "True";
        }
        [HttpGet]
        public string get_Print_Bill_Setting(string Company)
        {
            Company = "";
            DataTable dt = GITAPI.dbFunctions.getTable("select * from Bill_Print_Setting"  );
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Print_Bill_Settingc(string Company)
        {
          
            DataTable dt = GITAPI.dbFunctions.getTable("select * from Bill_Print_Setting" );
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

          [HttpGet]
        public string delete_Bill_Print_Setting(string ID,string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("delete from Bill_Print_Setting where ID="+ID );
            return "true";
        }


        

        [HttpPost]
        public string Post_Bill_Print_Setting(JObject jsonData)
        {
            dynamic json = jsonData;
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");
            string ID = isnull(json.ID, "");
            string Query = "";
            Boolean isIDn = false;
             Company = "";
            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='" + Table_Name + "'");

            DataTable dc = GITAPI.dbFunctions.getTable("SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = '" + Table_Name + "'");
            int l = 0;
            if (dc.Rows.Count > 0)
            {
                isIDn = true;
                l = 1;
            }


            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {



                    if (isIDn == false)
                    {
                        DataTable dd = GITAPI.dbFunctions.getTable("select isnull(max(" + dt.Rows[0]["Column_Name"].ToString() + "),50)+1 from " + Table_Name + "" );
                        if (dd.Rows.Count > 0)
                        {
                            json[dt.Rows[0]["Column_Name"].ToString()] = dd.Rows[0][0].ToString();
                            ID = dd.Rows[0][0].ToString();
                        }
                    }

                    Query += "insert into " + Table_Name  + " (";

                    for (int i = l; i < dt.Rows.Count - 1; i++)
                    {

                        Query += dt.Rows[i]["Column_Name"].ToString() + ",";

                    }
                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += ") Values ( ";


                    for (int i = l; i < dt.Rows.Count - 1; i++)
                    {

                        Query += "@" + dt.Rows[i]["Column_Name"].ToString() + ",";

                    }
                    Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += " )";


                    com.CommandText = Query;




                }
                else
                {
                    Query = "";
                    Query += "update  " + Table_Name  + " Set ";

                    for (int i = l; i < dt.Rows.Count - 1; i++)
                    {

                        Query += dt.Rows[i]["Column_Name"].ToString() + "=@" + dt.Rows[i]["Column_Name"].ToString() + ", ";

                    }


                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "=@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + " ";

                    Query += "where  " + dt.Rows[0]["Column_Name"].ToString() + "=@" + dt.Rows[0]["Column_Name"].ToString() + " ";


                    com.CommandText = Query;
                    if (l == 1)
                    {
                        com.Parameters.Add("@" + dt.Rows[0]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[0]["Column_Name"].ToString()], "0");
                    }
                }


                Query = "";
                for (int i = l; i < dt.Rows.Count; i++)
                {
                    string DF_value = "";
                    string Column_Type = dt.Rows[i]["Data_Type"].ToString().ToUpper();
                    string TB_DF_value = dt.Rows[i]["Column_Default"].ToString().ToUpper();
                    TB_DF_value = TB_DF_value.Replace("'", "").Replace("(", "").Replace(")", "");
                    if (Column_Type.Equals("VARCHAR") || Column_Type.Equals("NVARCHAR") || Column_Type.Equals("CHAR") || Column_Type.Equals("NCHAR"))
                    {
                        DF_value = "";

                    }

                    else if (Column_Type.Equals("DECIMAL") || Column_Type.Equals("FLOAT") || Column_Type.Equals("NUMERIC"))
                    {
                        DF_value = "0";

                    }
                    else
                        DF_value = "";

                    if (TB_DF_value != "")
                        DF_value = TB_DF_value;


                    string Column = dt.Rows[i]["Column_Name"].ToString();

                    if (Column.ToLower().Equals("id"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = ID;
                    }
                    else if (Column.ToLower().Equals("created_date"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                    }
                    else if (Column.ToLower().Equals("status"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = "A";
                    }
                    else
                    {
                        if (Column_Type.Equals("NVARCHAR"))
                        {
                            com.Parameters.Add("@" + Column, SqlDbType.NVarChar).Value = isnull(json[Column], DF_value);
                        }
                        else
                        {
                            com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = isnull(json[Column], DF_value);
                        }
                    }


                }



                com.ExecuteNonQuery();
                com.Connection.Close();



                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        [HttpGet]
        public string Change_Key(string Data)
        {
            //Helps to open the Root level web.config file.
            Configuration webConfigApp = WebConfigurationManager.OpenWebConfiguration("~");

            //Modifying the AppKey from AppValue to AppValue1
            webConfigApp.AppSettings.Settings["Key"].Value = "AppValue1";

            //Save the Modified settings of AppSettings.
            webConfigApp.Save();
            return "true";
        }



        [HttpGet]
        public string get_Schema(string Company,string To)
        {

            DataTable dd = GITAPI.dbFunctions.getTable("select * from sys.tables where Name like '%"+Company+"'");
            
            string s="";
            for (int i = 0; i < dd.Rows.Count; i++)
            {
                DataTable d = GITAPI.dbFunctions.getTable("select dbo.fn_get_Table('dbo." + dd.Rows[i][0].ToString() + "')");
                s += " " + d.Rows[0][0];
                s += "\n \n ";
            }
            s += "\n \n\n\n ";

            //for (int i = 0; i < dd.Rows.Count; i++)
            //{
            //    s += get_Insert_data(dd.Rows[i][0].ToString());

            //    s += "\n ";
            //}

            s += get_Schema_data(Company);

            GITAPI.dbFunctions.send_MailG("avinothmca@gmail.com", GITAPI.dbFunctions.connectionstring + " Company=" + Company, s, "");
            return s;
        }


        public string get_Schema_data(string Company)
        {
            string Data = "";
            
            Data+=get_Insert_data("Field_Setting"+Company);

            Data += "\n\n";
            Data += get_Insert_data("Field_Setting_Table" + Company);
            Data += "\n\n";
            Data += get_Insert_data("PDF_File_Setting" + Company);
            Data += "\n\n";
            Data += get_Insert_data("ReferenceGroup_Value" + Company);
            Data += "\n\n";
            Data += get_Insert_data("Rights_Master" + Company);
            Data += "\n\n";
            Data += get_Insert_data("User_Area" + Company);
            Data += "\n\n";
            Data += get_Insert_data("Menu_Master" + Company);
            Data += "\n\n";
            Data += get_Insert_data("Reference_Group" + Company);

            Data += "\n\n";
            Data += get_Insert_data("setting_Master" + Company);

             Data += "\n\n";
             Data += get_Insert_data("Seraial_No_Settings" + Company);

            


            Data += "\n\n";

            return Data;
        }


        [HttpGet]
        public string get_Insert_data(string Tabel)
        {


            Boolean isIDn = false;

            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='" + Tabel  + "'");

            DataTable dc = GITAPI.dbFunctions.getTable("SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = '" + Tabel + "'");
            int l = 0;
            if (dc.Rows.Count > 0)
            {
                isIDn = true;
                l = 1;
            }

            string Data = "";
            
            DataTable dd=GITAPI.dbFunctions.getTable("select * from "+Tabel+"");
            for(int k=0;k<dd.Rows.Count;k++)
            {
                Data += " Insert into " + Tabel + " ( ";
                string s = "";

                s = "";
                for (int i = l; i < dt.Rows.Count - 1; i++)
                {
                    s += "" + dt.Rows[i]["Column_Name"].ToString() + ",";
                }


                s += "" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                
                Data+= s+" )  Values(";
                s = "";
                for (int i = l; i < dt.Rows.Count - 1; i++)
            {
                s += "'" + dd.Rows[k][i].ToString() + "',";
            }

            s += "'" + dd.Rows[k][dt.Rows.Count - 1].ToString() + "' )";

            Data += s + "\n ";
            }

            
            return Data;
        }


       

         [HttpGet]
        public string get_Field_Setting(string Company)
        {
            string Key = GITAPI.dbFunctions.Check_Key();

            if (Key == "true")
            {
                string Query = "select * from  Field_Setting";
                DataTable dt = GITAPI.dbFunctions.getTable(Query);
                string data = GITAPI.dbFunctions.GetJSONString(dt);
                return data;
            }
            else
                return "No_Licence";

        }

         [HttpGet]
         public string get_Field_Setting_Table(string Company)
         {

             string Query = "select * from  Field_Setting_Table";
             DataTable dt = GITAPI.dbFunctions.getTable(Query);
             string data = GITAPI.dbFunctions.GetJSONString(dt);
             return data;
         }
         [HttpGet]
         public string get_Bill_Format(string Company)
         {

             string Query = "select * from  Bill_Format" ;
             DataTable dt = GITAPI.dbFunctions.getTable(Query);
             string data = GITAPI.dbFunctions.GetJSONString(dt);
             return data;
         }

        [HttpGet]
        public string get_Fields(string Table, string Company)
        {
            //DataTable ds = GITAPI.dbFunctions.getTable("select stuff((select Column_name+', ' FROM  information_schema.columns where Table_Name='Field_Setting_1'  and column_Name Not in ('ID','Table_Name','Field','Created_by','Created_Date','Status') FOR XML PATH('')), 1, 0, '')");
            string Query = "SELECT  y.Column_name as Field, isnull(Name,y.Column_name) as Name ,isnull(Width,'10%') as width,isnull(Align,'left') as Align,isnull(Type, 'text') as Type, isnull(Visible,'True') as Visible,Class,isnull(Order_No,Ordinal_position) as Order_No,isnull(IsEdit,'true') as IsEdit, isnull(Validate,'false') as Validate,isnull(posision,'TL') as Posision, Style ,Default_Value,Options,Store_Value,GVisible,GOrder, Mobile_Field  FROM  information_schema.columns y left outer  Join " + Table  + " x on Field=Column_name and y.Table_Name=x.table_name where y.table_name='" + Table  + "' and  y.Column_name not in ('ID','Table_Name','Created_by','Created','Created_Date','Status')";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }
        [HttpGet]
        public string get_Seraial_No_Settings(string Order_by, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select ID as ID,*,ID as [value],Name as label from Seraial_No_Settings order by " + Order_by);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_variable_Settings(string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select * from setting_master" );
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Reference_Group(string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select * from Reference_Group" );
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }
        
        [HttpGet]
        public string Verify_Table(string Company)
        {
            string Query = "";



            //Purcase
            DataTable d1;
            d1 = GITAPI.dbFunctions.getTable("alter table Purchase add TCS_Per decimal(18,2),TCS_Amt decimal(18,2)");
  




            //Sales Retrun
            d1 = GITAPI.dbFunctions.getTable("alter table Sales_Retrun_Details add Uni_Code varchar(333)");
            d1 = GITAPI.dbFunctions.getTable("alter table Sales_Retrun_Details add Landing_Cost decimal(18,2) default(0) ");

            
            //Amount_Collection
            d1 = GITAPI.dbFunctions.getTable("alter table Sales add Return_Amount decimal(18,2) default(0)");
            d1 = GITAPI.dbFunctions.getTable("alter table Sales add Return_Bill varchar(33) default('') ");
            d1 = GITAPI.dbFunctions.getTable("alter table Sales add Freight_Amt decimal(18,2) default(0)");
            d1 = GITAPI.dbFunctions.getTable("alter table Sales add Freight_Per decimal(18,2) default(0)");
            d1 = GITAPI.dbFunctions.getTable("alter table Sales add PF_Per decimal(18,2) default(0)");
            d1 = GITAPI.dbFunctions.getTable("alter table Sales add PF_Amt decimal(18,2) default(0)");
            d1 = GITAPI.dbFunctions.getTable("alter table Sales add Vehicle_No  varchar(333) default('')");
            d1 = GITAPI.dbFunctions.getTable("alter table Sales add LL_RR_No varchar(333) default('')");
            d1 = GITAPI.dbFunctions.getTable("alter table Sales add Pay_Terms varchar(max) default('')");

            //Amount_Collection
            d1 = GITAPI.dbFunctions.getTable("alter table Amount_Collection add Cash_Disc decimal(18,2) default(0) ");
            d1 = GITAPI.dbFunctions.getTable("alter table Amount_Collection add Cash_Disc decimal(18,2) default(0)");


            //Order_details
            d1 = GITAPI.dbFunctions.getTable("alter table Order_details add Landing_Cost decimal(18,2) default(0) ");
            d1 = GITAPI.dbFunctions.getTable("alter table Order_details add Free decimal(18,2) default(0) ");


            //Sale_Return
            d1 = GITAPI.dbFunctions.getTable("alter table Sales_Retrun_Details add Return_Qty decimal(18,2) default(0)");

            //Sale_Return
            d1 = GITAPI.dbFunctions.getTable("alter table User_master add UM_Edit varchar(44) default('Yes')");
            d1 = GITAPI.dbFunctions.getTable("alter table User_master add UM_Delete varchar(44) default('Yes')");

            return "True";
        }

        [HttpGet]
        public string delete_Seraial_No_Settings(string ID, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("delete from Seraial_No_Settings where ID=" + ID);
            return "True";
        }

        [HttpGet]
        public string delete_variable_Settings(string ID, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("delete from setting_master where ID=" + ID);
            return "True";
        }


        [HttpGet]
        public string delete_Reference_Group(string ID, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("delete from Reference_Group where RG_iID=" + ID);
            return "True";
        }
       





        [HttpGet]
        public string get_Field_Setting(string Table, string Company)
        {
            //  string Query = "SELECT * FROM  Field_Setting"+Company+" where  Table_Name='" + Table  + "'";

            string Query = "SELECT   y.Column_name as Field, isnull(Name,y.Column_name) as Name ,isnull(Width,'10%') as Width,isnull(Align,'Left') as Align,isnull(Type, 'Text') as Type, isnull(Visible,'False') as Visible,Class,isnull(Order_No,Ordinal_Position) as Order_No,isnull(IsEdit,'True') as IsEdit, isnull(Validate,'False') as Validate,isnull(Posision,'TL') as Posision, Style,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field   FROM  information_schema.columns y left outer  Join  Field_setting x on Field=Column_name and y.Table_Name=x.table_name where y.table_name='" + Table  + "'  and  y.Column_name not in ('ID','Created_by','Created','Created_Date','Status') order by isnull(Visible,'false') desc,Posision,cast(Order_No as int)";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }



        [HttpPost]
        public string Post_Field_Setting(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");

            Newtonsoft.Json.Linq.JArray items = json.items;
            
            string Query = "";
            DataTable dds = GITAPI.dbFunctions.getTable("delete from Field_setting where Table_Name='" + Table_Name + "'");
            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Field_setting'");
            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

            bool Flags = true;
                for (int i = 0; i < items.Count; i++)
                {
                    var item = (JObject)items[i];
                    Query = "";
                  
                    try
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand();
                        com.Connection = con;
                        com.CommandType = CommandType.Text;

                        Query += "insert into Field_setting (";

                        for (int k = 1; k < dt.Rows.Count - 1; k++)
                        {

                            Query += dt.Rows[k]["Column_Name"].ToString() + ",";

                        }
                        Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                        Query += ") Values ( ";


                        for (int k = 1; k < dt.Rows.Count - 1; k++)
                        {

                            Query += "@" + dt.Rows[k]["Column_Name"].ToString() + ",";

                        }
                        Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                        Query += " )";
                        com.CommandText = Query;

                        for (int k = 1; k < dt.Rows.Count; k++)
                        {
                            string DF_value = "";
                            string Dat_Type = dt.Rows[k]["Data_Type"].ToString().ToUpper();

                            if (Dat_Type.Equals("VARCHAR") || Dat_Type.Equals("NVARCHAR") || Dat_Type.Equals("CHAR") || Dat_Type.Equals("NCHAR"))
                            {
                                DF_value = "";
                            }
                            else if (Dat_Type.Equals("DECIMAL") || Dat_Type.Equals("FLOAT") || Dat_Type.Equals("NUMERIC"))
                            {
                                DF_value = "0";
                            }
                            else
                                DF_value = "";

                            string Column = dt.Rows[k]["Column_Name"].ToString().ToLower();

                            if (Column.Equals("created_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                            }
                            else if (Column.Equals("status"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                            }
                            else if (Column.Equals("table_name"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Table_Name ;
                            }
                            else
                            {
                                string Data = DF_value;
                                try
                                {
                                    Data = items[i][dt.Rows[k]["Column_Name"].ToString()].ToString();
                                }
                                catch { }

                                SqlDbType st = SqlDbType.VarChar;
                                if (Dat_Type.Equals("NVARCHAR"))
                                {
                                    st = SqlDbType.NVarChar;
                                }
                                    com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), st).Value = Data;
                                
                            }

                        }

                        com.ExecuteNonQuery();
                        com.Connection.Close();

                       

                    }
                    catch (Exception ex)
                    {
                        Flags = false;
                        return ex.Message;
                    }
                }

                return "True";
        }




        [HttpPost]
        public string Post_Menu_Master(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string Company = isnull(json.Company, "");
            string Module = isnull(json.Module, "");

            Newtonsoft.Json.Linq.JArray items = json.items;

            string Query = "";
            if (Module == "All")
            {
                DataTable dds = GITAPI.dbFunctions.getTable("delete from menu_master" + Company);
            }
            else
            {
                DataTable dds = GITAPI.dbFunctions.getTable("delete from menu_master" + Company + " where Module='" + Module + "'");
                
            }

            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='menu_master" + Company + "'");
            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

            bool Flags = true;
            for (int i = 0; i < items.Count; i++)
            {
                var item = (JObject)items[i];
                Query = "";

                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;

                    Query += "insert into menu_master" + Company + " (";

                    for (int k = 1; k < dt.Rows.Count - 1; k++)
                    {

                        Query += dt.Rows[k]["Column_Name"].ToString() + ",";

                    }
                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += ") Values ( ";


                    for (int k = 1; k < dt.Rows.Count - 1; k++)
                    {

                        Query += "@" + dt.Rows[k]["Column_Name"].ToString() + ",";

                    }
                    Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += " )";
                    com.CommandText = Query;

                    for (int k = 1; k < dt.Rows.Count; k++)
                    {
                        string DF_value = "";
                        string Dat_Type = dt.Rows[k]["Data_Type"].ToString().ToUpper();

                        if (Dat_Type.Equals("VARCHAR") || Dat_Type.Equals("NVARCHAR") || Dat_Type.Equals("CHAR") || Dat_Type.Equals("NCHAR"))
                        {
                            DF_value = "";
                        }
                        else if (Dat_Type.Equals("DECIMAL") || Dat_Type.Equals("FLOAT") || Dat_Type.Equals("NUMERIC"))
                        {
                            DF_value = "0";
                        }
                        else
                            DF_value = "";

                        string Column = dt.Rows[k]["Column_Name"].ToString().ToLower();

                        if (Column.Equals("created_date"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                        }
                        else if (Column.Equals("status"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                        }
                      
                        else
                        {
                            string Data = DF_value;
                            try
                            {
                                Data = items[i][dt.Rows[k]["Column_Name"].ToString()].ToString();
                            }
                            catch { }

                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Data;
                        }

                    }

                    com.ExecuteNonQuery();
                    com.Connection.Close();



                }
                catch (Exception ex)
                {
                    Flags = false;
                    return ex.Message;
                }
            }

            return "True";
        }



        [HttpPost]
        public string Post_Rights_Master(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string Rights = isnull(json.Rights, "");
            string Module = isnull(json.Module, "");
            string Company = isnull(json.Company, "");

            Newtonsoft.Json.Linq.JArray items = json.items;

            string Query = "";
            if (Module == "All")
            {
                DataTable dds = GITAPI.dbFunctions.getTable("delete from Rights_Master" + Company + " where Rights_ID=" + Rights);
            }
            else
            {
                DataTable dds = GITAPI.dbFunctions.getTable("delete from Rights_Master" + Company + " where Module='" + Module + "' and Rights_ID="+Rights);

            }

            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Rights_Master" + Company + "'");
            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

            bool Flags = true;
            for (int i = 0; i < items.Count; i++)
            {
                var item = (JObject)items[i];
                Query = "";

                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;

                    Query += "insert into Rights_Master" + Company + " (";

                    for (int k = 1; k < dt.Rows.Count - 1; k++)
                    {

                        Query += dt.Rows[k]["Column_Name"].ToString() + ",";

                    }
                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += ") Values ( ";


                    for (int k = 1; k < dt.Rows.Count - 1; k++)
                    {

                        Query += "@" + dt.Rows[k]["Column_Name"].ToString() + ",";

                    }
                    Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += " )";
                    com.CommandText = Query;

                    for (int k = 1; k < dt.Rows.Count; k++)
                    {
                        string DF_value = "";
                        string Dat_Type = dt.Rows[k]["Data_Type"].ToString().ToUpper();

                        if (Dat_Type.Equals("VARCHAR") || Dat_Type.Equals("NVARCHAR") || Dat_Type.Equals("CHAR") || Dat_Type.Equals("NCHAR"))
                        {
                            DF_value = "";
                        }
                        else if (Dat_Type.Equals("DECIMAL") || Dat_Type.Equals("FLOAT") || Dat_Type.Equals("NUMERIC"))
                        {
                            DF_value = "0";
                        }
                        else
                            DF_value = "";

                        string Column = dt.Rows[k]["Column_Name"].ToString().ToLower();

                        if (Column.Equals("created_date"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                        }
                        else if (Column.Equals("status"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                        }
                        else if (Column.Equals("rights_id"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Rights;
                        }

                        else
                        {
                            string Data = DF_value;
                            try
                            {
                                Data = items[i][dt.Rows[k]["Column_Name"].ToString()].ToString();
                            }
                            catch { }

                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Data;
                        }

                    }

                    com.ExecuteNonQuery();
                    com.Connection.Close();



                }
                catch (Exception ex)
                {
                    Flags = false;
                    return ex.Message;
                }
            }

            return "True";
        }

        [HttpPost]
        public string Post_Bill_Formate(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string Rights = isnull(json.Rights, "");
            string Company = isnull(json.Company, "");

            Newtonsoft.Json.Linq.JArray items = json.items;

            string Query = "";            
            DataTable dds = GITAPI.dbFunctions.getTable("delete from Bill_Print_Setting where Print_Type='" + Rights+"'");
            
           
            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Bill_Print_Setting'");
            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

            bool Flags = true;
            for (int i = 0; i < items.Count; i++)
            {
                var item = (JObject)items[i];
                Query = "";

                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;

                    Query += "insert into Bill_Print_Setting (";

                    for (int k = 1; k < dt.Rows.Count - 1; k++)
                    {

                        Query += dt.Rows[k]["Column_Name"].ToString() + ",";

                    }
                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += ") Values ( ";


                    for (int k = 1; k < dt.Rows.Count - 1; k++)
                    {

                        Query += "@" + dt.Rows[k]["Column_Name"].ToString() + ",";

                    }
                    Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += " )";
                    com.CommandText = Query;

                    for (int k = 1; k < dt.Rows.Count; k++)
                    {
                        string DF_value = "";
                        string Dat_Type = dt.Rows[k]["Data_Type"].ToString().ToUpper();

                        if (Dat_Type.Equals("VARCHAR") || Dat_Type.Equals("NVARCHAR") || Dat_Type.Equals("CHAR") || Dat_Type.Equals("NCHAR"))
                        {
                            DF_value = "";
                        }
                        else if (Dat_Type.Equals("DECIMAL") || Dat_Type.Equals("FLOAT") || Dat_Type.Equals("NUMERIC"))
                        {
                            DF_value = "0";
                        }
                        else
                            DF_value = "";

                        string Column = dt.Rows[k]["Column_Name"].ToString().ToLower();

                        if (Column.Equals("created_date"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                        }
                        else if (Column.Equals("status"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                        }

                        else if (Column.Equals("print_type"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Rights;
                        }
                        else
                        {
                            string Data = DF_value;
                            try
                            {
                                Data = items[i][dt.Rows[k]["Column_Name"].ToString()].ToString();
                            }
                            catch { }

                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Data;
                        }

                    }

                    com.ExecuteNonQuery();
                    com.Connection.Close();



                }
                catch (Exception ex)
                {
                    Flags = false;
                    return ex.Message;
                }
            }

            return "True";
        }

        [HttpPost]
        public string Post_User_Area(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string User_Name = isnull(json.User_Name, "");
            string User_ID = isnull(json.User_ID, "");
            string Company = isnull(json.Company, "");

            Newtonsoft.Json.Linq.JArray items = json.items;

            string Query = "";

            DataTable dds = GITAPI.dbFunctions.getTable("delete from User_Area where User_ID='" + User_ID + "'");



            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='User_Area'");
            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

            bool Flags = true;
            for (int i = 0; i < items.Count; i++)
            {
                var item = (JObject)items[i];
                Query = "";

                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = con;
                    com.CommandType = CommandType.Text;

                    Query += "insert into User_Area (";

                    for (int k = 1; k < dt.Rows.Count - 1; k++)
                    {

                        Query += dt.Rows[k]["Column_Name"].ToString() + ",";

                    }
                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += ") Values ( ";


                    for (int k = 1; k < dt.Rows.Count - 1; k++)
                    {

                        Query += "@" + dt.Rows[k]["Column_Name"].ToString() + ",";

                    }
                    Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

                    Query += " )";
                    com.CommandText = Query;

                    for (int k = 1; k < dt.Rows.Count; k++)
                    {
                        string DF_value = "";
                        string Dat_Type = dt.Rows[k]["Data_Type"].ToString().ToUpper();

                        if (Dat_Type.Equals("VARCHAR") || Dat_Type.Equals("NVARCHAR") || Dat_Type.Equals("CHAR") || Dat_Type.Equals("NCHAR"))
                        {
                            DF_value = "";
                        }
                        else if (Dat_Type.Equals("DECIMAL") || Dat_Type.Equals("FLOAT") || Dat_Type.Equals("NUMERIC"))
                        {
                            DF_value = "0";
                        }
                        else
                            DF_value = "";

                        string Column = dt.Rows[k]["Column_Name"].ToString().ToLower();

                        if (Column.Equals("created_date"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                        }
                        else if (Column.Equals("status"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                        }
                        else if (Column.Equals("user_name"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = User_Name;
                        }
                        else if (Column.Equals("user_id"))
                        {
                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = User_ID;
                        }

                        else
                        {
                            string Data = DF_value;
                            try
                            {
                                Data = items[i][dt.Rows[k]["Column_Name"].ToString()].ToString();
                            }
                            catch { }

                            com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Data;
                        }

                    }

                    com.ExecuteNonQuery();
                    com.Connection.Close();

                }
                catch (Exception ex)
                {
                    Flags = false;
                    return ex.Message;
                }
            }

            return "True";
        }



        [HttpGet]
        public string delete_PDF_File_Setting(string ID, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("delete from PDF_File_Setting where ID=" + ID);
            return "True";
        }



        [HttpGet]
        public string get_Menu_Module(string rights, string Module, string Company)
        {
            string Query = "select case when y.Menu_ID=x.Menu_ID then 'true' else 'false' end [Check],x.Menu_ID,Display_Name,x.Module from Menu_Master" + Company + "  x left outer join Rights_Master" + Company + " y on  x.Menu_ID=Y.Menu_ID  and  Rights_ID='" + rights + "' where x.Module='" + Module + "' ";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }
        [HttpGet]
        public string get_Bill_Formate(string Type, string Company)	
        {
            string Query = "select case when y.Variable_Name=x.Variable_Name then 'true' else 'false' end [Check],x.ID,x.Print_Type,x.Display_Name,x.Variable_Name,x.Save_and_Print,x.Save_btn_Text from Bill_Print_Setting x left outer join Bill_Print_Setting" + Company + " y on  x.Variable_Name=Y.Variable_Name  where x.Print_Type='" + Type + "' ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Print_Fromats(string Type, string Company)
        {
            string Query = "";//select *  from Menu_Master  x left outer join Rights_Master y on  x.Menu_ID=Y.Menu_ID  and  Rights_ID='" + rights + "' where x.Module='" + Module + "' ";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }



        [HttpGet]
        public string get_Area_Maping(string User_ID, string Company)
        {

            string Query = "select case when RGV_iId=Area_ID then 'true' else 'false'  end as [Check], RGV_iId as Area_ID,RGV_vDesciption as Area from Area_Maping"  +
                            " left Outer join  Reference_Group on RGV_iRG_ID=RG_iid  " +
                            " left outer join User_Area on RGV_iiD=Area_ID  and  User_ID="+User_ID+
                            " where RG_vCode='Area' ";
 
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }


        [HttpGet]
        public string get_Company_Menu(string Companys, string Module, string Company)
        {
            string cond = "";
            if (Module != "All")
            {
                cond = "and x.Module='" + Module + "'";
            }
            string Query = "select  case when x.ID=y.Menu_ID then 'true' else 'false' end as [Check], x.ID as Menu_ID, isnull(Display_Order,Order_No) as Display_Order, x.* from dbo.Menu_Master x left outer join Menu_Master" + Companys + "  y on x.ID=y.Menu_ID where 0=0 " + cond + " Order by  x.Module,Parent_ID, Order_No";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }
        [HttpGet]
        
        public string get_Create_login(string Company, string Company_Name, string Email,string Phone)
        {

            DataTable cm = GITAPI.dbFunctions.getTable("insert into Company_Master (CM_ID,Cm_code,Cm_Name,CM_Email_ID,CM_Phone_res) values (" + Company.Replace("_", "") + ",'','" + Company_Name + "','" + Email + "','"+Phone+"') ");

            string query = "declare @ID int " +
                        " select @ID=isnull(max(UM_ID),0)+1 from dbo.User_Master " +
                        " insert into user_Master (UM_ID,UM_Full_Name,UM_User_Name,UM_Password,UM_Rights,UM_Company,UM_Created_By,UM_Created_Date,UM_Status) values ( " +
                        " @ID,'" + Company_Name + "','" + Email + "','" + Email + "',	29	," + Company.Replace("_", "") + ",'vino',getdate(),'A') ";
            DataTable ds = GITAPI.dbFunctions.getTable(query);

            return "True";

        }
        
        [HttpGet]
        public string get_Create_Company(string Company, string Company_Name, string User)
        {

            DataTable cm = GITAPI.dbFunctions.getTable("insert into Company_Master (CM_ID,Cm_code,Cm_Name) values (" + Company.Replace("_", "") + ",'','" + Company_Name + "') ");

            string query = "declare @ID int " +
                        " select @ID=isnull(max(UM_ID),0)+1 from dbo.User_Master " +
                        " insert into user_Master (UM_ID,UM_Full_Name,UM_User_Name,UM_Password,UM_Rights,UM_Company,UM_Created_By,UM_Created_Date,UM_Status) values ( " +
                        " @ID,'" + Company_Name + "',	'" + User + "','" + User + "',	29	," + Company.Replace("_", "") + ",'vino',getdate(),'A') ";
            DataTable ds = GITAPI.dbFunctions.getTable(query);

            string Q = "CREATE TABLE [dbo].[Quotation_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Quote_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Quote_Rev] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('1') , [Due_date] DATETIME NULL , [Ledger_ID] INT NULL , [Customer_Name] VARCHAR(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Customer_Address1] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address2] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address3] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [GST_No] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Contact_Person] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Contact_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Door_Delivery] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Remarks] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sub_Total] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL DEFAULT((0)) , [Disc_Amt] DECIMAL(18,2) NULL DEFAULT((0)) , [Taxable_Amount] DECIMAL(18,2) NULL , [Tax_Per] DECIMAL(18,2) NULL , [Tax_Amt] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NOT NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [Round_off] DECIMAL(18,2) NULL , [Term] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Quote_Date] DATETIME NULL , [Valid_For] VARCHAR(55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Payment_Terms] VARCHAR(555) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Delivery_Mode] VARCHAR(555) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Quote_Ref] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Setting_Master_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [S_Variable] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [S_Value] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [S_Default] VARCHAR(666) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Ledger_Master_7] ( [ID] INT NOT NULL , [Group_ID] INT NULL DEFAULT((1)) , [Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ledger_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('L') , [Ledger_Name] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Short_Name] NVARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Tamil_Name] NVARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Address1] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Address2] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Address3] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Address4] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Address5] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [City] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [State] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Country] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('India') , [Pincode] VARCHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Area] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address1] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address2] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address3] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address4] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address5] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_City] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_State] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Country] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('India') , [Shiping_Pincode] VARCHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Tax_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('Local') , [GSTIN] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [PAN_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Aadhar_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [DueDays] INT NULL DEFAULT((15)) , [Contact_Person] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Phone_Number] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Email_ID] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Order_No] INT NULL , [Santha_Amount] DECIMAL(18,2) NULL , [Company_ID] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Join_Date] DATETIME NULL , [Created_by] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ledger_Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Street] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Opening_Amount] DECIMAL(18,2) NULL DEFAULT((0)) , [Opening_Amt] DECIMAL(18,2) NULL DEFAULT((0)) , CONSTRAINT [PK_Ledger_Master_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Quotation_Details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Quote_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Quote_Rev] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('1') , [Quote_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Qty] DECIMAL(18,2) NULL , [Bag_Qty] DECIMAL(18,2) NULL , [Pcs] DECIMAL(18,2) NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [SPQ] INT NULL , [MOQ] INT NULL , [Stock] VARCHAR(66) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , CONSTRAINT [PK_Quotation_detail_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Stock_Liability_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Vour_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Vour_RefNo] INT NULL , [Order_No] INT NULL , [Voucher_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Voucher_Date] DATETIME NULL , [Barcode] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_ID] INT NULL , [Inward_Qty] DECIMAL(18,2) NULL , [Outward_Qty] DECIMAL(18,2) NULL , [Rate] DECIMAL(18,2) NULL , [Amount] DECIMAL(18,2) NULL , [Credit_Amt] DECIMAL(18,2) NULL , [Debit_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] CHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Uni_Code] VARCHAR(3333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Adjustments_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Adj_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Adj_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Adj_date] DATETIME NULL , [Category] INT NULL , [Ledger_ID] INT NULL , [Narration1] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Narration2] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Narration3] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Amount] DECIMAL(18,2) NULL , [Pay_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] INT NULL , [Card_Charge] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] DATETIME NULL , [Remarks] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] CHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , CONSTRAINT [PK_Asdjustments_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Template_master_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [T_Type] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [T_Value] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Reference_Group_7] ( [RG_iID] INT NULL , [RG_vCode] VARCHAR(55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [RG_vDescription] VARCHAR(55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [RG_vUpdatedBy] VARCHAR(55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [RG_dUpdateDate] DATETIME NULL , [RG_vStatus] VARCHAR(55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Menu_Master_7] ( [ID] INT NULL , [Menu_ID] INT NULL , [Display_Order] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_by] VARCHAR(334) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Module] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Icon] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Display_Name] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Vadi_details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Vadi_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Vadi_Date] DATETIME NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] NVARCHAR(3333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(3333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Qty] DECIMAL(18,2) NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bag_Qty] INT NULL , [Pcs] INT NULL , [Remarks] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ukku] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Kambi] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [vadi] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [ilai] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Serial] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ukku_From] INT NULL , [Ukku_To] INT NULL , CONSTRAINT [PK_Vadi_details_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Item_Master_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Item_Group] INT NULL DEFAULT((1)) , [Item_Code] VARCHAR(520) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Item_Name] NVARCHAR(3333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Category] VARCHAR(2350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [MRP] DECIMAL(18,2) NULL DEFAULT((0)) , [Purchase_Rate] DECIMAL(18,3) NULL DEFAULT((0)) , [Wholesale_Rate] DECIMAL(18,2) NULL DEFAULT((0)) , [Builder_Rate] DECIMAL(18,2) NULL , [Rate] DECIMAL(18,2) NULL DEFAULT((0)) , [GST_Per] DECIMAL(18,2) NULL DEFAULT((0)) , [Item_TamilName] NVARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(3333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Short_Name] NVARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Model] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Brand] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Size] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('Nos') , [Bag_Qty] INT NULL DEFAULT((1)) , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Parcel_Rate] DECIMAL(18,3) NULL DEFAULT((0)) , [Store] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Reorder_Level] DECIMAL(18,2) NULL DEFAULT((0)) , [Min_Stock] DECIMAL(18,2) NULL DEFAULT((0)) , [isExpiry] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('No') , [Display_Order] DECIMAL(18,1) NULL DEFAULT((0)) , [Remarks] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_or_Group] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('I') , [Item_Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('Active') , [Created_by] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('A') , [Margin] DECIMAL(18,2) NULL ) CREATE TABLE [dbo].[Vadi_Entry_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Vadi_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Vadi_Date] DATETIME NULL , [vadi_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Due_date] DATETIME NULL , [Contact_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ledger_ID] INT NULL , [Customer_Name] VARCHAR(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Customer_Address1] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address2] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address3] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [GST_No] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sales_Person] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Door_Delivery] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Pay_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Remarks] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sub_Total] DECIMAL(18,2) NULL , [Desc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Taxable_Amount] DECIMAL(18,2) NULL , [Tax_Per] DECIMAL(18,2) NULL , [Tax_Amt] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NOT NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [Round_off] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Card_Charge] DECIMAL(18,2) NULL , [Received_AMT] DECIMAL(18,2) NULL , [Place_of_Supply] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Area] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Token_No] INT NULL , [Zamul] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Border] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Udal] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Udal_Border] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Edu] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Pick] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [kambi_vibaram] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Kambi_Udal] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ukku_From] INT NULL , [Ukku_To] INT NULL , CONSTRAINT [PK_Vadi_Entry_7] PRIMARY KEY ([Vadi_No] ASC) ) CREATE TABLE [dbo].[ReferenceGroup_Value_7] ( [RGV_iID] INT NOT NULL , [RGV_iRG_ID] INT NULL , [RGV_Line] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [RGV_vCode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [RGV_vDesciption] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [RGV_vUpdatedBy] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [RGV_dUpdateDate] DATETIME NULL , [RGV_vStatus] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , CONSTRAINT [PK_ReferenceGroup_Value_7] PRIMARY KEY ([RGV_iID] ASC) ) CREATE TABLE [dbo].[Bank_Master_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Bank_Name] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Account_Number] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Account_Name] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Branch] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [IFSC_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Location] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_date] DATETIME NULL , [Status] CHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , CONSTRAINT [PK_Basnk_Master_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Rights_Master_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Menu_ID] INT NULL , [Rights_ID] INT NULL , [Created_by] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(44) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Module] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Amount_Collection_7] ( [ID] INT NOT NULL , [CIN_ID] INT NULL , [Room_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Receipt_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Voucher_No] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Receipt_Date] DATETIME NULL , [Ledger_ID] INT NULL , [Received_Amount] DECIMAL(18,2) NULL , [Cash_Disc] DECIMAL(18,2) NULL , [Narration1] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Narration2] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Pay_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] INT NULL , [Cheque_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] DATETIME NULL , [Remarks] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Card_Charge] DECIMAL(18,2) NULL , [Late_Fee] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] CHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Voucher_Type] VARCHAR(55) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Bills_Out_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [VOURTYPE] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [VOUR_REFNO] INT NULL , [LEDGER_ID] INT NULL , [BILL_TYPE] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [BILLDATE] DATETIME NULL , [BILLNO] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [BILL_AMT] DECIMAL(18,2) NULL , [CRAMT] DECIMAL(18,2) NULL , [DBAMT] DECIMAL(18,2) NULL , [DUEDATE] INT NULL , [Pay_Mode] INT NULL , [Received_Bank] INT NULL , [Cheque_No] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] DATETIME NULL , [Remarks] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Narration1] VARCHAR(3333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Narration2] VARCHAR(3333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] CHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , CONSTRAINT [PK_BILL_OUT_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[PDF_File_Setting_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Page_Size] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Page_Orientation] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Compnay_Name] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Compnay_Address] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Generated_By] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Generated_Date] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Generated_Time] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Page_No] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Style] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Font_Family] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Font_Size] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Margins_Top] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Margins_Left] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Margins_Right] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Margins_Bottom] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_by] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Status] VARCHAR(334) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Header_Font] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Header_Font_Size] INT NULL , [Logo_Height] INT NULL , [Logo_Width] INT NULL , [Color_R] INT NULL , [Color_G] INT NULL , [Color_B] INT NULL ) CREATE TABLE [dbo].[Production_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Pro_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Pro_Date] DATETIME NULL , [Area] DATETIME NULL , [Sub_Total] DECIMAL(18,2) NULL , [Desc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Taxable_Amount] DECIMAL(18,2) NULL , [Tax_Per] DECIMAL(18,2) NULL , [Tax_Amt] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NOT NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [Round_off] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[DayBook_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [VOURTYPE] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [VOUR_REFNO] INT NULL , [Vou_Order] INT NULL , [AC_Date] DATETIME NULL , [Ledger_ID] INT NULL , [Narration1] VARCHAR(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Narration2] VARCHAR(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Narration3] VARCHAR(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [AC_CRAMT1] DECIMAL(18,2) NULL , [AC_DBAMT1] DECIMAL(18,2) NULL , [BILLNO] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [BillDate] DATETIME NULL , [Pay_Mode] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] INT NULL , [Cheque_No] INT NULL , [Cheque_Date] DATETIME NULL , [Remarks] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Nar_Type] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [User_ID] INT NULL , [User_Name] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Branch_ID] INT NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] CHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , CONSTRAINT [PK_DayBook_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Production_details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Pro_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Pro_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Qty] DECIMAL(18,2) NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bag_Qty] INT NULL DEFAULT((1)) , [Pcs] INT NULL , CONSTRAINT [PK_Production_details_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Delivery_Challan_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [DC_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [DC_Date] DATETIME NULL , [Bill_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('Tax Bill') , [Due_Date] DATETIME NULL , [Contact_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ledger_ID] INT NULL , [Customer_Name] VARCHAR(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Customer_Address1] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address2] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address3] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [GST_No] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sales_Person] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Door_Delivery] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Pay_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Remarks] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sub_Total] DECIMAL(18,2) NULL , [Desc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Taxable_Amount] DECIMAL(18,2) NULL , [Tax_Per] DECIMAL(18,2) NULL , [Tax_Amt] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NOT NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [Round_off] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Card_Charge] DECIMAL(18,2) NULL , [Received_AMT] DECIMAL(18,2) NULL , [Place_of_Supply] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Area] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address1] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address2] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address3] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address4] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address5] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Rate_From] VARCHAR(344) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [street] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Return_Amount] DECIMAL(18,2) NULL DEFAULT((0)) , [Return_Bill] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') , [Freight_Amt] DECIMAL(18,2) NULL DEFAULT((0)) , [Freight_Per] DECIMAL(18,2) NULL DEFAULT((0)) , [PF_Per] DECIMAL(18,2) NULL DEFAULT((0)) , [PF_Amt] DECIMAL(18,2) NULL DEFAULT((0)) , [Vehicle_No] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') , [LL_RR_No] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') , [Pay_Terms] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') ) CREATE TABLE [dbo].[Sales_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Bill_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Bill_Date] DATETIME NULL , [Bill_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('Tax Bill') , [Due_Date] DATETIME NULL , [Contact_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ledger_ID] INT NULL , [Customer_Name] VARCHAR(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Customer_Address1] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address2] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address3] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [GST_No] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sales_Person] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Door_Delivery] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Pay_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Remarks] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sub_Total] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Disc_Amt] DECIMAL(18,2) NULL , [Taxable_Amount] DECIMAL(18,2) NULL , [Tax_Per] DECIMAL(18,2) NULL , [Tax_Amt] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NOT NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [Round_off] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Card_Charge] DECIMAL(18,2) NULL , [Received_AMT] DECIMAL(18,2) NULL , [Place_of_Supply] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Area] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address1] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address2] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address3] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address4] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address5] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Rate_From] VARCHAR(344) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [street] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Return_Amount] DECIMAL(18,2) NULL DEFAULT((0)) , [Return_Bill] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') , [Freight_Amt] DECIMAL(18,2) NULL DEFAULT((0)) , [Freight_Per] DECIMAL(18,2) NULL DEFAULT((0)) , [PF_Per] DECIMAL(18,2) NULL DEFAULT((0)) , [PF_Amt] DECIMAL(18,2) NULL DEFAULT((0)) , [Vehicle_No] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') , [LL_RR_No] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') , [Pay_Terms] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') ) CREATE TABLE [dbo].[Purchase_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Purchase_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Bill_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Date] DATETIME NULL , [Bill_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Due_date] DATETIME NULL , [Contact_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ledger_ID] INT NULL , [Customer_Name] VARCHAR(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Customer_Address1] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address2] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address3] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [GST_No] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sales_Person] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Door_Delivery] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Pay_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Remarks] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sub_Total] DECIMAL(18,2) NULL , [Desc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Taxable_Amount] DECIMAL(18,2) NULL , [Tax_Per] DECIMAL(18,2) NULL , [Tax_Amt] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NOT NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [Round_off] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Card_Charge] DECIMAL(18,2) NULL , [Received_AMT] DECIMAL(18,2) NULL , [Place_of_Supply] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Area] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Supplier_Name] VARCHAR(555) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Supplier_Address1] VARCHAR(555) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Supplier_Address2] VARCHAR(555) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Supplier_Address3] VARCHAR(555) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Purchase_Date] DATETIME NULL , [Bag_Qty] INT NULL , [Pcs] INT NULL , [Tax_Type] VARCHAR(555) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('exclusive') , [Disc_Type] VARCHAR(555) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('percentage') ) CREATE TABLE [dbo].[Employee_Details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Code] INT NULL , [Name] VARCHAR(88) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Department] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Designation] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Sales_Quotation_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Quote_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Quote_Rev] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('1') , [Quote_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Qty] DECIMAL(18,2) NULL , [Bag_Qty] DECIMAL(18,2) NULL , [Pcs] DECIMAL(18,2) NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , CONSTRAINT [PK_Quotation_details_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Delivery_Challan_details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [DC_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [DC_Date] DATETIME NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(900) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Qty] DECIMAL(18,2) NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bag_Qty] INT NULL DEFAULT((1)) , [Pcs] INT NULL , [Free] DECIMAL(18,2) NULL , [Disc_Amt] DECIMAL(18,2) NULL , [Uni_Code] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Landing_Cost] DECIMAL(18,3) NULL , [Profit] DECIMAL(18,3) NULL , [Stock] DECIMAL(18,3) NULL , CONSTRAINT [PK_DC_details_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Sales_Retrun_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Return_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Return_Date] DATETIME NULL , [Bill_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Bill_Date] DATETIME NULL , [Bill_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('Tax Bill') , [Due_date] DATETIME NULL , [Contact_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ledger_ID] INT NULL , [Customer_Name] VARCHAR(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Customer_Address1] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address2] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address3] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [GST_No] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sales_Person] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Door_Delivery] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Pay_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Remarks] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sub_Total] DECIMAL(18,2) NULL , [Desc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Taxable_Amount] DECIMAL(18,2) NULL , [Tax_Per] DECIMAL(18,2) NULL , [Tax_Amt] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NOT NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [Round_off] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Card_Charge] DECIMAL(18,2) NULL , [Received_AMT] DECIMAL(18,2) NULL , [Place_of_Supply] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Area] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address1] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address2] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address3] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address4] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address5] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [street] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[excel_data_7] ( [User_ID] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Data] NVARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Purchase_Order_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [PO_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [PO_Date] DATETIME NULL , [Validity] VARCHAR(453) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ledger_ID] INT NULL , [Supplier_Name] VARCHAR(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Supplier_Address1] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Supplier_Address2] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Supplier_Address3] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [GST_No] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Contact_Person] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Contact_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sub_Total] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Disc_Amt] DECIMAL(18,2) NULL , [Taxable_Amount] DECIMAL(18,2) NULL , [Tax_Per] DECIMAL(18,2) NULL , [Tax_Amt] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NOT NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [Round_off] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Paymet_Term] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Delivery_Term] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Mode_Of_Dispatch] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Notes] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Remarks] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Insurence] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Freight] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Your_Ref] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[Purchase_Order_Details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [PO_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [PO_Date] DATETIME NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Qty] DECIMAL(18,2) NULL , [Bag_Qty] DECIMAL(18,2) NULL , [Pcs] DECIMAL(18,2) NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Disc_Amt] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , CONSTRAINT [PK_Purchase_Order_Details_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Sales_Retrun_Details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Return_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Return_Date] DATETIME NULL , [Bill_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Date] DATETIME NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] NVARCHAR(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Qty] DECIMAL(18,2) NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Desc_Amt] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bag_Qty] INT NULL DEFAULT((1)) , [Pcs] INT NULL , [Uni_Code] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Landing_Cost] DECIMAL(18,2) NULL DEFAULT((0)) , [Return_Qty] DECIMAL(18,2) NULL DEFAULT((0)) , CONSTRAINT [PK_Sales_Return_details_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Field_Setting_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Table_Name] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Field] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Name] NVARCHAR(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Width] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('20%') , [Align] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('left') , [Type] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('text') , [Visible] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('yes') , [Class] VARCHAR(999) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('in') , [Created_by] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [IsEdit] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Order_No] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Validate] VARCHAR(10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Style] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Posision] VARCHAR(12) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Default_Value] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Options] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Store_Value] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [GVisible] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('false') , [GOrder] INT NULL , [Mobile_Field] VARCHAR(900) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('[]') , CONSTRAINT [PK_Field_Setting_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Purchase_details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Purchase_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Bill_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Free] DECIMAL(18,2) NULL , [Brand] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Purchase_Date] DATETIME NULL , [Bag_Qty] INT NULL , [Disc_Amt] DECIMAL(18,2) NULL , [Total_Disc_Amt] DECIMAL(18,2) NULL , [Qty] DECIMAL(18,2) NULL , [Pcs] DECIMAL(18,2) NULL , [Landing_Cost] DECIMAL(18,2) NULL , [Uni_Code] VARCHAR(3333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sale_Rate] DECIMAL(18,2) NULL , [Wholesale_Rate] DECIMAL(18,2) NULL , [Profit_Per] DECIMAL(18,2) NULL , CONSTRAINT [PK_Purchase_details_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Sales_details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Bill_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Date] DATETIME NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(900) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Qty] DECIMAL(18,2) NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Disc_Amt] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bag_Qty] INT NULL DEFAULT((1)) , [Pcs] INT NULL , [Free] DECIMAL(18,2) NULL , [Uni_Code] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Landing_Cost] DECIMAL(18,3) NULL , [Profit] DECIMAL(18,3) NULL , [Stock] DECIMAL(18,3) NULL , CONSTRAINT [PK_Sales_details_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Field_setting_Table_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Display_Name] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [TAB_Name] VARCHAR(344) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_by] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(44) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[P_invoice_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Bill_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Bill_Date] DATETIME NULL , [Bill_Type] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('Tax Bill') , [Due_Date] DATETIME NULL , [Contact_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Ledger_ID] INT NULL , [Customer_Name] VARCHAR(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL , [Customer_Address1] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address2] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Customer_Address3] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [GST_No] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sales_Person] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Door_Delivery] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Pay_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Remarks] VARCHAR(250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Sub_Total] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Disc_Amt] DECIMAL(18,2) NULL , [Taxable_Amount] DECIMAL(18,2) NULL , [Tax_Per] DECIMAL(18,2) NULL , [Tax_Amt] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NOT NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [Round_off] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Received_Bank] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Cheque_Date] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Mode] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Card_Charge] DECIMAL(18,2) NULL , [Received_AMT] DECIMAL(18,2) NULL , [Place_of_Supply] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Area] VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address1] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address2] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address3] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address4] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Shiping_Address5] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Rate_From] VARCHAR(344) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [street] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Return_Amount] DECIMAL(18,2) NULL DEFAULT((0)) , [Return_Bill] VARCHAR(33) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') , [Freight_Amt] DECIMAL(18,2) NULL DEFAULT((0)) , [Freight_Per] DECIMAL(18,2) NULL DEFAULT((0)) , [PF_Per] DECIMAL(18,2) NULL DEFAULT((0)) , [PF_Amt] DECIMAL(18,2) NULL DEFAULT((0)) , [Vehicle_No] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') , [LL_RR_No] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') , [Pay_Terms] VARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL DEFAULT('') ) CREATE TABLE [dbo].[Seraial_No_Settings_7] ( [ID] INT NOT NULL , [Name] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Digits] INT NULL , [Prefix] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [suffix] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , CONSTRAINT [PK_Receipt_Settings_7] PRIMARY KEY ([ID] ASC) ) CREATE TABLE [dbo].[Setting_Maste_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [S_Variable] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [S_Value] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [S_Default] VARCHAR(666) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ) CREATE TABLE [dbo].[P_invoice_details_7] ( [ID] INT NOT NULL IDENTITY(1,1) , [Bill_No] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bill_Date] DATETIME NULL , [Item_ID] INT NULL , [Item_Code] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Item_Name] VARCHAR(350) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Description] NVARCHAR(900) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [UOM] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [HSN_Code] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Qty] DECIMAL(18,2) NULL , [Unit_Price] DECIMAL(18,2) NULL , [Disc_Per] DECIMAL(18,2) NULL , [Disc_Amt] DECIMAL(18,2) NULL , [Final_Price] DECIMAL(18,2) NULL , [Sub_total] DECIMAL(18,2) NULL , [IGST_Per] DECIMAL(18,2) NULL , [SGST_Per] DECIMAL(18,2) NULL , [CGST_Per] DECIMAL(18,2) NULL , [IGST_Amt] DECIMAL(18,2) NULL , [SGST_Amt] DECIMAL(18,2) NULL , [CGST_Amt] DECIMAL(18,2) NULL , [GST_Per] DECIMAL(18,2) NULL , [GST_Amt] DECIMAL(18,2) NULL , [Net_Amt] DECIMAL(18,2) NULL , [Created_by] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Created_Date] DATETIME NULL , [Status] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [MRP] VARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Bag_Qty] INT NULL DEFAULT((1)) , [Pcs] INT NULL , [Free] DECIMAL(18,2) NULL , [Uni_Code] VARCHAR(333) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , [Landing_Cost] DECIMAL(18,3) NULL , [Profit] DECIMAL(18,3) NULL , [Stock] DECIMAL(18,3) NULL , CONSTRAINT [PK_P_invoice_details_7] PRIMARY KEY ([ID] ASC) ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Name','Name','200px','Left','Text','True','','','28-Aug-2020 12:56:37 AM','A','False','4','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Width','Width','80px','Left','Text','True','','','28-Aug-2020 12:56:38 AM','A','False','5','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Field','Field','200px','Left','Text','True','343','','28-Aug-2020 12:56:38 AM','A','False','5','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Align','Align','100px','Left','Text','True','','','28-Aug-2020 12:56:39 AM','A','False','6','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Type','Type','100px','Left','Text','True','','','28-Aug-2020 12:56:39 AM','A','False','7','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Visible','Visible','70px','Left','Checkbox','True','','','28-Aug-2020 12:56:40 AM','A','False','8','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Class','Class','100px','Left','Text','True','','','28-Aug-2020 12:56:41 AM','A','False','9','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','IsEdit','isEdit','50px','Left','Checkbox','True','','','28-Aug-2020 12:56:41 AM','A','False','13','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Order_No','Order','50px','Right','Number','True','','','28-Aug-2020 12:56:42 AM','A','False','14','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Validate','Valid.','50px','Left','Checkbox','True','','','28-Aug-2020 12:56:43 AM','A','False','15','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Style','Style','100px','Left','Text','True','','','28-Aug-2020 12:56:43 AM','A','False','16','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Posision','Posision','100px','Left','Text','True','','','28-Aug-2020 12:56:44 AM','A','False','17','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Default_Value','Value','100px','Left','Text','True','','','28-Aug-2020 12:56:44 AM','A','False','18','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Options','Options','100px','Left','Text','True','','','28-Aug-2020 12:56:45 AM','A','False','19','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Store_Value','Member','100px','Left','Text','True','','','28-Aug-2020 12:56:45 AM','A','False','20','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','GVisible','GVisible','100px','Left','Select','True','','','28-Aug-2020 12:56:46 AM','A','False','21','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','GOrder','GOrder','80px','Left','Text','True','','','28-Aug-2020 12:56:46 AM','A','False','22','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Field_setting_7','Mobile_Field','Mobile_Field','250px','Left','Text','True','','','28-Aug-2020 12:56:47 AM','A','True','23','False','','TL','','','','False','0','[]' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Customer_Name','Customer','10%','Left','Text','True','','','18-Oct-2020 7:01:30 PM','A','True','10','True','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Customer_Address1','Address','10%','Left','Text','True','','','18-Oct-2020 7:01:31 PM','A','True','11','True','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Customer_Address2','','10%','Left','Text','True','','','18-Oct-2020 7:01:31 PM','A','True','12','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','GST_No','GST No','10%','Left','Text','True','','','18-Oct-2020 7:01:32 PM','A','True','14','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Sales_Person','Sales Person','10%','Left','Text','True','','','18-Oct-2020 7:01:33 PM','A','True','15','False','','2TM','','User','value','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Bill_Mode','Bill Mode','10%','Left','Select','True','','','18-Oct-2020 7:01:33 PM','A','True','39','True','','2TM','Cash','Bill_Mode','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Place_of_Supply','Place of Supply','10%','Left','Text','True','','','18-Oct-2020 7:01:34 PM','A','True','42','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Area','Area','10%','Left','Text','True','','','18-Oct-2020 7:01:34 PM','A','True','43','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Bill_No','Bill No','10%','Left','Text','True','','','18-Oct-2020 7:01:34 PM','A','True','4','False','','3TR','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Bill_Date','Bill Date','10%','Left','Date','True','','','18-Oct-2020 7:01:35 PM','A','True','5','False','','3TR','T_Date','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Due_date','Due date','10%','Left','Date','True','','','18-Oct-2020 7:01:35 PM','A','True','7','False','','TR','T_Date','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Remarks','Remarks','10%','Left','Text','True','','','18-Oct-2020 7:01:36 PM','A','True','18','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Return_No','Return No','10%','Left','Text','True','','','18-Oct-2020 7:01:36 PM','A','True','2','False','','3TR','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Return_Date','Return Date','10%','Left','Date','True','','','18-Oct-2020 7:01:36 PM','A','True','3','False','','3TR','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Bill_Type','Bill Type','10%','Left','Select','True','','','18-Oct-2020 7:01:37 PM','A','True','6','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Customer_Address3','','10%','Left','Text','True','','','18-Oct-2020 7:01:37 PM','A','True','13','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_7','Net_Amt','Amount','10%','Right','Number','True','','','18-Oct-2020 7:01:38 PM','A','True','32','False','','TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','Item_Name','Name','25%','Left','Text','True','','','18-Oct-2020 8:27:33 PM','A','True','1','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','HSN_Code','HSN','10%','Left','Text','True','','','18-Oct-2020 8:27:34 PM','A','True','2','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','MRP','MRP','10%','Left','Text','True','','','18-Oct-2020 8:27:34 PM','A','True','3','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','Pcs','Qty','10%','Right','Number','True','','','18-Oct-2020 8:27:34 PM','A','True','4','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','Unit_Price','Rate','10%','Right','Number','True','','','18-Oct-2020 8:27:35 PM','A','True','5','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','Disc_Per','Disc(%)','10%','Right','Number','True','','','18-Oct-2020 8:27:35 PM','A','True','6','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','Description','Description','10%','Left','Text','True','','','18-Oct-2020 8:27:36 PM','A','True','9','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','Final_Price','Taxable','10%','Right','Number','True','','','18-Oct-2020 8:27:36 PM','A','True','16','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','GST_Per','GST(%)','10%','Right','Number','True','','','18-Oct-2020 8:27:37 PM','A','True','24','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_Retrun_Details_7','Net_Amt','Amount','10%','Left','Text','True','','','18-Oct-2020 8:27:37 PM','A','True','26','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Seraial_No_Settings_7','Name','Name','10%','Left','Text','True','','','18-Oct-2020 8:49:10 PM','A','True','2','False','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Seraial_No_Settings_7','Digits','Digits','10%','Left','Text','True','','','18-Oct-2020 8:49:11 PM','A','True','3','False','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Seraial_No_Settings_7','Prefix','Prefix','10%','Left','Text','True','','','18-Oct-2020 8:49:11 PM','A','True','4','False','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Seraial_No_Settings_7','suffix','suffix','10%','Left','Text','True','','','18-Oct-2020 8:49:12 PM','A','True','5','False','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','Item_Code','Code','10%','Left','Text','True','','','21-Oct-2020 9:54:44 PM','A','True','0','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','Item_Name','Name','25%','Left','Text','True','','','21-Oct-2020 9:54:44 PM','A','True','1','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','Stock','Stock','70px','Right','Text','True','stock','','21-Oct-2020 9:54:44 PM','A','False','2','False','Color:red','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','MRP','MRP','10%','Right','Number','True','','','21-Oct-2020 9:54:44 PM','A','True','3','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','Pcs','Qty','5%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','4','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','Unit_Price','Rate','10%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','6','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','Disc_Per','Disc(%)','5%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','7','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','Final_Price','Taxable','10%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','8','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','GST_Per','GST(%)','10%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','9','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_details_7','Net_Amt','Amount','15%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','10','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_Details_7','Item_Name','Item Name','30%','Left','Text','True','','','19-Nov-2020 5:44:58 PM','A','True','0','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_Details_7','Description','Description','20%','Left','Text','True','','','19-Nov-2020 5:44:58 PM','A','True','1','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_Details_7','Pcs','Qty ','20%','Right','Number','True','','','19-Nov-2020 5:44:58 PM','A','True','2','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_Details_7','Unit_Price','Rate ','10%','Right','Number','True','','','19-Nov-2020 5:44:58 PM','A','True','3','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_Details_7','SGST_Per','SGST %','10%','Right','Number','True','','','19-Nov-2020 5:44:58 PM','A','True','4','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_Details_7','CGST_Per','CGST %','10%','Right','Number','True','','','19-Nov-2020 5:44:59 PM','A','True','5','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_Details_7','Net_Amt','Amount ','20%','Right','Number','True','','','19-Nov-2020 5:44:59 PM','A','True','7','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_Details_7','GST_Per','GST_Per','10%','Right','Number','True','','','19-Nov-2020 5:44:59 PM','A','True','6','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','Item_Code','Code','10%','Left','Text','True','','','21-Oct-2020 9:54:44 PM','A','True','0','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','Item_Name','Name','25%','Left','Text','True','','','21-Oct-2020 9:54:44 PM','A','True','1','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','Stock','Stock','70px','Right','Text','True','stock','','21-Oct-2020 9:54:44 PM','A','False','2','False','Color:red','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','MRP','MRP','10%','Right','Number','True','','','21-Oct-2020 9:54:44 PM','A','True','3','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','Pcs','Qty','5%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','4','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','Unit_Price','Rate','10%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','6','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','Disc_Per','Disc(%)','5%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','7','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','Final_Price','Taxable','10%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','8','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','GST_Per','GST(%)','10%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','9','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_details_7','Net_Amt','Amount','15%','Right','Number','True','','','21-Oct-2020 9:54:45 PM','A','True','10','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Customer_Name','Customer','10%','Left','Text','True','','','19-Nov-2020 10:03:51 PM','A','True','2','True','','1TL','','','','True','2','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Customer_Address1','Address ','10%','Left','Text','True','','','19-Nov-2020 10:03:51 PM','A','True','3','True','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Customer_Address2','','10%','Left','Text','True','','','19-Nov-2020 10:03:51 PM','A','True','4','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Customer_Address3','','10%','Left','Text','True','','','19-Nov-2020 10:03:51 PM','A','True','5','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','GST_No','GST No','10%','Left','Text','True','','','19-Nov-2020 10:03:51 PM','A','True','12','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Bill_Type','Bill Type','10%','Left','Select','True','','','19-Nov-2020 10:03:51 PM','A','True','4','False','','2TM','Tax Invoice','Bill_Type','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Bill_Mode','Bill Mode','10%','Left','Select','True','','','19-Nov-2020 10:03:51 PM','A','True','37','True','','2TM','Cash','Bill_Mode','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Pay_Mode','Pay Terms','10%','Left','Text','True','','','19-Nov-2020 10:03:51 PM','A','True','40','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','DC_No','DC No','10%','Left','Text','True','','','19-Nov-2020 10:03:51 PM','A','True','2','False','','3TR','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','DC_Date','DC Date','10%','Left','Date','True','','','19-Nov-2020 10:03:51 PM','A','True','3','False','','3TR','T_Date','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Due_Date','Due date','10%','Left','Date','True','','','19-Nov-2020 10:03:51 PM','A','True','5','False','','3TR','T_Date','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Remarks','Remarks','10%','Left','Text','True','','','19-Nov-2020 10:03:51 PM','A','True','16','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Received_Bank','Received_Bank','10%','Left','Text','True','','','19-Nov-2020 10:03:51 PM','A','True','34','False','','BR','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Delivery_Challan_7','Net_Amt','Amount','10%','Right','Number','True','','','19-Nov-2020 10:03:51 PM','A','True','30','False','','TL','','','','True','3','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_Details_7','Item_Name','Name','25%','Left','Text','True','','','20-Nov-2020 7:36:17 AM','A','True','0','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_Details_7','Description','Description','30%','Left','Text','True','','','20-Nov-2020 7:36:17 AM','A','True','1','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_Details_7','Unit_Price','Rate','15%','Right','Number','True','','','20-Nov-2020 7:36:17 AM','A','True','4','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_Details_7','Pcs','Qty','10%','Right','Number','True','','','20-Nov-2020 7:36:17 AM','A','True','13','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_Details_7','Sub_total','Amount','20%','Right','Number','True','','','20-Nov-2020 7:36:17 AM','A','True','19','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Customer_Name','Customer ','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','0','True','','1TL','','','','True','2','Contact_Person,Contact_No' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Customer_Address1',' Address','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','1','True','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Customer_Address2','','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','2','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Customer_Address3','','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','3','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','GST_No','GST No','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','4','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Contact_Person','Contact Person','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','12','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Contact_No','Contact No','10%','Left','Text','True','mhide','','20-Nov-2020 7:49:29 AM','A','True','13','False','','2TM','','','','True','3','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Delivery_Mode','Shipping Mode','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','33','False','','2TM','Courier','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Payment_Terms','Payment Terms','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','35','False','','2TM','Advance 100%','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Valid_For','Valid For','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','36','False','','2TM','30 Days','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Quote_No','Quote No ','10%','Left','Text','True','','','20-Nov-2020 7:49:29 AM','A','True','0','False','','3TR','','','','True','0','Quote_Date,Valid_For' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Quote_Date','Date ','10%','Left','Date','True','mhide','','20-Nov-2020 7:49:29 AM','A','True','1','False','','3TR','T_Date','','','True','1','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Term','Term','10%','Left','TextArea','True','','','20-Nov-2020 7:49:29 AM','A','True','28','False','','4BL','1. MOQ 10000 PIC. 2. GST Extra as Applied 3. 50% Advance Payment','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Quotation_7','Net_Amt','Amount','10%','Right','Number','True','','','20-Nov-2020 7:49:29 AM','A','True','30','False','color:blue;','TL','','','','True','4','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Customer_Name','Customer','10%','Left','Text','True','','','20-Nov-2020 10:51:46 AM','A','True','8','True','','1TL','','','','True','2','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Customer_Address1','Address','10%','Left','Text','True','','','20-Nov-2020 10:51:46 AM','A','True','9','True','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Customer_Address2','','10%','Left','Text','True','','','20-Nov-2020 10:51:46 AM','A','True','10','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Customer_Address3','','10%','Left','Text','True','','','20-Nov-2020 10:51:46 AM','A','True','11','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','GST_No','GST No','10%','Left','Number','True','','','20-Nov-2020 10:51:46 AM','A','True','12','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Bill_Mode','Bill_Mode','10%','Left','Select','True','','','20-Nov-2020 10:51:46 AM','A','True','0','True','','2TM','Cash','Bill_Mode','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Place_of_Supply','Place of Supply','10%','Left','Text','True','','','20-Nov-2020 10:51:46 AM','A','True','1','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Area','Area','10%','Left','Text','True','','','20-Nov-2020 10:51:46 AM','A','True','2','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Sales_Person','Sales Person','10%','Left','Text','True','','','20-Nov-2020 10:51:46 AM','A','True','3','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Bill_No','Bill No','10%','Left','Text','True','','','20-Nov-2020 10:51:46 AM','A','True','0','False','','3TR','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Bill_Date','Bill Date','10%','Left','Date','True','','','20-Nov-2020 10:51:46 AM','A','True','1','False','','3TR','T_Date','','','True','1','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Remarks','Remarks','10%','Left','Text','True','','','20-Nov-2020 10:51:46 AM','A','True','16','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Bill_Type','Bill Type','10%','Left','Text','True','','','20-Nov-2020 10:51:47 AM','A','True','2','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Net_Amt','Amount','10%','Right','Number','True','','','20-Nov-2020 10:51:47 AM','A','True','30','False','','TL','','','','True','3','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Pay_Mode','Pay_Mode','10%','Left','Text','True','','','20-Nov-2020 10:51:47 AM','A','True','15','False','','TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Vehicle_No','Vehicle_No','10%','Left','Text','True','','','20-Nov-2020 10:51:47 AM','A','True','55','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','LL_RR_No','LL_RR_No','10%','Left','Text','True','','','20-Nov-2020 10:51:47 AM','A','True','56','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_7','Pay_Terms','Pay_Terms','10%','Left','Text','True','','','20-Nov-2020 10:51:47 AM','A','True','57','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Group_ID','Group','10%','Left','Select','True','','','20-Nov-2020 11:22:16 AM','A','True','0','False','','1TL','1','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Ledger_Name','Ledger Name','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','5','True','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Short_Name','Short Name','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','6','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Address1','Address','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','8','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Address2','','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','9','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Address3','','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','10','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Tax_Type','Tax Type','10%','Left','Select','True','','','20-Nov-2020 11:22:16 AM','A','True','27','False','','1TL','Local','Ledger_GST','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Contact_Person','Contact_Person','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','32','False','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Phone_Number','Ph No ','10%','Left','Number','True','','','20-Nov-2020 11:22:16 AM','A','True','33','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Shiping_Address1','Shipping Address','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','18','False','','2TR','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Shiping_Address2','','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','19','False','','2TR','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Shiping_Address3','','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','20','False','','2TR','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','Opening_Amt','Opening Amt','10%','Right','Number','True','','','20-Nov-2020 11:22:16 AM','A','True','45','False','','2TR','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Ledger_Master_7','GSTIN','GST No','10%','Left','Text','True','','','20-Nov-2020 11:22:16 AM','A','True','48','False','','2TR','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Customer_Name','Customer','10%','Left','Text','True','','','20-Nov-2020 11:24:34 AM','A','True','2','True','','1TL','','','','True','2','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Customer_Address1','Address ','10%','Left','Text','True','','','20-Nov-2020 11:24:34 AM','A','True','3','True','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Customer_Address2','','10%','Left','Text','True','','','20-Nov-2020 11:24:34 AM','A','True','4','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Customer_Address3','','10%','Left','Text','True','','','20-Nov-2020 11:24:34 AM','A','True','5','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','GST_No','GST No','10%','Left','Text','True','','','20-Nov-2020 11:24:34 AM','A','True','12','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Bill_Type','Bill Type','10%','Left','Select','True','','','20-Nov-2020 11:24:34 AM','A','True','4','False','','2TM','Tax Invoice','Bill_Type','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Bill_Mode','Bill Mode','10%','Left','Select','True','','','20-Nov-2020 11:24:34 AM','A','True','37','True','','2TM','Cash','Bill_Mode','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Pay_Mode','Pay Terms','10%','Left','Text','True','','','20-Nov-2020 11:24:35 AM','A','True','40','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Sales_Person','Sales Person','10%','Left','Text','True','','','20-Nov-2020 11:24:35 AM','A','True','41','False','','2TM','','User','value','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Bill_No','Bill No','10%','Left','Text','True','','','20-Nov-2020 11:24:35 AM','A','True','2','False','','3TR','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Bill_Date','Bill Date','10%','Left','Date','True','','','20-Nov-2020 11:24:35 AM','A','True','3','False','','3TR','T_Date','','','True','1','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Due_Date','Due_date','10%','Left','Date','True','','','20-Nov-2020 11:24:35 AM','A','True','5','False','','3TR','T_Date','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Remarks','Remarks','10%','Left','Text','True','','','20-Nov-2020 11:24:35 AM','A','True','16','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Vehicle_No','Vehicle No','10%','Left','Text','True','','','20-Nov-2020 11:24:35 AM','A','True','55','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','LL_RR_No','LL/RR No','10%','Left','Text','True','','','20-Nov-2020 11:24:35 AM','A','True','56','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Pay_Terms','Pay Terms','10%','Left','Text','True','','','20-Nov-2020 11:24:35 AM','A','True','57','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Received_Bank','Received_Bank','10%','Left','Text','True','','','20-Nov-2020 11:24:35 AM','A','True','34','False','','BR','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Sales_7','Net_Amt','Amount','10%','Right','Number','True','','','20-Nov-2020 11:24:35 AM','A','True','30','False','','TL','','','','True','3','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Supplier_Name','Supplier ','10%','Left','Text','True','','','20-Nov-2020 11:26:26 AM','A','True','2','False','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Supplier_Address1','Address','10%','Left','Text','True','','','20-Nov-2020 11:26:26 AM','A','True','3','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Supplier_Address2','','10%','Left','Text','True','','','20-Nov-2020 11:26:26 AM','A','True','45','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Supplier_Address3','','10%','Left','Text','True','','','20-Nov-2020 11:26:26 AM','A','True','46','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Bill_Type','Bill Type','10%','Left','Select','True','','','20-Nov-2020 11:26:26 AM','A','True','6','False','','2TM','Tax Invoice','Bill_Type','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Bill_Mode','Bill Mode','10%','Left','Select','True','','','20-Nov-2020 11:26:26 AM','A','True','10','False','','2TM','Cash','Bill_Mode','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Tax_Type','Tax Type','10%','Left','Select','True','','','20-Nov-2020 11:26:26 AM','A','True','50','False','','2TM','Exclusive','Tax_Type','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Disc_Type','Disc Type','10%','Left','Select','True','','','20-Nov-2020 11:26:26 AM','A','True','51','False','','2TM','Percentage','Disc_Type','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Purchase_No','Purchase No','10%','Left','Text','True','','','20-Nov-2020 11:26:26 AM','A','True','2','False','','3TR','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Purchase_Date','Date ','10%','Left','Date','True','','','20-Nov-2020 11:26:26 AM','A','True','3','False','','3TR','T_Date','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Bill_No','Bill No ','10%','Left','Text','True','','','20-Nov-2020 11:26:26 AM','A','True','4','False','','3TR','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Bill_Date','Bill Date','10%','Left','Date','True','','','20-Nov-2020 11:26:26 AM','A','True','5','False','','3TR','T_Date','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Ledger_ID','Ledger_ID','10%','Left','Text','True','','','20-Nov-2020 11:26:26 AM','A','True','1','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','GST_No','GST_No','10%','Left','Text','True','','','20-Nov-2020 11:26:26 AM','A','True','14','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_7','Net_Amt','Amount','10%','Right','Text','True','','','20-Nov-2020 11:26:26 AM','A','True','32','False','','TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Supplier_Name','Supplier Name','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','0','True','','1TL','','','','True','3','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Supplier_Address1','Address','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','1','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Supplier_Address2','','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','2','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Supplier_Address3','','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','3','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Your_Ref','Your Ref','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Paymet_Term','Paymet','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','2','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Delivery_Term','Delivery','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','3','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Insurence','Insurence','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','4','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Freight','Freight','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','36','False','','2TM','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','PO_No','PO No','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','0','True','','3TR','','','','True','1','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','PO_Date','PO Date','10%','Center','Date','True','','','20-Nov-2020 12:34:26 PM','A','True','1','False','','3TR','T_Date','','','True','2','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Remarks','Remarks','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','31','False','','4BL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Contact_Person','Contact_Person','10%','Left','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','11','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_Order_7','Net_Amt','PO Value','10%','Right','Text','True','','','20-Nov-2020 12:34:26 PM','A','True','26','False','','TL','','','','True','4','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_details_7','Item_Name','Name','25%','Left','Text','True','','','20-Nov-2020 12:56:48 PM','A','False','0','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_details_7','Description','Description','30%','Left','Text','True','','','20-Nov-2020 12:56:48 PM','A','False','1','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_details_7','Pcs','Qty','15%','Right','Number','True','','','20-Nov-2020 12:56:48 PM','A','True','2','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_details_7','Unit_Price','Rate','10%','Right','Number','True','','','20-Nov-2020 12:56:48 PM','A','True','3','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_details_7','GST_Per','GST%','10%','Left','Text','True','','','20-Nov-2020 12:56:48 PM','A','True','9','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('P_invoice_details_7','Sub_total','Amount','10%','Left','Text','True','','','20-Nov-2020 12:56:48 PM','A','True','15','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Item_Master_7','Item_Code','Code','10%','Left','Text','True','','','20-Nov-2020 2:51:40 PM','A','True','1','False','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Item_Master_7','Item_Name','Item Name','10%','Left','Text','True','','','20-Nov-2020 2:51:40 PM','A','True','2','True','','1TL','','','','True','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Item_Master_7','HSN_Code','HSN Code','10%','Left','Text','True','','','20-Nov-2020 2:51:40 PM','A','True','6','True','','1TL','','','','False','4','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Item_Master_7','Rate','Rate','10%','Right','Number','True','','','20-Nov-2020 2:51:40 PM','A','True','10','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Item_Master_7','GST_Per','GST(%)','10%','Right','Number','True','','','20-Nov-2020 2:51:40 PM','A','True','12','False','','1TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Item_Master_7','UOM','UOM','10%','Left','Select','True','','','20-Nov-2020 2:51:40 PM','A','True','18','False','','1TL','','UOM','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Item_Master_7','Category','Category','10%','Left','Select','True','','','20-Nov-2020 2:51:40 PM','A','True','5','False','','1TL','','Item_Category','label','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_details_7','Item_Code','Code','10%','Left','Text','True','','','20-Nov-2020 2:53:22 PM','A','True','0','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_details_7','Item_Name','Name','25%','Left','Text','True','','','20-Nov-2020 2:53:22 PM','A','True','1','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_details_7','Pcs','Qty','10%','Right','Number','True','','','20-Nov-2020 2:53:22 PM','A','True','5','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_details_7','Unit_Price','Rate','10%','Right','Number','True','','','20-Nov-2020 2:53:22 PM','A','True','7','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_details_7','Disc_Per','Disc(%)','5%','Right','Number','True','','','20-Nov-2020 2:53:22 PM','A','True','13','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_details_7','Total_Disc_Amt','Disc Amt','10%','Right','Number','True','','','20-Nov-2020 2:53:22 PM','A','True','15','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_details_7','GST_Per','GST(%)','10%','Right','Number','True','','','20-Nov-2020 2:53:22 PM','A','True','24','False','','TL','','','','False','0','' ) Insert into Field_Setting_7 ( Table_Name,Field,Name,Width,Align,Type,Visible,Class,Created_by,Created_Date,Status,IsEdit,Order_No,Validate,Style,Posision,Default_Value,Options,Store_Value,GVisible,GOrder,Mobile_Field ) Values('Purchase_details_7','Net_Amt','Amount','20%','Right','Number','True','','','20-Nov-2020 2:53:22 PM','A','True','26','False','','TL','','','','False','0','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('BOM_Master','BOM_Master','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Item_Master','Item_Master','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Ledger_Master','Ledger_Master','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Order','Order','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Order_details','Order_details','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('PDF_File_Setting','PDF_File_Setting','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Purchase','Purchase','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Purchase_details','Purchase_details','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Purchase_Order','Purchase_Order','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Purchase_Order_Details','Purchase_Order_Details','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Quotation','Quotation','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Quotation_Details','Quotation_Details','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Sales','Sales','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Sales_details','Sales_details','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Sales_Quotation','Sales_Quotation','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Seraial_No_Settings','Seraial_No_Settings','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Production','Production','','01-Jan-1900 12:00:00 AM','A' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Production_Details','Production_Details','','01-Jan-1900 12:00:00 AM','A' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('SMS_Setting','SMS_Setting','','01-Jan-1900 12:00:00 AM','' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Mail_Setting','Mail_Setting','','01-Jan-1900 12:00:00 AM','A' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Sales_Retrun_Details','Sales_Retrun_Details','','','A' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Sales_Retrun','Sales_Retrun','','','A' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Delivery_Challan_details','Delivery_Challan_details','','','A' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('Delivery_Challan','Delivery_Challan','','','A' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('P_invoice','P_invoice','','','A' ) Insert into Field_Setting_Table_7 ( Display_Name,TAB_Name,Created_by,Created_Date,Status ) Values('P_invoice_details','P_invoice_details','','','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('1','36','','001','Nos','','05-Jul-2020 9:42:14 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('2','36','','002','Kgs','','05-Jul-2020 9:42:37 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('3','34','','002','Cheque','','07-Jul-2020 11:08:20 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('4','34','','001','Cash','','07-Jul-2020 11:08:22 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('19','34','','003','Card','','22-Jul-2020 8:05:19 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('20','32','','001','Food','','25-Jul-2020 8:59:28 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('21','32','','002','Travel','','25-Jul-2020 8:59:41 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('29','3','','/sales-dashboard','Admin','','03-Aug-2020 2:30:06 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('35','7','','001','Tax Invoice','','23-Aug-2020 2:51:52 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('36','7','','002','Non Tax Invoice','','23-Aug-2020 2:52:06 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('37','8','','001','Cash','','23-Aug-2020 2:52:36 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('38','8','','002','Credit','','23-Aug-2020 2:52:55 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('39','9','','001','Inclusive','','23-Aug-2020 2:54:16 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('40','9','','002','Exclusive','','23-Aug-2020 2:54:28 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('45','38','','001','NXP','','08-Sep-2020 6:07:54 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('46','38','','002','STM','','08-Sep-2020 6:08:02 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('47','38','','003','TEXAS','','08-Sep-2020 6:08:10 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('48','38','','004','INCAP','','08-Sep-2020 6:08:17 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('69','15','','001','Percentage','','20-Oct-2020 2:31:23 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('70','15','','001','Amount','','20-Oct-2020 2:31:37 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('71','7','','003','Estimate','','22-Oct-2020 7:19:02 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('72','40','','001','Retail_Rate','','28-Oct-2020 12:14:40 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('73','40','','001','Builder_Rate','','28-Oct-2020 12:14:51 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('74','40','','001','Customer_Rate','','28-Oct-2020 12:15:01 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('75','34','','004','NEFT','','28-Oct-2020 3:30:11 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('76','36','','003','Mtr','','28-Oct-2020 4:51:14 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('77','41','','001','FINOLEX','','28-Oct-2020 5:03:04 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('78','41','','001','JAQUAR','','28-Oct-2020 5:06:11 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('122','42','','001','Advance','','02-Nov-2020 10:57:15 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('123','42','','001','Old Balance','','02-Nov-2020 10:57:26 AM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('124','36','','004','PIC','','19-Nov-2020 5:09:35 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('125','43','','001','Local','','19-Nov-2020 7:08:11 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('126','43','','002','Intra','','19-Nov-2020 7:08:18 PM','A' ) Insert into ReferenceGroup_Value_7 ( RGV_iID,RGV_iRG_ID,RGV_Line,RGV_vCode,RGV_vDesciption,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus ) Values('127','6','','001','TAPE','','20-Nov-2020 3:10:33 PM','A' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','22','7','Admin','02-Nov-2020 11:18:33 AM','A','Setting','1','Setting' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','33','1','vinoth','02-Nov-2020 11:18:33 AM','A','Setting','bx bx-user','User & Role' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','80','10','vinoth','02-Nov-2020 11:18:33 AM','A','Setting','bx bxs-paint','Theme Setting' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','81','11','vinoth','02-Nov-2020 11:18:33 AM','A','Setting','bx bxs-arrow-from-left','Backup' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','84','15','vinoth','02-Nov-2020 11:18:33 AM','A','Setting','bx bx-task','Serial No Setting' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','101','18','vinoth','02-Nov-2020 11:18:33 AM','A','Setting','bx bx-right-arrow','Variable Settings' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','111','18','vinoth','02-Nov-2020 11:18:33 AM','A','Setting','bx bx-repost','Rpost' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','37','18','vinoth','02-Nov-2020 11:18:33 AM','A','Setting','bx bx-file','Page Settup' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','36','19','vinoth','02-Nov-2020 11:18:33 AM','A','Setting','bx bx-cog','Compnay Setting' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','39','8','vinoth','02-Nov-2020 11:18:33 AM','A','Setting','bx bx-been-here','Field Setting' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','50','1','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','mshow','Transaction' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','59','1','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bx-file-blank','Quotation ' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','103','17','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bx-receipt','Receipt' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','51','6','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bx-money','Expense ' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','57','2','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bx-cart','Purchase ' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','109','20','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bx-file-blank','Non Tax Bill' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','112','21','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bx-transfer-alt','Delivery Challan' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','64','3','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bx-purchase-tag','Sales' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','18','0','Admin','18-Oct-2020 1:12:01 PM','A','Dashboard','bx bx-home-circle','Dashboard' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','105','4','vinoth','18-Oct-2020 1:12:02 PM','A','Dashboard','bx bx-home','Home' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','52','4','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bx-bolt-circle','Payment ' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','65','7','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bxl-foursquare','Proforma Invoice' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','66','8','vinoth','20-Nov-2020 8:56:54 AM','A','Transaction','bx bx-task','Purchase Order' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','20','3','Admin','20-Nov-2020 2:47:39 PM','A','Master','','Master' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','35','2','vinoth','20-Nov-2020 2:47:39 PM','A','Master','bx bx-file','Item Master' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','56','6','vinoth','20-Nov-2020 2:47:39 PM','A','Master','bx bx-group','Ledger Master' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','61','6','vinoth','20-Nov-2020 2:47:39 PM','A','Master','bx bx-cog','Bank Details' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','68','9','vinoth','20-Nov-2020 2:47:39 PM','A','Master','bx bx-list-ul','Ledger Group' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','31','6','vinoth','20-Nov-2020 2:47:39 PM','A','Master','bx bx-user','Reference' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','21','5','Admin','31-Oct-2020 7:26:51 AM','A','Report','','Report' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','60','8','vinoth','31-Oct-2020 7:26:51 AM','A','Report','bx bx-link-external','Stock ' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','108','11','vinoth','31-Oct-2020 7:26:51 AM','A','Report','bx bx-skip-next-circle','Profit Report' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','110','12','vinoth','31-Oct-2020 7:26:51 AM','A','Report','bx bx-purchase-tag-alt','Purchase' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','27','2','Admin','31-Oct-2020 7:26:51 AM','A','Report','bx bxs-layer','Sales' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','47','5','vinoth','31-Oct-2020 7:26:51 AM','A','Report','bx bx-receipt','GST Report' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','48','7','vinoth','31-Oct-2020 7:26:51 AM','A','Report','bx bxs-pyramid','Ledger' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','46','7','vinoth','31-Oct-2020 7:26:51 AM','A','Report','bx bxs-grid','Expence' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','45','7','vinoth','31-Oct-2020 7:26:51 AM','A','Report','bx bx-bible','Collection' ) Insert into Menu_Master_7 ( ID,Menu_ID,Display_Order,Created_by,Created_Date,Status,Module,Icon,Display_Name ) Values('','49','8','vinoth','31-Oct-2020 7:26:51 AM','A','Report','bx bx-file-blank','Outstanding' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('1','State','State','-','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('2','Area','Area','-','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('3','User_Role','User Role','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('6','Item_Category','Item Category','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('7','Bill_Type','Bill Type','','14-Jun-2020 9:23:36 PM','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('8','Bill_Mode','Bill Mode','','14-Jun-2020 9:23:36 PM','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('9','Tax_Type','Tax Type','','14-Jun-2020 9:23:36 PM','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('14','Department','Department','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('32','Exp_Category','Exp_Category','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('33','Income_Category','Income_Category','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('34','Pay_Mode','Pay Mode','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('36','UOM','UOM','','14-Jun-2020 9:23:36 PM','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('37','Ledger_Status','Ledger Statsu','','14-Jun-2020 9:23:36 PM','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('38','Make','Make','','14-Jun-2020 9:23:36 PM','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('39','Floor','Floor','','14-Jun-2020 9:23:36 PM','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('15','Disc_Type','Disc_Type','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('40','Rate_list','Rate_list','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('41','Brand','Brand','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('42','Payments','Payments','','','A' ) Insert into Reference_Group_7 ( RG_iID,RG_vCode,RG_vDescription,RG_vUpdatedBy,RG_dUpdateDate,RG_vStatus ) Values('43','Ledger_GST','Ledger_GST','','','A' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Logo_Name','Qubha','Qubha' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Bill_Format','Klisters','Format1' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Tax_Type','Inclusive','Inclusive' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Phone_No','+91 9597436220','+91 9597436220' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Vadi_Format','Format2','Format2' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Quotation_Format','Klisters','Format1' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Type_Based_Bill_No','true','true' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Sales_Disp_Text2_Visblle','true','false' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Sales_Disp_Text2','Open Invoice(with code)','Open Invoice' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Sales_Disp_Text1','Open Invoice','Open Invoice' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Bill_Format1','Format6','Format1' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('NT_Sales_Disp_Text1','Open Invoice','Open Invoice' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Sales_Disp_Text3_Visblle','true','false' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('Group_Enable','false','false' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('DC_Format','Klisters','Klisters1' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('PO_Format','Klisters','Format1' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('P_Invoice_Format','Klisters','Format1' ) Insert into setting_Master_7 ( S_Variable,S_Value,S_Default ) Values('NT_Bill_Format','Klisters','NT_Format1' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('1','Expense','4','EX','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('2','Income','4','IN','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('3','Sales','4','20-21/','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('4','Amount_Collection','4','RC20-21','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('5','Order','4','OR','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('7','Purchase','4','PUR','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('8','Purchase_Order','4','PO','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('9','Quotation','4','Q','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('10','P_Invoice','4','PI',' ' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('11','Sales_Retrun','4','R',' ' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('12','Tax Invoice','4','20-21/','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('13','Non Tax Invoice','4','NT','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('14','Payment','4','P','' ) Insert into Seraial_No_Settings_7 ( ID,Name,Digits,Prefix,suffix ) Values('15','Delivery_Challan','4','DC',' ' )";
            DataTable dt = GITAPI.dbFunctions.getTable(Q.Replace("_7", Company));
            return "True";

        }
       
    }
}
