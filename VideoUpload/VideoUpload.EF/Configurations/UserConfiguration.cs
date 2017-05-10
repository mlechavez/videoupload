using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            HasKey(x => x.UserID);

            Property(x => x.UserID)
                .HasMaxLength(128)
                .IsRequired();

            Property(x => x.UserName)
                .IsRequired();
            Property(x => x.FirstName)
                .IsRequired();
            Property(x => x.LastName)
                .IsRequired();
            Property(x => x.PasswordHash)
                .HasMaxLength(128);
            Property(x => x.SecurityStamp)
                .HasMaxLength(128);
            Property(x => x.Email)
                .IsRequired();
            Property(x => x.EmailPass)
                .IsRequired();

            HasMany(x => x.UserClaims)
                .WithOptional(x => x.User)
                .HasForeignKey(x => x.UserID);

            HasMany(x => x.Histories)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserID);         
        }
    }
}
