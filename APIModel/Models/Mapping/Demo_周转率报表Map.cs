using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_周转率报表Map : EntityTypeConfiguration<Demo_周转率报表>
    {
        public Demo_周转率报表Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.区域)
                .HasMaxLength(50);

            this.Property(t => t.级别)
                .HasMaxLength(50);

            this.Property(t => t.仓库)
                .HasMaxLength(50);

            this.Property(t => t.代金_A)
                .HasMaxLength(50);

            this.Property(t => t.增值产品)
                .HasMaxLength(50);

            this.Property(t => t.号码卡)
                .HasMaxLength(50);

            this.Property(t => t.龙终端)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Demo_周转率报表");
            this.Property(t => t.区域).HasColumnName("区域");
            this.Property(t => t.级别).HasColumnName("级别");
            this.Property(t => t.仓库).HasColumnName("仓库");
            this.Property(t => t.代金_A).HasColumnName("代金-A");
            this.Property(t => t.增值产品).HasColumnName("增值产品");
            this.Property(t => t.号码卡).HasColumnName("号码卡");
            this.Property(t => t.龙终端).HasColumnName("龙终端");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
