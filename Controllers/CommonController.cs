using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;


namespace API.Controllers
{
    public class CommonController : ApiController
    {




        private readonly string secretKey = "GenuineIT12345678901234567890123456"; // 32-byte key

        [HttpPost]
        public IHttpActionResult get_Login(JObject jsonData)
        {
            try
            {
                dynamic json = jsonData;
                string UserName = isnull(json.UserName, "");
                string Password = isnull(json.Password, "");
                string IpAddress = isnull(json.IpAddress, "");
                //string User_Type = isnull(json.UserType, "");

                DataTable dt = GITAPI.dbFunctions.getTable("select *,RGV_vDesciption as Rights_Name,RGV_vCode as Route_URL from  user_Master x left outer join ReferenceGroup_Value on RGV_iID=UM_Rights left outer join Company_Master y on UM_Company =y.cm_ID where  UM_User_Name='" + UserName + "' and UM_Password='" + Password + "'");


                JObject ob = new JObject();

                if (dt.Rows.Count > 0)
                {
                    ob["UM_Company"] = dt.Rows[0]["UM_Company"].ToString();
                    ob["UM_User_Name"] = dt.Rows[0]["UM_User_Name"].ToString();
                    ob["UM_Full_Name"] = dt.Rows[0]["UM_Full_Name"].ToString();
                    ob["UM_Rights"] = dt.Rows[0]["UM_Rights"].ToString();
                    ob["Route_URL"] = dt.Rows[0]["Route_URL"].ToString();
                    ob["Rights_Name"] = dt.Rows[0]["Rights_Name"].ToString();
                    string data = GITAPI.dbFunctions.GetJSONString(dt);
                    JObject User_Data = JObject.Parse(data);
                    ob["User_Data"] = User_Data["record"];
                    GITAPI.dbFunctions.UserActivityLog(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), IpAddress, "Login");

                    string token = GenerateJwtToken(UserName, Password);
                    // Set the token in a secure HttpOnly cookie
                    var isHttps = HttpContext.Current.Request.IsSecureConnection;
                    var cookie = new HttpCookie("authToken", token)
                    {
                        HttpOnly = true, // Prevent access from JavaScript
                        Secure = isHttps,   // Send only over HTTPS
                        Path = "/",      // Available across the site
                        Expires = DateTime.UtcNow.AddYears(1)
                    };
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    return Ok(new { success = true, token = token, userData = ob });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        private string GenerateJwtToken(string userId, string Password)
        {
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();



            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId), // Store only user ID
                    new Claim("password", Password)
                }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [HttpGet]
        //[Authorize] // Requires authentication
        public IHttpActionResult ValidateToken()
        {
            try
            {
                string token = GetTokenFromHeaderOrCookie();
                if (string.IsNullOrEmpty(token)) return Unauthorized();
                var key = Encoding.UTF8.GetBytes(secretKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Extract JSON from token
                string UserName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string Password = principal.Claims.FirstOrDefault(c => c.Type == "password")?.Value;

                if (string.IsNullOrEmpty(UserName))
                    return Unauthorized();

                DataTable dt = GITAPI.dbFunctions.getTable("select RGV_vDesciption as Rights_Name,RGV_vCode as Route_URL,* from  user_Master x left outer join ReferenceGroup_Value on RGV_iID=UM_Rights left outer join Company_Master y on UM_Company =y.cm_ID where  UM_User_Name='" + UserName + "' and UM_Password='" + Password + "'");


                JObject ob = new JObject();

                if (dt.Rows.Count > 0)
                {
                    ob["UM_Company"] = dt.Rows[0]["UM_Company"].ToString();
                    ob["UM_User_Name"] = dt.Rows[0]["UM_User_Name"].ToString();
                    ob["UM_Full_Name"] = dt.Rows[0]["UM_Full_Name"].ToString();
                    ob["UM_Rights"] = dt.Rows[0]["UM_Rights"].ToString();
                    ob["Route_URL"] = dt.Rows[0]["Route_URL"].ToString();
                    ob["Rights_Name"] = dt.Rows[0]["Rights_Name"].ToString();
                    string data = GITAPI.dbFunctions.GetJSONString(dt);
                    JObject User_Data = JObject.Parse(data);
                    ob["User_Data"] = User_Data["record"];

                    return Ok(new { success = true, userData = ob });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }




        [HttpGet]
        public IHttpActionResult GetLogin()
        {
            try
            {
                string token = GetTokenFromHeaderOrCookie();
                if (string.IsNullOrEmpty(token)) return Unauthorized();
                var key = Encoding.UTF8.GetBytes(secretKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Extract JSON from token
                string UserName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string Password = principal.Claims.FirstOrDefault(c => c.Type == "password")?.Value;

                if (string.IsNullOrEmpty(UserName))
                    return Unauthorized();
                DataTable dt = GITAPI.dbFunctions.getTable("select RGV_vDesciption as Rights_Name,RGV_vCode as Route_URL,* from  user_Master x left outer join ReferenceGroup_Value on RGV_iID=UM_Rights left outer join Company_Master y on UM_Company =y.cm_ID where  UM_User_Name='" + UserName + "' and UM_Password='" + Password + "'");

                JObject ob = new JObject();
                if (dt.Rows.Count > 0)
                {
                    ob["UM_Company"] = dt.Rows[0]["UM_Company"].ToString();
                    ob["UM_User_Name"] = dt.Rows[0]["UM_User_Name"].ToString();
                    ob["UM_Full_Name"] = dt.Rows[0]["UM_Full_Name"].ToString();
                    ob["UM_Rights"] = dt.Rows[0]["UM_Rights"].ToString();
                    ob["Route_URL"] = dt.Rows[0]["Route_URL"].ToString();
                    ob["Rights_Name"] = dt.Rows[0]["Rights_Name"].ToString();
                    string data = GITAPI.dbFunctions.GetJSONString(dt);
                    JObject User_Data = JObject.Parse(data);
                    ob["User_Data"] = User_Data["record"];
                    GITAPI.dbFunctions.UserActivityLog(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), "", "Login Open");

                    return Ok(new { success = true, userData = ob });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        public IHttpActionResult Logout()
        {

            string token = GetTokenFromHeaderOrCookie();
            if (string.IsNullOrEmpty(token)) return Unauthorized();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            // Extract JSON from token
            string UserName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string Password = principal.Claims.FirstOrDefault(c => c.Type == "password")?.Value;

            if (string.IsNullOrEmpty(UserName))
                return Unauthorized();

            DataTable dt = GITAPI.dbFunctions.getTable("select RGV_vDesciption as Rights_Name,RGV_vCode as Route_URL,* from  user_Master x left outer join ReferenceGroup_Value on RGV_iID=UM_Rights left outer join Company_Master y on UM_Company =y.cm_ID where  UM_User_Name='" + UserName + "' and UM_Password='" + Password + "'");

            if (dt.Rows.Count > 0)
            {
                var cookie = new HttpCookie("authToken")
                {
                    Expires = DateTime.Now.AddDays(-1), // Set expiration date to past
                    Path = "/"
                };

                HttpContext.Current.Response.Cookies.Add(cookie);
                GITAPI.dbFunctions.UserActivityLog(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), "", "Logout");
                return Ok(new { success = true });
            }
            else
            {
                return Unauthorized();
            }
        }


        private string GetTokenFromHeaderOrCookie()
        {
            var authHeader = Request.Headers.Authorization;
            if (authHeader != null && authHeader.Scheme == "Bearer")
            {
                return authHeader.Parameter;
            }

            var authCookie = HttpContext.Current?.Request?.Cookies["authToken"];
            if (authCookie != null)
            {
                return authCookie.Value;
            }

            return null;
        }


        [HttpGet]
        public string get_Menu_for_user(string Rights, string Company)
        {
            try
            {

                JObject ob = new JObject();
                string condi = "";
                DataTable dt = null;
                DataTable dd = GITAPI.dbFunctions.getTable("select  RGV_vDesciption from ReferenceGroup_Value where RGV_iID=" + Rights);
                try
                {
                    if (dd.Rows[0][0].ToString().ToLower() != "admin")
                    {
                        condi = "and Rights_ID=" + Rights;

                        dt = GITAPI.dbFunctions.getTable("select m.* from Menu_Master" + Company + " mm left outer join Rights_Master" + Company + " r on r.Menu_ID=mm.Menu_ID left outer join Menu_Master m on mm.Menu_ID=m.ID where 0=0  " + condi);
                    }
                    else
                    {

                        dt = GITAPI.dbFunctions.getTable("select m.* from Menu_Master" + Company + " mm  left outer join Menu_Master m on mm.Menu_ID=m.ID where 0=0  " + condi);
                    }
                }
                catch { }
                string data = GITAPI.dbFunctions.GetJSONString(dt);

                JObject MenuData = JObject.Parse(data);
                ob["MenuData"] = MenuData["record"];

                string Query = "select * from  Field_Setting";
                dt = GITAPI.dbFunctions.getTable(Query);
                data = GITAPI.dbFunctions.GetJSONString(dt);

                JObject FieldSettingData = JObject.Parse(data);
                ob["FieldSettingData"] = FieldSettingData["record"];

                string q = "select  *,(select [RG_vCode]  from Reference_Group where RG_iID=RGV_IRG_ID ) as Ref_ID ,RGV_vDesciption as label, RGV_IID as  [value] from ReferenceGroup_Value where RGV_vStatus='A'  order by Ref_ID, RGV_vDesciption";
                dt = GITAPI.dbFunctions.getTable(q);
                data = GITAPI.dbFunctions.GetJSONString(dt);

                JObject ReferenceGroupData = JObject.Parse(data);
                ob["ReferenceGroupData"] = ReferenceGroupData["record"];
                return ob.ToString(Newtonsoft.Json.Formatting.None);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        public string insert_User_Master(JObject jsonData)
        {
            dynamic json = jsonData;
            string UM_ID = isnull(json.UM_ID, "");
            string UM_Full_Name = isnull(json.UM_Full_Name, "");
            string UM_User_Name = isnull(json.UM_User_Name, "");
            string UM_Password = isnull(json.UM_Password, "");
            string UM_Rights = isnull(json.UM_Rights, "");
            string UM_Edit = isnull(json.UM_Edit, "");
            string UM_Delete = isnull(json.UM_Delete, "");

            string UM_Company = isnull(json.UM_Company, "");
            string UM_Created_By = isnull(json.UM_Created_By, "");
            string UM_Created_Date = isnull(json.UM_Created_Date, "");
            string UM_Status = isnull(json.UM_Status, "");
            string Company = isnull(json.Company, "");

            UM_Company=Company.Replace("_","");
            Company="";


            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (UM_ID == "0")
                {
                    DataTable dd = GITAPI.dbFunctions.getTable("select  isnull(max(UM_iD),0)+1 from user_Master");
                    if (dd.Rows.Count > 0)
                    {
                        UM_ID = dd.Rows[0][0].ToString();
                    }

                    com.CommandText = "insert into User_Master (UM_ID,UM_Full_Name,UM_User_Name,UM_Password,UM_Rights,UM_Delete,UM_Edit,UM_Company,UM_Created_By,UM_Created_Date,UM_Status) Values (@UM_ID, @UM_Full_Name,@UM_User_Name,@UM_Password,@UM_Rights,@UM_Edit,@UM_Delete,@UM_Company,@UM_Created_By,getdate(), 'A'  )";
                }
                else
                {
                    com.CommandText = "update User_Master Set UM_Full_Name=@UM_Full_Name, UM_User_Name=@UM_User_Name, UM_Password=@UM_Password, UM_Rights=@UM_Rights,UM_Delete=@UM_Delete,UM_Edit=@UM_Edit, UM_Company=@UM_Company, UM_Created_By=@UM_Created_By where  UM_ID=@UM_ID ";
                    
                }
                com.Parameters.Add("@UM_ID", SqlDbType.VarChar).Value = UM_ID;
                com.Parameters.Add("@UM_Full_Name", SqlDbType.VarChar).Value = UM_Full_Name;
                com.Parameters.Add("@UM_User_Name", SqlDbType.VarChar).Value = UM_User_Name;
                com.Parameters.Add("@UM_Password", SqlDbType.VarChar).Value = UM_Password;
                com.Parameters.Add("@UM_Rights", SqlDbType.VarChar).Value = UM_Rights;
                com.Parameters.Add("@UM_Edit", SqlDbType.VarChar).Value = UM_Edit;
                com.Parameters.Add("@UM_Delete", SqlDbType.VarChar).Value = UM_Delete;

                com.Parameters.Add("@UM_Company", SqlDbType.VarChar).Value = UM_Company;
                com.Parameters.Add("@UM_Created_By", SqlDbType.VarChar).Value = UM_Created_By;
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
        public string get_User_Master(string Company)
        {
            try
            {
                DataTable dt = GITAPI.dbFunctions.getTable("select *,dbo.get_ref_value(UM_Rights) as Rights from User_Master where UM_Company=" + Company.Replace("_", ""));
                string data = GITAPI.dbFunctions.GetJSONString(dt);
                return data;
            }
            catch {

                return "[]";
            }

        }
      

        [HttpGet]
        public string delete_User_Master(string ID, string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("delete from User_Master  where UM_ID=" + ID);
            return "True";
        }


       [HttpPost]  
       public  string Insert_Company(JObject jsonData)
        {

          dynamic json = jsonData;

           string ID = GITAPI.dbFunctions.isnull(json.ID, "0");
           string Organization_Name=GITAPI.dbFunctions.isnull(json.Organization_Name,"");
           string Industry_Type = GITAPI.dbFunctions.isnull(json.Industry_Type, "");
           string Business_Location = GITAPI.dbFunctions.isnull(json.Business_Location, "");
           string Address1 = GITAPI.dbFunctions.isnull(json.Address1, "");
           string Address2 = GITAPI.dbFunctions.isnull(json.Address2, "");
           string Address3 = GITAPI.dbFunctions.isnull(json.Address3, "");
           string City = GITAPI.dbFunctions.isnull(json.City, "");
           string State = GITAPI.dbFunctions.isnull(json.State, "");
           string Pincode = GITAPI.dbFunctions.isnull(json.Pincode, "");
           string GST_No = GITAPI.dbFunctions.isnull(json.GST_No, "");
           string Website = GITAPI.dbFunctions.isnull(json.Website, "");
           string Phone_No = GITAPI.dbFunctions.isnull(json.Phone_No, "");
           string Primary_Contact = GITAPI.dbFunctions.isnull(json.Primary_Contact, "");
           string Mobile_No = GITAPI.dbFunctions.isnull(json.Mobile_No, "");
           string Email_ID = GITAPI.dbFunctions.isnull(json.Email_ID, "");
           string Password = GITAPI.dbFunctions.isnull(json.Password, "");
           string Created_by = GITAPI.dbFunctions.isnull(json.Created_by, "");
           string Compnay = GITAPI.dbFunctions.isnull(json.Created_by, "");


           bool isnew = false;
            SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                if (ID == "0")
                {
                    isnew = true;
                    com.CommandText = "insert into Company_Master ( ID, Organization_Name, Industry_Type, Business_Location, Address1, Address2, Address3, City, State, Pincode, GST_No, Website, Phone_No, Primary_Contact, Mobile_No, Email_ID, Password, Created_by, Created_Date, Status ) Values ( @ID, @Organization_Name, @Industry_Type, @Business_Location, @Address1, @Address2, @Address3, @City, @State, @Pincode, @GST_No, @Website, @Phone_No, @Primary_Contact, @Mobile_No, @Email_ID, @Password, @Created_by, getdate(), 'A') ";
                    DataTable dt = GITAPI.dbFunctions.getTable("select  isnull(max(ID),0)+1 from Company_Master");
                    ID = dt.Rows[0][0].ToString();
                }
                else
                {

                    com.CommandText = "update Company_Master Set Organization_Name=@Organization_Name, Industry_Type=@Industry_Type, Business_Location=@Business_Location, Address1=@Address1, Address2=@Address2, Address3=@Address3, City=@City, State=@State, Pincode=@Pincode, GST_No=@GST_No, Website=@Website, Phone_No=@Phone_No, Primary_Contact=@Primary_Contact, Mobile_No=@Mobile_No, Email_ID=@Email_ID, Password=@Password, Created_by=@Created_by where ID=@ID ";
                }

                com.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
                com.Parameters.Add("@Organization_Name", SqlDbType.VarChar).Value = Organization_Name;
                com.Parameters.Add("@Industry_Type", SqlDbType.VarChar).Value = Industry_Type;
                com.Parameters.Add("@Business_Location", SqlDbType.VarChar).Value = Business_Location;
                com.Parameters.Add("@Address1", SqlDbType.VarChar).Value = Address1;
                com.Parameters.Add("@Address2", SqlDbType.VarChar).Value = Address2;
                com.Parameters.Add("@Address3", SqlDbType.VarChar).Value = Address3;
                com.Parameters.Add("@City", SqlDbType.VarChar).Value = City;
                com.Parameters.Add("@State", SqlDbType.VarChar).Value = State;
                com.Parameters.Add("@Pincode", SqlDbType.VarChar).Value = Pincode;
                com.Parameters.Add("@GST_No", SqlDbType.VarChar).Value = GST_No;
                com.Parameters.Add("@Website", SqlDbType.VarChar).Value = Website;
                com.Parameters.Add("@Phone_No", SqlDbType.VarChar).Value = Phone_No;
                com.Parameters.Add("@Primary_Contact", SqlDbType.VarChar).Value = Primary_Contact;
                com.Parameters.Add("@Mobile_No", SqlDbType.VarChar).Value = Mobile_No;
                com.Parameters.Add("@Email_ID", SqlDbType.VarChar).Value = Email_ID;
                com.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
                com.Parameters.Add("@Created_by", SqlDbType.VarChar).Value = Created_by;
                com.ExecuteNonQuery();
                con.Close();


                if (isnew)
                {

                con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
                try
                {
                con.Open();
                com = new SqlCommand();
                com.Connection = con;
                com.CommandType = CommandType.Text;
                com.CommandText = "insert into User_Master (  User_Name, Email_ID, Phone_No, Password, Provider, Tocken, Employee_ID, Company_ID, Rights, Created_by, Created_Date, Status ) Values ( @User_Name, @Email_ID, @Phone_No, @Password, @Provider, @Tocken, @Employee_ID, @Company_ID, @Rights, @Created_by, getdate(),'A') ";
                com.Parameters.Add("@User_Name", SqlDbType.VarChar).Value = Organization_Name;
                com.Parameters.Add("@Email_ID", SqlDbType.VarChar).Value = Email_ID;
                com.Parameters.Add("@Phone_No", SqlDbType.VarChar).Value = "";
                com.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;
                com.Parameters.Add("@Provider", SqlDbType.VarChar).Value = "Qubha";
                com.Parameters.Add("@Tocken", SqlDbType.VarChar).Value = ID;
                com.Parameters.Add("@Employee_ID", SqlDbType.VarChar).Value = "";
                com.Parameters.Add("@Company_ID", SqlDbType.VarChar).Value = ID;
                com.Parameters.Add("@Rights", SqlDbType.VarChar).Value = "Admin";
                com.Parameters.Add("@Created_by", SqlDbType.VarChar).Value = Created_by;
                com.ExecuteNonQuery();
                con.Close();

                
                 }
                catch (Exception ex)
                 {
                 }

                }

                DataTable dtc = GITAPI.dbFunctions.getTable("select    ID,User_Name,Email_ID,Phone_No,Password,Provider,Tocken,Employee_ID,Company_ID,Rights,Created_by,Created_Date,Status,Server_URL from user_master where  Email_ID='" + Email_ID + "'");
                if (dtc.Rows.Count > 0)
                {
                    string data = GITAPI.dbFunctions.GetJSONString(dtc);
                    return data;
                }
                else
                {
                    return "";
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }




       public static string isnull(dynamic data, string d1)
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
       public string Update_Company_Master(JObject jsonData)
       {
           dynamic json = jsonData;
           string CM_ID = isnull(json.CM_ID, "");
           string CM_Code = isnull(json.CM_Code, "");
           string CM_Name = isnull(json.CM_Name, "");
           string CM_Address1 = isnull(json.CM_Address1, "");
           string CM_Address2 = isnull(json.CM_Address2, "");
           string CM_Address3 = isnull(json.CM_Address3, "");
           string CM_Address4 = isnull(json.CM_Address4, "");
           string CM_Address5 = isnull(json.CM_Address5, "");
           string CM_Email_ID = isnull(json.CM_Email_ID, "");
           string CM_GST_No = isnull(json.CM_GST_No, "");
           string CM_State_Code = isnull(json.CM_State_Code, "");
           string CM_State = isnull(json.CM_State, "");
           string CM_Pan_No = isnull(json.CM_Pan_No, "");
           string CM_Phone_off = isnull(json.CM_Phone_off, "");
           string CM_Phone_Res = isnull(json.CM_Phone_Res, "");
           string CM_Sub_Head = isnull(json.CM_Sub_Head, "");
           string CM_Subject_To = isnull(json.CM_Subject_To, "");
           string CM_Bank_Account1 = isnull(json.CM_Bank_Account1, "");
           string CM_Bank_Account2 = isnull(json.CM_Bank_Account2, "");
           string CM_Created_by = isnull(json.CM_Created_by, "");
           string CM_Created_date = isnull(json.CM_Created_date, "");
           string CM_Status = isnull(json.CM_Status, "");
           string CM_Type = isnull(json.CM_Type, "");
           string CM_From_Year = isnull(json.CM_From_Year, "");
           string CM_To_Year = isnull(json.CM_To_Year, "");
           string CM_Year = isnull(json.CM_Year, "");
           string CM_PO_Type = isnull(json.CM_PO_Type, "");
           string CM_Logo = isnull(json.CM_Logo, "");
           string CM_Height = isnull(json.CM_Height, "");
           string CM_Width = isnull(json.CM_Width, "");
           string CM_Bank_Name = isnull(json.CM_Bank_Name, "");
           string CM_Acc_Name = isnull(json.CM_Acc_Name, "");
           string CM_Acc_Number = isnull(json.CM_Acc_Number, "");
           string CM_IFSC = isnull(json.CM_IFSC, "");
           string CM_Branch = isnull(json.CM_Branch, "");
           string CM_Sales_Footer = isnull(json.CM_Sales_Footer, "");
           string CM_Sales_Term = isnull(json.CM_Sales_Term, "");
           string Company = isnull(json.Company, "");

           SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
           try
           {
               con.Open();
               SqlCommand com = new SqlCommand();
               com.Connection = con;
               com.CommandType = CommandType.Text;
               com.CommandText = "update Company_Master Set CM_Sales_Term=@CM_Sales_Term,CM_Sales_Footer=@CM_Sales_Footer,CM_Branch=@CM_Branch,CM_Code=@CM_Code,CM_Bank_Name=@CM_Bank_Name,CM_Acc_Name=@CM_Acc_Name,CM_Acc_Number=@CM_Acc_Number,CM_IFSC=@CM_IFSC, CM_Name=@CM_Name, CM_Address1=@CM_Address1, CM_Address2=@CM_Address2, CM_Address3=@CM_Address3, CM_Address4=@CM_Address4, CM_Address5=@CM_Address5, CM_Email_ID=@CM_Email_ID, CM_GST_No=@CM_GST_No, CM_State_Code=@CM_State_Code, CM_State=@CM_State, CM_Pan_No=@CM_Pan_No, CM_Phone_off=@CM_Phone_off, CM_Phone_Res=@CM_Phone_Res,CM_Created_by=@CM_Created_by, CM_Created_date=@CM_Created_date, CM_Status=@CM_Status where  CM_ID=@CM_ID ";
               com.Parameters.Add("@CM_ID", SqlDbType.VarChar).Value = CM_ID;
               com.Parameters.Add("@CM_Code", SqlDbType.NVarChar).Value = CM_Code;
               com.Parameters.Add("@CM_Name", SqlDbType.NVarChar).Value = CM_Name;
               com.Parameters.Add("@CM_Address1", SqlDbType.NVarChar).Value = CM_Address1;
               com.Parameters.Add("@CM_Address2", SqlDbType.NVarChar).Value = CM_Address2;
               com.Parameters.Add("@CM_Address3", SqlDbType.NVarChar).Value = CM_Address3;
               com.Parameters.Add("@CM_Address4", SqlDbType.NVarChar).Value = CM_Address4;
               com.Parameters.Add("@CM_Address5", SqlDbType.NVarChar).Value = CM_Address5;
               com.Parameters.Add("@CM_Email_ID", SqlDbType.NVarChar).Value = CM_Email_ID;
               com.Parameters.Add("@CM_GST_No", SqlDbType.VarChar).Value = CM_GST_No;
               com.Parameters.Add("@CM_State_Code", SqlDbType.VarChar).Value = CM_State_Code;
               com.Parameters.Add("@CM_State", SqlDbType.VarChar).Value = CM_State;
               com.Parameters.Add("@CM_Pan_No", SqlDbType.VarChar).Value = CM_Pan_No;
               com.Parameters.Add("@CM_Phone_off", SqlDbType.VarChar).Value = CM_Phone_off;
               com.Parameters.Add("@CM_Phone_Res", SqlDbType.VarChar).Value = CM_Phone_Res;
               com.Parameters.Add("@CM_Created_by", SqlDbType.VarChar).Value = CM_Created_by;
               com.Parameters.Add("@CM_Created_date", SqlDbType.VarChar).Value = CM_Created_date;
               com.Parameters.Add("@CM_Status", SqlDbType.VarChar).Value = CM_Status;
               com.Parameters.Add("@CM_Bank_Name", SqlDbType.VarChar).Value = CM_Bank_Name;
               com.Parameters.Add("@CM_Acc_Name", SqlDbType.VarChar).Value = CM_Acc_Name;

               com.Parameters.Add("@CM_IFSC", SqlDbType.VarChar).Value = CM_IFSC;
               com.Parameters.Add("@CM_Acc_Number", SqlDbType.VarChar).Value = CM_Acc_Number;
               com.Parameters.Add("@CM_Branch", SqlDbType.VarChar).Value = CM_Branch;
               com.Parameters.Add("@CM_Sales_Footer", SqlDbType.VarChar).Value = CM_Sales_Footer;
               com.Parameters.Add("@CM_Sales_Term", SqlDbType.VarChar).Value = CM_Sales_Term;

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
       public string insert_SMS_setting(JObject jsonData)
       {
           dynamic json = jsonData;
           string ID = isnull(json.ID, "");
           string Line1 = isnull(json.Line1, "");
           string Line2 = isnull(json.Line2, "");
           string Line3 = isnull(json.Line3, "");
           string Line4 = isnull(json.Line4, "");          

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
               
                   com.CommandText = "update SMS_Setting Set Line1=@Line1,Line2=@Line2,Line3=@Line3,Line4=@Line4,Created_by=@Created_by where ID=@ID ";
                   com.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;
               
               com.Parameters.Add("@Line1", SqlDbType.VarChar).Value = Line1;
               com.Parameters.Add("@Line2", SqlDbType.VarChar).Value = Line2;
               com.Parameters.Add("@Line3", SqlDbType.VarChar).Value = Line3;
               com.Parameters.Add("@Line4", SqlDbType.VarChar).Value = Line4;
              
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
       [HttpPost]
       public string insert_Gmail_setting(JObject jsonData)
       {
           dynamic json = jsonData;
           string ID = isnull(json.ID, "");
           string Email_From = isnull(json.Email_From, "");
           string From_Name = isnull(json.From_Name, "");
           string SMTP_Host = isnull(json.SMTP_Host, "");
           string SMTP_User = isnull(json.SMTP_User, "");
           string SMTP_Password = isnull(json.SMTP_Password, "");
           string SMTP_Port = isnull(json.SMTP_Port, "");
           string SMTP_Security = isnull(json.SMTP_Security, "");
           string SMTP_Domain = isnull(json.SMTP_Domain, "");
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

               com.CommandText = "update Mail_Setting Set Email_From=@Email_From,From_Name=@From_Name,SMTP_Host=@SMTP_Host,SMTP_User=@SMTP_User,SMTP_Password=@SMTP_Password,SMTP_Port=@SMTP_Port,SMTP_Security=@SMTP_Security,SMTP_Domain=@SMTP_Domain,Created_by=@Created_by where ID=@ID ";
               com.Parameters.Add("@ID", SqlDbType.VarChar).Value = ID;

               com.Parameters.Add("@Email_From", SqlDbType.VarChar).Value = Email_From;
               com.Parameters.Add("@From_Name", SqlDbType.VarChar).Value = From_Name;
               com.Parameters.Add("@SMTP_Host", SqlDbType.VarChar).Value = SMTP_Host;
               com.Parameters.Add("@SMTP_User", SqlDbType.VarChar).Value = SMTP_User;
               com.Parameters.Add("@SMTP_Password", SqlDbType.VarChar).Value = SMTP_Password;
               com.Parameters.Add("@SMTP_Port", SqlDbType.VarChar).Value = SMTP_Port;
               com.Parameters.Add("@SMTP_Security", SqlDbType.VarChar).Value = SMTP_Security;
               com.Parameters.Add("@SMTP_Domain", SqlDbType.VarChar).Value = SMTP_Domain;
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



       [HttpPost]
       public string Template_setting(JObject jsonData)
       {
           dynamic json = jsonData;

           string Type = isnull(json.Type, "");
           string Format = isnull(json.Format, "");
           string Company = isnull(json.Company, "");

           SqlConnection con = new SqlConnection(GITAPI.dbFunctions.connectionstring);
           try
           {
               con.Open();
               SqlCommand com = new SqlCommand();
               com.Connection = con;
               com.CommandType = CommandType.Text;

               DataTable dd = GITAPI.dbFunctions.getTable("select * from Template_master where T_Type='" + Type + "'");
               if (dd.Rows.Count > 0)
               {

                   com.CommandText = "update Template_master Set T_Value=@Format  where T_Type=@Type";
               }
               else
               {
                   com.CommandText = "insert into Template_master(T_Type,T_Value) values(@Type,@Format)";
               }

               com.Parameters.Add("@Format", SqlDbType.VarChar).Value = Format;
               com.Parameters.Add("@Type", SqlDbType.VarChar).Value = Type;
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
       [System.Web.Mvc.OutputCache(Duration = 86400, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "none")]
       public string Get_Companys(string Company)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select * from Company_Master");
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
        public string Get_Setting_Master(string Company)
        {
            DataTable dt = GITAPI.dbFunctions.getTable("select * from Setting_Master");
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
       public string Get_SMS(string Company)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select * from SMS_Setting where ID=1");
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
       public string Test_SMS(string Phone_No,string Msg,string Company)
       {
          GITAPI.dbFunctions.Send_SMS(Phone_No, Msg, Company);
          return "";
       }

       [HttpGet]
       public string Get_Gmail(string Company)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select * from Mail_Setting where ID=1");
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
       public string SendNotification(string DeviceToken, string title, string msg,string Link)
       {
           return GITAPI.dbFunctions.SendNotification(DeviceToken, title, msg, Link);
       }





        


       [HttpGet]
       public string Get_Company(string Company)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select *  from dbo.Company_Master where CM_ID='" + Company.Replace("_","") + "'");
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
       public string Get_Industry(string Company)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select * from Industry_Type where status='A'");
          
               string data = GITAPI.dbFunctions.GetJSONString(dt);
               return data;
          
       }





       [HttpGet]
       public string Check_Company_Mail(string Mail)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select *  from dbo.Company_Master where Email_ID='"+Mail+"'");
           if (dt.Rows.Count > 0)
           {
               string data = GITAPI.dbFunctions.GetJSONString(dt);
               return data;
           }
           else
           {
               return "";
           }
       }



       [HttpGet]
       public string Change_Password(string User_ID,string Old_Password,string New_Password)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select *  from  user_Master where UM_ID='" + User_ID + "' and  UM_Password='"+Old_Password+"'");
           if (dt.Rows.Count > 0)
           {
               dt = GITAPI.dbFunctions.getTable("Update user_Master set  UM_Password='" + New_Password + "' where UM_ID='" + User_ID + "'");
               return "True";
           }
           else
           {
               return "Invalid Password!!!";
           }
       }
        

   

       [HttpGet]
       public string get_Lodge_Login(string UserName, string Password)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select * from  User_Master x left outer join Company_Master y on UM_Company =y.cm_ID where  UM_User_Name='" + UserName+"' and UM_Password='"+Password+"'");
          
           if (dt.Rows.Count > 0)
           {
               string data = GITAPI.dbFunctions.GetJSONString(dt);
               return data;
           }
           else
           {
               return "";
           }
       }

    


       [HttpGet]
       public string get_Login(string UserName,string Password)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select    ID,User_Name,Email_ID,Phone_No,Password,Provider,Tocken,Employee_ID,Company_ID,Rights,Created_by,Created_Date,Status,Server_URL  from user_master where (Phone_No='" + UserName + "' or Email_ID='" + UserName + "') and Password='" + Password + "'");
           if (dt.Rows.Count > 0)
           {
               string data = GITAPI.dbFunctions.GetJSONString(dt);
               return data;
           }
           else
           {
               return "";
           }
       }


       [HttpGet]
       public string get_Login1(string UserName, string Password)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select * from Member_Master_1 where Member_ID='" + UserName + "'  and Password='" + Password + "'");
           if (dt.Rows.Count > 0)
           {
               string data = GITAPI.dbFunctions.GetJSONString(dt);
               return data;
           }
           else
           {
               return "";
           }
       }


        [Authorize]
       [HttpGet]
       public string get_Menus(string rights, string Company)
       {
           DataTable dt = GITAPI.dbFunctions.getTable("select lower(Menu_Name) as Menu,lower(Visibility) as Visibility from Rights_Master" + Company + " where Rights_Name='" + rights + "'");

           string data="{\"record\" :[{";
           for (int i = 0; i < dt.Rows.Count-1; i++)
           {
               data+="\""+dt.Rows[i]["Menu"].ToString()+"\":\""+dt.Rows[i]["Visibility"].ToString()+"\",";
               
           }
            if(dt.Rows.Count>0)
           data += "\"" + dt.Rows[dt.Rows.Count-1]["Menu"].ToString() + "\":\"" + dt.Rows[dt.Rows.Count-1]["Visibility"].ToString() + "\"";

           data +="}]}";
            
           return data;
          
       }


    }
}
