using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VIPInfo_TempMap : EntityTypeConfiguration<VIP_VIPInfo_Temp>
    {
        public VIP_VIPInfo_TempMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.MemberName)
                .HasMaxLength(50);

            this.Property(t => t.Sex)
                .HasMaxLength(10);

            this.Property(t => t.MobiPhone)
                .HasMaxLength(50);

            this.Property(t => t.TelePhone)
                .HasMaxLength(50);

            this.Property(t => t.QQ)
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(150);

            this.Property(t => t.IDCard)
                .HasMaxLength(50);

            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.UpdUser)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Password)
                .HasMaxLength(50);

            this.Property(t => t.userName)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.LZUser)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_VIPInfo_Temp");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.MemberName).HasColumnName("MemberName");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.MobiPhone).HasColumnName("MobiPhone");
            this.Property(t => t.TelePhone).HasColumnName("TelePhone");
            this.Property(t => t.QQ).HasColumnName("QQ");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.IDCard_ID).HasColumnName("IDCard_ID");
            this.Property(t => t.IDCard).HasColumnName("IDCard");
            this.Property(t => t.Validity).HasColumnName("Validity");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.Balance).HasColumnName("Balance");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.UpdUser).HasColumnName("UpdUser");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.userName).HasColumnName("userName");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.LZUser).HasColumnName("LZUser");

            // Relationships
            this.HasOptional(t => t.Pro_SellListInfo_Temp)
                .WithMany(t => t.VIP_VIPInfo_Temp)
                .HasForeignKey(d => d.SellListID);

        }
    }
}
