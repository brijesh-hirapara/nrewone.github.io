using SparkleWeb.Model.Common;
using Microsoft.AspNetCore.Mvc;
using SparkleWeb.model.Common;
using SparkleWeb.model.MasterBlog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.Repository.IRepository
{
    public interface IBlogData
    {
        Task<IEnumerable<BlogData>> GetBlogaDataList(string Search, string columnname, [FromQuery] Pagination pagination);
        Task<BlogData> GetBlogDatatById(int id);
        Task<BaseResult> UpdateBlogData(BlogDataViewModels model);
        Task<BaseResult> AddBlogData(BlogDataViewModels model);
        Task<BaseResult> DeleteBlogData(int id);
    }
}
