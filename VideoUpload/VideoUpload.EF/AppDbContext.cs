using System.Data.Entity;
using System.Diagnostics;
using VideoUpload.Core.Entities;
using VideoUpload.EF.Configurations;

namespace VideoUpload.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            :base("name=LocalAppDbContext")
        {
        }
        public AppDbContext(string nameOrconnectionString)
            :base(nameOrconnectionString)
        {
            this.Database.Log = (message) => Debug.Write(message);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> Attachments { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Activity> Activities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserClaimConfiguration());
            modelBuilder.Configurations.Add(new PostConfigurations());
            modelBuilder.Configurations.Add(new PostAttachmentConfiguration());
            modelBuilder.Configurations.Add(new ActivityConfiguration());
        }
    }
}
