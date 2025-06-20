using Genuine_API;
using Genuine_API.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class TransactionController : ApiController
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
        public string get_Purchase_No(string Type, string Company)
        {
            try
            {
                string Query = "";
                Query += "declare @digit int ";
                Query += "declare @Prefix varchar(33) ";
                Query += "declare @suffix varchar(33) ";
                Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='" + Type + "' ";

                Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(pur_purchase_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Purchase ";
                Query += "where left(pur_purchase_no,len(@Prefix))=@Prefix ";
                Query += "and right(pur_purchase_no,len(@suffix))=@suffix ";

                DataTable dt = GITAPI.dbFunctions.getTable(Query);

                return dt.Rows[0][0].ToString();
            }
            catch { return ""; }
        }


        [HttpGet]
        public string get_Estimation_No(string Company)
        {
            try
            {
                string Query = "";
                Query += "declare @digit int ";
                Query += "declare @Prefix varchar(33) ";
                Query += "declare @suffix varchar(33) ";
                Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Estimation' ";

                Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(est_unino,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Estimation ";
                Query += "where left(est_unino,len(@Prefix))=@Prefix ";
                Query += "and right(est_unino,len(@suffix))=@suffix ";

                DataTable dt = GITAPI.dbFunctions.getTable(Query);

                return dt.Rows[0][0].ToString();
            }
            catch { return ""; }
        }


        [HttpGet]
        public string get_Material_Movement_No(string Company)
        {
            try
            {
                string Query = "";
                Query += "declare @digit int ";
                Query += "declare @Prefix varchar(33) ";
                Query += "declare @suffix varchar(33) ";
                Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Material_Movement' ";

                Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(mm_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Material_Movement ";
                Query += "where left(mm_no,len(@Prefix))=@Prefix ";
                Query += "and right(mm_no,len(@suffix))=@suffix ";

                DataTable dt = GITAPI.dbFunctions.getTable(Query);

                return dt.Rows[0][0].ToString();
            }
            catch { return ""; }
        }

        [HttpGet]
        public string get_othercol_No(string Company)
        {
            try
            {
                string Query = "";
                Query += "declare @digit int ";
                Query += "declare @Prefix varchar(33) ";
                Query += "declare @suffix varchar(33) ";
                Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Machiner' ";

                Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(oc_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Other_Collection ";
                Query += "where left(oc_no,len(@Prefix))=@Prefix ";
                Query += "and right(oc_no,len(@suffix))=@suffix ";

                DataTable dt = GITAPI.dbFunctions.getTable(Query);

                return dt.Rows[0][0].ToString();
            }
            catch { return ""; }
        }

        [HttpGet]
        public string get_Contractor_No(string Company)
        {
            try
            {
                string Query = "";
                Query += "declare @digit int ";
                Query += "declare @Prefix varchar(33) ";
                Query += "declare @suffix varchar(33) ";
                Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Contractor' ";

                Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(oc_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Other_Collection ";
                Query += "where left(oc_no,len(@Prefix))=@Prefix ";
                Query += "and right(oc_no,len(@suffix))=@suffix ";

                DataTable dt = GITAPI.dbFunctions.getTable(Query);

                return dt.Rows[0][0].ToString();
            }
            catch { return ""; }
        }


        [HttpGet]
        public string get_OtherCollection_No(string Type, string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='" + Type + "' ";

            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(oc_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Other_Collection ";
            Query += "where left(oc_no,len(@Prefix))=@Prefix ";
            Query += "and right(oc_no,len(@suffix))=@suffix ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }

        [HttpGet]
        public string get_Receipt_No1(string Company)
        {
            try
            {
                string Query = "";
                Query += "declare @digit int ";
                Query += "declare @Prefix varchar(33) ";
                Query += "declare @suffix varchar(33) ";
                Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Payment' ";

                Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(cb_uniqno,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Balance ";
                Query += "where left(cb_uniqno,len(@Prefix))=@Prefix ";
                Query += "and right(cb_uniqno,len(@suffix))=@suffix  and cb_vtype='Payment'";

                DataTable dt = GITAPI.dbFunctions.getTable(Query);

                return dt.Rows[0][0].ToString();
            }
            catch { return ""; }
        }

        [HttpGet]
        public string get_Contra_No(string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Contra' ";
            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(c_ref_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Contra ";
            Query += "where left(c_ref_no,len(@Prefix))=@Prefix ";
            Query += "and right(c_ref_no,len(@suffix))=@suffix ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }


        [HttpGet]
        public string get_contractor(string Company = "0")
        {

            string condi = "";
            condi += " and  Con_Status='A' ";
            if (Company.ToLower() != "0")
            {
                condi += " and  Con_Company='" + Company.Replace("_", "") + "'";
            }


            string Query = "select *,Con_Id as value,Con_Name as label from Contractor_Master where 0=0" + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Item_Stock(string godown, string Check_Stock, string Company)
        {
            string Condi = "";
            string Stk_con = "";
            string Query = "";
            if (Check_Stock.ToLower().Equals("true"))
            {
                Stk_con = "and s.stock>0";



                Query = "select Row_number() over(order by i.pm_item_name,pur_purchase_date,p.pur_uni_code) as value, i.pm_rate as Order_Rate, i.pm_id as  Item_ID," +
                          " i.pm_item_name as label,'' as Free,i.pm_id as ID,i.pm_item_code as Item_Code,i.pm_item_name as Item_Name,i.pm_description as [Description],i.pm_model as Model," +
                          " i.pm_category as Category,i.pm_hsn_code as HSN_Code,i.pm_purprice as Purchase_Rate,i.pm_rate as Rate,0 as Wholesale_Rate," +
                          " pm_gst_per as GST_Per,1 as Item_Group,i.pm_item_tamilname as Item_TamilName,i.pm_mrpprice as MRP,i.pm_uom as UOM,0 as Stocks," +
                          " i.pm_short_name as Short_Name,pm_brand as Brand,p.pur_go_down as Store,s.Stock, s.Stock as Stock1, 0 as IN_Qty, dbo.date_(p.pur_bill_date) as P_Date,p.pur_landing_cost as Landing_Cost, " +
                          " (case when p.pur_rate > 0 then cast((p.pur_disc_amt/ p.pur_rate)*100 as decimal(18, 1))  else 0 end) as Disc_per, p.pur_uni_code as Uni_Code, p.pur_landing_cost as Purchse_Price " +
                          " from Product_Master i" +
                          " left outer join" +
                          " (select Uni_code, Item_ID, sum(Inward_Qty-Outward_Qty) as Stock from stock_Details" +
                          "  where 0=0" +
                          "  group by Uni_code,Item_ID" +
                          "  having sum(Inward_Qty - Outward_Qty)>0" +
                          "  ) s on  s.Item_ID = i.pm_id" +
                          " left outer join Purchase_Details p on s.Uni_code=p.pur_uni_code" +
                          " where 0=0  " + Stk_con + " " +
                          " order by i.pm_item_name,i.pm_mrpprice,convert(varchar, p.pur_bill_date, 112)";

            }
            //else
            //{

            //    Query = "select i.ID as value, i.Rate as Order_Rate, i.ID as  Item_ID, i.item_Name as label,'' as Free,  i.*, " +
            //            "isnull( (select sum(Inward_Qty-Outward_Qty) as Stock  from stock_liability" + Company + " s  where i.ID=s.Item_ID),0) as Stock,  " +
            //            "isnull( (select sum(Inward_Qty-Outward_Qty) as Stock  from stock_liability" + Company + " s  where i.ID=s.Item_ID),0) as Stock1, " +
            //            " 0 as IN_Qty,  " +
            //            " '' as P_Date, " +
            //            " isnull(i.L_Cost,0) as Landing_Cost, " +
            //            " ''+cast(i.ID as varchar ) as  Uni_Code,  " +
            //            " isnull(i.L_Cost,0) as Purchse_Price,   " +
            //            " isnull((select top(1) LRate from LRate_Master" + Company + " l where  i.id=l.item_ID and Ledger_ID=7),0) as L_Rate,  " +
            //            " isnull((select top(1) P_Rate from LRate_Master" + Company + " l where  i.id=l.item_ID and Ledger_ID=7),0) as LP_Rate, " +
            //            " (select dbo.date_(Bill_Date) from LRate_Master" + Company + " l where  i.id=l.item_ID and Ledger_ID=7) as L_Date   " +
            //            "    from item_Master" + Company + " i  " +
            //            "   where 0=0    " + Condi + " " +
            //            "   order by i.Item_Name,i.MRP ";
            //}
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_pruchase_details(string From, string To, string Type, string Branch_ID, string Company)
        {
            string condi = "";
            condi += " and convert(Varchar,x.pur_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.pur_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (Type.ToLower() != "all")
            {
                condi += " and  x.pur_type='" + Type + "'";
            }
            if (Branch_ID.ToLower() != "0")
            {
                condi += " and  x.pur_branch='" + Branch_ID + "'";
            }

            string query = "select  dbo.Date_(pur_purchase_date) as pur_purchase_date, dbo.Date_(pur_bill_date) as pur_bill_date,*,dbo.Date_(pur_purchase_date) as Purchase_Date_ from Purchase x   where 0=0 " + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(query);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }

        [HttpGet]
        public string get_Purchase_Entry_details(string Purchase_No, string Type, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select *, dbo.date_(pur_purchase_date) as pur_purchase_date, dbo.Date_(pur_bill_date) as pur_bill_date, pur_qty-isnull((select sum(inward_qty-outward_qty) from stock_Details s where s.Uni_code=y.pur_uni_code),0) as pur_sales_qty from Purchase_details y where pur_purchase_no='" + Purchase_No + "' and pur_type='" + Type + "'  order by  pur_id ");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }



        [HttpGet]
        public string get_pruchase_details1(string Purchase_No, string Company)
        {
            string Query = "";
            string condi = "";
            condi += " and pur_status='A'";
            Query += "select * from Purchase where 0=0 and  pur_type='Purchase' and pur_purchase_no='" + Purchase_No + "'" + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        //public JObject Post_Purchase1(JObject jsonData)
        //{
        //    dynamic json = jsonData;
        //    JObject ob = jsonData;
        //    double TotalPrice = 0.0;
        //    double Final_Amt = 0.0;
        //    double DiscPer = 0.0;
        //    double Disc = 0.0;
        //    double GST_Per = 0.0;
        //    double GST_Amt = 0.0;
        //    double IGST_Per = 0.0;
        //    double SGST_Per = 0.0;
        //    double CGST_Per = 0.0;

        //    DiscPer = float.Parse(isnull(json.pur_DiscPer, "0"));
        //    Disc = float.Parse(isnull(json.pur_Disc, "0"));
        //    TotalPrice = float.Parse(isnull(json.pur_Qty, "")) * float.Parse(isnull(json.pur_Unit_Price, ""));
        //    Final_Amt = TotalPrice - ((TotalPrice * (DiscPer / 100)) + Disc);
        //    GST_Per= float.Parse(isnull(json.pur_GST_Per, "0"));

        //    if (isnull(json.pur_Bill_Type, "").ToLower().Equals("inclusive"))
        //    {
        //        Final_Amt = (Final_Amt / (100 + GST_Per) * 100);

        //    }


        //    if (isnull(json.pur_SupStcd, "").ToLower().Equals("33"))
        //    {
        //        SGST_Per = GST_Per/2;
        //        CGST_Per = GST_Per / 2;
        //        IGST_Per = 0.00;
        //    }
        //    else
        //    {
        //        SGST_Per = 0.00;
        //        CGST_Per = 0.00;
        //        IGST_Per = GST_Per;
        //    }
        //    GST_Amt = (Final_Amt / 100) * GST_Per;
        //    ob["pur_DiscPer"] = DiscPer;
        //    ob["pur_Disc"] = Disc;
        //    ob["pur_Sub_total"] = TotalPrice;
        //    ob["pur_Final_Amt"] = Final_Amt;
        //    ob["pur_SGST_Per"] = SGST_Per;
        //    ob["pur_CGST_Per"] = CGST_Per;
        //    ob["pur_IGST_Per"] = IGST_Per;
        //    ob["pur_SGST_Amt"] = (Final_Amt/100)* SGST_Per;
        //    ob["pur_CGST_Amt"] = (Final_Amt / 100) * CGST_Per;
        //    ob["pur_IGST_Amt"] = (Final_Amt / 100) * IGST_Per;
        //    ob["pur_GST_Amt"] = GST_Amt;
        //    ob["pur_Net_Amt"] = Final_Amt+GST_Amt;

        //    return ob;
        //}

        //    public string Post_Purchase_1(JObject jsonData)
        //{
        //    jsonData = Post_Purchase1(jsonData);
        //    dynamic json = jsonData;
        //    string Company = isnull(json.Company, "");
        //    string Table_Name = isnull(json.Table_Name, "");
        //    string ID = isnull(json.pur_ID, "0");
        //    string User = isnull(json.Created_by, "");
        //    string Query = "";
        //    Boolean isIDn = false;

        //    DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='" + Table_Name + "'");

        //    DataTable dc = GITAPI.dbFunctions.getTable("SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = '" + Table_Name + "'");
        //    int l = 0;
        //    if (dc.Rows.Count > 0)
        //    {
        //        isIDn = true;
        //        l = 1;
        //    }


        //    SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

        //    try
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand();
        //        com.Connection = con;
        //        com.CommandType = CommandType.Text;
        //        if (ID == "0")
        //        {



        //            if (isIDn == false)
        //            {
        //                DataTable dd = GITAPI.dbFunctions.getTable("select isnull(max(" + dt.Rows[0]["Column_Name"].ToString() + "),0)+1 from " + Table_Name );
        //                if (dd.Rows.Count > 0)
        //                {
        //                    json[dt.Rows[0]["Column_Name"].ToString()] = dd.Rows[0][0].ToString();
        //                    ID = dd.Rows[0][0].ToString();
        //                }
        //            }

        //            Query += "insert into " + Table_Name + " (";

        //            for (int i = l; i < dt.Rows.Count - 1; i++)
        //            {

        //                Query += dt.Rows[i]["Column_Name"].ToString() + ",";

        //            }
        //            Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

        //            Query += ") Values ( ";


        //            for (int i = l; i < dt.Rows.Count - 1; i++)
        //            {

        //                Query += "@" + dt.Rows[i]["Column_Name"].ToString() + ",";

        //            }
        //            Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

        //            Query += " )";


        //            com.CommandText = Query;




        //        }
        //        else
        //        {

        //            Query = "";
        //            Query += "update  " + Table_Name + " Set ";

        //            for (int i = l; i < dt.Rows.Count - 1; i++)
        //            {

        //                Query += dt.Rows[i]["Column_Name"].ToString() + "=@" + dt.Rows[i]["Column_Name"].ToString() + ", ";

        //            }


        //            Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "=@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + " ";

        //            Query += "where  " + dt.Rows[0]["Column_Name"].ToString() + "=@" + dt.Rows[0]["Column_Name"].ToString() + " ";


        //            com.CommandText = Query;
        //            if (l == 1)
        //            {
        //                com.Parameters.Add("@" + dt.Rows[0]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[0]["Column_Name"].ToString()], "0");
        //            }
        //        }


        //        Query = "";
        //        for (int i = l; i < dt.Rows.Count; i++)
        //        {
        //            string DF_value = "";
        //            string Column_Type = dt.Rows[i]["Data_Type"].ToString().ToUpper();
        //            string TB_DF_value = dt.Rows[i]["Column_Default"].ToString().ToUpper();
        //            TB_DF_value = TB_DF_value.Replace("'", "").Replace("(", "").Replace(")", "");
        //            if (Column_Type.Equals("VARCHAR") || Column_Type.Equals("NVARCHAR") || Column_Type.Equals("CHAR") || Column_Type.Equals("NCHAR"))
        //            {
        //                DF_value = "";

        //            }

        //            else if (Column_Type.Equals("DECIMAL") || Column_Type.Equals("FLOAT") || Column_Type.Equals("NUMERIC"))
        //            {
        //                DF_value = "0";

        //            }
        //            else
        //                DF_value = "";

        //            if (TB_DF_value != "")
        //                DF_value = TB_DF_value;


        //            string Column = dt.Rows[i]["Column_Name"].ToString();

        //            if (Column.ToLower().Equals("pur_id"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = ID;
        //            }
        //            else if (Column.ToLower().Equals("pur_created_date"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
        //            }
        //            else if (Column.ToLower().Equals("pur_status"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = "A";
        //            }

        //            else if (Column.ToLower().Equals("pur_created_by"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
        //            }
        //            else if (Column.ToLower().Equals("pur_created_by"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
        //            }
        //            else
        //            {
        //                if (Column_Type.Equals("NVARCHAR"))
        //                {
        //                    com.Parameters.Add("@" + Column, SqlDbType.NVarChar).Value = isnull(json[Column], DF_value);
        //                }
        //                else
        //                {
        //                    com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = isnull(json[Column], DF_value);
        //                }
        //            }


        //        }



        //        com.ExecuteNonQuery();
        //        com.Connection.Close();



        //        return "True";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}



        public string Post_Estimation(JObject jsonData)
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

                    if (Column.ToLower().Equals(ColumnPerfix + "id"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = ID;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "created_date"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = "A";
                    }

                    else if (Column.ToLower().Equals(ColumnPerfix + "created_by"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "company"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Company.Replace("_", "");
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
        public string get_Estimation_details(string From, string To, string Branch_ID, string Company)
        {
            string Query = "";
            string condi = "";
            if (!Branch_ID.ToLower().Equals("0"))
            {
                condi += " and  est_branch='" + Branch_ID + "' ";
            }
            //if (!Company.ToLower().Equals("0"))
            //{
            //    condi += " and  est_company='" + Company.Replace("_", "") + "' ";
            //}
            //condi += " and convert(Varchar,est_tenderdate,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,est_tenderdate,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";
            condi += " and est_status='A' ";
            Query += "select dbo.date_(est_date) as [est_date],dbo.date_(est_tenderdate) as [est_tenderdate],dbo.date_(est_agreementdate) as [est_agreementdate],dbo.date_(est_completiondate) as [est_completiondate],* from estimation where  0=0" + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string delete_Estimation(string ID, string UserName, string Company)
        {
            string q = "Update Estimation set est_status='D',est_updated_by='" + UserName + "',est_updated_date='" + GITAPI.dbFunctions.getdate() + "' where est_id='" + ID + "'";
            DataTable dd5 = GITAPI.dbFunctions.getTable(q);

            return "True";
        }

        [HttpGet]
        public string get_Estimation_details1(string Company)
        {
            string Query = "";
            string condi = "";

            //if (!Company.ToLower().Equals("0"))
            //{
            //    condi += " and  est_company='" + Company.Replace("_", "") + "' ";
            //}
            condi += " and est_status='A' ";
            Query += "select est_projectname as label,dbo.date_(est_date) as [est_date],dbo.date_(est_tenderdate) as [est_tenderdate],dbo.date_(est_agreementdate) as [est_agreementdate],dbo.date_(est_completiondate) as [est_completiondate],est_id as value,* from estimation where  0=0" + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Customer_Master(string Company)
        {
            string q = "select  * from Ledger_Master";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }



        //[HttpPost]
        //public string Post_Purchase_2(JObject jsonData)
        //{
        //    dynamic json = jsonData;
        //    string ID = isnull(json.Pur_ID, "0");
        //    string Company = isnull(json.Company, "");
        //    string Ledger_ID = isnull(json.pur_SupID, "");
        //    string Purchse_No = isnull(json.pur_Purchase_No, "");
        //    string Purchase_Date = isnull(json.pur_Purchase_Date, "");
        //    string Bill_No = isnull(json.pur_Bill_No, "");
        //    string Bill_Date = isnull(json.pur_Bill_Date, "");
        //   // string Purchse_No = isnull(json.Bill_No, "");
        //    string Item_Rate_Update = isnull(json.Item_Rate_Update, "false");
        //    string Table_Name = isnull(json.Table_Name, "");


        //    //string Bill_Date = isnull(json.Bill_Date, "");

        //    Newtonsoft.Json.Linq.JArray items = json.items;

        //    string Query = "";
        //    Boolean isIDn = false;

        //    DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='" + Table_Name + "'");

        //    DataTable dc = GITAPI.dbFunctions.getTable("SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = '" + Table_Name + "'");
        //    int l = 0;
        //    if (dc.Rows.Count > 0)
        //    {
        //        isIDn = true;
        //        l = 1;
        //    }


        //    SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);

        //    try
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand();
        //        com.Connection = con;
        //        com.CommandType = CommandType.Text;
        //        if (ID == "0")
        //        {



        //            if (isIDn == false)
        //            {
        //                DataTable dd = GITAPI.dbFunctions.getTable("select isnull(max(" + dt.Rows[0]["Column_Name"].ToString() + "),0)+1 from " + Table_Name);
        //                if (dd.Rows.Count > 0)
        //                {
        //                    json[dt.Rows[0]["Column_Name"].ToString()] = dd.Rows[0][0].ToString();
        //                    ID = dd.Rows[0][0].ToString();
        //                }
        //            }

        //            Query += "insert into " + Table_Name + " (";

        //            for (int i = l; i < dt.Rows.Count - 1; i++)
        //            {

        //                Query += dt.Rows[i]["Column_Name"].ToString() + ",";

        //            }
        //            Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

        //            Query += ") Values ( ";


        //            for (int i = l; i < dt.Rows.Count - 1; i++)
        //            {

        //                Query += "@" + dt.Rows[i]["Column_Name"].ToString() + ",";

        //            }
        //            Query += "@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "";

        //            Query += " )";


        //            com.CommandText = Query;




        //        }
        //        else
        //        {

        //            Query = "";
        //            Query += "update  " + Table_Name + " Set ";

        //            for (int i = l; i < dt.Rows.Count - 1; i++)
        //            {

        //                Query += dt.Rows[i]["Column_Name"].ToString() + "=@" + dt.Rows[i]["Column_Name"].ToString() + ", ";

        //            }


        //            Query += dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + "=@" + dt.Rows[dt.Rows.Count - 1]["Column_Name"].ToString() + " ";

        //            Query += "where  " + dt.Rows[0]["Column_Name"].ToString() + "=@" + dt.Rows[0]["Column_Name"].ToString() + " ";


        //            com.CommandText = Query;
        //            if (l == 1)
        //            {
        //                com.Parameters.Add("@" + dt.Rows[0]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[0]["Column_Name"].ToString()], "0");
        //            }
        //        }


        //        Query = "";
        //        for (int i = l; i < dt.Rows.Count; i++)
        //        {
        //            string DF_value = "";
        //            string Column_Type = dt.Rows[i]["Data_Type"].ToString().ToUpper();
        //            string TB_DF_value = dt.Rows[i]["Column_Default"].ToString().ToUpper();
        //            TB_DF_value = TB_DF_value.Replace("'", "").Replace("(", "").Replace(")", "");
        //            if (Column_Type.Equals("VARCHAR") || Column_Type.Equals("NVARCHAR") || Column_Type.Equals("CHAR") || Column_Type.Equals("NCHAR"))
        //            {
        //                DF_value = "";

        //            }

        //            else if (Column_Type.Equals("DECIMAL") || Column_Type.Equals("FLOAT") || Column_Type.Equals("NUMERIC"))
        //            {
        //                DF_value = "0";

        //            }
        //            else
        //                DF_value = "";

        //            if (TB_DF_value != "")
        //                DF_value = TB_DF_value;


        //            string Column = dt.Rows[i]["Column_Name"].ToString();

        //            if (Column.ToLower().Equals("pur_id"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = ID;
        //            }
        //            else if (Column.ToLower().Equals("pur_created_date"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
        //            }
        //            else if (Column.ToLower().Equals("pur_status"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = "A";
        //            }

        //            else if (Column.ToLower().Equals("pur_created_by"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
        //            }
        //            else if (Column.ToLower().Equals("pur_created_by"))
        //            {
        //                com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = User;
        //            }
        //            else
        //            {
        //                if (Column_Type.Equals("NVARCHAR"))
        //                {
        //                    com.Parameters.Add("@" + Column, SqlDbType.NVarChar).Value = isnull(json[Column], DF_value);
        //                }
        //                else
        //                {
        //                    com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = isnull(json[Column], DF_value);
        //                }
        //            }


        //        }



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
        public string delete_Purchase(string Purchase_No, string User_Name, string Company)
        {
            string q = "insert into Purchase_Delete select '" + User_Name + "','" + GITAPI.dbFunctions.getdate() + "',* from Purchase where pur_purchase_no='" + Purchase_No + "'";
            string q1 = "insert into Purchase_Details_Delete select '" + User_Name + "','" + GITAPI.dbFunctions.getdate() + "',* from Purchase_details where pur_purchase_no='" + Purchase_No + "'";
            DataTable dd5 = GITAPI.dbFunctions.getTable("delete from Stock_Details  where  Vour_Type='" + Acc_Data.G_Type_Purchase + "' and Voucher_No='" + Purchase_No + "'");
            DataTable dt4c = GITAPI.dbFunctions.getTable("delete from Balance  where  cb_billno='" + Purchase_No + "' and cb_vtype='Purchase'");
            DataTable dd2 = GITAPI.dbFunctions.getTable(q + " delete  from Purchase where pur_purchase_no='" + Purchase_No + "'");
            DataTable dd3 = GITAPI.dbFunctions.getTable(q1 + " delete  from Purchase_details where pur_purchase_no='" + Purchase_No + "'");

            return "True";
        }

        [HttpPost]
        public string Post_Purchase(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "0");
            string Company = isnull(json.Company, "");
            string Created_by = isnull(json.Created_by, "");
            string Ledger_ID = isnull(json.pur_ledger_id, "");
            string Purchse_No = isnull(json.pur_bill_no, "");
            string Item_Rate_Update = isnull(json.Item_Rate_Update, "false");
            string ColumnPerfix = isnull(json.ColumnPerfix, "");


            string Bill_Date = isnull(json.pur_bill_date, "");

            Newtonsoft.Json.Linq.JArray items = json.items;



            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Purchase'");
            string Query = "";

            string Purchse_Date = "";

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {

                    Purchse_Date = isnull(json.pur_purchase_date, "");
                    Purchse_No = get_Purchase_No("Purchase", Company);

                    Query += "insert into Purchase (";

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

                    Purchse_Date = isnull(json.pur_purchase_date, "");
                    Purchse_No = isnull(json.pur_purchase_no, "");


                    Query = "";
                    Query += "update  Purchase Set ";

                    for (int i = 1; i < dt.Rows.Count - 1; i++)
                    {
                        string col = dt.Rows[i]["Column_Name"].ToString();
                        if (!col.ToLower().Equals(ColumnPerfix + "created_date") ||
                             !col.ToLower().Equals(ColumnPerfix + "created_by"))
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
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Created_by;
                    }
                    else if (Column.Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                    }
                    else if (Column.Equals(ColumnPerfix + "purchase_no"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No;
                    }
                    else if (Column.Equals(ColumnPerfix + "purchase_date"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_Date;
                    }
                    else
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[i]["Column_Name"].ToString()], DF_value);
                    }


                }

                com.ExecuteNonQuery();
                com.Connection.Close();



                string q1 = "insert into Purchase_Details_Delete select '','" + GITAPI.dbFunctions.getdate() + "',* from Purchase_Details where pur_purchase_no='" + Purchse_No + "'";
                DataTable dds = GITAPI.dbFunctions.getTable(q1 + " delete from Purchase_Details where pur_purchase_no='" + Purchse_No + "'");

                dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Purchase_details'");
                bool Flags = true;
                for (int i = 0; i < items.Count; i++)
                {
                    var item = (JObject)items[i];
                    Query = "";
                    con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
                    try
                    {
                        con.Open();
                        com = new SqlCommand();
                        com.Connection = con;
                        com.CommandType = CommandType.Text;

                        Query += "insert into Purchase_details (";

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
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Created_by;
                            }
                            else if (Column.Equals(ColumnPerfix + "status"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                            }
                            else if (Column.Equals(ColumnPerfix + "purchase_no"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No;
                            }
                            else if (Column.Equals(ColumnPerfix + "purchase_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_Date;
                            }
                            else if (Column.Equals(ColumnPerfix + "bill_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Bill_Date;
                            }
                            else if (Column.Equals(ColumnPerfix + "landing_cost"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = ((decimal.Parse(items[i]["pur_net_amt"].ToString())) / ((decimal.Parse(items[i]["pur_qty"].ToString())) + (decimal.Parse(items[i]["pur_free"].ToString())))).ToString("0.00");
                            }
                            else if (Column.Equals(ColumnPerfix + "uni_code"))
                            {

                                //string Landing_Cost = (decimal.Parse(items[i]["Net_Amt"].ToString()) / ((decimal.Parse(items[i]["Qty"].ToString()))+(decimal.Parse(items[i]["Free"].ToString())))).ToString("00000.00").Replace(".","_");
                                //com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = int.Parse(items[i]["Item_ID"].ToString()).ToString("000000") + "-" + int.Parse(Ledger_ID).ToString("000000") + "-" + Landing_Cost;

                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No + '~' + int.Parse(items[i]["pur_prodid"].ToString()).ToString("0000");
                            }
                            else
                            {
                                string columnName = dt.Rows[k]["Column_Name"].ToString();
                                string data = DF_value;

                                try
                                {
                                    data = items[i][columnName].ToString();
                                }
                                catch
                                {
                                    try
                                    {
                                        data = isnull(json[columnName], DF_value);
                                    }
                                    catch
                                    {
                                        data = DF_value;
                                    }
                                }


                                com.Parameters.Add("@" + columnName, SqlDbType.VarChar).Value = data;
                            }

                        }


                        //try
                        //{
                        //    DataTable df1 = GITAPI.dbFunctions.getTable("update item_Master" + Company + " set Suplier_ID=" + Ledger_ID + ", Purchase_Rate=" + decimal.Parse(items[i]["Unit_Price"].ToString()) + " where ID=" + items[i]["Item_ID"].ToString());
                        //}
                        //catch { }

                        //if (Item_Rate_Update.ToLower().Equals("true"))
                        //{

                        //    DataTable df = GITAPI.dbFunctions.getTable("update item_Master" + Company + " set  wholesale_Rate=" + items[i]["Wholesale_Rate"].ToString() + " , Rate=" + items[i]["Sale_Rate"].ToString() + " ,GST_Per=" + items[i]["GST_Per"].ToString() + " where ID=" + items[i]["Item_ID"].ToString());
                        //}
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

                    DataTable dc1 = GITAPI.dbFunctions.getTable("select pur_id from Purchase where pur_purchase_no='" + Purchse_No + "'");
                    if (dc1.Rows.Count > 0)
                    {
                        ID = dc1.Rows[0][0].ToString();
                    }



                    //dt = GITAPI.dbFunctions.getTable("delete from Stock_Details where Voucher_No='" + Purchse_No + "' and Vour_Type='Purchase'");


                    //// Item Wise Qty Update
                    //Query = "insert into Stock_Details"+
                    //      " (Uni_Code,Vour_Type, Vour_RefNo, Order_No, Voucher_No, Voucher_Date, Item_ID, Inward_Qty, Outward_Qty, Rate, Amount, Credit_Amt, Debit_Amt, Created_by, Created_Date, [Status], Barcode ) " +
                    //             " select pur_uni_code," +
                    //             "'purchase'," +
                    //             "pur_id," +
                    //             "3," +
                    //             " pur_purchase_no, " +
                    //             "pur_purchase_date," +
                    //             "[pur_prodid]," +
                    //             "pur_qty+pur_free AS INWARD_QTY, " +
                    //             "0  as Out_Qty, " +
                    //             "[pur_rate] AS RATE," +
                    //             "[pur_rate]*pur_qty AS AMOUNT, " +
                    //             "[pur_rate]*pur_qty  AS CRAMOUNT," +
                    //             "0 AS DEBIT_AMT," +
                    //             "pur_created_by,   " +
                    //             "pur_created_date, " +
                    //             "pur_status ,pur_prodcode  " +
                    //             " from  " +
                    //             " Purchase_details"+
                    //             " where pur_purchase_no= '" + Purchse_No + "'";

                    //DataTable dd = GITAPI.dbFunctions.getTable(Query);

                    Acc_Data.Accounts_Update(Acc_Data.G_Type_Purchase, ID, Company, "Purchase");

                }



                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        [HttpPost]
        public string Post_StockIN(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "0");
            string Company = isnull(json.Company, "");
            string Created_by = isnull(json.Created_by, "");
            string Ledger_ID = isnull(json.pur_ledger_id, "");
            string Purchse_No = isnull(json.pur_purchase_no, "");
            string Item_Rate_Update = isnull(json.Item_Rate_Update, "false");
            string ColumnPerfix = isnull(json.ColumnPerfix, "");


            string Bill_Date = isnull(json.pur_bill_date, "");

            Newtonsoft.Json.Linq.JArray items = json.items;



            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Purchase'");
            string Query = "";

            string Purchse_Date = "";

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {

                    Purchse_Date = isnull(json.pur_purchase_date, "");
                    Purchse_No = get_Purchase_No("Stock IN", Company);

                    Query += "insert into Purchase (";

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

                    Purchse_Date = isnull(json.pur_purchase_date, "");
                    Purchse_No = isnull(json.pur_purchase_no, "");


                    Query = "";
                    Query += "update  Purchase Set ";

                    for (int i = 1; i < dt.Rows.Count - 1; i++)
                    {
                        string col = dt.Rows[i]["Column_Name"].ToString();
                        if (!col.ToLower().Equals(ColumnPerfix + "created_date") ||
                             !col.ToLower().Equals(ColumnPerfix + "created_by"))
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
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Created_by;
                    }
                    else if (Column.Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                    }
                    else if (Column.Equals(ColumnPerfix + "purchase_no"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No;
                    }
                    else if (Column.Equals(ColumnPerfix + "purchase_date"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_Date;
                    }
                    else
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[i]["Column_Name"].ToString()], DF_value);
                    }


                }

                com.ExecuteNonQuery();
                com.Connection.Close();



                string q1 = "insert into Purchase_Details_Delete select '','" + GITAPI.dbFunctions.getdate() + "',* from Purchase_Details where pur_purchase_no='" + Purchse_No + "'";
                DataTable dds = GITAPI.dbFunctions.getTable(q1 + " delete from Purchase_Details where pur_purchase_no='" + Purchse_No + "'");

                dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Purchase_details'");
                bool Flags = true;
                for (int i = 0; i < items.Count; i++)
                {
                    var item = (JObject)items[i];
                    Query = "";
                    con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
                    try
                    {
                        con.Open();
                        com = new SqlCommand();
                        com.Connection = con;
                        com.CommandType = CommandType.Text;

                        Query += "insert into Purchase_details (";

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
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Created_by;
                            }
                            else if (Column.Equals(ColumnPerfix + "status"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                            }
                            else if (Column.Equals(ColumnPerfix + "purchase_no"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No;
                            }
                            else if (Column.Equals(ColumnPerfix + "purchase_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_Date;
                            }
                            else if (Column.Equals(ColumnPerfix + "bill_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Bill_Date;
                            }
                            else if (Column.Equals(ColumnPerfix + "landing_cost"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = ((decimal.Parse(items[i]["pur_net_amt"].ToString())) / ((decimal.Parse(items[i]["pur_qty"].ToString())) + (decimal.Parse(items[i]["pur_free"].ToString())))).ToString("0.00");
                            }
                            else if (Column.Equals(ColumnPerfix + "uni_code"))
                            {

                                //string Landing_Cost = (decimal.Parse(items[i]["Net_Amt"].ToString()) / ((decimal.Parse(items[i]["Qty"].ToString()))+(decimal.Parse(items[i]["Free"].ToString())))).ToString("00000.00").Replace(".","_");
                                //com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = int.Parse(items[i]["Item_ID"].ToString()).ToString("000000") + "-" + int.Parse(Ledger_ID).ToString("000000") + "-" + Landing_Cost;

                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No + '~' + int.Parse(items[i]["pur_prodid"].ToString()).ToString("0000");
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

                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Data;
                            }

                        }


                        //try
                        //{
                        //    DataTable df1 = GITAPI.dbFunctions.getTable("update item_Master" + Company + " set Suplier_ID=" + Ledger_ID + ", Purchase_Rate=" + decimal.Parse(items[i]["Unit_Price"].ToString()) + " where ID=" + items[i]["Item_ID"].ToString());
                        //}
                        //catch { }

                        //if (Item_Rate_Update.ToLower().Equals("true"))
                        //{

                        //    DataTable df = GITAPI.dbFunctions.getTable("update item_Master" + Company + " set  wholesale_Rate=" + items[i]["Wholesale_Rate"].ToString() + " , Rate=" + items[i]["Sale_Rate"].ToString() + " ,GST_Per=" + items[i]["GST_Per"].ToString() + " where ID=" + items[i]["Item_ID"].ToString());
                        //}
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

                    DataTable dc1 = GITAPI.dbFunctions.getTable("select pur_id from Purchase where pur_purchase_no='" + Purchse_No + "'");
                    if (dc1.Rows.Count > 0)
                    {
                        ID = dc1.Rows[0][0].ToString();
                    }



                    dt = GITAPI.dbFunctions.getTable("delete from Stock_Details where Voucher_No='" + Purchse_No + "' and Vour_Type='Purchase'");


                    // Item Wise Qty Update
                    Query = "insert into Stock_Details" +
                          " (Uni_Code,Vour_Type, Vour_RefNo, Order_No, Voucher_No, Voucher_Date, Item_ID, Inward_Qty, Outward_Qty, Rate, Amount, Credit_Amt, Debit_Amt, Created_by, Created_Date, [Status], Barcode ) " +
                                 " select pur_uni_code," +
                                 "'purchase'," +
                                 "pur_id," +
                                 "3," +
                                 " pur_purchase_no, " +
                                 "pur_purchase_date," +
                                 "[pur_prodid]," +
                                 "pur_qty+pur_free AS INWARD_QTY, " +
                                 "0  as Out_Qty, " +
                                 "[pur_rate] AS RATE," +
                                 "[pur_rate]*pur_qty AS AMOUNT, " +
                                 "[pur_rate]*pur_qty  AS CRAMOUNT," +
                                 "0 AS DEBIT_AMT," +
                                 "pur_created_by,   " +
                                 "pur_created_date, " +
                                 "pur_status ,pur_prodcode  " +
                                 " from  " +
                                 " Purchase_details" +
                                 " where pur_purchase_no= '" + Purchse_No + "'";

                    DataTable dd = GITAPI.dbFunctions.getTable(Query);

                    //Acc_Data.Accounts_Update(GITAPI.Acc_Data.G_Type_Purchase, ID, Company, "Purchase");

                }



                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string get_Pro_No(string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Production' ";

            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(pur_purchase_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Purchase ";
            Query += "where left(pur_purchase_no,len(@Prefix))=@Prefix ";
            Query += "and right(pur_purchase_no,len(@suffix))=@suffix ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }

        [HttpGet]
        public string delete_Production(string Pro_No, string User_Name, string Company)
        {
            string q = "insert into Purchase_Delete select '" + User_Name + "','" + GITAPI.dbFunctions.getdate() + "',* from Purchase where pur_purchase_no='" + Pro_No + "'";
            string q1 = "insert into Purchase_Details_Delete select '" + User_Name + "','" + GITAPI.dbFunctions.getdate() + "',* from Purchase_details where pur_purchase_no='" + Pro_No + "'";
            DataTable dd5 = GITAPI.dbFunctions.getTable("delete from Stock_Details  where  (Vour_Type='Production' or Vour_Type='Production_Issue') and Voucher_No='" + Pro_No + "'");
            DataTable dd2 = GITAPI.dbFunctions.getTable(q + " delete  from Purchase where pur_purchase_no='" + Pro_No + "'");
            DataTable dd3 = GITAPI.dbFunctions.getTable(q1 + " delete  from Purchase_details where pur_purchase_no='" + Pro_No + "'");

            return "True";
        }

        [HttpGet]
        public string get_Production_Entry_details(string Pro_No, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select  dbo.date_(pur_purchase_date) as pur_purchase_date, dbo.Date_(pur_bill_date) as pur_bill_date,* from Purchase_details where pur_purchase_no='" + Pro_No + "'  order by  pur_id ");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Production_Detail(string From, string To, string User, string order_by, string Branch_ID, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,x.pur_purchase_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.pur_purchase_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (Branch_ID.ToLower() != "0")
            {
                condi += " and  x.pur_branch='" + Branch_ID + "'";
            }
            if (User.ToLower() != "all")
            {
                condi += " and  x.pur_created_by='" + User + "'";
            }

            condi += " and pur_type='Production'";
            string query = "select dbo.Date_(pur_purchase_date) as Pro_Date, *,dbo.Date_(pur_purchase_date) as Pro_Date_ from Purchase x   where 0=0 " + condi + " order by x." + order_by;
            DataTable dt = GITAPI.dbFunctions.getTable(query);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }


        [HttpPost]
        public string Post_Production(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "0");
            string Company = isnull(json.Company, "");
            string Created_by = isnull(json.Created_by, "");
            string pur_purchase_no = isnull(json.pur_purchase_no, "");
            string ColumnPerfix = isnull(json.ColumnPerfix, "");


            Newtonsoft.Json.Linq.JArray items = json.items;



            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='purchase'");
            string Query = "";

            string pur_purchase_date = "";

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {
                    pur_purchase_date = GITAPI.dbFunctions.getdate();
                    pur_purchase_no = get_Pro_No(Company);

                    Query += "insert into purchase (";

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



                    pur_purchase_date = isnull(json.pur_purchase_date, "");
                    pur_purchase_no = isnull(json.pur_purchase_no, "");


                    Query = "";
                    Query += "update  purchase Set ";

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
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Created_by;
                    }
                    else if (Column.Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                    }
                    else if (Column.Equals(ColumnPerfix + "purchase_date"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = pur_purchase_date;
                    }
                    else if (Column.Equals(ColumnPerfix + "bill_date"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = pur_purchase_date;
                    }
                    else
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[i]["Column_Name"].ToString()], DF_value);
                    }


                }

                com.ExecuteNonQuery();
                com.Connection.Close();




                DataTable dds = GITAPI.dbFunctions.getTable("delete from Purchase_details where pur_purchase_no='" + pur_purchase_no + "'");

                dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Purchase_details'");
                bool Flags = true;
                for (int i = 0; i < items.Count; i++)
                {
                    var item = (JObject)items[i];
                    Query = "";
                    con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
                    try
                    {
                        con.Open();
                        com = new SqlCommand();
                        com.Connection = con;
                        com.CommandType = CommandType.Text;

                        Query += "insert into Purchase_details (";

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
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Created_by;
                            }
                            else if (Column.Equals(ColumnPerfix + "status"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                            }
                            else if (Column.Equals(ColumnPerfix + "purchase_no"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = pur_purchase_no;
                            }
                            else if (Column.Equals(ColumnPerfix + "purchase_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = pur_purchase_date;
                            }
                            else if (Column.Equals(ColumnPerfix + "bill_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = pur_purchase_date;
                            }
                            else if (Column.Equals(ColumnPerfix + "uni_code"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = pur_purchase_no + '~' + int.Parse(items[i]["pur_prodid"].ToString()).ToString("0000");
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


                if (Flags)
                {

                    DataTable dc1 = GITAPI.dbFunctions.getTable("select ID from purchase where pur_purchase_no='" + pur_purchase_no + "'");
                    if (dc1.Rows.Count > 0)
                    {
                        ID = dc1.Rows[0][0].ToString();
                    }



                    dt = GITAPI.dbFunctions.getTable("delete from Stock_Details where Voucher_No='" + pur_purchase_no + "' and Vour_Type='Production'");


                    // Item Wise Qty Update
                    Query = "insert into Stock_Details" +
                          " (Vour_Type, Vour_RefNo, Order_No, Voucher_No, Voucher_Date, Item_ID, Inward_Qty, Outward_Qty, Rate, Amount, Credit_Amt, Debit_Amt, Created_by, Created_Date, [Status], Barcode ) " +
                          " select " +
                          "'Production'," +
                                 "pur_id," +
                                 "3," +
                                 " pur_purchase_no, " +
                                 "pur_purchase_date," +
                                 "[pur_prodid]," +
                                 "pur_qty+pur_free AS INWARD_QTY, " +
                                 "0  as Out_Qty, " +
                                 "[pur_rate] AS RATE," +
                                 "[pur_rate]*pur_qty AS AMOUNT, " +
                                 "[pur_rate]*pur_qty  AS CRAMOUNT," +
                                 "0 AS DEBIT_AMT," +
                                 "pur_created_by,   " +
                                 "pur_created_date, " +
                                 "pur_status ,pur_prodcode  " +
                                 " from  " +
                                 " purchase_details" +
                                 " where pur_purchase_no= '" + pur_purchase_no + "'";

                    DataTable dd = GITAPI.dbFunctions.getTable(Query);


                    string RM_OUT = System.Configuration.ConfigurationSettings.AppSettings["RM_OUT"];

                    if (RM_OUT.ToLower().Equals("yes"))
                    {
                        // Item Wise Qty Update
                        Query = "insert into Stock_Details" +
                              " (Vour_Type, Vour_RefNo, Order_No, Voucher_No, Voucher_Date, Item_ID, Inward_Qty, Outward_Qty, Rate, Amount, Credit_Amt, Debit_Amt, Created_by, Created_Date, [Status], Barcode ) " +
                                     " select " +
                                     "'Production_Issue'," +
                                     "p.pur_id," +
                                     "3," +
                                     "p.pur_purchase_no, " +
                                     "p.pur_purchase_date," +
                                     "[Bom_RMId]," +
                                     "0 AS INWARD_QTY, " +
                                     "(pur_qty*Bom_Qty) as Out_Qty, " +
                                     "[pur_rate] AS RATE," +
                                     "[pur_rate]*pur_qty AS AMOUNT, " +
                                     "0 AS CRAMOUNT," +
                                     "[pur_rate]*(pur_qty*Bom_Qty) AS DEBIT_AMT," +
                                 "pur_created_by,   " +
                                 "pur_created_date, " +
                                 "pur_status ,pur_prodcode  " +
                                     " from  " +
                                     " purchase_details p " +
                                     " Left outer join BOM_Master on Bom_ProdId=pur_prodid where pur_purchase_no= '" + pur_purchase_no + "'";
                    }

                    dd = GITAPI.dbFunctions.getTable(Query);




                }



                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        [HttpGet]
        public string delete_Material_Movement(string Purchase_No, string Company)
        {
            string q = "insert into Material_Movement_Delete select '','" + GITAPI.dbFunctions.getdate() + "',* from Material_Movement where mm_no='" + Purchase_No + "'";
            string q1 = "insert into Material_Movement_Details_Delete select '','" + GITAPI.dbFunctions.getdate() + "',* from Material_Movement_Details where mm_no='" + Purchase_No + "'";
            DataTable dd5 = GITAPI.dbFunctions.getTable("delete from Stock_Details  where  Vour_Type='Material_Movement' and Voucher_No='" + Purchase_No + "'");
            //DataTable dt4c = GITAPI.dbFunctions.getTable("delete from Balance  where  cb_billno='" + Purchase_No + "' and cb_vtype='Material_Movement'");
            DataTable dd2 = GITAPI.dbFunctions.getTable(q + " delete  from Material_Movement where mm_no='" + Purchase_No + "'");
            DataTable dd3 = GITAPI.dbFunctions.getTable(q1 + " delete  from Material_Movement_details where mm_no='" + Purchase_No + "'");

            return "True";
        }

        [HttpGet]
        public string get_Material_Movement_details(string From, string To, string Branch_ID, string Company)
        {
            string condi = "";
            condi += " and convert(Varchar,x.mm_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.mm_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (Branch_ID.ToLower() != "0")
            {
                condi += " and  x.mm_branch='" + Branch_ID + "'";
            }

            string query = "select  dbo.Date_(mm_date) as mm_date, dbo.Date_(mm_projectdate) as mm_projectdate,* from Material_Movement x   where 0=0 " + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(query);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }



        [HttpGet]
        public string Material_Movement_details(string Purchase_No, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select dbo.date_(mm_date) as mm_date,dbo.date_(mm_projectdate) as mm_projectdate,*, mm_qty-isnull((select sum(inward_qty-outward_qty) from stock_Details s where s.Uni_code=y.mm_uni_code),0) as pur_sales_qty from Material_Movement_details y where mm_no='" + Purchase_No + "'  order by  mm_id ");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpPost]
        public string Post_Material_Movement(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "0");
            string Company = isnull(json.Company, "");
            string Ledger_ID = isnull(json.mm_ledger_id, "");
            string Purchse_No = isnull(json.mm_bill_no, "");
            string Item_Rate_Update = isnull(json.Item_Rate_Update, "false");
            string ColumnPerfix = isnull(json.ColumnPerfix, "");


            //string Bill_Date = isnull(json.pur_bill_date, "");

            Newtonsoft.Json.Linq.JArray items = json.items;



            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Material_Movement'");
            string Query = "";

            string Purchse_Date = "";

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {

                    Purchse_Date = isnull(json.mm_date, "");
                    Purchse_No = get_Material_Movement_No(Company);

                    Query += "insert into Material_Movement (";

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

                    Purchse_Date = isnull(json.mm_date, "");
                    Purchse_No = isnull(json.mm_no, "");


                    Query = "";
                    Query += "update  Material_Movement Set ";

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
                    else if (Column.Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                    }
                    else if (Column.Equals(ColumnPerfix + "no"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No;
                    }
                    else if (Column.Equals(ColumnPerfix + "date"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_Date;
                    }
                    else
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = isnull(json[dt.Rows[i]["Column_Name"].ToString()], DF_value);
                    }


                }

                com.ExecuteNonQuery();
                com.Connection.Close();



                string q = "insert into Material_Movement_Details_Delete select '','" + GITAPI.dbFunctions.getdate() + "',* from Material_Movement_Details where mm_no='" + Purchse_No + "'";
                DataTable dds = GITAPI.dbFunctions.getTable(q + " delete from Material_Movement_Details where mm_no='" + Purchse_No + "'");

                dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Material_Movement_details'");
                bool Flags = true;
                for (int i = 0; i < items.Count; i++)
                {
                    var item = (JObject)items[i];
                    Query = "";
                    con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
                    try
                    {
                        con.Open();
                        com = new SqlCommand();
                        com.Connection = con;
                        com.CommandType = CommandType.Text;

                        Query += "insert into Material_Movement_details (";

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
                            else if (Column.Equals(ColumnPerfix + "status"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = "A";
                            }
                            else if (Column.Equals(ColumnPerfix + "no"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No;
                            }
                            else if (Column.Equals(ColumnPerfix + "date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_Date;
                            }
                            //else if (Column.Equals(ColumnPerfix + "bill_date"))
                            //{
                            //    com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Bill_Date;
                            //}
                            //else if (Column.Equals(ColumnPerfix + "landing_cost"))
                            //{
                            //    com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = ((decimal.Parse(items[i]["pur_net_amt"].ToString())) / ((decimal.Parse(items[i]["pur_qty"].ToString())) + (decimal.Parse(items[i]["pur_free"].ToString())))).ToString("0.00");
                            //}
                            //else if (Column.Equals(ColumnPerfix + "uni_code"))
                            //{

                            //    //string Landing_Cost = (decimal.Parse(items[i]["Net_Amt"].ToString()) / ((decimal.Parse(items[i]["Qty"].ToString()))+(decimal.Parse(items[i]["Free"].ToString())))).ToString("00000.00").Replace(".","_");
                            //    //com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = int.Parse(items[i]["Item_ID"].ToString()).ToString("000000") + "-" + int.Parse(Ledger_ID).ToString("000000") + "-" + Landing_Cost;

                            //    com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No + '~' + int.Parse(items[i]["pur_prodid"].ToString()).ToString("0000");
                            //}
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
                if (Flags)
                {
                    DataTable dc1 = GITAPI.dbFunctions.getTable("select mm_id from Material_Movement where mm_no='" + Purchse_No + "'");
                    if (dc1.Rows.Count > 0)
                    {
                        ID = dc1.Rows[0][0].ToString();
                    }



                    dt = GITAPI.dbFunctions.getTable("delete from Stock_Details where Voucher_No='" + Purchse_No + "' and Vour_Type='Material_Movement'");


                    // Item Wise Qty Update
                    Query = "insert into Stock_Details" +
                          " (Uni_Code,Vour_Type, Vour_RefNo, Order_No, Voucher_No, Voucher_Date, Item_ID, Inward_Qty, Outward_Qty, Rate, Amount, Credit_Amt, Debit_Amt, Created_by, Created_Date, [Status], Barcode ) " +
                                 " select mm_uni_code as Uni_Code," +
                                 "'Material_Movement' as Vour_Type," +
                                 "mm_id as Vour_RefNo," +
                                 "'4' as Order_No," +
                                 "mm_no as Voucher_No," +
                                 "mm_date as Voucher_Date," +
                                 "mm_prodid as Item_ID," +
                                 "0 as Inward_Qty," +
                                 "mm_qty as Outward_Qty," +
                                 "mm_rate as Rate," +
                                 "mm_rate*mm_qty AS AMOUNT," +
                                 "mm_net_amt as Credit_Amt," +
                                 "0 AS DEBIT_AMT," +
                                 "mm_created_by as Created_by," +
                                 "mm_created_date as mm_created_date," +
                                 "mm_status as Status," +
                                 "mm_prodcode as Barcode " +
                                 "from Material_Movement_details " +
                                 "where mm_no= '" + Purchse_No + "'";
                    DataTable dd = GITAPI.dbFunctions.getTable(Query);
                    Acc_Data.Accounts_Update(Acc_Data.G_Type_Expenses, ID, Company, "Material_Movement");
                }
                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string delete_Tyre_Entry(string ID, string UserName, string Company)
        {
            string q = "UPDATE Tyre_Entry SET trr_status='D',trr_del_date='" + GITAPI.dbFunctions.getdate() + "',trr_del_by='" + UserName + "'  where trr_id=" + ID;
            GITAPI.dbFunctions.getTable(q);
            return "True";
        }


        [HttpGet]
        public string get_Tyre_Entry(string From, string To, string Order_by, string Branch_ID, string Company = "0")
        {
            string condi = "";
            condi += " and convert(Varchar,trr_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,trr_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            condi += " and trr_status='A' ";
            if (!Branch_ID.Equals("0"))
            {
                condi += " and trr_branch='" + Branch_ID +"' ";
            }
            if (!Order_by.Equals(""))
            {
                condi += " Order by " + Order_by;
            }
            string q = "select dbo.Date_(trr_date) as trr_date,dbo.Date_(trr_asmdate) as trr_asmdate,dbo.Date_(trr_dsmdate) as trr_dsmdate,trr_position,* from Tyre_Entry where 0=0 " + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpPost]
        public string Post_Tyre_Entry(JObject jsonData)
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
        public string get_Adjustments(string From, string To, string Type, string Category, string Pay_Mode, string Company)
        {
            string Condi = "";
            if (!(Type == "" || Type == null || Type.ToLower() == "all"))
            {

                Condi += "and [Adj_Type]='" + Type + "' ";
            }

            if (!(Category == "" || Category == null || Category == "0" || Category.ToLower() == "all"))
            {
                Condi += "and [Ledger_ID]='" + Category + "' ";
            }

            if (!(Pay_Mode == "" || Pay_Mode == null || Pay_Mode == "0" || Pay_Mode == "All"))
            {
                Condi += "and pay_mode='" + Pay_Mode + "' ";
            }

            string Query = "select " +
                " ID, " +
                " Adj_Type, " +
                " Adj_No, " +
                " Category, " +
                " dbo.get_Ledger_Name(Ledger_ID) as Ledger_Name , " +
                " Ledger_ID, " +
                " Narration1, " +
                " Narration2, " +
                " Narration3, " +
                " Amount, " +
                " Pay_Mode, " +
                " dbo.get_ref_value(Pay_Mode) as Pay_Mode_ , " +
                " Received_Bank, " +
                " Cheque_No, " +
                " Cheque_Date,Card_Charge, " +
                " Remarks, " +
                " dbo.Date_(Adj_Date) as Adj_Date, " +
                " dbo.Date_(Adj_Date) as Adj_Date_, " +
                 " dbo.Time_(created_date) as Time_, " +
                " Created_by " +
                "  from Adjustments where convert(varchar,Adj_Date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "'" +
                " and convert(varchar,Adj_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'" +
                " " + Condi + " " +
                " order by [Adj_No] ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpPost]
        public string insert_Adjustments(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string Adj_Type = isnull(json.Adj_Type, "");
            string Adj_No = isnull(json.Adj_No, "");
            string Adj_Date = isnull(json.Adj_Date, "");
            string Category = isnull(json.Category, "");
            string Ledger_ID = isnull(json.Ledger_ID, "");
            string Narration1 = isnull(json.Narration1, "");
            string Narration2 = isnull(json.Narration2, "");
            string Narration3 = isnull(json.Narration3, "");
            string Amount = isnull(json.Amount, "0");
            string Pay_Mode = isnull(json.Pay_Mode, "");
            string Received_Bank = isnull(json.Received_Bank, "");
            string Cheque_No = isnull(json.Cheque_No, "");
            string Cheque_Date = isnull(json.Cheque_Date, "");
            string Remarks = isnull(json.Remarks, "");

            string Card_Charge = isnull(json.Card_Charge, "0");
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
                if (ID == "0")
                {

                    com.CommandText = "insert into Adjustments (Card_Charge,Adj_Type,Adj_No,Adj_Date,Category,Ledger_ID,Narration1,Narration2,Narration3,Amount,Pay_Mode,Received_Bank,Cheque_No,Cheque_Date,Remarks,Created_by,Created_Date,Status) Values (@Card_Charge, @Adj_Type,@Adj_No,@Adj_Date,@Category,@Category,@Narration1,@Narration2,@Narration3,@Amount,@Pay_Mode,@Received_Bank,@Cheque_No,@Cheque_Date,@Remarks,@Created_by,'" + GITAPI.dbFunctions.getdate() + "', 'A'  )";
                }
                else
                {
                    com.CommandText = "update Adjustments Set Card_Charge=@Card_Charge, Adj_Type=@Adj_Type, Adj_No=@Adj_No, Adj_Date=@Adj_Date, Category=@Category, Ledger_ID=@Category, Narration1=@Narration1, Narration2=@Narration2, Narration3=@Narration3, Amount=@Amount, Pay_Mode=@Pay_Mode, Received_Bank=@Received_Bank, Cheque_No=@Cheque_No, Cheque_Date=@Cheque_Date, Remarks=@Remarks, Created_by=@Created_by where  ID=@ID ";
                    com.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
                }

                com.Parameters.Add("@Adj_Type", SqlDbType.VarChar).Value = Adj_Type;

                com.Parameters.Add("@Card_Charge", SqlDbType.VarChar).Value = Card_Charge;


                com.Parameters.Add("@Adj_No", SqlDbType.VarChar).Value = Adj_No;
                com.Parameters.Add("@Adj_Date", SqlDbType.VarChar).Value = Adj_Date;
                com.Parameters.Add("@Category", SqlDbType.VarChar).Value = Category;
                com.Parameters.Add("@Ledger_ID", SqlDbType.VarChar).Value = Ledger_ID;
                com.Parameters.Add("@Narration1", SqlDbType.VarChar).Value = Narration1;
                com.Parameters.Add("@Narration2", SqlDbType.VarChar).Value = Narration2;
                com.Parameters.Add("@Narration3", SqlDbType.VarChar).Value = Narration3;
                com.Parameters.Add("@Amount", SqlDbType.VarChar).Value = Amount;
                com.Parameters.Add("@Pay_Mode", SqlDbType.VarChar).Value = Pay_Mode;
                com.Parameters.Add("@Received_Bank", SqlDbType.VarChar).Value = Received_Bank;
                com.Parameters.Add("@Cheque_No", SqlDbType.VarChar).Value = Cheque_No;
                com.Parameters.Add("@Cheque_Date", SqlDbType.VarChar).Value = Cheque_Date;
                com.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = Remarks;
                com.Parameters.Add("@Created_by", SqlDbType.VarChar).Value = Created_by;
                com.ExecuteNonQuery();
                com.Connection.Close();


                //DataTable dts = GITAPI.dbFunctions.getTable("select ID,dbo.get_ref_value(" + Category + ")  from  Adjustments  where Adj_No='" + Adj_No + "'");
                //ID = dts.Rows[0][0].ToString();

                //if (Adj_Type.ToLower().Equals("expense"))
                //{
                //    Acc_Data.Accounts_Update(VINAPI.Acc_Data.G_Type_Expenses, ID, Company, dts.Rows[0][1].ToString());
                //}
                //else if (Adj_Type.ToLower().Equals("income"))
                //{
                //    Acc_Data.Accounts_Update(VINAPI.Acc_Data.G_Type_Income, ID, Company);
                //}

                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        [HttpGet]
        public string delete_OtherCollection(string Purchase_No, string Company)
        {
            string q = "insert into Other_Collection_Delete select '','" + GITAPI.dbFunctions.getdate() + "',* from Other_Collection where oc_no='" + Purchase_No + "'";
            DataTable dt4c = GITAPI.dbFunctions.getTable("delete from Balance  where  cb_billno='" + Purchase_No + "' ");
            DataTable dd2 = GITAPI.dbFunctions.getTable(q + " delete  from Other_Collection where oc_no='" + Purchase_No + "'");

            return "True";
        }


        [HttpGet]
        public string delete_Adjustments(string Type, string ID, string UserName, string Company)
        {
            string q = "insert into Other_Collection_Delete select '" + UserName + "','" + GITAPI.dbFunctions.getdate() + "',* from Other_Collection where oc_id='" + ID + "'";
            DataTable dt4c = GITAPI.dbFunctions.getTable("delete from DayBook  where  db_vour_refno='" + ID + "' and db_vourtype='" + Type + "' ");
            DataTable dd2 = GITAPI.dbFunctions.getTable(q + " delete  from Other_Collection where oc_id='" + ID + "'");

            return "True";
        }

        [HttpGet]
        public string get_OtherCollection_details(string From, string To, string Type, string Bank, string Branch_ID, string Company)
        {
            string Query = "";
            string condi = "";
            if (!Type.ToLower().Equals("all"))
            {
                condi += " and oc_type='" + Type + "' ";
            }
            if (!Bank.ToLower().Equals("0"))
            {
                condi += " and oc_received_bank='" + Bank + "' ";
            }
            if (!Branch_ID.ToLower().Equals("0"))
            {
                condi += " and oc_branch='" + Branch_ID + "' ";
            }
            if (!Company.ToLower().Equals("0"))
            {
                condi += " and oc_company='" + Company.Replace("_", "") + "' ";
            }
            condi += " and convert(Varchar,oc_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,oc_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            condi += " order by convert(varchar,oc_date,112) asc ";
            Query += "select dbo.date_(oc_date) as [oc_date],dbo.get_ref_value(oc_pay_mode) as oc_paymode,Account_Number as oc_received_bank,* from Other_Collection " +
                "left outer join bank_master on ID=oc_received_bank where  0=0" + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        //[HttpPost]
        public string Post_Other_Collection(JObject jsonData)
        {
            dynamic json = jsonData;
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");
            string ID = isnull(json.ID, "0");
            string User = isnull(json.Created_by, "");
            string ColumnPerfix = isnull(json.ColumnPerfix, "");
            string Type = isnull(json.oc_type, "0");
            string oc_no = isnull(json.oc_no, "");
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
                    if (Type.Equals("MachinerUsage"))
                    {
                        oc_no = get_othercol_No(Company);
                    }
                    else if (Type.Equals("Contractor"))
                    {

                        oc_no = get_Contractor_No(Company);
                    }
                    else
                    {

                        oc_no = get_OtherCollection_No(Type, Company);
                    }

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
                string VType = "";
                DataTable dc1 = GITAPI.dbFunctions.getTable("select oc_id,oc_vtype from Other_Collection where oc_no='" + oc_no + "'");
                if (dc1.Rows.Count > 0)
                {
                    ID = dc1.Rows[0][0].ToString();
                    VType = dc1.Rows[0][1].ToString();
                }

                //Acc_Data.Accounts_Update(Acc_Data.G_Type_Expenses, ID, Company, Type);
                if (Type.ToLower().Equals("expense"))
                {
                    Acc_Data.Accounts_Update1(Acc_Data.G_Type_Expenses, ID, Company, "");
                }
                else if (Type.ToLower().Equals("income"))
                {
                    Acc_Data.Accounts_Update1(Acc_Data.G_Type_Income, ID, Company, "");
                }
                //else if (Type.ToLower().Equals("loan"))
                //{
                //    Acc_Data.Accounts_Update1(Acc_Data.G_Type_loan, ID, Company, "");
                //}
                else if (Type.ToLower().Equals("loan") && VType.ToLower().Equals("loan"))
                {
                    Acc_Data.Accounts_Update1(Acc_Data.G_Type_loan, ID, Company, "");
                }
                else if (Type.ToLower().Equals("loan") && VType.ToLower().Equals("loan-return"))
                {
                    Acc_Data.Accounts_Update1(Acc_Data.G_Type_Loan_Return, ID, Company, "");
                }
                else if (Type.ToLower().Equals("emd") && VType.ToLower().Equals("emd"))
                {
                    Acc_Data.Accounts_Update1(Acc_Data.G_Type_Deposit, ID, Company, "");
                }
                else if (Type.ToLower().Equals("emd") && VType.ToLower().Equals("emd-return"))
                {
                    Acc_Data.Accounts_Update1(Acc_Data.G_Type_Deposit_Return, ID, Company, "");
                }
                else if (Type.ToLower().Equals("vehicle maintanace"))
                {
                    Acc_Data.Accounts_Update1(Acc_Data.G_Type_Vehicle, ID, Company, "");
                }
                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        [HttpGet]
        public string get_os_po_bills(string ID, string Company)
        {

            string Query = " select cus_contact_number as Phone_Number,cus_name as Ledger_Name,Bill_No as label, " +
                " Bill_Amount as Net_Amt,Bal_Amount as Bill_Amount,Bill_No as value,Bill_No as BillNo,dbo.Date_(Bill_Date) as Bill_Date,Voucher_Type as Bill_Type, " +
                " Due_Date,datediff(day, cast(dbo.date_(Due_Date) as datetime),cast(getdate() as datetime)) as days, " +
                " 'Bill No : '+Bill_No as Narration from Amount_Balance left outer join Ledger_Master on cus_id=Ledger_id " +
                " where convert(Varchar,Bill_Date,112)<='" + DateTime.Parse(GITAPI.dbFunctions.getdate()).ToString("yyyyMMdd") + "' and Ledger_id=" + ID + " " +
                " and Bal_Amount!=0 order by   cast(dbo.date_(Bill_Date) as datetime) ";
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }



        [HttpGet]
        public string get_Outstanding_Payment(string Date, string Customer, string Area, string order_by, string Company)
        {

            string condi = "";

            //condi += " and convert(Varchar,y.cb_billdate,112)<='" + DateTime.Parse(Date).ToString("yyyyMMdd") + "'";


            if (Customer.ToLower() != "0")
            {
                condi += " and   y.cb_ledger_id='" + Customer + "'";
            }
            if (Area.ToLower() != "all")
            {
                condi += " and   l.cus_city='" + Area + "'";
            }
            //string q = "select isnull(y.cb_billno,'') as Bill_No, '' as Bill_Date, l.cus_id as Ledger_ID,l.cus_city as Area,  l.cus_name as Customer_Name,l.cus_contactno as Contact_No,sum(y.cb_amountin-y.cb_amountout) as Bill_Amount, sum(y.cb_amountin-y.cb_amountout) as Amount, '' as Due_Date_,0 as Due_Days  from Ledger_Master l left outer join Balance y on l.cus_id=y.cb_ledger_id where 0=0 " + condi + "  group by  y.cb_billno,l.cus_id,l.cus_name,l.cus_city,l.cus_contactno"; //having sum(y.cb_amountin-y.cb_amountout)>0";


            string q = "SELECT cus_id as Customer_ID,cus_name as Customer_Name,cus_contactno as Phone_No,cus_address1 as Street,cus_city as Area,0 as count,0 as Amount FROM Ledger_Master where cus_status = 'A'";
            DataTable dt = GITAPI.dbFunctions.getTable(q);


            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_ledger_his(string From, string To, string ID, string Company)
        {


            //string Query = "select cb_pay_mode as Mode,dbo.date_(cb_date) as Rec_Date, b.cb_created_by as Rec_user , dbo.date_(cb_billdate) as Bill_Date, (datediff(day,cast(dbo.date_(cb_billdate) as datetime), cast(GETDATE() as datetime))) as days,* from Balance b " +
            //    " where b.cb_ledger_id=" + ID + " and convert(Varchar,cb_billdate,112)>='" + System.DateTime.Parse(From).ToString("yyyyMMdd") + "'  and  convert(Varchar,cb_billdate,112)<='" + System.DateTime.Parse(To).ToString("yyyyMMdd") + "' order by cb_billdate desc ,cb_billno,cb_vtype desc ";

            ////string q = "select  BillNo as label, sum(DBAmt-CRAMT) as Bill_Amount, BillNo as [value] from bills_out" + Company + " x where   ledger_id=" + ID + " group by BillNo having  sum(DBAmt-CRAMT)!=0 order by BillNo";
            //DataTable dt = GITAPI.dbFunctions.getTable(Query);
            //string data = GITAPI.dbFunctions.GetJSONString(dt);
            return "[]";
        }


        [HttpGet]
        public string get_Paid_Details(string From, string To, string Bank, string Company)
        {

            string condi = "";

            //condi += " and convert(Varchar,y.cb_billdate,112)<='" + DateTime.Parse(Date).ToString("yyyyMMdd") + "'";


            if (Bank.ToLower() != "0")
            {
                condi += " and   x.cb_received_bank='" + Bank + "' ";
            }
            if (Company.ToLower() != "0")
            {
                condi += " and   x.cb_company='" + Company.Replace("_", "") + "' ";
            }


            string Query = " select x.cb_id as ID,x.cb_uniqno as Receipt_No,x.cb_billno as Bill_No,dbo.Date_(x.cb_date) as AC_Date,y.cus_name as Name, " +
                " y.cus_city as Area,est_projectname as cb_project,cb_amountout as  Amount, Account_Number as cb_received_bank,x.*  from Balance x " +
                " left outer join Ledger_Master y on x.cb_ledger_id=y.cus_id " +
                " left outer join Estimation  on est_id=x.cb_project_id " +
                " left outer join bank_master on ID=cb_received_bank " +
                " where cb_vtype='Payment'  " +
                "and  convert(Varchar,x.cb_date,112)>='" + System.DateTime.Parse(From).ToString("yyyyMMdd") + "' " +
                "and  convert(Varchar,x.cb_date,112)<='" + System.DateTime.Parse(To).ToString("yyyyMMdd") + "' " + condi + " order by convert(Varchar,x.cb_date,112),cb_id desc";

            DataTable dt1 = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt1);
            return data;
        }

        [HttpGet]
        public string get_Ledger_Paid_Details(string From, string To, string Ledger, string Company)
        {
            string Query = " select x.cb_id as ID,x.cb_uniqno as Receipt_No,x.cb_billno as Bill_No,dbo.Date_(x.cb_date) as AC_Date,y.cus_name as Name, " +
                " y.cus_city as Area,cb_amountout as  Amount, cb_pay_mode as Pay_Mode_,cb_disc as Cash_Disc  from Balance x " +
                " left outer join  Ledger_Master y on x.cb_ledger_id=y.cus_id where cb_vtype='Payment'  and cb_ledger_id='" + Ledger + "'" +
                "and  convert(Varchar,x.cb_date,112)>='" + System.DateTime.Parse(From).ToString("yyyyMMdd") + "' " +
                "and  convert(Varchar,x.cb_date,112)<='" + System.DateTime.Parse(To).ToString("yyyyMMdd") + "' order by convert(Varchar,x.cb_date,112) desc";

            DataTable dt1 = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt1);
            return data;
        }


        [HttpGet]
        public string delete_Paid_Amount(string ID, string UserName, string Company)
        {
            DataTable dc1 = GITAPI.dbFunctions.getTable("select cb_ledger_id from balance  where cb_id=" + ID);
            string q = "insert into balance_Delete select '" + UserName + "','" + GITAPI.dbFunctions.getdate() + "',* from balance where cb_id=" + ID;
            q += " delete from balance where cb_id=" + ID +
            " delete from daybook where db_vour_refno=" + ID + " and db_vourtype='" + Acc_Data.G_Type_Payment + "' ";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            Acc_Data.Balance_Update(dc1.Rows[0][0].ToString(), "", Company);
            return "True";
        }

        public string insert_Payment(JObject jsonData)
        {
            dynamic json = jsonData;
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");
            string ID = isnull(json.ID, "0");
            string User = isnull(json.Created_by, "");
            string uniqno = isnull(json.cb_uniqno, "");
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

                    uniqno = get_Receipt_No1(Company);
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
                    else if (Column.ToLower().Equals(ColumnPerfix + "uniqno"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = uniqno;
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
                //DataTable dc1 = GITAPI.dbFunctions.getTable("select oc_id,oc_vtype from Other_Collection where oc_no='" + oc_no + "'");
                //if (dc1.Rows.Count > 0)
                //{
                //    ID = dc1.Rows[0][0].ToString();
                //}
                Acc_Data.Accounts_Update1(Acc_Data.G_Type_Payment, uniqno, Company, "");
                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpGet]
        public string get_Transport_EntryNO(string Type, string Company)
        {
            try
            {
                string Query = "";
                Query += "declare @digit int ";
                Query += "declare @Prefix varchar(33) ";
                Query += "declare @suffix varchar(33) ";
                Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='" + Type + "' ";

                Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(tpt_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Transport_Entry ";
                Query += "where left(tpt_no,len(@Prefix))=@Prefix ";
                Query += "and right(tpt_no,len(@suffix))=@suffix ";

                DataTable dt = GITAPI.dbFunctions.getTable(Query);

                return dt.Rows[0][0].ToString();
            }
            catch { return ""; }
        }

        [HttpGet]
        public string delete_Transport_Entry(string Purchase_No, string Company)
        {
            string q = "insert into Transport_Entry_Delete " +
                " select * from Transport_Entry where tpt_no = '" + Purchase_No + "' " +
                " delete from Transport_Entry where tpt_no = '" + Purchase_No + "' ";
            //DataTable dt4c = GITAPI.dbFunctions.getTable("delete from Balance  where  cb_billno='" + Purchase_No + "' ");
            DataTable dd2 = GITAPI.dbFunctions.getTable(q);

            return "True";
        }

        [HttpGet]
        public string get_Transport_Entry_details(string From, string To, string Type, string Branch_ID, string Company)
        {
            string Query = "";
            string condi = "";
            if (!Type.ToLower().Equals("all"))
            {
                condi += " and tpt_type='" + Type + "' ";
            }
            if (!Branch_ID.ToLower().Equals("0"))
            {
                condi += " and  tpt_branch='" + Branch_ID + "' ";
            }
            condi += " and convert(Varchar,tpt_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,tpt_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";
            condi += " order by Convert(Varchar,tpt_date,112) asc";
            Query += "select dbo.date_(tpt_date) as [tpt_date],* from Transport_Entry where  0=0" + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }
        public string Post_Transport_Entry(JObject jsonData)
        {
            dynamic json = jsonData;
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");
            string ID = isnull(json.tpt_id, "0");
            string User = isnull(json.Created_by, "");
            string ColumnPerfix = isnull(json.ColumnPerfix, "");
            string Type = isnull(json.tpt_type, "0");
            string tpt_no = isnull(json.tpt_no, "");
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

                    tpt_no = get_Transport_EntryNO(Type, Company);
                    json["tpt_no"] = tpt_no.ToString();
                    if (isIDn == false)
                    {
                        DataTable dd = GITAPI.dbFunctions.getTable("select isnull(max(" + dt.Rows[0]["Column_Name"].ToString() + "),0)+1 from " + Table_Name);
                        if (dd.Rows.Count > 0)
                        {
                            json[dt.Rows[0]["Column_Name"].ToString()] = dd.Rows[0][0].ToString();
                            ID = dd.Rows[0][0].ToString();
                        }
                    }


                    Query += " if not exists (select 1 from " + Table_Name + " where tpt_type=@tpt_type and tpt_date=@tpt_date and tpt_projectid=@tpt_projectid and tpt_projectno=@tpt_projectno and tpt_projectname=@tpt_projectname and tpt_contact_no=@tpt_contact_no and tpt_ledger_id=@tpt_ledger_id and tpt_ledger_name=@tpt_ledger_name and tpt_ledger_address1=@tpt_ledger_address1 and tpt_ledger_address2=@tpt_ledger_address2 and tpt_ledger_address3=@tpt_ledger_address3 and tpt_gst_no=@tpt_gst_no and tpt_transid=@tpt_transid and tpt_transport=@tpt_transport and tpt_tpttype=@tpt_tpttype and tpt_fromid=@tpt_fromid and tpt_from=@tpt_from and tpt_toid=@tpt_toid and tpt_to=@tpt_to and tpt_material_name=@tpt_material_name and tpt_ttype=@tpt_ttype and tpt_load=@tpt_load and tpt_opening=@tpt_opening and tpt_closing=@tpt_closing and tpt_tothours=@tpt_tothours and tpt_amount=@tpt_amount and tpt_remarks=@tpt_remarks and tpt_narration1=@tpt_narration1 and tpt_narration2=@tpt_narration2 and tpt_narration3=@tpt_narration3) begin ";
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

                    Query += " ) end " +
                        " else " +
                        " begin " +
                        " raiserror('Already Entered ,Try New', 16, -1, '') " +
                        " end ";


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
                //DataTable dc1 = GITAPI.dbFunctions.getTable("select mm_id from Other_Collection where oc_no='" + oc_no + "'");
                //if (dc1.Rows.Count > 0)
                //{
                //    ID = dc1.Rows[0][0].ToString();
                //}

                //Acc_Data.Accounts_Update(Acc_Data.G_Type_Expenses, ID, Company, Type);
                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        [HttpGet]
        public string get_CR_DB_Amount(string Company, string Ledger_ID)
        {

            string Query = "select  isnull(sum(db_cramt1-db_dbamt1),0) from daybook  where  db_received_bank='" + Ledger_ID + "' and db_branch_id='" + Company.Replace("_", "") + "' ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            try
            {
                return dt.Rows[0][0].ToString();
            }
            catch
            {
                return "0";
            }

        }

        [HttpGet]
        public string delete_Contra(string ID, string UserName, string Company)
        {
            string q = "insert into Contra_Delete " +
                " select '" + UserName + "','" + GITAPI.dbFunctions.getdate() + "',* from Contra where c_id='" + ID + "' " +
                " delete from DayBook  where  db_vour_refno='" + ID + "' and db_vourtype='Contra' " +
                " delete from Contra where c_id='" + ID + "' ";
            DataTable dd2 = GITAPI.dbFunctions.getTable(q);

            return "True";
        }

        [HttpGet]
        public string get_Contra(string From, string To, string Pay_Mode, string Branch_ID, string Company)
        {
            string Condi = "";


            if (!(Pay_Mode == "" || Pay_Mode == null || Pay_Mode == "0" || Pay_Mode == "All"))
            {
                Condi += "and c_pay_mode='" + Pay_Mode + "' ";
            }

            if (!(Branch_ID.ToLower().Equals("0")))
            {
                Condi += "and c_company='" + Branch_ID + "' ";
            }
            //Condi += " order by convert(varchar,c_ref_date,112) asc ";

            string Query = "select " +
                " c.c_id as ID, " +
                " c.c_ref_no as Ref_No,c_from_account as From_Account,c_to_account as To_Account, " +
                " b1.Bank_Name+'-'+b1.Account_Number as  From_Account_, " +
                " b2.Bank_Name+'-'+b2.Account_Number as To_Account_, " +
                " c_naration as Naration, " +
                " c.c_amount as Amount, " +
                " c.c_pay_mode as Pay_Mode, " +
                " dbo.get_ref_value(c_pay_mode) as Pay_Mode_ , " +
                " c_received_bank as Received_Bank, " +
                " c_cheque_no as Cheque_No, " +
                " c_cheque_date as Cheque_Date,c_card_charge as Card_Charge, " +
                " c_remarks as Remarks, " +
                " dbo.Date_(c.c_ref_date) as Ref_Date, " +
                " dbo.Date_(c.c_ref_date) as Ref_Date_, " +
                 " dbo.Time_(c.c_created_date) as Time_, " +
                " c.c_created_by " +
                "  from Contra c left outer join  Bank_Master b1 on c_from_account=b1.ID   left outer join  Bank_Master b2 on c_to_account=b2.ID  where convert(varchar,c_Ref_Date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "'" +
                " and convert(varchar,c_Ref_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'" +
                " " + Condi + " " +
                " order by convert(varchar,c_ref_date,112) asc ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpPost]
        public string insert_Contra(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.ID, "");
            string Ref_No = isnull(json.Ref_No, "");
            string Ref_Date = isnull(json.Ref_Date, "");
            string From_Account = isnull(json.From_Account, "");
            string To_Account = isnull(json.To_Account, "");
            string Received_Bank = isnull(json.Received_Bank, "");

            string Naration = isnull(json.Naration, "");
            string Amount = isnull(json.Amount, "0");
            string Pay_Mode = isnull(json.Pay_Mode, "");
            string Cheque_No = isnull(json.Cheque_No, "");
            string Cheque_Date = isnull(json.Cheque_Date, "");
            string Remarks = isnull(json.Remarks, "");

            string Card_Charge = isnull(json.Card_Charge, "0");
            string Created_by = isnull(json.Created_by, "");
            string Created_Date = isnull(json.Created_Date, "");
            string Status = isnull(json.Status, "");
            string Company = isnull(json.Branch_ID, "");

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {

                    com.CommandText = "insert into Contra (c_card_charge,c_ref_no,c_ref_date,c_from_account,c_to_account,c_naration,c_amount,c_pay_mode,c_received_bank,c_cheque_no,c_cheque_date,c_remarks,c_created_by,c_created_date,c_status,c_company) Values (@Card_Charge,@Ref_No,@Ref_Date,@From_Account,@To_Account,@Naration,@Amount,@Pay_Mode,@Received_Bank,@Cheque_No,@Cheque_Date,@Remarks,@Created_by,'" + GITAPI.dbFunctions.getdate() + "', 'A',@c_company  )";
                }
                else
                {
                    com.CommandText = "update Contra Set c_card_charge=@Card_Charge, c_ref_no=@Ref_No, c_ref_date=@Ref_Date, c_from_account=@From_Account, c_to_account=@To_Account, c_naration=@Naration,  c_amount=@Amount, c_pay_mode=@Pay_Mode, c_received_bank=@Received_Bank, c_cheque_no=@Cheque_No, c_cheque_date=@Cheque_Date, c_remarks=@Remarks, c_del_by=@Created_by, c_del_date='" + GITAPI.dbFunctions.getdate() + "' where  c_id=@ID ";
                    com.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
                }


                com.Parameters.Add("@Card_Charge", SqlDbType.VarChar).Value = Card_Charge;


                com.Parameters.Add("@Ref_No", SqlDbType.VarChar).Value = Ref_No;
                com.Parameters.Add("@Ref_Date", SqlDbType.VarChar).Value = Ref_Date;
                com.Parameters.Add("@From_Account", SqlDbType.VarChar).Value = From_Account;
                com.Parameters.Add("@To_Account", SqlDbType.VarChar).Value = To_Account;
                com.Parameters.Add("@Naration", SqlDbType.VarChar).Value = Naration;
                com.Parameters.Add("@Amount", SqlDbType.VarChar).Value = Amount;
                com.Parameters.Add("@Pay_Mode", SqlDbType.VarChar).Value = Pay_Mode;
                com.Parameters.Add("@Received_Bank", SqlDbType.VarChar).Value = Received_Bank;
                com.Parameters.Add("@Cheque_No", SqlDbType.VarChar).Value = Cheque_No;
                com.Parameters.Add("@Cheque_Date", SqlDbType.VarChar).Value = Cheque_Date;
                com.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = Remarks;
                com.Parameters.Add("@Created_by", SqlDbType.VarChar).Value = Created_by;
                com.Parameters.Add("@c_company", SqlDbType.VarChar).Value = Company;
                com.ExecuteNonQuery();
                com.Connection.Close();


                DataTable dts = GITAPI.dbFunctions.getTable("select c_id  from  Contra  where c_ref_no='" + Ref_No + "'");
                ID = dts.Rows[0][0].ToString();
                Acc_Data.Accounts_Update1(Acc_Data.G_Type_Contra, ID, Company, "");


                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string Upload2b_Master(string ID, string Name, string Company, string Description)
        {
            try
            {
                var exMessage = string.Empty;

                // Check if any file is uploaded
                if (System.Web.HttpContext.Current.Request.Files.Count == 0)
                {
                    return "No files uploaded.";
                }

                string uploadPath = "~/Image/";
                System.Web.HttpPostedFile file = null;

                for (int i = 0; i < System.Web.HttpContext.Current.Request.Files.Count; i++)
                {
                    file = System.Web.HttpContext.Current.Request.Files[i];

                    if (file == null || file.ContentLength <= 0)
                    {
                        return "File not found or empty.";
                    }

                    if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(uploadPath)))
                    {
                        Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(uploadPath));
                    }

                    file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(uploadPath + "/Item_data.xlsx"));
                }

                string xslPath = System.Web.HttpContext.Current.Server.MapPath(uploadPath + "/Item_data.xlsx");
                Error = "";

                DataTable items = Import_To_Grid(xslPath, ".xlsx", "yes");

                if (!string.IsNullOrEmpty(Error))
                {
                    return Error;
                }

                string Table_Name = "GSTR2A_Data";
                string query2 = "INSERT INTO GSTR2A_Data (Ref_Date,Ref_ID,GSTIN,LedgerName,InvoiceNumber,InvoiceDate,InvoiceValue, GST,TaxableValue,IGST,CGST,SGST,Remarks,Created_by,Created_date,Status,Company) " +
                    " SELECT distinct '" + GITAPI.dbFunctions.getdate() + "',t1.#,t1.GSTIN,t1.Name,t1.[Invoice Number], CONVERT(VARCHAR(10), CONVERT(date, t1.[Invoice Date], 105), 112), Replace(t1.[Invoice Value],',',''),t1.GST,Replace(t1.[Taxable Value],',',''),Replace(Replace(t1.IGST,'-','0'),',',''),Replace(Replace(t1.CGST,'-','0'),',',''),Replace(Replace(t1.SGST,'-','0'),',',''),t1.Remarks, 'ADMIN',  " +
                    " '" + GITAPI.dbFunctions.getdate() + "','A','" + Company.Replace("_", "") + "' FROM GSTR2A_TempData t1 LEFT JOIN GSTR2A_Data t2 ON t2.GSTIN=t1.GSTIN and t2.LedgerName=t1.Name and t2.InvoiceNumber = t1.[Invoice Number] and " +
                    " Convert(varchar,t2.InvoiceDate,112)=CONVERT(VARCHAR(10), CONVERT(date, t1.[Invoice Date], 105), 112) and t2.InvoiceValue=Replace(t1.[Invoice Value],',','') and t2.GST=t1.GST  and t2.TaxableValue=Replace(t1.[Taxable Value],',','')  and t2.IGST=Replace(Replace(t1.IGST,'-','0'),',','') and t2.CGST=Replace(Replace(t1.CGST,'-','0'),',','') and t2.SGST=Replace(Replace(t1.SGST,'-','0'),',','') WHERE t2.InvoiceNumber IS NULL";

                if (items.Rows.Count > 0)
                {
                    GITAPI.dbFunctions.getTable("truncate table GSTR2A_TempData");
                    bulkinsert(items, "GSTR2A_TempData");
                    GITAPI.dbFunctions.getTable(query2);
                }

                return Error;
            }
            catch (Exception ex)
            {
                dbFunctions.Logs(ex.Message, "ADMIN");
                return ex.Message;
            }
        }

        private DataTable Import_To_Grid(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";

            switch (Extension)
            {
                case ".xls":
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    break;
                case ".xlsx":
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\"";
                    break;
            }

            DataTable dt = new DataTable();

            try
            {
                conStr = String.Format(conStr, FilePath, isHDR);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;

                connExcel.Open();
                DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dtExcelSchema.Rows.Count > 0)
                {
                    string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                    oda.SelectCommand = cmdExcel;
                    oda.Fill(dt);
                }

                connExcel.Close();

                if (dt.Rows.Count > 0)
                {
                    dt.Rows.RemoveAt(0);
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                dbFunctions.Logs(ex.Message, "ADMIN");
            }

            return dt;
        }


        [HttpPost]
        public string Upload2b_Master2(string ID, string Name, string Company, string Description)
        {

            try
            {
                var exMessage = string.Empty;
                System.Web.HttpPostedFile myFile = System.Web.HttpContext.Current.Request.Files[0];


                string uploadPath = "~/Image/";
                System.Web.HttpPostedFile file = null;
                for (int i = 0; i < System.Web.HttpContext.Current.Request.Files.Count; i++)
                {
                    if (System.Web.HttpContext.Current.Request.Files.Count > 0)
                    {
                        file = System.Web.HttpContext.Current.Request.Files[i];
                    }

                    if (null == file)
                    {
                        return " file not found";
                    }

                    // Make sure the file has content
                    if (!(file.ContentLength > 0))
                    {
                        return " file not found";
                    }

                    if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(uploadPath)))
                    {
                        Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(uploadPath));
                    }

                    file.SaveAs(System.Web.HttpContext.Current.Server.MapPath(uploadPath + "/Item_data.xlsx"));

                }
                string xslPath = System.Web.HttpContext.Current.Server.MapPath(uploadPath + "/Item_data.xlsx");

                Error = "";
                //string query = "SELECT * from [" + "Sheet1" + "$]";
                //DataTable items = getFromExcel(xslPath, query);


                //string query = "SELECT * from [" + GetSheetNames(xslPath, ".xlsx") + "]";

                if (Error != "")
                {
                    return Error;
                }

                Error = "";
                DataTable items = Import_To_Grid(xslPath, ".xlsx", "yes");//getFromExcel(xslPath, query);


                if (Error != "")
                {
                    return Error;
                }
                string Table_Name = "GSTR2A_Data";
                string query2 = "INSERT INTO GSTR2A_Data (Ref_Date,Ref_ID,GSTIN,LedgerName,InvoiceNumber,InvoiceDate,InvoiceValue, GST,TaxableValue,IGST,CGST,SGST,Remarks,Created_by,Created_date,Status,Company) " +
                    " SELECT distinct '" + GITAPI.dbFunctions.getdate() + "',t1.#,t1.GSTIN,t1.Name,t1.[Invoice Number], CONVERT(VARCHAR(10), CONVERT(date, t1.[Invoice Date], 105), 112), Replace(t1.[Invoice Value],',',''),t1.GST,Replace(t1.[Taxable Value],',',''),Replace(Replace(t1.IGST,'-','0'),',',''),Replace(Replace(t1.CGST,'-','0'),',',''),Replace(Replace(t1.SGST,'-','0'),',',''),t1.Remarks, 'ADMIN',  " +
                    " '" + GITAPI.dbFunctions.getdate() + "','A','" + Company.Replace("_", "") + "' FROM GSTR2A_TempData t1 LEFT JOIN GSTR2A_Data t2 ON t2.GSTIN=t1.GSTIN and t2.LedgerName=t1.Name and t2.InvoiceNumber = t1.[Invoice Number] and " +
                    " Convert(varchar,t2.InvoiceDate,112)=CONVERT(VARCHAR(10), CONVERT(date, t1.[Invoice Date], 105), 112) and t2.InvoiceValue=Replace(t1.[Invoice Value],',','') and t2.GST=t1.GST  and t2.TaxableValue=Replace(t1.[Taxable Value],',','')  and t2.IGST=Replace(Replace(t1.IGST,'-','0'),',','') and t2.CGST=Replace(Replace(t1.CGST,'-','0'),',','') and t2.SGST=Replace(Replace(t1.SGST,'-','0'),',','') WHERE t2.InvoiceNumber IS NULL";
                if (items.Rows.Count > 0)
                {
                    GITAPI.dbFunctions.getTable("truncate table GSTR2A_TempData");
                    bulkinsert(items, "GSTR2A_TempData");
                    GITAPI.dbFunctions.getTable(query2);
                }


                //bulkinsert(items, "GSTR2A_TempData");
                //for (int i = 0; i < items.Rows.Count; i++)
                //{

                //    SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
                //    try
                //    {
                //        con.Open();
                //        SqlCommand com = new SqlCommand();
                //        com.Connection = con;
                //        com.CommandType = CommandType.Text;
                //        com.CommandText = " if not exists (select 1 from GSTR2A_Data where GSTIN=@GSTIN and LedgerName=@LedgerName and InvoiceNumber=@InvoiceNumber and InvoiceDate=@InvoiceDate and InvoiceValue=@InvoiceValue and GST=@GST and TaxableValue=@TaxableValue and IGST=@IGST and CGST=@CGST and SGST=@SGST) " +
                //        " begin " +
                //        " insert into GSTR2A_Data (Ref_Date,Ref_ID,GSTIN,LedgerName,InvoiceNumber,InvoiceDate,InvoiceValue,GST,TaxableValue,IGST,CGST,SGST,Remarks,Created_by,Created_date,Status) Values ( @Ref_Date,@Ref_ID,@GSTIN,@LedgerName,@InvoiceNumber,@InvoiceDate,@InvoiceValue,@GST,@TaxableValue,@IGST,@CGST,@SGST,@Remarks,@Created_by,@Created_date,@Status )" +
                //        " end ";
                //        com.Parameters.Add("@Ref_Date", SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                //        com.Parameters.Add("@Ref_ID", SqlDbType.VarChar).Value = i + 1;
                //        try
                //        {
                //            com.Parameters.Add("@GSTIN", SqlDbType.VarChar).Value = items.Rows[i]["GSTIN"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@GSTIN", SqlDbType.VarChar).Value = "";
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@LedgerName", SqlDbType.VarChar).Value = items.Rows[i]["Name"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@LedgerName", SqlDbType.VarChar).Value = "";
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@InvoiceNumber", SqlDbType.VarChar).Value = items.Rows[i]["Invoice Number"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@InvoiceNumber", SqlDbType.VarChar).Value = "";
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@InvoiceDate", SqlDbType.VarChar).Value = items.Rows[i]["Invoice Date"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@InvoiceDate", SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@InvoiceValue", SqlDbType.VarChar).Value = items.Rows[i]["Invoice Value"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@InvoiceValue", SqlDbType.VarChar).Value = "0";
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@GST", SqlDbType.VarChar).Value = items.Rows[i]["GST"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@GST", SqlDbType.VarChar).Value = "0";
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@TaxableValue", SqlDbType.VarChar).Value = items.Rows[i]["Taxable Value"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@TaxableValue", SqlDbType.VarChar).Value = "0";
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@IGST", SqlDbType.VarChar).Value = items.Rows[i]["IGST"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@IGST", SqlDbType.VarChar).Value = "0";
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@CGST", SqlDbType.VarChar).Value = items.Rows[i]["CGST"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@CGST", SqlDbType.VarChar).Value = "0";
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@SGST", SqlDbType.VarChar).Value = items.Rows[i]["SGST"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@SGST", SqlDbType.VarChar).Value = "0";
                //        }
                //        try
                //        {
                //            com.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = items.Rows[i]["Remarks"].ToString();
                //        }
                //        catch
                //        {
                //            com.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = "";
                //        }
                //        com.Parameters.Add("@Created_by", SqlDbType.VarChar).Value = "ADMIN";
                //        com.Parameters.Add("@Created_date", SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();
                //        com.Parameters.Add("@Status", SqlDbType.VarChar).Value = "A";
                //        com.ExecuteNonQuery();
                //        com.Connection.Close();
                //    }
                //    catch (Exception ex)
                //    {
                //        dbFunctions.Logs(ex.Message, "ADMIN");
                //        Error += ex.Message;
                //        //return ex.Message;
                //    }

                //}

                return Error;
            }
            catch (Exception ex)
            {
                dbFunctions.Logs(ex.Message, "ADMIN");
                return ex.Message;
            }

        }

        public void bulkinsert(DataTable dt, string Table_Name)
        {
            try
            {
                SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
                //create object of SqlBulkCopy which help to insert
                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                //assign Destination table name
                objbulk.DestinationTableName = Table_Name;
                //objbulk.ColumnMappings = GetDatabaseList1();

                con.Open();
                //insert bulk Records into DataBase.
                objbulk.WriteToServer(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                dbFunctions.Logs(ex.Message, "ADMIN");
            }
        }

        public string Error = "";
        private DataTable getFromExcel(string ExcelPath, string query)
        {
            Error = "";
            DataTable dt = new DataTable();
            try
            {

                DataColumn col = dt.Columns.Add("RowNumber", typeof(int));
                col.AutoIncrementSeed = 1;
                col.AutoIncrement = true;

                //string cs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelPath + ";Extended Properties=Excel 12.0;";
                string cs = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                //cs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";

                OleDbConnection con = new OleDbConnection(cs);

                OleDbCommand cmd = new OleDbCommand(query, con);
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);

            }
            catch (Exception ex)
            {

                Error = ex.Message;
                dbFunctions.Logs(ex.Message, "ADMIN");
            }
            return dt;
        }


        public static string GetSheetNames(string path, string Extension)
        {
            try
            {
                List<string> sheets = new List<string>();
                string connectionString = "";
                switch (Extension)
                {
                    case ".xls": //Excel 97-03
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                        break;
                    case ".xlsx": //Excel 07
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                        break;
                }
                DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connectionString;
                connection.Open();
                DataTable tbl = connection.GetSchema("Tables");
                connection.Close();


                foreach (DataRow row in tbl.Rows)
                {
                    string sheetName = (string)row["TABLE_NAME"];
                    if (sheetName.EndsWith("$"))
                    {
                        sheetName = sheetName.Substring(0, sheetName.Length - 1);
                    }
                    return sheetName;
                }
                return "";
            }
            catch (Exception ex)
            {

                dbFunctions.Logs(ex.Message, "ADMIN");
                //        MessageBox.Show("Excel File Not Loaded, Please Load Correct Excel", "EDI Process - Larch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ex.Message;
            }

        }




        private DataTable Import_To_Grid2(string FilePath, string Extension, string isHDR)
        {
            string conStr = "";
            switch (Extension)
            {
                case ".xls": //Excel 97-03
                    conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    break;
                case ".xlsx": //Excel 07
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\"";
                    break;
            }


            DataTable dt = new DataTable();
            try
            {
                conStr = String.Format(conStr, FilePath, isHDR);
                OleDbConnection connExcel = new OleDbConnection(conStr);
                OleDbCommand cmdExcel = new OleDbCommand();
                OleDbDataAdapter oda = new OleDbDataAdapter();
                cmdExcel.Connection = connExcel;

                //Get the name of First Sheet
                connExcel.Open();
                DataTable dtExcelSchema;
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

                cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";


                oda.SelectCommand = cmdExcel;
                oda.Fill(dt);
                connExcel.Close();
                dt.Rows.RemoveAt(0);
            }
            catch (Exception ex)
            {

                dbFunctions.Logs(ex.Message, "ADMIN");
            }
            return dt;
        }


        [HttpGet]
        public string get_GST2b(string From, string To, string Company)
        {
            string Condi = "";

            if (!Company.ToLower().Equals("0"))
            {
                Condi += " and Company='" + Company.Replace("_", "") + "' ";
            }

            string Query = "select " +
                " dbo.Date_(InvoiceDate) as InvoiceDate,isnull(InvoiceValue,0) as InvoiceValue,* " +
                "  from GSTR2A_Data  where convert(varchar,InvoiceDate,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' " +
                " and convert(varchar,InvoiceDate,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "' " + Condi +
                " and isnull(InvoiceValue,0)!=0  order by convert(varchar,InvoiceDate,112) asc";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Closing_No(string ID, string Company)
        {

            string Query = "select max(lbe_closing_km) as Closing,convert(varchar,max(lbe_date),104) as date from Log_Book_Entry where lbe_status='A' and lbe_vehicle_id='" + ID + "' ";

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

        [HttpGet]
        public string delete_Log_Book_Entry(string ID, string UserName, string Company)
        {
            string q = "UPDATE Log_Book_Entry SET lbe_status='D',lbe_del_date='" + GITAPI.dbFunctions.getdate() + "',lbe_del_by='" + UserName + "'  where lbe_id=" + ID;
            GITAPI.dbFunctions.getTable(q);
            return "True";
        }


        [HttpGet]
        public string get_Log_Book_Entry(string From, string To, string Order_by, string Branch_ID, string Company = "0")
        {
            string condi = "";
            condi += " and convert(Varchar,lbe_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,lbe_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ";

            condi += " and lbe_status='A' ";
            if (!Branch_ID.Equals("0"))
            {
                condi += " and lbe_branch='" + Branch_ID + "' ";
            }
            if (!Order_by.Equals(""))
            {
                condi += " Order by " + Order_by;
            }
            string q = "select dbo.Date_(lbe_date) as lbe_date,* from Log_Book_Entry where 0=0 " + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        [HttpPost]
        public string Post_Log_Book_Entry(JObject jsonData)
        {
            dynamic json = jsonData;
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");
            string ID = isnull(json.ID, "0");
            string Created_by = isnull(json.Created_by, "");
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
                    if (Column.ToLower().Equals(ColumnPerfix + "id"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = ID;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "created_date"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();

                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = "A";
                    }

                    else if (Column.ToLower().Equals(ColumnPerfix + "created_by"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Created_by;

                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "del_by"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Created_by;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "del_date"))
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
        //bunk_entry

        public string Post_Bunk_Entry(JObject jsonData)
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
        public string get_Bunk_No(string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='bunk' ";

            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(be_voucherno,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Bunk_Entry ";
            Query += "where left(be_voucherno,len(@Prefix))=@Prefix ";
            Query += "and right(be_voucherno,len(@suffix))=@suffix ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }


        [HttpGet]
        public string get_Bunk_Detail(string From, string To, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,x.be_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.be_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            //if (User.ToLower() != "all")
            //{
            //    condi += " and  x.be_created_by='" + User + "'";
            //}

            condi += " and be_type='Bunk'";
            string q = ("select be_id,dbo.Date_(be_date) as be_date,be_voucherno,be_product as be_product,be_category_id,be_category as be_category,be_narration1 as be_narration1,be_ledger_id,be_ledger_name as be_ledger_name,be_projectid,est_projectname as be_projectname,be_qty as be_qty,isnull(be_km,0) as be_km,be_rate as be_rate ,Convert(decimal(18,2),be_amount) as be_amount,be_created_by as be_created_by from  Bunk_Entry x  " +
                 " LEFT OUTER JOIN Estimation on be_projectid = est_id " + " where 0=0 " + condi + " order by be_date ASC");


            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }

        [HttpGet]
        public string delete_bunk(string ID, string UserName, string Company)
        {
            string q = "insert into Bunk_delete " +
                " select '" + UserName + "','" + GITAPI.dbFunctions.getdate() + "',* from Bunk_Entry where be_id='" + ID +
                "' delete from Bunk_Entry where be_id='" + ID + "'";
            DataTable d = GITAPI.dbFunctions.getTable(q);

            return "True";
        }

        // Daily_bunk_entry

        [HttpGet]
        public string delete_Daily_Bunk_Entry(string ID, string UserName, string Company)
        {
            string q = "UPDATE Daily_Bunk_Entry SET dbe_status='D',dbe_del_date='" + GITAPI.dbFunctions.getdate() + "',dbe_del_by='" + UserName + "'  where dbe_id=" + ID;
            GITAPI.dbFunctions.getTable(q);
            return "True";
        }




        [HttpGet]
        public string get_Daily_Bunk_Entry_Details(string From, string To, string Order_by, string Company = "0")
        {
            string condi = "";
            condi += " and convert(Varchar,dbe_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,dbe_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ";

            condi += " and dbe_status='A' ";
            if (!Order_by.Equals(""))
            {
                condi += " Order by " + Order_by;
            }
            string q = "select dbo.Date_(dbe_date) as dbe_date,* from Daily_Bunk_Entry where 0=0 " + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }


        [HttpPost]
        public string Post_Daily_Bunk_Entry(JObject jsonData)
        {
            dynamic json = jsonData;
            string Company = isnull(json.Company, "");
            string Table_Name = isnull(json.Table_Name, "");
            string ID = isnull(json.ID, "0");
            string Created_by = isnull(json.Created_by, "");
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
                    if (Column.ToLower().Equals(ColumnPerfix + "id"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = ID;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "created_date"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = GITAPI.dbFunctions.getdate();

                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "status"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = "A";
                    }

                    else if (Column.ToLower().Equals(ColumnPerfix + "created_by"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Created_by;

                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "branch"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Company.Replace("_", "");
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "del_by"))
                    {
                        com.Parameters.Add("@" + Column, SqlDbType.VarChar).Value = Created_by;
                    }
                    else if (Column.ToLower().Equals(ColumnPerfix + "del_date"))
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

        // Daily_BunK_NO
        public string get_Bunk_No1(string BunkNo, string Company)
        {

            string Query = "select max(dbe_bunk_closing) as Closing,convert(varchar,max(dbe_date),104) as date from Daily_Bunk_Entry where dbe_status='A' and dbe_bunk_no='" + BunkNo + "' ";

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
                    Bill_No = get_Bill_No(Company, Bill_Type);
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
                    else if (Column.Equals(ColumnPerfix + "bill_no"))
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



                DataTable dds = GITAPI.dbFunctions.getTable("delete from Sales_Details where sal_bill_no='" + Bill_No + "'");

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



                    dt = GITAPI.dbFunctions.getTable("delete from Stock_Details where sd_voucher_no='" + Sal_No + "' and sd_vour_type='" + Acc_Data.G_Type_Sales + "'");


                    // Item Wise Qty Update
                    Query = "insert into Stock_Details" +
                          " (sd_uni_code,sd_vour_type,sd_vour_refno,sd_order_no,sd_voucher_no,sd_voucher_date,sd_prod_id,sd_inward_qty,sd_outward_qty,sd_rate,sd_amount,sd_credit_amt,sd_debit_amt,sd_created_by,sd_created_date,sd_status,sd_barcode)" +
                                 " select sal_uni_code," +
                                 "'" + Acc_Data.G_Type_Sales + "'," +
                                 "sal_id," +
                                 "2," +
                                 " sal_no, " +
                                 "sal_date," +
                                 "[sal_prod_id]," +
                                 "0 AS INWARD_QTY, " +
                                 "sal_qty+sal_free  as Out_Qty, " +
                                 "[sal_rate] AS RATE," +
                                 "[sal_rate]*sal_qty AS AMOUNT, " +
                                 "[sal_rate]*sal_qty  AS CRAMOUNT," +
                                 "0 AS DEBIT_AMT," +
                                 "sal_created_by,   " +
                                 "sal_created_date, " +
                                 "sal_status ,sal_prod_code  " +
                                 " from  " +
                                 " sales_details" +
                                 " where sal_no= '" + Sal_No + "'";

                    DataTable dd = GITAPI.dbFunctions.getTable(Query);

                    //Acc_Data.Accounts_Update(Acc_Data.G_Type_Sales, ID, Company, "Sales");

                }

                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [HttpGet]
        public string get_Bill_No(string Company, string Bill_Type)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='" + Bill_Type + "' ";
            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(sal_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Sales ";
            Query += "where left(sal_no,len(@Prefix))=@Prefix ";
            Query += "and right(sal_no,len(@suffix))=@suffix ";


            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }

        [HttpGet]
        public string get_Bill_No(string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Invoice' ";
            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(sal_bill_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Sales ";
            Query += "where left(sal_bill_no,len(@Prefix))=@Prefix ";
            Query += "and right(sal_bill_no,len(@suffix))=@suffix ";


            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }

        [HttpGet]
        public string get_Stock_by_Item_ID(string Item_ID, string Company)
        {
            string Query = "select x.sd_prod_id as Item_ID,sum(sd_inward_qty-sd_outward_qty)  as Qty, " +
                " (select pm_purprice from Product_Master  y where x.sd_prod_id=pm_id)  as Landing_Cost " +
                " from Stock_Details   x  " +
                " where x.sd_prod_id=" + Item_ID + "  group by x.sd_prod_id ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Saleswise_Item(string ID, string Bill_Mode, string From, string To, string Sales_person, string User, string Pay_Mode, string Area, string Company)
        {


            string condi = "";

            condi += " and x.sal_Bill_No='" + ID + "'";
            condi += " and convert(Varchar,x.sal_Bill_Date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.sal_Bill_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (User.ToLower() != "all")
            {
                condi += " and  y.sal_Created_by='" + User + "'";
            }

            //if (Sales_person.ToLower() != ("all"))
            //{
            //    condi += " and  x.Sales_person='" + Sales_person + "'";
            //}

            if (Pay_Mode.ToLower() != ("0") && Sales_person.ToLower() != ("all"))
            {
                condi += " and  x.sal_Pay_Mode='" + Pay_Mode + "'";
            }


            if (Bill_Mode.ToLower() != ("all"))
            {
                condi += " and  x.Bill_Mode='" + Bill_Mode + "'";
            }

            //if (Area.ToLower() != ("all"))
            //{
            //    condi += " and  x.sal_Area='" + Area + "'";
            //}

            //condi += Condi_Sales;




            string Qurey = "select x.sal_bill_no as Bill_No,dbo.Date_(x.sal_date) as sal_date, dbo.date_(y.sal_Bill_date) as Date,y.sal_Ledger_name as Customer_name,sal_prod_name as Item_Name,sal_qty as Qty,x.sal_free as Free,sal_rate as Unit_Price,x.sal_net_amt as Net_Amt from Sales_details" +
                           " x left outer join Sales y on x.sal_Bill_No=y.sal_Bill_No where 0=0" + condi;

            DataTable dt = GITAPI.dbFunctions.getTable(Qurey);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }
        [HttpGet]
        public string get_Sale_Detail(string From, string To, string User, string Area, string order_by, string Bill_Type, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,x.sal_Bill_Date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.sal_Bill_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (User.ToLower() != "all")
            {
                condi += " and  x.sal_Created_by='" + User + "'";
            }

            if (Bill_Type.ToLower() != "all")
            {
                condi += " and  x.sal_Bill_Type='" + Bill_Type + "'";
            }

            //if (Area.ToLower() != "all")
            //{
            //    condi += " and  x.sal_Area='" + Area + "'";
            //}
            //string s = "select dbo.Date_(Bill_Date) as Bill_Date,dbo.get_ref_value" + Company + "(Department) as Department,x.Department as Department_,x.*,Ledger_Name,dbo.Date_(Bill_Date) as Bill_Date_ from Sales" + Company + " x left outer join Ledger_Master" + Company + " l on l.ID=x.Ledger_id  where 0=0 " + condi + " order by x." + order_by;
            string s = "select dbo.Date_(sal_Bill_Date) as sal_bill_date,dbo.Date_(sal_due_date) as sal_due_date,dbo.Date_(sal_date) as sal_date,x.*,cus_name as Ledger_Name,dbo.Date_(sal_Bill_Date) as Bill_Date_ from Sales x left outer join Ledger_Master l on l.Cus_ID=x.Sal_Ledger_id  where 0=0 " + condi + " order by x." + order_by;
            DataTable dt = GITAPI.dbFunctions.getTable(s);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }



        [HttpGet]
        public string get_Sales_Entry_details(string Bill_No, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select dbo.Date_(sal_date) as sal_date,*, dbo.date_(sal_Bill_Date) as Date from sales_details where sal_Bill_No='" + Bill_No + "'  order by  sal_ID ");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string delete_Opening_Entry(string ID, string UserName, string Item_ID, string Purchase_No, string Rate, string Company)
        {
            string q = " delete from Purchase_details where pur_id=" + ID;
            q += " delete from Stock_Details where sd_vour_type='Opening' and sd_vour_refno='" + ID + "' ";
            GITAPI.dbFunctions.getTable(q);
            return "True";
        }

        [HttpGet]
        public string get_OpeningStock_Item(string Ledger_ID, string Date, string Company)
        {

            string condi = "";
            if (Ledger_ID.ToLower() != "0")
            {
                condi += " and  x.pur_ledger_id='" + Ledger_ID + "'";
            }

            condi += " and  x.pur_no='Opening'";

            DataTable dt = GITAPI.dbFunctions.getTable("select  pur_id as ID,'' as Category,pur_prod_name as Item_Name,pur_rate as Rate,pur_qty as Qty,pur_net_amt as Total,dbo.Date_(pur_date) as pur_date, dbo.Date_(pur_bill_date) as pur_bill_date,* from Purchase_details x   where 0=0 " + condi);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }

    }
}
