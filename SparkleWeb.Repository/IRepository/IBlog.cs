using SparkleWeb.Model.Common;
using Microsoft.AspNetCore.Mvc;
using SparkleWeb.model.Blog;
using SparkleWeb.model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.Repository.IRepository
{
   public interface IBlog
    {
        Task<IEnumerable<Blogs>> GetBlogList(string Search, string columnname, [FromQuery] Pagination pagination);
        Task<Blogs> GetBlogtById(int id);
        Task<BaseResult> UpdateBlog(Blogs model);
        Task<BaseResult> AddBlog(Blogs model);
        Task<BaseResult> DeleteBlog(int id);
    }
}
