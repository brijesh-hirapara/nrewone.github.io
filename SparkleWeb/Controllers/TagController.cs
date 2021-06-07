using SparkleWeb.Model.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparkleWeb.model.Blog;
using SparkleWeb.model.Common;
using SparkleWeb.model.DataContext;
using SparkleWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparkleWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private ITags _Tag;
        private ApplicationDbContext _context;
        public TagController(ITags Tag, ApplicationDbContext context)
        {
            _Tag = Tag;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetTegsList(string Search, string columnname, [FromQuery] Pagination pagination)
        {

            return Ok(await _Tag.GetTegsList(Search, columnname, pagination));

        }
        //[Authorize(Roles = "SuparAdmin,Admin,Employee")]
        [HttpPost]
        [Route("AddTags")]
        public async Task<ActionResult<BaseResult>> AddTags(Tags model)
        {
            try
            {

                var data = await _Tag.AddTags(model);
                return data;

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }
        }
      

        [HttpGet]
        [Route("GetTagsById")]
        public async Task<IActionResult> GetTagsById(int id)
        {
            try
            {
                var post = await _Tag.GetTagsById(id);
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

        //[Authorize(Roles = "SuparAdmin,Admin")]
        [HttpPut]
        [Route("UpdateTags")]
        public async Task<ActionResult<BaseResult>> UpdateTags(Tags model)
        {

            try
            {

                var data= await _Tag.UpdateTags(model);
                return data;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }

        }
        //[Authorize(Roles = "SuparAdmin")]
        [HttpDelete]
        [Route("DeleteTags")]
        public async Task<ActionResult<BaseResult>> DeleteTags(int id)
        {
            if (id != null)
            {

                var data= await _Tag.DeleteTags(id);
                return data;
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");

        }
    }
}
