using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Off_AduitProInfoMap : EntityTypeConfiguration<Off_AduitProInfo>
    {
        public Off_AduitProInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Off_AduitProInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.AduitTypeID).HasColumnName("AduitTypeID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Price).HasColumnName("Price");

            // Relationships
            this.HasRequired(t => t.Off_AduitTypeInfo)
                .WithMany(t => t.Off_AduitProInfo)
                .HasForeignKey(d => d.AduitTypeID);
            this.HasRequired(t => t.Pro_SellType)
                .WithMany(t => t.Off_AduitProInfo)
                .HasForeignKey(d => d.SellType);
            this.HasRequired(t => t.Pro_ProMainInfo)
                .WithMany(t => t.Off_AduitProInfo)
                .HasForeignKey(d => d.ProMainID);

        }
    }
}
