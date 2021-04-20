﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Ubay_CourseRegistration
{
    public class LoginHelper
    {
        private const string _sessionKey = "IsLogined";
        private const string _sessionKey_Account = "Account";
        public static bool HasLogined()
        {
            bool? val = HttpContext.Current.Session[_sessionKey] as bool?;

            if (val.HasValue && val.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 嘗試登入
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool TryLogin(string account, string pwd)
        {
            if (LoginHelper.HasLogined())
                return true;

            //Get user account from DB
            DataTable dt = DBAccountManager.GetUserAccount(account);

            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            //bool isAccountRight = string.Compare("admin", account, true) == 0;
            //bool isPasswordRight = string.Compare("Ys123", pwd) == 0;
            string dbPwd = dt.Rows[0].Field<string>("password");
            string dbFName = dt.Rows[0].Field<string>("Manager_FirstName");
            string dbLName = dt.Rows[0].Field<string>("Manager_LastName");
            bool isPasswordRight = string.Compare(dbPwd, pwd) == 0;

            //if (isAccountRight && isPasswordRight)
            if (isPasswordRight)

            {
                HttpContext.Current.Session[_sessionKey_Account] = account;
                HttpContext.Current.Session[_sessionKey] = true;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 登出目前使用者，如果還沒登入就不執行
        /// </summary>
        public static void Logout()
        {
            if (!LoginHelper.HasLogined())
                return;
            HttpContext.Current.Session.Remove(_sessionKey);
            HttpContext.Current.Session.Remove(_sessionKey_Account);

        }

        /// <summary>
        /// 取得已登入者的資訊，如果還沒登入回傳空字串
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUserInfo()
        {
            if (!LoginHelper.HasLogined())
                return string.Empty;

            return HttpContext.Current.Session[_sessionKey_Account] as string;
        }
    }
}