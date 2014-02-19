using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Package_GroupInfoMap : EntityTypeConfiguration<View_Package_GroupInfo>
    {
        public View_Package_GroupInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.GroupName)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SellTypeName)
                .HasMaxLength(50);

            this.Property(t => t.SubNote)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("View_Package_GroupInfo");
            this.Property(t => t.SellTypeID).HasColumnName("SellTypeID");
            this.Property(t => t.IsMust).HasColumnName("IsMust");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.TempOffID).HasColumnName("TempOffID");
            this.Property(t => t.SellTypeName).HasColumnName("SellTypeName");
            this.Property(t => t.SubNote).HasColumnName("SubNote");
        }
    }
}
