using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class BranchConfiguration : EntityTypeConfiguration<Branch>
    {
        public BranchConfiguration()
        {
            ToTable("Branches");
            HasKey(x => x.BranchID);

            Property(x => x.BranchID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
                .IsRequired();

            HasMany(x => x.Users)
                .WithRequired(x => x.Branch)
                .HasForeignKey(x => x.BranchID);

            HasMany(x => x.Posts)
                .WithRequired(x => x.Branch)
                .HasForeignKey(x => x.BranchID);

            HasMany(x => x.Jobcards)
                .WithRequired(x => x.Branch)
                .HasForeignKey(x => x.BranchID);                
        }
    }
}
