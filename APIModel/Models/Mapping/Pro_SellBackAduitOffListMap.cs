using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellBackAduitOffListMap : EntityTypeConfiguration<Pro_SellBackAduitOffList>
    {
        public Pro_SellBackAduitOffListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Pro_SellBackAduitOffList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.SpecalOffID).HasColumnName("SpecalOffID");
            this.Property(t => t.OffPrice).HasColumnName("OffPrice");

            // Relationships
            this.HasRequired(t => t.Pro_SellBackAduit)
                .WithMany(t => t.Pro_SellBackAduitOffList)
                .HasForeignKey(d => d.AduitID);
            this.HasRequired(t => t.Pro_SellSpecalOffList)
                .WithMany(t => t.Pro_SellBackAduitOffList)
                .HasForeignKey(d => d.SpecalOffID);

        }
    }
}
