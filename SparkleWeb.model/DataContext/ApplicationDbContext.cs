using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using SparkleWeb.model.Account;
using SparkleWeb.model.Blog;
using SparkleWeb.model.Career;
using SparkleWeb.model.Common;
using SparkleWeb.model.MasterBlog;
using SparkleWeb.model.Portfolio;

namespace SparkleWeb.model.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options) : base(Options)
        {

        }
        public DbSet<Blogs> blogs { get; set; }
        public DbSet<BlogData> BlogData { get; set; }
        public DbSet<BlogViewModel> blogViewModels { get; set; }
        public DbSet<Tags> Tag { get; set; }
        public DbSet<CareerData> CareerData { get; set; }
        public DbSet<PortfolioModel> Portfolio { get; set; }

    }
}
