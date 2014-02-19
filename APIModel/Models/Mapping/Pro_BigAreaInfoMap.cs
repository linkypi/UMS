using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_BigAreaInfoMap : EntityTypeConfiguration<Pro_BigAreaInfo>
    {
        public Pro_BigAreaInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.BigAreaID);

            // Properties
            this.Property(t => t.BigAreaName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_BigAreaInfo");
            this.Property(t => t.BigAreaID).HasColumnName("BigAreaID");
            this.Property(t => t.BigAreaName).HasColumnName("BigAreaName");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.Note).HasColumnName("Note");
        }
    }
}
