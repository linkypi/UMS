using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_TypeInfoMap : EntityTypeConfiguration<Pro_TypeInfo>
    {
        public Pro_TypeInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.TypeID);

            // Properties
            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ChildFormURL)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_TypeInfo");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ChildFormURL).HasColumnName("ChildFormURL");
        }
    }
}
