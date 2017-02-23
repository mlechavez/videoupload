using System.Data.Entity;
using VideoUpload.Core.Entities;
using VideoUpload.EF.Configurations;

namespace VideoUpload.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            :base("name=AppDbContext")
        {
        }
        public AppDbContext(string nameOrconnectionString)
            :base(nameOrconnectionString)
        {
        }

        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new VideoConfigurations());
        }
    }
}
