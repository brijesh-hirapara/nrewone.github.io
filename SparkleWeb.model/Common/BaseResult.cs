using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SparkleWeb.model.Common
{
    public class BaseResult
    {
        public static bool IsInDebuggingMode { get; set; }
    
        public string Messsage { get; set; }

        public IList<string> Errors { get; set; }

        [JsonIgnore]
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.OK;

        public void AddExceptionLog(Exception ex)
        {
            if (ResponseStatusCode == HttpStatusCode.OK)
                ResponseStatusCode = HttpStatusCode.BadRequest;

            // It is really bad idea to show exceptions in production.
            if (!IsInDebuggingMode) return;

            if (Errors == null) // Initialize if needed
                Errors = new List<string>();

            Errors.Add(ex.Message);
            if (ex.InnerException != null)
                AddExceptionLog(ex.InnerException);
        }
    }
}
