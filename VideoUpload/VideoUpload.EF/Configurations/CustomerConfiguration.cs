using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("Customers");
            HasKey(x => x.CustomerCode);

            Property(x => x.CustomerCode)
                .IsRequired();

            Property(x => x.DateOfBirth)
                .IsOptional();
        }
    }
}
