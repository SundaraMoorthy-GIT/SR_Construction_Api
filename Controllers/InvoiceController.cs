using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Genuine_API.Controllers
{
    public class InvoiceController : ApiController
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
        public string get_Sales_No(string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Sales' ";
            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(sal_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Sales ";
            Query += "where left(sal_no,len(@Prefix))=@Prefix ";
            Query += "and right(sal_no,len(@suffix))=@suffix ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }

        [HttpGet]
        public string get_Bill_No(string Bill_Type, string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='" + Bill_Type + "' ";
            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(sal_bill_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Sales ";
            Query += "where left(sal_bill_no,len(@Prefix))=@Prefix ";
            Query += "and right(sal_bill_no,len(@suffix))=@suffix ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }


        [HttpGet]
        public string get_Sale_Detail(string From, string To, string User, string Area, string order_by, string Bill_Type, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,x.sal_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.sal_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (User.ToLower() != "all")
            {
                condi += " and  x.sal_created_by='" + User + "'";
            }

            if (Bill_Type.ToLower() != "all")
            {
                condi += " and  x.sal_bill_type='" + Bill_Type + "'";
            }

            if (Area.ToLower() != "all")
            {
                condi += " and  x.sal_area='" + Area + "'";
            }


            string s = " select dbo.Date_(sal_bill_date) as Bill_Date,dbo.Date_(sal_po_date) as sal_po_date,x.*,cus_name as Ledger_Name," +
                " dbo.Date_(sal_bill_date) as Bill_Date_ from Sales x " +
                " left outer join Ledger_Master l on l.cus_id=x.sal_ledger_id where 0=0 " + condi + " order by x." + order_by;
            DataTable dt = GITAPI.dbFunctions.getTable(s);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }


        [HttpGet]
        public string get_Sales_Entry_details(string Bill_No, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select dbo.Date_(sal_date) as sal_date,dbo.Date_(sal_po_date) as sal_po_date,*, dbo.date_(sal_Bill_Date) as Date from sales_details where sal_Bill_No='" + Bill_No + "'  order by  sal_id ");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string delete_Sales(string Bill_No, string UserName, string Company)
        {
            string q = "insert into Sales_Delete select '" + UserName + "','" + GITAPI.dbFunctions.getdate() + "',* from Sales where sal_no='" + Bill_No + "'";
            string q1 = "insert into Sales_Details_Delete select '" + UserName + "','" + GITAPI.dbFunctions.getdate() + "',* from Sales_details where sal_no='" + Bill_No + "'";
            //DataTable dd5 = GITAPI.dbFunctions.getTable("delete from Stock_Details  where  sd_vour_type='" + Acc_Data.G_Type_Sales + "' and sd_voucher_no='" + Bill_No + "'");
            //DataTable dt4c = GITAPI.dbFunctions.getTable("delete from Balance  where  cb_billno='" + Bill_No + "' and cb_vtype='Sales'");
            DataTable dd2 = GITAPI.dbFunctions.getTable(q + " delete  from Sales where sal_no='" + Bill_No + "'");
            DataTable dd3 = GITAPI.dbFunctions.getTable(q1 + " delete  from Sales_details where sal_no='" + Bill_No + "'");

            return "True";
        }

        [HttpPost]
        public string Post_Sales(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "0");
            string Company = isnull(json.Company, "");
            string CREATED_BY = isnull(json.Created_by, "");
            string Sal_No = isnull(json.sal_no, "");
            string Bill_No = isnull(json.sal_bill_no, "");
            string Order_No = isnull(json.Order_No, "");
            string Ledger_ID = isnull(json.sal_ledger_id, "");
            string Bill_Type = isnull(json.sal_bill_type, "Tax Invoice");
            string Type_Based_Bill_No = isnull(json.Type_Based_Bill_No, "false");
            string Ledger_Update = isnull(json.Ledger_Update, "false");
            string Receipt_Print = isnull(json.Receipt_Print, "false");
            string Bill_Mode = isnull(json.sal_bill_mode, "Credit");
            string Balance_SMS = isnull(json.Balance_SMS, "false");
            string R_Type = isnull(json.R_Type, "Apply");
            string Return_Amount = isnull(json.Return_Amount, "0");
            string Receipt_Company = isnull(json.Receipt_Company, Company);
            string Receipt_Titel = isnull(json.Receipt_Company, "");
            string ColumnPerfix = isnull(json.ColumnPerfix, "");

            Newtonsoft.Json.Linq.JArray items = json.items;
            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Sales'");
            string Query = "";

            string Bill_Date = "";

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {
                    Bill_Date = isnull(json.sal_bill_date, "");
                    //Bill_No = get_Bill_No(Bill_Type, Company);
                    Query += "insert into sales (";
                    for (int i = 1; i < dt.Rows.Count - 1; i++)
                    {
                        Query += dt.Rows[i]["Column_Name"].ToString() + ",";
                    }
                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";
                    Query += ") Values ( ";
                    for (int i = 1; i < dt.Rows.Count - 1; i++)
                    {
                        Query += "@" + dt.Rows[i]["Column_Name"].ToString() + ",";
                    }
                    Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";
                    Query += " )";

                    com.CommandText = Query;

                }
                else
                {
                    Bill_Date = isnull(json.sal_bill_date, "");
                    Bill_No = isnull(json.sal_bill_no, "");
                    Query = "";
                    Query += "update  Sales Set ";
                    for (int i = 1; i < dt.Rows.Count - 1; i++)
                    {
                        Query += dt.Rows[i]["Column_Name"].ToString() + "=@" + dt.Rows[i]["Column_Name"].ToString() + ", ";
                    }
                    Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "=@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + " ";
                    Query += "where  " + dt.Rows[0]["Column_Name"].ToString() + "=@" + dt.Rows[0]["Column_Name"].ToString() + " ";
                    com.CommandText = Query;
                    com.Parameters.Add("@" + dt.Rows[0]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[0]["Column_Name"].ToString()], "0");
                }

                Query = "";
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    string DF_value = "";
                    if (dt.Rows[i]["Data_Type"].ToString().ToUpper().Equals("VARCHAR") || dt.Rows[i]["Data_Type"].ToString().ToUpper().Equals("NVARCHAR") || dt.Rows[i]["Data_Type"].ToString().ToUpper().Equals("CHAR") || dt.Rows[i]["Data_Type"].ToString().ToUpper().Equals("NCHAR"))
                    {
                        DF_value = "";
                    }
                    else if (dt.Rows[i]["Data_Type"].ToString().ToUpper().Equals("DECIMAL") || dt.Rows[i]["Data_Type"].ToString().ToUpper().Equals("FLOAT") || dt.Rows[i]["Data_Type"].ToString().ToUpper().Equals("NUMERIC"))
                    {
                        DF_value = "0";
                    }
                    else
                        DF_value = "";

                    string Column = dt.Rows[i]["Column_Name"].ToString().ToLower();

                    if (Column.Equals(ColumnPerfix + "created_date"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                    }
                    else if (Column.Equals(ColumnPerfix + "created_by"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = CREATED_BY;
                    }
                    else if (Column.Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                    }
                    else if (Column.Equals("bill_no"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Bill_No;
                    }
                    else if (Column.Equals(ColumnPerfix + "bill_date"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Bill_Date;
                    }
                    else
                    {
                        if (dt.Rows[i]["Data_Type"].ToString().ToUpper().Equals("NVARCHAR"))
                        {
                            com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.NVarChar).Value = isnull(json[dt.Rows[i]["Column_Name"].ToString()], DF_value);
                        }
                        else
                        {
                            com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[i]["Column_Name"].ToString()], DF_value);
                        }
                    }


                }

                com.ExecuteNonQuery();
                com.Connection.Close();


                //if (Ledger_Update.ToLower().Equals("true"))
                //{
                //    DataTable d1t = GITAPI.dbFunctions.getTable("update Ledger_master" + Company + " set Address1='" + json.Customer_Address1 + "',Address2='" + json.Customer_Address2 + "',Address3='" + json.Customer_Address3 + "', Area='" + json.Area + "',Phone_Number='" + json.Contact_No + "',GSTIN='" + json.GST_No + "' where ID=" + json.Ledger_ID);
                //}

                string update_query = " delete from Sales_Details where sal_bill_no='" + Bill_No + "'";

                DataTable dds = GITAPI.dbFunctions.getTable(update_query);


                dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Sales_details'");
                bool Flags = true;
                for (int i = 0; i < items.Count; i++)
                {

                    items[i]["MRP_Disc_Per"] = "0";
                    items[i]["Profit"] = "0";

                    //DataTable ps = GITAPI.dbFunctions.getTable("select 1 from Item_Master" + Company + " where Item_Name='" + items[i]["Item_Name"] + "'");
                    //if (ps.Rows.Count <= 0)
                    //{
                    //    string iD_ = "0";
                    //    ps = GITAPI.dbFunctions.getTable("select isnull(Max(ID),0)+1 from Item_master" + Company);
                    //    if (ps.Rows.Count > 0)
                    //    {
                    //        iD_ = ps.Rows[0][0].ToString();
                    //        items[i]["Item_ID"] = iD_;
                    //    }

                    //    string q = "insert into  Item_master" + Company + " (Item_Code,Item_Name,Rate,GST_Per) values (" + iD_ + ",'" + items[i]["Item_Name"] + "','" + items[i]["Unit_Price"] + "','" + items[i]["GST_Per"] + "')";
                    //    ps = GITAPI.dbFunctions.getTable(q);

                    //}



                    var item = (JObject)items[i];
                    Query = "";
                    con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
                    try
                    {
                        con.Open();
                        com = new SqlCommand();
                        com.Connection = con;
                        com.CommandType = CommandType.Text;

                        Query += "insert into sales_details (";

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

                            if (Column.Equals(ColumnPerfix + "created_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                            }
                            else if (Column.Equals(ColumnPerfix + "created_by"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = CREATED_BY;
                            }
                            else if (Column.Equals(ColumnPerfix + "status"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                            }
                            else if (Column.Equals(ColumnPerfix + "bill_no"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Bill_No;
                            }
                            else if (Column.Equals(ColumnPerfix + "date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Bill_Date;
                            }
                            else if (Column.Equals(ColumnPerfix + "no"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Sal_No;
                            }
                            else if (Column.Equals(ColumnPerfix + "bill_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Bill_Date;
                            }

                            else
                            {
                                string Data = DF_value;
                                try
                                {
                                    Data = items[i][dt.Rows[k]["Column_Name"].ToString()].ToString();
                                }
                                catch
                                {
                                    try
                                    {
                                        Data = isnull(json[dt.Rows[k]["Column_Name"].ToString()], DF_value);
                                    }
                                    catch
                                    {
                                        Data = DF_value;
                                    }
                                }

                                if (Dat_Type.ToString().ToUpper().Equals("NVARCHAR"))
                                {
                                    com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.NVarChar).Value = Data;
                                }
                                else
                                {
                                    com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Data;
                                }

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

                if (Flags)
                {
                    DataTable dc1 = GITAPI.dbFunctions.getTable("select sal_id from sales where sal_no='" + Sal_No + "'");
                    if (dc1.Rows.Count > 0)
                    {
                        ID = dc1.Rows[0][0].ToString();
                    }



                    //dt = GITAPI.dbFunctions.getTable("delete from Stock_Details where sd_voucher_no='" + Sal_No + "' and sd_vour_type='" + GITAPI.Acc_Data.G_Type_Sales + "'");


                    //// Item Wise Qty Update
                    //Query = "insert into Stock_Details" +
                    //      " (sd_uni_code,sd_vour_type,sd_vour_refno,sd_order_no,sd_voucher_no,sd_voucher_date,sd_prod_id,sd_inward_qty,sd_outward_qty,sd_rate,sd_amount,sd_credit_amt,sd_debit_amt,sd_created_by,sd_created_date,sd_status,sd_barcode)" +
                    //             " select sal_uni_code," +
                    //             "'" + GITAPI.Acc_Data.G_Type_Sales + "'," +
                    //             "sal_id," +
                    //             "2," +
                    //             " sal_no, " +
                    //             "sal_date," +
                    //             "[sal_prod_id]," +
                    //             "0 AS INWARD_QTY, " +
                    //             "sal_qty+sal_free  as Out_Qty, " +
                    //             "[sal_rate] AS RATE," +
                    //             "[sal_rate]*sal_qty AS AMOUNT, " +
                    //             "[sal_rate]*sal_qty  AS CRAMOUNT," +
                    //             "0 AS DEBIT_AMT," +
                    //             "sal_created_by,   " +
                    //             "sal_created_date, " +
                    //             "sal_status ,sal_prod_code  " +
                    //             " from  " +
                    //             " sales_details" +
                    //             " where sal_no= '" + Sal_No + "'";

                    //DataTable dd = GITAPI.dbFunctions.getTable(Query);

                    Acc_Data.Accounts_Update(Acc_Data.G_Type_Sales, ID, Company, "Sales");

                }

                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


    }
}
