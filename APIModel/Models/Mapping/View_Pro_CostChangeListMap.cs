using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_CostChangeListMap : EntityTypeConfiguration<View_Pro_CostChangeList>
    {
        public View_Pro_CostChangeListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.EndDate, t.RetailPrice });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.RetailPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_Pro_CostChangeList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ChangeID).HasColumnName("ChangeID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.OldCostPrice).HasColumnName("OldCostPrice");
            this.Property(t => t.NewCostPrice).HasColumnName("NewCostPrice");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.UpdateFlag).HasColumnName("UpdateFlag");
            this.Property(t => t.RetailPrice).HasColumnName("RetailPrice");
        }
    }
}
