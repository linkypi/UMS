using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellTypeProduct_bakMap : EntityTypeConfiguration<Pro_SellTypeProduct_bak>
    {
        public Pro_SellTypeProduct_bakMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellTypeProduct_bak");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.LowPrice).HasColumnName("LowPrice");
            this.Property(t => t.MinPrice).HasColumnName("MinPrice");
            this.Property(t => t.MaxPrice).HasColumnName("MaxPrice");
            this.Property(t => t.IsTicketUseful).HasColumnName("IsTicketUseful");
            this.Property(t => t.IsAduit).HasColumnName("IsAduit");
            this.Property(t => t.ProCost).HasColumnName("ProCost");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_SellTypeProduct_bak)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_SellType)
                .WithMany(t => t.Pro_SellTypeProduct_bak)
                .HasForeignKey(d => d.SellType);

        }
    }
}
