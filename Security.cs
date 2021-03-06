﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using ASPXUtils;
using System.Configuration;
using System.Diagnostics;
namespace WebSite.Core
{
    public static class Security
    { 
        public static bool IsAdmin()
        {
            if (Utils.IsLocal())
            { 
                //HttpContext.Current.Response.Write("<div style='color:#660000;position:fixed;float:left;top:1px;left:1px;font-size:20px;'>LOCAL</div>");
                //if (ASPXUtils.Settings.GetSetting("tim") == "1")
                    return true; 
            }
            return  Settings.GetSetting("admin") == "1"; 
        } 
        public static void SetAdmin()
        {
            Settings.SaveSetting("admin", "1");
            HttpContext.Current.Response.Cookies["admin"].Value = "1";
            HttpContext.Current.Response.Cookies["admin"].Expires = DateTime.Now.AddMinutes(130);
        }
        public static void ClearAdmin()
        {
            Settings.SaveSetting("admin", "0");
            HttpContext.Current.Response.Cookies["admin"].Value = "0";
            HttpContext.Current.Response.Cookies["admin"].Expires = DateTime.Now.AddMinutes(130);
        }
        public static string ReadOnlyQName()
        {
            return "C1ea217eeef3".ToString();
        }
        public static string ReadOnlyQValue()
        {
            return EncodeDecode.EncodeURLSafe("703a8cb6", "A3211d22");
        } 
        public static string ReadOnlyLink(string HTTP_HOST)
        {
            HTTP_HOST = HTTP_HOST.Replace("http://", "");
            HTTP_HOST = HTTP_HOST.Replace("/", "");
            string ReadOnly = String.Format(
            "http://{0}/default.aspx?{1}={2}"
            , HTTP_HOST
            , ReadOnlyQName()
            , ReadOnlyQValue()
            ); 
            return ReadOnly;
        }

        public static string InstallQName()
        {
            int addDays = DateTime.Now.AddMinutes(DateTime.Now.Minute * DateTime.Now.Minute).Minute;
            //HttpContext.Current.Response.Write("<br>" + addDays.ToString());

            string val = DateTime.Now.AddDays(addDays).ToShortDateString().Replace("/", "");
            //HttpContext.Current.Response.Write("<br>"+ val);

            string nam = EncodeDecode.EncodeURLSafe(val, addDays.ToString());
            //HttpContext.Current.Response.Write("<br>" + nam);

            nam = nam.Replace("%", "~"); 
            return nam;
        }
        public static string InstallQValue()
        {
            int addDays = DateTime.Now.AddMinutes(DateTime.Now.Minute*DateTime.Now.Minute).Minute;
            //HttpContext.Current.Response.Write("<br>" + addDays.ToString());

            string val = DateTime.Now.AddDays(addDays).ToShortDateString().Replace("/", "");
            //HttpContext.Current.Response.Write("<br>" + val);

            return EncodeDecode.EncodeURLSafe(val, addDays.ToString());
        } 
        public static string InstallLink(string HTTP_HOST)
        {
            HTTP_HOST = HTTP_HOST.Replace("http://", "");
            HTTP_HOST = HTTP_HOST.Replace("/", "");
            string ReadOnly = String.Format(
            "http://{0}/E898F2AA-1D40-4BAE-B588-7557304545CD.aspx?{1}={2}"
            , HTTP_HOST
            , InstallQName()
            , InstallQValue()
            );
            return ReadOnly;
        }


    }
}
 