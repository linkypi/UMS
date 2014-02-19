using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Package_GroupInfoMap : EntityTypeConfiguration<Package_GroupInfo>
    {
        public Package_GroupInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.GroupName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.SubNote)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Package_GroupInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.GroupID).HasColumnName("GroupID");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.IsMust).HasColumnName("IsMust");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.TempOffID).HasColumnName("TempOffID");
            this.Property(t => t.SubNote).HasColumnName("SubNote");

            // Relationships
            this.HasRequired(t => t.Package_GroupInfo2)
                .WithOptional(t => t.Package_GroupInfo1);
            this.HasOptional(t => t.Package_GroupTypeInfo)
                .WithMany(t => t.Package_GroupInfo)
                .HasForeignKey(d => d.GroupID);
            this.HasOptional(t => t.Pro_SellType)
                .WithMany(t => t.Package_GroupInfo)
                .HasForeignKey(d => d.SellType);
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.Package_GroupInfo)
                .HasForeignKey(d => d.OffID);
            this.HasOptional(t => t.VIP_OffListAduit)
                .WithMany(t => t.Package_GroupInfo)
                .HasForeignKey(d => d.TempOffID);

        }
    }
}
