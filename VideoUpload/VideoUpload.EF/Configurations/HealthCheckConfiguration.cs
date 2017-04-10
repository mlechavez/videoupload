using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class HealthCheckConfiguration : EntityTypeConfiguration<HealthCheck>
    {
        public HealthCheckConfiguration()
        {
            ToTable("HealthChecks");
            HasKey(x => x.HcCode);

            Property(x => x.HcCode)
                .IsRequired();
        }
    }
}
