using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Demo_提成报表Map : EntityTypeConfiguration<Demo_提成报表>
    {
        public Demo_提成报表Map()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.营销员)
                .HasMaxLength(500);

            this.Property(t => t.职位)
                .HasMaxLength(500);

            this.Property(t => t.仓库)
                .HasMaxLength(500);

            this.Property(t => t.片区)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Demo_提成报表");
            this.Property(t => t.日期).HasColumnName("日期");
            this.Property(t => t.营销员).HasColumnName("营销员");
            this.Property(t => t.职位).HasColumnName("职位");
            this.Property(t => t.仓库).HasColumnName("仓库");
            this.Property(t => t.片区).HasColumnName("片区");
            this.Property(t => t.广信延保销售数量).HasColumnName("广信延保销售数量");
            this.Property(t => t.广信延保提成).HasColumnName("广信延保提成");
            this.Property(t => t.本人销售数量).HasColumnName("本人销售数量");
            this.Property(t => t.本人销售提成).HasColumnName("本人销售提成");
            this.Property(t => t.本人销售退机).HasColumnName("本人销售退机");
            this.Property(t => t.本人销售退机金额).HasColumnName("本人销售退机金额");
            this.Property(t => t.非本人销售数量).HasColumnName("非本人销售数量");
            this.Property(t => t.非本人销售提成).HasColumnName("非本人销售提成");
            this.Property(t => t.非本人销售退机).HasColumnName("非本人销售退机");
            this.Property(t => t.非本人销售退机金额).HasColumnName("非本人销售退机金额");
            this.Property(t => t.终端提成总额).HasColumnName("终端提成总额");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
