using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_OutOrderIMEIMap : EntityTypeConfiguration<Pro_OutOrderIMEI>
    {
        public Pro_OutOrderIMEIMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_OutOrderIMEI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OutListID).HasColumnName("OutListID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_OutOrderList)
                .WithMany(t => t.Pro_OutOrderIMEI)
                .HasForeignKey(d => d.OutListID);

        }
    }
}
