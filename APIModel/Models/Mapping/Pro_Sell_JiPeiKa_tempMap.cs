using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_Sell_JiPeiKa_tempMap : EntityTypeConfiguration<Pro_Sell_JiPeiKa_temp>
    {
        public Pro_Sell_JiPeiKa_tempMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_Sell_JiPeiKa_temp");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");

            // Relationships
            this.HasRequired(t => t.Pro_SellListInfo_Temp)
                .WithMany(t => t.Pro_Sell_JiPeiKa_temp)
                .HasForeignKey(d => d.SellListID);

        }
    }
}
