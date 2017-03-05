using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class PostConfigurations : EntityTypeConfiguration<Post>
    {
        public PostConfigurations()
        {
            ToTable("Posts");
            HasKey(x => x.PostID);

            Property(x => x.PostID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.PlateNumber)
                .IsRequired();

            Property(x => x.Description)
                .IsMaxLength()
                .IsRequired();

            Property(x => x.DateEdited)
                .IsOptional();
        }
    }
}
