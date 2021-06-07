using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparkleWeb.model.Career;
using SparkleWeb.model.Common;
using SparkleWeb.model.DataContext;
using SparkleWeb.Model.Common;
using SparkleWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.Repository.Repository
{
    public class CareerRepo : ICareer
    {
        private ApplicationDbContext _context;
        public CareerRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CareerData>> GetCareerDataList(string Search, string columnname, [FromQuery] Pagination pagination)
        {
            IQueryable<CareerData> query = _context.CareerData;
            if (!string.IsNullOrEmpty(Search) && !string.IsNullOrEmpty(columnname))
            {
                if (columnname == "Title")
                {
                    query = query.Where(e => e.Title.Contains(Search));
                }
            }
            return await query.Where(x => x.IsDeleted== false).OrderBy(x => x.Title).Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();
        }
        public async Task<BaseResult> AddCareer(CareerDataViewModels model)
        {
            CareerData objModel = new CareerData();
            objModel.Title = model.Title;
            objModel.Description = model.Description;
            objModel.Designation = model.Designation;
            objModel.Experience = model.Experience;
            objModel.Location = model.Location;
            objModel.Education = model.Education;
            objModel.SortOrder = model.SortOrder;
            objModel.IsActive = model.IsActive;
            objModel.IsDeleted = false;
            objModel.CreatedDate = DateTime.Now;
            var result = await _context.CareerData.AddAsync(objModel);
            if (result != null)
            {
                await _context.SaveChangesAsync();
                return new BaseResult { Messsage = "Success" };
            }
            return new BaseResult { Messsage = "Error" };
        }
        public async Task<CareerData> GetCareerDataById(int id)
        {
            var data = await _context.CareerData.Where(x => x.CareerId == id).FirstOrDefaultAsync();
            if (data != null)
            {
                return data;
            }
            return null;
        }
        public async Task<BaseResult> UpdateCareerData(CareerDataViewModels model)
        {
            if (model != null)
            {
                CareerData objModel = new CareerData();
                objModel.CareerId = model.CareerId;
                objModel.Title = model.Title;
                objModel.Description = model.Description;
                objModel.Designation = model.Designation;
                objModel.Experience = model.Experience;
                objModel.Location = model.Location;
                objModel.Education = model.Education;
                objModel.SortOrder = model.SortOrder;
                objModel.IsActive = model.IsActive;
                objModel.IsDeleted = false;
                objModel.UpdatedDate = DateTime.Now;
                _context.CareerData.Update(objModel);
                await _context.SaveChangesAsync();
                return new BaseResult { Messsage = "Success" };
            }
            return new BaseResult { Messsage = "Error" };
        }

        public async Task<BaseResult> DeleteCareerData(int id)
        {
            if (id != null)
            {
                CareerData objModel = new CareerData();
                objModel.CareerId = id;
                objModel.IsActive = false;
                objModel.IsDeleted = true;
                objModel.UpdatedDate = DateTime.Now;
                _context.CareerData.Update(objModel);
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
