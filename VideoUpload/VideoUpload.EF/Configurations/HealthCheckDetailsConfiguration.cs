using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class HealthCheckDetailsConfiguration : EntityTypeConfiguration<HealthCheckDetails>
    {
        public HealthCheckDetailsConfiguration()
        {
            ToTable("HealthCheckDetails");
            HasKey(x => x.HealCheckDetailsID);

                     
        }
    }
}
