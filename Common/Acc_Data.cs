using System.Data;
using System.Data.SqlClient;

namespace Genuine_API
{
  public class Acc_Data
  {
        internal static readonly string G_Type_Sales;
        public static string G_Type_Purchase = "Purchase";
        public static int G_Order_Purchase = 1;
        public static string G_Type_Expenses = "Expenses";
        public static int G_Order_Expenses = 2;
        public static string G_Type_Income = "INCOME";
        public static int G_Order_Income = 3;
        public static string G_Type_Payment = "Payment";
        public static int G_Order_Payment = 6;
        public static string G_Type_loan = "Loan";
        public static int G_Order_loan = 6;
        public static string G_Type_Deposit = "EMD";
        public static int G_Order_Deposit = 4;
        public static string G_Type_Vehicle = "Vehicle Maintanace";
        public static int G_Order_Vehicle = 8;
        public static string G_Type_Deposit_Return = "EMD-Return";
        public static int G_Order_Deposit_Return = 7;
        public static string G_Type_Loan_Return = "Loan-Return";
        public static int G_Order_Loan_Return = 7;
        public static string G_Type_Contra = "CONTRA";
        public static int G_Order_Contra = 6;

        public static void Accounts_Update(
      string vou_type,
      string vou_no,
      string Compnay,
      string Type = "")
    {
      DataTable table;
            string query = "";
            if (vou_type == Acc_Data.G_Type_Purchase)
            {
                string str;
                if (vou_no == "")
                {
                    table = GITAPI.dbFunctions.getTable("Delete from balance where cb_vtype='" + vou_type + "'");
                    str = " where 0=0   ";
                }
                else
                {
                    table = GITAPI.dbFunctions.getTable("Delete from balance where cb_vtype='" + vou_type + "' and cb_vour_refno=" + vou_no);
                    str = " where pur_id=" + vou_no;
                }
                query = " insert into balance (cb_vtype,cb_vour_refno,cb_ledger_id,cb_uniqno,cb_bill_type,cb_billno,cb_billdate,cb_duedate,cb_date,cb_billamt,cb_amountin,cb_amountout,cb_disc,cb_pay_mode,cb_received_bank,cb_cheque_no,cb_cheque_date,cb_ChequeStatus,cb_remarks,cb_narration1,cb_narration2,cb_created_by,cb_created_date,cb_status,cb_company) select 'Purchase' as cb_vtype,pur_id,pur_ledger_id,pur_purchase_no,'" + G_Type_Purchase + "' as cb_bill_type,pur_bill_no,pur_bill_date,pur_bill_date,GETDATE() as cb_duedate,pur_net_amt,pur_net_amt,0 as cb_amountout,0 as cb_disc,pur_pay_mode,0 as cb_received_bank,'' as cb_cheque_no,NULL as cb_cheque_date,'' as cb_ChequeStatus,'Purchase-Bill' as cb_remarks,'Bill No :'+pur_bill_no+' Purchase No :'+pur_purchase_no as cb_narration1,'' as cb_narration1,pur_created_by,GETDATE() as cb_created_date,'A' as cb_status,'" + Compnay.Replace("_", "") + "' as cb_company from Purchase" + str;
                table = GITAPI.dbFunctions.getTable(query);
                table = GITAPI.dbFunctions.getTable("select cb_ledger_id from balance where cb_vtype='" + vou_type + "' and cb_vour_refno='" + vou_no + "' ");
                Balance_Update(table.Rows[0][0].ToString(), "", Compnay);
            }
            if (vou_type == Acc_Data.G_Type_Expenses)
            {
                if (Type == "Material_Movement")
                {
                    string str;
                    if (vou_no == "")
                    {
                        table = GITAPI.dbFunctions.getTable("Delete from balance where cb_vtype='" + Type + "'");
                        str = " where 0=0   ";
                    }
                    else
                    {
                        table = GITAPI.dbFunctions.getTable("Delete from balance where cb_vtype='" + Type + "' and cb_vour_refno=" + vou_no);
                        str = " where mm_id=" + vou_no;
                    }
                    table = GITAPI.dbFunctions.getTable(" insert into balance (cb_vtype,cb_vour_refno,cb_ledger_id,cb_uniqno,cb_bill_type,cb_billno,cb_billdate,cb_duedate,cb_date,cb_billamt,cb_amountin,cb_amountout,cb_disc,cb_pay_mode,cb_received_bank,cb_cheque_no,cb_cheque_date,cb_ChequeStatus,cb_remarks,cb_narration1,cb_narration2,cb_created_by,cb_created_date,cb_status,cb_company) select 'Material_Movement' as cb_vtype,mm_id,mm_ledger_id,mm_no,'" + G_Type_Expenses + "' as cb_bill_type,mm_no,mm_projectdate,mm_projectdate,GETDATE() as cb_duedate,mm_net_amt,mm_net_amt,0 as cb_amountout,0 as cb_disc,'CRIDET',0 as cb_received_bank,'' as cb_cheque_no,NULL as cb_cheque_date,'' as cb_ChequeStatus,'Material Movement' as cb_remarks,'MM No :'+mm_no+' Project No :'+mm_projectno as cb_narration1,'' as cb_narration1,mm_created_by,GETDATE() as cb_created_date,'A' as cb_status,'" + Compnay.Replace("_", "") + "' as cb_company from Material_Movement" + str);
                }
                else
                {
                    string str;
                    if (vou_no == "")
                    {
                        table = GITAPI.dbFunctions.getTable("Delete from balance where cb_vtype='" + Type + "'");
                        str = " where 0=0   ";
                    }
                    else
                    {
                        table = GITAPI.dbFunctions.getTable("Delete from balance where cb_vtype='" + Type + "' and cb_vour_refno=" + vou_no);
                        str = " where oc_id=" + vou_no;
                    }
                    string q = " insert into balance (cb_vtype,cb_vour_refno,cb_ledger_id,cb_uniqno,cb_bill_type,cb_billno,cb_billdate,cb_duedate,cb_date,cb_billamt,cb_amountin,cb_amountout,cb_disc,cb_pay_mode,cb_received_bank,cb_cheque_no,cb_cheque_date,cb_ChequeStatus,cb_remarks,cb_narration1,cb_narration2,cb_created_by,cb_created_date,cb_status,cb_company) select oc_type as cb_vtype,oc_id,oc_ledger_id,oc_no,'" + G_Type_Expenses + "' as cb_bill_type,oc_no,oc_date,oc_date,GETDATE() as cb_duedate,oc_amount,oc_amount,0 as cb_amountout,0 as cb_disc,oc_pay_mode,oc_received_bank as cb_received_bank,'' as cb_cheque_no,NULL as cb_cheque_date,'' as cb_ChequeStatus,oc_type as cb_remarks,'oc no :'+oc_no+' Project No :'+oc_projectno as cb_narration1,'' as cb_narration1,oc_created_by,GETDATE() as cb_created_date,'A' as cb_status,'" + Compnay.Replace("_", "") + "' as cb_company from Other_Collection" + str;
                    table = GITAPI.dbFunctions.getTable(q);
                }
            }
        }


        public static  void Accounts_Update1(string vou_type,
      string vou_no,
      string Compnay,
      string Type = "")
        {
            DataTable dt;
            string Query = "";
            string Condition = "";
            if (vou_type == Acc_Data.G_Type_Income)
            {
                if (vou_no == "")
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "'");
                    Condition = "";
                }
                else
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "' and db_vour_refno=" + vou_no);
                    Condition = " where oc_id=" + vou_no;
                }

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                       " select '" + vou_type + "'," + G_Order_Income + ",oc_id,oc_date,oc_categoryid,oc_category,oc_ledger_id,oc_ledger_name, oc_narration1,oc_narration2,'',oc_amount,0,oc_no,oc_date,oc_pay_mode,oc_received_bank,oc_cheque_no,oc_cheque_date,oc_remarks,oc_created_by,'" + GITAPI.dbFunctions.getdate() + "',oc_status,'"+Compnay.Replace("_","")+"' " +
                       " from Other_Collection " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);
            }
            if (vou_type == Acc_Data.G_Type_Deposit)
            {
                if (vou_no == "")
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "'");
                    Condition = "";
                }
                else
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where (db_vourtype='" + Acc_Data.G_Type_Deposit_Return + "' or db_vourtype='" + Acc_Data.G_Type_Deposit + "') and db_vour_refno=" + vou_no);
                    Condition = " where oc_id=" + vou_no;
                }

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                       " select '" + vou_type + "'," + G_Order_Deposit + ",oc_id,oc_date,oc_categoryid,oc_category,oc_ledger_id,oc_ledger_name, oc_narration1,oc_narration2,'',0,oc_amount,oc_no,oc_date,oc_pay_mode,oc_received_bank,oc_cheque_no,oc_cheque_date,oc_remarks,oc_created_by,'" + GITAPI.dbFunctions.getdate() + "',oc_status,'" + Compnay.Replace("_", "") + "' " +
                       " from Other_Collection " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);
            }
            if (vou_type == Acc_Data.G_Type_Deposit_Return)
            {
                if (vou_no == "")
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "'");
                    Condition = "";
                }
                else
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where (db_vourtype='" + Acc_Data.G_Type_Deposit_Return + "' or db_vourtype='" + Acc_Data.G_Type_Deposit + "') and db_vour_refno=" + vou_no);
                    Condition = " where oc_id=" + vou_no;
                }

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                       " select '" + vou_type + "'," + G_Order_Deposit_Return + ",oc_id,oc_date,oc_categoryid,oc_category,oc_ledger_id,oc_ledger_name, oc_narration1,oc_narration2,'',oc_amount,0,oc_no,oc_date,oc_pay_mode,oc_received_bank,oc_cheque_no,oc_cheque_date,oc_remarks,oc_created_by,'" + GITAPI.dbFunctions.getdate() + "',oc_status,'" + Compnay.Replace("_", "") + "' " +
                       " from Other_Collection " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);
            }
            else if(vou_type == Acc_Data.G_Type_Expenses)
            {
                if (vou_no == "")
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "'");
                    Condition = "";
                }
                else
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "' and db_vour_refno=" + vou_no);
                    Condition = " where oc_id=" + vou_no;
                }

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                       " select '" + vou_type + "'," + G_Order_Expenses + ",oc_id,oc_date,oc_categoryid,oc_category,oc_ledger_id,oc_ledger_name, oc_narration1,oc_narration2,'',0,oc_amount,oc_no,oc_date,oc_pay_mode,oc_received_bank,oc_cheque_no,oc_cheque_date,oc_remarks,oc_created_by,'" + GITAPI.dbFunctions.getdate() + "',oc_status,'" + Compnay.Replace("_", "") + "' " +
                       " from Other_Collection " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);
            }
            else if (vou_type == Acc_Data.G_Type_Vehicle)
            {
                if (vou_no == "")
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "'");
                    Condition = "";
                }
                else
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "' and db_vour_refno=" + vou_no);
                    Condition = " where oc_id=" + vou_no;
                }

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                       " select '" + vou_type + "'," + G_Order_Vehicle + ",oc_id,oc_date,oc_categoryid,oc_category,oc_ledger_id,oc_ledger_name, oc_narration1,oc_narration2,'',0,oc_amount,oc_no,oc_date,oc_pay_mode,oc_received_bank,oc_cheque_no,oc_cheque_date,oc_remarks,oc_created_by,'" + GITAPI.dbFunctions.getdate() + "',oc_status,'" + Compnay.Replace("_", "") + "' " +
                       " from Other_Collection " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);
            }
            if (vou_type == Acc_Data.G_Type_Loan_Return)
            {
                if (vou_no == "")
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "'");
                    Condition = "";
                }
                else
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where (db_vourtype='" + Acc_Data.G_Type_Loan_Return + "' or db_vourtype='" + Acc_Data.G_Type_loan + "') and db_vour_refno=" + vou_no);
                    Condition = " where oc_id=" + vou_no;
                }

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                       " select '" + vou_type + "'," + G_Order_Loan_Return + ",oc_id,oc_date,oc_categoryid,oc_category,oc_ledger_id,oc_ledger_name, oc_narration1,oc_narration2,'',oc_amount,0,oc_no,oc_date,oc_pay_mode,oc_received_bank,oc_cheque_no,oc_cheque_date,oc_remarks,oc_created_by,'" + GITAPI.dbFunctions.getdate() + "',oc_status,'" + Compnay.Replace("_", "") + "' " +
                       " from Other_Collection " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);
            }
          
            else if (vou_type == Acc_Data.G_Type_loan)
            {
                if (vou_no == "")
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "'");
                    Condition = "";
                }
                else
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where (db_vourtype='" + Acc_Data.G_Type_Loan_Return + "' or db_vourtype='" + Acc_Data.G_Type_loan + "') and db_vour_refno=" + vou_no);
                    Condition = " where oc_id=" + vou_no;
                }

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                       " select '" + vou_type + "'," + G_Order_loan + ",oc_id,oc_date,oc_categoryid,oc_category,oc_ledger_id,oc_ledger_name, oc_narration1,oc_narration2,'',0,oc_amount,oc_no,oc_date,oc_pay_mode,oc_received_bank,oc_cheque_no,oc_cheque_date,oc_remarks,oc_created_by,'" + GITAPI.dbFunctions.getdate() + "',oc_status,'" + Compnay.Replace("_", "") + "' " +
                       " from Other_Collection " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);
            }

            else if (vou_type == G_Type_Contra)
            {

                if (vou_no == "")
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "'");
                    Condition = "";
                }
                else
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "' and db_vour_refno=" + vou_no);
                    Condition = " where c_id=" + vou_no;
                }

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                       " select '" + vou_type + "'," + G_Order_Contra + ",c_id,c_ref_date,0,'Bank From',c_from_account,Bank_Name+'-'+Account_Number, c_naration,'','',c_amount,0,c_ref_no,c_ref_date,c_pay_mode,c_to_account,c_cheque_no,c_cheque_date,c_remarks,c_created_by,'" + GITAPI.dbFunctions.getdate() + "',c_status,'" + Compnay.Replace("_", "") + "' " +
                       " from Contra left outer join  Bank_Master y on c_from_account=y.ID " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                    " select '" + vou_type + "'," + G_Order_Contra + ",c_id,c_ref_date,0,'Bank To',c_to_account,Bank_Name+'-'+Account_Number, c_naration,'','',0,c_amount,c_ref_no,c_ref_date,c_pay_mode,c_from_account,c_cheque_no,c_cheque_date,c_remarks,c_created_by,'" + GITAPI.dbFunctions.getdate() + "',c_status,'" + Compnay.Replace("_", "") + "' " +
                    " from Contra left outer join  Bank_Master y on c_to_account=y.ID " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);

            }
            else if (vou_type == G_Type_Payment)
            {

                if (vou_no == "")
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "'");
                    Condition = "";
                }
                else
                {
                    dt = GITAPI.dbFunctions.getTable("Delete from DayBook where db_vourtype='" + vou_type + "' and db_billno='" + vou_no+"' ");
                    Condition = " where cb_vtype='PAYMENT' and  cb_uniqno='" + vou_no+"'";
                }

                Query = "insert into daybook (db_vourtype,db_vou_order,db_vour_refno,db_date,db_group_id,db_group,db_ledger_id,db_ledger_name,db_narration1,db_narration2,db_narration3,db_cramt1,db_dbamt1,db_billno,db_billdate,db_paymode,db_received_bank,db_cheque_no,db_cheque_date,db_remarks,db_created_by,db_created_date,db_status,db_branch_id)" +
                       " select '" + vou_type + "'," + G_Order_Payment + ",cb_id,cb_date,0,'PAYMENT',cb_ledger_id,cus_name,cb_remarks,'','',0,cb_amountout,cb_uniqno,cb_date,dbo.get_ref_id(cb_pay_mode),cb_received_bank,cb_cheque_no,cb_cheque_date,cb_remarks,cb_created_by,'" + GITAPI.dbFunctions.getdate() + "',cb_status,'" + Compnay.Replace("_", "") + "' " +
                       " from Balance left outer join  Ledger_Master on cus_id=cb_ledger_id " + Condition;

                dt = GITAPI.dbFunctions.getTable(Query);
                dt = GITAPI.dbFunctions.getTable("select cb_ledger_id from balance where cb_vtype='PAYMENT' and  cb_uniqno='" + vou_no + "'");
                Balance_Update(dt.Rows[0][0].ToString(), "", Compnay);

            }
        }


        public static void Balance_Update(string Ledger_ID, string Type, string Compnay)
        {
            DataTable dt;
            string Query = "";
            string Condition = "";
            dt = GITAPI.dbFunctions.getTable("Delete from Amount_Balance where Ledger_ID=" + Ledger_ID);
            Condition = " (" + Ledger_ID + ",''," + Compnay.Replace("_", "") + ")";

            string Query1 = " insert into Amount_Balance " +
                " select * from fn_Amount_Balance" + Condition;
            dt = GITAPI.dbFunctions.getTable(Query1);
        }

    }
}
