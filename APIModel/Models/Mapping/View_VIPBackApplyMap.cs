using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_VIPBackApplyMap : EntityTypeConfiguration<View_VIPBackApply>
    {
        public View_VIPBackApplyMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Applyed });

            // Properties
            this.Property(t => t.MemberName)
                .HasMaxLength(50);

            this.Property(t => t.MobiPhone)
                .HasMaxLength(50);

            this.Property(t => t.IDCard)
                .HasMaxLength(50);

            this.Property(t => t.StartTime)
                .HasMaxLength(100);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.CardTypeName)
                .HasMaxLength(50);

            this.Property(t => t.VIPType)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.Applyed)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("View_VIPBackApply");
            this.Property(t => t.MemberName).HasColumnName("MemberName");
            this.Property(t => t.MobiPhone).HasColumnName("MobiPhone");
            this.Property(t => t.IDCard_ID).HasColumnName("IDCard_ID");
            this.Property(t => t.IDCard).HasColumnName("IDCard");
            this.Property(t => t.Validity).HasColumnName("Validity");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.CardTypeName).HasColumnName("CardTypeName");
            this.Property(t => t.VIPType).HasColumnName("VIPType");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.Applyed).HasColumnName("Applyed");
        }
    }
}
