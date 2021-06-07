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
   public interface ITags
    {
        Task<IEnumerable<Tags>> GetTegsList(string Search, string columnname, [FromQuery] Pagination pagination);
        Task<Tags> GetTagsById(int id);
        Task<BaseResult> UpdateTags(Tags model);
        Task<BaseResult> AddTags(Tags model);
        Task<BaseResult> DeleteTags(int id);
    }
}
