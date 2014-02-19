using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class SMS_SellBackMap : EntityTypeConfiguration<SMS_SellBack>
    {
        public SMS_SellBackMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.SellBackID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            this.Property(t => t.BillID)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            this.Property(t => t.CusPhone)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SMS_SellBack");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellBackID).HasColumnName("SellBackID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.BackMoney).HasColumnName("BackMoney");
            this.Property(t => t.BackCount).HasColumnName("BackCount");
            this.Property(t => t.BillID).HasColumnName("BillID");
            this.Property(t => t.CusName).HasColumnName("CusName");
            this.Property(t => t.CusPhone).HasColumnName("CusPhone");
        }
    }
}
