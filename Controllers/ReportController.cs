using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;
using Newtonsoft.Json;
using System.Data.SqlClient;
using GenCode128;
using iTextSharp.text.html.simpleparser;
using System.Net;
using ClosedXML.Excel;

namespace Genuine_API.Controllers
{
    public class ReportController : Controller
    {
        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult Purchase_Order(string PO_No, string Company)
        {
            string counts = "1";

            string Query = " declare @val decimal(18,2) " +
                " set @val=100 set @val=@val/100 " +
                " select row_number() over(partition by BC_ID order by BC_ID,ID asc) as SNo, " +
                " BC_ID,BC_Name as BillHead, BranchName,BAddress1,BAddress2, " +
                " BPhone1,BPhone2,BGSTNo,BState,BStateCode,BTerms1,BTerms2,BTerms3,BTerms4, " +
                " BEmail,BFormat,BFilePath,BHead,BGSTPath,BESTPath, BBank,BAccNo,BIFSC,BBranch, " +
                " BAccName, Brand,Product,TName,PHSNCode,PUOM,PCode,PType, Customer,CAddress, " +
                " CAddr2,CCity,CContactno,CGSTNo,CState,CStateCode,CRoute, DCustomer,DAddress, " +
                " DAddr2,DCity,DContactno,DGSTNo,DState,DStateCode,DRoute, Invoice,sum(Qty) as Qty, " +
                " Date as Date,Price,[Price With Tax],sum(Amount) as Amount,DiscPer,sum(Disc) as Disc, " +
                " sum(TaxableValue) as TaxableValue, convert(decimal(18,2),[Price With Tax]*sum(Qty)) as BDisc, " +
                " sum(tot) as tot,sum(Freight) as Freight, GSTPer,CESS,Branch,Bankid,Userid,MRP, " +
                " Barcode,VehicleNo,SaleMode,Transport,ewaybillno,ewaybilldate,vaildupto,irn,ID,Cash,Card,SRoute,SCellNo,OB, " +
                " 'CGST%'=convert(decimal(18,2),case when CStateCode=BStateCode then (GSTPer/2) else 0 end), " +
                " 'SGST%'=convert(decimal(18,2),case when CStateCode=BStateCode then (GSTPer/2) else 0 end), " +
                " 'IGST%'=convert(decimal(18,2),case when CStateCode=BStateCode then 0 else (GSTPer) end), " +
                " 'CGST'=convert(decimal(18,2),case when CStateCode=BStateCode then sum(GST/2) else 0 end), " +
                " 'SGST'=convert(decimal(18,2),case when CStateCode=BStateCode then sum(GST/2) else 0 end), " +
                " 'IGST'=convert(decimal(18,2),case when CStateCode=BStateCode then 0 else sum(GST) end), " +
                " 'vat6'=convert(decimal(18,2),case when GSTPer=12 then sum(GST) else 0 end), " +
                " 'vat9'=convert(decimal(18,2),case when GSTPer=18 then sum(GST) else 0 end), " +
                " 'vat14'=convert(decimal(18,2),case when GSTPer=28 then sum(GST) else 0 end), " +
                " 'vat5'=convert(decimal(18,2),case when GSTPer=5 then sum(GST) else 0 end), " +
                " 'TaxV0'=convert(decimal(18,2),case when GSTPer=0 then sum(TaxableValue) else 0 end)," +
                " 'TaxV5'=convert(decimal(18,2),case when GSTPer=5 then sum(TaxableValue) else 0 end)," +
                " 'TaxV12'=convert(decimal(18,2),case when GSTPer=12 then sum(TaxableValue) else 0 end)," +
                " 'TaxV18'=convert(decimal(18,2),case when GSTPer=18 then sum(TaxableValue) else 0 end)," +
                " 'TaxV28'=convert(decimal(18,2),case when GSTPer=28 then sum(TaxableValue) else 0 end)," +
                " convert(decimal(18,2),sum(TDisc)) as TDisc, case when CStateCode=BStateCode then 'GST' " +
                " else 'IGST' end as TaxCalc, Customer+'-'+Invoice+'('+Date+')' as BillName,Due_Date from " +
                " (SELECT CM_Name as BranchName,CM_Address1 as BAddress1,CM_Address2 as BAddress2, " +
                " CM_Phone_off as BPhone1,CM_Phone_Res as BPhone2, CM_GST_No as BGSTNo,CM_State as BState, " +
                " CM_State_Code as BStateCode,CM_Sales_Fotter as BTerms1,CM_Sales_Terms as BTerms2, " +
                " CM_Sales_Term as BTerms3,CM_Sales_Footer as BTerms4,CM_Email_ID as BEmail,'' as BFormat, " +
                " '' as BFilePath, 'DELIVERY CHALLAN' as BHead,'' as BGSTPath,'' as BESTPath, " +
                " CM_Bank_Name as BBank,CM_Acc_Number as BAccNo,CM_IFSC as BIFSC,CM_Branch as BBranch, " +
                " CM_Acc_Name as BAccName,pm_brand as Brand,dc_prod_name as Product,dc_prod_name as TName, " +
                " dc_hsn_code as PHSNCode,dbo.get_ref_value(dc_uom) as PUOM,dc_prod_code as PCode, " +
                " pm_type as PType,isnull(c.cus_name,a.dc_ledger_name) as Customer, " +
                " isnull(a.dc_address1,c.cus_address1) as CAddress,isnull(a.dc_address2,c.cus_address2) " +
                " as CAddr2,ISNULL(po.dc_city, c.Cus_City) + '-' + ISNULL(po.dc_pincode, c.cus_pincode) as CCity, isnull(a.dc_contact_no,c.cus_contact_number) as CContactno, " +
                " isnull(c.Cus_GSTIn,a.dc_gstin) as CGSTNo,c.Cus_state as CState, isnull(c.Cus_SCode,'33') as CStateCode,c.cus_area as CRoute, " +
                " isnull(CM_Name,'') as DCustomer,isnull(po.dc_dis_address1,CM_Address1) as DAddress,isnull(po.dc_dis_address2,CM_Address2) as DAddr2," +
                " ISNULL(po.dc_dis_city, CM_Address4) + '-' + ISNULL(po.dc_dis_pincode, CM_Address5) as DCity, CM_Phone_off as DContactno, CM_GST_No as DGSTNo,CM_State as DState,CM_State_Code as DStateCode, " +
                " '' as DRoute, a.dc_bill_no as Invoice,dc_qty as Qty, " +
                " convert(varchar(2),a.dc_date,104)+'-'+right(convert(varchar(6),a.dc_date,106),3)+'-'+(convert(varchar,right(year(a.dc_date),2))) as Date, " +
                " right(a.dc_created_date,8) as Time, convert(decimal(18,2),(dc_rate*@val)) as Price, " +
                " convert(decimal(18,2),round((dc_rate*@val)*((100+a.dc_gst_per)/100),2)) as [Price With Tax], " +
                " convert(decimal(18,2),(dc_qty*(dc_rate*@val))) as Amount, convert(decimal(18,2),(po.dc_disc_per)) " +
                " as DiscPer, convert(decimal(18,2),a.dc_disc_amt) AS Disc, convert(decimal(18,2),a.dc_taxable_amount) " +
                " as TaxableValue, convert(decimal(18,2),a.dc_net_amt) as tot, convert(decimal(18,2),0) as Freight, " +
                " 'GST'=convert(decimal(18,2),a.dc_gst_amt), 'GSTPer'=convert(decimal(18,0),(((a.dc_gst_per)))), " +
                " 'CESS'=0, a.dc_company as Branch, 1 as Bankid, a.dc_created_by as Userid, dc_mrp as MRP, " +
                " a.dc_remarks as Barcode,dc_vehicleno as VehicleNo, a.dc_pay_mode as SaleMode, dc_transname as Transport,dc_ewaybill as ewaybillno,CONVERT(varchar, dc_ewaybilldate, 103) + ' ' + CONVERT(varchar, dc_ewaybilldate, 108) as ewaybilldate, CONVERT(varchar, dc_vaildupto, 103) + ' ' + CONVERT(varchar, dc_vaildupto, 108) as vaildupto,'' as irn, " +
                " a.dc_id as ID, 0 as Cash, 0 as Card, '' as SRoute, cus_contact_number as SCellNo, 0 as OB, " +
                " 0-0 as TDisc, convert(varchar(2),po.dc_due_date,104)+'-'+right(convert(varchar(6),po.dc_due_date,106),3)+'-'+(convert(varchar,right(year(po.dc_due_date),2))) as Due_Date " +
                " FROM Delivery_Challan_details a " +
                " left outer join Delivery_Challan po on po.dc_no=a.dc_no " +
                " left outer join product_master on pm_id=a.dc_prod_id " +
                " left outer join Ledger_Master c on c.cus_id=a.dc_ledger_id " +
                " left outer join Company_Master on CM_ID=a.dc_company where a.dc_no='" + PO_No + "' and a.dc_company='" + Company.Replace("_", "") + "' ) " +
                " as x left outer join billcopies on BC_ID<='" + counts + "' " +
                " group by BHead,BranchName,BAddress1,BAddress2,BPhone1,BPhone2,BGSTNo,BState,BStateCode, " +
                " BTerms1,BTerms2,BTerms3,BTerms4,BEmail,BFormat,BFilePath,BGSTPath,BESTPath, BBank,BAccNo, " +
                " BIFSC,BBranch,BAccName,BC_ID,BC_Name,Brand,Product,TName,PHSNCode,PUOM,PCode,PType, " +
                " Customer,CAddress,CAddr2,CCity,CContactno,CGSTNo,CState,CStateCode,CRoute, DCustomer, " +
                " DAddress,DAddr2,DCity,DContactno,DGSTNo,DState,DStateCode,DRoute, Invoice,Date,Price, " +
                " [Price With Tax],DiscPer,GSTPer,CESS,Branch,Bankid,Userid,MRP,Barcode,VehicleNo,SaleMode, " +
                " Transport,ewaybillno,ewaybilldate,vaildupto,irn,ID,Cash,Card, SRoute,SCellNo,OB,Time,Due_Date order by BC_ID,ID asc ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);
            ReportClass rptH = new ReportClass();
            rptH.FileName = Server.MapPath("~/Crystal_Report/GST_BillFormat_A4_7.rpt");

            rptH.Load();
            rptH.SetDataSource(dt);

            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");

        }


        private DataTable toDataTable(JArray json)
        {
            var result = new DataTable();
            var jArray = json;
            //Initialize the columns, If you know the row type, replace this   
            foreach (var row in jArray)
            {
                foreach (var jToken in row)
                {
                    var jproperty = jToken as JProperty;
                    if (jproperty == null) continue;
                    if (result.Columns[jproperty.Name] == null)
                        result.Columns.Add(jproperty.Name, typeof(string));
                }
            }
            foreach (var row in jArray)
            {
                var datarow = result.NewRow();
                foreach (var jToken in row)
                {
                    var jProperty = jToken as JProperty;
                    if (jProperty == null) continue;
                    datarow[jProperty.Name] = jProperty.Value.ToString();
                }
                result.Rows.Add(datarow);
            }

            return result;
        }





        string Company_Name = "HI-TEX CREATIONS";
        string Company_Address1 = "180,SOWDESWARI NAGAR,";
        string Company_Address2 = "AGAR,ELAMPILLAI";
        string Company_Address3 = "SALEM";
        string Company_Address4 = "";
        string GST_NO = "";
        string Phone_NO = "";


        public void Get_Compay_Details(string id)
        {
            string cmp_Id = "";
            cmp_Id = id.Replace('_', ' ');
            DataTable dt = GITAPI.dbFunctions.getTable("select * from Company_Master where CM_ID=" + cmp_Id.Trim());
            if (dt.Rows.Count > 0)
            {
                Company_Name = dt.Rows[0]["CM_Name"].ToString();

                Company_Address1 = dt.Rows[0]["CM_Address1"].ToString();

                Company_Address2 = dt.Rows[0]["CM_Address2"].ToString();

                Company_Address3 = dt.Rows[0]["CM_Address3"].ToString();
                //Company_Address4 = dt.Rows[0]["City"].ToString();
                GST_NO = dt.Rows[0]["CM_GST_No"].ToString();
                Phone_NO = dt.Rows[0]["CM_Phone_off"].ToString();


            }

        }

        public ActionResult JsontToExcel_Upload(string User, string Company, string File_Name, string File_Type)
        {
            JObject jsonData;
            DataTable dd = GITAPI.dbFunctions.getTable("select * from Excel_Data" + Company + " where user_ID='" + User + "'");

            if (dd.Rows.Count > 0)
            {
                jsonData = JObject.Parse(dd.Rows[0]["Data"].ToString());

                dynamic json = jsonData;
                string ID = GITAPI.dbFunctions.isnull(json.ID, "");
                string FileName = GITAPI.dbFunctions.isnull(json.FileName, "Data");
                string Report_Name = GITAPI.dbFunctions.isnull(json.Report_Name, "");


                Newtonsoft.Json.Linq.JArray items = json.items;
                Newtonsoft.Json.Linq.JArray Header = json.Header;

                //DataTable dt = toDataTable(items);

                DataTable th = toDataTable(Header);

                Get_Compay_Details(Company);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "utf-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                Response.Write("<BR><BR><BR>");
                //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
                Response.Write("<Table border='1' bgColor='#ffffff' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:12.0pt; font-family:Times New Roman; background:white;'> ");

                //"  <caption ><font style='font-size:13.0pt;'><B>" + Company_Name + "</b></font><br> " + Company_Address1 + "<br>" + Company_Address2 + "" + Company_Address3 + "<br> <B>" + Report_Name + "</B> </caption> ");
                //am getting my grid's column headers


                Response.Write(" <TR>");
                for (int j = 0; j < th.Rows.Count; j++)
                {      //write in new column
                    if (j == 0)
                    {
                        Response.Write("<Td><B> # </B></Td>");
                    }

                    Response.Write("<Td>");
                    //Get column headers  and make it as bold in excel columns
                    Response.Write("<B>");
                    Response.Write(th.Rows[j]["Name"].ToString());
                    Response.Write("</B>");
                    Response.Write("</Td>");
                }
                Response.Write("</TR>");

                //for (int j = 0; j < dt.Rows.Count; j++)
                {//write in new row
                    Response.Write("<TR style='font-size:11.0pt; font-family:Times New Roman; background:white;'>");
                    for (int i = 0; i < th.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            //Response.Write("<Td> " + (j + 1) + "</Td>");
                        }

                        Response.Write("<Td>");
                        //Response.Write(dt.Rows[j]["" + th.Rows[i]["Field"].ToString() + ""].ToString());
                        Response.Write("</Td>");
                    }

                    Response.Write("</TR>");
                }
                Response.Write("</Table>");
                Response.Write("</font>");




                Response.Flush();
                Response.End();
            }
            return View("Index");


        }



          


        public ActionResult JsontToExcel(string User, string Company, string File_Name, string File_Type)
        {
            // Fetch data from the database
            DataTable dd = GITAPI.dbFunctions.getTable($"select * from Excel_Data{Company} where user_ID='{User}'");

            if (dd.Rows.Count > 0)
            {
                JObject jsonData = JObject.Parse(dd.Rows[0]["Data"].ToString());

                // Extract dynamic data
                dynamic json = jsonData;
                string FileName = GITAPI.dbFunctions.isnull(json.FileName, "Data");
                string Report_Name = GITAPI.dbFunctions.isnull(json.Report_Name, "");
                JArray items = json.items;
                JArray Header = json.Header;

                DataTable dt = toDataTable(items); // For items (data)
                DataTable th = toDataTable(Header); // For header 

                // Set company details if necessary
                Get_Compay_Details(Company);

                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(Report_Name);

                        // Add company details at the top
                        int headerRow = 1;
                        var companyNameCell = ws.Cell(headerRow, 1);
                        companyNameCell.Value = Company_Name;
                        companyNameCell.Style.Font.Bold = true;
                        ws.Range(headerRow, 1, headerRow, th.Rows.Count + 1).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        headerRow++;
                        if(!Company_Address1.Equals(""))
                        {
                            ws.Cell(headerRow, 1).Value = Company_Address1;
                            ws.Range(headerRow, 1, headerRow, th.Rows.Count + 1).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            headerRow++;
                        }
                        if (!Company_Address2.Equals(""))
                        {
                            ws.Cell(headerRow, 1).Value = Company_Address2;
                            ws.Range(headerRow, 1, headerRow, th.Rows.Count + 1).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            headerRow++;
                        }
                        if (!Company_Address3.Equals(""))
                        {
                            ws.Cell(headerRow, 1).Value = Company_Address3;
                            ws.Range(headerRow, 1, headerRow, th.Rows.Count + 1).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            headerRow++;
                        }
                        if (!Report_Name.Equals(""))
                        {
                            var Report_NameCell = ws.Cell(headerRow, 1);
                            Report_NameCell.Value = Report_Name;
                            Report_NameCell.Style.Font.Bold = true;
                            ws.Range(headerRow, 1, headerRow, th.Rows.Count + 1).Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                            headerRow++;
                        }
                        // Increment row for data header
                        //headerRow += 2;
                        // Add headers (including S.No)
                        var headerCell1 = ws.Cell(headerRow, 1);
                        headerCell1.Value = "#";
                        headerCell1.Style.Font.Bold = true;
                        headerCell1.Style.Fill.BackgroundColor = XLColor.LightGray;  // You can choose any color
                        headerCell1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        headerCell1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        headerCell1.Style.Border.OutsideBorderColor = XLColor.Black;

                        for (int col = 0; col < th.Rows.Count; col++)
                        {
                            var headerCell = ws.Cell(headerRow, col + 2);
                            headerCell.Value = th.Rows[col]["Name"].ToString();
                            headerCell.Style.Font.Bold = true;
                            headerCell.Style.Fill.BackgroundColor = XLColor.LightGray;  // You can choose any color
                            headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            headerCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            headerCell.Style.Border.OutsideBorderColor = XLColor.Black;
                        }

                        // Add Data Table (items)
                        int startRow = headerRow + 1; // Start below the header row

                        // Dynamically insert data into the worksheet
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            var RowCell1 = ws.Cell(startRow + j, 1);
                            RowCell1.Value = j + 1;
                            RowCell1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            RowCell1.Style.Border.OutsideBorderColor = XLColor.Black;
                            for (int i = 0; i < th.Rows.Count; i++)
                            {
                                var columnName = th.Rows[i]["Field"].ToString();
                                var cellValue = dt.Rows[j][columnName]?.ToString() ?? string.Empty;
                                //ws.Cell(startRow + j, i + 2).Value = cellValue;
                                var RowCell = ws.Cell(startRow + j, i + 2);
                                //RowCell.Value = cellValue;
                                // Check if the value is a time
                                if (th.Rows[i]["Type"].ToString().Equals("Time"))
                                {
                                    RowCell.Value = cellValue; // Assign the parsed time
                                    RowCell.Style.NumberFormat.Format = "h:mm AM/PM"; // Set desired time format (12-hour clock with AM/PM)
                                }
                                else if (th.Rows[i]["Type"].ToString().Equals("Date"))
                                {
                                    // For date values, handle them separately
                                    RowCell.Value = cellValue;
                                    RowCell.Style.NumberFormat.Format = "dd-MMM-yyyy"; // Example: Set date format
                                }
                                else if (th.Rows[i]["Type"].ToString().Equals("datetime-local"))
                                {
                                    // For date values, handle them separately
                                    RowCell.Value = cellValue;
                                    RowCell.Style.NumberFormat.Format = "dd-MMM-yyyy h:mm AM/PM"; // Example: Set date format
                                }
                                else
                                {
                                    // For non-date/time values
                                    RowCell.Value = cellValue;
                                }
                                RowCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                RowCell.Style.Border.OutsideBorderColor = XLColor.Black;
                            }
                        }
                        // Add Total Row
                        int totalRow = startRow + dt.Rows.Count;
                        var totalCell1 = ws.Cell(totalRow, 1);
                        totalCell1.Value = "";
                        totalCell1.Style.Font.Bold = true;
                        totalCell1.Style.Fill.BackgroundColor = XLColor.Wheat;  // You can choose any color
                        totalCell1.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        totalCell1.Style.Border.OutsideBorderColor = XLColor.Black;

                        for (int i = 0; i < th.Rows.Count; i++)
                        {
                            var columnName = th.Rows[i]["Field"].ToString();

                            // Check if the column is numeric
                            decimal columnTotal = 0;
                            bool isNumeric = true;

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (decimal.TryParse(dt.Rows[j][columnName]?.ToString(), out decimal value))
                                {
                                    columnTotal += value;
                                }
                                else
                                {
                                    isNumeric = false;
                                    break;
                                }
                            }

                            if (isNumeric)
                            {
                                var totalCell = ws.Cell(totalRow, i + 2);
                                totalCell.Value = columnTotal; // Shift by 1 for S.No
                                totalCell.Style.Font.Bold = true;
                                totalCell.Style.Fill.BackgroundColor = XLColor.Wheat;  // You can choose any color
                                totalCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                totalCell.Style.Border.OutsideBorderColor = XLColor.Black;
                            }
                            else
                            {
                                var totalCell = ws.Cell(totalRow, i + 2);
                                totalCell.Value = "";
                                totalCell.Style.Font.Bold = true;
                                totalCell.Style.Fill.BackgroundColor = XLColor.Wheat;  // You can choose any color
                                totalCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                totalCell.Style.Border.OutsideBorderColor = XLColor.Black;
                            }
                        }

                        // Adjust column widths to fit the contents
                        ws.Columns().AdjustToContents();

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");

                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception and handle it appropriately
                    // For example: Log.Error(ex);
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "An error occurred while generating the report.");
                }
            }

            return new EmptyResult(); // No need to redirect as the response is handled directly
        }

        public ActionResult JsontToExcel_1(string User, string Company, string File_Name, string File_Type)
        {
            JObject jsonData;
            DataTable dd = GITAPI.dbFunctions.getTable("select * from Excel_Data" + Company + " where user_ID='" + User + "'");

            if (dd.Rows.Count > 0)
            {
                jsonData = JObject.Parse(dd.Rows[0]["Data"].ToString());

                dynamic json = jsonData;
                string ID = GITAPI.dbFunctions.isnull(json.ID, "");
                string FileName = GITAPI.dbFunctions.isnull(json.FileName, "Data");
                string Report_Name = GITAPI.dbFunctions.isnull(json.Report_Name, "");
                string Group_Name = GITAPI.dbFunctions.isnull(json.Group_Name, "");


                Newtonsoft.Json.Linq.JArray items = json.items;
                Newtonsoft.Json.Linq.JArray Header = json.Header;

                DataTable dt = toDataTable(items);

                DataTable th = toDataTable(Header);

                Get_Compay_Details(Company);

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + FileName + ".xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "utf-8";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
                Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
                Response.Write("<BR><BR><BR>");
                //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
                Response.Write("<Table border='1' bgColor='#ffffff' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:12.0pt; font-family:Times New Roman; background:white;'> " +

                "  <caption ><font style='font-size:13.0pt;'><B>" + Company_Name + "</b></font><br> " + Company_Address1 + "<br>" + Company_Address2 + "" + Company_Address3 + "<br> <B>" + Report_Name + "<br> <B>" + Group_Name + "</B> </caption> ");
                //am getting my grid's column headers


                Response.Write(" <TR>");
                for (int j = 0; j < th.Rows.Count; j++)
                {      //write in new column
                    if (j == 0)
                    {
                        Response.Write("<Td><B> # </B></Td>");
                    }

                    Response.Write("<Td>");
                    //Get column headers  and make it as bold in excel columns
                    Response.Write("<B>");
                    Response.Write(th.Rows[j]["Name"].ToString());
                    Response.Write("</B>");
                    Response.Write("</Td>");
                }
                Response.Write("</TR>");
                int s = 1;
                string date = "";
                for (int j = 0; j < dt.Rows.Count; j++)
                {//write in new row
                    Response.Write("<TR style='font-size:11.0pt; font-family:Times New Roman; background:white;'>");
                    for (int i = 0; i < th.Rows.Count; i++)
                    {
                        try
                        {
                            date = dt.Rows[j - 1]["tpt_date"].ToString();
                        }
                        catch
                        {
                            date = dt.Rows[j]["tpt_date"].ToString();
                        }
                        if (i == 0)
                        {
                            if (dt.Rows[j]["tpt_date"].ToString() == date)
                            {
                                if (j == 0)
                                {
                                    Response.Write("<Td> " + (s) + "</Td>");
                                }
                                else
                                {
                                    Response.Write("<Td>  </Td>");
                                }
                            }
                            else
                            {
                                s = s + 1;
                                Response.Write("<Td> " + (s) + "</Td>");

                            }
                        }

                        Response.Write("<Td>");
                        Response.Write(dt.Rows[j]["" + th.Rows[i]["Field"].ToString() + ""].ToString());
                        Response.Write("</Td>");
                    }

                    Response.Write("</TR>");
                }




                Response.Write("<TR style='font-size:11.0pt; font-family:Times New Roman; background:wheat;'>");


                for (int i = 0; i < th.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        Response.Write("<Td><B></B></Td>");
                    }

                    decimal d = 0.0m;
                    int l_ = 0;

                    for (int k = 0; k < dt.Rows.Count; k++)
                    {

                        try
                        {
                            d += decimal.Parse(dt.Rows[k]["" + th.Rows[i]["Field"].ToString() + ""].ToString());
                        }
                        catch
                        {
                            l_ = 1;
                            break;
                        }
                    }

                    Response.Write("<Td>");
                    try
                    {
                        if (l_ == 0)
                        {

                            Response.Write(d);
                        }
                    }
                    catch { }
                    Response.Write("</Td>");
                }

                Response.Write("</TR>");

                dynamic result = from tab in dt.AsEnumerable()
                                 group tab by tab["tpt_projectname"]
                     into groupDt
                                 select new
                                 {
                                     tpt_projectname = groupDt.Key,
                                     tpt_amount = groupDt.Sum((r) => decimal.Parse(r["tpt_amount"].ToString())),
                                     //FixedSalary = groupDt.Select(r => r["FixedSalary"].ToString()).FirstOrDeafult()
                                 };

                string json3 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json3);
                // Copy rows to DataTable
                DataTable dtTemp = pDt;//toDataTable(result);



                string str = "[{ \"Field\":\"tpt_projectname\" ,\"Name\":\"Project Name\",\"align\":\"\"}," +
                              " { \"Field\":\"tpt_amount\" ,\"Name\":\"Amount\",\"align\":\"right\"}]";

                Newtonsoft.Json.Linq.JArray item1 = Newtonsoft.Json.Linq.JArray.Parse(str);

                DataTable th1 = toDataTable(item1);
                Response.Write("<br><br>");
                for (int j = 0; j < dtTemp.Rows.Count; j++)
                {//write in new row

                    Response.Write("<TR style='font-size:11.0pt; font-family:Times New Roman; background:white;'>");

                    for (int i = 0; i < th1.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            Response.Write("<Td> </Td>");
                            Response.Write("<Td> " + (j + 1) + "</Td>");
                        }

                        Response.Write("<Td>");
                        Response.Write(dtTemp.Rows[j]["" + th1.Rows[i]["Field"].ToString() + ""].ToString());
                        Response.Write("</Td>");
                    }

                    Response.Write("</TR>");
                }




                Response.Write("<TR style='font-size:11.0pt; font-family:Times New Roman; background:wheat;'>");


                for (int i = 0; i < th1.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        Response.Write("<Td><B></B></Td>");
                        Response.Write("<Td><B></B></Td>");
                    }

                    decimal d = 0.0m;
                    int l_ = 0;

                    for (int k = 0; k < dtTemp.Rows.Count; k++)
                    {

                        try
                        {
                            d += decimal.Parse(dtTemp.Rows[k]["" + th1.Rows[i]["Field"].ToString() + ""].ToString());
                        }
                        catch
                        {
                            l_ = 1;
                            break;
                        }
                    }

                    Response.Write("<Td>");
                    try
                    {
                        if (l_ == 0)
                        {

                            Response.Write(d);
                        }
                    }
                    catch { }
                    Response.Write("</Td>");
                }

                Response.Write("</TR>");
                Response.Write("</Table>");
                Response.Write("</font>");




                Response.Flush();
                Response.End();
            }
            return View("Index");


        }



    }
}
