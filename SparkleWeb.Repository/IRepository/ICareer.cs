using Microsoft.AspNetCore.Mvc;
using SparkleWeb.model.Career;
using SparkleWeb.model.Common;
using SparkleWeb.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.Repository.IRepository
{
    public interface ICareer
    {
        Task<IEnumerable<CareerData>> GetCareerDataList(string Search, string columnname, [FromQuery] Pagination pagination);
        Task<CareerData> GetCareerDataById(int id);
        Task<BaseResult> UpdateCareerData(CareerDataViewModels model);
        Task<BaseResult> AddCareer(CareerDataViewModels model);
        Task<BaseResult> DeleteCareerData(int id);
    }
}
