using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_BorowOrderIMEIMap : EntityTypeConfiguration<Pro_BorowOrderIMEI>
    {
        public Pro_BorowOrderIMEIMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_BorowOrderIMEI");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.BorowListID).HasColumnName("BorowListID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");

            // Relationships
            this.HasOptional(t => t.Pro_BorowListInfo)
                .WithMany(t => t.Pro_BorowOrderIMEI)
                .HasForeignKey(d => d.BorowListID);

        }
    }
}
