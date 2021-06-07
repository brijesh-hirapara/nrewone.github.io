using Microsoft.AspNetCore.Mvc;
using SparkleWeb.model.Common;
using SparkleWeb.model.Portfolio;
using SparkleWeb.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.Repository.IRepository
{
    public interface IPortfolio
    {
        Task<IEnumerable<PortfolioModel>> GetPortfolioDataList(string Search, string columnname, [FromQuery] Pagination pagination);
        Task<PortfolioModel> GetPortfolioDataById(int id);
        Task<BaseResult> UpdatePortfolioData(PortfolioViewModel model);
        Task<BaseResult> AddPortfolio(PortfolioViewModel model);
        Task<BaseResult> DeletePortfolioData(int id);
    }
}
