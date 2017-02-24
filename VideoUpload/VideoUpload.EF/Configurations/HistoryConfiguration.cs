using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class HistoryConfiguration : EntityTypeConfiguration<History>
    {
        public HistoryConfiguration()
        {
            ToTable("Histories");
            HasKey(x => x.HistoryID);

            Property(x => x.HistoryID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Recipient)
                .IsRequired();
        }
    }
}
