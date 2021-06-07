using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Common
{
    public class BlogViewModel
    {
        [Key]
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? BlogId { get; set; }
    }
}
