using Genuine_API.Common;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class ReportsController : ApiController
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
        public string get_DB_Bank_Details(string From, string To, string Branch_ID, string Company)
        {
            string condi = "";
            if (!Branch_ID.ToLower().Equals("0"))
            {
                condi += " and  db_branch_id='" + Branch_ID + "' ";
            }
            string Quary = "select y.Account_Number+'-'+Bank_Name as Ledger_Name ,sum(x.db_cramt1-x.db_dbamt1) as Amount from DayBook x left outer join Bank_Master y on  x.db_received_bank=y.ID where 0=0 "+ condi + " group by y.Account_Number+'-'+Bank_Name ";
            DataTable dt = GITAPI.dbFunctions.getTable(Quary);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Transport_Report(string From, string To, string transid, string Project, string type, string Branch_ID, string Company)
        {
            string condi = "";
            condi += " and convert(Varchar,tpt_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,tpt_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (!transid.ToLower().Equals("0"))
            {
                condi += " and  tpt_transid='" + transid + "' ";
            }

            if (!Branch_ID.ToLower().Equals("0"))
            {
                condi += " and  tpt_branch='" + Branch_ID + "' ";
            }

            if (!transid.ToLower().Equals("0"))
            {
                condi += " and  tpt_transid='" + transid + "' ";
            }

            if (!type.ToLower().Equals("all"))
            {
                condi += " and  tpt_narration1='" + type + "' ";
            }

            if (!Project.ToLower().Equals("all"))
            {
                condi += " and  tpt_projectname='" + Project + "' ";
            }
            condi += " order by convert(varchar,tpt_date,112) asc ";
            string q = "select dbo.date_(tpt_date) as [tpt_date],tpt_type,tpt_no,tpt_projectname,tpt_contact_no, " +
    " tpt_ledger_name,tpt_transport,tpt_tpttype,br_rate as Rate,tpt_from, tpt_to,tpt_material_name,tpt_ttype,tpt_load, " +
    " tpt_opening,tpt_closing,tpt_tothours, tpt_amount,tpt_remarks,tpt_narration1,tpt_narration2, " +
    " tpt_narration3,tpt_created_by from Transport_Entry left outer join Vehicle_Basic_Rate on br_tpttype =tpt_type and br_type=tpt_tpttype where 0=0 " + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Pur_Productwise_Report(string From, string To, string Company)
        {
            string condi = "";
            condi += " and convert(Varchar,pur_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,pur_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";


            //if (!Company.ToLower().Equals("0"))
            //{
            //    condi += " and  tpt_company='" + Company.Replace("_", "") + "' ";
            //}

            DataTable dt = GITAPI.dbFunctions.getTable("select * from purchase_details where 0=0 " + condi);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }
        [HttpGet]
        public string get_Pur_Daywise_Report(string From, string To, string Company)
        {
            string condi = "";
            condi += " and convert(Varchar,pur_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,pur_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";


            //if (!Company.ToLower().Equals("0"))
            //{
            //    condi += " and  tpt_company='" + Company.Replace("_", "") + "' ";
            //}

            DataTable dt = GITAPI.dbFunctions.getTable("select dbo.date_(pur_purchase_date) as [pur_purchase_date], dbo.date_(pur_bill_date) as [pur_bill_date],* from purchase_details where 0=0 " + condi);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }
        [HttpGet]
        public string get_Pur_userwise_Report(string From, string To, string Company)
        {
            string condi = "";
            condi += " and convert(Varchar,pur_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,pur_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";


            //if (!Company.ToLower().Equals("0"))
            //{
            //    condi += " and  tpt_company='" + Company.Replace("_", "") + "' ";
            //}

            DataTable dt = GITAPI.dbFunctions.getTable("select * from purchase_details where 0=0 " + condi);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }
        [HttpGet]
        public string get_current_wise_Report(string From, string To, string Company)
        {
            string condi = "";
            condi += " and convert(Varchar,Voucher_Date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,Voucher_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            //if (!Company.ToLower().Equals("0"))
            //{
            //    condi += " and  tpt_company='" + Company.Replace("_", "") + "' ";
            //}

            DataTable dt = GITAPI.dbFunctions.getTable("select * from Stock_Details where 0=0 " + condi);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }
        [HttpGet]
        public string get_category_wise_Report(string From, string To, string Company)
        {
            string condi = "";
            condi += " and convert(Varchar,Voucher_Date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,Voucher_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";


            //if (!Company.ToLower().Equals("0"))
            //{
            //    condi += " and  tpt_company='" + Company.Replace("_", "") + "' ";
            //}

            DataTable dt = GITAPI.dbFunctions.getTable("select * from Stock_Details where 0=0 " + condi);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Purchase_Report(string From, string To, string Bill_Mode, string User, string Pay_Mode, string Area, string order_by, string Company)
        {

            string condi = "";

            condi += " and convert(Varchar,x.pur_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.pur_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (User.ToLower() != "all")
            {
                condi += " and  x.pur_created_by='" + User + "'";
            }

            if (Pay_Mode.ToLower() != ("0"))
            {
                condi += " and  x.pur_pay_mode='" + Pay_Mode + "'";
            }


            if (Bill_Mode.ToLower() != ("all"))
            {
                condi += " and  x.pur_bill_mode='" + Bill_Mode + "'";
            }
            condi += " and  x.pur_type='Purchase' ";


            condi += " and  y.pur_prodname!='' ";
            string query = " select x.pur_purchase_no as Purchase_No, x.pur_bill_no as Bill_No, x.pur_bill_date as Purchase_Date, dbo.date_(x.pur_bill_date) as Purchase_Date_, x.pur_ledger_id as Ledger_ID,x.pur_ledger_name as Supplier_Name, " +
                " x.pur_contact_no as Contact_No , x.pur_bill_mode as Bill_Mode, y.pur_prodname as Item_Name, sum(y.pur_qty) as Qty,sum(y.pur_net_amt) as Amount, " +
                " x.pur_created_by as [User],sum(y.pur_qty*y.pur_rate) as [S_Rate],sum((y.pur_qty*y.pur_rate)-y.pur_net_amt) as Profit,x.pur_pay_mode as Pay_Mode_ " +
                " from Purchase x  left outer join  Purchase_details y on  x.pur_purchase_no=y.pur_purchase_no " +
                "  where 0=0 " + condi + " " +
                " group by y.pur_prodname ,x.pur_bill_mode,x.pur_created_by,dbo.date_(x.pur_bill_date),x.pur_purchase_no,x.pur_pay_mode, " +
                " x.pur_bill_date ,x.pur_ledger_id,x.pur_ledger_name, x.pur_bill_no,x.pur_contact_no order by " + order_by;

            DataTable dt = GITAPI.dbFunctions.getTable(query);

            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }

        [HttpGet]
        public string get_Daybook(string From, string To, string Ledger_ID, string order_by, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,db_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,db_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (Ledger_ID.ToLower() != ("0"))
            {
                condi += " and db_received_bank='" + Ledger_ID + "'";
            }
            string Query1 = "";
            if (Ledger_ID.ToLower() != ("0"))
            {
                Query1 = "select '' as Ledger_Name,'' as ProjectName,'' as Ledger, '' as AC_Date,'Opeing' as Narration1, isnull((select abs(sum(db_dbamt1-db_cramt1)) from DayBook where db_branch_id='" + Company.Replace("_", "") + "' and  db_received_bank='" + Ledger_ID + "' and convert(Varchar,db_date,112)<'" + DateTime.Parse(From).ToString("yyyyMMdd") + "' group by db_received_bank  having sum(db_dbamt1-db_cramt1)<0),0)  as Credit, isnull((select abs(sum(db_dbamt1-db_cramt1)) from DayBook where db_branch_id='" + Company.Replace("_", "") + "' and db_received_bank='" + Ledger_ID + "' and convert(Varchar,db_date,112)<'" + DateTime.Parse(From).ToString("yyyyMMdd") + "' group by db_received_bank  having sum(db_dbamt1-db_cramt1)>0),0) as Debit,0 as Balance union all ";
            }
            else
            {
                Query1 = "select '' as Ledger_Name,'' as ProjectName,'' as Ledger, '' as AC_Date,'Opeing' as Narration1, isnull((select abs(sum(db_dbamt1-db_cramt1)) from DayBook where db_branch_id='" + Company.Replace("_", "") + "' and   convert(Varchar,db_date,112)<'" + DateTime.Parse(From).ToString("yyyyMMdd") + "'  having sum(db_dbamt1-db_cramt1)<0),0)  as Credit, isnull((select abs(sum(db_dbamt1-db_cramt1)) from DayBook where  db_branch_id='" + Company.Replace("_", "") + "' and convert(Varchar,db_date,112)<'" + DateTime.Parse(From).ToString("yyyyMMdd") + "'  having sum(db_dbamt1-db_cramt1)>0),0) as Debit,0 as Balance union all ";
            }

            string Query = Query1 + " select db_group as Ledger_Name,CASE WHEN db_group = 'PAYMENT' THEN cb_project ELSE oc_projectname END AS ProjectName,db_ledger_name as Ledger, dbo.date_(db_date) as AC_Date,db_narration1 as Narration1,db_cramt1 as Credit,db_dbamt1 as Debit,0 as Balance from DayBook x left outer join Other_Collection on oc_id=db_vour_refno left outer join balance on cb_id=db_vour_refno left outer join  Bank_Master y on x.db_received_bank=y.ID where 0=0 and db_branch_id='" + Company.Replace("_", "") + "' " + condi;
            DataTable dt = GITAPI.dbFunctions.getTable("select * from (" + Query + " )t order by cast(AC_Date as datetime)");
            try
            {
                dt.Rows[0]["Balance"] = decimal.Parse(dt.Rows[0]["Credit"].ToString()) - decimal.Parse(dt.Rows[0]["Debit"].ToString());

                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Balance"] = decimal.Parse(dt.Rows[i - 1]["Balance"].ToString()) + decimal.Parse(dt.Rows[i]["Credit"].ToString()) - decimal.Parse(dt.Rows[i]["Debit"].ToString());
                }
            }
            catch (Exception ex)
            {
                dbFunctions.Logs(ex.ToString(), "");
            }

            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }

        [HttpGet]
        public string get_Gst_Comparison_Report(string From, string To, string Company)
        {

            string condi = "";

            if (!Company.ToLower().Equals("0"))
            {
                condi += " and cb_company='" + Company.Replace("_", "") + "' ";
            }
            string Query1 = " ;WITH CTE as ( select cus_id,cus_name as [Name],cus_gstin as GSTNo,sum(isnull(cb_amountout,0)) as Amount " +
                " from Ledger_Master left outer join Balance on cb_ledger_id=cus_id " + condi +
                " and convert(varchar(6),cb_date,112)>=convert(varchar(6),'" + DateTime.Parse(From).ToString("yyyyMMdd") + "',112) " +
                " and convert(varchar(6),cb_date,112)<=convert(varchar(6),'" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ,112) " +
                " where cus_status='A' group by cus_id,cus_name,cus_gstin ";
            condi = "";
            if (!Company.ToLower().Equals("0"))
            {
                condi += " and oc_company='" + Company.Replace("_", "") + "' ";
            }
            Query1 += "  union all   select cus_id,cus_name as [Name],cus_gstin as GSTNo,sum(isnull(oc_amount,0)) as Amount " +
                " from Ledger_Master left outer join Other_Collection on oc_ledger_id=cus_id " + condi +
                " and convert(varchar(6),oc_date,112)>=convert(varchar(6),'" + DateTime.Parse(From).ToString("yyyyMMdd") + "',112) " +
                " and convert(varchar(6),oc_date,112)<=convert(varchar(6),'" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ,112) " +
                " where cus_status='A' and oc_type='Vehicle Maintanace' group by cus_id,cus_name,cus_gstin ";
            condi = "";

            if (!Company.ToLower().Equals("0"))
            {
                condi += " and Company='" + Company.Replace("_", "") + "' ";
            }
            Query1 += "  ) select datename(month, '" + DateTime.Parse(From).ToString("yyyyMMdd") + "') +' To '+datename(month, '" + DateTime.Parse(To).ToString("yyyyMMdd") + "') as [Month], " +
                " cus_id as ID,[Name],GSTNo,(Amount) as Amount, isnull(sum(([TaxableValue]+[IGST]+[CGST]+[SGST])),0) as [BAmount],isnull(Amount,0)-isnull(sum(([TaxableValue]+[IGST]+[CGST]+[SGST])),0) " +
                " as [Diff] from (select cus_id,[Name],GSTNo,sum(Amount) as Amount from CTE group by cus_id,[Name],GSTNo) as x " +
                " left outer join GSTR2A_Data on GSTIN=GSTNo and convert(varchar(6),InvoiceDate,112)>=" +
                "convert(varchar(6),'" + DateTime.Parse(From).ToString("yyyyMMdd") + "',112) and " +
                " convert(varchar(6),InvoiceDate,112)<=convert(varchar(6),'" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ,112) " + condi +
                " group by [Name],GSTNo,cus_id,Amount having sum(isnull(Amount,0))+sum((isnull([TaxableValue],0)+isnull([IGST],0)+isnull([CGST],0)+isnull([SGST],0)))!=0";

            DataTable dt = GITAPI.dbFunctions.getTable(Query1);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Purchase_outstanding(string Date, string Customer, string Area, string Type, string order_by, string Company)
        {

            string condi = "";

            //condi += " and convert(Varchar,y.cb_billdate,112)<='" + DateTime.Parse(Date).ToString("yyyyMMdd") + "'";


            if (Customer.ToLower() != "0")
            {
                condi += " and   y.Ledger_ID='" + Customer + "'";
            }
            if (Area.ToLower() != "all")
            {
                condi += " and   l.cus_city='" + Area + "'";
            }

            string column = "datediff(day, cast(dbo.date_(isnull(y.Due_Date,'" + GITAPI.dbFunctions.getdate() + "')) as datetime),cast('" + GITAPI.dbFunctions.getdate() + "' as datetime))";

            string Dues = "( case when  " + column + " <=0  then '<=0' else" +
                          " case when (" + column + ">0 and " + column + "<=15 ) then '1-15 Days' else " +
                          " case when (" + column + ">=16 and " + column + "<=30)  then '16-30 Days' else " +
                          " case when (" + column + ">=31 and " + column + "<=45)  then '31-54 Days' else " +
                          " case when  " + column + ">45   then '>45 Days'  end  end end end end) as Dues";
            string q = "select y.Bill_No,dbo.date_(y.Bill_Date) as Bill_Date,l.cus_id as Ledger_ID,l.cus_name as Customer_Name, l.cus_contact_number as Contact_No, " +
                " y.Bill_Amount,y.Bal_Amount as Amount,dbo.date_(isnull(y.Due_Date,'')) as Due_Date_, " +
                " datediff(day, cast(dbo.date_(y.Due_Date) as datetime),cast('" + GITAPI.dbFunctions.getdate() + "' as datetime)) as Due_Days, " +
                " " + Dues + ", l.cus_city as Area,'' as Sales_Person from Amount_Balance y " +
                " left outer join Ledger_Master l on cus_id=Ledger_Id where 0=0 " + condi + " order by l.cus_name ";
            string q1 = " select isnull(y.Bill_No,'1') as Bill_No,dbo.Date_(isnull(y.Bill_Date,GETDATE())) as Bill_Date,l.cus_id as Ledger_ID,l.cus_name as Customer_Name, l.cus_contact_number as Contact_No, " +
                " isnull(y.Bill_Amount,0) as Bill_Amount,isnull(y.Bal_Amount,1) as Amount,dbo.date_(isnull(y.Due_Date,'" + GITAPI.dbFunctions.getdate() + "')) as Due_Date_, " +
                " datediff(day, cast(dbo.date_(isnull(y.Due_Date,'" + GITAPI.dbFunctions.getdate() + "')) as datetime),cast('" + GITAPI.dbFunctions.getdate() + "' as datetime)) as Due_Days, " +
                " " + Dues + ", l.cus_city as Area,'' as Sales_Person from Ledger_Master l " +
                " left outer join Amount_Balance y on cus_id=Ledger_Id and Bal_Amount!=0 where 0=0 " + condi + " order by l.cus_name ";

            string query = "";

            if (Type.ToLower().Equals("all"))
            {
                query = q1;
            }
            else
            {
                query = q;
            }

            DataTable dt = GITAPI.dbFunctions.getTable(query);


            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Amount_Collection(string From, string To, string Customer, string User, string Area, string Pay_Mode, string order_by, string Company)
        {

            string condi = "";
            condi += "and cb_vtype='Payment' and convert(Varchar,x.cb_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.cb_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (Customer.ToLower() != ("0"))
            {
                condi += " and  x.cb_ledger_id='" + Customer + "'";
            }

            if (Area.ToLower() != ("all"))
            {
                condi += " and  l.cus_city='" + Area + "'";
            }

            if (User.ToLower() != ("all"))
            {
                condi += " and  x.cb_created_by='" + User + "'";
            }

            if (!Company.ToLower().Equals("0"))
            {
                condi += " and  x.cb_company='" + Company.Replace("_", "") + "' ";
            }

            if (Pay_Mode.ToLower() != ("0"))
            {
                condi += " and  dbo.get_ref_id(cb_pay_mode)='" + Pay_Mode + "'";
            }

            DataTable dt = GITAPI.dbFunctions.getTable("select cus_city as Area,cb_uniqno as Receipt_No,cb_date as Receipt_Date,0 as Intrest_Amt,0 as Principal_Amt,0 as Process_Amt, dbo.date_(cb_date) as RCDate_, dbo.Time_(x.cb_created_date) as RCTime_,cb_ledger_id as Ledger_ID,cus_code as Code, cus_name  as Ledger_Name,cb_amountout as Amount,dbo.get_ref_id(cb_pay_mode) as Pay_Mode ,cb_pay_mode as Pay_Mode_, cb_received_bank as Received_Bank,cb_billno as Bill_No,x.cb_created_by as Created_By from Balance x  left outer join Ledger_Master l on cb_ledger_id=l.cus_id  where 0=0  " + condi + "  order by " + order_by);

            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }

        [HttpGet]
        public string get_Tyre_Entry_Report(string From, string To, string Order_by, string Company = "0")
        {
            string condi = "";
            condi += " and convert(Varchar,trr_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,trr_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            condi += " and trr_status='A' ";
            if (!Order_by.Equals(""))
            {
                condi += " Order by " + Order_by;
            }
            string q = "select dbo.Date_(trr_date) as trr_date,trr_qty as trr_qty,ty_purno as ty_purno,trr_tyre_no as trr_tyre_no,ty_Brand as ty_Brand,ty_size as ty_size,trr_vehicle_no as trr_vehicle_no,trr_position as trr_position,trr_km as trr_km,trr_amount as trr_amount from Tyre_Entry left outer join Tyre_master on trr_id=ty_id where 0=0 " + condi + "";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]


        public string get_Log_Book_Entry_Report(string From, string To, string Branch_ID, string Company = "0")
        {
            string condi = "";
            condi += " AND CONVERT(VARCHAR, lbe_date, 112) >= '" + DateTime.Parse(From).ToString("yyyyMMdd") + "' ";
            condi += " AND CONVERT(VARCHAR, lbe_date, 112) <= '" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ";
            condi += " AND lbe_status = 'A' ";
            if (!Branch_ID.Equals("0"))
            {
                condi += " lbe_branch='" + Branch_ID + "' ";
            }
            string q = "WITH KM_Calculation AS ( " +
             " SELECT Veh_Type, " +
             " lbe_vehicle_no, " +
             " MIN(CASE WHEN CONVERT(VARCHAR, lbe_date, 112) >= '" + DateTime.Parse(From).ToString("yyyyMMdd") + "' " +
             " THEN lbe_starting_km END) AS lbe_starting_km, " +
             " MAX(CASE WHEN CONVERT(VARCHAR, lbe_date, 112) <= '" + DateTime.Parse(To).ToString("yyyyMMdd") + "' " +
             " THEN lbe_closing_km END) AS lbe_closing_km, " +
             " SUM(lbe_running_km) AS total_running_km, " +
             " (SELECT SUM(be_qty) " +
             " FROM Bunk_Entry " +
             " WHERE be_ledger_name = lbe_vehicle_no " +
             " AND CONVERT(VARCHAR, be_date, 112) >= '" + DateTime.Parse(From).ToString("yyyyMMdd") + "' " +
             " AND CONVERT(VARCHAR, be_date, 112) <= '" + DateTime.Parse(To).ToString("yyyyMMdd") + "') AS Total_Diesel " +
             " FROM Log_Book_Entry " +
             " LEFT OUTER JOIN Vehicle_Master ON Veh_ID = lbe_vehicle_id " +
             " WHERE lbe_status = 'A' " +
             condi +
             " GROUP BY Veh_Type, lbe_vehicle_no " +
             ") " +
             "SELECT lbe_vehicle_no, " +
             " Veh_Type, " +
             " lbe_starting_km, " +
             " lbe_closing_km, " +
             " total_running_km AS Total_Running_KM, " +
             " Total_Diesel, " +
             " CASE " +
             " WHEN Veh_Type IN ('JCB', 'HITACHI') THEN " +
             " CONVERT(DECIMAL(18,2), NULLIF(Total_Diesel, 0) / NULLIF(total_running_km, 0)) " +
             " WHEN Veh_Type IN ('TIPPER', 'TRACTOR','MILLER','TANKER','ROLLER','PICKUP','CARS','GRADER','PAVER','OPEN BODY') THEN " +
             " CONVERT(DECIMAL(18,2), NULLIF(total_running_km, 0) / NULLIF(Total_Diesel, 0)) " +
             " ELSE 0 " +
             " END AS PerKm " +
             " FROM KM_Calculation;";


            //string q = @"select lbe_id, Veh_Type as Veh_Type, lbe_vehicle_no, lbe_starting_km, lbe_closing_km, lbe_running_km 
            //     from Log_Book_Entry
            //     left outer join Vehicle_Master on Veh_ID = lbe_vehicle_id
            //     where lbe_status = 'A' " + condi + " group by lbe_vehicle_no, lbe_id, Veh_Type, lbe_starting_km, lbe_closing_km, lbe_running_km;";

            //string q = "select lbe_id,Veh_Type as Veh_Type,lbe_vehicle_no,lbe_starting_km,lbe_closing_km,lbe_running_km from Log_Book_Entry" +
            //    " left outer join Vehicle_Master  on Veh_ID=lbe_vehicle_id where 0=0 " + condi + " " + "group by lbe.lbe_vehicle_no;";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }

        // bunk report


        public string get_Bunk_Entry_Report(string From, string To, string Company = "0")
        {
            string condi = "";
            condi += " and convert(Varchar,be_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,be_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ";

            string q = "select  CONCAT(be_ledger_name, '', be_narration1) AS [LedgerDescription], " +
                 " be_category AS Category, " +
                 " est_projectname AS Site, " +
                 " SUM(be_amount) AS Amount " +
                 " FROM Bunk_Entry " +
                 " LEFT OUTER JOIN Estimation ON be_projectid = est_id " +
                 " WHERE be_status = 'A' " + condi +
                 " GROUP BY be_ledger_name, be_category, be_product, est_projectname,be_narration1" +
                 " ORDER BY LedgerDescription ASC;";

            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }


        //bunk_entry_report
        public string get_Bunk_daily_Entry_report(string From, string To, string Order_by, string Company = "0")
        {
            string condi = "";
            condi += " and convert(Varchar,dbe_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,dbe_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ";

            condi += " and dbe_status='A' ";
            //if (!Order_by.Equals(""))
            //{
            //    condi += " Order by " + Order_by;
            //}
            string q = "SELECT dbe_bunk_no, MIN(dbe_bunk_opening) AS dbe_bunk_opening, MAX(dbe_bunk_closing) AS dbe_bunk_closing,  ABS(MIN(dbe_bunk_opening) - MAX(dbe_bunk_closing)) AS dbe_closing_liter, AVG(dbe_bunk_rate) AS dbe_bunk_rate ,  ABS(MIN(dbe_bunk_opening) - MAX(dbe_bunk_closing)) * AVG(dbe_bunk_rate) as  dbe_bunk_Amount FROM  Daily_Bunk_Entry where 0=0  " + condi +
                "GROUP BY dbe_bunk_no " +
                "ORDER BY dbe_bunk_no;";
            DataTable dt = GITAPI.dbFunctions.getTable(q);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }


        ////////////////////purchasereport//////////////
        ///


       
        public string get_Auditor_pruchase_Report(string From, string To, string Type, string Company)
        {
            string condi = "";
            condi += " and convert(Varchar,pd.pur_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,pd.pur_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (Type.ToLower() != "all")
            {
                condi += " and  pd.pur_type='" + Type + "'";
            }

            string query = "select pd.pur_ledger_name, pd.pur_gst_no, pd.pur_bill_no, CONVERT(varchar, pd.pur_bill_date, 103) AS pur_bill_date, " +
               "pd.pur_hsn_code as pur_hsn_code, pd.pur_qty as pur_qty, dbo.get_ref_value(pd.pur_uom) as UOM, p.pur_net_amt as INV_Value, " +
               "pd.pur_gst_per, sum(pd.pur_gst_amt) as Taxable_amount, sum(pd.pur_igst_amt) as igst_amt, sum(pd.pur_sgst_amt) as sgst_amt, " +
               "sum(pd.pur_cgst_amt) as cgst_amt " +
               "from Purchase_Details pd " +
               "left outer join Purchase p on p.pur_purchase_no = pd.pur_purchase_no " +
               "where 0 = 0 " + condi + " " +
               "group by pd.pur_ledger_name, pd.pur_gst_no, pd.pur_bill_no, pd.pur_bill_date, pd.pur_gst_per, p.pur_net_amt, " +
               "pd.pur_gst_amt, pd.pur_hsn_code, pd.pur_qty, pd.pur_uom";

            DataTable dt = GITAPI.dbFunctions.getTable(query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Fuel_Entry_Reports(string From, string To, string Type, string Branch_ID, string Company)
        {
            string Query = "";
            string condi = "";
            if (!Branch_ID.ToLower().Equals("0"))
            {
                condi += " and be_branch='" + Branch_ID + "' ";
            }
            if (!Type.ToLower().Equals("all"))
            {
                condi += " and be_type='" + Type + "' ";
            }
            condi += " and rtrim(ltrim(be_product)) IN ('Diesel','Petrol') ";
            condi += " and rtrim(ltrim(be_category)) IN ('MV Vechile','Rental') ";
            //if (!Company.ToLower().Equals("0"))
            //{
            //    condi += " and oc_company='" + Company.Replace("_", "") + "' ";
            //}
            condi += " and convert(Varchar,be_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,be_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            condi += " order by convert(varchar,be_date,112) asc ";
            Query += "select *,Convert(decimal(18,2),KM/be_qty) as PerKm from( " +
                " select convert(varchar,be_date,103) as date,be_ledger_name as be_ledger_name, " +
                 " be_product as be_product,be_type as be_type,be_category as be_category, " +
                " be_qty as be_qty,be_rate as be_rate, " +
                " be_km as be_km,be_amount as be_amount,be_km-lag(be_km) over (partition by be_ledger_name " +
                " order by right(be_ledger_name,3) ASC,convert(varchar,be_date,112) ASC) as KM, be_date " +
                " from Bunk_Entry ) as x " +
                " where  0=0" + condi;
            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Purchase_Detail(string From, string To, string User, string Area, string order_by, string Bill_Type, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,x.pur_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.pur_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            //if (User.ToLower() != "all")
            //{
            //    condi += " and  x.sal_created_by='" + User + "'";
            //}

            if (Bill_Type.ToLower() != "all")
            {
                condi += " and  x.pur_bill_type='" + Bill_Type + "'";
            }

            if (Area.ToLower() != "all")
            {
                condi += " and  x.pur_area='" + Area + "'";
            }


            string s = " select pur_bill_no as Bill_No,dbo.Date_(pur_bill_date) as Bill_Date, " +
                " dbo.Date_(pur_bill_date) as pur_bill_date,pur_gst_no as GST_No,pur_taxable_amount as Taxable_Amount, " +
                " pur_tax_per as Tax_Per,pur_igst_amt as IGST_Amt,pur_sgst_amt as SGST_Amt,pur_cgst_amt as CGST_Amt, " +
                " pur_tax_amt as Tax_Amt,pur_net_amt as Net_Amt,cus_name as Ledger_Name, " +
                " dbo.Date_(pur_bill_date) as Bill_Date_ from Purchase x " +
                " left outer join Ledger_Master l on l.cus_id=x.pur_ledger_id where 0=0 " + condi + " order by x." + order_by;
            DataTable dt = GITAPI.dbFunctions.getTable(s);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }


        [HttpGet]
        public string get_Purchase_Gst_Detail(string From, string To, string User, string Area, string GST_No, string Bill_Type, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,y.pur_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,y.pur_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            //if (User.ToLower() != "all")
            //{
            //  condi += " and  x.Created_by='" + User + "'";
            // }

            if (Bill_Type.ToLower() != "all")
            {
                condi += " and  y.pur_bill_type='" + Bill_Type + "'";
            }
            if (GST_No.ToLower() == "0")
            {
                condi += " and  len(y.pur_gst_no)<14 ";
            }
            else if (GST_No.ToLower().Length > 5)
            {
                condi += " and len(y.pur_gst_no)>14";
            }

            // if (Area.ToLower() != "all")
            //{
            //   condi += " and  x.Area='" + Area + "'";
            // }



            String Query = "select x.pur_bill_no as Bill_No,dbo.date_(y.pur_bill_date) as Bill_Date,y.pur_ledger_name as Ledger_Name, " +
                " y.pur_gst_no as GST_No,y.pur_sub_total as Sub_Total,y.pur_disc_amt as Disc_Amt,y.pur_taxable_amount as Taxable_Amount," +
                         " sum(case when x.pur_gst_per=0 then x.pur_sub_total else 0 end)  as GST_Taxable_0," +
                         " sum(case when x.pur_gst_per=0 then x.pur_gst_amt else 0 end ) as GST_Tax_0," +
                         " sum(case when x.pur_gst_per=5 then x.pur_sub_total else 0 end)  as GST_Taxable_5," +
                         " sum(case when x.pur_gst_per=5 then x.pur_gst_amt else 0 end)  as GST_Tax_5," +
                         " sum(case when x.pur_gst_per=12 then x.pur_sub_total else 0 end)  as GST_Taxable_12," +
                         " sum(case when x.pur_gst_per=12 then x.pur_gst_amt else 0 end)  as GST_Tax_12," +
                         " sum(case when x.pur_gst_per=18 then x.pur_sub_total else 0 end)  as GST_Taxable_18," +
                         " sum(case when x.pur_gst_per=18 then x.pur_gst_amt else 0 end)  as GST_Tax_18," +
                         " sum(case when x.pur_gst_per=28 then x.pur_sub_total else 0 end)  as GST_Taxable_28," +
                         " sum(case when x.pur_gst_per=28 then x.pur_gst_amt else 0 end)  as GST_Tax_28," +
                         " sum(x.pur_gst_amt)-y.pur_tax_amt as SD_Tax,y.pur_tax_amt as s_Tax,y.pur_net_amt as Net_Amt from Purchase_details" +
                         " x left outer join Purchase y on x.pur_purchase_no=y.pur_purchase_no where 0=0 " + condi +
                         " group by x.pur_bill_no,dbo.date_(y.pur_bill_date),y.pur_ledger_name,y.pur_gst_no,y.pur_sub_total, " +
                         " y.pur_disc_amt,y.pur_taxable_amount,y.pur_net_amt,y.pur_tax_amt   order by x.pur_bill_no ";


            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }



        [HttpGet]
        public string get_Pur_HSNWise_GST(string From, string To, string User, string Area, string GST_No, string Bill_Type, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,y.pur_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,y.pur_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            //if (User.ToLower() != "all")
            //{
            //  condi += " and  x.Created_by='" + User + "'";
            // }

            if (Bill_Type.ToLower() != "all")
            {
                condi += " and  y.pur_bill_type='" + Bill_Type + "'";
            }
            if (GST_No.ToLower() == "0")
            {
                condi += " and  len(y.pur_gst_no)<14 ";
            }
            else if (GST_No.ToLower().Length > 5)
            {
                condi += " and len(y.pur_gst_no)>14";
            }

            // if (Area.ToLower() != "all")
            //{
            //   condi += " and  x.Area='" + Area + "'";
            // }



            String Query = "select y.pur_bill_no as Invoice_No,dbo.date_(y.pur_bill_date) as Invoice_Date,y.pur_gst_no as GST_No, " +
                " y.pur_ledger_name as Ledger_Name, x.pur_hsn_code as HSN_Code,sum(x.pur_qty) as Qty, " +
                " sum(x.pur_taxable_amount) as Taxable,x.pur_gst_per as GST_Per,sum(x.pur_sgst_amt) as SGST, " +
                " sum(x.pur_cgst_amt) as CGST,sum(x.pur_igst_amt) as IGST,sum(x.pur_net_amt) as Net " +
                " from Purchase_details x left outer join Purchase y on x.pur_purchase_no=y.pur_purchase_no where 0=0 " + condi +
                " group by y.pur_bill_no,dbo.date_(y.pur_bill_date),y.pur_gst_no,y.pur_ledger_name, " +
                " x.pur_hsn_code,x.pur_gst_per order by y.pur_bill_no ";


            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }


        [HttpGet]
        public string get_Currenct_Stock(string From, string To, string Branch_ID, string Company)
        {


            string condi = "";

            //if (!(Category.ToLower() == ("all") || Category == "0" || Category == null))
            //{
            //    condi += " and  pd_category='" + Category + "'";
            //}

            //if (!(Brand.ToLower() == ("all") || Brand == "0" || Brand == null))
            //{
            //    condi += " and  pd_brand='" + Brand + "'";
            //}

            //if (Branch_ID.ToLower() == "0")
            //{
            //    condi += " and  Branch_ID='"+Branch_ID+"' ";
            //}
            string Query = " select Item_ID,pm_item_code as Item_Code,pm_item_name as Item_Name,pm_description as Description, " +
                " pm_category as Category,y.pm_rate as Rate,y.pm_mrpprice as MRP, " +
                " sum(Inward_Qty-OutWard_Qty) as Stock ,cast(sum(Inward_Qty-OutWard_Qty)*y.pm_rate as decimal(18,2)) as Value  " +
                " from Stock_Details x " +
                " Left outer join Product_Master y on x.Item_ID=y.pm_id where 0=0  " + condi + " " +
                "  group by pm_item_name,pm_item_code,Item_ID,pm_category,pm_description,y.pm_rate,y.pm_mrpprice " +
                " having sum(Inward_Qty-OutWard_Qty)!=0 " +
                " order by pm_item_name ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }


        [HttpGet]
        public string get_Item_Stock(string Item_ID, string From, string To, string Company)
        {


            string condi = "";
            condi += " and convert(Varchar,x.Voucher_Date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.Voucher_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            string Query = "select *,dbo.Date_(Voucher_Date) as Voucher_Date_,0 as Balance from Stock_Details x Left outer join Product_Master y on x.Item_ID=y.pm_id where   Item_ID=" + Item_ID + " order by Voucher_Date";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }


        [HttpGet]
        public string get_Stock_Report(string From, string To, string Branch_ID, string Company)
        {


            string condi = "";


            //if (!(Group.ToLower() == ("all") || Group == "0" || Group == null))
            //{
            //    condi += " and  Item_Group='" + Group + "'";
            //}


            string Query = " select pm_id as ID,pm_item_code as Item_Code,pm_item_name as Item_Name,pm_description as Description,pm_category as Category,y.pm_rate as Rate, " +
                           " (select isnull(sum(Inward_Qty-OutWard_Qty),0) stock from Stock_Details z where z.Item_ID=y.pm_id and convert(Varchar,z.Voucher_Date,112)<'" + DateTime.Parse(From).ToString("yyyyMMdd") + "' ) as Opening," +
                           " (select isnull(sum(Inward_Qty),0) stock from Stock_Details z where z.Item_ID=y.pm_id and convert(Varchar,z.Voucher_Date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,z.Voucher_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "') as Inward," +
                           " (select isnull(sum(OutWard_Qty),0) stock from Stock_Details z where z.Item_ID=y.pm_id and convert(Varchar,z.Voucher_Date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,z.Voucher_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ) as Outward," +
                           " (select isnull(sum(Inward_Qty-OutWard_Qty),0) stock from Stock_Details z where z.Item_ID=y.pm_id and convert(Varchar,z.Voucher_Date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "' ) as Closing" +
                           " from  Product_Master y  where 0=0 " + condi +
                           "  order by pm_item_name ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;

        }



    }
}
