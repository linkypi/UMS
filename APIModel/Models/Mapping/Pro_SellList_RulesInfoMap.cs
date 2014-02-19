using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellList_RulesInfoMap : EntityTypeConfiguration<Pro_SellList_RulesInfo>
    {
        public Pro_SellList_RulesInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Pro_SellList_RulesInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.Rules_ProMain_ID).HasColumnName("Rules_ProMain_ID");
            this.Property(t => t.OffPrice).HasColumnName("OffPrice");
            this.Property(t => t.RealPrice).HasColumnName("RealPrice");
            this.Property(t => t.ShowToCus).HasColumnName("ShowToCus");
            this.Property(t => t.CanGetBack).HasColumnName("CanGetBack");

            // Relationships
            this.HasOptional(t => t.Pro_SellListInfo)
                .WithMany(t => t.Pro_SellList_RulesInfo)
                .HasForeignKey(d => d.SellListID);
            this.HasOptional(t => t.Rules_Pro_RulesTypeInfo)
                .WithMany(t => t.Pro_SellList_RulesInfo)
                .HasForeignKey(d => d.Rules_ProMain_ID);

        }
    }
}
