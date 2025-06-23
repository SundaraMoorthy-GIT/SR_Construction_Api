using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using TaxProEInvoice.API;
using TaxProEWB.API;


namespace Genuine_API.Controllers
{
    public class EInvoiceController : ApiController
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
        public string DateFormat(string Date)
        {
            string str = Date;
            string format = "dd/MM/yyyy hh:mm:ss tt"; // Format of the input string

            DateTime dateTime = DateTime.ParseExact(str, format, null);
            string formattedDate = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            return formattedDate.ToString();
        }



        public EWBSession EwbSession = new EWBSession();
        bool isProduction = true; // set this based on your config or environment


        public void load_EwbSession(string Company)
        {
            try
            {
                DataTable dd = GITAPI.dbFunctions.getTable("select * from Company_Master where CM_ID='" + Company.Replace("_", "") + "'");
                EwbSession.LoadAPISettingsFromConfigFile = true;
                EwbSession.LoadAPILoginDetailsFromConfigFile = true;
                EwbSession.EwbApiSetting.AspUserId = dd.Rows[0]["CM_AspUserId"].ToString();
                EwbSession.EwbApiSetting.AspPassword = dd.Rows[0]["CM_AspPassword"].ToString();
                EwbSession.EwbApiSetting.EWBClientId = dd.Rows[0]["CM_ClientId"].ToString();
                EwbSession.EwbApiSetting.EWBClientSecret = dd.Rows[0]["CM_ClientSecret"].ToString();
                EwbSession.EwbApiSetting.EWBGSPUserID = dd.Rows[0]["CM_GSPUserID"].ToString();
                if (isProduction)
                {
                    // Use production values from DataTable
                    EwbSession.EwbApiSetting.GSPName = dd.Rows[0]["CM_GSPName"].ToString();
                    EwbSession.EwbApiSetting.AuthUrl = "https://einvapi.charteredinfo.com/v1.03/dec/auth";
                    EwbSession.EwbApiSetting.BaseUrl = dd.Rows[0]["CM_BaseUrl"].ToString();
                    EwbSession.EwbApiSetting.AspUrl = null;
                    EwbSession.EwbApiLoginDetails.EwbGstin = dd.Rows[0]["CM_Gstin"].ToString();
                    EwbSession.EwbApiLoginDetails.EwbUserID = dd.Rows[0]["CM_UserID"].ToString();
                    EwbSession.EwbApiLoginDetails.EwbPassword = dd.Rows[0]["CM_Password"].ToString();
                }
                else
                {
                    // Use sandbox values for testing or development
                    EwbSession.EwbApiSetting.GSPName = "TaxPro_Sandbox"; // Sandbox GSP Name
                    EwbSession.EwbApiSetting.BaseUrl = "http://gstsandbox.charteredinfo.com/ewaybillapi/v1.03"; // Sandbox Base URL
                    EwbSession.EwbApiSetting.AspUrl = null; // Sandbox ASP URL
                    EwbSession.EwbApiLoginDetails.EwbGstin = "34AACCC1596Q002"; // Sandbox GSTIN
                    EwbSession.EwbApiLoginDetails.EwbUserID = "TaxProEnvPON"; // Sandbox User ID
                    EwbSession.EwbApiLoginDetails.EwbPassword = "abc34*"; // Sandbox Password
                }
                EwbSession.EwbApiLoginDetails.EwbAppKey = dd.Rows[0]["CM_AppKey"].ToString();
                EwbSession.EwbApiLoginDetails.EwbAuthToken = dd.Rows[0]["CM_AuthToken"].ToString();
                EwbSession.EwbApiLoginDetails.EwbSEK = dd.Rows[0]["CM_SEK"].ToString();
                EwbSession.EwbApiLoginDetails.EwbTokenExp = DateTime.Parse(dd.Rows[0]["CM_TokenExp"].ToString());

            }
            catch { }
        }

        public void Insert_EwbSession(JObject jsonData, string Company)
        {
            dynamic json = jsonData;

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = @"UPDATE company_Master
                                SET CM_GSPName = @CM_GSPName,
                                    CM_AspUserId = @CM_AspUserId,
                                    CM_AspPassword = @CM_AspPassword,
                                    CM_ClientId = @CM_ClientId,
                                    CM_ClientSecret = @CM_ClientSecret,
                                    CM_GSPUserID = @CM_GSPUserID,
                                    CM_BaseUrl = @CM_BaseUrl,
                                    CM_AspUrl = @CM_AspUrl,
                                    CM_Gstin = @CM_Gstin,
                                    CM_UserID = @CM_UserID,
                                    CM_Password = @CM_Password,
                                    CM_AppKey = @CM_AppKey,
                                    CM_AuthToken = @CM_AuthToken,
                                    CM_TokenExp = @CM_TokenExp,
                                    CM_SEK = @CM_SEK
                                WHERE CM_ID = @CM_ID";
                com.Parameters.Add("@CM_GSPName", SqlDbType.VarChar).Value = json.EwbApiSetting.GSPName;
                com.Parameters.Add("@CM_AspUserId", SqlDbType.VarChar).Value = json.EwbApiSetting.AspUserId;
                com.Parameters.Add("@CM_AspPassword", SqlDbType.VarChar).Value = json.EwbApiSetting.AspPassword;
                com.Parameters.Add("@CM_ClientId", SqlDbType.VarChar).Value = json.EwbApiSetting.EWBClientId;
                com.Parameters.Add("@CM_ClientSecret", SqlDbType.VarChar).Value = json.EwbApiSetting.EWBClientSecret;
                com.Parameters.Add("@CM_GSPUserID", SqlDbType.VarChar).Value = json.EwbApiSetting.EWBGSPUserID;
                com.Parameters.Add("@CM_BaseUrl", SqlDbType.VarChar).Value = json.EwbApiSetting.BaseUrl;
                com.Parameters.Add("@CM_AspUrl", SqlDbType.VarChar).Value = json.EwbApiSetting.AspUrl;
                com.Parameters.Add("@CM_Gstin", SqlDbType.VarChar).Value = json.EwbApiLoginDetails.EwbGstin;
                com.Parameters.Add("@CM_UserID", SqlDbType.VarChar).Value = json.EwbApiLoginDetails.EwbUserID;
                com.Parameters.Add("@CM_Password", SqlDbType.VarChar).Value = json.EwbApiLoginDetails.EwbPassword;
                com.Parameters.Add("@CM_AppKey", SqlDbType.VarChar).Value = json.EwbApiLoginDetails.EwbAppKey;
                com.Parameters.Add("@CM_AuthToken", SqlDbType.VarChar).Value = json.EwbApiLoginDetails.EwbAuthToken;
                com.Parameters.Add("@CM_TokenExp", SqlDbType.VarChar).Value = json.EwbApiLoginDetails.EwbTokenExp;
                com.Parameters.Add("@CM_SEK", SqlDbType.VarChar).Value = json.EwbApiLoginDetails.EwbSEK;
                com.Parameters.Add("@CM_ID", SqlDbType.VarChar).Value = Company.Replace("_", "");

                com.ExecuteNonQuery();
            }
            catch { }
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetAuthToken(string Company)
        {
            try
            {

                load_EwbSession(Company);

                try
                {
                    DateTime tokenExp = EwbSession.EwbApiLoginDetails.EwbTokenExp.Value;
                    if (tokenExp > DateTime.Now)
                    {
                        string validUntil = tokenExp.ToString("dd-MMM-yy HH:mm:ss");
                        return Ok("Validity of previous Auth Token is not expired. It will expire on : " + validUntil);
                    }
                }
                catch { }

                //Shared.SaveAPILoginDetails(EwbSession.EwbApiLoginDetails);
                TxnRespWithObjAndInfo<EWBSession> txnResp = await EWBAPI.GetAuthTokenAsync(EwbSession);
                string jsonString = JsonConvert.SerializeObject(EwbSession);

                // Parse JSON string to JObject
                JObject jObject = JObject.Parse(jsonString);
                Insert_EwbSession(jObject, Company);
                return Ok(txnResp); // Return the successful response
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // Return 500 status code for internal errors
            }
        }


        [HttpGet]
        public async Task<IHttpActionResult> Get_GSTNDetails(string GSTIN, string Company)
        {
            load_EwbSession(Company);
            string rtbResponse;

            try
            {
                var response = await EWBAPI.GetGSTNDetailAsync(EwbSession, GSTIN);

                if (response.IsSuccess)
                {
                    rtbResponse = JsonConvert.SerializeObject(response.RespObj);
                }
                else
                {
                    rtbResponse = JsonConvert.SerializeObject(response.TxnOutcome); ;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                rtbResponse = $"Error: {ex.Message}";
            }
            // Return both ewbGen and rtbResponse
            return Ok(new { Data = GSTIN, Response = rtbResponse });
        }


        [HttpGet]
        public async Task<IHttpActionResult> Gen_DC_EWB(string DC_No, string Company)
        {


            load_EwbSession(Company);
            string ewbGen = CreateEwbGen(DC_No, Company);

            string rtbResponse;
            var ewbGen2 = JsonConvert.DeserializeObject<dynamic>(ewbGen);

            try
            {
                var response = await EWBAPI.GenEWBAsync(EwbSession, ewbGen);

                if (response.IsSuccess)
                {
                    rtbResponse = JsonConvert.SerializeObject(response.RespObj);
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(rtbResponse);
                    string ewayBillDate = DateFormat(json.ewayBillDate.ToString());
                    string validUpto = DateFormat(json.validUpto.ToString());
                    string q = "update   Delivery_Challan set dc_ewaybill='" + json.ewayBillNo + "',dc_ewaybilldate='" + ewayBillDate
                    + "',dc_vaildupto='" + validUpto
                    + "',dc_ewayremarks='" + json.alert + "' where dc_no='" + DC_No + "'";
                    DataTable dp = GITAPI.dbFunctions.getTable(q);
                }
                else
                {
                    rtbResponse = await HandleErrorResponse(response, ewbGen);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                rtbResponse = $"Error: {ex.Message}";
            }
            // Return both ewbGen and rtbResponse
            return Ok(new { Data = ewbGen2, Response = rtbResponse });
        }



        [HttpGet]
        public async Task<IHttpActionResult> Get_DC_CancelEWB(string ewayBillNo, string cancelrsncode, string cancelrmrk, string User, string Company)
        {


            load_EwbSession(Company);
            string ewbGen = "{" +
              " \"ewbNo\": \"" + ewayBillNo + "\",  " +
              " \"cancelRsnCode\": \"" + cancelrsncode + "\",  " +
              " \"cancelRmrk\": \"" + cancelrmrk + "\"}  ";
            string rtbResponse;

            try
            {
                var response = await EWBAPI.CancelEWBAsync(EwbSession, ewbGen);

                if (response.IsSuccess)
                {
                    rtbResponse = JsonConvert.SerializeObject(response.RespObj);
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(rtbResponse);
                    string ewayBillDate = DateFormat(json.cancelDate.ToString());
                    string q = "update   Delivery_Challan set dc_canceldate='" + ewayBillDate + "', " +
                        " dc_cancelby='" + User + "', dc_cancelrsncode='" + cancelrsncode + "', " +
                        " dc_cancelrmrk='" + cancelrmrk + "'  where dc_ewaybill='" + ewayBillNo + "'";
                    DataTable dp = GITAPI.dbFunctions.getTable(q);
                }
                else
                {
                    rtbResponse = JsonConvert.SerializeObject(response.TxnOutcome); ;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                rtbResponse = $"Error: {ex.Message}";
            }
            // Return both ewbGen and rtbResponse
            return Ok(new { Data = ewbGen, Response = rtbResponse });
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetEwayBillsByDate(string Date, string Company)
        {
            load_EwbSession(Company);
            string rtbResponse;

            try
            {
                var response = await EWBAPI.GetEwayBillsByDateAsync(EwbSession, Date);

                if (response.IsSuccess)
                {
                    rtbResponse = JsonConvert.SerializeObject(response.RespObj);
                }
                else
                {
                    rtbResponse = JsonConvert.SerializeObject(response.TxnOutcome); ;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                rtbResponse = $"Error: {ex.Message}";
            }
            // Return both ewbGen and rtbResponse
            return Ok(new { Data = Date, Response = rtbResponse });
        }



        [HttpGet]
        public async Task<IHttpActionResult> Get_DC_UpdtVehicleNo(string ewayBillNo, string Company)
        {


            load_EwbSession(Company);
            string rtbResponse;

            ReqVehicleNoUpdtPl reqVehicleNo = new ReqVehicleNoUpdtPl();
            reqVehicleNo.ewbNo = long.Parse(ewayBillNo);
            reqVehicleNo.vehicleNo = "PVC9999"; /*PVC1239*/
            reqVehicleNo.fromPlace = "FRAZER TOWN";
            reqVehicleNo.fromState = 05;
            reqVehicleNo.reasonCode = "1";
            reqVehicleNo.reasonRem = "vehicle broke down";
            reqVehicleNo.transDocNo = "LR180321";//LR180321
            reqVehicleNo.transDocDate = "28/06/2018";
            reqVehicleNo.transMode = "2";
            reqVehicleNo.vehicleType = "R";

            try
            {
                var response = await EWBAPI.UpdateVehicleNosync(EwbSession, reqVehicleNo);

                if (response.IsSuccess)
                {
                    rtbResponse = JsonConvert.SerializeObject(response.RespObj);
                    //dynamic json = JsonConvert.DeserializeObject<dynamic>(rtbResponse);
                    //string ewayBillDate = DateTime.Parse(json.cancelDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                    //string q = "update   Delivery_Challan set dc_canceldate='" + ewayBillDate + "', " +
                    //    " dc_cancelby='" + User + "', dc_cancelrsncode='" + cancelrsncode + "', " +
                    //    " dc_cancelrmrk='" + cancelrmrk + "'  where dc_ewaybill='" + ewayBillNo + "'";
                    //DataTable dp = GITAPI.dbFunctions.getTable(q);
                }
                else
                {
                    rtbResponse = JsonConvert.SerializeObject(response.TxnOutcome); ;
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                rtbResponse = $"Error: {ex.Message}";
            }
            // Return both ewbGen and rtbResponse
            return Ok(new { Data = reqVehicleNo, Response = rtbResponse });
        }

        private string CreateEwbGen(string DC_No, string Company)
        {

            DataTable dt = GITAPI.dbFunctions.getTable("select * from Delivery_Challan left outer join Company_Master on CM_Id=dc_company where dc_no='" + DC_No + "' and dc_company='" + Company.Replace("_", "") + "' ");

            string dd = DateTime.Parse(dt.Rows[0]["dc_date"].ToString()).ToString("dd/MM/yyyy").Replace("-", "/");


            string Data = "";
            Data += "{" +
              " \"supplyType\": \"O\",  " +
              " \"subSupplyType\": \"5\",  " +
              " \"subSupplyDesc\": \"\",  " +
              " \"docType\": \"CHL\",  " +
              " \"docNo\": \"" + dt.Rows[0]["dc_bill_no"].ToString() + "\",  " +
              " \"docDate\": \"" + dd.ToString() + "\",  " +
              " \"fromGstin\": \"" + dt.Rows[0]["CM_Gstin"].ToString() + "\",  " +
              " \"fromTrdName\": \"" + dt.Rows[0]["CM_Name"].ToString() + "\",  " +
              " \"fromAddr1\": \"" + dt.Rows[0]["dc_dis_address1"].ToString() + "\",  " +
              " \"fromAddr2\": \"" + dt.Rows[0]["dc_dis_address2"].ToString() + "\",  " +
              " \"fromPlace\": \"" + dt.Rows[0]["dc_dis_city"].ToString() + "\",  " +
              " \"fromPincode\": \"" + dt.Rows[0]["dc_dis_pincode"].ToString() + "\",  " +
              " \"fromStateCode\": \"" + dt.Rows[0]["CM_State_Code"].ToString() + "\",  " +
              " \"actFromStateCode\": \"" + dt.Rows[0]["CM_State_Code"].ToString() + "\",  " +
              " \"toGstin\": \"" + dt.Rows[0]["dc_gstin"].ToString() + "\",  " +
              " \"toTrdName\": \"" + dt.Rows[0]["dc_ledger_name"].ToString() + "\",  " +
              " \"toAddr1\": \"" + dt.Rows[0]["dc_address1"].ToString() + "\",  " +
              " \"toAddr2\": \"" + dt.Rows[0]["dc_address2"].ToString() + "\",  " +
              " \"toPlace\": \"" + dt.Rows[0]["dc_city"].ToString() + "\",  " +
              " \"toPincode\": \"" + dt.Rows[0]["dc_pincode"].ToString() + "\",  " +
              " \"actToStateCode\": \"" + dt.Rows[0]["dc_scode"].ToString() + "\",  " +
              " \"toStateCode\": \"" + dt.Rows[0]["dc_scode"].ToString() + "\",  " +
              " \"transactionType\": \"4\",  " +
              " \"dispatchFromGSTIN\": \"" + dt.Rows[0]["dc_dis_gstin"].ToString() + "\",  " +
              " \"shipToGSTIN\": \"" + dt.Rows[0]["dc_ship_gstin"].ToString() + "\",  " +
              " \"shipToTradeName\": \"" + dt.Rows[0]["dc_ship_ledger_name"].ToString() + "\",  " +
              " \"otherValue\": \"0\",  " +
              " \"totalValue\": \"" + dt.Rows[0]["dc_sub_total"].ToString() + "\",  " +
              " \"cgstValue\": \"" + double.Parse(dt.Rows[0]["dc_cgst_amt"].ToString()) + "\",  " +
              " \"sgstValue\": \"" + double.Parse(dt.Rows[0]["dc_sgst_amt"].ToString()) + "\",  " +
              " \"igstValue\": \"" + double.Parse(dt.Rows[0]["dc_igst_amt"].ToString()) + "\",  " +
              " \"cessValue\": \"0\",  " +
              " \"cessNonAdvolValue\": \"0\",  " +
              " \"totInvValue\": \"0\",  " +
              " \"transMode\": \"1\",  " +
              " \"transDistance\": \"" + dt.Rows[0]["dc_distance"].ToString() + "\",  " +
              " \"transporterName\": \"" + dt.Rows[0]["dc_transname"].ToString() + "\",  " +
              " \"transporterId\": \"\",  " +
              " \"transDocNo\": \"\",  " +
              " \"transDocDate\": \"\",  " +
              " \"vehicleNo\": \"" + dt.Rows[0]["dc_vehicleno"].ToString() + "\",  " +
              " \"vehicleType\": \"R\",  " +
              " \"itemList\": [  ";
            DataTable dtitems;
            dtitems = GITAPI.dbFunctions.getTable("select dbo.get_ref_value(dc_uom) as dc_uom,* from Delivery_Challan_Details where  dc_no='" + DC_No + "' and dc_company='" + Company.Replace("_", "") + "' ");
            for (int i = 0; i < dtitems.Rows.Count; i++)
            {
                Data += "  {  " +
                    "   \"productName\": \"" + dtitems.Rows[i]["dc_prod_name"].ToString() + "\",  " +
                    "   \"productDesc\": \"" + dtitems.Rows[i]["dc_prod_name"].ToString() + "\",  " +
                    "   \"hsnCode\": \"" + dtitems.Rows[i]["dc_hsn_code"].ToString() + "\",  " +
                    "   \"quantity\": \"" + dtitems.Rows[i]["dc_qty"].ToString() + "\",  " +
                    "   \"qtyUnit\": \"" + dtitems.Rows[i]["dc_uom"].ToString() + "\",  " +
                    "   \"taxableAmount\": \"" + double.Parse(dtitems.Rows[i]["dc_taxable_amount"].ToString()) + "\",  " +
                    "   \"sgstRate\": \"" + double.Parse(dtitems.Rows[i]["dc_sgst_per"].ToString()) + "\",  " +
                    "   \"cgstRate\": \"" + double.Parse(dtitems.Rows[i]["dc_cgst_per"].ToString()) + "\",  " +
                    "   \"igstRate\": \"" + double.Parse(dtitems.Rows[i]["dc_igst_per"].ToString()) + "\",  " +
                    "   \"cessRate\": \"0\",  " +
                    "   \"cessNonAdvol\": \"0\" " +
                    " }  ";
                if (i < dtitems.Rows.Count - 1)
                {
                    Data += ",";
                }
            }
            Data += "] }";
            return Data;
        }

        private async Task<string> HandleErrorResponse(TxnRespWithObjAndInfo<RespGenEwbPl> response, string ewbGen)
        {
            if (response.TxnOutcome.Contains("702") && !string.IsNullOrEmpty(response.Info))
            {
                try
                {
                    var respInfo = JsonConvert.DeserializeObject<TaxProEWB.API.RespInfoPl>(response.Info);
                    //ewbGen.transDistance = respInfo.distance;

                    var retryResponse = await EWBAPI.GenEWBAsync(EwbSession, ewbGen);
                    return retryResponse.IsSuccess
                        ? JsonConvert.SerializeObject(retryResponse.RespObj)
                        : retryResponse.TxnOutcome;
                }
                catch (JsonSerializationException ex)
                {
                    // Log the exception
                    return $"Deserialization error: {ex.Message}";
                }
                catch (Exception ex)
                {
                    // Log the exception
                    return $"Error: {ex.Message}";
                }
            }

            return response.TxnOutcome;
        }

        [HttpGet]
        public string get_DC_No(string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='Delivery_Challan' ";

            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(dc_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Delivery_Challan ";
            Query += "where left(dc_no,len(@Prefix))=@Prefix ";
            Query += "and right(dc_no,len(@suffix))=@suffix ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }


        [HttpGet]
        public string get_DC_Bill_No(string Company)
        {
            string Query = "";
            Query += "declare @digit int ";
            Query += "declare @Prefix varchar(33) ";
            Query += "declare @suffix varchar(33) ";
            Query += "select  @digit=Digits, @Prefix=Prefix,@suffix=suffix from Seraial_No_Settings where [Name]='DC_No' ";

            Query += "select @Prefix+right('00000'+cast((isnull( max(cast(replace(replace(dc_bill_no,@Prefix,''),@suffix,'') as int)),0)+1) as varchar(33)),@digit) +@suffix as Number from  Delivery_Challan ";
            Query += "where left(dc_bill_no,len(@Prefix))=@Prefix ";
            Query += "and right(dc_bill_no,len(@suffix))=@suffix ";

            DataTable dt = GITAPI.dbFunctions.getTable(Query);

            return dt.Rows[0][0].ToString();
        }

        [HttpGet]
        public string delete_Delivery_Challan(string PO_No, string UserName, string Company)
        {
            DataTable dd2 = GITAPI.dbFunctions.getTable("delete  from Delivery_Challan where dc_no='" + PO_No + "'");
            DataTable dd3 = GITAPI.dbFunctions.getTable("delete  from Delivery_Challan_details where dc_no='" + PO_No + "'");
            return "True";
        }

        [HttpGet]
        public string get_Delivery_Challan_Item(string ID, string From, string To, string order_by, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,x.dc_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.dc_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            if (ID.ToLower() != "all")
            {
                condi += " and  x.dc_no='" + ID + "'";
            }


            DataTable dt = GITAPI.dbFunctions.getTable("select  dbo.Date_(dc_date) as dc_date, dbo.Date_(dc_bill_date) as dc_bill_date,* from Delivery_Challan_details x   where 0=0 " + condi + " order by x." + order_by);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }


        [HttpGet]
        public string get_Delivery_Challan_details(string PO_No, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select dbo.date_(dc_date) as dc_date, *, dbo.date_(dc_date) as Date from Delivery_Challan_details where dc_no='" + PO_No + "'  order by  dc_id ");
            string data = GITAPI.dbFunctions.GetJSONString(dt);
            return data;
        }

        [HttpGet]
        public string get_Delivery_Challan_Details(string From, string To, string User, string Type, string order_by, string Company)
        {

            string condi = "";
            condi += " and convert(Varchar,x.dc_bill_date,112)>='" + DateTime.Parse(From).ToString("yyyyMMdd") + "' and convert(Varchar,x.dc_bill_date,112)<='" + DateTime.Parse(To).ToString("yyyyMMdd") + "'";

            //if (User.ToLower() != "all")
            //{
            //    condi += " and  x.dc_created_by='" + User + "'";
            //}

            //if (Type.ToLower() != "all")
            //{
            //    condi += " and  x.po_type='" + Type + "'";
            //}


            DataTable dt = GITAPI.dbFunctions.getTable("select  dbo.Date_(dc_date) as dc_date, dbo.Date_(dc_bill_date) as dc_bill_date,*,dbo.Date_(dc_date) as Purchase_Date_ from Delivery_Challan x   where 0=0 " + condi + " order by x." + order_by);
            string data = GITAPI.dbFunctions.GetJSONString_(dt);
            return data;
        }

        [HttpPost]
        public string Post_Delivery_Challan(JObject jsonData)
        {
            dynamic json = jsonData;
            string ID = isnull(json.dc_id, "0");
            string Company = isnull(json.Company, "");
            string Created_by = isnull(json.Created_by, "");
            string Ledger_ID = isnull(json.dc_ledger_id, "");
            string Purchse_No = isnull(json.dc_no, "");
            string Purchse_Type = isnull(json.dc_type, "Delivery_Challan");
            string Item_Rate_Update = isnull(json.Item_Rate_Update, "false");
            string ColumnPerfix = isnull(json.ColumnPerfix, "");


            string Bill_Date = isnull(json.dc_bill_date, "");

            Newtonsoft.Json.Linq.JArray items = json.items;


            Boolean isIDn = false;

            DataTable dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Delivery_Challan'");

            DataTable dc = GITAPI.dbFunctions.getTable("SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = 'Delivery_Challan'");
            string Query = "";
            int l = 0;
            if (dc.Rows.Count > 0)
            {
                isIDn = true;
                l = 1;
            }
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

                    Purchse_Date = isnull(json.dc_date, "");
                    Purchse_No = get_DC_No(Company);
                    if (isIDn == false)
                    {
                        DataTable dd = GITAPI.dbFunctions.getTable("select isnull(max(" + dt.Rows[0]["Column_Name"].ToString() + "),0)+1 from Delivery_Challan");
                        if (dd.Rows.Count > 0)
                        {
                            json[dt.Rows[0]["Column_Name"].ToString()] = dd.Rows[0][0].ToString();
                            ID = dd.Rows[0][0].ToString();
                        }
                    }
                    Query += "insert into Delivery_Challan (";

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

                    Purchse_Date = isnull(json.dc_date, "");
                    Purchse_No = isnull(json.dc_no, "");


                    Query = "";
                    Query += "update  Delivery_Challan Set ";

                    for (int i = l; i < dt.Rows.Count - 1; i++)
                    {
                        string col = dt.Rows[i]["Column_Name"].ToString();
                        if (col.ToLower().Equals(ColumnPerfix + "ewaybill") ||
                            col.ToLower().Equals(ColumnPerfix + "ewaybilldate") ||
                            col.ToLower().Equals(ColumnPerfix + "vaildupto") ||
                            col.ToLower().Equals(ColumnPerfix + "ewayremarks"))
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
                    else if (Column.Equals(ColumnPerfix + "no"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No;
                    }
                    else if (Column.Equals(ColumnPerfix + "company"))
                    {
                        com.Parameters.Add("@" + dt.Rows[i]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Company.Replace("_", "");
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




                DataTable dds = GITAPI.dbFunctions.getTable("delete from Delivery_Challan_Details where dc_no='" + Purchse_No + "'");

                dt = GITAPI.dbFunctions.getTable("SELECT  * FROM  information_schema.columns where table_name='Delivery_Challan_details'");
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

                        Query += "insert into Delivery_Challan_details (";

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
                            else if (Column.Equals(ColumnPerfix + "no"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No;
                            }
                            else if (Column.Equals(ColumnPerfix + "company"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Company.Replace("_", "");
                            }
                            else if (Column.Equals(ColumnPerfix + "date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_Date;
                            }
                            else if (Column.Equals(ColumnPerfix + "bill_date"))
                            {
                                com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Bill_Date;
                            }
                            //else if (Column.Equals(ColumnPerfix + "landing_cost"))
                            //{
                            //    com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = ((decimal.Parse(items[i]["pur_net_amt"].ToString())) / ((decimal.Parse(items[i]["pur_qty"].ToString())) + (decimal.Parse(items[i]["pur_free"].ToString())))).ToString("0.00");
                            //}
                            //else if (Column.Equals(ColumnPerfix + "uni_code"))
                            //{
                            //    com.Parameters.Add("@" + dt.Rows[k]["Column_Name"].ToString(), SqlDbType.VarChar).Value = Purchse_No + '~' + int.Parse(items[i]["dc_prod_id"].ToString()).ToString("0000");
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
                        return ex.Message;
                    }
                }


                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        eInvoiceSession eInvSession = new eInvoiceSession(true, true);

        public void load_eInvoiceSession(string Company)
        {
            try
            {
                DataTable dd = GITAPI.dbFunctions.getTable("select * from Company_Master where CM_ID='" + Company.Replace("_", "") + "'");
                eInvSession.LoadAPISettingsFromConfigFile = false;
                eInvSession.LoadAPILoginDetailsFromConfigFile = false;
                eInvSession.eInvApiSetting.AspUserId = dd.Rows[0]["CM_AspUserId"].ToString();
                eInvSession.eInvApiSetting.AspPassword = dd.Rows[0]["CM_AspPassword"].ToString();
                eInvSession.eInvApiSetting.client_id = dd.Rows[0]["CM_ClientId"].ToString();
                eInvSession.eInvApiSetting.client_secret = dd.Rows[0]["CM_ClientSecret"].ToString();
                if (isProduction)
                {
                    // Use values from DataTable for production
                    eInvSession.eInvApiSetting.GSPName = dd.Rows[0]["CM_IRN_GSPName"].ToString();
                    eInvSession.eInvApiSetting.AuthUrl = dd.Rows[0]["CM_IRN_AspUrl"].ToString();
                    eInvSession.eInvApiSetting.BaseUrl = dd.Rows[0]["CM_IRN_BaseUrl"].ToString();
                    eInvSession.eInvApiSetting.EwbByIRN = dd.Rows[0]["CM_IRN_EwbBaseUrl"].ToString();
                    eInvSession.eInvApiSetting.CancelEwbUrl = dd.Rows[0]["CM_IRN_CancelEWB"].ToString();
                    eInvSession.eInvApiLoginDetails.UserName = dd.Rows[0]["CM_UserID"].ToString();
                    eInvSession.eInvApiLoginDetails.Password = dd.Rows[0]["CM_Password"].ToString();
                    eInvSession.eInvApiLoginDetails.GSTIN = dd.Rows[0]["CM_Gstin"].ToString();
                }
                else
                {
                    // Use sandbox values
                    eInvSession.eInvApiSetting.GSPName = "TaxPro_Sandbox";
                    eInvSession.eInvApiSetting.AuthUrl = "http://gstsandbox.charteredinfo.com/eivital/v1.04";
                    eInvSession.eInvApiSetting.BaseUrl = "http://gstsandbox.charteredinfo.com/eicore/v1.03";
                    eInvSession.eInvApiSetting.EwbByIRN = "http://gstsandbox.charteredinfo.com/eiewb/v1.03";
                    eInvSession.eInvApiSetting.CancelEwbUrl = "http://gstsandbox.charteredinfo.com/v1.03";
                    eInvSession.eInvApiLoginDetails.UserName = "TaxProEnvPON"; //dd.Rows[0]["CM_UserID"].ToString();
                    eInvSession.eInvApiLoginDetails.Password = "abc34*";//dd.Rows[0]["CM_Password"].ToString();
                    eInvSession.eInvApiLoginDetails.GSTIN = "34AACCC1596Q002";//dd.Rows[0]["CM_Gstin"].ToString();
                }

                eInvSession.eInvApiLoginDetails.AppKey = dd.Rows[0]["CM_IRN_AppKey"].ToString();
                eInvSession.eInvApiLoginDetails.AuthToken = dd.Rows[0]["CM_IRN_AuthToken"].ToString();
                eInvSession.eInvApiLoginDetails.Sek = dd.Rows[0]["CM_IRN_SEK"].ToString();
                eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp = DateTime.Parse(dd.Rows[0]["CM_IRN_TokenExp"].ToString());

            }
            catch { }
        }

        public void Insert_eInvoiceSession(JObject jsonData, string Company)
        {
            dynamic json = jsonData;

            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = @"UPDATE company_Master
                                SET CM_IRN_GSPName = @CM_GSPName,
                                    CM_AspUserId = @CM_AspUserId,
                                    CM_AspPassword = @CM_AspPassword,
                                    CM_ClientId = @CM_ClientId,
                                    CM_ClientSecret = @CM_ClientSecret,
                                    CM_IRN_BaseUrl = @CM_BaseUrl,
                                    CM_IRN_AspUrl = @CM_AspUrl,
                                    CM_IRN_EwbBaseUrl = @CM_IRN_EwbBaseUrl,
                                    CM_IRN_CancelEWB = @CM_IRN_CancelEWB,
                                    CM_Gstin = @CM_Gstin,
                                    CM_UserID = @CM_UserID,
                                    CM_Password = @CM_Password,
                                    CM_IRN_AppKey = @CM_AppKey,
                                    CM_IRN_AuthToken = @CM_AuthToken,
                                    CM_IRN_TokenExp = @CM_TokenExp,
                                    CM_IRN_SEK = @CM_SEK
                                WHERE CM_ID = @CM_ID";
                com.Parameters.Add("@CM_GSPName", SqlDbType.VarChar).Value = json.eInvApiSetting.GSPName;
                com.Parameters.Add("@CM_AspUserId", SqlDbType.VarChar).Value = json.eInvApiSetting.AspUserId;
                com.Parameters.Add("@CM_AspPassword", SqlDbType.VarChar).Value = json.eInvApiSetting.AspPassword;
                com.Parameters.Add("@CM_ClientId", SqlDbType.VarChar).Value = json.eInvApiSetting.client_id;
                com.Parameters.Add("@CM_ClientSecret", SqlDbType.VarChar).Value = json.eInvApiSetting.client_secret;
                com.Parameters.Add("@CM_BaseUrl", SqlDbType.VarChar).Value = json.eInvApiSetting.BaseUrl;
                com.Parameters.Add("@CM_AspUrl", SqlDbType.VarChar).Value = json.eInvApiSetting.AuthUrl;
                com.Parameters.Add("@CM_IRN_EwbBaseUrl", SqlDbType.VarChar).Value = json.eInvApiSetting.EwbByIRN;
                com.Parameters.Add("@CM_IRN_CancelEWB", SqlDbType.VarChar).Value = json.eInvApiSetting.CancelEwbUrl;
                com.Parameters.Add("@CM_Gstin", SqlDbType.VarChar).Value = json.eInvApiLoginDetails.GSTIN;
                com.Parameters.Add("@CM_UserID", SqlDbType.VarChar).Value = json.eInvApiLoginDetails.UserName;
                com.Parameters.Add("@CM_Password", SqlDbType.VarChar).Value = json.eInvApiLoginDetails.Password;
                com.Parameters.Add("@CM_AppKey", SqlDbType.VarChar).Value = json.eInvApiLoginDetails.AppKey;
                com.Parameters.Add("@CM_AuthToken", SqlDbType.VarChar).Value = json.eInvApiLoginDetails.AuthToken;
                com.Parameters.Add("@CM_TokenExp", SqlDbType.VarChar).Value = json.eInvApiLoginDetails.E_InvoiceTokenExp;
                com.Parameters.Add("@CM_SEK", SqlDbType.VarChar).Value = json.eInvApiLoginDetails.Sek;
                com.Parameters.Add("@CM_ID", SqlDbType.VarChar).Value = Company.Replace("_", "");

                com.ExecuteNonQuery();
            }
            catch (Exception ex) { }
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetAuthTokenAsync(string Company)
        {
            try
            {

                load_eInvoiceSession(Company);

                try
                {
                    DateTime tokenExp = eInvSession.eInvApiLoginDetails.E_InvoiceTokenExp.Value;
                    if (tokenExp > DateTime.Now)
                    {
                        string validUntil = tokenExp.ToString("dd-MMM-yy HH:mm:ss");
                        return Ok("Validity of previous Auth Token is not expired. It will expire on : " + validUntil);
                    }
                }
                catch { }

                TaxProEInvoice.API.TxnRespWithObj<eInvoiceSession> txnRespWithObj = await eInvoiceAPI.GetAuthTokenAsync(eInvSession);
                string jsonString = JsonConvert.SerializeObject(eInvSession);

                // Parse JSON string to JObject
                JObject jObject = JObject.Parse(jsonString);
                if (txnRespWithObj.IsSuccess)
                {
                    Insert_eInvoiceSession(jObject, Company);
                }
                return Ok(txnRespWithObj); // Return the successful response
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); // Return 500 status code for internal errors
            }
        }


        [HttpGet]
        public async Task<IHttpActionResult> Gen_IRN(string InvoiceNo, string Company)
        {
            load_eInvoiceSession(Company);
            string reqPlGenIRN = CreateIRN(InvoiceNo, Company);
            var reqPlGenIRN2 = JsonConvert.DeserializeObject<dynamic>(reqPlGenIRN);

            string rtbResponse;

            try
            {
                var response = await eInvoiceAPI.GenIRNAsync(eInvSession, reqPlGenIRN, 250);

                if (response.IsSuccess)
                {
                    rtbResponse = JsonConvert.SerializeObject(response.RespObj);
                    dynamic json = JsonConvert.DeserializeObject<dynamic>(rtbResponse);
                    RespPlGenIRN respPlGenIRN = response.RespObj;
                    Image im = respPlGenIRN.QrCodeImage;
                    // Second update with QR code
                    using (SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring))
                    {
                        con.Open();
                        using (SqlCommand com = new SqlCommand())
                        {
                            com.Connection = con;
                            com.CommandType = CommandType.Text;
                            com.CommandText = "update sales Set sal_ackno=@ackno, sal_ackdt=@ackdt, sal_irn=@irn_no, sal_qrcode=@QR_Image where sal_bill_no=@uniqno";

                            com.Parameters.Add("@ackno", SqlDbType.VarChar).Value = json.AckNo;
                            com.Parameters.Add("@ackdt", SqlDbType.VarChar).Value = json.AckDt;
                            com.Parameters.Add("@uniqno", SqlDbType.VarChar).Value = InvoiceNo;
                            com.Parameters.Add("@irn_no", SqlDbType.VarChar).Value = json.Irn;

                            byte[] imageData = null;
                            using (var ms = new MemoryStream())
                            {

                                MemoryStream ms1 = new MemoryStream();
                                im.Save(ms1, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imageData = ms1.GetBuffer();

                            }

                            com.Parameters.Add("@QR_Image", SqlDbType.Image).Value = (object)imageData;


                            com.ExecuteNonQuery();
                        }
                    }

                }
                else
                {
                    string errorDesc, errorCodes;
                    rtbResponse = HandleTxnErrorsAndInfo<RespPlGenIRN>(response, out errorDesc, out errorCodes);
                }
            }
            catch (Exception ex)
            {
                rtbResponse = $"Exception: {ex.Message}";
            }

            return Ok(new { Data = reqPlGenIRN2, Response = rtbResponse });
        }




        private string CreateIRN(string InvoiceNo, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select * from Sales left outer join Company_Master on CM_ID=sal_branch where sal_bill_no='" + InvoiceNo + "' and sal_branch='" + Company.Replace("_", "") + "'");
            string dd = DateTime.Parse(dt.Rows[0]["sal_bill_date"].ToString()).ToString("dd/MM/yyyy").Replace("-", "/");

            string Data = "{ " +
              " \"Version\": \"1.1\",  " +
              " \"TranDtls\": {  " +
              " \"TaxSch\": \"GST\",  " +
              "  \"SupTyp\": \"B2B\",  " +
              " \"RegRev\": \"N\",  " +
              " \"EcmGstin\": null,  " +
              " \"IgstOnIntra\": \"N\"  " +
              " },  " +
              " \"DocDtls\": {  " +
              " \"Typ\": \"INV\",  " +
               "  \"No\": \"" + dt.Rows[0]["sal_bill_no"].ToString() + "\",  " +
                " \"Dt\": \"" + dd + "\"  " +
              " },  " +
              " \"SellerDtls\": {  " +
              "  \"Gstin\": \"" + dt.Rows[0]["CM_GST_No"].ToString() + "\",  " +
               "  \"LglNm\": \"" + dt.Rows[0]["CM_Name"].ToString() + "\",  " +
               "  \"TrdNm\": \"" + dt.Rows[0]["CM_Name"].ToString() + " \",  " +
               "  \"Addr1\": \"" + dt.Rows[0]["CM_Address1"].ToString() + "\",  " +
               "  \"Addr2\": \"" + dt.Rows[0]["CM_Address2"].ToString() + "\",  " +
               "  \"Loc\": \"" + dt.Rows[0]["CM_Address3"].ToString() + "\",  " +
               "  \"Pin\": " + dt.Rows[0]["CM_Address5"].ToString() + ",  " +
               "  \"Stcd\": \"" + dt.Rows[0]["CM_State_Code"].ToString() + "\",  " +
               "  \"Ph\": \"" + dt.Rows[0]["CM_Phone_Res"].ToString() + "\",  " +
               "  \"Em\": \"" + dt.Rows[0]["CM_Email_ID"].ToString() + "\"  " +
                " },  " +
                " \"BuyerDtls\": {  " +
                "     \"Gstin\": \"" + dt.Rows[0]["sal_gstin"].ToString() + "\",  " +
                "   \"LglNm\": \"" + dt.Rows[0]["sal_ledger_name"].ToString() + "\",  " +
                "   \"TrdNm\": \"" + dt.Rows[0]["sal_ledger_name"].ToString() + "\",  " +
                "  \"Pos\": \"" + dt.Rows[0]["sal_scode"].ToString() + "\",  " +
                "  \"Addr1\": \"" + dt.Rows[0]["sal_address1"].ToString() + "\",  " +
                "  \"Addr2\": \"" + dt.Rows[0]["sal_address2"].ToString() + "\",  " +
                "  \"Loc\": \"" + dt.Rows[0]["sal_address3"].ToString() + "\",  " +
                "  \"Pin\": " + dt.Rows[0]["sal_pincode"].ToString() + ",  " +
                "  \"Stcd\": \"" + dt.Rows[0]["sal_scode"].ToString() + "\",  " +
                "  \"Ph\": \"" + dt.Rows[0]["sal_contact_no"].ToString() + "\",  " +
                "  \"Em\": \"xyz@yahoo.com\"  " +
                " },  " +
              " \"DispDtls\": {  " +
               "  \"Nm\": \"" + dt.Rows[0]["CM_Name"].ToString() + "\",  " +
               "  \"Addr1\": \"" + dt.Rows[0]["CM_Address1"].ToString() + "\",  " +
               "  \"Addr2\": \"" + dt.Rows[0]["CM_Address2"].ToString() + "\",  " +
               "  \"Loc\": \"" + dt.Rows[0]["CM_Address3"].ToString() + "\",  " +
               "  \"Pin\":" + dt.Rows[0]["CM_Address5"].ToString() + ",  " +
               "  \"Stcd\": \"" + dt.Rows[0]["CM_State_Code"].ToString() + "\"  " +
              " },  " +
              " \"ShipDtls\": {  " +
               "  \"Gstin\": \"" + dt.Rows[0]["sal_gstin"].ToString() + "\",  " +
               "  \"LglNm\": \"" + dt.Rows[0]["sal_ledger_name"].ToString() + "\",  " +
               "  \"TrdNm\": \"" + dt.Rows[0]["sal_ledger_name"].ToString() + "\",  " +
               "  \"Addr1\": \"" + dt.Rows[0]["sal_address1"].ToString() + "\",  " +
               "  \"Addr2\": \"" + dt.Rows[0]["sal_address2"].ToString() + "\",  " +
               "  \"Loc\": \"" + dt.Rows[0]["sal_address3"].ToString() + "\",  " +
               "  \"Pin\": " + dt.Rows[0]["sal_pincode"].ToString() + ",  " +
               "  \"Stcd\": \"" + dt.Rows[0]["sal_scode"].ToString() + "\"  " +
               " },  " +
               " \"ItemList\": [  ";


            DataTable ddp = GITAPI.dbFunctions.getTable("select * from Sales_details where sal_bill_no='" + InvoiceNo + "' and sal_branch='" + Company.Replace("_", "") + "'");

            for (int i = 0; i < ddp.Rows.Count; i++)
            {

                Data += "  {  " +
                  "   \"SlNo\": \"" + i + "\",  " +
                   "  \"PrdDesc\": \"" + ddp.Rows[i]["sal_prod_name"] + "\",  " +
                    " \"IsServc\": \"N\",  " +
                    " \"HsnCd\": \"" + ddp.Rows[i]["sal_hsn_code"] + "\",  " +
                    " \"Barcde\": \"" + ddp.Rows[i]["sal_no"] + "\",  " +
                    " \"Qty\": " + ddp.Rows[i]["sal_qty"] + ",  " +
                    " \"FreeQty\": 0,  " +
                    " \"Unit\": \"" + ddp.Rows[i]["sal_uom"] + "\",  " +
                    " \"UnitPrice\": " + decimal.Parse(ddp.Rows[i]["sal_rate"].ToString()).ToString("0.00") + ",  " +
                    " \"TotAmt\": " + (decimal.Parse(ddp.Rows[i]["sal_rate"].ToString()) * decimal.Parse(ddp.Rows[i]["sal_qty"].ToString())).ToString("0.00") + ",  " +
                    " \"Discount\": " + decimal.Parse(ddp.Rows[i]["sal_total_disc_amt"].ToString()).ToString("0.00") + ",  " +
                    " \"PreTaxVal\": 0,  " +
                    " \"AssAmt\":  " + ((decimal.Parse(ddp.Rows[i]["sal_rate"].ToString()) * decimal.Parse(ddp.Rows[i]["sal_qty"].ToString())) - (decimal.Parse(ddp.Rows[i]["sal_total_disc_amt"].ToString()))).ToString("0.00") + ",  " +
                    " \"GstRt\": " + ddp.Rows[i]["sal_gst_per"].ToString() + ",  " +
                    " \"IgstAmt\": " + ddp.Rows[i]["sal_igst_amt"] + ",  " +
                    " \"CgstAmt\": " + ddp.Rows[i]["sal_cgst_amt"] + ",  " +
                    " \"SgstAmt\": " + ddp.Rows[i]["sal_sgst_amt"] + ",  " +
                    " \"CesRt\": 0 ,  " +
                    " \"CesAmt\": 0,  " +
                    " \"CesNonAdvlAmt\": 0,  " +
                    " \"StateCesRt\": 0,  " +
                    " \"StateCesAmt\": 0.00,  " +
                    " \"StateCesNonAdvlAmt\": 0,  " +
                    " \"OthChrg\": 0,  " +
                    " \"TotItemVal\":  " + decimal.Parse(ddp.Rows[i]["sal_net_amt"].ToString()).ToString("0.00") + " ,  " +
                    " \"OrdLineRef\": \"-\",  " +
                    " \"OrgCntry\": \"AG\",  " +
                    " \"PrdSlNo\": \"12345\",  " +
                    " \"BchDtls\": {  " +
                    "   \"Nm\": \"00000\",  " +
                    "   \"Expdt\": \"01/08/2020\",  " +
                     "  \"wrDt\": \"01/09/2020\"  " +
                    " },  " +
                    " \"AttribDtls\": [  " +
                    "   {  " +
                     "    \"Nm\": \"No\",  " +
                      "   \"Val\": \"0\"  " +
                     "  }  " +
                   "  ]  " +
                  " }  ";

                if (i < ddp.Rows.Count - 1)
                {
                    Data += ",";
                }
            }

            Data += " ],  " +
            " \"ValDtls\": {  " +
             "  \"AssVal\": " + dt.Rows[0]["sal_taxable_amount"].ToString() + ",  " +
             "  \"CgstVal\": " + dt.Rows[0]["sal_cgst_amt"].ToString() + ",  " +
             "  \"SgstVal\": " + dt.Rows[0]["sal_sgst_amt"].ToString() + ",  " +
              " \"IgstVal\": " + dt.Rows[0]["sal_igst_amt"].ToString() + ",  " +
              " \"CesVal\": 0,  " +
              " \"StCesVal\":0,  " +
              " \"Discount\":" + decimal.Parse(dt.Rows[0]["sal_disc_amt"].ToString()).ToString("0") + ",  " +
              " \"OthChrg\": 0,  " +
              " \"RndOffAmt\": " + decimal.Parse(dt.Rows[0]["sal_round_off"].ToString()).ToString("0") + ",  " +
              " \"TotInvVal\": " + dt.Rows[0]["sal_net_amt"].ToString() + ",  " +
              " \"TotInvValFc\": " + dt.Rows[0]["sal_net_amt"].ToString() + "" +
            " },  " +
            " \"PayDtls\": {  " +
             "  \"Nm\": \"ABCDE\",  " +
             "  \"Accdet\": \"5697389713210\",  " +
             "  \"Mode\": \"Cash\",  " +
             "  \"Fininsbr\": \"SBIN11000\",  " +
             "  \"Payterm\": \"100\",  " +
             "  \"Payinstr\": \"Gift\",  " +
             "  \"Crtrn\": \"test\",  " +
             "  \"Dirdr\": \"test\",  " +
             "  \"Crday\": 100,  " +
             "  \"Paidamt\": 10000,  " +
             "  \"Paymtdue\": 5000  " +
            " },  " +
            " \"RefDtls\": {  " +
             "  \"InvRm\": \"TEST\",  " +
              " \"DocPerdDtls\": {  " +
               "  \"InvStDt\": \"01/08/2020\",  " +
                " \"InvEndDt\": \"01/09/2020\"  " +
              " },  " +
              " \"PrecDocDtls\": [  " +
               "  {  " +
               "    \"InvNo\": \"DOC/002\",  " +
                "   \"InvDt\": \"01/08/2020\",  " +
                "   \"OthRefNo\": \"123456\"  " +
               "  }  " +
              " ],  " +
              " \"ContrDtls\": [  " +
               "  {  " +
                "   \"RecAdvRefr\": \"Doc/003\",  " +
                "   \"RecAdvDt\": \"01/08/2020\",  " +
                "   \"Tendrefr\": \"Abc001\",  " +
                "   \"Contrrefr\": \"Co123\",  " +
                "   \"Extrefr\": \"Yo456\",  " +
                "   \"Projrefr\": \"Doc-456\",  " +
                "   \"Porefr\": \"Doc-789\",  " +
                "   \"PoRefDt\": \"01/08/2020\"  " +
               "  }  " +
             "  ]  " +
            " },  " +
            " \"AddlDocDtls\": [  " +
             "  {  " +
             "    \"Url\": \"https://einv-apisandbox.nic.in\",  " +
              "   \"Docs\": \"Test Doc\",  " +
               "  \"Info\": \"Document Test\"  " +
              " }  " +
            " ],  " +
            " \"ExpDtls\": {  " +
             "  \"ShipBNo\": \"A-248\",  " +
             "  \"ShipBDt\": \"" + dd + "\",  " +
             "  \"Port\": \"INABG1\",  " +
             "  \"RefClm\": \"N\",  " +
             "  \"ForCur\": \"AED\",  " +
             "  \"CntCode\": \"AE\"  " +
           "  }  " +
          " } ";
            return Data;
        }



        private string HandleTxnErrorsAndInfo<T>(TaxProEInvoice.API.TxnRespWithObj<T> txnRespWithObj, out string errorDesc, out string errorCodes)
        {
            errorDesc = string.Empty;
            errorCodes = string.Empty;

            try
            {
                // Handle ErrorDetails
                if (txnRespWithObj.ErrorDetails != null)
                {
                    foreach (var errPl in txnRespWithObj.ErrorDetails)
                    {
                        errorCodes += errPl.ErrorCode + ",";
                        errorDesc += $"{errPl.ErrorCode}: {errPl.ErrorMessage}{Environment.NewLine}";
                    }
                }

                // Handle InfoDetails
                if (txnRespWithObj.InfoDetails != null)
                {
                    foreach (var infoPl in txnRespWithObj.InfoDetails)
                    {
                        try
                        {
                            var json = JsonConvert.SerializeObject(infoPl.Desc);
                            switch (infoPl.InfCd)
                            {
                                case "DUPIRN":
                                    var dupIrn = JsonConvert.DeserializeObject<DupIrnPl>(json);
                                    break;

                                case "EWBERR":
                                    var ewbErrors = JsonConvert.DeserializeObject<List<EwbErrPl>>(json);
                                    break;

                                case "ADDNLNFO":
                                    string additionalInfo = infoPl.Desc?.ToString() ?? string.Empty;
                                    break;
                            }
                        }
                        catch (Exception innerEx)
                        {
                            errorDesc += $"InfoDetails parse error: {innerEx.Message}{Environment.NewLine}";
                        }
                    }
                }

                return string.IsNullOrWhiteSpace(errorDesc) ? "No error details provided." : errorDesc;
            }
            catch (Exception ex)
            {
                errorDesc += $"Unexpected error: {ex.Message}";
                return "Error while processing response.";
            }
        }




    }

}