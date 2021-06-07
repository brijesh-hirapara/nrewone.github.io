using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Portfolio
{
    public class PortfolioViewModel
    {
        public int PortfolioId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Url { get; set; }
        public string CategoryId { get; set; }
        public int SortOrder { get; set; }
        public Boolean? IsActive { get; set; }
   
    }
}
