using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class UserClaimConfiguration : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimConfiguration()
        {
            ToTable("UserClaims");
            HasKey(x => x.ClaimID);

            Property(x => x.ClaimID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(x => x.ClaimType)
                .IsMaxLength()
                .IsOptional();

            Property(x => x.ClaimValue)
                .IsMaxLength()
                .IsOptional();

            Property(x => x.UserID)
                .HasMaxLength(128)
                .IsOptional();
        }
    }
}
