using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SellOffAduitInfoListMap : EntityTypeConfiguration<View_SellOffAduitInfoList>
    {
        public View_SellOffAduitInfoListMap()
        {
            // Primary Key
            this.HasKey(t => t.AduitID);

            // Properties
            this.Property(t => t.AduitDate)
                .HasMaxLength(100);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser)
                .HasMaxLength(50);

            this.Property(t => t.AduitID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_SellOffAduitInfoList");
            this.Property(t => t.IsPass).HasColumnName("IsPass");
            this.Property(t => t.AduitDate).HasColumnName("AduitDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.AduitUser).HasColumnName("AduitUser");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
        }
    }
}
