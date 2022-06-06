
using JobSearch.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JobSearch.API.Database
{
    public class JobSearchContext : DbContext
    {
        public JobSearchContext(DbContextOptions<JobSearchContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
    }
}
