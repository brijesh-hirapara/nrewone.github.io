using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Account
{
    public class UploadViewModel
    {
        public IFormFile Upload { get; set; }
    }
}
