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

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> Attachments { get; set; }
        public DbSet<History> Histories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PostConfigurations());
            modelBuilder.Configurations.Add(new PostAttachmentConfiguration());
        }
    }
}
