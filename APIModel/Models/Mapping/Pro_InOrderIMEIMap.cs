using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_InOrderIMEIMap : EntityTypeConfiguration<Pro_InOrderIMEI>
    {
        public Pro_InOrderIMEIMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_InOrderIMEI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_InOrderList)
                .WithMany(t => t.Pro_InOrderIMEI)
                .HasForeignKey(d => d.InListID);

        }
    }
}
