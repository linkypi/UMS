using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_OutOrderListMap : EntityTypeConfiguration<Pro_OutOrderList>
    {
        public Pro_OutOrderListMap()
        {
            // Primary Key
            this.HasKey(t => t.OutListID);

            // Properties
            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_OutOrderList");
            this.Property(t => t.OutListID).HasColumnName("OutListID");
            this.Property(t => t.OutID).HasColumnName("OutID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProID).HasColumnName("ProID");

            // Relationships
            this.HasOptional(t => t.Pro_InOrderList)
                .WithMany(t => t.Pro_OutOrderList)
                .HasForeignKey(d => d.InListID);
            this.HasOptional(t => t.Pro_OutInfo)
                .WithMany(t => t.Pro_OutOrderList)
                .HasForeignKey(d => d.OutID);

        }
    }
}
