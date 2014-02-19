using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_PersonSellbackCountMap : EntityTypeConfiguration<View_PersonSellbackCount>
    {
        public View_PersonSellbackCountMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProCount, t.ID, t.selllistid });

            // Properties
            this.Property(t => t.ProCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.selllistid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_PersonSellbackCount");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.SellDate).HasColumnName("SellDate");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.selllistid).HasColumnName("selllistid");
        }
    }
}
