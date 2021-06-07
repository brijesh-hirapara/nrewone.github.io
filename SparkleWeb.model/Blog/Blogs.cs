using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.Blog
{
    public class Blogs
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
