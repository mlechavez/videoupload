using System.Data.Entity.ModelConfiguration;
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

            Property(x => x.Approver)
                .HasMaxLength(15)
                .IsOptional();

            Property(x => x.DatePlayedVideo)
                .IsOptional();

            HasMany(x => x.Attachments)
                .WithRequired(x => x.Post)
                .HasForeignKey(x => x.PostID);

            HasMany(x => x.Histories)
                .WithRequired(x => x.Post)
                .HasForeignKey(x => x.PostID);
        }
    }
}
