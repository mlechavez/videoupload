using System.Data.Entity.ModelConfiguration;
using VideoUpload.Core.Entities;

namespace VideoUpload.EF.Configurations
{
    internal class AppLogConfiguration : EntityTypeConfiguration<AppLog>
    {
        public AppLogConfiguration()
        {
            ToTable("AppLogs");
            HasKey(x => x.AppLogID);

            Property(x => x.AppLogID)
                .IsRequired()
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(x => x.LogDate)
                .IsRequired();
    }
    }
}
