using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Rules_AllCurrentRulesInfoMap : EntityTypeConfiguration<Rules_AllCurrentRulesInfo>
    {
        public Rules_AllCurrentRulesInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.RulesName)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Rules_AllCurrentRulesInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RulesID).HasColumnName("RulesID");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.RulesTypeID).HasColumnName("RulesTypeID");
            this.Property(t => t.RulesName).HasColumnName("RulesName");
            this.Property(t => t.ShowToCus).HasColumnName("ShowToCus");
            this.Property(t => t.CanGetBack).HasColumnName("CanGetBack");
            this.Property(t => t.ProMainID).HasColumnName("ProMainID");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.OffPrice).HasColumnName("OffPrice");
            this.Property(t => t.MaxPrice).HasColumnName("MaxPrice");
            this.Property(t => t.MinPrice).HasColumnName("MinPrice");
            this.Property(t => t.OrderBy).HasColumnName("OrderBy");
            this.Property(t => t.Salay).HasColumnName("Salay");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Rules_ProMain_ID).HasColumnName("Rules_ProMain_ID");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Rules_AllCurrentRulesInfo)
                .HasForeignKey(d => d.HallID);

        }
    }
}
