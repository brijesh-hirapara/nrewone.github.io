using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparkleWeb.model.Common;
using SparkleWeb.model.DataContext;
using SparkleWeb.model.Portfolio;
using SparkleWeb.Model.Common;
using SparkleWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.Repository.Repository
{
    public class PortfolioRepo : IPortfolio
    {
        private ApplicationDbContext _context;
        public PortfolioRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PortfolioModel>> GetPortfolioDataList(string Search, string columnname, [FromQuery] Pagination pagination)
        {
            IQueryable<PortfolioModel> query = _context.Portfolio;
            if (!string.IsNullOrEmpty(Search) && !string.IsNullOrEmpty(columnname))
            {
                if (columnname == "Title")
                {
                    query = query.Where(e => e.Title.Contains(Search));
                }
            }
            return await query.Where(x => x.IsDeleted == false).OrderBy(x => x.Title).Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();
        }
        public async Task<BaseResult> AddPortfolio(PortfolioViewModel model)
        {
            PortfolioModel objModel = new PortfolioModel();

            objModel.Title = model.Title;
            objModel.Description = model.Description;
            objModel.Image = model.Image;
            objModel.Url = model.Url;
            objModel.CategoryId = model.CategoryId;
            objModel.SortOrder = model.SortOrder;
            objModel.IsActive = model.IsActive;
            objModel.IsDeleted = false;
            objModel.CreatedDate = DateTime.Now;
            var result = await _context.Portfolio.AddAsync(objModel);
            if (result != null)
            {
                await _context.SaveChangesAsync();
                return new BaseResult { Messsage = "Success" };
            }
            return new BaseResult { Messsage = "Error" };
        }
        public async Task<PortfolioModel> GetPortfolioDataById(int id)
        {
            var data = await _context.Portfolio.Where(x => x.PortfolioId == id).FirstOrDefaultAsync();
            if (data != null)
            {
                return data;
            }
            return null;
        }
        public async Task<BaseResult> UpdatePortfolioData(PortfolioViewModel model)
        {
            if (model != null)
            {
                PortfolioModel objModel = new PortfolioModel();
                objModel.PortfolioId = model.PortfolioId;
                objModel.Title = model.Title;
                objModel.Description = model.Description;
                objModel.Image = model.Image;
                objModel.Url = model.Url;
                objModel.CategoryId = model.CategoryId;
                objModel.SortOrder = model.SortOrder;
                objModel.IsActive = model.IsActive;
                objModel.IsDeleted = false;
                objModel.UpdatedDate = DateTime.Now;
                _context.Portfolio.Update(objModel);
                await _context.SaveChangesAsync();
                return new BaseResult { Messsage = "Success" };
            }
            return new BaseResult { Messsage = "Error" };
        }

        public async Task<BaseResult> DeletePortfolioData(int id)
        {
            if (id != null)
            {
                PortfolioModel objModel = new PortfolioModel();
                objModel.PortfolioId = id;
                objModel.IsActive = false;
                objModel.IsDeleted = true;
                objModel.UpdatedDate = DateTime.Now;
                _context.Portfolio.Update(objModel);
                await _context.SaveChangesAsync();
                return new BaseResult { Messsage = "Error" };
            }
            else
            {
                return new BaseResult { Messsage = "Success" };
            }
        }
    }
}
