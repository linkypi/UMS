using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class SMS_SignPayInListInfoMap : EntityTypeConfiguration<SMS_SignPayInListInfo>
    {
        public SMS_SignPayInListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SMS_SignPayInListInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SignListID).HasColumnName("SignListID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");

            // Relationships
            this.HasOptional(t => t.Pro_InOrderList)
                .WithMany(t => t.SMS_SignPayInListInfo)
                .HasForeignKey(d => d.InListID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.SMS_SignPayInListInfo)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.SMS_SignSendPayInfo)
                .WithMany(t => t.SMS_SignPayInListInfo)
                .HasForeignKey(d => d.SignListID);

        }
    }
}
