using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellBackSpecalOffListMap : EntityTypeConfiguration<Pro_SellBackSpecalOffList>
    {
        public Pro_SellBackSpecalOffListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellBackSpecalOffList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.BackID).HasColumnName("BackID");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.OffPrice).HasColumnName("OffPrice");

            // Relationships
            this.HasOptional(t => t.Pro_SellBack)
                .WithMany(t => t.Pro_SellBackSpecalOffList)
                .HasForeignKey(d => d.BackID);
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.Pro_SellBackSpecalOffList)
                .HasForeignKey(d => d.OffID);

        }
    }
}
