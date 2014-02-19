using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_OtherSellBackCountMap : EntityTypeConfiguration<View_OtherSellBackCount>
    {
        public View_OtherSellBackCountMap()
        {
            // Primary Key
            this.HasKey(t => t.ProCount);

            // Properties
            this.Property(t => t.ProCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_OtherSellBackCount");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.SellDate).HasColumnName("SellDate");
            this.Property(t => t.HallID).HasColumnName("HallID");
        }
    }
}
