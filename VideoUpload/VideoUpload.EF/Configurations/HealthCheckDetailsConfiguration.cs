﻿using System.Data.Entity.ModelConfiguration;
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
