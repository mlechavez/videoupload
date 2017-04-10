using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class JobcardConfiguration : EntityTypeConfiguration<Jobcard>
    {
        public JobcardConfiguration()
        {
            ToTable("Jobcards");
            HasKey(x => x.JobcardNo);

            Property(x => x.JobcardNo)
                .IsRequired();       
        }
    }
}
