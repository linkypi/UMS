using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_BorowAduitListMap : EntityTypeConfiguration<Pro_BorowAduitList>
    {
        public Pro_BorowAduitListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_BorowAduitList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.BAduitID).HasColumnName("BAduitID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");

            // Relationships
            this.HasOptional(t => t.Pro_BorowAduit)
                .WithMany(t => t.Pro_BorowAduitList)
                .HasForeignKey(d => d.BAduitID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_BorowAduitList)
                .HasForeignKey(d => d.ProID);

        }
    }
}
