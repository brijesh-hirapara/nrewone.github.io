using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.model.MasterBlog
{
    public class BlogDataViewModels
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Status { get; set; }
        public string Meta_Title { get; set; }
        public string Meta_Keyword { get; set; }
        public string Meta_Description { get; set; }
        public string CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        //public IFormFile BlogFileName { get; set; }
    }
}
