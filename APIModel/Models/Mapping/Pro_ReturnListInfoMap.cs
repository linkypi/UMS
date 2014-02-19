using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ReturnListInfoMap : EntityTypeConfiguration<Pro_ReturnListInfo>
    {
        public Pro_ReturnListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ReturnListID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ReturnListInfo");
            this.Property(t => t.ReturnListID).HasColumnName("ReturnListID");
            this.Property(t => t.ReturnID).HasColumnName("ReturnID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.BorowListID).HasColumnName("BorowListID");

            // Relationships
            this.HasOptional(t => t.Pro_InOrderList)
                .WithMany(t => t.Pro_ReturnListInfo)
                .HasForeignKey(d => d.InListID);
            this.HasOptional(t => t.Pro_ReturnInfo)
                .WithMany(t => t.Pro_ReturnListInfo)
                .HasForeignKey(d => d.ReturnID);

        }
    }
}
