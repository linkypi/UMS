using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class SMS_SignSendPayInfoMap : EntityTypeConfiguration<SMS_SignSendPayInfo>
    {
        public SMS_SignSendPayInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Receiver)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SMS_SignSendPayInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.RealPay).HasColumnName("RealPay");
            this.Property(t => t.RealCount).HasColumnName("RealCount");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.PayDate).HasColumnName("PayDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Receiver).HasColumnName("Receiver");

            // Relationships
            this.HasRequired(t => t.SMS_SignInfo)
                .WithMany(t => t.SMS_SignSendPayInfo)
                .HasForeignKey(d => d.SellID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.SMS_SignSendPayInfo)
                .HasForeignKey(d => d.UserID);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.SMS_SignSendPayInfo1)
                .HasForeignKey(d => d.Receiver);

        }
    }
}
