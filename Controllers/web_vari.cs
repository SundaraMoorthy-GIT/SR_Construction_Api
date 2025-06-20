using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Genuine_API.Controllers
{
    public class web_vari
    {
        public static string KOT_IP = System.Configuration.ConfigurationSettings.AppSettings["KOT_IP"];
        public static string NT_Print = System.Configuration.ConfigurationSettings.AppSettings["NT_Print"];
        public static string Print_Frontend = System.Configuration.ConfigurationSettings.AppSettings["Print_Frontend"];

        public static string Stock_Update = System.Configuration.ConfigurationSettings.AppSettings["Stock_Update"];


        public static string SmtpServer = System.Configuration.ConfigurationSettings.AppSettings["SmtpServer"];
        public static string Port = System.Configuration.ConfigurationSettings.AppSettings["Port"];

        public static string From = System.Configuration.ConfigurationSettings.AppSettings["From"];
        public static string To = System.Configuration.ConfigurationSettings.AppSettings["To"];


        public static string M_Password = System.Configuration.ConfigurationSettings.AppSettings["M_Password"];


        public static string Vat = System.Configuration.ConfigurationSettings.AppSettings["Vat"];
        public static string Hotel_Stock = System.Configuration.ConfigurationSettings.AppSettings["Hotel_Stock"];


        public static string Last = System.Configuration.ConfigurationSettings.AppSettings["last"];
        public static string gap = System.Configuration.ConfigurationSettings.AppSettings["gap"];
        public static string path = System.Configuration.ConfigurationSettings.AppSettings["path"];

        public static string Stock_Validate = System.Configuration.ConfigurationSettings.AppSettings["Stock_Validate"];

        public static string Receipt_No = "";
    }
}