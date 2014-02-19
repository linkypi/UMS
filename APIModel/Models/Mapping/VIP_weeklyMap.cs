using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_weeklyMap : EntityTypeConfiguration<VIP_weekly>
    {
        public VIP_weeklyMap()
        {
            // Primary Key
            this.HasKey(t => t.weeklyId);

            // Properties
            this.Property(t => t.newsIdList)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_weekly");
            this.Property(t => t.weeklyId).HasColumnName("weeklyId");
            this.Property(t => t.weeklyNum).HasColumnName("weeklyNum");
            this.Property(t => t.weeklyDate).HasColumnName("weeklyDate");
            this.Property(t => t.newsIdList).HasColumnName("newsIdList");
        }
    }
}
