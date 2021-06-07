using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Common
{
    public class EmailTemplate
    {
        public static string VerifyEmailTemplate(string verificationLink)
        {
            return string.Format("Dear User, <a hreaf=\"{0}\">Click here</a> to verify your email \nor copy paste following link in a browser: {0}", verificationLink);
        }

        public static string ResetPasswordEmailTemplate(string verificationLink)
        {
            return string.Format("Dear User, <a hreaf=\"{0}\">Click here</a> to rest your password \nor copy paste following link in a browser: {0}", verificationLink);
        }
        public static string ContactUsEmailTemplate(ContactUs contactUs)
        {
            return string.Format("Dear {0} {1},\n Name : {0} {1} <br>\n EmailAddress : {2}\n Mobile No. : {3}\n Subject : {4} \n Message : {5} \n\n\n Thanks & Regards,\n Sparkle Web", contactUs.FirstName,contactUs.LastName,contactUs.EmailAddress,contactUs.MobileNo,contactUs.Subject,contactUs.Message);
        }
    }
}
