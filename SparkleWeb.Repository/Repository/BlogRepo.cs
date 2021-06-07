using SparkleWeb.Model.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SparkleWeb.model.Blog;
using SparkleWeb.model.Common;
using SparkleWeb.model.DataContext;
using SparkleWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkleWeb.Repository.Repository
{
    public class BlogRepo : IBlog
    {
        private ApplicationDbContext _context;
        public BlogRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResult> AddBlog(Blogs model)
        {
            var result = await _context.blogs.AddAsync(model);
            if (result != null)
            {
                await _context.SaveChangesAsync();
                return new BaseResult { Messsage = "Success" };
            }
            return new BaseResult { Messsage = "Error" };
        }

        public async Task<BaseResult> DeleteBlog(int id)
        {

            var result = await _context.blogs.FindAsync(id);
            if(result==null)
            {
                return new BaseResult { Messsage = "Error" };
            }
            _context.blogs.Remove(result);
            await _context.SaveChangesAsync();
            return new BaseResult { Messsage = "Success" };
        }


        public async Task<IEnumerable<Blogs>> GetBlogList(string Search, string columnname, [FromQuery] Pagination pagination)
        {

            IQueryable<Blogs> query = _context.blogs;

            if (!string.IsNullOrEmpty(Search) && !string.IsNullOrEmpty(columnname))
            {
                if (columnname == "Name")
                {
                    query = query.Where(e => e.Name.Contains(Search));
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
            return await query.OrderBy(x => x.Name).Skip((pagination.PageNumber - 1) * pagination.PageSize)
           .Take(pagination.PageSize).ToListAsync();
            //return await _context.blogs.ToListAsync();
        }

        public async Task<Blogs> GetBlogtById(int id)
        {
            var data=  await _context.blogs.Where(x => x.CategoryId == id).FirstOrDefaultAsync();
            if(data!=null)
            {
                return data;
            }
            return null;
        }

        public async Task<BaseResult> UpdateBlog(Blogs model)
        {
            if (model != null)
            {

               _context.blogs.Update(model);
                await _context.SaveChangesAsync();
              
                    return new BaseResult { Messsage = "Success" };
                
               
            }
            return new BaseResult { Messsage = "Error" };
        }
    }
}