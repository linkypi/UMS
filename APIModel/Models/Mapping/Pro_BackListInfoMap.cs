using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_BackListInfoMap : EntityTypeConfiguration<Pro_BackListInfo>
    {
        public Pro_BackListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.BackListID);

            // Properties
            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_BackListInfo");
            this.Property(t => t.BackListID).HasColumnName("BackListID");
            this.Property(t => t.BackID).HasColumnName("BackID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProID).HasColumnName("ProID");

            // Relationships
            this.HasOptional(t => t.Pro_BackInfo)
                .WithMany(t => t.Pro_BackListInfo)
                .HasForeignKey(d => d.BackID);
            this.HasOptional(t => t.Pro_InOrderList)
                .WithMany(t => t.Pro_BackListInfo)
                .HasForeignKey(d => d.InListID);

        }
    }
}
