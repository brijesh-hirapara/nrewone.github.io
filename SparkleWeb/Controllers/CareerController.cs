using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparkleWeb.model.Career;
using SparkleWeb.model.Common;
using SparkleWeb.model.DataContext;
using SparkleWeb.Model.Common;
using SparkleWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparkleWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CareerController : ControllerBase
    {
        private ICareer _career;
        private ApplicationDbContext _context;
        public CareerController(ICareer career, ApplicationDbContext context)
        {
            _career = career;
            _context = context;
        }
        // Get list of Career data
        [HttpGet]
        public async Task<IActionResult> GetCareerDataList(string Search, string columnname, [FromQuery] Pagination pagination)
        {
            return Ok(await _career.GetCareerDataList(Search, columnname, pagination));
        }

        // Add Career Data
        [HttpPost]
        [Route("AddCareerData")]
        public async Task<ActionResult<BaseResult>> AddCareerData(CareerDataViewModels model)
        {
            try
            {
                var data = await _career.AddCareer(model);
                return data;

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }
        }

        //Get Career Id wise single record data
        [HttpGet]
        [Route("GetCareerDataById")]

        public async Task<IActionResult> GetCareerDataById(int id)
        {

            try
            {
                var post = await _career.GetCareerDataById(id);
                if (post == null)
                {
                    return NotFound();
                }
                return Ok(post);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Update Career Data
        [HttpPut]
        [Route("UpdateBlogData")]
        public async Task<ActionResult<BaseResult>> UpdateCareerData(CareerDataViewModels model)
        {
            try
            {
                var data = await _career.UpdateCareerData(model);
                return data;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }
        }
        //Delete Career data by Id
        [HttpDelete]
        [Route("DeleteCareerData")]
        public async Task<ActionResult<BaseResult>> DeleteCareerData(int id)
        {
            if (id != null)
            {
                var data = await _career.DeleteCareerData(id);
                return data;
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }
        }
    }
}
