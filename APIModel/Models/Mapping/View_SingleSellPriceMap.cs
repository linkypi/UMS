using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SingleSellPriceMap : EntityTypeConfiguration<View_SingleSellPrice>
    {
        public View_SingleSellPriceMap()
        {
            // Primary Key
            this.HasKey(t => t.Price);

            // Properties
            this.Property(t => t.Price)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_SingleSellPrice");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.ProID).HasColumnName("ProID");
        }
    }
}
