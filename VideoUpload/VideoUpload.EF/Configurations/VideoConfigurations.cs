using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class VideoConfigurations : EntityTypeConfiguration<Video>
    {
        public VideoConfigurations()
        {
            ToTable("Videos");
            HasKey(x => x.VideoID);

            Property(x => x.Title)
                .IsRequired();

            Property(x => x.Description)
                .IsMaxLength()
                .IsRequired();
        }
    }
}
