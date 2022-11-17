using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BTL_QLCuaHangBanQuanAo.Model.Class
{
    internal class Valid
    {
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }    
        
        public bool IsPhone(string phone)
        {
            try
            {
                string strRegex = @"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)";
                Regex re = new Regex(strRegex);
                if(re.IsMatch(phone)) { return true; }
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
