using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoUpload.EF.Configurations
{
    internal class PostAttachmentConfiguration : EntityTypeConfiguration<PostAttachment>
    {
        public PostAttachmentConfiguration()
        {
            ToTable("Attachments");
            HasKey(x => x.PostAttachmentID);

            Property(x => x.PostID)
                .IsOptional();

            Property(x => x.DateCreated)
                .IsOptional();
        }
    }
}
