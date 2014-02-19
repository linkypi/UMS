using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_BorowListInfoMap : EntityTypeConfiguration<Pro_BorowListInfo>
    {
        public Pro_BorowListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.BorowListID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_BorowListInfo");
            this.Property(t => t.BorowListID).HasColumnName("BorowListID");
            this.Property(t => t.BorowID).HasColumnName("BorowID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.RetCount).HasColumnName("RetCount");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");

            // Relationships
            this.HasOptional(t => t.Pro_BorowInfo)
                .WithMany(t => t.Pro_BorowListInfo)
                .HasForeignKey(d => d.BorowID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_BorowListInfo)
                .HasForeignKey(d => d.ProID);

        }
    }
}
