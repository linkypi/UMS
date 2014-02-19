using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_newspicsMap : EntityTypeConfiguration<VIP_newspics>
    {
        public VIP_newspicsMap()
        {
            // Primary Key
            this.HasKey(t => t.newspicid);

            // Properties
            this.Property(t => t.newspic)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_newspics");
            this.Property(t => t.newspicid).HasColumnName("newspicid");
            this.Property(t => t.newsId).HasColumnName("newsId");
            this.Property(t => t.newspic).HasColumnName("newspic");
            this.Property(t => t.newsindex).HasColumnName("newsindex");
        }
    }
}
