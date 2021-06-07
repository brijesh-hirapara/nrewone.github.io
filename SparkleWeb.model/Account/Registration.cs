using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Account
{
    public class Registration
    {
        public string Name { get; set; }

        public string Emailid { get; set; }

        public string Password { get; set; }

        public string MobileNo { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }



    }
}
