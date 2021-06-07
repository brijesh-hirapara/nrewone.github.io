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
    public class TagRepo : ITags
    {
        private ApplicationDbContext _context;
        public TagRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResult> AddTags(Tags model)
        {
            var result = await _context.Tag.AddAsync(model);
            if (result != null)
            {
                await _context.SaveChangesAsync();
                return new BaseResult { Messsage = "Success" };
            }
            return new BaseResult { Messsage = "Error" };
        }

        public async Task<BaseResult> DeleteTags(int id)
        {

            var result = await _context.Tag.FindAsync(id);
            if(result==null)
            {
                return new BaseResult { Messsage = "Error" };
            }
            _context.Tag.Remove(result);
            await _context.SaveChangesAsync();
            return new BaseResult { Messsage = "Success" };
        }


        public async Task<IEnumerable<Tags>> GetTegsList(string Search, string columnname, [FromQuery] Pagination pagination)
        {

            IQueryable<Tags> query = _context.Tag;

            if (!string.IsNullOrEmpty(Search) && !string.IsNullOrEmpty(columnname))
            {
                if (columnname == "Name")
                {
                    query = query.Where(e => e.Name.Contains(Search));
                }
               
            }
            return await query.OrderBy(x => x.Name).Skip((pagination.PageNumber - 1) * pagination.PageSize)
           .Take(pagination.PageSize).ToListAsync();
            //return await _context.blogs.ToListAsync();
        }

        public async Task<Tags> GetTagsById(int id)
        {
            var data=  await _context.Tag.Where(x => x.TagId == id).FirstOrDefaultAsync();
            if(data!=null)
            {
                return data;
            }
            return null;
        }

        public async Task<BaseResult> UpdateTags(Tags model)
        {
            if (model != null)
            {

               _context.Tag.Update(model);
                await _context.SaveChangesAsync();
              
                    return new BaseResult { Messsage = "Success" };
                
               
            }
            return new BaseResult { Messsage = "Error" };
        }
    }
}