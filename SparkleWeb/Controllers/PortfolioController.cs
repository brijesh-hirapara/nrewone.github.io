using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparkleWeb.model.Common;
using SparkleWeb.model.DataContext;
using SparkleWeb.model.Portfolio;
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
    public class PortfolioController : ControllerBase
    {
        private IPortfolio _career;
        private ApplicationDbContext _context;
        public PortfolioController(IPortfolio Portfolio, ApplicationDbContext context)
        {
            _career = Portfolio;
            _context = context;
        }
        // Get list of Portfolio data
        [HttpGet]
        public async Task<IActionResult> GetPortfolioDataList(string Search, string columnname, [FromQuery] Pagination pagination)
        {
            return Ok(await _career.GetPortfolioDataList(Search, columnname, pagination));
        }

        // Add Portfolio Data
        [HttpPost]
        [Route("AddPortfolio")]
        public async Task<ActionResult<BaseResult>> AddPortfolio(PortfolioViewModel model)
        {
            try
            {
                var data = await _career.AddPortfolio(model);
                return data;

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }
        }

        //Get Portfolio Id wise single record data
        [HttpGet]
        [Route("GetPortfolioDataById")]

        public async Task<IActionResult> GetPortfolioDataById(int id)
        {

            try
            {
                var post = await _career.GetPortfolioDataById(id);
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

        //Update Portfolio Data
        [HttpPut]
        [Route("UpdatePortfolioData")]
        public async Task<ActionResult<BaseResult>> UpdatePortfolioData(PortfolioViewModel model)
        {
            try
            {
                var data = await _career.UpdatePortfolioData(model);
                return data;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }
        }
        //Delete Portfolio data by Id
        [HttpDelete]
        [Route("DeletePortfolioData")]
        public async Task<ActionResult<BaseResult>> DeletePortfolioData(int id)
        {
            if (id != null)
            {
                var data = await _career.DeletePortfolioData(id);
                return data;
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }
        }
    }
}
