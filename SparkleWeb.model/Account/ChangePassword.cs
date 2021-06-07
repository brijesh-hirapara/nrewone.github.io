using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Account
{
   public class ChangePassword
    {
        public string oldpassword { get; set; }
        public string newpassword { get; set; }
        public string Email { get; set; }
    }
}
