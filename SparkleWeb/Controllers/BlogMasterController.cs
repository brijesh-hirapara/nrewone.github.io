using SparkleWeb.Model.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparkleWeb.model.Common;
using SparkleWeb.model.DataContext;
using SparkleWeb.model.MasterBlog;
using SparkleWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;

namespace SparkleWeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogMasterController : ControllerBase
    {
        private IBlogData _blog;
        private ApplicationDbContext _context;
        public BlogMasterController(IBlogData blog, ApplicationDbContext context)
        {
            _blog = blog;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetBlogaDataList(string Search, string columnname, [FromQuery] Pagination pagination)
        {

            return Ok(await _blog.GetBlogaDataList(Search, columnname, pagination));

        }
        //[Authorize(Roles = "SuparAdmin,Admin,Employee")]
        [HttpPost]
        [Route("AddBlogData")]
        public async Task<ActionResult<BaseResult>> AddBlogData([FromForm] BlogDataViewModels model)
        {
            try
            {
                //BlogDataViewModels model = new BlogDataViewModels();
                //var file = Request.Form.Files[0];
                var file = model.ImageFile;
                if (file.Length > 0)
                {
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        model.Image = fileName;
                        var fullPath = Path.Combine(pathToSave, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }
                model.CreatedDate = DateTime.Now;
                var data = await _blog.AddBlogData(model);
                return data;

            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }
        }
        [HttpGet]
        [Route("GetCategoryList")]

        public async Task<IActionResult> GetCategoryList(int id)
        {

            var result = await (from A in _context.BlogData
                                join B in _context.blogViewModels on A.BlogId equals B.BlogId
                                join C in _context.blogs on B.CategoryId equals C.CategoryId
                                where B.BlogId == id
                                select new
                                {
                                    C.CategoryId,
                                    C.Name,
                                    C.Status
                                }).ToListAsync();

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "BlogCategory data is empty");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetBlogDatatById")]
        public async Task<IActionResult> GetBlogDatatById(int id)
        {
            try
            {
                var post = await _blog.GetBlogDatatById(id);
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
        [Route("UpdateBlogData")]
        public async Task<ActionResult<BaseResult>> UpdateBlogData([FromForm] BlogDataViewModels model)
        {

            try
            {
                //BlogDataViewModels model = new BlogDataViewModels();
                //var file = Request.Form.Files[0];
                var file = model.ImageFile;
                if (file.Length > 0)
                {
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        model.Image = fileName;
                        var fullPath = Path.Combine(pathToSave, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }
                model.CreatedDate = DateTime.Now;
                var data = await _blog.UpdateBlogData(model);
                return data;

            }
            catch (Exception Ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            }

            //try
            //{
            //    model.UpdatedDate = DateTime.Now;
            //    var data = await _blog.UpdateBlogData(model);
            //    return data;
            //}
            //catch (Exception)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");
            //}

        }
        //[Authorize(Roles = "SuparAdmin")]
        [HttpDelete]
        [Route("DeleteBlogData")]
        public async Task<ActionResult<BaseResult>> DeleteBlogData(int id)
        {
            if (id != null)
            {
                var data = await _blog.DeleteBlogData(id);
                return data;
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Eror is retrieving Data from database");

        }
    }
}
