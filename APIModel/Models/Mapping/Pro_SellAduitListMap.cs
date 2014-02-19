using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellAduitListMap : EntityTypeConfiguration<Pro_SellAduitList>
    {
        public Pro_SellAduitListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Pro_SellAduitList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellAuditID).HasColumnName("SellAuditID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.OffMoney).HasColumnName("OffMoney");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");
            this.Property(t => t.SellTypeID).HasColumnName("SellTypeID");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_SellAduitList)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_SellAduit)
                .WithMany(t => t.Pro_SellAduitList)
                .HasForeignKey(d => d.SellAuditID);

        }
    }
}
