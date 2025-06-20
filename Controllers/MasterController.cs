using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web.Http;

namespace Genuine_API.Controllers
{
    public class MasterController : ApiController
    {

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

        [HttpGet]
        public string get_reference(string Company)
        {
            string q = "select  *,(select [RG_vCode]  from Reference_Group where RG_iID=RGV_IRG_ID ) as Ref_ID ,RGV_vDesciption as label, RGV_IID as  [value] from ReferenceGroup_Value  order by Ref_ID, RGV_vDesciption";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpGet]
        public string get_reference_Group(string Company)
        {

            DataTable dt = GITAPI.dbFunctions.getTable("select RG_vDescription as label, RG_vCode as  value  from Reference_Group where RG_vStatus='A'");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpPost]
        public string UploadCompanyImage(string ID, string Name, string Company, string Description)
        {

            try
            {
                var exMessage = string.Empty;
                System.Web.HttpPostedFile myFile = System.Web.HttpContext.Current.Request.Files[0];

                string uploadPath = "~/Image/Company/";
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




                    file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(uploadPath + "/" + ID + ".png"));
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
        public string Upload_Sign_Image(string ID, string Name, string Company, string Description)
        {

            try
            {
                var exMessage = string.Empty;
                System.Web.HttpPostedFile myFile = System.Web.HttpContext.Current.Request.Files[0];

                string uploadPath = "~/Image/Company/";
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




                    file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(uploadPath + "/S_" + ID + ".png"));
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
        public string Upload_Expense_Document(string Ref_Type, string Ref_ID, string Name, string Description, string Company)
        {

            try
            {
                var exMessage = string.Empty;
                System.Web.HttpPostedFile myFile = System.Web.HttpContext.Current.Request.Files[0];

                string uploadPath = "/Image/C" + Company + "/Expense";
                System.Web.HttpPostedFile file = null;
                for (int i = 0; i < System.Web.HttpContext.Current.Request.Files.Count; i++)
                {
                    if (System.Web.HttpContext.Current.Request.Files.Count > 0)
                    {
                        file = System.Web.HttpContext.Current.Request.Files[i];
                    }

                    if (null == file)
                    {
                        return "file not found";
                    }

                    // Make sure the file has content
                    if (!(file.ContentLength > 0))
                    {
                        return "file not found";
                    }

                    if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(uploadPath)))
                    {
                        Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(uploadPath));
                    }

                    string s = Path.GetExtension(file.FileName);
                    s = s.ToLower();
                    string Type_ = ".png";

                    if (s.Equals(".jpg") || s.Equals(".jepg") || s.Equals(".png"))
                    {
                        Type_ = ".png";
                    }
                    else
                    {
                        Type_ = s;
                    }


                    string File_Name = file.FileName.Replace(s, "");



                    //Upload File
                    string FID = "";
                    DataTable dd = GITAPI.dbFunctions.getTable("select isnull(max(id),0)+1 from  Document_Upload" + Company);
                    if (dd.Rows.Count > 0)
                    {
                        FID = dd.Rows[0][0].ToString();
                    }
                    //File_Name = Ref_Type + "_" + Ref_ID + "_" + FID;
                    File_Name = Name;
                    File_Name = uploadPath + "/" + File_Name + Type_;

                    if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(File_Name)))
                    {
                        File.Delete(System.Web.HttpContext.Current.Server.MapPath(File_Name));
                    }

                    file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(File_Name));
                    insert_Document_Upload1(FID, Ref_Type, Ref_ID, Ref_Type, File_Name, uploadPath, Name, Description, Company);


                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "True";
        }

        public string insert_Document_Upload1(
            string ID,
            string Ref_Type,
            string Ref_ID,
            string File_Type,
            string File_Name,
            string File_Location,
            string Name,
            string Description,
            string Company)
        {

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;

                com.CommandText = "insert into Document_Upload" + Company + " (ID,Ref_Type,Ref_ID,File_Type,File_Name,File_Location,Name,Description,Created_by,Created_Date,Status) Values (@ID, @Ref_Type,@Ref_ID,@File_Type,@File_Name,@File_Location,@Name,@Description,@Created_by,getdate(), 'A'  )";
                //    com.CommandText = "update Document_Upload" + Company + " Set Ref_Type=@Ref_Type, Ref_ID=@Ref_ID, File_Type=@File_Type, File_Name=@File_Name, File_Location=@File_Location, Name=@Name, Description=@Description, Created_by=@Created_by where  ID=@ID ";
                com.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
                com.Parameters.Add("@Ref_Type", SqlDbType.VarChar).Value = Ref_Type;
                com.Parameters.Add("@Ref_ID", SqlDbType.VarChar).Value = Ref_ID;
                com.Parameters.Add("@File_Type", SqlDbType.VarChar).Value = File_Type;
                com.Parameters.Add("@File_Name", SqlDbType.VarChar).Value = File_Name;
                com.Parameters.Add("@File_Location", SqlDbType.VarChar).Value = File_Location;
                com.Parameters.Add("@Name", SqlDbType.VarChar).Value = Name;
                com.Parameters.Add("@Description", SqlDbType.VarChar).Value = Description;
                com.Parameters.Add("@Created_by", SqlDbType.VarChar).Value = "System";
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
        public string get_dashboard(string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select y.Route_Link as label  from Menu_Master" + Company + " x left outer join Menu_Master y on x.Menu_id=y.ID where y.Module='Dashboard' and y.type='Menu' ");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Ref_Code(string Type, string Company)
        {
            string Query = "";

            Query += "select right('000'+cast( isnull(max(cast( RGV_vCode as int)),0)+1 as varchar(33)),3) as [No]  from  ReferenceGroup_Value" +
                    " where (select RG_Vcode from Reference_Group where RGV_iRG_ID=RG_IID)='" + Type + "' " +
                    " and isnumeric(RGV_vCode)=1";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }

        [HttpGet]
        public string delete_Reference_Values(string ID, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("delete from Referencegroup_Value where RGV_iId=" + ID);
            return "True";

        }

        [HttpPost]
        public string insert_Reference_Values(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string Ref_ID = isnull(json.Ref_ID, "");
            string Code = isnull(json.Code, "");
            string Descrption = isnull(json.Descrption, "");
            string Remarks = isnull(json.Remarks, "");
            string Created_by = isnull(json.Created_by, "");
            string Created_Date = isnull(json.Created_Date, "");
            string Status = isnull(json.Status, "");
            string Company = isnull(json.Company, "");


            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                Boolean isEdit = false;
                string old_Category = "";
                if (ID == "0")
                {
                    isEdit = false;
                    DataTable dt = GITAPI.dbFunctions.getTable("select isnull(max(RGV_iID),0)+1 from ReferenceGroup_Value");
                    if (dt.Rows.Count > 0)
                    {
                        ID = dt.Rows[0][0].ToString();
                    }

                    com.CommandText = "insert into ReferenceGroup_Value (RGV_iID,RGV_iRG_ID,RGV_vCode,RGV_vDesciption,RGV_Line,RGV_vUpdatedBy,RGV_dUpdateDate,RGV_vStatus) Values (@ID,@Ref_ID,@Code,@Descrption,@Remarks,@Created_by,getdate(), 'A'  )";
                }
                else
                {

                    DataTable dds = GITAPI.dbFunctions.getTable("select * from ReferenceGroup_Value  where  RGV_iID=" + ID);
                    old_Category = dds.Rows[0]["RGV_vDesciption"].ToString();
                    isEdit = true;

                    com.CommandText = "update ReferenceGroup_Value Set RGV_iRG_ID=@Ref_ID, RGV_vCode=@Code, RGV_vDesciption=@Descrption, RGV_Line=@Remarks, RGV_vUpdatedBy=@Created_by where  RGV_iID=@ID ";

                }
                com.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
                DataTable dd = GITAPI.dbFunctions.getTable("select  RG_IID FROM Reference_Group where RG_vCode='" + Ref_ID + "'");
                com.Parameters.Add("@Ref_ID", SqlDbType.VarChar).Value = dd.Rows[0][0].ToString();
                com.Parameters.Add("@Code", SqlDbType.VarChar).Value = Code;
                com.Parameters.Add("@Descrption", SqlDbType.VarChar).Value = Descrption;
                com.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = Remarks;
                com.Parameters.Add("@Created_by", SqlDbType.VarChar).Value = Created_by;
                com.ExecuteNonQuery();
                com.Connection.Close();

                //if (isEdit == true && dd.Rows[0][0].ToString() == "6")
                //{
                //    DataTable dd1 = GITAPI.dbFunctions.getTable("update Item_master" + Company + " set Category='" + Descrption + "' where  Category='" + old_Category + "'");
                //}
                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string Post_Menu(JObject jsonData)
        {
            dynamic json = jsonData;
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");
            string ID = isnull(json.ID, "0");
            string Query = "";
            Boolean isIDn = false;

            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='" + Table_Name + "" + Company + "'");

            DataTable dc = GITAPI.dbFunctions.getTable("SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = '" + Table_Name + "" + Company + "'");
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
                        DataTable dd = GITAPI.dbFunctions.getTable("select isnull(max(" + dt.Rows[0]["Column_Name"].ToString() + "),50)+1 from " + Table_Name + "" + Company);
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
                    Query = "";
                    Query += "update  " + Table_Name + " Set ";

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

        //[HttpGet]
        //public string get_Item_Master(string Order_by, string Category = "all", string Sub_Category = "all", string Item_Status = "all", string Company = "")
        //{

        //    string condi = "";

        //    if (Category.ToLower() != "all")
        //    {
        //        condi += " and  Category='" + Category.Replace("~", "&") + "'";
        //    }
        //    if (Sub_Category.ToLower() != "all")
        //    {
        //        condi += " and  Sub_Category='" + Sub_Category + "'";
        //    }

        //    if (Item_Status.ToLower() != "all")
        //    {
        //        condi += " and  Item_Status='" + Item_Status + "'";
        //    }


        //    string Query = "select ID as ID,*,ID as [value],Item_Name as label,0 as Order_Qty,'' as Bag,'' as Qty from Item_Master" + Company + " where 0=0" + condi + " and  Item_or_Group='I'  order by " + Order_by;
        //    DataTable dt = GITAPI.dbFunctions.getTable(Query);
        //    string data = GITAPI.dbFunctions.GetJSONString(dt);
        //    return data;
        //}

        [HttpGet]
        public string get_Item_Master(string Order_by, string Company)
        {

            string condi = "";


            string Query = "select pm_id as ID,pm_id as ID1,pm_category as Category,pm_item_name as Item_Name,pm_rate as Rate, pm_item_code as Item_Code,pm_uom as UOM,pm_wholesale_rate as Wholesale_Rate,pm_hsn_code as HSN_Code, pm_gst_per as GST_Per,pm_stock as Stock,pm_item_group as Item_Group,pm_item_tamilname as Item_TamilName,pm_description as [Description], pm_size as Size,pm_bag_qty as Bag_Qty,pm_mrpprice as MRP,pm_purprice as Purchase_Rate,pm_min_stock as Min_Stock ,pm_id as [value],pm_item_name as label,0 as Order_Qty,'' as Bag,'' as Qty from Product_Master where 0=0" + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Customer_Master(string Status, string vType, string Order_by, string Company = "0")
        {
            string condi = "";

            if (Status.ToLower() != "all")
            {
                condi += " and  cus_status='" + Status + "'";
            }
            if (vType.ToLower() != "all")
            {
                condi += " and  cus_type='" + vType + "'";
            }

            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  cus_company='" + Company.Replace("_", "") + "'";
            //}

            if (Order_by.ToLower() != "0")
            {
                condi += " Order by " + Order_by;
            }
            string q = "select  *,FORMAT(cus_created_date,'dd/MM/yyyy hh:mm tt') as cus_created_date from Ledger_Master  where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpGet]
        public string get_Customer_Master1(string vType, string Company = "0")
        {
            string condi = "";

            if (vType.ToLower() != "all")
            {
                condi += " and  cus_type='" + vType + "'";
            }

            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  cus_branch='" + Company.Replace("_", "") + "'";
            //}
            string q = "select  *,FORMAT(cus_created_date,'dd/MM/yyyy hh:mm tt') as CDate from Ledger_Master  where cus_status='A'" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpGet]
        public string delete_Customer_Master(string ID, string UserName, string Status, string Company)
        {
            string q = "UPDATE Ledger_Master SET cus_status='" + Status + "',cus_del_date='" + GITAPI.dbFunctions.getdate() + "',cus_del_by='" + UserName + "'  where cus_id=" + ID;
            GITAPI.dbFunctions.getTable(q);
            return "True";
        }


        public string Post_Data(JObject jsonData)
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
                        //if (auto == 0)
                        //{
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                        //}
                        //else
                        //{
                        //    string created_date = "";
                        //    TempQ= "select "+Column+" from " + Table_Name + " where " +ColumnPerfix + "id="+ ID + "";
                        //    DataTable dd = GITAPI.dbFunctions.getTable(TempQ);
                        //    created_date = dd.Rows[0][0].ToString();
                        //    DateTime time = DateTime.Parse(created_date); ;              // Use current time
                        //    string format = "yyyy-MM-dd HH:mm:ss";    // modify the format depending upon input required in the column in database 
                        //    com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = time.ToString(format);
                        //}
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = "A";
                    }

                    else if (Column.ToLower().Equals(ColumnPerfix + "created_by") && auto == 0)
                    {
                        //if (auto == 0)
                        //{
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
                        //}
                        //else
                        //{
                        //    string createdby = "";
                        //    TempQ = "select " + Column + " from " + Table_Name + " where " + ColumnPerfix + "id=" + ID + "";
                        //    DataTable dd = GITAPI.dbFunctions.getTable(TempQ);
                        //    createdby = dd.Rows[0][0].ToString();

                        //    com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = createdby.ToString();
                        //}
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "company"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Company.Replace("_", "");
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "delby") && auto == 1)
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "deldate") && auto == 1)
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

        //[HttpPost]
        //public string insert_Customer_Master(JObject jsonData)
        //{
        //    dynamic json = jsonData;
        //    string Cus_ID = isnull(json.Cus_ID, "");
        //    string Cus_maxid = "";
        //    string Cus_type = isnull(json.Cus_type, "");
        //    string Cus_Code = isnull(json.Cus_Code, "");
        //    string Cus_Name = isnull(json.Cus_Name, "");
        //    string Cus_Address = isnull(json.Cus_Address, "");
        //    string Cus_Addr2 = isnull(json.Cus_Addr2, "");
        //    string Cus_City = isnull(json.Cus_City, "");
        //    string Cus_Pincode = isnull(json.Cus_Pincode, "");
        //    string Cus_ContactPerson = isnull(json.Cus_ContactPerson, "");
        //    string Cus_ContactNo = isnull(json.Cus_ContactNo, "");
        //    string Cus_MobileNo = isnull(json.Cus_MobileNo, "");
        //    string Cus_GSTIN = isnull(json.Cus_GSTIN, "");
        //    string Cus_StateId = isnull(json.Cus_StateId, "");
        //    string Cus_State = isnull(json.Cus_State,"");
        //    //string old_Category = "";
        //    string Cus_SCode = isnull(json.Cus_SCode, "");
        //    string Cus_OB = isnull(json.Cus_OB, "0");
        //    string Cus_CreatedBy = isnull(json.Cus_CreatedBy, "");
        //    string Company = isnull(json.Company, "");

        //    string Cus_Company = Company.Replace("_", "");


        //    SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
        //    try
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand();
        //        com.Connection = con;
        //        com.CommandType = CommandType.Text;
        //        //DataTable dds = GITAPI.dbFunctions.getTable("select * from ReferenceGroup_Value  where  RGV_iID=" + Cus_StateId);
        //        //if (dds.Rows.Count > 0)
        //        //{
        //        //    old_Category = dds.Rows[0]["RGV_vDesciption"].ToString();
        //        //}
        //        if (Cus_ID == "0")
        //        {
        //            DataTable dd = GITAPI.dbFunctions.getTable("select  isnull(max(Cus_ID),0)+1 from Customer_Master");
        //            if (dd.Rows.Count > 0)
        //            {
        //                Cus_ID = dd.Rows[0][0].ToString();
        //            }

        //            DataTable dd1 = GITAPI.dbFunctions.getTable("select isnull(max(convert(int,Cus_maxid)),0)+1 from Customer_Master");
        //            if (dd1.Rows.Count > 0)
        //            {
        //                Cus_maxid = dd1.Rows[0][0].ToString();
        //            }

        //            com.CommandText = "insert into Customer_Master (Cus_ID,Cus_maxid,Cus_type,Cus_Code,Cus_Name,Cus_Address,Cus_Addr2,Cus_City,Cus_Pincode,Cus_ContactPerson,Cus_ContactNo,Cus_MobileNo,Cus_GSTIN,Cus_State,Cus_SCode,Cus_OB,Cus_Status,Cus_date,Cus_CreatedBy,Cus_Branch,Cus_Company,Cus_StateId) Values " +
        //                "(@Cus_ID,@Cus_maxid,@Cus_type,@Cus_Code,@Cus_Name,@Cus_Address,@Cus_Addr2,@Cus_City,@Cus_Pincode,@Cus_ContactPerson,@Cus_ContactNo,@Cus_MobileNo,@Cus_GSTIN,@Cus_State,@Cus_SCode,@Cus_OB,'A',GETDATE(),@Cus_CreatedBy,@Cus_Branch,@Cus_Company,@Cus_StateId)";
        //        }
        //        else
        //        {
        //            com.CommandText = "update Customer_Master Set Cus_Code=@Cus_Code, Cus_Name=@Cus_Name, Cus_Address=@Cus_Address, Cus_Addr2=@Cus_Addr2,Cus_City=@Cus_City,Cus_ContactPerson=@Cus_ContactPerson, Cus_ContactNo=@Cus_ContactNo,Cus_MobileNo=@Cus_MobileNo," +
        //                "Cus_GSTIN=@Cus_GSTIN,Cus_State=@Cus_State,Cus_SCode=@Cus_SCode,Cus_DelBy=@Cus_CreatedBy,Cus_DelDate=getdate(),Cus_StateId=@Cus_StateId where  Cus_ID=@Cus_ID ";

        //        }
        //        com.Parameters.Add("@Cus_ID", SqlDbType.VarChar).Value = Cus_ID;
        //        com.Parameters.Add("@Cus_maxid", SqlDbType.VarChar).Value = Cus_maxid;
        //        com.Parameters.Add("@Cus_type", SqlDbType.VarChar).Value = Cus_type;
        //        com.Parameters.Add("@Cus_Code", SqlDbType.VarChar).Value = Cus_Code;
        //        com.Parameters.Add("@Cus_Name", SqlDbType.VarChar).Value = Cus_Name;
        //        com.Parameters.Add("@Cus_Address", SqlDbType.VarChar).Value = Cus_Address;
        //        com.Parameters.Add("@Cus_Addr2", SqlDbType.VarChar).Value = Cus_Addr2;
        //        com.Parameters.Add("@Cus_City", SqlDbType.VarChar).Value = Cus_City;
        //        com.Parameters.Add("@Cus_Pincode", SqlDbType.VarChar).Value = Cus_Pincode;
        //        com.Parameters.Add("@Cus_ContactPerson", SqlDbType.VarChar).Value = Cus_ContactPerson;
        //        com.Parameters.Add("@Cus_ContactNo", SqlDbType.VarChar).Value = Cus_ContactNo;
        //        com.Parameters.Add("@Cus_MobileNo", SqlDbType.VarChar).Value = Cus_MobileNo;
        //        com.Parameters.Add("@Cus_GSTIN", SqlDbType.VarChar).Value = Cus_GSTIN;
        //        com.Parameters.Add("@Cus_State", SqlDbType.VarChar).Value = Cus_State;
        //        com.Parameters.Add("@Cus_SCode", SqlDbType.VarChar).Value = Cus_SCode;
        //        com.Parameters.Add("@Cus_OB", SqlDbType.VarChar).Value = Cus_OB;
        //        com.Parameters.Add("@Cus_CreatedBy", SqlDbType.VarChar).Value = Cus_CreatedBy;
        //        com.Parameters.Add("@Cus_Branch", SqlDbType.VarChar).Value = Cus_Company;
        //        com.Parameters.Add("@Cus_Company", SqlDbType.VarChar).Value = Cus_Company;
        //        com.Parameters.Add("@Cus_StateId", SqlDbType.VarChar).Value = Cus_StateId;
        //        com.ExecuteNonQuery();
        //        com.Connection.Close();
        //        return "True";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}


        [HttpGet]
        public string get_product_Master(string Status, string vType, string Order_by, string Company = "0")
        {

            string condi = "";

            if (Status.ToLower() == "undefined")
            {
                Status = "A";
            }
            if (Status.ToLower() != "all")
            {
                condi += " and  pm_status='" + Status + "'";
            }
            if (vType.ToLower() != "all")
            {
                condi += " and  pm_type='" + vType + "'";
            }

            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  pm_company='" + Company.Replace("_", "") + "'";
            //}

            if (Order_by.ToLower() != "")
            {
                condi += " Order by " + Order_by;
            }

            string Query = "select *,FORMAT(pm_created_date,'dd/MM/yyyy hh:mm tt') as CDate from Product_Master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_product_Master1(string Company = "0")
        {

            string condi = "";

            condi += " and  pm_status='A'";

            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  pm_company='" + Company.Replace("_", "") + "'";
            //}


            string Query = "select pm_id as ID,pm_maxid as ID1,pm_item_group as Item_Group,pm_item_code as Item_Code,pm_item_name as Item_Name,pm_item_name as label,pm_id as value,pm_item_tamilname as Item_TamilName,'' as Description," +
                " pm_short_name as Short_Name,pm_brand as Category,pm_model as Model,pm_size as Size,pm_uom as UOM,pm_bag_qty as Bag_Qty,pm_gst_per as GST_Per, " +
                " pm_hsn_code as HSN_Code,pm_mrpprice as MRP,pm_rate as Rate,0 as Wholesale_Rate,pm_purprice as Purchase_Rate,0 as Parcel_Rate,0 as Order_Qty,'' as Bag,'' as Qty from Product_Master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string delete_Product_Master(string ID, string UserName, string Status, string Company)
        {
            string q = "UPDATE Product_Master SET pm_status='" + Status + "',pm_deldate='" + GITAPI.dbFunctions.getdate() + "',pm_delby='" + UserName + "'  where pm_id=" + ID;
            GITAPI.dbFunctions.getTable(q);
            return "True";
        }



        [HttpPost]
        public string insert_Product_Master(JObject jsonData)
        {
            dynamic json = jsonData;
            string PM_id = isnull(json.PM_id, "");
            string PM_maxid = "";
            string pm_type = isnull(json.pm_type, "");
            string pm_Code = isnull(json.pm_Code, "");
            string pm_Group = isnull(json.pm_Group, "");
            string pm_name = isnull(json.pm_name, "");
            string pm_tname = isnull(json.pm_tname, "");
            string pm_brand = isnull(json.pm_brand, "");

            string pm_UOM = isnull(json.pm_UOM, "");
            string old_Category = "";
            string pm_hsncode = isnull(json.pm_hsncode, "");
            string pm_nqty = isnull(json.pm_nqty, "");
            string pm_purprice = isnull(json.pm_purprice, "");
            string pm_mrpprice = isnull(json.pm_mrpprice, "");
            string pm_GST = isnull(json.pm_GST, "");
            string pm_CreatedBy = isnull(json.pm_CreatedBy, "");
            string pm_OB = isnull(json.pm_OB, "0");
            string pm_ratetype = isnull(json.pm_ratetype, "");
            string pm_barcode = "";
            string pm_rate = isnull(json.pm_rate, "0");
            string pm_ARate = isnull(json.pm_ARate, "0");
            string pm_AMargin = isnull(json.pm_AMargin, "0");
            string pm_AMarginrs = isnull(json.pm_AMarginrs, "0");
            string pm_BRate = isnull(json.pm_BRate, "0");
            string pm_BMargin = isnull(json.pm_BMargin, "0");
            string pm_BMarginrs = isnull(json.pm_BMarginrs, "0");
            string pm_CRate = isnull(json.pm_CRate, "0");
            string pm_CMargin = isnull(json.pm_CMargin, "0");
            string pm_CMarginrs = isnull(json.pm_CMarginrs, "0");
            string pm_DRate = isnull(json.pm_DRate, "0");
            string pm_DMargin = isnull(json.pm_DMargin, "0");
            string pm_DMarginrs = isnull(json.pm_DMarginrs, "0");
            string pm_Cess = isnull(json.pm_Cess, "0");
            string pm_EmpCom = isnull(json.pm_EmpCom, "0");
            string pm_AGCom = isnull(json.pm_AGCom, "0");
            string Company = isnull(json.Company, "");

            string pm_branch = Company.Replace("_", "");

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                DataTable dds = GITAPI.dbFunctions.getTable("select * from ReferenceGroup_Value  where  RGV_iID=" + pm_UOM);
                if (dds.Rows.Count > 0)
                {
                    old_Category = dds.Rows[0]["RGV_vDesciption"].ToString();
                }
                if (PM_id == "0")
                {
                    DataTable dd = GITAPI.dbFunctions.getTable("select  isnull(max(PM_id),0)+1 from Product_Master");
                    if (dd.Rows.Count > 0)
                    {
                        PM_id = dd.Rows[0][0].ToString();
                    }

                    DataTable dd1 = GITAPI.dbFunctions.getTable("select isnull(max(convert(int,PM_maxid)),0)+1 from product_master");
                    if (dd.Rows.Count > 0)
                    {
                        PM_maxid = dd1.Rows[0][0].ToString();
                    }
                    DataTable dd2 = GITAPI.dbFunctions.getTable("select isnull(max(convert(int,pm_barcode)),999)+1 from product_master");
                    if (dd.Rows.Count > 0)
                    {
                        pm_barcode = dd2.Rows[0][0].ToString();
                    }

                    com.CommandText = "INSERT INTO Product_Master" +
                "(PM_id, PM_maxid, pm_type, pm_Code, pm_Group, pm_name, pm_tname, pm_brand, pm_UOM, pm_hsncode, pm_nqty, pm_purprice, pm_mrpprice, pm_GST, pm_Status, pm_date, " +
                "pm_CreatedBy, pm_branch, pm_OB, pm_ratetype, pm_barcode, pm_rate, pm_ARate, pm_AMargin, pm_AMarginrs, pm_BRate, pm_BMargin, pm_BMarginrs, pm_CRate, pm_CMargin, " +
                "pm_CMarginrs, pm_DRate, pm_DMargin, pm_DMarginrs, pm_Cess, pm_EmpCom, pm_AGCom,pm_UOMId)     VALUES " +
                "(@PM_id, @PM_maxid, @pm_type, @pm_Code, @pm_Group, @pm_name, @pm_tname, @pm_brand, @pm_UOM, @pm_hsncode, @pm_nqty, @pm_purprice, @pm_mrpprice, @pm_GST, " +
                "'A', GETDATE(), @pm_CreatedBy, @pm_branch, @pm_OB, @pm_ratetype, @pm_barcode, @pm_rate, @pm_ARate, @pm_AMargin, @pm_AMarginrs, @pm_BRate, @pm_BMargin, " +
                "@pm_BMarginrs, @pm_CRate, @pm_CMargin, @pm_CMarginrs, @pm_DRate, @pm_DMargin, @pm_DMarginrs, @pm_Cess, @pm_EmpCom, @pm_AGCom,@pm_UOMId)";
                }
                else
                {
                    com.CommandText = "UPDATE Product_Master SET pm_type=@pm_type,pm_Code=@pm_Code,pm_Group=@pm_Group,pm_name=@pm_name,pm_tname=@pm_tname,pm_brand=@pm_brand, " +
                "pm_UOM = @pm_UOM,pm_hsncode = @pm_hsncode,pm_nqty = @pm_nqty,pm_purprice = @pm_purprice,pm_mrpprice = @pm_mrpprice,pm_GST = @pm_GST,pm_OB = @pm_OB, " +
                "pm_ratetype = @pm_ratetype,pm_rate = @pm_rate,pm_ARate = @pm_ARate,pm_AMargin = @pm_AMargin,pm_AMarginrs = @pm_AMarginrs,pm_BRate = @pm_BRate, " +
                "pm_BMargin = @pm_BMargin,pm_BMarginrs = @pm_BMarginrs,pm_CRate = @pm_CRate,pm_CMargin = @pm_CMargin,pm_CMarginrs = @pm_CMarginrs,pm_DRate = @pm_DRate,pm_UOMId=@pm_UOMId, " +
                "pm_DMargin = @pm_DMargin,pm_DMarginrs = @pm_DMarginrs,pm_Cess = @pm_Cess,pm_EmpCom = @pm_EmpCom,pm_AGCom = @pm_AGCom,pm_DelDate = GETDATE(),pm_DelBy = @pm_CreatedBy " +
                "WHERE PM_id = @PM_id";

                }
                com.Parameters.Add("@PM_id", SqlDbType.VarChar).Value = PM_id;
                com.Parameters.Add("@PM_maxid", SqlDbType.VarChar).Value = PM_maxid;
                com.Parameters.Add("@pm_type", SqlDbType.VarChar).Value = pm_type;
                com.Parameters.Add("@pm_Code", SqlDbType.VarChar).Value = pm_Code;
                com.Parameters.Add("@pm_Group", SqlDbType.VarChar).Value = pm_Group;
                com.Parameters.Add("@pm_name", SqlDbType.VarChar).Value = pm_name;
                com.Parameters.Add("@pm_tname", SqlDbType.VarChar).Value = pm_tname;
                com.Parameters.Add("@pm_brand", SqlDbType.VarChar).Value = pm_brand;
                com.Parameters.Add("@pm_UOM", SqlDbType.VarChar).Value = old_Category;
                com.Parameters.Add("@pm_hsncode", SqlDbType.VarChar).Value = pm_hsncode;
                com.Parameters.Add("@pm_nqty", SqlDbType.VarChar).Value = pm_nqty;
                com.Parameters.Add("@pm_purprice", SqlDbType.VarChar).Value = pm_purprice;
                com.Parameters.Add("@pm_mrpprice", SqlDbType.VarChar).Value = pm_mrpprice;
                com.Parameters.Add("@pm_GST", SqlDbType.VarChar).Value = pm_GST;
                com.Parameters.Add("@pm_CreatedBy", SqlDbType.VarChar).Value = pm_CreatedBy;
                com.Parameters.Add("@pm_branch", SqlDbType.VarChar).Value = pm_branch;
                com.Parameters.Add("@pm_OB", SqlDbType.VarChar).Value = pm_OB;
                com.Parameters.Add("@pm_ratetype", SqlDbType.VarChar).Value = pm_ratetype;
                com.Parameters.Add("@pm_barcode", SqlDbType.VarChar).Value = pm_barcode;
                com.Parameters.Add("@pm_rate", SqlDbType.VarChar).Value = pm_rate;
                com.Parameters.Add("@pm_ARate", SqlDbType.VarChar).Value = pm_ARate;
                com.Parameters.Add("@pm_AMargin", SqlDbType.VarChar).Value = pm_AMargin;
                com.Parameters.Add("@pm_AMarginrs", SqlDbType.VarChar).Value = pm_AMarginrs;
                com.Parameters.Add("@pm_BRate", SqlDbType.VarChar).Value = pm_BRate;
                com.Parameters.Add("@pm_BMargin", SqlDbType.VarChar).Value = pm_BMargin;
                com.Parameters.Add("@pm_BMarginrs", SqlDbType.VarChar).Value = pm_BMarginrs;
                com.Parameters.Add("@pm_CRate", SqlDbType.VarChar).Value = pm_CRate;
                com.Parameters.Add("@pm_CMargin", SqlDbType.VarChar).Value = pm_CMargin;
                com.Parameters.Add("@pm_CMarginrs", SqlDbType.VarChar).Value = pm_CMarginrs;
                com.Parameters.Add("@pm_DRate", SqlDbType.VarChar).Value = pm_DRate;
                com.Parameters.Add("@pm_DMargin", SqlDbType.VarChar).Value = pm_DMargin;
                com.Parameters.Add("@pm_DMarginrs", SqlDbType.VarChar).Value = pm_DMarginrs;
                com.Parameters.Add("@pm_Cess", SqlDbType.VarChar).Value = pm_Cess;
                com.Parameters.Add("@pm_EmpCom", SqlDbType.VarChar).Value = pm_EmpCom;
                com.Parameters.Add("@pm_AGCom", SqlDbType.VarChar).Value = pm_AGCom;
                com.Parameters.Add("@pm_UOMId", SqlDbType.VarChar).Value = pm_UOM;
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
        public string get_employee_master(string Status, string Order_by, string Company = "0")
        {

            string condi = "";

            if (Status.ToLower() == "undefined")
            {
                Status = "A";
            }

            if (Status.ToLower() != "all")
            {
                condi += " and  Emp_status='" + Status + "'";
            }
            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  Emp_Company='" + Company.Replace("_", "") + "'";
            //}

            if (Order_by.ToLower() != "" || Order_by.ToLower() != null)
            {
                condi += " Order by " + Order_by;
            }

            string Query = "select *,FORMAT(Emp_date,'dd/MM/yyyy hh:mm tt') as CDate from employee_master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string delete_employee_master(string ID, string UserName, string Status, string Company)
        {
            GITAPI.dbFunctions.getTable("UPDATE employee_master SET Emp_status='" + Status + "',Emp_DelDate='" + GITAPI.dbFunctions.getdate() + "',Emp_DelBy='" + UserName + "'  where Emp_id=" + ID);
            return "True";
        }


        [HttpPost]
        public string insert_employee_master(JObject jsonData)
        {
            dynamic json = jsonData;
            string Emp_id = isnull(json.Emp_id, "");
            string Emp_maxid = "";
            string Emp_code = isnull(json.Emp_code, "");
            string Emp_name = isnull(json.Emp_name, "");
            string Emp_address = isnull(json.Emp_address, "");
            string Emp_phone = isnull(json.Emp_phone, "");
            string Emp_email = isnull(json.Emp_email, "");
            string Emp_designation = isnull(json.Emp_designation, "");
            string Emp_department = isnull(json.Emp_department, "");
            string Emp_Salary = isnull(json.Emp_Salary, "0");
            string Emp_commission = isnull(json.Emp_commission, "0");
            string Emp_CreatedBy = isnull(json.Emp_CreatedBy, "");
            string Company = isnull(json.Company, "");

            string Emp_Company = Company.Replace("_", "");


            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (Emp_id == "0")
                {
                    DataTable dd = GITAPI.dbFunctions.getTable("select  isnull(max(Emp_id),0)+1 from employee_master");
                    if (dd.Rows.Count > 0)
                    {
                        Emp_id = dd.Rows[0][0].ToString();
                    }

                    DataTable dd1 = GITAPI.dbFunctions.getTable("select isnull(max(convert(int,Emp_maxid)),0)+1 from employee_master");
                    if (dd1.Rows.Count > 0)
                    {
                        Emp_maxid = dd1.Rows[0][0].ToString();
                    }
                    com.CommandText = "insert into employee_master (Emp_id,Emp_maxid,Emp_code,Emp_name,Emp_address,Emp_phone,Emp_email,Emp_designation,Emp_department,Emp_Salary,Emp_commission,Emp_Status,Emp_date,Emp_CreatedBy,Emp_Company) Values " +
                        "(@Emp_id,@Emp_maxid,@Emp_code,@Emp_name,@Emp_address,@Emp_phone,@Emp_email,@Emp_designation,@Emp_department,@Emp_Salary,@Emp_commission,'A',GETDATE(),@Emp_CreatedBy,@Emp_Company)";
                }
                else
                {
                    com.CommandText = "update employee_master Set Emp_code=@Emp_code, Emp_name=@Emp_name, Emp_address=@Emp_address, Emp_phone=@Emp_phone,Emp_designation=@Emp_designation, Emp_department=@Emp_department,Emp_Salary=@Emp_Salary," +
                        "Emp_commission=@Emp_commission,Emp_DelBy=@Emp_CreatedBy,Emp_DelDate=getdate() where  Emp_id=@Emp_id ";

                }
                com.Parameters.Add("@Emp_id", SqlDbType.VarChar).Value = Emp_id;
                com.Parameters.Add("@Emp_maxid", SqlDbType.VarChar).Value = Emp_maxid;
                com.Parameters.Add("@Emp_code", SqlDbType.VarChar).Value = Emp_code;
                com.Parameters.Add("@Emp_name", SqlDbType.VarChar).Value = Emp_name;
                com.Parameters.Add("@Emp_address", SqlDbType.VarChar).Value = Emp_address;
                com.Parameters.Add("@Emp_phone", SqlDbType.VarChar).Value = Emp_phone;
                com.Parameters.Add("@Emp_email", SqlDbType.VarChar).Value = Emp_email;
                com.Parameters.Add("@Emp_designation", SqlDbType.VarChar).Value = Emp_designation;
                com.Parameters.Add("@Emp_department", SqlDbType.VarChar).Value = Emp_department;
                com.Parameters.Add("@Emp_Salary", SqlDbType.VarChar).Value = Emp_Salary;
                com.Parameters.Add("@Emp_commission", SqlDbType.VarChar).Value = Emp_commission;
                com.Parameters.Add("@Emp_CreatedBy", SqlDbType.VarChar).Value = Emp_CreatedBy;
                com.Parameters.Add("@Emp_Branch", SqlDbType.VarChar).Value = Emp_Company;
                com.Parameters.Add("@Emp_Company", SqlDbType.VarChar).Value = Emp_Company;
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
        public string get_contractor_master(string Status, string Company = "0")
        {

            string condi = "";

            if (Status.ToLower() == "undefined")
            {
                Status = "A";
            }

            if (Status.ToLower() != "all")
            {
                condi += " and  Con_Status='" + Status + "'";
            }
            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  Con_Company='" + Company.Replace("_", "") + "'";
            //}


            string Query = "select *,FORMAT(Con_date,'dd/MM/yyyy hh:mm tt') as CDate from Contractor_Master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string delete_contractor_master(string ID, string UserName, string Status, string Company)
        {
            GITAPI.dbFunctions.getTable("UPDATE Contractor_Master SET Con_Status=" + Status + ",Con_DelDate=GETDATE(),Con_DelBy=" + UserName + "  where Con_Id=" + ID);
            return "True";
        }


        [HttpPost]
        public string insert_contractor_master(JObject jsonData)
        {
            dynamic json = jsonData;
            string Con_Id = isnull(json.Con_Id, "");
            string Con_Code = isnull(json.Con_Code, "");
            string Con_Name = isnull(json.Con_Name, "");
            string Con_Address = isnull(json.Con_Address, "");
            string Con_Addr2 = isnull(json.Con_Addr2, "");
            string Con_ContactNo = isnull(json.Con_ContactNo, "");
            string Con_GSTIN = isnull(json.Con_GSTIN, "");
            string Con_State = isnull(json.Con_State, "");
            string Con_OB = isnull(json.Con_OB, "0");
            string Con_CreatedBy = isnull(json.Con_CreatedBy, "");
            string Company = isnull(json.Company, "");

            string Con_Company = Company.Replace("_", "");
            string old_Category = "";


            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                DataTable dds = GITAPI.dbFunctions.getTable("select * from ReferenceGroup_Value  where  RGV_iID=" + Con_State);
                if (dds.Rows.Count > 0)
                {
                    old_Category = dds.Rows[0]["RGV_vDesciption"].ToString();
                }
                if (Con_Id == "0")
                {
                    DataTable dd = GITAPI.dbFunctions.getTable("select  isnull(max(Con_Id),0)+1 from Contractor_Master");
                    if (dd.Rows.Count > 0)
                    {
                        Con_Id = dd.Rows[0][0].ToString();
                    }

                    com.CommandText = "insert into Contractor_Master (Con_Id,Con_Code,Con_Name,Con_Address,Con_Addr2,Con_ContactNo,Con_GSTIN,Con_State,Con_OB,Con_Status,Con_date,Con_CreatedBy,Con_Company,Con_StateId) Values " +
                        "(@Con_Id,@Con_Code,@Con_Name,@Con_Address,@Con_Addr2,@Con_ContactNo,@Con_GSTIN,@Con_State,@Con_OB,'A',GETDATE(),@Con_CreatedBy,@Con_Company,@Con_StateId)";
                }
                else
                {
                    com.CommandText = "update Contractor_Master Set Con_Code=@Con_Code, Con_Name=@Con_Name, Con_Address=@Con_Address, Con_Addr2=@Con_Addr2,Con_GSTIN=@Con_GSTIN, Con_State=@Con_State,Con_OB=@Con_OB," +
                        "Con_DelBy=@Con_CreatedBy,Con_DelDate=getdate(),Con_StateId=@Con_StateId where  Con_Id=@Con_Id ";

                }
                com.Parameters.Add("@Con_Id", SqlDbType.VarChar).Value = Con_Id;
                com.Parameters.Add("@Con_Code", SqlDbType.VarChar).Value = Con_Code;
                com.Parameters.Add("@Con_Name", SqlDbType.VarChar).Value = Con_Name;
                com.Parameters.Add("@Con_Address", SqlDbType.VarChar).Value = Con_Address;
                com.Parameters.Add("@Con_Addr2", SqlDbType.VarChar).Value = Con_Addr2;
                com.Parameters.Add("@Con_ContactNo", SqlDbType.VarChar).Value = Con_ContactNo;
                com.Parameters.Add("@Con_GSTIN", SqlDbType.VarChar).Value = Con_GSTIN;
                com.Parameters.Add("@Con_State", SqlDbType.VarChar).Value = old_Category;
                com.Parameters.Add("@Con_OB", SqlDbType.VarChar).Value = Con_OB;
                com.Parameters.Add("@Con_CreatedBy", SqlDbType.VarChar).Value = Con_CreatedBy;
                //com.Parameters.Add("@Con_Branch", SqlDbType.VarChar).Value = Con_Company;
                com.Parameters.Add("@Con_Company", SqlDbType.VarChar).Value = Con_Company;
                com.Parameters.Add("@Con_StateId", SqlDbType.VarChar).Value = Con_State;
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
        public string get_vehicle_master(string Status, string Order_by, string Company = "0")
        {

            string condi = "";

            if (Status.ToLower() == "undefined")
            {
                Status = "A";
            }

            if (Status.ToLower() != "all")
            {
                condi += " and  Veh_Status='" + Status + "'";
            }
            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  Veh_Company='" + Company.Replace("_", "") + "'";
            //}
            if (Order_by.ToLower() != "")
            {
                condi += " Order by " + Order_by;
            }


            string Query = "select *,dbo.Date_(Veh_Insurance) as Insurance,dbo.Date_(Veh_FCDate) as FCDate,FORMAT(Veh_Date,'dd/MM/yyyy hh:mm tt') as CDate,dbo.Date_(Veh_PermitDate) as PermitDate from Vehicle_Master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string delete_vehicle_master(string ID, string UserName, string Status, string Company)
        {
            GITAPI.dbFunctions.getTable("UPDATE Vehicle_Master SET Veh_Status='" + Status + "',Veh_DelDate='" + GITAPI.dbFunctions.getdate() + "',Veh_DelBy='" + UserName + "'  where Veh_ID=" + ID);
            return "True";
        }


        [HttpPost]
        public string insert_vehicle_master(JObject jsonData)
        {
            dynamic json = jsonData;
            string Veh_ID = isnull(json.Veh_ID, "");
            string Veh_Code = isnull(json.Veh_Code, "");
            string Veh_VehicleNo = isnull(json.Veh_VehicleNo, "");
            string Veh_Type = isnull(json.Veh_Type, "");
            string Veh_Make = isnull(json.Veh_Make, "");
            string Veh_Model = isnull(json.Veh_Model, "");
            string Veh_Insurance = isnull(json.Veh_Insurance, "");
            string Veh_FCDate = isnull(json.Veh_FCDate, "");
            string Veh_PermitDate = isnull(json.Veh_PermitDate, "");
            string Veh_CreatedBy = isnull(json.Veh_CreatedBy, "");
            string Company = isnull(json.Company, "");

            string Veh_Company = Company.Replace("_", "");
            string old_Category = "";


            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                DataTable dds = GITAPI.dbFunctions.getTable("select * from ReferenceGroup_Value  where  RGV_iID=" + Veh_Type);
                if (dds.Rows.Count > 0)
                {
                    old_Category = dds.Rows[0]["RGV_vDesciption"].ToString();
                }
                if (Veh_ID == "0")
                {
                    DataTable dd = GITAPI.dbFunctions.getTable("select  isnull(max(Veh_ID),0)+1 from Vehicle_Master");
                    if (dd.Rows.Count > 0)
                    {
                        Veh_ID = dd.Rows[0][0].ToString();
                    }

                    com.CommandText = "insert into Vehicle_Master (Veh_ID,Veh_Code,Veh_VehicleNo,Veh_Type,Veh_Make,Veh_Model,Veh_Insurance,Veh_FCDate,Veh_PermitDate,Veh_Status,Veh_date,Veh_CreatedBy,Veh_Company,Veh_TypeId) Values " +
                        "(@Veh_ID,@Veh_Code,@Veh_VehicleNo,@Veh_Type,@Veh_Make,@Veh_Model,@Veh_Insurance,@Veh_FCDate,@Veh_PermitDate,'A',GETDATE(),@Veh_CreatedBy,@Veh_Company,@Veh_TypeId)";
                }
                else
                {
                    com.CommandText = "update Vehicle_Master Set Veh_Code=@Veh_Code, Veh_VehicleNo=@Veh_VehicleNo, Veh_Type=@Veh_Type, Veh_Make=@Veh_Make,Veh_Insurance=@Veh_Insurance, Veh_FCDate=@Veh_FCDate,Veh_PermitDate=@Veh_PermitDate," +
                        "Veh_DelBy=@Veh_CreatedBy,Veh_DelDate=getdate(),Veh_TypeId=@Veh_TypeId where  Veh_ID=@Veh_ID ";

                }
                com.Parameters.Add("@Veh_ID", SqlDbType.VarChar).Value = Veh_ID;
                com.Parameters.Add("@Veh_Code", SqlDbType.VarChar).Value = Veh_Code;
                com.Parameters.Add("@Veh_VehicleNo", SqlDbType.VarChar).Value = Veh_VehicleNo;
                com.Parameters.Add("@Veh_Type", SqlDbType.VarChar).Value = old_Category;
                com.Parameters.Add("@Veh_Make", SqlDbType.VarChar).Value = Veh_Make;
                com.Parameters.Add("@Veh_Model", SqlDbType.VarChar).Value = Veh_Model;
                com.Parameters.Add("@Veh_Insurance", SqlDbType.VarChar).Value = Veh_Insurance;
                com.Parameters.Add("@Veh_FCDate", SqlDbType.VarChar).Value = Veh_FCDate;
                com.Parameters.Add("@Veh_PermitDate", SqlDbType.VarChar).Value = Veh_PermitDate;
                com.Parameters.Add("@Veh_CreatedBy", SqlDbType.VarChar).Value = Veh_CreatedBy;
                //com.Parameters.Add("@Veh_Branch", SqlDbType.VarChar).Value = Veh_Company;
                com.Parameters.Add("@Veh_Company", SqlDbType.VarChar).Value = Veh_Company;
                com.Parameters.Add("@Veh_TypeId", SqlDbType.VarChar).Value = Veh_Type;
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
        public string get_Product(string Status, string vType, string Company = "0")
        {

            string condi = "";

            if (Status.ToLower() == "undefined")
            {
                Status = "A";
            }
            if (Status.ToLower() != "all")
            {
                condi += " and  pm_status='" + Status + "'";
            }
            if (vType.ToLower() != "all")
            {
                condi += " and  pm_type='" + vType + "'";
            }

            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  pm_company='" + Company.Replace("_", "") + "'";
            //}


            string Query = "select PM_id as value,pm_item_name as label,* from Product_Master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

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
                        if (col.ToLower().Equals(ColumnPerfix + "created_date") ||
                            //col.ToLower().Equals(ColumnPerfix + "created_by") ||
                            //col.ToLower().Equals(ColumnPerfix + "created_by") ||
                            col.ToLower().Equals(ColumnPerfix + "created_by"))
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
                    else if (Column.ToLower().Equals(ColumnPerfix + "branch"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Company.Replace("_", "");
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



        [HttpGet]
        public string get_Branch_Code(string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Serial_No_Settings where [Name]='Branch' ";

            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(bm_code,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Branch_Master ";
            Query += "where left(bm_code,len(@Prefix))=@Prefix ";
            Query += "and right(bm_code,len(@suffix))=@suffix ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }

        [HttpGet]
        public string get_Manager_Name(string Status, string Order_by, string Company = "0")
        {

            string condi = "";

            if (Status.ToLower() == "undefined")
            {
                Status = "A";
            }
            if (Status.ToLower() != "all")
            {
                condi += " and  Branch_status='" + Status + "'";
            }
            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  pm_company='" + Company.Replace("_", "") + "'";
            //}

            if (Order_by.ToLower() != "")
            {
                condi += " Order by " + Order_by;
            }

            string Query = "select * from Branch_Master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string Delete_Branch_Details(string ID, string UserName, string Status, string Company)
        {
            string q = "UPDATE Branch_Master SET Branch_status='" + Status + "',updated_at='" + GITAPI.dbFunctions.getdate() + "',Created_by='" + UserName + "'  where Branch_id=" + ID;
            GITAPI.dbFunctions.getTable(q);
            return "True";
        }


        [HttpGet]
        public string get_BOM_Master(string Status, string ID = "0", string Company = "0")
        {

            string condi = "";

            if (Status.ToLower() == "undefined")
            {
                Status = "A";
            }

            if (Status.ToLower() != "all")
            {
                condi += " and  Bom_Status='" + Status + "'";
            }
            if (ID.ToLower() != "0")
            {
                condi += " and  Bom_ProdId='" + ID + "'";
            }
            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  Bom_Company='" + Company.Replace("_", "") + "'";
            //}


            string Query = "select *,FORMAT(Bom_Date,'dd/MM/yyyy hh:mm tt') as CDate,(select pm_item_name from Product_Master  where  PM_id=Bom_RMId) as RawMaterial from BOM_Master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string delete_bom_master(string ID, string UserName, string Status, string Company)
        {
            GITAPI.dbFunctions.getTable("UPDATE BOM_Master SET Bom_Status='" + Status + "',Bom_DelDate=GETDATE(),Bom_DelBy='" + UserName + "'  where Bom_ID=" + ID);
            return "True";
        }


        [HttpPost]
        public string insert_bom_master(JObject jsonData)
        {
            dynamic json = jsonData;
            string Bom_ID = isnull(json.Bom_ID, "");
            string Bom_ProdId = isnull(json.Bom_ProdId, "");
            string Bom_Product = isnull(json.Bom_Product, "");
            string Bom_RMId = isnull(json.Bom_RMId, "");
            string Bom_RawMaterial = isnull(json.Bom_RawMaterial, "");
            string Bom_Qty = isnull(json.Bom_Qty, "");
            string Bom_CreatedBy = isnull(json.CreatedBy, "");
            string Company = isnull(json.Company, "");

            string Bom_Company = Company.Replace("_", "");


            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                DataTable dds = GITAPI.dbFunctions.getTable("select * from Product_Master  where  PM_id=" + Bom_ProdId);
                if (dds.Rows.Count > 0)
                {
                    Bom_Product = dds.Rows[0]["pm_item_name"].ToString();
                }
                DataTable dds1 = GITAPI.dbFunctions.getTable("select * from Product_Master  where  PM_id=" + Bom_RMId);
                if (dds1.Rows.Count > 0)
                {
                    Bom_RawMaterial = dds1.Rows[0]["pm_item_name"].ToString();
                }
                if (Bom_ID == "0")
                {
                    DataTable dd = GITAPI.dbFunctions.getTable("select  isnull(max(Bom_ID),0)+1 from BOM_Master");
                    if (dd.Rows.Count > 0)
                    {
                        Bom_ID = dd.Rows[0][0].ToString();
                    }

                    com.CommandText = "insert into BOM_Master (Bom_ID,Bom_ProdId,Bom_Product,Bom_RMId,Bom_RawMaterial,Bom_Qty,Bom_Status,Bom_date,Bom_CreatedBy,Bom_Company) Values " +
                        "(@Bom_ID,@Bom_ProdId,@Bom_Product,@Bom_RMId,@Bom_RawMaterial,@Bom_Qty,'A',GETDATE(),@Bom_CreatedBy,@Bom_Company)";
                }
                else
                {
                    com.CommandText = "update BOM_Master Set Bom_ProdId=@Bom_ProdId, Bom_Product=@Bom_Product, Bom_RMId=@Bom_RMId, Bom_RawMaterial=@Bom_RawMaterial," +
                        "Bom_DelBy=@Bom_CreatedBy,Bom_DelDate=getdate() where  Bom_ID=@Bom_ID ";

                }
                com.Parameters.Add("@Bom_ID", SqlDbType.VarChar).Value = Bom_ID;
                com.Parameters.Add("@Bom_ProdId", SqlDbType.VarChar).Value = Bom_ProdId;
                com.Parameters.Add("@Bom_Product", SqlDbType.VarChar).Value = Bom_Product;
                com.Parameters.Add("@Bom_RMId", SqlDbType.VarChar).Value = Bom_RMId;
                com.Parameters.Add("@Bom_RawMaterial", SqlDbType.VarChar).Value = Bom_RawMaterial;
                com.Parameters.Add("@Bom_Qty", SqlDbType.VarChar).Value = Bom_Qty;
                com.Parameters.Add("@Bom_CreatedBy", SqlDbType.VarChar).Value = Bom_CreatedBy;
                com.Parameters.Add("@Bom_Branch", SqlDbType.VarChar).Value = Bom_Company;
                com.Parameters.Add("@Bom_Company", SqlDbType.VarChar).Value = Bom_Company;
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
        public string get_Bank_Master(string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select *,Bank_Name+'-'+Account_Number as label,ID as [value] from Bank_Master");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string delete_Bank_Master(string ID, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("delete from Bank_Master where ID=" + ID);
            return "True";
        }


        [HttpPost]
        public string insert_Bank_Master(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string Bank_Name = isnull(json.Bank_Name, "");
            string Account_Number = isnull(json.Account_Number, "");
            string Account_Name = isnull(json.Account_Name, "");
            string Branch = isnull(json.Branch, "");
            string IFSC_Code = isnull(json.IFSC_Code, "");
            string Location = isnull(json.Location, "");
            string Created_by = isnull(json.Created_by, "");
            string Created_date = isnull(json.Created_date, "");
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
                    DataTable dd = GITAPI.dbFunctions.getTable("select  isnull(max(ID),0)+1 from Bank_Master");
                    if (dd.Rows.Count > 0)
                    {
                        ID = dd.Rows[0][0].ToString();
                    }
                    com.CommandText = "insert into Bank_Master (ID,Bank_Name,Account_Number,Account_Name,Branch,IFSC_Code,Location,Created_by,Created_date,Status) Values (@ID,@Bank_Name,@Account_Number,@Account_Name,@Branch,@IFSC_Code,@Location,@Created_by,getdate(), 'A'  )";
                }
                else
                {
                    com.CommandText = "update Bank_Master Set Bank_Name=@Bank_Name, Account_Number=@Account_Number, Account_Name=@Account_Name, Branch=@Branch, IFSC_Code=@IFSC_Code, Location=@Location, Created_by=@Created_by where  ID=@ID ";

                }
                com.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
                com.Parameters.Add("@Bank_Name", SqlDbType.VarChar).Value = Bank_Name;
                com.Parameters.Add("@Account_Number", SqlDbType.VarChar).Value = Account_Number;
                com.Parameters.Add("@Account_Name", SqlDbType.VarChar).Value = Account_Name;
                com.Parameters.Add("@Branch", SqlDbType.VarChar).Value = Branch;
                com.Parameters.Add("@IFSC_Code", SqlDbType.VarChar).Value = IFSC_Code;
                com.Parameters.Add("@Location", SqlDbType.VarChar).Value = Location;
                com.Parameters.Add("@Created_by", SqlDbType.VarChar).Value = Created_by;
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
        public string get_Transport_Rating_Master(string Status, string Order_by, string Company)
        {
            string condi = "";
            if (Status.ToLower().Equals("undefined"))
            {
                Status = "A";
            }
            if (Status.ToLower().Equals("a"))
            {
                condi += " and trm_status='" + Status + "' ";

            }
            //if(!Company.Equals("0"))
            //{
            //    condi += " and trm_company='" + Company.Replace("_", "") + "' ";
            //}
            if (!Order_by.Equals("0"))
            {
                condi += " Order by " + Order_by;
            }
            string Query = "select * from Transport_Rating_Master Where 0=0 " + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string delete_Transport_Rating_Master(string ID, string UserName, string Status, string Company)
        {
            GITAPI.dbFunctions.getTable("UPDATE Transport_Rating_Master SET trm_status='" + Status + "',trm_del_date='" + GITAPI.dbFunctions.getdate() + "',trm_del_by='" + UserName + "'  where trm_id=" + ID);
            return "True";
        }

        public string Post_Transport_Rating_Master(JObject jsonData)
        {
            dynamic json = jsonData;
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");
            string ID = isnull(json.trm_id, "0");
            string User = isnull(json.Created_by, "");
            string from1 = isnull(json.trm_from, "");
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

                    DataTable dd1 = GITAPI.dbFunctions.getTable("select isnull(max(trm_fid),(select isnull(max(trm_fid),0)+1 from Transport_Rating_Master)) from " + Table_Name + " where trm_from='" + from1 + "'");
                    if (dd1.Rows.Count > 0)
                    {
                        json["trm_fid"] = dd1.Rows[0][0].ToString();
                        //ID = dd1.Rows[0][0].ToString();
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
                        if (col.ToLower().Equals("trm_created_date"))
                        { }
                        else if (col.ToLower().Equals("trm_created_by"))
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
                    if (Column.ToLower().Equals("trm_id"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = ID;
                    }
                    else if (Column.ToLower().Equals("trm_created_date") && auto == 0)
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                    }
                    else if (Column.ToLower().Equals("trm_status"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = "A";
                    }

                    else if (Column.ToLower().Equals("trm_created_by") && auto == 0)
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
                    }
                    else if (Column.ToLower().Equals("trm_company"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Company.Replace("_", "");
                    }
                    else if (Column.ToLower().Equals("trm_del_by") && auto == 1)
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
                    }
                    else if (Column.ToLower().Equals("trm_del_date") && auto == 1)
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

        [HttpPost]
        public string JsontToExcel(JObject jsonData)
        {
            dynamic json = jsonData;
            string User = GITAPI.dbFunctions.isnull(json.User, "");
            string Company = GITAPI.dbFunctions.isnull(json.Company, "");

            DataTable dd = GITAPI.dbFunctions.getTable("select 1 from Excel_Data" + Company + " where user_ID='" + User + "'");
            if (dd.Rows.Count <= 0)
            {
                DataTable ds = GITAPI.dbFunctions.getTable("insert into Excel_Data" + Company + " (User_ID,Data) Values ('" + User + "','')");
            }

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = "update Excel_Data" + Company + "  set Data=@Data where User_ID=@User_ID";
                com.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = User;
                com.Parameters.Add("@Data", SqlDbType.NVarChar).Value = jsonData.ToString();
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
        public string get_vehicle(string Type = "all", string Company = "0")
        {

            string condi = "";


            if (Type.ToLower() != "all")
            {
                if (Type.ToLower().Equals("tipper/tractor"))
                {
                    condi += " and  (Veh_Type='Tipper' or Veh_Type='Tractor') ";
                }
                else if (Type.ToLower().Equals("jcb/hitachi"))
                {
                    condi += " and  (Veh_Type='JCB' or Veh_Type='Hitachi') ";
                }
                else
                {
                    condi += " and  Veh_Type='" + Type + "'";
                }
            }
            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  Veh_Company='" + Company.Replace("_", "") + "'";
            //}
            condi += " and  Veh_Status='A' ";

            string Query = "select Veh_VehicleNo as label,Veh_ID as value,*,dbo.Date_(Veh_Insurance) as Insurance,dbo.Date_(Veh_FCDate) as FCDate,FORMAT(Veh_Date,'dd/MM/yyyy hh:mm tt') as CDate,dbo.Date_(Veh_PermitDate) as PermitDate from Vehicle_Master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }



        [HttpGet]
        public string get_From_TRM(string Company = "0")
        {

            string condi = "";


            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  trm_company='" + Company.Replace("_", "") + "'";
            //}

            //condi += " and  trm_status='A' ";
            //condi += " order by trm_from asc ";

            string Query = "select distinct tpt_from as label,tpt_from as value from Transport_Entry where tpt_narration1='tractor' " + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_To_TRM(string Company = "0")
        {

            string condi = "";


            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  trm_company='" + Company.Replace("_", "") + "'";
            //}

            //condi += " and  trm_status='A' ";
            //condi += " order by trm_to asc ";

            string Query = "select distinct tpt_to as label,tpt_to as value from Transport_Entry where tpt_narration1='tractor' " + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }





        [HttpGet]
        public string get_From_Area(string Company = "0")
        {

            string condi = "";


            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  trm_company='" + Company.Replace("_", "") + "'";
            //}

            condi += " and  trm_status='A' ";
            condi += " order by trm_from asc ";

            string Query = "select distinct trm_from as label,trm_fid as value from Transport_Rating_Master where 0=0 " + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_To_Area(string Company = "0")
        {

            string condi = "";


            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  trm_company='" + Company.Replace("_", "") + "'";
            //}

            condi += " and  trm_status='A' ";
            condi += " order by trm_to asc ";

            string Query = "select trm_to as label,trm_id as value,trm_from as from1,* from Transport_Rating_Master where 0=0 " + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Vehicle_Basic_Rate(string Company = "0")
        {

            string condi = "";


            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  br_company='" + Company.Replace("_", "") + "'";
            //}

            condi += " and  br_status='A' ";

            string Query = "select * from Vehicle_Basic_Rate where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_FromMaster(string Company = "0")
        {
            string condi = "";

            string query = " select cus_id as value,cus_name as label from Ledger_Master where cus_status='A' " +
                "";
            //" union all " +
            //" select RGV_iID as value,RGV_vDesciption as label from ReferenceGroup_Value where RGV_iRG_ID=9 and RGV_vStatus='A' ";
            DataTable dt = GITAPI.dbFunctions.getTable(query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Project(string Company = "0")
        {

            string condi = "";


            //if (Company.ToLower() != "0")
            //{
            //    condi += " and  est_company='" + Company.Replace("_", "") + "'";
            //}

            condi += " and  est_status='A' ";
            condi += " order by est_id asc ";

            string Query = "select est_projectname as label,est_id as value,* from estimation where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_ledger_Master2(string Company = "0")
        {
            string condi = "";

            string q1 = " select cus_id as value,cus_name as label from Ledger_Master where cus_group_id=4 and cus_status='A' union all ";
            //" select Emp_id as value,Emp_name as label from Employee_Master where Emp_status='A' union all ";
            q1 += " select con_id as value,Con_Name as label from Contractor_Master where Con_Status='A' union all ";
            q1 += " select Veh_ID as value,Veh_VehicleNo as label from Vehicle_Master where Veh_Status='A' ";

            string q = "select * from(" + q1 + ") as x order by label " + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpGet]
        public string get_ledger_Master1(string Company = "0")
        {
            string condi = "";

            condi += " and cus_status='A' ";

            condi += " Order by cus_name";
            string q = "select cus_name as label,cus_id as value,* from Ledger_Master where 0=0 " + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpGet]
        public string get_ledger_Master(string Status, string Type, string Order_by, string Company = "0")
        {
            string condi = "";
            if (!Status.ToUpper().Equals(""))
            {
                condi += " and cus_status='" + Status + "' ";

            }
            if (!Type.ToUpper().Equals(""))
            {
                condi += " and cus_type='" + Type + "' ";

            }
            //if (!Company.Equals("0"))
            //{
            //    condi += " and cus_branch='" + Company.Replace("_", "") + "' ";
            //}
            if (!Order_by.Equals(""))
            {
                condi += " Order by " + Order_by;
            }
            string q = "select cus_name as label,cus_id as value,* from Ledger_Master where 0=0 " + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }


        public string Post_Master_Data(JObject jsonData)
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
                        if (col.ToLower().Equals(ColumnPerfix + "created_date") ||
                            //col.ToLower().Equals(ColumnPerfix + "created_by") ||
                            //col.ToLower().Equals(ColumnPerfix + "created_by") ||
                            col.ToLower().Equals(ColumnPerfix + "created_by"))
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
                    else if (Column.ToLower().Equals(ColumnPerfix + "branch"))
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

        [HttpGet]
        public string delete_Tyre_Master(string ID, string UserName, string Status, string Company)
        {
            string q = "UPDATE Tyre_Master SET ty_status='" + Status + "',ty_del_date='" + GITAPI.dbFunctions.getdate() + "',ty_del_by='" + UserName + "'  where ty_id=" + ID;
            GITAPI.dbFunctions.getTable(q);
            return "True";
        }


        [HttpGet]
        public string get_Tyre_Master(string Status, string Order_by, string Company = "0")
        {
            string condi = "";
            if (!Status.ToUpper().Equals(""))
            {
                condi += " and ty_status='" + Status + "' ";

            }
            if (!Order_by.Equals(""))
            {
                condi += " Order by " + Order_by;
            }
            string q = "select ty_no as label,ty_id as value,* from Tyre_Master where 0=0 " + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }
        [HttpGet]
        public string delete_Menu_Master(string ID)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("delete from Menu_Master where ID=" + ID);
            return "True";
        }


        [HttpPost]
        public string Post_Tyre_Master(JObject jsonData)
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
                        if (col.ToLower().Equals(ColumnPerfix + "created_date") ||
                            col.ToLower().Equals(ColumnPerfix + "created_by"))
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
                    else if (Column.ToLower().Equals(ColumnPerfix + "branch"))
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
    }
}
