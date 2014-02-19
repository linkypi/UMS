using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_ProMainInfoMap : EntityTypeConfiguration<View_ProMainInfo>
    {
        public View_ProMainInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ProMainID);

            // Properties
            this.Property(t => t.ProMainID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProMainName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_ProMainInfo");
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.ProMainName).HasColumnName("ProMainName");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.Introduction).HasColumnName("Introduction");
        }
    }
}
