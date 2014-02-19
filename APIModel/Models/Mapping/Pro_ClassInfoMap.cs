using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ClassInfoMap : EntityTypeConfiguration<Pro_ClassInfo>
    {
        public Pro_ClassInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ClassID);

            // Properties
            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ClassInfo");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.HasSalary).HasColumnName("HasSalary");
            this.Property(t => t.ClassTypeID).HasColumnName("ClassTypeID");

            // Relationships
            this.HasOptional(t => t.Pro_ClassType)
                .WithMany(t => t.Pro_ClassInfo)
                .HasForeignKey(d => d.ClassTypeID);

        }
    }
}
