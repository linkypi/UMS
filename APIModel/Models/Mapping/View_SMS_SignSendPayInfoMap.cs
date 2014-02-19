using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SMS_SignSendPayInfoMap : EntityTypeConfiguration<View_SMS_SignSendPayInfo>
    {
        public View_SMS_SignSendPayInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.PayDate)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.Receiver)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_SMS_SignSendPayInfo");
            this.Property(t => t.RealPay).HasColumnName("RealPay");
            this.Property(t => t.RealCount).HasColumnName("RealCount");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.PayDate).HasColumnName("PayDate");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Receiver).HasColumnName("Receiver");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
