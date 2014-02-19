using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Package_GroupTypeInfoMap : EntityTypeConfiguration<Package_GroupTypeInfo>
    {
        public Package_GroupTypeInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.GroupName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Package_GroupTypeInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
        }
    }
}
