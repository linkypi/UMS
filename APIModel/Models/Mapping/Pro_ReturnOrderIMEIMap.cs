using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ReturnOrderIMEIMap : EntityTypeConfiguration<Pro_ReturnOrderIMEI>
    {
        public Pro_ReturnOrderIMEIMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ReturnOrderIMEI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ReturnListID).HasColumnName("ReturnListID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_ReturnListInfo)
                .WithMany(t => t.Pro_ReturnOrderIMEI)
                .HasForeignKey(d => d.ReturnListID);

        }
    }
}
