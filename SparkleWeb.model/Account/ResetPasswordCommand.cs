using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Account
{
   public class ResetPasswordCommand
    {
        public string token { get; set; }

        public string newPassword { get; set; }

        public string email { get; set; }
    }
}
