using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_GroupTypeInfoMap : EntityTypeConfiguration<View_GroupTypeInfo>
    {
        public View_GroupTypeInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.GroupName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.OldGroupName)
                .HasMaxLength(50);

            this.Property(t => t.OldClassName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_GroupTypeInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.OldGroupName).HasColumnName("OldGroupName");
            this.Property(t => t.OldClassName).HasColumnName("OldClassName");
        }
    }
}
