using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class SMS_SignSendPayInfo_DeleteMap : EntityTypeConfiguration<SMS_SignSendPayInfo_Delete>
    {
        public SMS_SignSendPayInfo_DeleteMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SMS_SignSendPayInfo_Delete");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.RealPay).HasColumnName("RealPay");
            this.Property(t => t.RealCount).HasColumnName("RealCount");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");

            // Relationships
            this.HasOptional(t => t.SMS_SignInfo)
                .WithMany(t => t.SMS_SignSendPayInfo_Delete)
                .HasForeignKey(d => d.SellID);

        }
    }
}
