using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_newsMap : EntityTypeConfiguration<VIP_news>
    {
        public VIP_newsMap()
        {
            // Primary Key
            this.HasKey(t => t.newsId);

            // Properties
            this.Property(t => t.newsTitle)
                .HasMaxLength(100);

            this.Property(t => t.newsAbstract)
                .HasMaxLength(250);

            this.Property(t => t.newsContent)
                .HasMaxLength(300);

            this.Property(t => t.newsAuthor)
                .HasMaxLength(50);

            this.Property(t => t.newsSfrom)
                .HasMaxLength(50);

            this.Property(t => t.newsSpic)
                .HasMaxLength(100);

            this.Property(t => t.newsPicList)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_news");
            this.Property(t => t.newsId).HasColumnName("newsId");
            this.Property(t => t.newsTitle).HasColumnName("newsTitle");
            this.Property(t => t.newsAbstract).HasColumnName("newsAbstract");
            this.Property(t => t.newsContent).HasColumnName("newsContent");
            this.Property(t => t.newsAuthor).HasColumnName("newsAuthor");
            this.Property(t => t.newsSfrom).HasColumnName("newsSfrom");
            this.Property(t => t.newsSpic).HasColumnName("newsSpic");
            this.Property(t => t.newsPicList).HasColumnName("newsPicList");
            this.Property(t => t.newsCreatetime).HasColumnName("newsCreatetime");
            this.Property(t => t.weeklyId).HasColumnName("weeklyId");
        }
    }
}
