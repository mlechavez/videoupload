﻿using System.Data.Entity;
using System.Diagnostics;
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
            
            //this.Database.Log = (message) => Debug.Write(message);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> Attachments { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Jobcard> Jobcards { get; set; }
        public DbSet<HealthCheck> HealthChecks { get; set; }
        public DbSet<HealthCheckDetails> HealthCheckDetails { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserClaimConfiguration());
            modelBuilder.Configurations.Add(new PostConfigurations());
            modelBuilder.Configurations.Add(new PostAttachmentConfiguration());
            modelBuilder.Configurations.Add(new ActivityConfiguration());
            modelBuilder.Configurations.Add(new HistoryConfiguration());
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new JobcardConfiguration());
            modelBuilder.Configurations.Add(new HealthCheckConfiguration());
            modelBuilder.Configurations.Add(new HealthCheckDetailsConfiguration());
            modelBuilder.Configurations.Add(new BranchConfiguration());
            modelBuilder.Configurations.Add(new AppLogConfiguration());
        }
    }
}
