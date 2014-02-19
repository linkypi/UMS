using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_ProInfoMap : EntityTypeConfiguration<Demo_ProInfo>
    {
        public Demo_ProInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ProID);

            // Properties
            this.Property(t => t.ProClass)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_ProInfo");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProClass).HasColumnName("ProClass");
            this.Property(t => t.ProType).HasColumnName("ProType");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");
        }
    }
}
