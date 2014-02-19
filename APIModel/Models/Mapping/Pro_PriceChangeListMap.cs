using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_PriceChangeListMap : EntityTypeConfiguration<Pro_PriceChangeList>
    {
        public Pro_PriceChangeListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_PriceChangeList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ChangeID).HasColumnName("ChangeID");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.LowPrice).HasColumnName("LowPrice");
            this.Property(t => t.MinPrice).HasColumnName("MinPrice");
            this.Property(t => t.MaxPrice).HasColumnName("MaxPrice");
            this.Property(t => t.IsTicketUseful).HasColumnName("IsTicketUseful");
            this.Property(t => t.IsAduit).HasColumnName("IsAduit");

            // Relationships
            this.HasOptional(t => t.Pro_PriceChange)
                .WithMany(t => t.Pro_PriceChangeList)
                .HasForeignKey(d => d.ChangeID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_PriceChangeList)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_SellType)
                .WithMany(t => t.Pro_PriceChangeList)
                .HasForeignKey(d => d.SellType);

        }
    }
}
