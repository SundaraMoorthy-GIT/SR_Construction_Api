using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Genuine_API.Controllers
{
    public class PDFController : Controller
    {


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


        public void load_Page_Setting(string Company)
        {

            DataTable dsp = GITAPI.dbFunctions.getTable("select * from pdf_File_Setting" + Company + " where  ID=1");
            if (dsp.Rows.Count > 0)
            {


                GITAPI.dbFunctions.PDF_Page_Size = dsp.Rows[0]["Page_Size"].ToString().ToLower();
                GITAPI.dbFunctions.PDF_Page_Orentation = dsp.Rows[0]["Page_Orientation"].ToString().ToLower();
                GITAPI.dbFunctions.PDF_Top = float.Parse(dsp.Rows[0]["Margins_Top"].ToString());
                GITAPI.dbFunctions.PDF_Bottom = float.Parse(dsp.Rows[0]["Margins_Bottom"].ToString());
                GITAPI.dbFunctions.PDF_left = float.Parse(dsp.Rows[0]["Margins_Left"].ToString());
                GITAPI.dbFunctions.PDF_Right = float.Parse(dsp.Rows[0]["Margins_Right"].ToString());

                GITAPI.dbFunctions.PDF_Font_Family = dsp.Rows[0]["Font_Family"].ToString();
                GITAPI.dbFunctions.PDF_Font_Size = float.Parse(dsp.Rows[0]["Font_Size"].ToString());


                GITAPI.dbFunctions.PDF_Compnay_Name = dsp.Rows[0]["Compnay_Name"].ToString().ToLower();
                GITAPI.dbFunctions.PDF_Compnay_Address = dsp.Rows[0]["Compnay_Address"].ToString().ToLower();
                GITAPI.dbFunctions.PDF_Page_Number = dsp.Rows[0]["Page_No"].ToString().ToLower();
                GITAPI.dbFunctions.PDF_Generated_by = dsp.Rows[0]["Generated_By"].ToString().ToLower();
                GITAPI.dbFunctions.PDF_Generated_Date = dsp.Rows[0]["Generated_Date"].ToString().ToLower();
                GITAPI.dbFunctions.PDF_Generated_Time = dsp.Rows[0]["Generated_Time"].ToString().ToLower();


                try
                {
                    GITAPI.dbFunctions.PDF_Header_Font = dsp.Rows[0]["Header_Font"].ToString().ToLower();
                    GITAPI.dbFunctions.PDF_Header_Font_Size = int.Parse(dsp.Rows[0]["Header_Font_Size"].ToString());
                    GITAPI.dbFunctions.PDF_Logo_Height = int.Parse(dsp.Rows[0]["Logo_Height"].ToString());
                    GITAPI.dbFunctions.PDF_Logo_Width = int.Parse(dsp.Rows[0]["Logo_Width"].ToString());
                    GITAPI.dbFunctions.PDF_Color_R = int.Parse(dsp.Rows[0]["PDF_Color_R"].ToString());
                    GITAPI.dbFunctions.PDF_Color_G = int.Parse(dsp.Rows[0]["PDF_Color_G"].ToString());
                    GITAPI.dbFunctions.PDF_Color_B = int.Parse(dsp.Rows[0]["PDF_Color_B"].ToString());


                }
                catch
                {
                }
            }

        }


        public void Load_Comapny(string Company)
        {

            DataTable dd = GITAPI.dbFunctions.getTable("select * from Company_Master where CM_ID='" + Company.Replace("_", "") + "'");

            GITAPI.dbFunctions.CM_Code = dd.Rows[0]["CM_Code"].ToString();
            GITAPI.dbFunctions.CM_Name = dd.Rows[0]["CM_Name"].ToString();
            GITAPI.dbFunctions.CM_Address1 = dd.Rows[0]["CM_Address1"].ToString();
            GITAPI.dbFunctions.CM_Address2 = dd.Rows[0]["CM_Address2"].ToString();
            GITAPI.dbFunctions.CM_Address3 = dd.Rows[0]["CM_Address3"].ToString();
            GITAPI.dbFunctions.CM_Address4 = dd.Rows[0]["CM_Address4"].ToString();
            GITAPI.dbFunctions.CM_Address5 = dd.Rows[0]["CM_Address5"].ToString();
            GITAPI.dbFunctions.CM_Email_ID = dd.Rows[0]["CM_Email_ID"].ToString();
            GITAPI.dbFunctions.CM_GST_No = dd.Rows[0]["CM_GST_No"].ToString();
            GITAPI.dbFunctions.CM_State_Code = dd.Rows[0]["CM_State_Code"].ToString();
            GITAPI.dbFunctions.CM_State = dd.Rows[0]["CM_State"].ToString();

            GITAPI.dbFunctions.CM_Bank_Name = dd.Rows[0]["CM_Bank_Name"].ToString();
            GITAPI.dbFunctions.CM_Acc_Number = dd.Rows[0]["CM_Acc_Number"].ToString();
            GITAPI.dbFunctions.CM_IFSC = dd.Rows[0]["CM_IFSC"].ToString();
            GITAPI.dbFunctions.CM_Branch = dd.Rows[0]["CM_Branch"].ToString();
            GITAPI.dbFunctions.CM_Sales_Term = dd.Rows[0]["CM_Sales_Term"].ToString();


        }


        public float[] GetHeaderWidths(Font font, params string[] headers1)
        {
            var total = 0;
            string[] headers = new string[headers1.Length + 1];
            headers[0] = GITAPI.dbFunctions.S_No;
            for (int i = 0; i < headers1.Length; i++)
            {
                headers[i + 1] = headers1[i];
            }
            var columns = headers.Length;
            var widths = new int[columns];
            for (var i = 0; i < columns; ++i)
            {
                var w = font.GetCalculatedBaseFont(true).GetWidth(headers[i]);
                total += w;
                widths[i] = w;
            }
            var result = new float[columns];
            for (var i = 0; i < columns; ++i)
            {
                result[i] = (float)widths[i] / total * 100;
            }
            return result;
        }

        public string strFormat(string data, int len)
        {
            string d = "                                           " + data;

            return d.Substring(d.Length - len, len);
        }


        public string User_Name = "";

        public class PageEventHelper : PdfPageEventHelper
        {
            PdfContentByte cb;
            PdfTemplate template;


            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                cb = writer.DirectContent;
                template = cb.CreateTemplate(300, 50);
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                int pageN = writer.PageNumber;
                String text = "";
                //float len = this.RunDateFont.BaseFont.GetWidthPoint(text, this.RunDateFont.Size);
                if (GITAPI.dbFunctions.PDF_Generated_by == "true")
                    text += "User :" + GITAPI.dbFunctions.username + "                                                                               ";
                if (GITAPI.dbFunctions.PDF_Page_Number == "true")
                    text += "Page " + pageN.ToString() + " of ";


                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                float len = bf.GetWidthPoint(text, 8);


                iTextSharp.text.Rectangle pageSize = document.PageSize;

                cb.SetRGBColorFill(100, 100, 100);

                cb.BeginText();



                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(document.LeftMargin + 5, pageSize.GetBottom(document.BottomMargin) + 5);
                cb.ShowText(text);

                cb.EndText();
                cb.AddTemplate(template, document.LeftMargin + len + 5, pageSize.GetBottom(document.BottomMargin) + 5);
            }

            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);

                template.BeginText();

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                template.SetFontAndSize(bf, 8);
                template.SetTextMatrix(0, 0);

                string PN = "";
                if (GITAPI.dbFunctions.PDF_Page_Number == "true")
                {
                    PN = (writer.PageNumber - 1).ToString();
                }

                string Date = "";
                string Time = "";


                if (GITAPI.dbFunctions.PDF_Generated_Date == "true")
                    Date = System.DateTime.Now.ToString("dd-MM-yyyy");

                if (GITAPI.dbFunctions.PDF_Generated_Date == "true")
                    Time = System.DateTime.Now.ToString("hh:mm tt");

                template.ShowText("" + PN + "                                                                     " + Date + " " + Time);
                template.EndText();
            }
        }

        public FileResult Export_Pdf(string User, string Company, string File_Name, string File_Type)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;
                DataTable dd = GITAPI.dbFunctions.getTable("select * from Excel_Data" + Company + " where user_ID='" + User + "'");

                if (dd.Rows.Count > 0)
                {
                    jsonData = JObject.Parse(dd.Rows[0]["Data"].ToString());

                    dynamic json = jsonData;
                    string ID = GITAPI.dbFunctions.isnull(json.ID, "");
                    string FileName = GITAPI.dbFunctions.isnull(json.FileName, "Data");
                    File_Name = FileName;
                    string Report_Name = GITAPI.dbFunctions.isnull(json.Report_Name, "");
                    string Company_Name = GITAPI.dbFunctions.isnull(json.Company_Name, "");

                    GITAPI.dbFunctions.Total_Row = GITAPI.dbFunctions.isnull(json.Total_Row, "");

                    GITAPI.dbFunctions.Total_Amount = GITAPI.dbFunctions.isnull(json.Total_Amount, "");
                    string Head_Type = GITAPI.dbFunctions.isnull(json.Head_Type, "name_only");

                    Newtonsoft.Json.Linq.JArray items = json.items;
                    Newtonsoft.Json.Linq.JArray Header1 = json.Header;

                    //string Company_Data = GITAPI.dbFunctions.isnull(json.Company_Data, "");
                    dynamic CN_json = json.Company_Data;

                    string CM_Code = GITAPI.dbFunctions.isnull(CN_json.CM_Code, "");
                    string CM_Name = GITAPI.dbFunctions.isnull(CN_json.CM_Name, "");
                    string CM_Address1 = GITAPI.dbFunctions.isnull(CN_json.CM_Address1, "");
                    string CM_Address2 = GITAPI.dbFunctions.isnull(CN_json.CM_Address2, "");
                    string CM_Address3 = GITAPI.dbFunctions.isnull(CN_json.CM_Address3, "");
                    string CM_Address4 = GITAPI.dbFunctions.isnull(CN_json.CM_Address4, "");
                    string CM_Address5 = GITAPI.dbFunctions.isnull(CN_json.CM_Address5, "");
                    string CM_Email_ID = GITAPI.dbFunctions.isnull(CN_json.CM_Email_ID, "");
                    string CM_GST_No = GITAPI.dbFunctions.isnull(CN_json.CM_GST_No, "");
                    string CM_State_Code = GITAPI.dbFunctions.isnull(CN_json.CM_State_Code, "");
                    string CM_State = GITAPI.dbFunctions.isnull(CN_json.CM_State, "");





                    DataTable td = toDataTable(items);
                    DataTable th = toDataTable(Header1);
                    load_Page_Setting(Company);

                    Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);
                    if (GITAPI.dbFunctions.PDF_Page_Size == "a4")
                    {
                        document = new Document(PageSize.A4, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                        if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                        {

                            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        }

                    }
                    else if (GITAPI.dbFunctions.PDF_Page_Size == "a3")
                    {
                        document = new Document(PageSize.A3, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                        if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                        {

                            document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
                        }
                    }
                    else if (GITAPI.dbFunctions.PDF_Page_Size == "a5")
                    {
                        document = new Document(PageSize.A5, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                        if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                        {

                            document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                        }
                    }



                    PdfWriter writer = PdfWriter.GetInstance(document, stream);

                    writer.PageEvent = new Border1();

                    PageEventHelper pageEventHelper = new PageEventHelper();
                    writer.PageEvent = pageEventHelper;
                    document.Open();
                    GITAPI.dbFunctions.PDF_Bottom = 10;



                    FontFactory.RegisterDirectories();

                    iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
                    iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);
                    iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                    iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                    iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
                    iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                    iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                    iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
                    iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
                    iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                    iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);



                    iTextSharp.text.Font subject = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                    subject.SetColor(105, 105, 105);
                    small_Font.SetColor(105, 105, 105);



                    document.Open();


                    Paragraph title;
                    Paragraph text;




                    Paragraph Header;

                    Header = new Paragraph(CM_Name, fn_Header);
                    Header.Alignment = Element.ALIGN_CENTER;
                    document.Add(Header);


                    string Address = "";
                    if (Head_Type.ToLower().Equals("name_only"))
                    {
                        if (CM_Address1.Trim() != "")
                            Address += CM_Address1 + "\n";

                        if (CM_Address2.Trim() != "")
                            Address += CM_Address2 + "\n";

                        if (CM_Address3.Trim() != "")
                            Address += CM_Address3 + "\n";

                        if (CM_Address4.Trim() != "")
                            Address += CM_Address4 + "\n";

                    }

                    title = new Paragraph(Address, Fn_Address);
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);


                    title = new Paragraph(Report_Name, titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 10;
                    document.Add(title);

                    string _txt = "User :" + User;
                    _txt += "\n";
                    text = new Paragraph("" + _txt, regularFont);
                    text.SpacingAfter = 10;


                    PdfPCell Leftcell;
                    PdfPCell Rightcell;


                    PdfPTable table2 = new PdfPTable(2);
                    table2.WidthPercentage = 100;
                    Leftcell = new PdfPCell(text);
                    Leftcell.Border = 0;
                    Leftcell.PaddingBottom = 8;
                    table2.AddCell(Leftcell);


                    _txt = "Date:" + GITAPI.dbFunctions.getdate();
                    _txt += "\n";
                    text = new Paragraph("" + _txt, regularFont);
                    text.SpacingAfter = 10;

                    Rightcell = new PdfPCell(text);
                    Rightcell.Border = 0;
                    Rightcell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    Rightcell.PaddingBottom = 8;
                    table2.AddCell(Rightcell);


                    //document.Add(table2);




                    PdfPTable table = new PdfPTable(th.Rows.Count + 1);
                    table.WidthPercentage = 100;
                    table.HeaderRows = 1;
                    string[] headers = new string[th.Rows.Count];

                    for (int k = 0; k < th.Rows.Count; k++)
                    {

                        if (k == 0)
                        {
                            PdfPCell cel = new PdfPCell(new Phrase("#", regularFontBold));
                            cel.BackgroundColor = new iTextSharp.text.BaseColor(223, 227, 232);


                            cel.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);

                            cel.PaddingLeft = 4;
                            cel.PaddingRight = 4;
                            cel.PaddingTop = 8;
                            cel.PaddingBottom = 8;


                            table.AddCell(cel);

                        }
                        headers[k] = th.Rows[k]["Name"].ToString();

                        PdfPCell cell = new PdfPCell(new Phrase(th.Rows[k]["Name"].ToString(), regularFontBold));

                        // cell.Width = this.Columns[k].Width;
                        // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(223, 227, 232);


                        cell.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);

                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;
                        cell.PaddingTop = 8;
                        cell.PaddingBottom = 8;


                        try
                        {
                            string Align = th.Rows[k]["Align"].ToString();

                            if (Align.ToLower().Equals("right"))
                            {
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                            }
                            else if (Align.ToLower().Equals("center"))
                            {

                                cell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                            }

                        }
                        catch { }




                        table.AddCell(cell);

                    }

                    if (td.Rows.Count <= 9)
                    {
                        GITAPI.dbFunctions.S_No = "#";
                    }
                    else if (td.Rows.Count <= 99)
                    {
                        GITAPI.dbFunctions.S_No = "# ";
                    }
                    else if (td.Rows.Count <= 300)
                    {
                        GITAPI.dbFunctions.S_No = "#   ";
                    }
                    else if (td.Rows.Count <= 999)
                    {
                        GITAPI.dbFunctions.S_No = "#   ";
                    }

                    table.SetWidths(GetHeaderWidths(font, headers));




                    //Add values of DataTable in pdf file
                    for (int i = 0; i < td.Rows.Count; i++)
                    {

                        for (int j = 0; j < th.Rows.Count; j++)
                        {


                            if (j == 0)
                            {

                                PdfPCell ce = new PdfPCell(new Phrase((i + 1).ToString(), regularFont));

                                ce.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                                ce.PaddingLeft = 4;
                                ce.PaddingRight = 4;
                                ce.PaddingTop = 8;
                                ce.PaddingBottom = 8;
                                ce.BorderWidth = 1;
                                table.AddCell(ce);

                            }

                            string colum = th.Rows[j]["Field"].ToString();
                            PdfPCell cell = new PdfPCell(new Phrase(td.Rows[i][colum].ToString(), regularFont));

                            cell.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                            cell.PaddingLeft = 4;
                            cell.PaddingRight = 4;
                            cell.PaddingTop = 8;
                            cell.PaddingBottom = 8;
                            cell.BorderWidth = 1;

                            try
                            {
                                string Align = th.Rows[j]["Align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    cell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                                }

                            }
                            catch { }

                            table.AddCell(cell);
                        }
                    }


                    document.Add(table);
                    writer.PageEvent = new PDF_Report_Footer();



                    document.Close();
                }
                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");
            }
        }





        public FileResult Export_Pdfx(string User, string Company, string File_Name, string File_Type)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;
                DataTable dd = GITAPI.dbFunctions.getTable("select * from Excel_Data" + Company + " where user_ID='" + User + "'");

                if (dd.Rows.Count > 0)
                {
                    jsonData = JObject.Parse(dd.Rows[0]["Data"].ToString());

                    dynamic json = jsonData;
                    string ID = GITAPI.dbFunctions.isnull(json.ID, "");
                    string FileName = GITAPI.dbFunctions.isnull(json.FileName, "Data");
                    File_Name = FileName;
                    string Report_Name = GITAPI.dbFunctions.isnull(json.Report_Name, "");
                    string Company_Name = GITAPI.dbFunctions.isnull(json.Company_Name, "");
                    string Ledger_ID = GITAPI.dbFunctions.isnull(json.Ledger_ID, "0");
                    string From = GITAPI.dbFunctions.isnull(json.From, "");
                    string To = GITAPI.dbFunctions.isnull(json.To, "");
                    string cr = GITAPI.dbFunctions.isnull(json.cr, "");
                    string db = GITAPI.dbFunctions.isnull(json.db, "");
                    string Total = GITAPI.dbFunctions.isnull(json.Total, "");

                    GITAPI.dbFunctions.Total_Row = GITAPI.dbFunctions.isnull(json.Total_Row, "");

                    GITAPI.dbFunctions.Total_Amount = GITAPI.dbFunctions.isnull(json.Total_Amount, "");
                    string Head_Type = GITAPI.dbFunctions.isnull(json.Head_Type, "name_only");

                    Newtonsoft.Json.Linq.JArray items = json.items;
                    Newtonsoft.Json.Linq.JArray Header1 = json.Header;

                    //string Company_Data = GITAPI.dbFunctions.isnull(json.Company_Data, "");
                    dynamic CN_json = json.Company_Data;

                    string CM_Code = GITAPI.dbFunctions.isnull(CN_json.CM_Code, "");
                    string CM_Name = GITAPI.dbFunctions.isnull(CN_json.CM_Name, "");
                    string CM_Address1 = GITAPI.dbFunctions.isnull(CN_json.CM_Address1, "");
                    string CM_Address2 = GITAPI.dbFunctions.isnull(CN_json.CM_Address2, "");
                    string CM_Address3 = GITAPI.dbFunctions.isnull(CN_json.CM_Address3, "");
                    string CM_Address4 = GITAPI.dbFunctions.isnull(CN_json.CM_Address4, "");
                    string CM_Address5 = GITAPI.dbFunctions.isnull(CN_json.CM_Address5, "");
                    string CM_Email_ID = GITAPI.dbFunctions.isnull(CN_json.CM_Email_ID, "");
                    string CM_GST_No = GITAPI.dbFunctions.isnull(CN_json.CM_GST_No, "");
                    string CM_State_Code = GITAPI.dbFunctions.isnull(CN_json.CM_State_Code, "");
                    string CM_State = GITAPI.dbFunctions.isnull(CN_json.CM_State, "");





                    DataTable td = toDataTable(items);
                    DataTable th = toDataTable(Header1);
                    load_Page_Setting(Company);

                    Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);
                    if (GITAPI.dbFunctions.PDF_Page_Size == "a4")
                    {
                        document = new Document(PageSize.A4, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                        if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                        {

                            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        }

                    }
                    else if (GITAPI.dbFunctions.PDF_Page_Size == "a3")
                    {
                        document = new Document(PageSize.A3, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                        if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                        {

                            document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
                        }
                    }
                    else if (GITAPI.dbFunctions.PDF_Page_Size == "a5")
                    {
                        document = new Document(PageSize.A5, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                        if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                        {

                            document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                        }
                    }



                    PdfWriter writer = PdfWriter.GetInstance(document, stream);

                    writer.PageEvent = new Border1();

                    PageEventHelper pageEventHelper = new PageEventHelper();
                    writer.PageEvent = pageEventHelper;
                    document.Open();
                    GITAPI.dbFunctions.PDF_Bottom = 10;



                    FontFactory.RegisterDirectories();

                    iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
                    iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);
                    iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                    iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                    iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
                    iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                    iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                    iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
                    iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
                    iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                    iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);



                    iTextSharp.text.Font subject = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                    subject.SetColor(105, 105, 105);
                    small_Font.SetColor(105, 105, 105);



                    document.Open();


                    Paragraph title;
                    Paragraph text;




                    Paragraph Header;

                    Header = new Paragraph(CM_Name, fn_Header);
                    Header.Alignment = Element.ALIGN_CENTER;
                    document.Add(Header);


                    string Address = "";
                    if (Head_Type.ToLower().Equals("name_only"))
                    {
                        if (CM_Address1.Trim() != "")
                            Address += CM_Address1 + "\n";

                        if (CM_Address2.Trim() != "")
                            Address += CM_Address2 + "\n";

                        if (CM_Address3.Trim() != "")
                            Address += CM_Address3 + "\n";

                        if (CM_Address4.Trim() != "")
                            Address += CM_Address4 + "\n";

                    }

                    title = new Paragraph(Address, Fn_Address);
                    title.Alignment = Element.ALIGN_CENTER;
                    document.Add(title);


                    title = new Paragraph(Report_Name, titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 10;
                    document.Add(title);

                    string _txt = "Ledger  :" + Ledger_ID;
                    _txt += "\n";

                    DataTable dd1 = GITAPI.dbFunctions.getTable("select * from Ledger_Master" + Company + " where id='" + Ledger_ID + "'");
                    if (dd1.Rows.Count > 0)
                    {
                        _txt = "Ledger Name :" + dd1.Rows[0]["Ledger_Name"];
                        _txt += "\n";

                    }


                    text = new Paragraph("" + _txt, regularFont);
                    text.SpacingAfter = 10;


                    PdfPCell Leftcell;
                    PdfPCell Rightcell;


                    PdfPTable table2 = new PdfPTable(2);
                    table2.WidthPercentage = 100;
                    Leftcell = new PdfPCell(text);
                    Leftcell.Border = 0;
                    Leftcell.PaddingBottom = 8;
                    table2.AddCell(Leftcell);





                    text = new Paragraph("From  :" + From + " To  :" + To, regularFont);

                    text.SpacingAfter = 10;
                    Rightcell = new PdfPCell(text);
                    Rightcell = new PdfPCell(text);
                    Rightcell.Border = 0;
                    Rightcell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    Rightcell.PaddingBottom = 8;
                    table2.AddCell(Rightcell);

                    PdfPTable table3 = new PdfPTable(2);
                    table3.WidthPercentage = 100;
                    text = new Paragraph("From Date:" + From, regularFont);

                    Rightcell = new PdfPCell(text);
                    table3.AddCell(Rightcell);


                    document.Add(table2);

                    document.Add(table3);



                    PdfPTable table = new PdfPTable(th.Rows.Count + 1);
                    table.WidthPercentage = 100;
                    table.HeaderRows = 1;
                    string[] headers = new string[th.Rows.Count];

                    for (int k = 0; k < th.Rows.Count; k++)
                    {

                        if (k == 0)
                        {
                            PdfPCell cel = new PdfPCell(new Phrase("#", regularFontBold));
                            cel.BackgroundColor = new iTextSharp.text.BaseColor(223, 227, 232);


                            cel.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);

                            cel.PaddingLeft = 4;
                            cel.PaddingRight = 4;
                            cel.PaddingTop = 8;
                            cel.PaddingBottom = 8;


                            table.AddCell(cel);

                        }
                        headers[k] = th.Rows[k]["Name"].ToString();

                        PdfPCell cell = new PdfPCell(new Phrase(th.Rows[k]["Name"].ToString(), regularFontBold));

                        // cell.Width = this.Columns[k].Width;
                        // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(223, 227, 232);


                        cell.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);

                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;
                        cell.PaddingTop = 8;
                        cell.PaddingBottom = 8;


                        try
                        {
                            string Align = th.Rows[k]["Align"].ToString();

                            if (Align.ToLower().Equals("right"))
                            {
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                            }
                            else if (Align.ToLower().Equals("center"))
                            {

                                cell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                            }

                        }
                        catch { }




                        table.AddCell(cell);

                    }

                    if (td.Rows.Count <= 9)
                    {
                        GITAPI.dbFunctions.S_No = "#";
                    }
                    else if (td.Rows.Count <= 99)
                    {
                        GITAPI.dbFunctions.S_No = "# ";
                    }
                    else if (td.Rows.Count <= 300)
                    {
                        GITAPI.dbFunctions.S_No = "#   ";
                    }
                    else if (td.Rows.Count <= 999)
                    {
                        GITAPI.dbFunctions.S_No = "#   ";
                    }

                    table.SetWidths(GetHeaderWidths(font, headers));




                    //Add values of DataTable in pdf file
                    for (int i = 0; i < td.Rows.Count; i++)
                    {

                        for (int j = 0; j < th.Rows.Count; j++)
                        {


                            if (j == 0)
                            {

                                PdfPCell ce = new PdfPCell(new Phrase((i + 1).ToString(), regularFont));

                                ce.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                                ce.PaddingLeft = 4;
                                ce.PaddingRight = 4;
                                ce.PaddingTop = 8;
                                ce.PaddingBottom = 8;
                                ce.BorderWidth = 1;
                                table.AddCell(ce);

                            }

                            string colum = th.Rows[j]["Field"].ToString();
                            PdfPCell cell = new PdfPCell(new Phrase(td.Rows[i][colum].ToString(), regularFont));

                            cell.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                            cell.PaddingLeft = 4;
                            cell.PaddingRight = 4;
                            cell.PaddingTop = 8;
                            cell.PaddingBottom = 8;
                            cell.BorderWidth = 1;

                            try
                            {
                                string Align = th.Rows[j]["Align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    cell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                                }

                            }
                            catch { }


                            table.AddCell(cell);
                        }
                    }

                    {

                        PdfPCell S1 = new PdfPCell(new Phrase("Total", regularFont));
                        S1.Colspan = 3;
                        S1.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                        S1.PaddingLeft = 4;
                        S1.PaddingRight = 4;
                        S1.PaddingTop = 8;
                        S1.PaddingBottom = 8;
                        S1.BorderWidth = 1;

                        text = new Paragraph("" + cr, regularFontBold_Black);

                        PdfPCell S2 = new PdfPCell(text);

                        S2.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                        S2.HorizontalAlignment = Element.ALIGN_RIGHT;

                        S2.PaddingLeft = 4;
                        S2.PaddingRight = 4;
                        S2.PaddingTop = 8;
                        S2.PaddingBottom = 8;
                        S2.BorderWidth = 1;
                        text = new Paragraph("" + db, regularFontBold_Black);

                        PdfPCell S3 = new PdfPCell(text);
                        S3.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                        S3.HorizontalAlignment = Element.ALIGN_RIGHT;

                        S3.PaddingLeft = 4;
                        S3.PaddingRight = 4;
                        S3.PaddingTop = 8;
                        S3.PaddingBottom = 8;
                        S3.BorderWidth = 1;
                        table.AddCell(S1);
                        table.AddCell(S2);
                        table.AddCell(S3);
                    }


                    {
                        PdfPCell S1 = new PdfPCell(new Phrase("Balance", regularFont));
                        S1.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);

                        S1.Colspan = 3;
                        S1.PaddingLeft = 4;
                        S1.PaddingRight = 4;
                        S1.PaddingTop = 8;
                        S1.PaddingBottom = 8;
                        S1.BorderWidth = 1;

                        text = new Paragraph("" + Total, regularFontBold_Black);

                        PdfPCell S2 = new PdfPCell(text);
                        S2.Colspan = 2;
                        S2.HorizontalAlignment = Element.ALIGN_RIGHT;
                        S2.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);

                        S2.PaddingLeft = 4;
                        S2.PaddingRight = 4;
                        S2.PaddingTop = 8;
                        S2.PaddingBottom = 8;
                        S2.BorderWidth = 1;

                        table.AddCell(S1);
                        table.AddCell(S2);

                    }

                    document.Add(table);
                    //writer.PageEvent = new PDF_Report_Footer();



                    document.Close();
                }
                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");
            }
        }



        public FileResult Export_Invoice(string User, string Company, string File_Name, string File_Type, string Bill_No)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;


                Load_Comapny(Company);
                Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);

                document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                writer.PageEvent = new Border();
                //writer.PageEvent = new PDFFooter();
                PageEventHelper pageEventHelper = new PageEventHelper();
                writer.PageEvent = pageEventHelper;
                document.Open();


                iTextSharp.text.Font fn_Header = FontFactory.GetFont("verdana", 12, Font.BOLD);
                iTextSharp.text.Font Fn_Address = FontFactory.GetFont("verdana", 8);
                iTextSharp.text.Font font = FontFactory.GetFont("verdana", 9);
                iTextSharp.text.Font titleFont = FontFactory.GetFont("verdana", 10, Font.BOLD);
                iTextSharp.text.Font regularFont = FontFactory.GetFont("verdana", 9);
                iTextSharp.text.Font regularFontBold = FontFactory.GetFont("verdana", 9, Font.BOLD);
                iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont("verdana", 9, Font.BOLD, BaseColor.WHITE);
                iTextSharp.text.Font small_Font = FontFactory.GetFont("verdana", 7, Font.BOLD, BaseColor.LIGHT_GRAY);
                iTextSharp.text.Font small_Font1 = FontFactory.GetFont("verdana", 7, Font.BOLD, BaseColor.LIGHT_GRAY);



                string Address = "";

                if (GITAPI.dbFunctions.CM_Address1.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address1 + "\n";

                if (GITAPI.dbFunctions.CM_Address2.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address2 + "\n";

                if (GITAPI.dbFunctions.CM_Address3.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address3 + "\n";

                if (GITAPI.dbFunctions.CM_Address4.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address4 + "\n";




                MultiColumnText headercolumns = new MultiColumnText();
                headercolumns.AddRegularColumns(10f, document.PageSize.Width - 10f, 100f, 3);
                Paragraph para = new Paragraph(Address, Fn_Address);

                para.SpacingAfter = 9f;

                para.Alignment = Element.ALIGN_JUSTIFIED;

                headercolumns.AddElement(para);


                Paragraph Fhead = new Paragraph(GITAPI.dbFunctions.CM_Name, Fn_Address);

                Fhead.SpacingAfter = 9f;



                headercolumns.AddElement(Fhead);



                MultiColumnText columns = new MultiColumnText();

                //float left, float right, float gutterwidth, int numcolumns

                columns.AddRegularColumns(10f, document.PageSize.Width - 10f, 100f, 1);









                string a = "[{ \"header\":\"#\" ,\"field\":\"ID\",\"align\":\"lef\",\"width\":\"\"}," +
                               " { \"header\":\"Item Name\" ,\"field\":\"Item_Name\",\"align\":\"left\",\"width\":\"\"}," +
                              " { \"header\":\"HSN\" ,\"field\":\"HSN_Code\",\"align\":\"left\",\"width\":\"\"}," +
                              " { \"header\":\"Qty\" ,\"field\":\"Qty\",\"align\":\"right\",\"width\":\"\"}," +
                              " { \"header\":\"Price\" ,\"field\":\"Unit_Price\",\"align\":\"right\",\"width\":\"\"}," +
                              " { \"header\":\"Taxable\" ,\"field\":\"Final_Price\",\"align\":\"right\",\"width\":\"\"}," +
                              " { \"header\":\"CGST\" ,\"field\":\"SGST_Amt\",\"align\":\"right\",\"width\":\"\"}," +
                              " { \"header\":\"SGST\" ,\"field\":\"CGST_Amt\",\"align\":\"right\",\"width\":\"\"}," +
                              " { \"header\":\"Amount\" ,\"field\":\"Net_Amt\",\"align\":\"right\",\"width\":\"\"}]";


                Newtonsoft.Json.Linq.JArray Headers = Newtonsoft.Json.Linq.JArray.Parse(a);
                DataTable th = toDataTable(Headers);
                PdfPTable table = new PdfPTable(th.Rows.Count);
                table.WidthPercentage = 100;
                table.HeaderRows = 1;
                string[] headers = new string[th.Rows.Count];

                for (int k = 0; k < th.Rows.Count; k++)
                {
                    headers[k] = th.Rows[k]["header"].ToString();

                    Paragraph pk = new Paragraph(th.Rows[k]["header"].ToString(), regularFontBold_white);
                    PdfPCell cell = new PdfPCell();

                    // cell.Width = this.Columns[k].Width;
                    // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(0, 102, 0);

                    cell.PaddingTop = 0;

                    try
                    {
                        string Align = th.Rows[k]["align"].ToString();

                        if (Align.ToLower().Equals("right"))
                        {
                            pk.Alignment = Element.ALIGN_RIGHT;

                        }
                        else if (Align.ToLower().Equals("center"))
                        {

                            pk.Alignment = Element.ALIGN_MIDDLE;
                        }

                    }
                    catch { }

                    pk.SpacingAfter = 0;
                    pk.SpacingBefore = 0;


                    cell.AddElement(pk);
                    cell.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;
                    cell.PaddingTop = 8;
                    cell.PaddingBottom = 8;

                    cell.BorderWidthTop = 1;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1;

                    table.AddCell(cell);

                }



                float[] anchoDeColumnas = new float[] { 3f, 25f, 10f, 7f, 10f, 10f, 10f, 10f, 10f };
                table.SetWidths(anchoDeColumnas);


                DataTable td = GITAPI.dbFunctions.getTable("select *,UOM as UOM_ from   sales_details" + Company + " where Bill_No='" + Bill_No + "'");




                //Add values of DataTable in pdf file
                for (int i = 0; i < td.Rows.Count; i++)
                {

                    for (int j = 0; j < th.Rows.Count; j++)
                    {

                        string colum = th.Rows[j]["field"].ToString();

                        string data = td.Rows[i][colum].ToString();



                        PdfPCell cell = new PdfPCell();

                        if (colum == "ID")
                        {
                            Paragraph Item_Name = new Paragraph((i + 1).ToString(), regularFont);

                            cell.AddElement(Item_Name);


                        }
                        else if (colum == "Item_Name")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Item_Name"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["Description"].ToString(), small_Font);

                            Description.Alignment = Element.ALIGN_LEFT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }
                        else if (colum == "Qty")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Qty"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["UOM_"].ToString(), small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }

                        else if (colum == "SGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["SGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["SGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;

                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }

                        else if (colum == "CGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["CGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["CGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }
                        else
                        {
                            Paragraph P = new Paragraph(data, regularFont);




                            try
                            {
                                string Align = th.Rows[j]["align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    P.Alignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    P.Alignment = Element.ALIGN_MIDDLE;
                                }

                            }
                            catch { }

                            cell.AddElement(P);
                        }


                        // cell.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;

                        cell.BorderWidthTop = 0;
                        cell.BorderWidthBottom = 0;
                        cell.BorderWidthLeft = 0;
                        cell.BorderWidthRight = 1;





                        table.AddCell(cell);



                    }
                }


                DataTable ds = GITAPI.dbFunctions.getTable("select * from sales" + Company + " where Bill_No='" + Bill_No + "'");


                PdfPCell TotalCell = new PdfPCell(new Phrase("Total", regularFontBold));
                //TotalCell.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                TotalCell.HorizontalAlignment = Element.ALIGN_LEFT;

                TotalCell.PaddingLeft = 4;
                TotalCell.PaddingRight = 4;
                TotalCell.PaddingTop = 8;
                TotalCell.PaddingBottom = 8;
                TotalCell.Colspan = 5;

                TotalCell.BorderWidthTop = 1;
                TotalCell.BorderWidthBottom = 1;
                TotalCell.BorderWidthLeft = 0;
                TotalCell.BorderWidthRight = 1;

                table.AddCell(TotalCell);


                PdfPCell Taxable = new PdfPCell(new Phrase("" + ds.Rows[0]["Taxable_Amount"].ToString(), regularFontBold));

                //Taxable.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                Taxable.PaddingLeft = 4;
                Taxable.PaddingRight = 4;
                Taxable.PaddingTop = 8;
                Taxable.PaddingBottom = 8;
                Taxable.BorderWidthTop = 1;
                Taxable.BorderWidthBottom = 1;
                Taxable.BorderWidthLeft = 0;
                Taxable.BorderWidthRight = 1;

                Taxable.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(Taxable);


                PdfPCell SGST = new PdfPCell(new Phrase("" + ds.Rows[0]["CGST_Amt"].ToString(), regularFontBold));
                //SGST.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                SGST.PaddingLeft = 4;
                SGST.PaddingRight = 4;
                SGST.PaddingTop = 8;
                SGST.PaddingBottom = 8;

                SGST.BorderWidthTop = 1;
                SGST.BorderWidthBottom = 1;
                SGST.BorderWidthLeft = 0;
                SGST.BorderWidthRight = 1;


                SGST.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(SGST);

                PdfPCell CGST = new PdfPCell(new Phrase("" + ds.Rows[0]["SGST_Amt"].ToString(), regularFontBold));
                //CGST.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                CGST.PaddingLeft = 4;
                CGST.PaddingRight = 4;
                CGST.PaddingTop = 8;
                CGST.PaddingBottom = 8;

                CGST.BorderWidthTop = 1;
                CGST.BorderWidthBottom = 1;
                CGST.BorderWidthLeft = 0;
                CGST.BorderWidthRight = 1;

                CGST.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(CGST);


                PdfPCell Total_Amount = new PdfPCell(new Phrase("" + ds.Rows[0]["Net_Amt"].ToString(), regularFontBold));
                //Total_Amount.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                Total_Amount.PaddingLeft = 4;
                Total_Amount.PaddingRight = 4;
                Total_Amount.PaddingTop = 8;
                Total_Amount.PaddingBottom = 8;

                Total_Amount.BorderWidthTop = 1;
                Total_Amount.BorderWidthBottom = 1;
                Total_Amount.BorderWidthLeft = 0;
                Total_Amount.BorderWidthRight = 1;


                Total_Amount.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(Total_Amount);


                PdfPCell Total_Taxable = new PdfPCell(new Phrase("Total Taxable Value", regularFontBold));
                //Total_Taxable.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                Total_Taxable.HorizontalAlignment = Element.ALIGN_RIGHT;
                Total_Taxable.PaddingLeft = 4;
                Total_Taxable.PaddingRight = 4;
                Total_Taxable.Border = 0;
                Total_Taxable.Colspan = 5;
                table.AddCell(Total_Taxable);

                PdfPCell Total_Taxable_Amt = new PdfPCell(new Phrase("" + ds.Rows[0]["Taxable_Amount"].ToString(), regularFontBold));
                //Total_Taxable_Amt.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                Total_Taxable_Amt.HorizontalAlignment = Element.ALIGN_RIGHT;
                Total_Taxable_Amt.PaddingLeft = 4;
                Total_Taxable_Amt.PaddingRight = 4;
                Total_Taxable_Amt.Border = 0;
                Total_Taxable_Amt.Colspan = 4;
                table.AddCell(Total_Taxable_Amt);




                PdfPCell Round_Off_Label = new PdfPCell(new Phrase("Rounded Off ", regularFontBold));
                //Round_Off_Label.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                Round_Off_Label.HorizontalAlignment = Element.ALIGN_RIGHT;
                Round_Off_Label.Border = 0;
                Round_Off_Label.PaddingLeft = 4;
                Round_Off_Label.PaddingRight = 4;
                Round_Off_Label.Colspan = 5;
                table.AddCell(Round_Off_Label);


                PdfPCell Round_Off_Amt = new PdfPCell(new Phrase("0", regularFontBold));
                //Round_Off_Amt.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                Round_Off_Amt.HorizontalAlignment = Element.ALIGN_RIGHT;
                Round_Off_Amt.PaddingLeft = 4;
                Round_Off_Amt.PaddingRight = 4;
                Round_Off_Amt.Border = 0;
                Round_Off_Amt.Colspan = 4;
                table.AddCell(Round_Off_Amt);


                PdfPCell Nett_Amt_Label = new PdfPCell(new Phrase("Total Value (in figure) ", regularFontBold));
                //Nett_Amt_Label.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                Nett_Amt_Label.HorizontalAlignment = Element.ALIGN_RIGHT;
                Nett_Amt_Label.Border = 0;
                Nett_Amt_Label.PaddingLeft = 4;
                Nett_Amt_Label.PaddingRight = 4;
                Nett_Amt_Label.Colspan = 5;
                table.AddCell(Nett_Amt_Label);



                PdfPCell Nett_Amt = new PdfPCell(new Phrase("" + ds.Rows[0]["Net_Amt"].ToString(), regularFontBold));
                Nett_Amt.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                Nett_Amt.HorizontalAlignment = Element.ALIGN_RIGHT;
                Nett_Amt.Border = 0;
                Nett_Amt.PaddingLeft = 4;
                Nett_Amt.PaddingRight = 4;
                Nett_Amt.Colspan = 4;
                table.AddCell(Nett_Amt);

                long l = long.Parse(decimal.Parse(ds.Rows[0]["Net_Amt"].ToString()).ToString("0"));

                string word = GITAPI.dbFunctions.ConvertNumbertoWords(l);


                PdfPCell Net_word = new PdfPCell(new Phrase("" + word + " ONLY  ", regularFont));
                //Net_word.BorderColor = new iTextSharp.text.BaseColor(200, 200, 200);
                Net_word.HorizontalAlignment = Element.ALIGN_RIGHT;
                Net_word.Border = 0;
                Net_word.PaddingLeft = 4;
                Net_word.PaddingRight = 4;
                Net_word.Colspan = 9;
                table.AddCell(Net_word);


                Image jpg = Image.GetInstance(Server.MapPath("~/Image/Company/" + Company.Replace("_", "") + ".png"));
                jpg.ScaleToFit(50f, 30f);


                PdfPTable Btable = new PdfPTable(2);
                Btable.WidthPercentage = 100;


                PdfPCell b01 = new PdfPCell();
                b01.Border = 0;
                b01.AddElement(new Paragraph("INVOICE NO", fn_Header));
                b01.PaddingBottom = 5;


                PdfPCell b1 = new PdfPCell();
                b1.Border = 0;


                b1.PaddingTop = 5;
                b1.AddElement(new Paragraph("Issue Date", regularFont));
                b1.AddElement(new Paragraph("Due Date", regularFont));
                b1.AddElement(new Paragraph("Place of Supply", regularFont));


                PdfPCell b02 = new PdfPCell();
                b02.Border = 0;
                b02.AddElement(new Paragraph(": " + ds.Rows[0]["Bill_No"], fn_Header));
                b02.PaddingBottom = 5;


                PdfPCell b2 = new PdfPCell();
                b2.Border = 0;


                b2.PaddingTop = 5;
                b2.AddElement(new Paragraph(": " + DateTime.Parse(ds.Rows[0]["Bill_Date"].ToString()).ToString("dd-MM-yyyy"), regularFont));
                b2.AddElement(new Paragraph(": " + DateTime.Parse(ds.Rows[0]["Due_Date"].ToString()).ToString("dd-MM-yyyy"), regularFont));
                b2.AddElement(new Paragraph(": " + ds.Rows[0]["place_of_Supply"], regularFont));


                PdfPCell bb1 = new PdfPCell();
                bb1.Border = 0;
                bb1.AddElement(new Paragraph("Due Amount", regularFontBold_white));
                bb1.PaddingBottom = 10;
                bb1.VerticalAlignment = Element.ALIGN_MIDDLE;
                bb1.HorizontalAlignment = Element.ALIGN_MIDDLE;
                bb1.BackgroundColor = new iTextSharp.text.BaseColor(0, 102, 0);


                PdfPCell bb2 = new PdfPCell();
                bb2.Border = 0;
                bb2.PaddingBottom = 10;
                bb2.AddElement(new Paragraph(": 0", regularFontBold_white));
                bb2.VerticalAlignment = Element.ALIGN_MIDDLE;
                bb2.HorizontalAlignment = Element.ALIGN_MIDDLE;
                bb2.BackgroundColor = new iTextSharp.text.BaseColor(0, 102, 0);


                Btable.AddCell(b01);
                Btable.AddCell(b02);



                Btable.AddCell(bb1);
                Btable.AddCell(bb2);


                Btable.AddCell(b1);
                Btable.AddCell(b2);

                PdfPTable Htable = new PdfPTable(3);
                Htable.WidthPercentage = 100;
                float[] hs = new float[] { 35f, 35f, 35f };

                Htable.SetWidths(hs);
                PdfPCell c1 = new PdfPCell();
                c1.AddElement(jpg);
                c1.AddElement(new Paragraph(GITAPI.dbFunctions.CM_Name, regularFontBold));
                c1.AddElement(new Paragraph(Address, Fn_Address));
                c1.AddElement(new Paragraph(GITAPI.dbFunctions.CM_Email_ID, regularFontBold));
                c1.AddElement(new Paragraph(GITAPI.dbFunctions.CM_GST_No, regularFontBold));

                PdfPCell c2 = new PdfPCell();
                c2.AddElement(new Paragraph("Bill To", regularFontBold));
                c2.AddElement(new Paragraph(ds.Rows[0]["Customer_Name"].ToString(), regularFont));
                c2.AddElement(new Paragraph(ds.Rows[0]["Customer_Address1"].ToString(), regularFont));
                c2.AddElement(new Paragraph(ds.Rows[0]["Customer_Address2"].ToString(), regularFont));
                c2.AddElement(new Paragraph(ds.Rows[0]["Customer_Address3"].ToString(), regularFont));
                c2.AddElement(new Paragraph("GST : " + ds.Rows[0]["GST_No"].ToString(), regularFont));
                c2.AddElement(new Paragraph("Ph : " + ds.Rows[0]["Contact_No"].ToString(), regularFont));

                PdfPCell c3 = new PdfPCell();
                c3.AddElement(Btable);

                c1.Border = 0;
                c2.Border = 0;
                c3.Border = 0;
                c2.PaddingTop = 35;
                Htable.AddCell(c1);
                Htable.AddCell(c2);
                Htable.AddCell(c3);
                table.SpacingAfter = 20;
                columns.AddElement(Htable);
                columns.AddElement(table);


                document.Add(columns);

                document.Close();

                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");


            }
        }


        public FileResult Export_Quotation(string User, string Company, string File_Name, string File_Type, string Quote_No)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;

                Load_Comapny(Company);
                load_Page_Setting(Company);

                Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);
                if (GITAPI.dbFunctions.PDF_Page_Size == "a4")
                {
                    document = new Document(PageSize.A4, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    }

                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a3")
                {
                    document = new Document(PageSize.A3, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
                    }
                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a5")
                {
                    document = new Document(PageSize.A5, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                    }
                }



                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                writer.PageEvent = new Border();
                //writer.PageEvent = new PDFFooter();
                PageEventHelper pageEventHelper = new PageEventHelper();
                writer.PageEvent = pageEventHelper;
                document.Open();
                GITAPI.dbFunctions.PDF_Bottom = 10;

                iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
                iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);
                iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
                iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
                iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
                iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);



                iTextSharp.text.Font subject = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                subject.SetColor(105, 105, 105);
                small_Font.SetColor(105, 105, 105);
                int Color_R = 0;
                int Color_G = 100;
                int Color_B = 0;
                regularFontBold_white.SetColor(255, 255, 255);
                fn_Header.SetColor(Color_R, Color_G, Color_B);
                fn_Title.SetColor(Color_R, Color_G, Color_B);
                regularFontBold.SetColor(Color_R, Color_G, Color_B);


                float Rows = 0;
                float Rows_Lent = 35 + (10 - GITAPI.dbFunctions.PDF_Font_Size);

                string Address = "";

                if (GITAPI.dbFunctions.CM_Address1.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address1 + "\n";

                if (GITAPI.dbFunctions.CM_Address2.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address2 + "\n";

                if (GITAPI.dbFunctions.CM_Address3.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address3 + "\n";

                if (GITAPI.dbFunctions.CM_Address4.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address4 + "\n";





                DataTable ds = GITAPI.dbFunctions.getTable("select * from Quotation" + Company + " where Quote_No='" + Quote_No + "'");



                Image jpg = Image.GetInstance(Server.MapPath("~/Image/Company/" + Company.Replace("_", "") + ".png"));
                jpg.ScaleToFit(50f, 30f);


                PdfPTable Btable = new PdfPTable(1);
                Btable.WidthPercentage = 100;


                PdfPCell b01 = new PdfPCell();
                b01.Border = 0;
                Paragraph Pc = new Paragraph("QUOTATION                ", fn_Title);
                Pc.Alignment = Element.ALIGN_RIGHT;
                Pc.Font.SetColor(Color_R, Color_G, Color_B);


                Paragraph Pcc = new Paragraph("" + GITAPI.dbFunctions.CM_Name, fn_Header);
                Pcc.Alignment = Element.ALIGN_RIGHT;
                Pcc.Font.SetColor(Color_R, Color_G, Color_B);

                b01.PaddingRight = 20;
                b01.AddElement(Pc);
                b01.AddElement(Pcc);
                jpg.Alignment = Element.ALIGN_RIGHT;
                b01.AddElement(jpg);
                Btable.AddCell(b01);


                PdfPTable Line = new PdfPTable(1);
                Line.WidthPercentage = 100;

                PdfPCell l1 = new PdfPCell();
                l1.Border = 0;
                l1.BackgroundColor = new iTextSharp.text.BaseColor(Color_R, Color_G, Color_B);

                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.GREEN, Element.ALIGN_LEFT, 1)));
                Line.SpacingAfter = 5;
                Line.SpacingBefore = 5;
                l1.Padding = 0;
                Line.AddCell(l1);



                PdfPTable Htable = new PdfPTable(2);
                Htable.WidthPercentage = 100;
                float[] hs = new float[] { 50f, 50f };

                Htable.SetWidths(hs);
                PdfPCell c1 = new PdfPCell();

                // c1.AddElement(new Paragraph(CM_Name, regularFontBold));
                Paragraph Cp1 = new Paragraph("ADDRESS ", regularFontBold);
                //Cp1.Font.SetColor(247, 68, 69);
                c1.AddElement(Cp1);
                c1.AddElement(new Paragraph(Address, Fn_Address));
                c1.AddElement(new Paragraph(GITAPI.dbFunctions.CM_Email_ID, Fn_Address));
                c1.AddElement(new Paragraph("GSTIN : " + GITAPI.dbFunctions.CM_GST_No, Fn_Address));


                PdfPCell c3 = new PdfPCell();
                c3.AddElement(Btable);

                c1.Border = 0;
                c3.Border = 0;
                Htable.AddCell(c1);
                Htable.AddCell(c3);




                PdfPCell c2 = new PdfPCell();
                Paragraph pQ = new Paragraph("QUOTE TO :", regularFontBold);
                pQ.Font.SetColor(Color_R, Color_G, Color_B);
                c2.AddElement(pQ);
                c2.AddElement(new Paragraph(ds.Rows[0]["Customer_Name"].ToString(), regularFont));
                c2.AddElement(new Paragraph(ds.Rows[0]["Customer_Address1"].ToString(), regularFont));
                c2.AddElement(new Paragraph(ds.Rows[0]["Customer_Address2"].ToString(), regularFont));
                c2.AddElement(new Paragraph(ds.Rows[0]["Customer_Address3"].ToString(), regularFont));
                c2.AddElement(new Paragraph("GST : " + ds.Rows[0]["GST_No"].ToString(), regularFont));
                // c2.AddElement(new Paragraph("Ph : " + ds.Rows[0]["Contact_No"].ToString(), regularFont));
                c2.Border = 0;

                PdfPCell q1 = new PdfPCell();
                q1.AddElement(new Paragraph("Quote No", regularFont));
                q1.AddElement(new Paragraph("Date", regularFont));

                q1.AddElement(new Paragraph("Validity", regularFont));
                q1.AddElement(new Paragraph("Payment Terms", regularFont));
                q1.AddElement(new Paragraph("Shipping Method", regularFont));

                q1.Border = 0;

                PdfPCell q2 = new PdfPCell();
                q2.AddElement(new Paragraph(": " + ds.Rows[0]["Quote_No"].ToString(), regularFontBold_Black));
                q2.AddElement(new Paragraph(": " + DateTime.Parse(ds.Rows[0]["Quote_Date"].ToString()).ToString("dd-MMM-yyyy"), regularFont));

                q2.AddElement(new Paragraph(": " + ds.Rows[0]["Valid_For"].ToString(), regularFont));
                q2.AddElement(new Paragraph(": " + ds.Rows[0]["Payment_Terms"].ToString(), regularFont));
                q2.AddElement(new Paragraph(": " + ds.Rows[0]["Delivery_Mode"].ToString(), regularFont));
                q2.Border = 0;


                PdfPTable Customer_Table = new PdfPTable(3);
                Customer_Table.WidthPercentage = 100;
                float[] hs1 = new float[] { 80f, 35f, 35f };
                Customer_Table.SetWidths(hs1);
                Customer_Table.AddCell(c2);
                Customer_Table.AddCell(q1);
                Customer_Table.AddCell(q2);



                PdfPTable Sub_Table = new PdfPTable(1);
                Sub_Table.WidthPercentage = 100;
                //float[] hs1 = new float[] { 80f, 20f, 30f };
                //Customer_Table.SetWidths(hs1);
                PdfPCell qf1 = new PdfPCell();
                qf1.AddElement(new Paragraph("Dear,  " + ds.Rows[0]["Contact_Person"].ToString(), regularFontBold_Black));
                qf1.Border = 0;
                Sub_Table.AddCell(qf1);

                PdfPCell qf2 = new PdfPCell();
                qf2.AddElement(new Paragraph("Thank you for your intrest in our company and opportunity to quote.", subject));
                qf2.Border = 0;
                qf2.Padding = 0;
                Sub_Table.AddCell(qf2);

                PdfPCell qf3 = new PdfPCell();
                qf3.AddElement(new Paragraph("We are plased to quote as follows :", subject));
                qf3.Border = 0;
                Sub_Table.AddCell(qf3);



                Sub_Table.SpacingAfter = 10;




                PdfPTable Bottaom_Table = new PdfPTable(1);
                Bottaom_Table.WidthPercentage = 100;
                //float[] hs1 = new float[] { 80f, 20f, 30f };
                //Customer_Table.SetWidths(hs1);
                PdfPCell bf1 = new PdfPCell();
                bf1.AddElement(new Paragraph("Terms & Condition", titleFont));
                bf1.Border = 0;


                PdfPCell bf2 = new PdfPCell();

                string Terms = "" + ds.Rows[0]["Term"].ToString();

                Paragraph pTerms = new Paragraph(" " + Terms, regularFont);
                bf2.AddElement(pTerms);
                bf2.Border = 0;
                bf2.PaddingLeft = 20;
                Bottaom_Table.SpacingAfter = 30;

                int c = GITAPI.dbFunctions.Line_Count(Terms);

                Rows = Rows + (c * Rows_Lent);

                Bottaom_Table.AddCell(bf1);
                Bottaom_Table.AddCell(bf2);







                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(GITAPI.dbFunctions.PDF_left, document.PageSize.Width - GITAPI.dbFunctions.PDF_Right, 100f, 1);

                DataTable th = GITAPI.dbFunctions.getTable("select 'ID' as Field, '#' as Name, 'center' as Align, '5%' as Width ,0 as Order_No union all select Field,Name,Align,Width,order_No from Field_Setting" + Company + " where    Table_Name='Quotation_Details" + Company + "' order by order_No ");


                string W = "0";
                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Description"))
                    {
                        W = th.Rows[i]["Width"].ToString().Replace("%", "");
                        th.Rows[i].Delete();
                    }
                }

                th.AcceptChanges();

                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Item_Name"))
                    {
                        th.Rows[i]["Width"] = (decimal.Parse(th.Rows[i]["Width"].ToString().Replace("%", "")) + decimal.Parse(W)).ToString();

                    }
                }



                PdfPTable table = new PdfPTable(th.Rows.Count);
                table.WidthPercentage = 100;
                table.HeaderRows = 1;
                string[] headers = new string[th.Rows.Count];

                for (int k = 0; k < th.Rows.Count; k++)
                {
                    headers[k] = th.Rows[k]["Name"].ToString();



                    Paragraph tab_Header = new Paragraph(th.Rows[k]["Name"].ToString(), regularFontBold_white);

                    PdfPCell cell = new PdfPCell();



                    try
                    {
                        string Align = th.Rows[k]["Align"].ToString();

                        if (Align.ToLower().Equals("right"))
                        {
                            tab_Header.Alignment = Element.ALIGN_RIGHT;

                        }
                        else if (Align.ToLower().Equals("center"))
                        {
                            tab_Header.Alignment = Element.ALIGN_CENTER;
                        }

                    }
                    catch { }


                    cell.AddElement(tab_Header);

                    // cell.Width = this.Columns[k].Width;
                    // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(Color_R, Color_G + 30, Color_B);

                    cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    //cell.BorderColor = new iTextSharp.text.BaseColor(237, 67, 69);

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;
                    cell.PaddingTop = 0;
                    cell.PaddingBottom = 8;

                    cell.BorderWidthTop = 1;
                    cell.BorderWidthBottom = 1;
                    if (k == 0)
                    {
                        cell.BorderWidthLeft = 1;
                    }
                    else
                    {
                        cell.BorderWidthLeft = 0;
                    }
                    cell.BorderWidthRight = 1;
                    table.AddCell(cell);
                }


                float[] anchoDeColumnas = new float[th.Rows.Count];

                for (int i = 0; i < th.Rows.Count; i++)
                {
                    anchoDeColumnas[i] = float.Parse(th.Rows[i]["Width"].ToString().Replace("%", ""));

                }
                table.SetWidths(anchoDeColumnas);
                DataTable td = GITAPI.dbFunctions.getTable("select *,UOM as UOM_ from   Quotation_details" + Company + " where Quote_No='" + Quote_No + "'");


                //Add values of DataTable in pdf file
                for (int i = 0; i < td.Rows.Count; i++)
                {

                    for (int j = 0; j < th.Rows.Count; j++)
                    {

                        string colum = th.Rows[j]["Field"].ToString();
                        string data = td.Rows[i][colum].ToString();

                        PdfPCell cell = new PdfPCell();

                        if (colum == "ID")
                        {
                            Paragraph Item_Name = new Paragraph((i + 1).ToString(), regularFont);
                            cell.AddElement(Item_Name);

                        }
                        else if (colum == "Item_Name")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Item_Name"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["Description"].ToString(), small_Font);
                            Description.Alignment = Element.ALIGN_LEFT;

                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }
                        else if (colum == "Qty")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Qty"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["UOM"].ToString(), small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }

                        else if (colum == "SGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["SGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["SGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }

                        else if (colum == "CGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["CGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["CGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }
                        else
                        {
                            Paragraph P = new Paragraph(data, regularFont);
                            try
                            {
                                string Align = th.Rows[j]["Align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    P.Alignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    P.Alignment = Element.ALIGN_CENTER;
                                }

                            }
                            catch { }

                            cell.AddElement(P);
                        }


                        cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;

                        cell.BorderWidthTop = 0;
                        cell.BorderWidthBottom = 0;
                        if (j == 0)
                        {
                            cell.BorderWidthLeft = 1;
                        }
                        else
                        {
                            cell.BorderWidthLeft = 0;
                        }
                        cell.BorderWidthRight = 1;
                        table.AddCell(cell);



                    }
                }




                Rows = Rows + (td.Rows.Count * Rows_Lent);
                for (int j = 0; j < th.Rows.Count; j++)
                {
                    PdfPCell cell = new PdfPCell();

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;


                    cell.FixedHeight = 370 - Rows;
                    cell.BorderWidthTop = 0;
                    cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    cell.BorderWidthBottom = 1;
                    if (j == 0)
                    {
                        cell.BorderWidthLeft = 1;
                    }
                    else
                    {
                        cell.BorderWidthLeft = 0;
                    }
                    cell.BorderWidthRight = 1;
                    table.AddCell(cell);
                }

                table.SpacingAfter = 10;



                PdfPTable Footer_Table = new PdfPTable(1);
                Footer_Table.WidthPercentage = 100;
                //float[] hs1 = new float[] { 80f, 20f, 30f };
                //Customer_Table.SetWidths(hs1);
                PdfPCell ff1 = new PdfPCell();
                Bottaom_Table.CalculateHeights();
                float d = Bottaom_Table.GetRowHeight(0);
                Paragraph pff = new Paragraph("**WAITING FOR YOUR VALUALBLE PURCHASE ORDER**", Fn_Address);
                pff.Alignment = Element.ALIGN_CENTER;
                ff1.Border = 0;



                ff1.AddElement(pff);
                Footer_Table.AddCell(ff1);

                Footer_Table.SpacingAfter = 10;






                columns.AddElement(Htable);
                columns.AddElement(Line);
                columns.AddElement(Customer_Table);
                columns.AddElement(Line);
                columns.AddElement(Sub_Table);

                //Bottaom_Table.SetExtendLastRow(true,false);
                columns.AddElement(table);
                columns.AddElement(Bottaom_Table);
                columns.AddElement(Line);
                columns.AddElement(Footer_Table);



                document.Add(columns);






                document.Close();

                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");


            }
        }


        public FileResult Export_Invoice_1(string User, string Company, string File_Name, string File_Type, string Bill_No)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;
                Load_Comapny(Company);
                load_Page_Setting(Company);

                Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);
                if (GITAPI.dbFunctions.PDF_Page_Size == "a4")
                {
                    document = new Document(PageSize.A4, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    }

                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a3")
                {
                    document = new Document(PageSize.A3, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
                    }
                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a5")
                {
                    document = new Document(PageSize.A5, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                    }
                }



                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                writer.PageEvent = new Border();

                //writer.PageEvent = new PDFFooter();
                // PageEventHelper pageEventHelper = new PageEventHelper();
                //writer.PageEvent = pageEventHelper;
                document.Open();
                GITAPI.dbFunctions.PDF_Bottom = 10;



                FontFactory.RegisterDirectories();

                iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
                iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);
                iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
                iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
                iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
                iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);



                iTextSharp.text.Font subject = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                subject.SetColor(105, 105, 105);
                small_Font.SetColor(105, 105, 105);
                int Color_R = 0;
                int Color_G = 0;
                int Color_B = 0;
                regularFontBold_white.SetColor(0, 0, 0);
                fn_Header.SetColor(Color_R, Color_G, Color_B);
                fn_Title.SetColor(Color_R, Color_G, Color_B);
                regularFontBold.SetColor(Color_R, Color_G, Color_B);


                float Rows = 0;
                float Rows_Lent = 35 + (10 - GITAPI.dbFunctions.PDF_Font_Size);

                string Address = "";

                if (GITAPI.dbFunctions.CM_Address1.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address1 + "\n";

                if (GITAPI.dbFunctions.CM_Address2.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address2 + "\n";

                if (GITAPI.dbFunctions.CM_Address3.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address3 + "\n";

                if (GITAPI.dbFunctions.CM_Address4.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address4 + "\n";


                DataTable ds = GITAPI.dbFunctions.getTable("select * from Sales" + Company + " where Bill_No='" + Bill_No + "'");





                PdfPTable Btable = new PdfPTable(1);
                Btable.WidthPercentage = 100;


                PdfPCell b01 = new PdfPCell();
                b01.Border = 0;
                Paragraph Pc = new Paragraph("TAX INVOICE                ", fn_Title);
                Pc.Alignment = Element.ALIGN_RIGHT;
                Pc.Font.SetColor(Color_R, Color_G, Color_B);


                Paragraph Pcc = new Paragraph("" + GITAPI.dbFunctions.CM_Name, fn_Header);
                Pcc.Alignment = Element.ALIGN_RIGHT;
                Pcc.Font.SetColor(Color_R, Color_G, Color_B);

                b01.PaddingRight = 20;
                b01.AddElement(Pc);
                b01.AddElement(Pcc);

                Btable.AddCell(b01);


                PdfPTable Line = new PdfPTable(1);
                Line.WidthPercentage = 100;
                PdfPCell l1 = new PdfPCell();
                l1.Border = 0;
                Line.AddCell(l1);



                PdfPTable Htable = new PdfPTable(3);
                Htable.WidthPercentage = 100;

                float[] hs = new float[] { 40f, 60f, 40f };

                Htable.SetWidths(hs);
                PdfPCell c1 = new PdfPCell();

                Paragraph H1 = new Paragraph(GITAPI.dbFunctions.CM_Name, fn_Header);

                H1.Alignment = Element.ALIGN_CENTER;
                c1.AddElement(H1);


                Paragraph H2 = new Paragraph(Address, regularFont);

                H2.Alignment = Element.ALIGN_CENTER;

                H2.SpacingAfter = 10;
                c1.AddElement(H2);

                c1.Border = 1;
                PdfPCell i1 = new PdfPCell();
                Image jpg = Image.GetInstance(Server.MapPath("~/Image/Company/" + Company.Replace("_", "") + ".png"));
                jpg.ScaleToFit(150f, 100f);
                jpg.Alignment = Element.ALIGN_CENTER;
                PdfPCell i2 = new PdfPCell();
                i1.Padding = 20;
                i2.Border = 0;
                i1.AddElement(jpg);
                i1.Border = 0;
                i1.BorderWidthBottom = 1;
                i2.BorderWidthBottom = 1;
                c1.BorderWidthBottom = 1;
                //i1.BackgroundColor = BaseColor.GREEN;
                //c1.BackgroundColor = BaseColor.GREEN;
                //i2.BackgroundColor = BaseColor.GREEN;
                Htable.AddCell(i1);
                Htable.AddCell(c1);
                Htable.AddCell(i2);
                //Htable.AddCell(c3);

                PdfPCell i4 = new PdfPCell();
                i4.Colspan = 3;
                i4.PaddingTop = 0;

                i4.BorderWidthTop = 0;
                i4.BorderWidthLeft = 0;
                i4.BorderWidthRight = 0;
                Paragraph T1 = new Paragraph("TAX INVOICE", regularFontBold_Black);
                T1.Alignment = Element.ALIGN_CENTER;
                T1.SpacingBefore = 0;
                T1.SpacingAfter = 5;

                i4.AddElement(T1);


                Htable.AddCell(i4);



                PdfPCell c2 = new PdfPCell();
                Paragraph pQ = new Paragraph("BILLING TO :-", regularFontBold);
                pQ.Font.SetColor(Color_R, Color_G, Color_B);
                c2.AddElement(pQ);

                string Customer = "  " + ds.Rows[0]["Customer_Name"].ToString() + "\n";
                Customer += "  " + ds.Rows[0]["Customer_Address1"].ToString() + "\n";
                Customer += "  " + ds.Rows[0]["Customer_Address2"].ToString() + "\n";
                Customer += "  " + ds.Rows[0]["Customer_Address3"].ToString() + "\n";
                Customer += "  " + "GST : " + ds.Rows[0]["GST_No"].ToString();

                Paragraph bt1 = new Paragraph(Customer, regularFont);
                bt1.Font.SetColor(Color_R, Color_G, Color_B);
                bt1.SpacingAfter = 10;
                c2.PaddingLeft = 5;
                c2.AddElement(bt1);
                c2.Border = 0;




                PdfPCell D2 = new PdfPCell();
                Paragraph DP = new Paragraph("SHIPPING  TO :-", regularFontBold);
                DP.Font.SetColor(Color_R, Color_G, Color_B);
                D2.AddElement(DP);
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address1"].ToString(), regularFont));
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address2"].ToString(), regularFont));
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address3"].ToString(), regularFont));
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address4"].ToString(), regularFont));
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address5"].ToString(), regularFont));

                // c2.AddElement(new Paragraph("Ph : " + ds.Rows[0]["Contact_No"].ToString(), regularFont));
                D2.Border = 0;
                D2.BorderWidthLeft = 1;
                D2.BorderWidthRight = 1;
                D2.PaddingLeft = 5;
                PdfPCell q1 = new PdfPCell();
                q1.AddElement(new Paragraph("Bill No", regularFont));
                q1.AddElement(new Paragraph("Date", regularFont));


                q1.AddElement(new Paragraph("Payment Terms", regularFont));
                q1.AddElement(new Paragraph("Shipping Method", regularFont));

                q1.Border = 0;

                PdfPCell q2 = new PdfPCell();
                try
                {
                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["Bill_No"].ToString(), regularFontBold_Black));
                    q2.AddElement(new Paragraph(": " + DateTime.Parse(ds.Rows[0]["Bill_Date"].ToString()).ToString("dd-MMM-yyyy"), regularFont));

                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["Payment_Terms"].ToString(), regularFont));
                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["Delivery_Mode"].ToString(), regularFont));
                }
                catch { }
                q2.Border = 0;

                bool Ship_Adr = true;
                int splt = 4;
                if (ds.Rows[0]["Shiping_Address1"].ToString() == "")
                {
                    Ship_Adr = false;
                    splt = 3;
                }

                PdfPTable Customer_Table = new PdfPTable(splt);
                Customer_Table.WidthPercentage = 100;
                float[] hs1 = new float[] { 100f, 100f, 50f, 50f };
                float[] hsx = new float[] { 200f, 50f, 50f };

                if (Ship_Adr)
                {
                    Customer_Table.SetWidths(hs1);
                }
                else
                {
                    Customer_Table.SetWidths(hsx);
                }
                Customer_Table.AddCell(c2);

                if (Ship_Adr)
                {
                    Customer_Table.AddCell(D2);
                }
                Customer_Table.AddCell(q1);
                Customer_Table.AddCell(q2);







                PdfPTable Bottaom_Table = new PdfPTable(1);
                Bottaom_Table.WidthPercentage = 100;
                //float[] hs1 = new float[] { 80f, 20f, 30f };
                //Customer_Table.SetWidths(hs1);
                PdfPCell bf1 = new PdfPCell();
                bf1.AddElement(new Paragraph("Terms & Condition", titleFont));
                bf1.Border = 0;


                Bottaom_Table.AddCell(bf1);








                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(GITAPI.dbFunctions.PDF_left, document.PageSize.Width - GITAPI.dbFunctions.PDF_Right, 100f, 1);


                DataTable th = GITAPI.dbFunctions.getTable("select 'ID' as Field, '#' as Name, 'center' as Align, '5%' as Width ,0 as Order_No union all select Field,Name,Align,Width,order_No from Field_Setting" + Company + " where    Table_Name='Sales_Details" + Company + "' order by order_No ");


                string W = "0";
                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Description"))
                    {
                        W = th.Rows[i]["Width"].ToString().Replace("%", "");
                        th.Rows[i].Delete();
                    }
                }

                th.AcceptChanges();

                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Item_Name"))
                    {
                        th.Rows[i]["Width"] = (decimal.Parse(th.Rows[i]["Width"].ToString().Replace("%", "")) + decimal.Parse(W)).ToString();

                    }
                }



                PdfPTable table = new PdfPTable(th.Rows.Count);
                table.WidthPercentage = 100;
                table.HeaderRows = 1;


                string[] headers = new string[th.Rows.Count];

                for (int k = 0; k < th.Rows.Count; k++)
                {
                    headers[k] = th.Rows[k]["Name"].ToString();



                    Paragraph tab_Header = new Paragraph(th.Rows[k]["Name"].ToString(), regularFontBold_white);

                    PdfPCell cell = new PdfPCell();



                    try
                    {
                        string Align = th.Rows[k]["Align"].ToString();

                        if (Align.ToLower().Equals("right"))
                        {
                            tab_Header.Alignment = Element.ALIGN_RIGHT;

                        }
                        else if (Align.ToLower().Equals("center"))
                        {
                            tab_Header.Alignment = Element.ALIGN_CENTER;
                        }

                    }
                    catch { }


                    cell.AddElement(tab_Header);

                    // cell.Width = this.Columns[k].Width;
                    // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.BackgroundColor = new iTextSharp.text.BaseColor(Color_R, Color_G + 30, Color_B);

                    //cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    //cell.BorderColor = new iTextSharp.text.BaseColor(237, 67, 69);

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;
                    cell.PaddingTop = 0;
                    cell.PaddingBottom = 8;

                    cell.BorderWidthTop = 1;
                    cell.BorderWidthBottom = 1;
                    if (k == 0)
                    {
                        cell.BorderWidthLeft = 0;
                    }

                    table.AddCell(cell);
                }


                float[] anchoDeColumnas = new float[th.Rows.Count];

                for (int i = 0; i < th.Rows.Count; i++)
                {
                    anchoDeColumnas[i] = float.Parse(th.Rows[i]["Width"].ToString().Replace("%", ""));

                }
                table.SetWidths(anchoDeColumnas);
                DataTable td = GITAPI.dbFunctions.getTable("select * from   Sales_details" + Company + " where Bill_No='" + Bill_No + "'");


                //Add values of DataTable in pdf file
                for (int i = 0; i < td.Rows.Count; i++)
                {

                    for (int j = 0; j < th.Rows.Count; j++)
                    {

                        string colum = th.Rows[j]["Field"].ToString();
                        string data = td.Rows[i][colum].ToString();

                        PdfPCell cell = new PdfPCell();

                        if (colum == "ID")
                        {
                            Paragraph Item_Name = new Paragraph((i + 1).ToString(), regularFont);
                            cell.AddElement(Item_Name);

                        }
                        else if (colum == "Item_Name")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Item_Name"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["Description"].ToString(), small_Font);
                            Description.Alignment = Element.ALIGN_LEFT;

                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }
                        else if (colum == "Qty")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Qty"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["UOM"].ToString(), small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }

                        else if (colum == "SGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["SGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["SGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }

                        else if (colum == "CGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["CGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["CGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }
                        else
                        {
                            Paragraph P = new Paragraph(data, regularFont);
                            try
                            {
                                string Align = th.Rows[j]["Align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    P.Alignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    P.Alignment = Element.ALIGN_CENTER;
                                }

                            }
                            catch { }

                            cell.AddElement(P);
                        }


                        //  cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;
                        cell.FixedHeight = Rows_Lent;
                        cell.BorderWidthTop = 0;
                        cell.BorderWidthBottom = 0;
                        if (j == 0)
                        {
                            cell.BorderWidthLeft = 0;
                        }

                        //cell.BorderWidthRight = 1;
                        table.AddCell(cell);



                    }
                }




                Rows = Rows + (td.Rows.Count * Rows_Lent);
                for (int j = 0; j < th.Rows.Count; j++)
                {
                    PdfPCell cell = new PdfPCell();

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;


                    cell.FixedHeight = 350 - Rows;
                    cell.BorderWidthTop = 0;
                    //cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    cell.BorderWidthBottom = 1;
                    if (j == 0)
                    {
                        cell.BorderWidthLeft = 0;
                    }

                    //cell.BorderWidthRight = 1;
                    table.AddCell(cell);
                }

                table.SpacingAfter = 10;





                columns.AddElement(Htable);

                columns.AddElement(Customer_Table);

                table.SpacingAfter = 150;
                //table.SetExtendLastRow(true, true);
                table.IsExtendLastRow(true);

                float x = table.TotalHeight;



                columns.AddElement(table);
                columns.AddElement(Line);

                document.Add(columns);
                GITAPI.dbFunctions.Company = Company;
                GITAPI.dbFunctions.Bill_No = Bill_No;
                writer.PageEvent = new Report_Footer();


                document.Close();

                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");

            }
        }



        public FileResult Export_P_Invoice_1(string User, string Company, string File_Name, string File_Type, string Bill_No)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;
                Load_Comapny(Company);
                load_Page_Setting(Company);

                Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);
                if (GITAPI.dbFunctions.PDF_Page_Size == "a4")
                {
                    document = new Document(PageSize.A4, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    }

                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a3")
                {
                    document = new Document(PageSize.A3, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
                    }
                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a5")
                {
                    document = new Document(PageSize.A5, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                    }
                }



                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                writer.PageEvent = new Border();

                //writer.PageEvent = new PDFFooter();
                //PageEventHelper pageEventHelper = new PageEventHelper();
                //                writer.PageEvent = pageEventHelper;
                document.Open();
                GITAPI.dbFunctions.PDF_Bottom = 10;



                FontFactory.RegisterDirectories();

                iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
                iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);
                iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
                iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
                iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
                iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);



                iTextSharp.text.Font subject = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                subject.SetColor(105, 105, 105);
                small_Font.SetColor(105, 105, 105);
                int Color_R = 0;
                int Color_G = 0;
                int Color_B = 0;
                regularFontBold_white.SetColor(0, 0, 0);
                fn_Header.SetColor(Color_R, Color_G, Color_B);
                fn_Title.SetColor(Color_R, Color_G, Color_B);
                regularFontBold.SetColor(Color_R, Color_G, Color_B);


                float Rows = 0;
                float Rows_Lent = 35 + (10 - GITAPI.dbFunctions.PDF_Font_Size);

                string Address = "";

                if (GITAPI.dbFunctions.CM_Address1.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address1 + "\n";

                if (GITAPI.dbFunctions.CM_Address2.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address2 + "\n";

                if (GITAPI.dbFunctions.CM_Address3.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address3 + "\n";

                if (GITAPI.dbFunctions.CM_Address4.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address4 + "\n";


                DataTable ds = GITAPI.dbFunctions.getTable("select * from P_Invoice" + Company + " where Bill_No='" + Bill_No + "'");





                PdfPTable Btable = new PdfPTable(1);
                Btable.WidthPercentage = 100;


                PdfPCell b01 = new PdfPCell();
                b01.Border = 0;
                Paragraph Pc = new Paragraph("PROFORMA INVOICE                ", fn_Title);
                Pc.Alignment = Element.ALIGN_RIGHT;
                Pc.Font.SetColor(Color_R, Color_G, Color_B);


                Paragraph Pcc = new Paragraph("" + GITAPI.dbFunctions.CM_Name, fn_Header);
                Pcc.Alignment = Element.ALIGN_RIGHT;
                Pcc.Font.SetColor(Color_R, Color_G, Color_B);

                b01.PaddingRight = 20;
                b01.AddElement(Pc);
                b01.AddElement(Pcc);

                Btable.AddCell(b01);


                PdfPTable Line = new PdfPTable(1);
                Line.WidthPercentage = 100;
                PdfPCell l1 = new PdfPCell();
                l1.Border = 0;
                Line.AddCell(l1);



                PdfPTable Htable = new PdfPTable(3);
                Htable.WidthPercentage = 100;

                float[] hs = new float[] { 20f, 80f, 20f };

                Htable.SetWidths(hs);
                PdfPCell c1 = new PdfPCell();

                Paragraph H1 = new Paragraph(GITAPI.dbFunctions.CM_Name, fn_Header);

                H1.Alignment = Element.ALIGN_CENTER;
                c1.AddElement(H1);


                Paragraph H2 = new Paragraph(Address, regularFont);

                H2.Alignment = Element.ALIGN_CENTER;

                H2.SpacingAfter = 10;
                c1.AddElement(H2);

                c1.Border = 1;
                PdfPCell i1 = new PdfPCell();
                Image jpg = Image.GetInstance(Server.MapPath("~/Image/Company/" + Company.Replace("_", "") + ".png"));
                jpg.ScaleToFit(100f, 60f);
                jpg.Alignment = Element.ALIGN_CENTER;
                PdfPCell i2 = new PdfPCell();
                i2.Border = 0;
                i1.AddElement(jpg);
                i1.Border = 0;
                i1.BorderWidthBottom = 1;
                i2.BorderWidthBottom = 1;
                c1.BorderWidthBottom = 1;
                //i1.BackgroundColor = BaseColor.GREEN;
                //c1.BackgroundColor = BaseColor.GREEN;
                //i2.BackgroundColor = BaseColor.GREEN;
                Htable.AddCell(i1);
                Htable.AddCell(c1);
                Htable.AddCell(i2);
                //Htable.AddCell(c3);

                PdfPCell i4 = new PdfPCell();
                i4.Colspan = 3;
                i4.PaddingTop = 0;

                i4.BorderWidthTop = 0;
                i4.BorderWidthLeft = 0;
                i4.BorderWidthRight = 0;
                Paragraph T1 = new Paragraph("PROFORMA INVOICE", regularFontBold_Black);
                T1.Alignment = Element.ALIGN_CENTER;
                T1.SpacingBefore = 0;
                T1.SpacingAfter = 5;

                i4.AddElement(T1);


                Htable.AddCell(i4);



                PdfPCell c2 = new PdfPCell();
                Paragraph pQ = new Paragraph("BILLING TO :-", regularFontBold);
                pQ.Font.SetColor(Color_R, Color_G, Color_B);
                c2.AddElement(pQ);

                string Customer = "  " + ds.Rows[0]["Customer_Name"].ToString() + "\n";
                Customer += "  " + ds.Rows[0]["Customer_Address1"].ToString() + "\n";
                Customer += "  " + ds.Rows[0]["Customer_Address2"].ToString() + "\n";
                Customer += "  " + ds.Rows[0]["Customer_Address3"].ToString() + "\n";
                Customer += "  " + "GST : " + ds.Rows[0]["GST_No"].ToString();

                Paragraph bt1 = new Paragraph(Customer, regularFont);
                bt1.Font.SetColor(Color_R, Color_G, Color_B);
                bt1.SpacingAfter = 10;
                c2.PaddingLeft = 5;
                c2.AddElement(bt1);
                c2.Border = 0;




                PdfPCell D2 = new PdfPCell();
                Paragraph DP = new Paragraph("SHIPPING  TO :-", regularFontBold);
                DP.Font.SetColor(Color_R, Color_G, Color_B);
                D2.AddElement(DP);
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address1"].ToString(), regularFont));
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address2"].ToString(), regularFont));
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address3"].ToString(), regularFont));
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address4"].ToString(), regularFont));
                D2.AddElement(new Paragraph(ds.Rows[0]["Shiping_Address5"].ToString(), regularFont));

                // c2.AddElement(new Paragraph("Ph : " + ds.Rows[0]["Contact_No"].ToString(), regularFont));
                D2.Border = 0;
                D2.BorderWidthLeft = 1;
                D2.BorderWidthRight = 1;
                D2.PaddingLeft = 5;
                PdfPCell q1 = new PdfPCell();
                q1.AddElement(new Paragraph("Bill No", regularFont));
                q1.AddElement(new Paragraph("Date", regularFont));


                q1.AddElement(new Paragraph("Payment Terms", regularFont));
                q1.AddElement(new Paragraph("Shipping Method", regularFont));

                q1.Border = 0;

                PdfPCell q2 = new PdfPCell();
                try
                {
                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["Bill_No"].ToString(), regularFontBold_Black));
                    q2.AddElement(new Paragraph(": " + DateTime.Parse(ds.Rows[0]["Bill_Date"].ToString()).ToString("dd-MMM-yyyy"), regularFont));

                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["Payment_Terms"].ToString(), regularFont));
                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["Delivery_Mode"].ToString(), regularFont));
                }
                catch { }
                q2.Border = 0;


                PdfPTable Customer_Table = new PdfPTable(4);
                Customer_Table.WidthPercentage = 100;
                float[] hs1 = new float[] { 100f, 100f, 50f, 50f };
                Customer_Table.SetWidths(hs1);
                Customer_Table.AddCell(c2);
                Customer_Table.AddCell(D2);
                Customer_Table.AddCell(q1);
                Customer_Table.AddCell(q2);







                PdfPTable Bottaom_Table = new PdfPTable(1);
                Bottaom_Table.WidthPercentage = 100;
                //float[] hs1 = new float[] { 80f, 20f, 30f };
                //Customer_Table.SetWidths(hs1);
                PdfPCell bf1 = new PdfPCell();
                bf1.AddElement(new Paragraph("Terms & Condition", titleFont));
                bf1.Border = 0;


                Bottaom_Table.AddCell(bf1);








                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(GITAPI.dbFunctions.PDF_left, document.PageSize.Width - GITAPI.dbFunctions.PDF_Right, 100f, 1);

                DataTable th = GITAPI.dbFunctions.getTable("select 'ID' as Field, '#' as Name, 'center' as Align, '5%' as Width ,0 as Order_No union all select Field,Name,Align,Width,order_No from Field_Setting" + Company + " where    Table_Name='Sales_Details" + Company + "' order by order_No ");


                string W = "0";
                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Description"))
                    {
                        W = th.Rows[i]["Width"].ToString().Replace("%", "");
                        th.Rows[i].Delete();
                    }
                }

                th.AcceptChanges();

                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Item_Name"))
                    {
                        th.Rows[i]["Width"] = (decimal.Parse(th.Rows[i]["Width"].ToString().Replace("%", "")) + decimal.Parse(W)).ToString();

                    }
                }



                PdfPTable table = new PdfPTable(th.Rows.Count);
                table.WidthPercentage = 100;
                table.HeaderRows = 1;
                string[] headers = new string[th.Rows.Count];

                for (int k = 0; k < th.Rows.Count; k++)
                {
                    headers[k] = th.Rows[k]["Name"].ToString();



                    Paragraph tab_Header = new Paragraph(th.Rows[k]["Name"].ToString(), regularFontBold_white);

                    PdfPCell cell = new PdfPCell();



                    try
                    {
                        string Align = th.Rows[k]["Align"].ToString();

                        if (Align.ToLower().Equals("right"))
                        {
                            tab_Header.Alignment = Element.ALIGN_RIGHT;

                        }
                        else if (Align.ToLower().Equals("center"))
                        {
                            tab_Header.Alignment = Element.ALIGN_CENTER;
                        }

                    }
                    catch { }


                    cell.AddElement(tab_Header);

                    // cell.Width = this.Columns[k].Width;
                    // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.BackgroundColor = new iTextSharp.text.BaseColor(Color_R, Color_G + 30, Color_B);

                    //cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    //cell.BorderColor = new iTextSharp.text.BaseColor(237, 67, 69);

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;
                    cell.PaddingTop = 0;
                    cell.PaddingBottom = 8;

                    cell.BorderWidthTop = 1;
                    cell.BorderWidthBottom = 1;
                    if (k == 0)
                    {
                        cell.BorderWidthLeft = 0;
                    }

                    table.AddCell(cell);
                }


                float[] anchoDeColumnas = new float[th.Rows.Count];

                for (int i = 0; i < th.Rows.Count; i++)
                {
                    anchoDeColumnas[i] = float.Parse(th.Rows[i]["Width"].ToString().Replace("%", ""));

                }
                table.SetWidths(anchoDeColumnas);
                DataTable td = GITAPI.dbFunctions.getTable("select * from   P_Invoice_details" + Company + " where Bill_No='" + Bill_No + "'");


                //Add values of DataTable in pdf file
                for (int i = 0; i < td.Rows.Count; i++)
                {

                    for (int j = 0; j < th.Rows.Count; j++)
                    {

                        string colum = th.Rows[j]["Field"].ToString();
                        string data = td.Rows[i][colum].ToString();

                        PdfPCell cell = new PdfPCell();

                        if (colum == "ID")
                        {
                            Paragraph Item_Name = new Paragraph((i + 1).ToString(), regularFont);
                            cell.AddElement(Item_Name);

                        }
                        else if (colum == "Item_Name")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Item_Name"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["Description"].ToString(), small_Font);
                            Description.Alignment = Element.ALIGN_LEFT;

                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }
                        else if (colum == "Qty")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Qty"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["UOM"].ToString(), small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }

                        else if (colum == "SGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["SGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["SGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }

                        else if (colum == "CGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["CGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["CGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }
                        else
                        {
                            Paragraph P = new Paragraph(data, regularFont);
                            try
                            {
                                string Align = th.Rows[j]["Align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    P.Alignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    P.Alignment = Element.ALIGN_CENTER;
                                }

                            }
                            catch { }

                            cell.AddElement(P);
                        }


                        //  cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;

                        cell.BorderWidthTop = 0;
                        cell.BorderWidthBottom = 0;
                        if (j == 0)
                        {
                            cell.BorderWidthLeft = 0;
                        }

                        //cell.BorderWidthRight = 1;
                        table.AddCell(cell);



                    }
                }




                Rows = Rows + (td.Rows.Count * Rows_Lent);
                for (int j = 0; j < th.Rows.Count; j++)
                {
                    PdfPCell cell = new PdfPCell();

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;


                    //  cell.FixedHeight = 370 - Rows;
                    cell.BorderWidthTop = 0;
                    //cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    cell.BorderWidthBottom = 1;
                    if (j == 0)
                    {
                        cell.BorderWidthLeft = 0;
                    }

                    //cell.BorderWidthRight = 1;
                    table.AddCell(cell);
                }

                table.SpacingAfter = 10;





                columns.AddElement(Htable);

                columns.AddElement(Customer_Table);

                table.SpacingAfter = 150;
                //table.SetExtendLastRow(true, true);
                columns.AddElement(table);

                columns.AddElement(Line);


                document.Add(columns);
                GITAPI.dbFunctions.Company = Company;
                GITAPI.dbFunctions.Bill_No = Bill_No;
                writer.PageEvent = new Profroma_Report_Footer();


                document.Close();

                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");

            }
        }



        public FileResult Export_PO_1(string User, string Company, string File_Name, string File_Type, string PO_No)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;
                Load_Comapny(Company);
                load_Page_Setting(Company);

                Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);
                if (GITAPI.dbFunctions.PDF_Page_Size == "a4")
                {
                    document = new Document(PageSize.A4, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    }

                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a3")
                {
                    document = new Document(PageSize.A3, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
                    }
                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a5")
                {
                    document = new Document(PageSize.A5, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                    }
                }



                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                writer.PageEvent = new Border();

                //writer.PageEvent = new PDFFooter();
                PageEventHelper pageEventHelper = new PageEventHelper();
                writer.PageEvent = pageEventHelper;
                document.Open();
                GITAPI.dbFunctions.PDF_Bottom = 10;



                FontFactory.RegisterDirectories();

                iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
                iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);
                iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
                iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
                iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
                iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);



                iTextSharp.text.Font subject = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                subject.SetColor(105, 105, 105);
                small_Font.SetColor(105, 105, 105);
                int Color_R = 0;
                int Color_G = 0;
                int Color_B = 0;
                regularFontBold_white.SetColor(0, 0, 0);
                fn_Header.SetColor(Color_R, Color_G, Color_B);
                fn_Title.SetColor(Color_R, Color_G, Color_B);
                regularFontBold.SetColor(Color_R, Color_G, Color_B);


                float Rows = 0;
                float Rows_Lent = 35 + (10 - GITAPI.dbFunctions.PDF_Font_Size);

                string Address = "";

                if (GITAPI.dbFunctions.CM_Address1.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address1 + "\n";

                if (GITAPI.dbFunctions.CM_Address2.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address2 + "\n";

                if (GITAPI.dbFunctions.CM_Address3.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address3 + "\n";

                if (GITAPI.dbFunctions.CM_Address4.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address4 + "\n";


                DataTable ds = GITAPI.dbFunctions.getTable("select * from Purchase_Order" + Company + " where PO_No='" + PO_No + "'");





                PdfPTable Line = new PdfPTable(1);
                Line.WidthPercentage = 100;
                PdfPCell l1 = new PdfPCell();
                l1.Border = 0;
                Line.AddCell(l1);



                PdfPTable Htable = new PdfPTable(3);
                Htable.WidthPercentage = 100;

                float[] hs = new float[] { 20f, 80f, 20f };

                Htable.SetWidths(hs);
                PdfPCell c1 = new PdfPCell();

                Paragraph H1 = new Paragraph(GITAPI.dbFunctions.CM_Name, fn_Header);

                H1.Alignment = Element.ALIGN_CENTER;
                c1.AddElement(H1);


                Paragraph H2 = new Paragraph(Address, regularFont);

                H2.Alignment = Element.ALIGN_CENTER;

                H2.SpacingAfter = 10;
                c1.AddElement(H2);

                c1.Border = 1;
                PdfPCell i1 = new PdfPCell();
                Image jpg = Image.GetInstance(Server.MapPath("~/Image/Company/" + Company.Replace("_", "") + ".png"));
                jpg.ScaleToFit(100f, 60f);
                jpg.Alignment = Element.ALIGN_CENTER;
                PdfPCell i2 = new PdfPCell();
                i2.Border = 0;
                i1.AddElement(jpg);
                i1.Border = 0;
                i1.BorderWidthBottom = 1;
                i2.BorderWidthBottom = 1;
                c1.BorderWidthBottom = 1;
                //i1.BackgroundColor = BaseColor.GREEN;
                //c1.BackgroundColor = BaseColor.GREEN;
                //i2.BackgroundColor = BaseColor.GREEN;
                Htable.AddCell(i1);
                Htable.AddCell(c1);
                Htable.AddCell(i2);
                //Htable.AddCell(c3);

                PdfPCell i4 = new PdfPCell();
                i4.Colspan = 3;
                i4.PaddingTop = 0;

                i4.BorderWidthTop = 0;
                i4.BorderWidthLeft = 0;
                i4.BorderWidthRight = 0;
                Paragraph T1 = new Paragraph("PURCHASE ORDER", regularFontBold_Black);
                T1.Alignment = Element.ALIGN_CENTER;
                T1.SpacingBefore = 0;
                T1.SpacingAfter = 5;

                i4.AddElement(T1);


                Htable.AddCell(i4);



                PdfPCell c2 = new PdfPCell();
                Paragraph pQ = new Paragraph("PURCHASE FROM :-", regularFontBold);
                pQ.Font.SetColor(Color_R, Color_G, Color_B);
                c2.AddElement(pQ);

                string Customer = "  " + ds.Rows[0]["Supplier_Name"].ToString() + "\n";
                Customer += "  " + ds.Rows[0]["Supplier_Address1"].ToString() + "\n";
                Customer += "  " + ds.Rows[0]["Supplier_Address2"].ToString() + "\n";
                Customer += "  " + ds.Rows[0]["Supplier_Address3"].ToString() + "\n";
                Customer += "  " + "GST : " + ds.Rows[0]["GST_No"].ToString();

                Paragraph bt1 = new Paragraph(Customer, regularFont);
                bt1.Font.SetColor(Color_R, Color_G, Color_B);
                bt1.SpacingAfter = 10;
                c2.PaddingLeft = 5;
                c2.AddElement(bt1);
                c2.Border = 0;




                PdfPCell D2 = new PdfPCell();
                Paragraph DP = new Paragraph("SHIPPING  TO :-", regularFontBold);
                DP.Font.SetColor(Color_R, Color_G, Color_B);
                D2.AddElement(DP);
                D2.AddElement(new Paragraph(GITAPI.dbFunctions.CM_Name, regularFont));
                D2.AddElement(new Paragraph(GITAPI.dbFunctions.CM_Address1, regularFont));
                D2.AddElement(new Paragraph(GITAPI.dbFunctions.CM_Address2, regularFont));
                D2.AddElement(new Paragraph(GITAPI.dbFunctions.CM_Address3, regularFont));
                D2.AddElement(new Paragraph(GITAPI.dbFunctions.CM_GST_No, regularFont));

                // c2.AddElement(new Paragraph("Ph : " + ds.Rows[0]["Contact_No"].ToString(), regularFont));
                D2.Border = 0;
                D2.BorderWidthLeft = 1;
                D2.BorderWidthRight = 1;
                D2.PaddingLeft = 5;
                PdfPCell q1 = new PdfPCell();
                q1.AddElement(new Paragraph("PO No", regularFont));
                q1.AddElement(new Paragraph("Date", regularFont));

                q1.AddElement(new Paragraph("Validity", regularFont));
                q1.AddElement(new Paragraph("Payment Terms", regularFont));
                q1.AddElement(new Paragraph("Shipping Method", regularFont));

                q1.Border = 0;

                PdfPCell q2 = new PdfPCell();
                try
                {
                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["PO_No"].ToString(), regularFontBold_Black));
                    q2.AddElement(new Paragraph(": " + DateTime.Parse(ds.Rows[0]["Bill_Date"].ToString()).ToString("dd-MMM-yyyy"), regularFont));
                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["Valid_For"].ToString(), regularFont));
                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["Payment_Terms"].ToString(), regularFont));
                    q2.AddElement(new Paragraph(": " + ds.Rows[0]["Delivery_Mode"].ToString(), regularFont));
                }
                catch { }
                q2.Border = 0;


                PdfPTable Customer_Table = new PdfPTable(4);
                Customer_Table.WidthPercentage = 100;
                float[] hs1 = new float[] { 100f, 100f, 50f, 50f };
                Customer_Table.SetWidths(hs1);
                Customer_Table.AddCell(c2);
                Customer_Table.AddCell(D2);
                Customer_Table.AddCell(q1);
                Customer_Table.AddCell(q2);







                PdfPTable Bottaom_Table = new PdfPTable(1);
                Bottaom_Table.WidthPercentage = 100;
                //float[] hs1 = new float[] { 80f, 20f, 30f };
                //Customer_Table.SetWidths(hs1);
                PdfPCell bf1 = new PdfPCell();
                bf1.AddElement(new Paragraph("Terms & Condition", titleFont));
                bf1.Border = 0;


                Bottaom_Table.AddCell(bf1);








                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(GITAPI.dbFunctions.PDF_left, document.PageSize.Width - GITAPI.dbFunctions.PDF_Right, 100f, 1);

                DataTable th = GITAPI.dbFunctions.getTable("select 'ID' as Field, '#' as Name, 'center' as Align, '5%' as Width ,0 as Order_No union all select Field,Name,Align,Width,order_No from Field_Setting" + Company + " where    Table_Name='Sales_Details" + Company + "' order by order_No ");


                string W = "0";
                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Description"))
                    {
                        W = th.Rows[i]["Width"].ToString().Replace("%", "");
                        th.Rows[i].Delete();
                    }
                }

                th.AcceptChanges();

                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Item_Name"))
                    {
                        th.Rows[i]["Width"] = (decimal.Parse(th.Rows[i]["Width"].ToString().Replace("%", "")) + decimal.Parse(W)).ToString();

                    }
                }



                PdfPTable table = new PdfPTable(th.Rows.Count);
                table.WidthPercentage = 100;
                table.HeaderRows = 1;
                string[] headers = new string[th.Rows.Count];

                for (int k = 0; k < th.Rows.Count; k++)
                {
                    headers[k] = th.Rows[k]["Name"].ToString();



                    Paragraph tab_Header = new Paragraph(th.Rows[k]["Name"].ToString(), regularFontBold_white);

                    PdfPCell cell = new PdfPCell();



                    try
                    {
                        string Align = th.Rows[k]["Align"].ToString();

                        if (Align.ToLower().Equals("right"))
                        {
                            tab_Header.Alignment = Element.ALIGN_RIGHT;

                        }
                        else if (Align.ToLower().Equals("center"))
                        {
                            tab_Header.Alignment = Element.ALIGN_CENTER;
                        }

                    }
                    catch { }


                    cell.AddElement(tab_Header);

                    // cell.Width = this.Columns[k].Width;
                    // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.BackgroundColor = new iTextSharp.text.BaseColor(Color_R, Color_G + 30, Color_B);

                    //cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    //cell.BorderColor = new iTextSharp.text.BaseColor(237, 67, 69);

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;
                    cell.PaddingTop = 0;
                    cell.PaddingBottom = 8;

                    cell.BorderWidthTop = 1;
                    cell.BorderWidthBottom = 1;
                    if (k == 0)
                    {
                        cell.BorderWidthLeft = 0;
                    }

                    table.AddCell(cell);
                }


                float[] anchoDeColumnas = new float[th.Rows.Count];

                for (int i = 0; i < th.Rows.Count; i++)
                {
                    anchoDeColumnas[i] = float.Parse(th.Rows[i]["Width"].ToString().Replace("%", ""));

                }
                table.SetWidths(anchoDeColumnas);
                DataTable td = GITAPI.dbFunctions.getTable("select * from   Purchase_Order_Details" + Company + " where PO_No='" + PO_No + "'");


                //Add values of DataTable in pdf file
                for (int i = 0; i < td.Rows.Count; i++)
                {

                    for (int j = 0; j < th.Rows.Count; j++)
                    {

                        string colum = th.Rows[j]["Field"].ToString();
                        string data = td.Rows[i][colum].ToString();

                        PdfPCell cell = new PdfPCell();

                        if (colum == "ID")
                        {
                            Paragraph Item_Name = new Paragraph((i + 1).ToString(), regularFont);
                            cell.AddElement(Item_Name);

                        }
                        else if (colum == "Item_Name")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Item_Name"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["Description"].ToString(), small_Font);
                            Description.Alignment = Element.ALIGN_LEFT;

                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }
                        else if (colum == "Qty")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Qty"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["UOM"].ToString(), small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }

                        else if (colum == "SGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["SGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["SGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }

                        else if (colum == "CGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["CGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["CGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }
                        else
                        {
                            Paragraph P = new Paragraph(data, regularFont);
                            try
                            {
                                string Align = th.Rows[j]["Align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    P.Alignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    P.Alignment = Element.ALIGN_CENTER;
                                }

                            }
                            catch { }

                            cell.AddElement(P);
                        }


                        //  cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;

                        cell.BorderWidthTop = 0;
                        cell.BorderWidthBottom = 0;
                        if (j == 0)
                        {
                            cell.BorderWidthLeft = 0;
                        }

                        //cell.BorderWidthRight = 1;
                        table.AddCell(cell);



                    }
                }




                Rows = Rows + (td.Rows.Count * Rows_Lent);
                for (int j = 0; j < th.Rows.Count; j++)
                {
                    PdfPCell cell = new PdfPCell();

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;


                    //  cell.FixedHeight = 370 - Rows;
                    cell.BorderWidthTop = 0;
                    //cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    cell.BorderWidthBottom = 1;
                    if (j == 0)
                    {
                        cell.BorderWidthLeft = 0;
                    }

                    //cell.BorderWidthRight = 1;
                    table.AddCell(cell);
                }

                table.SpacingAfter = 10;





                columns.AddElement(Htable);

                columns.AddElement(Customer_Table);

                table.SpacingAfter = 150;
                //table.SetExtendLastRow(true, true);
                columns.AddElement(table);

                columns.AddElement(Line);


                document.Add(columns);
                GITAPI.dbFunctions.Company = Company;
                GITAPI.dbFunctions.Bill_No = PO_No;
                writer.PageEvent = new Purchse_Report_Footer();


                document.Close();

                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");

            }
        }


        public FileResult Export_PInvoice_1(string User, string Company, string File_Name, string File_Type, string Bill_No)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;
                Load_Comapny(Company);
                load_Page_Setting(Company);

                Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);
                if (GITAPI.dbFunctions.PDF_Page_Size == "a4")
                {
                    document = new Document(PageSize.A4, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    }

                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a3")
                {
                    document = new Document(PageSize.A3, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
                    }
                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a5")
                {
                    document = new Document(PageSize.A5, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                    }
                }



                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                // writer.PageEvent = new Border();

                //writer.PageEvent = new PDFFooter();
                PageEventHelper pageEventHelper = new PageEventHelper();
                writer.PageEvent = pageEventHelper;
                document.Open();
                GITAPI.dbFunctions.PDF_Bottom = 10;



                FontFactory.RegisterDirectories();

                iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
                iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);
                iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
                iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
                iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
                iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);



                iTextSharp.text.Font subject = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                subject.SetColor(105, 105, 105);
                small_Font.SetColor(105, 105, 105);
                int Color_R = 0;
                int Color_G = 0;
                int Color_B = 0;
                regularFontBold_white.SetColor(0, 0, 0);
                fn_Header.SetColor(Color_R, Color_G, Color_B);
                fn_Title.SetColor(Color_R, Color_G, Color_B);
                regularFontBold.SetColor(Color_R, Color_G, Color_B);


                float Rows = 0;
                float Rows_Lent = 35 + (10 - GITAPI.dbFunctions.PDF_Font_Size);

                string Address = "";

                if (GITAPI.dbFunctions.CM_Address1.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address1 + "\n";

                if (GITAPI.dbFunctions.CM_Address2.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address2 + "\n";

                if (GITAPI.dbFunctions.CM_Address3.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address3 + "\n";

                if (GITAPI.dbFunctions.CM_Address4.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address4 + "\n";


                DataTable ds = GITAPI.dbFunctions.getTable("select * from P_invoice" + Company + " where Bill_No='" + Bill_No + "'");
                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(GITAPI.dbFunctions.PDF_left, document.PageSize.Width - GITAPI.dbFunctions.PDF_Right, 100f, 1);




                PdfPTable Htable = new PdfPTable(2);
                Htable.WidthPercentage = 100;
                float[] hs = new float[] { 50f, 50f };

                Htable.SetWidths(hs);

                PdfPCell c1 = new PdfPCell();
                Paragraph p11 = new Paragraph("" + GITAPI.dbFunctions.CM_Name, fn_Title);
                Paragraph p12 = new Paragraph("" + Address, Fn_Address);
                c1.AddElement(p11);
                c1.AddElement(p12);
                c1.PaddingLeft = 50;
                c1.BorderWidth = 0;
                Htable.AddCell(c1);

                PdfPCell c2 = new PdfPCell();
                Paragraph p2 = new Paragraph("PURCHASE", fn_Header);
                p2.Alignment = Element.ALIGN_RIGHT;

                c2.AddElement(p2);
                c2.BorderWidth = 0;


                Paragraph pq1 = new Paragraph();
                pq1.Add(new Chunk("Purchase No", regularFont));
                pq1.Add(new Chunk(" : " + ds.Rows[0]["Bill_No"].ToString(), regularFontBold));
                Paragraph pqd = new Paragraph();//"Phone No: 0412 8765", Fn_Address);
                pqd.Add(new Chunk("Purchase Date", regularFont));
                pqd.Add(new Chunk(" : " + DateTime.Parse(ds.Rows[0]["Bill_Date"].ToString()).ToString("dd/MM/yyyy"), regularFontBold));

                pq1.Alignment = Element.ALIGN_RIGHT;
                pqd.Alignment = Element.ALIGN_RIGHT;
                c2.AddElement(pq1);
                c2.AddElement(pqd);
                c2.PaddingRight = 20;
                Htable.AddCell(c2);






                PdfPTable TAB2 = new PdfPTable(2);
                TAB2.WidthPercentage = 100;
                float[] TAB2W = new float[] { 50f, 50f };

                TAB2.SetWidths(TAB2W);
                titleFont.SetColor(255, 255, 255);
                PdfPCell T2c1 = new PdfPCell();
                T2c1.UseAscender = true;
                T2c1.PaddingTop = 8;
                T2c1.PaddingBottom = 8;
                Paragraph T2P1 = new Paragraph("Purchase From", titleFont);


                T2c1.AddElement(T2P1);


                T2c1.PaddingLeft = 50;
                BaseColor b = new BaseColor(84, 182, 231);
                T2c1.BackgroundColor = b;
                T2c1.BorderWidth = 0;
                TAB2.AddCell(T2c1);



                PdfPCell T2c2 = new PdfPCell();
                T2c2.UseAscender = true;
                T2c2.PaddingTop = 8;
                T2c2.PaddingBottom = 8;

                Paragraph T2P = new Paragraph("Skip To", titleFont);
                T2c2.AddElement(T2P);
                T2c2.BorderWidth = 0;
                T2c2.PaddingLeft = 50;

                // BaseColor b = new BaseColor(84,182,231);                
                T2c2.BackgroundColor = b;
                TAB2.AddCell(T2c2);

                PdfPTable TAB3 = new PdfPTable(2);
                TAB3.WidthPercentage = 100;
                float[] TAB3W = new float[] { 50f, 50f };

                TAB3.SetWidths(TAB3W);

                PdfPCell T3c1 = new PdfPCell();
                Paragraph T3P1 = new Paragraph();//"Taylor Dickens", Fn_Address);
                T3P1.Add(new Chunk("SBV TECHNOLOGIES ", regularFontBold));
                Paragraph T3P2 = new Paragraph("NO:19 NARAYANAN NAGAR", Fn_Address);
                Paragraph T3P3 = new Paragraph("THIRUPAPULIYUR", Fn_Address);
                Paragraph T3P4 = new Paragraph("CUDDALORE", Fn_Address);
                Paragraph T3P5 = new Paragraph("TAMILNADU: 607002", Fn_Address);
                Paragraph T3P6 = new Paragraph(" ", Fn_Address);
                Paragraph T3P7 = new Paragraph();
                T3P7.Add(new Chunk("PH: ", regularFontBold));
                T3P7.Add(new Chunk("9940763874", regularFont));


                T3c1.AddElement(T3P1);
                T3c1.AddElement(T3P2);
                T3c1.AddElement(T3P3);
                T3c1.AddElement(T3P4);
                T3c1.AddElement(T3P5);
                T3c1.AddElement(T3P6);
                T3c1.AddElement(T3P7);

                T3c1.PaddingLeft = 50;
                BaseColor b3 = new BaseColor(246, 247, 249);

                T3c1.BackgroundColor = b3;

                T3c1.BorderWidth = 0;
                TAB3.AddCell(T3c1);



                PdfPCell T3c2 = new PdfPCell();
                Paragraph T3P = new Paragraph();//"BOSTON Office", Fn_Address);

                T3P.Add(new Chunk("SBV TECHNOLOGIES ", regularFontBold));
                Paragraph T3CP2 = new Paragraph("NO:19 NARAYANAN NAGAR", Fn_Address);
                Paragraph T3CP3 = new Paragraph("THIRUPAPULIYUR", Fn_Address);
                Paragraph T3CP4 = new Paragraph("CUDDALORE", Fn_Address);
                Paragraph T3CP5 = new Paragraph("TAMILNADU: 607002", Fn_Address);
                Paragraph T3CP6 = new Paragraph(" ", Fn_Address);
                Paragraph T3CP7 = new Paragraph();
                T3CP7.Add(new Chunk("PH: ", regularFontBold));
                T3CP7.Add(new Chunk("9940763874", regularFont));


                T3c2.AddElement(T3P);
                T3c2.AddElement(T3CP2);
                T3c2.AddElement(T3CP3);
                T3c2.AddElement(T3CP4);
                T3c2.AddElement(T3CP5);
                T3c2.AddElement(T3CP6);
                T3c2.AddElement(T3CP7);



                T3c2.PaddingLeft = 50;
                BaseColor b2 = new BaseColor(246, 247, 249);

                T3c2.BackgroundColor = b2;

                T3c2.BorderWidth = 0;


                TAB3.AddCell(T3c2);

                PdfPTable TAB4 = new PdfPTable(4);
                TAB4.WidthPercentage = 100;
                float[] TAB4W = new float[] { 25f, 25f, 25f, 25f };

                TAB4.SetWidths(TAB4W);

                PdfPCell T4c1 = new PdfPCell();


                Paragraph T4P1 = new Paragraph("DELIVERY DATE", titleFont);


                T4c1.AddElement(T4P1);

                T4c1.UseAscender = true;
                T4c1.PaddingTop = 8;
                T4c1.PaddingBottom = 8;

                T4c1.BackgroundColor = b;
                T4c1.BorderWidth = 0;
                TAB4.AddCell(T4c1);



                PdfPCell T4c2 = new PdfPCell();
                Paragraph T4P = new Paragraph("REQUESTED BY", titleFont);
                T4c2.AddElement(T4P);
                T4c2.BorderWidth = 0;
                T4c2.PaddingLeft = 10;
                T4c2.UseAscender = true;
                T4c2.PaddingTop = 8;
                T4c2.PaddingBottom = 8;


                T4c2.BackgroundColor = b;
                TAB4.AddCell(T4c2);

                PdfPCell T4c3 = new PdfPCell();
                Paragraph T4CP = new Paragraph("APPROVED BY", titleFont);
                T4c3.AddElement(T4CP);
                T4c3.BorderWidth = 0;
                T4c3.PaddingLeft = 10;
                T4c3.UseAscender = true;
                T4c3.PaddingTop = 8;
                T4c3.PaddingBottom = 8;


                T4c3.BackgroundColor = b;
                TAB4.AddCell(T4c3);

                PdfPCell T4c4 = new PdfPCell();
                Paragraph T4C4P = new Paragraph("DEPORTMENT", titleFont);
                T4c4.AddElement(T4C4P);
                T4c4.BorderWidth = 0;
                T4c4.PaddingLeft = 10;
                T4c4.UseAscender = true;
                T4c4.PaddingTop = 8;
                T4c4.PaddingBottom = 8;


                T4c4.BackgroundColor = b;
                TAB4.AddCell(T4c4);

                PdfPTable TAB5 = new PdfPTable(4);
                TAB5.WidthPercentage = 100;
                float[] TAB5W = new float[] { 25f, 25f, 25f, 25f };

                TAB5.SetWidths(TAB5W);

                PdfPCell T5c1 = new PdfPCell();
                Paragraph T5P1 = new Paragraph();//5/2020", titleFont);
                T5P1.Add(new Chunk("06/06/2020", regularFont));


                T5c1.AddElement(T5P1);

                BaseColor b1 = new BaseColor(246, 247, 249);

                T5c1.BackgroundColor = b1;
                T5c1.PaddingLeft = 10;
                T5c1.UseAscender = true;
                T5c1.PaddingTop = 8;
                T5c1.PaddingBottom = 8;
                T5c1.BorderWidth = 0;
                TAB5.AddCell(T5c1);



                PdfPCell T5c2 = new PdfPCell();
                Paragraph T5P = new Paragraph();//"Patrick Smith", titleFont);
                T5P.Add(new Chunk("Patrick Smith", regularFont));
                T5c2.BackgroundColor = b1;

                T5c2.AddElement(T5P);
                T5c2.BorderWidth = 0;
                T5c2.PaddingLeft = 10;
                T5c2.UseAscender = true;
                T5c2.PaddingTop = 8;
                T5c2.PaddingBottom = 8;
                TAB5.AddCell(T5c2);

                PdfPCell T5c3 = new PdfPCell();
                Paragraph T5CP = new Paragraph();//"Patrick Smith", titleFont);
                T5CP.Add(new Chunk("Patrick Smith", regularFont));

                T5c3.BackgroundColor = b1;

                T5c3.AddElement(T5CP);
                T5c3.BorderWidth = 0;
                T5c3.PaddingLeft = 10;
                T5c3.UseAscender = true;
                T5c3.PaddingTop = 8;
                T5c3.PaddingBottom = 8;
                TAB5.AddCell(T5c3);

                PdfPCell T5c4 = new PdfPCell();
                Paragraph T5C4P = new Paragraph();//" IT DEPORTMENT", titleFont);
                T5C4P.Add(new Chunk("IT DEPORTMENT", regularFont));

                T5c4.BackgroundColor = b1;

                T5c4.AddElement(T5C4P);
                T5c4.BorderWidth = 0;
                T5c4.PaddingLeft = 10;
                T5c4.UseAscender = true;
                T5c4.PaddingTop = 8;
                T5c4.PaddingBottom = 8;
                TAB5.AddCell(T5c4);

                PdfPTable TAB6 = new PdfPTable(1);
                TAB6.WidthPercentage = 100;
                float[] TAB6W = new float[] { 100f };

                TAB6.SetWidths(TAB6W);

                PdfPCell T6c1 = new PdfPCell();
                Paragraph T6P1 = new Paragraph("NOTES", titleFont);


                T6c1.AddElement(T6P1);
                T6c1.BackgroundColor = b;

                T6c1.PaddingLeft = 10;
                T6c1.UseAscender = true;
                T6c1.PaddingTop = 8;
                T6c1.PaddingBottom = 8;
                T6c1.BorderWidth = 0;
                T6c1.BackgroundColor = b;

                TAB6.AddCell(T6c1);

                PdfPTable TAB7 = new PdfPTable(1);
                TAB7.WidthPercentage = 100;
                float[] TAB7W = new float[] { 100f };

                TAB7.SetWidths(TAB7W);

                PdfPCell T7c1 = new PdfPCell();
                Paragraph T7P1 = new Paragraph();
                T7P1.Add(new Chunk("Description ABC", regularFont));


                T7c1.AddElement(T7P1);

                T7c1.BackgroundColor = b1;

                T7c1.PaddingLeft = 10;

                T7c1.UseAscender = true;
                T7c1.PaddingTop = 8;
                T7c1.PaddingBottom = 8;
                T7c1.BorderWidth = 0;
                TAB7.AddCell(T7c1);

                columns.AddElement(Htable);


                columns.AddElement(TAB2);


                columns.AddElement(TAB3);
                TAB3.SpacingAfter = 8;

                columns.AddElement(TAB4);

                columns.AddElement(TAB5);

                TAB6.SpacingBefore = 8;

                columns.AddElement(TAB6);

                columns.AddElement(TAB7);
                TAB7.SpacingAfter = 8;







                DataTable th = GITAPI.dbFunctions.getTable("select 'ID' as Field, '#' as Name, 'center' as Align, '5%' as Width ,0 as Order_No union all select Field,Name,Align,Width,order_No from Field_Setting" + Company + " where    Table_Name='Purchase_Order_Details" + Company + "' order by order_No ");


                string W = "0";
                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Description"))
                    {
                        W = th.Rows[i]["Width"].ToString().Replace("%", "");
                        th.Rows[i].Delete();
                    }
                }

                th.AcceptChanges();

                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Item_Name"))
                    {
                        th.Rows[i]["Width"] = (decimal.Parse(th.Rows[i]["Width"].ToString().Replace("%", "")) + decimal.Parse(W)).ToString();

                    }
                }



                PdfPTable table = new PdfPTable(th.Rows.Count);
                table.WidthPercentage = 100;
                table.HeaderRows = 1;
                string[] headers = new string[th.Rows.Count];

                for (int k = 0; k < th.Rows.Count; k++)
                {
                    headers[k] = th.Rows[k]["Name"].ToString();



                    Paragraph tab_Header = new Paragraph(th.Rows[k]["Name"].ToString(), titleFont);

                    PdfPCell cell = new PdfPCell();



                    try
                    {
                        string Align = th.Rows[k]["Align"].ToString();

                        if (Align.ToLower().Equals("right"))
                        {
                            tab_Header.Alignment = Element.ALIGN_RIGHT;

                        }
                        else if (Align.ToLower().Equals("center"))
                        {
                            tab_Header.Alignment = Element.ALIGN_CENTER;
                        }

                    }
                    catch { }


                    cell.AddElement(tab_Header);

                    // cell.Width = this.Columns[k].Width;
                    // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.BackgroundColor = b;

                    //cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    cell.BorderColor = new iTextSharp.text.BaseColor(255, 255, 255);

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;
                    cell.PaddingTop = 0;
                    cell.PaddingBottom = 8;

                    cell.BorderWidthTop = 0;
                    cell.BorderWidthBottom = 1;


                    table.AddCell(cell);
                }


                float[] anchoDeColumnas = new float[th.Rows.Count];

                for (int i = 0; i < th.Rows.Count; i++)
                {
                    anchoDeColumnas[i] = float.Parse(th.Rows[i]["Width"].ToString().Replace("%", ""));

                }
                table.SetWidths(anchoDeColumnas);
                DataTable td = GITAPI.dbFunctions.getTable("select * from   purchase_order_details" + Company + " where Po_No='" + Bill_No + "'");


                //Add values of DataTable in pdf file
                for (int i = 0; i < td.Rows.Count; i++)
                {

                    for (int j = 0; j < th.Rows.Count; j++)
                    {

                        string colum = th.Rows[j]["Field"].ToString();
                        string data = td.Rows[i][colum].ToString();

                        PdfPCell cell = new PdfPCell();

                        if (colum == "ID")
                        {
                            Paragraph Item_Name = new Paragraph((i + 1).ToString(), regularFont);
                            cell.AddElement(Item_Name);

                        }
                        else if (colum == "Item_Name")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Item_Name"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["Description"].ToString(), small_Font);
                            Description.Alignment = Element.ALIGN_LEFT;

                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }
                        else if (colum == "Qty")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Qty"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["UOM"].ToString(), small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }

                        else if (colum == "SGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["SGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["SGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }

                        else if (colum == "CGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["CGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["CGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }
                        else
                        {
                            Paragraph P = new Paragraph(data, regularFont);
                            try
                            {
                                string Align = th.Rows[j]["Align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    P.Alignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    P.Alignment = Element.ALIGN_CENTER;
                                }

                            }
                            catch { }

                            cell.AddElement(P);
                        }


                        cell.BorderColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;

                        if (i % 2 == 0)
                        {
                            cell.BackgroundColor = b1;
                        }

                        cell.BorderWidthTop = 0;
                        cell.BorderWidthBottom = 0;
                        if (j == 0)
                        {
                            cell.BorderWidthLeft = 0;
                        }

                        //cell.BorderWidthRight = 1;
                        table.AddCell(cell);



                    }
                }


                table.SpacingAfter = 10;





                columns.AddElement(table);



                document.Add(columns);
                GITAPI.dbFunctions.Company = Company;
                GITAPI.dbFunctions.Bill_No = Bill_No;
                // writer.PageEvent = new Report_Footer();


                document.Close();

                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");

            }
        }



        public FileResult Export_Quotation_1(string User, string Company, string File_Name, string File_Type, string Quote_No)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;
                Load_Comapny(Company);
                load_Page_Setting(Company);

                Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);
                if (GITAPI.dbFunctions.PDF_Page_Size == "a4")
                {
                    document = new Document(PageSize.A4, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    }

                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a3")
                {
                    document = new Document(PageSize.A3, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
                    }
                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a5")
                {
                    document = new Document(PageSize.A5, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                    }
                }



                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                // writer.PageEvent = new Border();

                //writer.PageEvent = new PDFFooter();
                // PageEventHelper pageEventHelper = new PageEventHelper();
                // writer.PageEvent = pageEventHelper;
                document.Open();
                GITAPI.dbFunctions.PDF_Bottom = 10;



                FontFactory.RegisterDirectories();

                iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
                iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);
                iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
                iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
                iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
                iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);



                iTextSharp.text.Font subject = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                subject.SetColor(105, 105, 105);
                small_Font.SetColor(105, 105, 105);
                int Color_R = 0;
                int Color_G = 0;
                int Color_B = 0;
                regularFontBold_white.SetColor(0, 0, 0);
                fn_Header.SetColor(Color_R, Color_G, Color_B);
                fn_Title.SetColor(Color_R, Color_G, Color_B);
                regularFontBold.SetColor(Color_R, Color_G, Color_B);


                float Rows = 0;
                float Rows_Lent = 35 + (10 - GITAPI.dbFunctions.PDF_Font_Size);

                string Address = "";

                if (GITAPI.dbFunctions.CM_Address1.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address1 + "\n";

                if (GITAPI.dbFunctions.CM_Address2.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address2 + "\n";

                if (GITAPI.dbFunctions.CM_Address3.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address3 + "\n";

                if (GITAPI.dbFunctions.CM_Address4.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address4 + "\n";


                DataTable ds = GITAPI.dbFunctions.getTable("select * from Quotation" + Company + " where Quote_No='" + Quote_No + "'");
                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(GITAPI.dbFunctions.PDF_left, document.PageSize.Width - GITAPI.dbFunctions.PDF_Right, 100f, 1);


                Image jpg = Image.GetInstance(Server.MapPath("~/Image/Company/" + Company.Replace("_", "") + ".png"));
                jpg.ScaleToFit(150, 50);

                //columns.AddElement(jpg);


                PdfPTable Htable = new PdfPTable(2);
                Htable.WidthPercentage = 100;
                float[] hs = new float[] { 50f, 50f };

                Htable.SetWidths(hs);

                PdfPCell c1 = new PdfPCell();
                Paragraph p11 = new Paragraph("" + GITAPI.dbFunctions.CM_Name, fn_Title);
                Paragraph p12 = new Paragraph("" + Address, Fn_Address);
                c1.AddElement(jpg);

                //c1.AddElement(p11);
                //c1.AddElement(p12);
                c1.PaddingLeft = 20;
                c1.BorderWidth = 0;
                Htable.SpacingAfter = 10;
                Htable.AddCell(c1);




                PdfPCell c2 = new PdfPCell();
                Paragraph p2 = new Paragraph("QUOTATION", fn_Header);
                p2.Alignment = Element.ALIGN_RIGHT;

                c2.AddElement(p2);
                c2.BorderWidth = 0;


                Paragraph pq1 = new Paragraph();
                pq1.Add(new Chunk("Quote No", regularFont));
                pq1.Add(new Chunk(" : " + ds.Rows[0]["Quote_No"].ToString(), regularFontBold));
                Paragraph pqd = new Paragraph();//"Phone No: 0412 8765", Fn_Address);
                pqd.Add(new Chunk("Quote Date", regularFont));
                pqd.Add(new Chunk(" : " + DateTime.Parse(ds.Rows[0]["Quote_Date"].ToString()).ToString("dd/MM/yyyy"), regularFontBold));

                pq1.Alignment = Element.ALIGN_RIGHT;
                pqd.Alignment = Element.ALIGN_RIGHT;
                c2.AddElement(pq1);
                c2.AddElement(pqd);
                c2.PaddingRight = 20;
                Htable.AddCell(c2);




                PdfPTable TAB2 = new PdfPTable(2);
                TAB2.WidthPercentage = 100;
                float[] TAB2W = new float[] { 50f, 50f };

                TAB2.SetWidths(TAB2W);
                titleFont.SetColor(255, 255, 255);
                PdfPCell T2c1 = new PdfPCell();
                T2c1.UseAscender = true;
                T2c1.PaddingTop = 8;
                T2c1.PaddingBottom = 8;
                Paragraph T2P1 = new Paragraph("To", titleFont);


                T2c1.AddElement(T2P1);


                T2c1.PaddingLeft = 30;
                BaseColor b = new BaseColor(84, 182, 231);
                T2c1.BackgroundColor = b;
                T2c1.BorderWidth = 0;
                TAB2.AddCell(T2c1);



                PdfPCell T2c2 = new PdfPCell();
                T2c2.UseAscender = true;
                T2c2.PaddingTop = 8;
                T2c2.PaddingBottom = 8;

                Paragraph T2P = new Paragraph("FROM", titleFont);
                T2c2.AddElement(T2P);
                T2c2.BorderWidth = 0;
                T2c2.PaddingLeft = 50;

                // BaseColor b = new BaseColor(84,182,231);                
                T2c2.BackgroundColor = b;
                TAB2.AddCell(T2c2);

                PdfPTable TAB3 = new PdfPTable(2);
                TAB3.WidthPercentage = 100;
                float[] TAB3W = new float[] { 50f, 50f };

                TAB3.SetWidths(TAB3W);

                PdfPCell T3c1 = new PdfPCell();
                Paragraph T3P1 = new Paragraph();//"Taylor Dickens", Fn_Address);
                T3P1.Add(new Chunk(ds.Rows[0]["Customer_Name"].ToString(), regularFontBold));
                Paragraph T3P2 = new Paragraph(ds.Rows[0]["Customer_Address1"].ToString(), regularFont);
                Paragraph T3P3 = new Paragraph(ds.Rows[0]["Customer_Address2"].ToString(), regularFont);
                Paragraph T3P4 = new Paragraph(ds.Rows[0]["Customer_Address3"].ToString(), regularFont);
                Paragraph T3P5 = new Paragraph(ds.Rows[0]["GST_No"].ToString(), regularFont);
                Paragraph T3P6 = new Paragraph();

                T3P6.Add(new Chunk("" + ds.Rows[0]["Contact_Person"].ToString(), regularFontBold));
                Paragraph T3P7 = new Paragraph();//"Phone No: 0412 8765", Fn_Address);

                T3P7.Add(new Chunk("" + ds.Rows[0]["Contact_No"].ToString(), regularFont));
                Paragraph T3P8 = new Paragraph();
                T3P8.Add(new Chunk("Yort Ref : ", regularFontBold));
                T3P8.Add(new Chunk(" \n", regularFont));

                T3c1.AddElement(T3P1);
                T3c1.AddElement(T3P2);
                T3c1.AddElement(T3P3);
                T3c1.AddElement(T3P4);
                T3c1.AddElement(T3P5);
                T3c1.AddElement(T3P6);
                T3c1.AddElement(T3P7);
                T3c1.AddElement(T3P8);

                T3c1.PaddingLeft = 50;
                BaseColor b3 = new BaseColor(246, 247, 249);

                T3c1.BackgroundColor = b3;

                T3c1.BorderWidth = 0;
                TAB3.AddCell(T3c1);



                PdfPCell T3c2 = new PdfPCell();
                Paragraph T3P = new Paragraph("" + GITAPI.dbFunctions.CM_Name, regularFontBold);
                Paragraph T3CP1 = new Paragraph(GITAPI.dbFunctions.CM_Address1, regularFont);
                Paragraph T3CP2 = new Paragraph(GITAPI.dbFunctions.CM_Address2, regularFont);
                Paragraph T3CP3 = new Paragraph(GITAPI.dbFunctions.CM_Address3, regularFont);
                Paragraph T3CP4 = new Paragraph(GITAPI.dbFunctions.CM_GST_No, regularFont);
                Paragraph T3CP5 = new Paragraph();//"Phone No: 0412 8765", Fn_Address);
                T3CP5.Add(new Chunk(ds.Rows[0]["Contact_Person"].ToString(), regularFontBold));

                Paragraph T3CP6 = new Paragraph();//"Attn: Patrick", Fn_Address
                T3CP6.Add(new Chunk(ds.Rows[0]["Contact_No"].ToString(), regularFontBold));


                T3c2.AddElement(T3P);
                T3c2.AddElement(T3CP1);
                T3c2.AddElement(T3CP2);
                T3c2.AddElement(T3CP3);
                T3c2.AddElement(T3CP4);
                T3c2.AddElement(T3CP5);
                T3c2.AddElement(T3CP6);

                T3c2.PaddingLeft = 50;
                BaseColor b2 = new BaseColor(246, 247, 249);

                T3c2.BackgroundColor = b2;

                T3c2.BorderWidth = 0;


                TAB3.AddCell(T3c2);

                PdfPTable TAB4 = new PdfPTable(3);
                TAB4.WidthPercentage = 100;
                float[] TAB4W = new float[] { 30f, 40f, 30f };

                TAB4.SetWidths(TAB4W);

                PdfPCell T4c1 = new PdfPCell();


                Paragraph T4P1 = new Paragraph("DELIVERY MODE", titleFont);


                T4c1.AddElement(T4P1);

                T4c1.UseAscender = true;
                T4c1.PaddingTop = 8;
                T4c1.PaddingBottom = 8;

                T4c1.BackgroundColor = b;
                T4c1.BorderWidth = 0;
                TAB4.AddCell(T4c1);



                PdfPCell T4c2 = new PdfPCell();
                Paragraph T4P = new Paragraph("PAYMENT TERMS", titleFont);
                T4c2.AddElement(T4P);
                T4c2.BorderWidth = 0;
                T4c2.PaddingLeft = 10;
                T4c2.UseAscender = true;
                T4c2.PaddingTop = 8;
                T4c2.PaddingBottom = 8;


                T4c2.BackgroundColor = b;
                TAB4.AddCell(T4c2);

                PdfPCell T4c3 = new PdfPCell();
                Paragraph T4CP = new Paragraph("VALID FOR", titleFont);
                T4c3.AddElement(T4CP);
                T4c3.BorderWidth = 0;
                T4c3.PaddingLeft = 10;
                T4c3.UseAscender = true;
                T4c3.PaddingTop = 8;
                T4c3.PaddingBottom = 8;


                T4c3.BackgroundColor = b;
                TAB4.AddCell(T4c3);


                PdfPTable TAB5 = new PdfPTable(3);
                TAB5.WidthPercentage = 100;
                float[] TAB5W = new float[] { 30f, 40f, 30f };

                TAB5.SetWidths(TAB5W);

                PdfPCell T5c1 = new PdfPCell();
                Paragraph T5P1 = new Paragraph();//5/2020", titleFont);
                T5P1.Add(new Chunk("" + ds.Rows[0]["Delivery_Mode"].ToString(), regularFont));


                T5c1.AddElement(T5P1);

                BaseColor b1 = new BaseColor(246, 247, 249);

                T5c1.BackgroundColor = b1;
                T5c1.PaddingLeft = 10;
                T5c1.UseAscender = true;
                T5c1.PaddingTop = 8;
                T5c1.PaddingBottom = 8;
                T5c1.BorderWidth = 0;
                TAB5.AddCell(T5c1);



                PdfPCell T5c2 = new PdfPCell();
                Paragraph T5P = new Paragraph();//"Patrick Smith", titleFont);
                T5P.Add(new Chunk(ds.Rows[0]["Payment_Terms"].ToString(), regularFont));
                T5c2.BackgroundColor = b1;

                T5c2.AddElement(T5P);
                T5c2.BorderWidth = 0;
                T5c2.PaddingLeft = 10;
                T5c2.UseAscender = true;
                T5c2.PaddingTop = 8;
                T5c2.PaddingBottom = 8;
                TAB5.AddCell(T5c2);

                PdfPCell T5c3 = new PdfPCell();
                Paragraph T5CP = new Paragraph();//"Patrick Smith", titleFont);
                T5CP.Add(new Chunk(ds.Rows[0]["Valid_For"].ToString(), regularFont));

                T5c3.BackgroundColor = b1;

                T5c3.AddElement(T5CP);
                T5c3.BorderWidth = 0;
                T5c3.PaddingLeft = 10;
                T5c3.UseAscender = true;
                T5c3.PaddingTop = 8;
                T5c3.PaddingBottom = 8;
                TAB5.AddCell(T5c3);

                PdfPCell T5c4 = new PdfPCell();
                Paragraph T5C4P = new Paragraph();//" IT DEPORTMENT", titleFont);
                T5C4P.Add(new Chunk("IT DEPORTMENT", regularFont));

                T5c4.BackgroundColor = b1;

                T5c4.AddElement(T5C4P);
                T5c4.BorderWidth = 0;
                T5c4.PaddingLeft = 10;
                T5c4.UseAscender = true;
                T5c4.PaddingTop = 8;
                T5c4.PaddingBottom = 8;
                //TAB5.AddCell(T5c4);

                PdfPTable TAB6 = new PdfPTable(1);
                TAB6.WidthPercentage = 100;
                float[] TAB6W = new float[] { 100f };

                TAB6.SetWidths(TAB6W);

                PdfPCell T6c1 = new PdfPCell();
                Paragraph T6P1 = new Paragraph("TERMS :-", titleFont);


                T6c1.AddElement(T6P1);
                T6c1.BackgroundColor = b;

                T6c1.PaddingLeft = 10;
                T6c1.UseAscender = true;
                T6c1.PaddingTop = 8;
                T6c1.PaddingBottom = 8;
                T6c1.BorderWidth = 0;
                T6c1.BackgroundColor = b;

                TAB6.AddCell(T6c1);

                PdfPTable TAB7 = new PdfPTable(1);
                TAB7.WidthPercentage = 100;
                float[] TAB7W = new float[] { 100f };

                TAB7.SetWidths(TAB7W);

                PdfPCell T7c1 = new PdfPCell();
                Paragraph T7P1 = new Paragraph();
                T7P1.Add(new Chunk(ds.Rows[0]["Term"].ToString(), regularFont));


                T7c1.AddElement(T7P1);

                T7c1.BackgroundColor = b1;

                T7c1.PaddingLeft = 10;

                T7c1.UseAscender = true;
                T7c1.PaddingTop = 8;
                T7c1.PaddingBottom = 8;
                T7c1.BorderWidth = 0;
                TAB7.AddCell(T7c1);

                columns.AddElement(Htable);


                columns.AddElement(TAB2);


                columns.AddElement(TAB3);
                TAB3.SpacingAfter = 8;

                columns.AddElement(TAB4);

                columns.AddElement(TAB5);

                TAB6.SpacingBefore = 8;









                DataTable th = GITAPI.dbFunctions.getTable("select 'ID' as Field, '#' as Name, 'center' as Align, '5%' as Width ,0 as Order_No union all select Field,Name,Align,Width,order_No from Field_Setting" + Company + " where    Table_Name='Quotation_Details" + Company + "' order by order_No ");


                string W = "0";
                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Description"))
                    {
                        W = th.Rows[i]["Width"].ToString().Replace("%", "");
                        th.Rows[i].Delete();
                    }
                }

                th.AcceptChanges();

                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Item_Name"))
                    {
                        th.Rows[i]["Width"] = (decimal.Parse(th.Rows[i]["Width"].ToString().Replace("%", "")) + decimal.Parse(W)).ToString();

                    }
                }



                PdfPTable table = new PdfPTable(th.Rows.Count);
                table.WidthPercentage = 100;
                table.HeaderRows = 1;
                string[] headers = new string[th.Rows.Count];

                for (int k = 0; k < th.Rows.Count; k++)
                {
                    headers[k] = th.Rows[k]["Name"].ToString();



                    Paragraph tab_Header = new Paragraph(th.Rows[k]["Name"].ToString(), titleFont);

                    PdfPCell cell = new PdfPCell();



                    try
                    {
                        string Align = th.Rows[k]["Align"].ToString();

                        if (Align.ToLower().Equals("right"))
                        {
                            tab_Header.Alignment = Element.ALIGN_RIGHT;

                        }
                        else if (Align.ToLower().Equals("center"))
                        {
                            tab_Header.Alignment = Element.ALIGN_CENTER;
                        }

                    }
                    catch { }


                    cell.AddElement(tab_Header);

                    // cell.Width = this.Columns[k].Width;
                    // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.BackgroundColor = b;

                    //cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    cell.BorderColor = new iTextSharp.text.BaseColor(255, 255, 255);

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;
                    cell.PaddingTop = 0;
                    cell.PaddingBottom = 8;

                    cell.BorderWidthTop = 0;
                    cell.BorderWidthBottom = 1;


                    table.AddCell(cell);
                }


                float[] anchoDeColumnas = new float[th.Rows.Count];

                for (int i = 0; i < th.Rows.Count; i++)
                {
                    anchoDeColumnas[i] = float.Parse(th.Rows[i]["Width"].ToString().Replace("%", ""));

                }
                table.SetWidths(anchoDeColumnas);
                DataTable td = GITAPI.dbFunctions.getTable("select * from   Quotation_details" + Company + " where Quote_No='" + Quote_No + "'");


                //Add values of DataTable in pdf file
                for (int i = 0; i < td.Rows.Count; i++)
                {

                    for (int j = 0; j < th.Rows.Count; j++)
                    {

                        string colum = th.Rows[j]["Field"].ToString();
                        string data = td.Rows[i][colum].ToString();

                        PdfPCell cell = new PdfPCell();

                        if (colum == "ID")
                        {
                            Paragraph Item_Name = new Paragraph((i + 1).ToString(), regularFont);
                            cell.AddElement(Item_Name);

                        }
                        else if (colum == "Item_Name")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Item_Name"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["Description"].ToString(), small_Font);
                            Description.Alignment = Element.ALIGN_LEFT;

                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }
                        else if (colum == "Qty")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Qty"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["UOM"].ToString(), small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }

                        else if (colum == "SGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["SGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["SGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }

                        else if (colum == "CGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["CGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["CGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }
                        else
                        {
                            Paragraph P = new Paragraph(data, regularFont);
                            try
                            {
                                string Align = th.Rows[j]["Align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    P.Alignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    P.Alignment = Element.ALIGN_CENTER;
                                }

                            }
                            catch { }

                            cell.AddElement(P);
                        }


                        cell.BorderColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;

                        if (i % 2 == 0)
                        {
                            cell.BackgroundColor = b1;
                        }

                        cell.BorderWidthTop = 0;
                        cell.BorderWidthBottom = 0;
                        if (j == 0)
                        {
                            cell.BorderWidthLeft = 0;
                        }

                        //cell.BorderWidthRight = 1;
                        table.AddCell(cell);



                    }
                }


                table.SpacingBefore = 10;





                columns.AddElement(table);




                PdfPTable TAB9 = new PdfPTable(1);
                TAB9.WidthPercentage = 100;
                float[] TAB9W = new float[] { 100f };

                TAB9.SetWidths(TAB9W);

                PdfPCell T9c1 = new PdfPCell();
                Paragraph T9P1 = new Paragraph("₹ " + decimal.Parse(ds.Rows[0]["Net_Amt"].ToString()).ToString("##,##,###.00"), regularFontBold_Black);
                T9P1.Alignment = Element.ALIGN_RIGHT;
                T9c1.Border = 0;
                T9c1.AddElement(T9P1);

                TAB9.AddCell(T9c1);
                TAB9.SpacingAfter = 20;
                columns.AddElement(TAB9);





                columns.AddElement(TAB6);

                columns.AddElement(TAB7);





                document.Add(columns);
                GITAPI.dbFunctions.Company = Company;
                GITAPI.dbFunctions.Bill_No = Quote_No;
                // writer.PageEvent = new Report_Footer();


                document.Close();

                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");

            }
        }

        public FileResult Export_Invoice_2(string User, string Company, string File_Name, string File_Type, string Bill_No)
        {

            GITAPI.dbFunctions.username = User;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {

                JObject jsonData;
                Load_Comapny(Company);
                load_Page_Setting(Company);

                Document document = new Document(PageSize.A5, 10f, 10f, 10f, 10f);
                if (GITAPI.dbFunctions.PDF_Page_Size == "a4")
                {
                    document = new Document(PageSize.A4, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    }

                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a3")
                {
                    document = new Document(PageSize.A3, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A3.Rotate());
                    }
                }
                else if (GITAPI.dbFunctions.PDF_Page_Size == "a5")
                {
                    document = new Document(PageSize.A5, GITAPI.dbFunctions.PDF_left, GITAPI.dbFunctions.PDF_Right, GITAPI.dbFunctions.PDF_Top, GITAPI.dbFunctions.PDF_Bottom);

                    if (GITAPI.dbFunctions.PDF_Page_Orentation == "landscape")
                    {

                        document.SetPageSize(iTextSharp.text.PageSize.A5.Rotate());
                    }
                }



                PdfWriter writer = PdfWriter.GetInstance(document, stream);

                // writer.PageEvent = new Border();

                //writer.PageEvent = new PDFFooter();
                // PageEventHelper pageEventHelper = new PageEventHelper();
                // writer.PageEvent = pageEventHelper;
                document.Open();
                GITAPI.dbFunctions.PDF_Bottom = 10;



                FontFactory.RegisterDirectories();

                iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
                iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);
                iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
                iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
                iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
                iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
                iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
                iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);



                iTextSharp.text.Font subject = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2);
                subject.SetColor(105, 105, 105);
                small_Font.SetColor(105, 105, 105);
                int Color_R = 0;
                int Color_G = 0;
                int Color_B = 0;
                regularFontBold_white.SetColor(0, 0, 0);
                fn_Header.SetColor(Color_R, Color_G, Color_B);
                fn_Title.SetColor(Color_R, Color_G, Color_B);
                regularFontBold.SetColor(Color_R, Color_G, Color_B);


                float Rows = 0;
                float Rows_Lent = 35 + (10 - GITAPI.dbFunctions.PDF_Font_Size);

                string Address = "";

                if (GITAPI.dbFunctions.CM_Address1.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address1 + "\n";

                if (GITAPI.dbFunctions.CM_Address2.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address2 + "\n";

                if (GITAPI.dbFunctions.CM_Address3.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address3 + "\n";

                if (GITAPI.dbFunctions.CM_Address4.Trim() != "")
                    Address += GITAPI.dbFunctions.CM_Address4 + "\n";

                DataTable ds = GITAPI.dbFunctions.getTable("select * from Sales" + Company + " where Bill_No='" + Bill_No + "'");
                MultiColumnText columns = new MultiColumnText();
                columns.AddRegularColumns(GITAPI.dbFunctions.PDF_left, document.PageSize.Width - GITAPI.dbFunctions.PDF_Right, 100f, 1);


                Image jpg = Image.GetInstance(Server.MapPath("~/Image/Company/" + Company.Replace("_", "") + ".png"));
                jpg.ScaleToFit(GITAPI.dbFunctions.PDF_Logo_Height, GITAPI.dbFunctions.PDF_Logo_Width);

                //columns.AddElement(jpg);




                PdfPTable Htable = new PdfPTable(2);
                Htable.WidthPercentage = 100;
                float[] hs = new float[] { 75f, 25f };

                Htable.SetWidths(hs);

                PdfPCell c2 = new PdfPCell();

                string fontpath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\" + GITAPI.dbFunctions.PDF_Header_Font + ".TTF";
                BaseFont basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true);
                iTextSharp.text.Font fn_Title1 = new iTextSharp.text.Font(basefont, GITAPI.dbFunctions.PDF_Header_Font_Size, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLUE);

                //iTextSharp.text.Font fn_Title1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Header_Font, GITAPI.dbFunctions.PDF_Header_Font_Size , Font.BOLD);
                Paragraph p2 = new Paragraph(GITAPI.dbFunctions.CM_Name, fn_Title1);

                p2.Alignment = Element.ALIGN_LEFT;

                c2.AddElement(p2);
                c2.BorderWidth = 0;

                Paragraph pq1 = new Paragraph(GITAPI.dbFunctions.CM_Address1, regularFont);
                pq1.Alignment = Element.ALIGN_LEFT;

                Paragraph pqd = new Paragraph(GITAPI.dbFunctions.CM_Address2, regularFont);
                pqd.Alignment = Element.ALIGN_LEFT;
                Paragraph pqd1 = new Paragraph(GITAPI.dbFunctions.CM_Address3, regularFont);
                pqd1.Alignment = Element.ALIGN_LEFT;

                c2.AddElement(pq1);
                c2.AddElement(pqd);
                c2.AddElement(pqd1);

                c2.PaddingRight = 20;
                Htable.AddCell(c2);




                PdfPCell c1 = new PdfPCell();
                Paragraph p11 = new Paragraph("" + GITAPI.dbFunctions.CM_Name, fn_Title);
                Paragraph p12 = new Paragraph("" + Address, Fn_Address);
                c1.AddElement(jpg);
                jpg.Alignment = Element.ALIGN_RIGHT;


                //c1.AddElement(p11);
                //c1.AddElement(p12);
                c1.PaddingLeft = 20;
                c1.BorderWidth = 0;
                Htable.SpacingAfter = 10;
                Htable.AddCell(c1);




                PdfPTable TAB2 = new PdfPTable(2);
                TAB2.WidthPercentage = 100;
                float[] TAB2W = new float[] { 50f, 50f };

                TAB2.SetWidths(TAB2W);
                titleFont.SetColor(255, 255, 255);
                PdfPCell T2c1 = new PdfPCell();
                T2c1.UseAscender = true;
                T2c1.PaddingTop = 8;
                T2c1.PaddingBottom = 8;
                Paragraph T2P1 = new Paragraph("To", titleFont);


                T2c1.AddElement(T2P1);


                T2c1.PaddingLeft = 30;
                BaseColor b = new BaseColor(84, 182, 231);
                T2c1.BackgroundColor = b;
                T2c1.BorderWidth = 0;
                TAB2.AddCell(T2c1);



                PdfPCell T2c2 = new PdfPCell();
                T2c2.UseAscender = true;
                T2c2.PaddingTop = 8;
                T2c2.PaddingBottom = 8;

                Paragraph T2P = new Paragraph("FROM", titleFont);
                T2c2.AddElement(T2P);
                T2c2.BorderWidth = 0;
                T2c2.PaddingLeft = 50;

                // BaseColor b = new BaseColor(84,182,231);                
                T2c2.BackgroundColor = b;
                TAB2.AddCell(T2c2);

                PdfPTable TAB3 = new PdfPTable(2);
                TAB3.WidthPercentage = 100;
                float[] TAB3W = new float[] { 50f, 50f };

                TAB3.SetWidths(TAB3W);





                PdfPCell T3c2 = new PdfPCell();
                Paragraph T3P = new Paragraph(GITAPI.dbFunctions.CM_Name, regularFontBold);
                Paragraph T3CP1 = new Paragraph(GITAPI.dbFunctions.CM_Address1, regularFont);
                Paragraph T3CP2 = new Paragraph(GITAPI.dbFunctions.CM_Address2, regularFont);
                Paragraph T3CP3 = new Paragraph(GITAPI.dbFunctions.CM_Address3, regularFont);
                Paragraph T3CP4 = new Paragraph(GITAPI.dbFunctions.CM_GST_No, regularFont);



                T3c2.AddElement(T3P);
                T3c2.AddElement(T3CP1);
                T3c2.AddElement(T3CP2);
                T3c2.AddElement(T3CP3);
                T3c2.AddElement(T3CP4);

                T3c2.PaddingLeft = 50;
                BaseColor b2 = new BaseColor(246, 247, 249);

                T3c2.BackgroundColor = b2;

                T3c2.BorderWidth = 0;


                TAB3.AddCell(T3c2);

                PdfPTable TAB4 = new PdfPTable(3);
                TAB4.WidthPercentage = 100;
                float[] TAB4W = new float[] { 30f, 40f, 30f };

                TAB4.SetWidths(TAB4W);

                PdfPCell T4c1 = new PdfPCell();


                Paragraph T4P1 = new Paragraph("", titleFont);


                T4c1.AddElement(T4P1);

                T4c1.UseAscender = true;
                T4c1.PaddingTop = 2;
                T4c1.PaddingBottom = 2;

                T4c1.BackgroundColor = b;
                T4c1.BorderWidth = 0;
                TAB4.AddCell(T4c1);



                PdfPCell T4c2 = new PdfPCell();
                Paragraph T4P = new Paragraph("", titleFont);
                T4c2.AddElement(T4P);
                T4c2.BorderWidth = 0;
                T4c2.PaddingLeft = 10;
                T4c2.UseAscender = true;
                T4c2.PaddingTop = 2;
                T4c2.PaddingBottom = 2;


                T4c2.BackgroundColor = b;
                TAB4.AddCell(T4c2);

                PdfPCell T4c3 = new PdfPCell();
                Paragraph T4CP = new Paragraph("", titleFont);
                T4c3.AddElement(T4CP);
                T4c3.BorderWidth = 0;
                T4c3.PaddingLeft = 10;
                T4c3.UseAscender = true;
                T4c3.PaddingTop = 2;
                T4c3.PaddingBottom = 2;


                T4c3.BackgroundColor = b;
                TAB4.AddCell(T4c3);


                PdfPTable TAB5 = new PdfPTable(3);
                TAB5.WidthPercentage = 100;
                float[] TAB5W = new float[] { 40f, 40f, 20f };

                TAB5.SetWidths(TAB5W);

                PdfPCell T5c1 = new PdfPCell();
                Paragraph T5P1 = new Paragraph("Bill To:", regularFontBold);
                T5P1.SpacingAfter = 2;

                Paragraph T3P1 = new Paragraph(ds.Rows[0]["Customer_Name"].ToString(), regularFontBold);
                T3P1.SpacingAfter = 2;

                Paragraph T3P2 = new Paragraph(ds.Rows[0]["Customer_Address1"].ToString(), regularFont);
                Paragraph T3P3 = new Paragraph(ds.Rows[0]["Customer_Address2"].ToString(), regularFont);
                Paragraph T3P5 = new Paragraph();
                T3P5.Add(new Chunk("GST:", regularFont));
                T3P5.Add(new Chunk(ds.Rows[0]["GST_No"].ToString(), regularFont));
                T3P5.SpacingAfter = 2;

                Paragraph T3P7 = new Paragraph("Contact No:" + ds.Rows[0]["Customer_Address3"].ToString(), regularFont);
                T5c1.AddElement(T5P1);
                T5c1.AddElement(T3P1);
                T5c1.AddElement(T3P2);
                T5c1.AddElement(T3P3);
                T5c1.AddElement(T3P5);
                T5c1.AddElement(T3P7);
                T5c1.BorderColor = new iTextSharp.text.BaseColor(84, 182, 231);
                T5c1.BorderWidthRight = 5;

                T5c1.PaddingLeft = 50;
                BaseColor b3 = new BaseColor(246, 247, 249);

                //T5c1.BackgroundColor = b3;

                T5c1.BorderWidth = 0;




                BaseColor b1 = new BaseColor(246, 247, 249);

                // T5c1.BackgroundColor = b1;
                T5c1.PaddingLeft = 10;

                T5c1.UseAscender = true;
                T5c1.PaddingTop = 8;
                T5c1.PaddingBottom = 8;
                T5c1.BorderWidth = 0;

                TAB5.AddCell(T5c1);




                PdfPCell T5c2 = new PdfPCell();
                Paragraph T5b1 = new Paragraph();
                T5b1.Add(new Chunk(GITAPI.dbFunctions.CM_Bank_Name, regularFont));
                Paragraph T5b = new Paragraph("Acc No:" + GITAPI.dbFunctions.CM_Acc_Number, regularFont);
                Paragraph T5b2 = new Paragraph("IFSC:" + GITAPI.dbFunctions.CM_IFSC, regularFont);
                Paragraph T5B3 = new Paragraph(GITAPI.dbFunctions.CM_GST_No, regularFont);

                // T5c2.BackgroundColor = b1;

                T5c2.AddElement(T5b1);
                T5c2.AddElement(T5b);

                T5c2.AddElement(T5b2);
                T5c2.AddElement(T5B3);
                T5c2.BorderColor = new iTextSharp.text.BaseColor(84, 182, 231);
                T5c2.BorderWidthRight = 5;
                T5c2.BorderWidth = 0;
                T5c2.PaddingLeft = 10;
                T5c2.UseAscender = true;
                T5c2.PaddingTop = 20;
                T5c2.PaddingBottom = 8;
                TAB5.AddCell(T5c2);

                PdfPCell T5c3 = new PdfPCell();
                Paragraph T5CP = new Paragraph();
                T5CP.Add(new Chunk("Invoice No", regularFontBold));
                T5CP.Add(new Chunk(" : " + ds.Rows[0]["Bill_No"].ToString(), regularFontBold));
                T5CP.Alignment = Element.ALIGN_RIGHT;

                Paragraph T5CP1 = new Paragraph();
                T5CP1.Add(new Chunk("Date", regularFontBold));
                T5CP1.Add(new Chunk(" : " + DateTime.Parse(ds.Rows[0]["Bill_Date"].ToString()).ToString("dd/MM/yyyy"), regularFontBold));
                T5CP1.Alignment = Element.ALIGN_RIGHT;

                //T5c3.BackgroundColor = b1;

                T5c3.AddElement(T5CP);
                T5c3.AddElement(T5CP1);

                T5c3.BorderWidth = 0;
                T5c3.PaddingLeft = 10;
                T5c3.UseAscender = true;
                T5c3.PaddingTop = 8;
                T5c3.PaddingBottom = 8;
                TAB5.AddCell(T5c3);

                PdfPCell T5c4 = new PdfPCell();
                Paragraph T5C4P = new Paragraph();//" IT DEPORTMENT", titleFont);
                T5C4P.Add(new Chunk("IT DEPORTMENT", regularFont));

                T5c4.BackgroundColor = b1;

                T5c4.AddElement(T5C4P);
                T5c4.BorderWidth = 0;
                T5c4.PaddingLeft = 10;
                T5c4.UseAscender = true;
                T5c4.PaddingTop = 8;
                T5c4.PaddingBottom = 8;
                //TAB5.AddCell(T5c4);

                PdfPTable TAB6 = new PdfPTable(1);
                TAB6.WidthPercentage = 100;
                float[] TAB6W = new float[] { 100f };

                TAB6.SetWidths(TAB6W);

                PdfPCell T6c1 = new PdfPCell();
                Paragraph T6P1 = new Paragraph("TERMS :-", titleFont);


                T6c1.AddElement(T6P1);
                T6c1.BackgroundColor = b;

                T6c1.PaddingLeft = 10;
                T6c1.UseAscender = true;
                T6c1.PaddingTop = 8;
                T6c1.PaddingBottom = 8;
                T6c1.BorderWidth = 0;
                T6c1.BackgroundColor = b;

                TAB6.AddCell(T6c1);

                PdfPTable TAB7 = new PdfPTable(1);
                TAB7.WidthPercentage = 100;
                float[] TAB7W = new float[] { 100f };

                TAB7.SetWidths(TAB7W);

                PdfPCell T7c1 = new PdfPCell();
                Paragraph T7P1 = new Paragraph();
                T7P1.Add(new Chunk(GITAPI.dbFunctions.CM_Sales_Term, regularFont));


                T7c1.AddElement(T7P1);

                T7c1.BackgroundColor = b1;

                T7c1.PaddingLeft = 10;

                T7c1.UseAscender = true;
                T7c1.PaddingTop = 8;
                T7c1.PaddingBottom = 8;
                T7c1.BorderWidth = 0;
                TAB7.AddCell(T7c1);

                columns.AddElement(Htable);


                //columns.AddElement(TAB2);


                //columns.AddElement(TAB3);
                TAB3.SpacingAfter = 8;

                columns.AddElement(TAB4);

                columns.AddElement(TAB5);

                TAB6.SpacingBefore = 8;







                string a = "[{ \"Name\":\"#\" ,\"field\":\"ID\",\"align\":\"lef\",\"width\":\"10%\"}," +
                              " { \"Name\":\"Item Name\" ,\"field\":\"Item_Name\",\"align\":\"left\",\"width\":\"30%\"}," +
                             " { \"Name\":\"HSN\" ,\"field\":\"HSN_Code\",\"align\":\"left\",\"width\":\"10%\"}," +
                             " { \"Name\":\"Qty\" ,\"field\":\"Qty\",\"align\":\"right\",\"width\":\"15%\"}," +
                             " { \"Name\":\"Price\" ,\"field\":\"Unit_Price\",\"align\":\"right\",\"width\":\"15%\"}," +
                              " { \"Name\":\"Amount\" ,\"field\":\"Net_Amt\",\"align\":\"right\",\"width\":\"20%\"}]";


                Newtonsoft.Json.Linq.JArray Headers = Newtonsoft.Json.Linq.JArray.Parse(a);
                DataTable th = toDataTable(Headers);

                //DataTable th = GITAPI.dbFunctions.getTable("select 'ID' as Field, '#' as Name, 'center' as Align, '5%' as Width ,0 as order_No union all select Field,Name,Align,Width,order_No from Field_Setting" + Company + " where    Table_Name='Sales_Details" + Company + "' order by order_No ");


                string W = "0";
                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Description"))
                    {
                        W = th.Rows[i]["Width"].ToString().Replace("%", "");
                        th.Rows[i].Delete();
                    }
                }

                th.AcceptChanges();

                for (int i = th.Rows.Count - 1; i >= 0; i--)
                {
                    if (th.Rows[i]["Field"].ToString().Equals("Item_Name"))
                    {
                        th.Rows[i]["Width"] = (decimal.Parse(th.Rows[i]["Width"].ToString().Replace("%", "")) + decimal.Parse(W)).ToString();

                    }
                }



                PdfPTable table = new PdfPTable(th.Rows.Count);
                table.WidthPercentage = 100;
                table.HeaderRows = 1;
                string[] headers = new string[th.Rows.Count];

                for (int k = 0; k < th.Rows.Count; k++)
                {
                    headers[k] = th.Rows[k]["Name"].ToString();



                    Paragraph tab_Header = new Paragraph(th.Rows[k]["Name"].ToString(), titleFont);

                    PdfPCell cell = new PdfPCell();



                    try
                    {
                        string Align = th.Rows[k]["Align"].ToString();

                        if (Align.ToLower().Equals("right"))
                        {
                            tab_Header.Alignment = Element.ALIGN_RIGHT;

                        }
                        else if (Align.ToLower().Equals("center"))
                        {
                            tab_Header.Alignment = Element.ALIGN_CENTER;
                        }

                    }
                    catch { }


                    cell.AddElement(tab_Header);

                    // cell.Width = this.Columns[k].Width;
                    // cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    //cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.BackgroundColor = b;

                    //cell.BorderColor = new iTextSharp.text.BaseColor(169, 169, 169);
                    cell.BorderColor = new iTextSharp.text.BaseColor(255, 255, 255);

                    cell.PaddingLeft = 4;
                    cell.PaddingRight = 4;
                    cell.PaddingTop = 0;
                    cell.PaddingBottom = 8;

                    cell.BorderWidthTop = 0;
                    cell.BorderWidthBottom = 1;


                    table.AddCell(cell);
                }


                float[] anchoDeColumnas = new float[th.Rows.Count];

                for (int i = 0; i < th.Rows.Count; i++)
                {
                    anchoDeColumnas[i] = float.Parse(th.Rows[i]["Width"].ToString().Replace("%", ""));

                }
                table.SetWidths(anchoDeColumnas);
                DataTable td = GITAPI.dbFunctions.getTable("select x.*,y.Item_TamilName from   Sales_details" + Company + " x  Left outer join  item_master" + Company + " y on x.item_ID =y.ID where Bill_No='" + Bill_No + "'");


                //Add values of DataTable in pdf file
                for (int i = 0; i < td.Rows.Count; i++)
                {

                    for (int j = 0; j < th.Rows.Count; j++)
                    {

                        string colum = th.Rows[j]["Field"].ToString();
                        string data = td.Rows[i][colum].ToString();

                        PdfPCell cell = new PdfPCell();

                        if (colum == "ID")
                        {
                            Paragraph Item_Name = new Paragraph((i + 1).ToString(), regularFont);
                            cell.AddElement(Item_Name);

                        }
                        else if (colum == "Item_Name")
                        {

                            iTextSharp.text.Font regularFont1 = new iTextSharp.text.Font(basefont, GITAPI.dbFunctions.PDF_Font_Size - 1);

                            //iTextSharp.text.Font regularFont1 = FontFactory.GetFont("ARIALUNI", GITAPI.dbFunctions.PDF_Font_Size - 1);
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Item_TamilName"].ToString(), regularFont1);
                            Paragraph Description = new Paragraph(td.Rows[i]["Description"].ToString(), small_Font);
                            Description.Alignment = Element.ALIGN_LEFT;

                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }
                        else if (colum == "Qty")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["Qty"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["UOM"].ToString(), small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }

                        else if (colum == "SGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["SGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["SGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);

                        }

                        else if (colum == "CGST_Amt")
                        {
                            Paragraph Item_Name = new Paragraph(td.Rows[i]["CGST_Amt"].ToString(), regularFont);
                            Paragraph Description = new Paragraph(td.Rows[i]["CGST_per"].ToString() + "%", small_Font);
                            Item_Name.Alignment = Element.ALIGN_RIGHT;
                            Description.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(Item_Name);
                            cell.AddElement(Description);
                        }
                        else
                        {
                            Paragraph P = new Paragraph(data, regularFont);
                            try
                            {
                                string Align = th.Rows[j]["Align"].ToString();

                                if (Align.ToLower().Equals("right"))
                                {
                                    P.Alignment = Element.ALIGN_RIGHT;

                                }
                                else if (Align.ToLower().Equals("center"))
                                {

                                    P.Alignment = Element.ALIGN_CENTER;
                                }

                            }
                            catch { }

                            cell.AddElement(P);
                        }


                        cell.BorderColor = new iTextSharp.text.BaseColor(255, 255, 255);
                        cell.PaddingLeft = 4;
                        cell.PaddingRight = 4;

                        if (i % 2 == 0)
                        {
                            cell.BackgroundColor = b1;
                        }

                        cell.BorderWidthTop = 0;
                        cell.BorderWidthBottom = 0;
                        if (j == 0)
                        {
                            cell.BorderWidthLeft = 0;
                        }

                        //cell.BorderWidthRight = 1;
                        table.AddCell(cell);



                    }
                }


                table.SpacingBefore = 10;





                columns.AddElement(table);




                PdfPTable TAB9 = new PdfPTable(3);
                TAB9.WidthPercentage = 100;
                float[] TAB9W = new float[] { 50f, 30f, 20f };
                TAB9.SetWidths(TAB9W);
                PdfPCell T9c11 = new PdfPCell();
                Paragraph T9P2 = new Paragraph("Total", regularFontBold_Black);
                T9P2.Alignment = Element.ALIGN_LEFT;
                T9c11.PaddingTop = 8;
                T9c11.PaddingLeft = 20;

                T9c11.PaddingBottom = 0;

                T9P2.SpacingAfter = 5;
                T9P2.SpacingBefore = 5;

                T9c11.UseAscender = true;



                T9c11.BorderWidthRight = 0;
                T9c11.BorderWidthLeft = 0;
                T9c11.BorderWidthTop = 1;
                T9c11.BorderWidthBottom = 1;

                PdfPCell T9c12 = new PdfPCell();
                Paragraph T9P3 = new Paragraph("", regularFontBold_Black);
                T9P3.Alignment = Element.ALIGN_RIGHT;
                T9c12.PaddingTop = 8;

                T9P3.SpacingAfter = 5;
                T9P3.SpacingBefore = 5;

                T9c12.UseAscender = true;

                T9c12.BorderWidthRight = 0;
                T9c12.BorderWidthLeft = 0;
                T9c12.BorderWidthTop = 1;
                T9c12.BorderWidthBottom = 1;

                PdfPCell T9c1 = new PdfPCell();
                Paragraph T9P1 = new Paragraph("₹ " + decimal.Parse(ds.Rows[0]["Net_Amt"].ToString()).ToString("##,##,###.00"), regularFontBold_Black);
                T9P1.Alignment = Element.ALIGN_RIGHT;
                T9c1.PaddingTop = 8;

                T9P3.SpacingAfter = 5;
                T9P3.SpacingBefore = 5;

                T9c1.UseAscender = true;

                T9c1.Border = 0;
                T9c1.BorderWidthRight = 0;
                T9c1.BorderWidthLeft = 0;
                T9c1.BorderWidthTop = 1;
                T9c1.BorderWidthBottom = 1;

                T9c11.AddElement(T9P2);
                T9c12.AddElement(T9P3);
                T9c1.AddElement(T9P1);

                TAB9.AddCell(T9c11);
                TAB9.AddCell(T9c12);
                TAB9.AddCell(T9c1);

                //Paragraph w1 = new Paragraph("Amount in Words", Fn_Address);
                //s1.AddElement(w1);

                //long l = long.Parse(decimal.Parse(td.Rows[0]["Net_Amt"].ToString()).ToString("0"));

                //string word = GITAPI.dbFunctions.ConvertNumbertoWords(l);
                PdfPTable TABl = new PdfPTable(2);
                TABl.WidthPercentage = 100;
                float[] TABlW = new float[] { 50f, 50f };
                TABl.SetWidths(TABlW);
                PdfPCell T10c = new PdfPCell();
                Paragraph T10P = new Paragraph("INVOICE AMOUNT IN WORDS", regularFontBold);
                T10P.Alignment = Element.ALIGN_LEFT;
                T10P.SpacingAfter = 3;
                T10P.SpacingBefore = 5;

                T10c.PaddingLeft = 10;
                T10c.UseAscender = true;
                T10c.PaddingTop = 8;
                T10c.PaddingBottom = 8;
                T10c.BorderWidth = 0;
                //தனலக்ஷ்மி டிசைனர் 

                long l = long.Parse(decimal.Parse(ds.Rows[0]["Net_Amt"].ToString()).ToString("0"));

                string word = GITAPI.dbFunctions.ConvertNumbertoWords(l);
                Paragraph w2 = new Paragraph(word, Fn_Address);
                w2.Alignment = Element.ALIGN_LEFT;
                w2.SpacingAfter = 3;
                w2.SpacingBefore = 5;

                Paragraph T10PC2 = new Paragraph("TERMS AND CONDITIONS", regularFontBold);
                T10PC2.Alignment = Element.ALIGN_LEFT;
                T10PC2.SpacingAfter = 3;
                T10PC2.SpacingBefore = 5;
                Paragraph T10P3 = new Paragraph(GITAPI.dbFunctions.CM_Sales_Term, regularFont);
                T10P3.Alignment = Element.ALIGN_LEFT;
                T10P3.SpacingAfter = 3;
                T10P3.SpacingBefore = 5;
                T10c.AddElement(T10P);
                T10c.AddElement(w2);
                T10c.AddElement(T10PC2);
                T10c.AddElement(T10P3);


                PdfPCell T10c1 = new PdfPCell();
                PdfPTable TAB11 = new PdfPTable(2);
                T10c1.UseAscender = true;
                T10c1.Border = 0;
                T10c1.BorderWidthRight = 0;
                T10c1.BorderWidthLeft = 0;
                T10c1.BorderWidthTop = 0;
                T10c1.BorderWidthBottom = 0;
                float[] TAB11W = new float[] { 50f, 50f };
                TAB11.SetWidths(TAB11W);
                PdfPCell T11c0P = new PdfPCell();
                Paragraph tbr = new Paragraph("Sub Total", regularFont);
                tbr.Alignment = Element.ALIGN_LEFT;
                T11c0P.PaddingTop = 10;

                tbr.SpacingAfter = 5;
                T11c0P.Border = 0;

                T11c0P.UseAscender = true;
                T11c0P.Border = 0;
                T11c0P.BorderWidthRight = 0;
                T11c0P.BorderWidthLeft = 0;
                T11c0P.BorderWidthTop = 0;
                T11c0P.BorderWidthBottom = 1;

                Paragraph T10PC1 = new Paragraph("Total", regularFont);
                T10PC1.Alignment = Element.ALIGN_LEFT;
                T10PC1.SpacingAfter = 5;

                Paragraph tbr1 = new Paragraph("Received", regularFont);
                tbr1.Alignment = Element.ALIGN_LEFT;
                tbr1.SpacingAfter = 5;

                Paragraph T10PC13 = new Paragraph("Balance", regularFont);
                T10PC13.Alignment = Element.ALIGN_LEFT;
                T10PC13.SpacingAfter = 5;

                T11c0P.AddElement(tbr);
                T11c0P.AddElement(T10PC1);
                T11c0P.AddElement(tbr1);
                T11c0P.AddElement(T10PC13);


                PdfPCell T12c1 = new PdfPCell();

                Paragraph T10PC11 = new Paragraph(ds.Rows[0]["Net_Amt"].ToString(), regularFont);
                T10PC11.Alignment = Element.ALIGN_RIGHT;
                T10PC11.SpacingAfter = 5;

                Paragraph T10PC12 = new Paragraph(ds.Rows[0]["Net_Amt"].ToString(), regularFont);
                T10PC12.Alignment = Element.ALIGN_RIGHT;
                T10PC12.SpacingAfter = 5;

                Paragraph T1PC13 = new Paragraph(ds.Rows[0]["Net_Amt"].ToString(), regularFont);
                T1PC13.Alignment = Element.ALIGN_RIGHT;
                T1PC13.SpacingAfter = 5;

                Paragraph T10PC14 = new Paragraph(ds.Rows[0]["Net_Amt"].ToString(), regularFont);
                T10PC14.Alignment = Element.ALIGN_RIGHT;
                T10PC14.SpacingAfter = 5;
                TAB11.SpacingAfter = 20;

                Paragraph T10PC15 = new Paragraph("For" + GITAPI.dbFunctions.CM_Name, regularFont);
                T10PC15.Alignment = Element.ALIGN_LEFT;
                T10PC15.SpacingAfter = 5;

                T12c1.UseAscender = true;
                T12c1.PaddingTop = 10;

                T12c1.Border = 0;
                T12c1.BorderWidthRight = 0;
                T12c1.BorderWidthLeft = 0;
                T12c1.BorderWidthTop = 0;
                T12c1.BorderWidthBottom = 1;
                T12c1.AddElement(T10PC11);
                T12c1.AddElement(T10PC12);
                T12c1.AddElement(T1PC13);
                T12c1.AddElement(T10PC14);
                TAB11.AddCell(T11c0P);
                TAB11.AddCell(T12c1);

                T10c1.AddElement(TAB11);
                T10c1.AddElement(T10PC15);

                TABl.AddCell(T10c);

                TABl.AddCell(T10c1);







                TAB9.SpacingBefore = 5;

                columns.AddElement(TAB9);
                TAB9.SpacingAfter = 5;

                columns.AddElement(TABl);


                //Paragraph w1 = new Paragraph("Amount in Words", Fn_Address);
                //s1.AddElement(w1);

                //long l = long.Parse(decimal.Parse(td.Rows[0]["Net_Amt"].ToString()).ToString("0"));

                //string word = GITAPI.dbFunctions.ConvertNumbertoWords(l);

                document.Add(columns);
                GITAPI.dbFunctions.Company = Company;
                GITAPI.dbFunctions.Bill_No = Bill_No;
                //writer.PageEvent = new Report_Footer();


                document.Close();

                return File(stream.ToArray(), "application/pdf", File_Name + ".pdf");

            }
        }



    }




    public partial class Purchse_Report_Footer : PdfPageEventHelper
    {


        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Tax Invoice"));
            //cell.Border = Rectangle.NO_BORDER;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Top, writer.DirectContent);

            // Footer_Table.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Bottom + 150 + sum_Height, writer.DirectContent);

        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
            iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);

            iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
            iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
            iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
            iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
            iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
            iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
            iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
            iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);

            iTextSharp.text.Font fn_underLine = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK);
            iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.NORMAL, BaseColor.BLACK);

            base.OnEndPage(writer, document);
            PdfPTable Footer_Table = new PdfPTable(2);
            Footer_Table.WidthPercentage = 100;
            var pageBorder = new Rectangle(document.PageSize);
            Footer_Table.TotalWidth = pageBorder.Width - GITAPI.dbFunctions.PDF_left - GITAPI.dbFunctions.PDF_Right;

            int line_height = 25;
            PdfPTable Sum_Table = new PdfPTable(3);
            float[] G = new float[] { 49f, 2f, 49f };

            Sum_Table.SetWidths(G);
            Sum_Table.DefaultCell.Border = 0;
            int sum_Height = 0;
            string Space = "   ";
            DataTable td = GITAPI.dbFunctions.getTable("select * from   Purchase_Order" + GITAPI.dbFunctions.Company + " where PO_No='" + GITAPI.dbFunctions.Bill_No + "'");

            if (decimal.Parse(td.Rows[0]["Sub_Total"].ToString()) != decimal.Parse(td.Rows[0]["Net_Amt"].ToString()))
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Sub Total", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.BorderWidth = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Sub_Total"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }



            if (decimal.Parse(td.Rows[0]["Taxable_Amount"].ToString()) != decimal.Parse(td.Rows[0]["Sub_Total"].ToString()))
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Taxable ", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Taxable_Amount"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }



            if (decimal.Parse(td.Rows[0]["IGST_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("IGST(" + td.Rows[0]["IGST_Per"].ToString() + ")", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["IGST_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }




            if (decimal.Parse(td.Rows[0]["SGST_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("SGST(" + td.Rows[0]["SGST_Amt"].ToString() + "%)", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["SGST_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }




            if (decimal.Parse(td.Rows[0]["CGST_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("CGST(" + td.Rows[0]["CGST_Amt"].ToString() + "%)", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["CGST_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.BorderWidthBottom = 1;
                cs3.PaddingRight = 5;
                Sum_Table.AddCell(cs3);

                sum_Height += line_height;
            }


            if (decimal.Parse(td.Rows[0]["Round_off"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Round off :", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);
                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Round_off"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);

                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }

            if (decimal.Parse(td.Rows[0]["Net_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Net Amount:", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);
                cs2.Border = 0;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Net_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.PaddingRight = 5;
                cs3.Border = 0;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }

            Sum_Table.DefaultCell.Padding = 0;

            float[] h = new float[] { 60f, 40f };
            Footer_Table.SetWidths(h);

            Footer_Table.DefaultCell.Padding = 0;

            PdfPCell s1 = new PdfPCell();

            Paragraph w1 = new Paragraph("Amount in Words", Fn_Address);
            s1.AddElement(w1);

            long l = long.Parse(decimal.Parse(td.Rows[0]["Net_Amt"].ToString()).ToString("0"));

            string word = GITAPI.dbFunctions.ConvertNumbertoWords(l);
            Paragraph w2 = new Paragraph(word, Fn_Address);
            s1.AddElement(w2);
            s1.BorderWidth = 0;
            s1.BorderWidthTop = 1;
            s1.BorderWidthRight = 1;
            Footer_Table.AddCell(s1);



            PdfPCell s2 = new PdfPCell();
            s2.BorderWidth = 0;
            Footer_Table.AddCell(Sum_Table);



            PdfPCell fc1 = new PdfPCell();

            Paragraph ft1 = new Paragraph("Declaration", fn_underLine);


            fc1.AddElement(ft1);

            string Tersm = "" + GITAPI.dbFunctions.CM_Sales_Term;
            Paragraph ft2 = new Paragraph(Tersm, small_Font);
            fc1.AddElement(ft2);
            fc1.Border = 0;


            PdfPCell fc2 = new PdfPCell();

            fc2.FixedHeight = 75;

            Paragraph ft3 = new Paragraph("Company Bank Details", fn_underLine);
            fc2.AddElement(ft3);

            Tersm = "    Bank : " + GITAPI.dbFunctions.CM_Bank_Name + " \n    A/C No : " + GITAPI.dbFunctions.CM_Acc_Number + "\n    IFSC :" + GITAPI.dbFunctions.CM_IFSC + "\n    Branch : " + GITAPI.dbFunctions.CM_Branch;
            Paragraph ft4 = new Paragraph(Tersm, small_Font);

            fc2.AddElement(ft4);
            fc2.Border = 0;



            PdfPCell bc2 = new PdfPCell();

            Paragraph bt3 = new Paragraph("Received by", Fn_Address);

            bt3.Alignment = Element.ALIGN_CENTER;
            bc2.AddElement(bt3);
            bc2.Border = 0;



            PdfPCell bc1 = new PdfPCell();

            Paragraph bt11 = new Paragraph("For " + GITAPI.dbFunctions.CM_Name, Fn_Address);
            bt11.Alignment = Element.ALIGN_CENTER;
            bt11.SpacingAfter = 30;
            bc1.FixedHeight = 75;


            Paragraph bt12 = new Paragraph("Authorised Signature", Fn_Address);
            bt12.Alignment = Element.ALIGN_CENTER;

            bc1.AddElement(bt11);
            bc1.AddElement(bt12);

            bc1.Border = 0;
            bc2.Border = 0;



            fc1.BorderWidthTop = 1;
            fc1.BorderWidthBottom = 1;
            fc1.BorderWidthRight = 1;
            fc2.BorderWidthBottom = 1;
            fc2.BorderWidthTop = 1;
            bc1.BorderWidthBottom = 1;
            bc2.BorderWidthRight = 1;
            bc2.BorderWidthBottom = 1;


            fc1.Padding = 5;
            fc2.Padding = 5;
            bc1.Padding = 5;
            bc2.Padding = 5;


            Footer_Table.AddCell(fc1);

            Footer_Table.AddCell(fc2);

            Footer_Table.AddCell(bc2);

            Footer_Table.AddCell(bc1);


            Footer_Table.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Bottom + 150 + sum_Height, writer.DirectContent);


        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }

    }




    public partial class PDF_Report_Footer : PdfPageEventHelper
    {


        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Tax Invoice"));
            //cell.Border = Rectangle.NO_BORDER;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Top, writer.DirectContent);

            // Footer_Table.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Bottom + 150 + sum_Height, writer.DirectContent);

        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
            iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);

            iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
            iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
            iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
            iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
            iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
            iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
            iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
            iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);

            iTextSharp.text.Font fn_underLine = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK);
            iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.NORMAL, BaseColor.BLACK);

            base.OnEndPage(writer, document);
            PdfPTable Footer_Table = new PdfPTable(2);
            Footer_Table.WidthPercentage = 100;
            var pageBorder = new Rectangle(document.PageSize);
            Footer_Table.TotalWidth = pageBorder.Width - GITAPI.dbFunctions.PDF_left - GITAPI.dbFunctions.PDF_Right;


            float[] h = new float[] { 20f, 80f };
            Footer_Table.SetWidths(h);

            Footer_Table.DefaultCell.Padding = 0;

            PdfPCell s1 = new PdfPCell();
            Paragraph w1 = new Paragraph(GITAPI.dbFunctions.Total_Row, Fn_Address);
            s1.AddElement(w1);


            PdfPCell s2 = new PdfPCell();
            Paragraph w2 = new Paragraph(GITAPI.dbFunctions.Total_Amount, Fn_Address);
            w2.Alignment = Element.ALIGN_RIGHT;
            s2.AddElement(w2);
            s1.FixedHeight = 25;
            s1.BackgroundColor = BaseColor.LIGHT_GRAY;
            s2.BackgroundColor = BaseColor.LIGHT_GRAY;

            Footer_Table.AddCell(s1);

            Footer_Table.AddCell(s2);


            Footer_Table.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Bottom + 40, writer.DirectContent);


        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }

    }




    public partial class Profroma_Report_Footer : PdfPageEventHelper
    {


        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Tax Invoice"));
            //cell.Border = Rectangle.NO_BORDER;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Top, writer.DirectContent);

            // Footer_Table.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Bottom + 150 + sum_Height, writer.DirectContent);

        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
            iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);

            iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
            iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
            iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
            iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
            iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
            iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
            iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
            iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);

            iTextSharp.text.Font fn_underLine = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK);
            iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.NORMAL, BaseColor.BLACK);

            base.OnEndPage(writer, document);
            PdfPTable Footer_Table = new PdfPTable(2);
            Footer_Table.WidthPercentage = 100;
            var pageBorder = new Rectangle(document.PageSize);
            Footer_Table.TotalWidth = pageBorder.Width - GITAPI.dbFunctions.PDF_left - GITAPI.dbFunctions.PDF_Right;

            int line_height = 25;
            PdfPTable Sum_Table = new PdfPTable(3);
            float[] G = new float[] { 49f, 2f, 49f };

            Sum_Table.SetWidths(G);
            Sum_Table.DefaultCell.Border = 0;
            int sum_Height = 0;
            string Space = "   ";
            DataTable td = GITAPI.dbFunctions.getTable("select * from   p_invoice" + GITAPI.dbFunctions.Company + " where Bill_No='" + GITAPI.dbFunctions.Bill_No + "'");

            if (decimal.Parse(td.Rows[0]["Sub_Total"].ToString()) != decimal.Parse(td.Rows[0]["Net_Amt"].ToString()))
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Sub Total", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.BorderWidth = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Sub_Total"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }



            if (decimal.Parse(td.Rows[0]["Taxable_Amount"].ToString()) != decimal.Parse(td.Rows[0]["Sub_Total"].ToString()))
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Taxable ", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Taxable_Amount"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }



            if (decimal.Parse(td.Rows[0]["IGST_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("IGST(" + td.Rows[0]["IGST_Per"].ToString() + "%)", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["IGST_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }




            if (decimal.Parse(td.Rows[0]["SGST_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("SGST(" + td.Rows[0]["SGST_Per"].ToString() + "%)", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["SGST_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }




            if (decimal.Parse(td.Rows[0]["CGST_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("CGST(" + td.Rows[0]["CGST_Per"].ToString() + "%)", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["CGST_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.BorderWidthBottom = 1;
                cs3.PaddingRight = 5;
                Sum_Table.AddCell(cs3);

                sum_Height += line_height;
            }


            if (decimal.Parse(td.Rows[0]["Round_off"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Round off :", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);
                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Round_off"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);

                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }

            if (decimal.Parse(td.Rows[0]["Net_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Net Amount ", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);
                cs2.Border = 0;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Net_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.PaddingRight = 5;
                cs3.Border = 0;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }

            Sum_Table.DefaultCell.Padding = 0;

            float[] h = new float[] { 60f, 40f };
            Footer_Table.SetWidths(h);

            Footer_Table.DefaultCell.Padding = 0;

            PdfPCell s1 = new PdfPCell();

            Paragraph w1 = new Paragraph("Amount in Words", Fn_Address);
            s1.AddElement(w1);

            long l = long.Parse(decimal.Parse(td.Rows[0]["Net_Amt"].ToString()).ToString("0"));

            string word = GITAPI.dbFunctions.ConvertNumbertoWords(l);
            Paragraph w2 = new Paragraph(word, Fn_Address);
            s1.AddElement(w2);
            s1.BorderWidth = 0;
            s1.BorderWidthTop = 1;
            s1.BorderWidthRight = 1;
            Footer_Table.AddCell(s1);



            PdfPCell s2 = new PdfPCell();
            s2.BorderWidth = 0;
            Footer_Table.AddCell(Sum_Table);



            PdfPCell fc1 = new PdfPCell();

            fc1.UseAscender = true;
            fc1.PaddingTop = 5;
            Paragraph ft1 = new Paragraph("Declaration", fn_underLine);


            fc1.AddElement(ft1);

            string Tersm = "" + GITAPI.dbFunctions.CM_Sales_Term;
            Paragraph ft2 = new Paragraph(Tersm, small_Font);
            fc1.AddElement(ft2);
            fc1.Border = 0;


            PdfPCell fc2 = new PdfPCell();


            fc2.UseAscender = true;
            fc2.PaddingTop = 5;

            fc2.FixedHeight = 75;

            Paragraph ft3 = new Paragraph("Company Bank Details", fn_underLine);
            fc2.AddElement(ft3);

            Tersm = "    Bank : " + GITAPI.dbFunctions.CM_Bank_Name + " \n    A/C No : " + GITAPI.dbFunctions.CM_Acc_Number + "\n    IFSC :" + GITAPI.dbFunctions.CM_IFSC + "\n    Branch : " + GITAPI.dbFunctions.CM_Branch;
            Paragraph ft4 = new Paragraph(Tersm, small_Font);

            fc2.AddElement(ft4);
            fc2.Border = 0;



            PdfPCell bc2 = new PdfPCell();

            Paragraph bt3 = new Paragraph("Received by", Fn_Address);

            bt3.Alignment = Element.ALIGN_CENTER;
            bc2.AddElement(bt3);
            bc2.Border = 0;



            PdfPCell bc1 = new PdfPCell();

            Paragraph bt11 = new Paragraph("For " + GITAPI.dbFunctions.CM_Name, Fn_Address);
            bt11.Alignment = Element.ALIGN_CENTER;
            bt11.SpacingAfter = 30;
            bc1.FixedHeight = 75;


            Paragraph bt12 = new Paragraph("Authorised Signature", Fn_Address);
            bt12.Alignment = Element.ALIGN_CENTER;

            bc1.AddElement(bt11);
            bc1.AddElement(bt12);

            bc1.Border = 0;
            bc2.Border = 0;



            fc1.BorderWidthTop = 1;
            fc1.BorderWidthBottom = 1;
            fc1.BorderWidthRight = 1;
            fc2.BorderWidthBottom = 1;
            fc2.BorderWidthTop = 1;
            bc1.BorderWidthBottom = 1;
            bc2.BorderWidthRight = 1;
            bc2.BorderWidthBottom = 1;


            fc1.Padding = 5;
            fc2.Padding = 5;
            bc1.Padding = 5;
            bc2.Padding = 5;


            Footer_Table.AddCell(fc1);

            Footer_Table.AddCell(fc2);

            Footer_Table.AddCell(bc2);

            Footer_Table.AddCell(bc1);


            Footer_Table.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Bottom + 150 + sum_Height, writer.DirectContent);


        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }

    }


    public partial class Report_Footer : PdfPageEventHelper
    {


        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase("Tax Invoice"));
            //cell.Border = Rectangle.NO_BORDER;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Top, writer.DirectContent);

            // Footer_Table.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Bottom + 150 + sum_Height, writer.DirectContent);

        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            iTextSharp.text.Font fn_Header = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 5, Font.BOLD);
            iTextSharp.text.Font fn_Title = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size + 3, Font.BOLD);

            iTextSharp.text.Font font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
            iTextSharp.text.Font titleFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size, Font.BOLD);
            iTextSharp.text.Font regularFont = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1);
            iTextSharp.text.Font regularFontBold = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
            iTextSharp.text.Font small_Font = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 4);
            iTextSharp.text.Font small_Font1 = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 3);
            iTextSharp.text.Font regularFontBold_Black = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);
            iTextSharp.text.Font regularFontBold_white = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.BOLD);

            iTextSharp.text.Font fn_underLine = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 2, Font.BOLD | Font.UNDERLINE, BaseColor.BLACK);
            iTextSharp.text.Font Fn_Address = FontFactory.GetFont(GITAPI.dbFunctions.PDF_Font_Family, GITAPI.dbFunctions.PDF_Font_Size - 1, Font.NORMAL, BaseColor.BLACK);

            base.OnEndPage(writer, document);
            PdfPTable Footer_Table = new PdfPTable(2);
            Footer_Table.WidthPercentage = 100;
            var pageBorder = new Rectangle(document.PageSize);
            Footer_Table.TotalWidth = pageBorder.Width - GITAPI.dbFunctions.PDF_left - GITAPI.dbFunctions.PDF_Right;

            int line_height = 25;
            PdfPTable Sum_Table = new PdfPTable(3);
            float[] G = new float[] { 49f, 2f, 49f };

            Sum_Table.SetWidths(G);
            Sum_Table.DefaultCell.Border = 0;
            int sum_Height = 0;
            string Space = "   ";
            DataTable td = GITAPI.dbFunctions.getTable("select * from   sales" + GITAPI.dbFunctions.Company + " where Bill_No='" + GITAPI.dbFunctions.Bill_No + "'");

            if (decimal.Parse(td.Rows[0]["Sub_Total"].ToString()) != decimal.Parse(td.Rows[0]["Net_Amt"].ToString()))
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Sub Total", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.BorderWidth = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Sub_Total"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }



            if (decimal.Parse(td.Rows[0]["Taxable_Amount"].ToString()) != decimal.Parse(td.Rows[0]["Sub_Total"].ToString()))
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Taxable ", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Taxable_Amount"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }



            if (decimal.Parse(td.Rows[0]["IGST_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("IGST(" + td.Rows[0]["IGST_Per"].ToString() + "%)", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["IGST_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }




            if (decimal.Parse(td.Rows[0]["SGST_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("SGST(" + td.Rows[0]["SGST_Per"].ToString() + "%)", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["SGST_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }




            if (decimal.Parse(td.Rows[0]["CGST_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("CGST(" + td.Rows[0]["CGST_Per"].ToString() + "%)", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);

                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["CGST_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.Border = 0;
                cs3.BorderWidthBottom = 1;
                cs3.PaddingRight = 5;
                Sum_Table.AddCell(cs3);

                sum_Height += line_height;
            }


            if (decimal.Parse(td.Rows[0]["Round_off"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Round off :", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                cs1.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);
                cs2.Border = 0;
                cs2.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Round_off"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);

                cs3.Border = 0;
                cs3.PaddingRight = 5;
                cs3.BorderWidthBottom = 1;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }

            if (decimal.Parse(td.Rows[0]["Net_Amt"].ToString()) > 0)
            {
                PdfPCell cs1 = new PdfPCell();
                cs1.FixedHeight = line_height;
                Paragraph l1 = new Paragraph("Net Amount ", Fn_Address);
                l1.Alignment = Element.ALIGN_TOP;
                cs1.AddElement(l1);

                cs1.Border = 0;
                Sum_Table.AddCell(cs1);

                PdfPCell cs2 = new PdfPCell();
                cs2.FixedHeight = line_height;
                cs2.Padding = 0;
                Paragraph l2 = new Paragraph(":", Fn_Address);
                cs2.AddElement(l2);
                cs2.Border = 0;
                Sum_Table.AddCell(cs2);

                PdfPCell cs3 = new PdfPCell();
                cs3.FixedHeight = line_height;
                cs3.Padding = 0;
                Paragraph l3 = new Paragraph("" + td.Rows[0]["Net_Amt"].ToString() + Space, regularFontBold);
                l3.Alignment = Element.ALIGN_RIGHT;
                cs3.AddElement(l3);
                cs3.PaddingRight = 5;
                cs3.Border = 0;
                Sum_Table.AddCell(cs3);
                sum_Height += line_height;
            }

            Sum_Table.DefaultCell.Padding = 0;

            float[] h = new float[] { 60f, 40f };
            Footer_Table.SetWidths(h);

            Footer_Table.DefaultCell.Padding = 0;

            PdfPCell s1 = new PdfPCell();

            Paragraph w1 = new Paragraph("Amount in Words", Fn_Address);
            s1.AddElement(w1);

            long l = long.Parse(decimal.Parse(td.Rows[0]["Net_Amt"].ToString()).ToString("0"));

            string word = GITAPI.dbFunctions.ConvertNumbertoWords(l);
            Paragraph w2 = new Paragraph(word, Fn_Address);
            s1.AddElement(w2);
            s1.BorderWidth = 0;
            s1.BorderWidthTop = 1;
            s1.BorderWidthRight = 1;
            Footer_Table.AddCell(s1);



            PdfPCell s2 = new PdfPCell();
            s2.BorderWidth = 0;
            Footer_Table.AddCell(Sum_Table);



            PdfPCell fc1 = new PdfPCell();

            fc1.UseAscender = true;
            fc1.PaddingTop = 5;
            Paragraph ft1 = new Paragraph("Declaration", fn_underLine);


            fc1.AddElement(ft1);

            string Tersm = "" + GITAPI.dbFunctions.CM_Sales_Term;
            Paragraph ft2 = new Paragraph(Tersm, small_Font);
            fc1.AddElement(ft2);
            fc1.Border = 0;


            PdfPCell fc2 = new PdfPCell();


            fc2.UseAscender = true;
            fc2.PaddingTop = 5;

            fc2.FixedHeight = 75;

            Paragraph ft3 = new Paragraph("Company Bank Details", fn_underLine);
            fc2.AddElement(ft3);

            Tersm = "    Bank : " + GITAPI.dbFunctions.CM_Bank_Name + " \n    A/C No : " + GITAPI.dbFunctions.CM_Acc_Number + "\n    IFSC :" + GITAPI.dbFunctions.CM_IFSC + "\n    Branch : " + GITAPI.dbFunctions.CM_Branch;
            Paragraph ft4 = new Paragraph(Tersm, small_Font);

            fc2.AddElement(ft4);
            fc2.Border = 0;



            PdfPCell bc2 = new PdfPCell();

            Paragraph bt3 = new Paragraph("Received by", Fn_Address);

            bt3.Alignment = Element.ALIGN_CENTER;
            bc2.AddElement(bt3);
            bc2.Border = 0;



            PdfPCell bc1 = new PdfPCell();

            Paragraph bt11 = new Paragraph("For " + GITAPI.dbFunctions.CM_Name, Fn_Address);
            bt11.Alignment = Element.ALIGN_CENTER;
            bt11.SpacingAfter = 30;
            bc1.FixedHeight = 75;

            Image jpg = Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/Image/Company/" + GITAPI.dbFunctions.Company.Replace("_", "") + ".png"));
            jpg.ScaleToFit(50f, 30f);


            Paragraph bt12 = new Paragraph("Authorised Signature", Fn_Address);
            bt12.Alignment = Element.ALIGN_CENTER;

            bc1.AddElement(bt11);
            // bc1.AddElement(jpg);
            bc1.AddElement(bt12);

            bc1.Border = 0;
            bc2.Border = 0;



            fc1.BorderWidthTop = 1;
            fc1.BorderWidthBottom = 1;
            fc1.BorderWidthRight = 1;
            fc2.BorderWidthBottom = 1;
            fc2.BorderWidthTop = 1;
            bc1.BorderWidthBottom = 1;
            bc2.BorderWidthRight = 1;
            bc2.BorderWidthBottom = 1;


            fc1.Padding = 5;
            fc2.Padding = 5;
            bc1.Padding = 5;
            bc2.Padding = 5;


            Footer_Table.AddCell(fc1);

            Footer_Table.AddCell(fc2);

            Footer_Table.AddCell(bc2);

            Footer_Table.AddCell(bc1);


            Footer_Table.WriteSelectedRows(0, -1, GITAPI.dbFunctions.PDF_left, document.Bottom + 150 + sum_Height, writer.DirectContent);


        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }

    }

    public class PDFFooter : PdfPageEventHelper
    {
        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 10F;
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            cell = new PdfPCell(new Phrase(""));
            cell.Border = Rectangle.NO_BORDER;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            DateTime horario = DateTime.Now;
            base.OnEndPage(writer, document);
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            PdfPCell cell;
            tabFot.TotalWidth = 300F;
            iTextSharp.text.Font regularFont = FontFactory.GetFont("verdana", 9);
            cell = new PdfPCell(new Phrase("Provider Signature", regularFont));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, 150, document.Bottom + 20, writer.DirectContent);

        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    }

    public partial class Border : PdfPageEventHelper
    {

        public override void OnEndPage(PdfWriter writer, Document doc)
        {

            var content = writer.DirectContent;
            var pageBorder = new Rectangle(doc.PageSize);

            pageBorder.Left += doc.LeftMargin;
            pageBorder.Right -= doc.RightMargin;
            pageBorder.Top -= doc.TopMargin;
            pageBorder.Bottom += doc.BottomMargin;

            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(pageBorder.Left, pageBorder.Bottom, pageBorder.Width, pageBorder.Height);
            content.Stroke();
        }

    }


    public partial class Border1 : PdfPageEventHelper
    {

        public override void OnEndPage(PdfWriter writer, Document doc)
        {

            var content = writer.DirectContent;
            var pageBorder = new Rectangle(doc.PageSize);

            pageBorder.Left += doc.LeftMargin;
            pageBorder.Right -= doc.RightMargin;
            pageBorder.Top -= doc.TopMargin;
            pageBorder.Bottom += doc.BottomMargin + 15;

            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(pageBorder.Left, pageBorder.Bottom, pageBorder.Width, pageBorder.Height);
            content.Stroke();
        }

    }
}
