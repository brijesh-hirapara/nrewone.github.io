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
    public class BlogController : ControllerBase
    {
        private IBlog _blog;
        private ApplicationDbContext _context;
        public BlogController(IBlog blog, ApplicationDbContext context)
        {
            _blog = blog;
            _context = context;
        }
        // Get BLoglist data
        [HttpGet]
        public async Task<IActionResult> GetBlogsList(string Search, string columnname, [FromQuery] Pagination pagination)
        {

            return Ok(await _blog.GetBlogList(Search, columnname, pagination));

        }
        //[Authorize(Roles = "SuparAdmin,Admin,Employee")]
        [HttpPost]
        [Route("AddBlog")]
        public async Task<ActionResult<BaseResult>> AddBlog(Blogs model)
        {
            try
            {
                
                var data = await _blog.AddBlog(model);
                return data;

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }
        }
      

        [HttpGet]
        [Route("GetBlogById")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            try
            {
                var post = await _blog.GetBlogtById(id);
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
        [Route("UpdateBlog")]
        public async Task<ActionResult<BaseResult>> UpdateBlog(Blogs model)
        {

            try
            {

                var data= await _blog.UpdateBlog(model);
                return data;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }

        }
        //[Authorize(Roles = "SuparAdmin")]
        [HttpDelete]
        [Route("DeleteBlog")]
        public async Task<ActionResult<BaseResult>> DeleteBlog(int id)
        {
            if (id != null)
            {

                var data= await _blog.DeleteBlog(id);
                return data;
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");

        }
    }
}
