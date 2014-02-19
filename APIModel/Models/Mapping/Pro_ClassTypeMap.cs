using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ClassTypeMap : EntityTypeConfiguration<Pro_ClassType>
    {
        public Pro_ClassTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ClassTypeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ClassType");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ClassTypeName).HasColumnName("ClassTypeName");
            this.Property(t => t.AsPrice).HasColumnName("AsPrice");
            this.Property(t => t.Note).HasColumnName("Note");
        }
    }
}
