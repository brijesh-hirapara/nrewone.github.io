using SparkleWeb.Model.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparkleWeb.model.Blog;
using SparkleWeb.model.Common;
using SparkleWeb.model.DataContext;
using SparkleWeb.model.MasterBlog;
using SparkleWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.Repository.Repository
{
    public class BlogDataRepo : IBlogData
    {
        private ApplicationDbContext _context;
        public BlogDataRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResult> AddBlogData(BlogDataViewModels model)
        {
            //string uniqueFileName = UploadedFile(model);
            BlogData blogs = new BlogData
            {
                CategoryId = model.CategoryId,
                Description = model.Description,
                Image = model.Image,
                CreatedDate = DateTime.Now,
                Meta_Description = model.Meta_Description,
                Meta_Title = model.Meta_Title,
                Meta_Keyword = model.Meta_Keyword,
                Status = model.Status,
                Title = model.Title,
            };

            var result = await _context.BlogData.AddAsync(blogs);
            if (result != null)
            {
                await _context.SaveChangesAsync();
                BlogData blogData = result.Entity;

                string[] CategoryIds = model.CategoryId.Split(",");
                for (int i = 0; i < CategoryIds.Length; i++)
                {
                    BlogViewModel viewmodel = new BlogViewModel();
                    viewmodel.BlogId = blogData.BlogId;
                    viewmodel.CategoryId = int.Parse(CategoryIds[i]);
                    var data = await _context.blogViewModels.AddAsync(viewmodel);
                    await _context.SaveChangesAsync();
                }
                return new BaseResult { Messsage = "Success" };
            }
            return new BaseResult { Messsage = "Error" };
        }
        private string UploadedFile(BlogDataViewModels model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                ////string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                //var folderName = Path.Combine("Resources", "Images");
                //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                //string filePath = Path.Combine(pathToSave, uniqueFileName);
                //using (var fileStream = new FileStream(filePath, FileMode.Create))
                //{
                //    model.Image.CopyTo(fileStream);
                //}
            }
            return uniqueFileName;
        }
        public async Task<BaseResult> DeleteBlogData(int id)
        {
            var data = _context.blogViewModels.Where(x => x.BlogId == id).ToList();
            _context.blogViewModels.RemoveRange(data);
            await _context.SaveChangesAsync();
            var result = await _context.BlogData.FindAsync(id);
            _context.BlogData.Remove(result);
            await _context.SaveChangesAsync();
            return new BaseResult { Messsage = "Success" };
        }

        public async Task<IEnumerable<BlogData>> GetBlogaDataList(string Search, string columnname, [FromQuery] Pagination pagination)
        {
            var Data = _context.BlogData;

            IQueryable<BlogData> query = _context.BlogData;

            if (!string.IsNullOrEmpty(Search) && !string.IsNullOrEmpty(columnname))
            {
                if (columnname == "Title")
                {
                    query = query.Where(e => e.Title.Contains(Search));
                }
                else if (columnname == "Status")
                {
                    query = query.Where(e => e.Status.Contains(Search));
                }
                //var listado = await (from user in query
                //                     join viewmodel in _context.viewmodels on user.AppointmentId equals viewmodel.AppointmentId
                //                     join service in _context.services on viewmodel.ServiceId equals service.ServiceId
                //                     select new { })
                //                .ToListAsync();

            }

            //foreach (var result in query)
            //{
            //    var totalPrice = (from m in _context.blogViewModels
            //                      join n in _context.blogs on m.CategoryId equals n.CategoryId
            //                      where m.BlogId == result.BlogId
            //                      select new { Name = n.Name,Status=n.Status }
            //                    ).ToList();


            //}
            return await query.OrderBy(x => x.Title).Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize).ToListAsync();


        }

        public async Task<BlogData> GetBlogDatatById(int id)
        {
            return await _context.BlogData.Where(x => x.BlogId == id).FirstOrDefaultAsync();
        }

        public async Task<BaseResult> UpdateBlogData(BlogDataViewModels model)
        {
            if (model != null)
            {
                string uniqueFileName = UploadedFile(model);
                BlogData blogs = new BlogData
                {
                  
                    //CategoryId = model.CategoryId,
                    //Description = model.Description,
                    //Image = uniqueFileName,
                    //Meta_Description = model.Meta_Description,
                    //Meta_Title = model.Meta_Title,
                    //Meta_Keyword = model.Meta_Keyword,
                    //Status = model.Status,
                    //UpdatedDate = DateTime.Now,
                    //Title = model.Title
                      BlogId = model.BlogId != 0 ? model.BlogId : 0,
                    CategoryId = model.CategoryId,
                    Description = model.Description,
                    Image = model.Image,
                    CreatedDate = DateTime.Now,
                    Meta_Description = model.Meta_Description,
                    Meta_Title = model.Meta_Title,
                    Meta_Keyword = model.Meta_Keyword,
                    Status = model.Status,
                    Title = model.Title,
                };
                _context.BlogData.Update(blogs);
                await _context.SaveChangesAsync();

                var data = _context.blogViewModels.Where(x => x.BlogId == blogs.BlogId).ToList();
                _context.blogViewModels.RemoveRange(data);
                await _context.SaveChangesAsync();

                string[] categoryIds = model.CategoryId.Split(",");
                for (int i = 0; i < categoryIds.Length; i++)
                {
                    BlogViewModel viewmodel = new BlogViewModel();
                    viewmodel.BlogId = model.BlogId;
                    viewmodel.CategoryId = int.Parse(categoryIds[i]);
                    var result = await _context.blogViewModels.AddAsync(viewmodel);
                    await _context.SaveChangesAsync();
                }

                return new BaseResult { Messsage = "Success" };
            }
            return new BaseResult { Messsage = "Error" };
        }

    }
}
