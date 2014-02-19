using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_VIP_RenewBackAduitMap : EntityTypeConfiguration<View_VIP_RenewBackAduit>
    {
        public View_VIP_RenewBackAduitMap()
        {
            // Primary Key
            this.HasKey(t => new { t.AduitUser, t.AduitDate, t.Aduited, t.Passed, t.Used, t.UseDate, t.IDCard_ID, t.CurrentValidity, t.Point, t.CardTypeName, t.HallName, t.RenewMoney, t.RenewTypeName, t.RenewValidity, t.RenewPoint, t.ID, t.RenewDate, t.NewEndTime, t.OldStartTime, t.EndTime, t.AduitMoney, t.AduitPoint, t.BackValidity });

            // Properties
            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AduitDate)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ApplyUser)
                .HasMaxLength(50);

            this.Property(t => t.ApplyDate)
                .HasMaxLength(100);

            this.Property(t => t.Aduited)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Passed)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Used)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.UseDate)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.MemberName)
                .HasMaxLength(50);

            this.Property(t => t.MobiPhone)
                .HasMaxLength(50);

            this.Property(t => t.IDCard_ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IDCard)
                .HasMaxLength(50);

            this.Property(t => t.CurrentValidity)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.StartTime)
                .HasMaxLength(100);

            this.Property(t => t.Point)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.VIPType)
                .HasMaxLength(50);

            this.Property(t => t.CardTypeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RenewMoney)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RenewTypeName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.RenewValidity)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RenewPoint)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RenewDate)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.NewEndTime)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Sex)
                .HasMaxLength(10);

            this.Property(t => t.AduitMoney)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AduitPoint)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BackValidity)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OldEndDate)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("View_VIP_RenewBackAduit");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.AduitUser).HasColumnName("AduitUser");
            this.Property(t => t.AduitDate).HasColumnName("AduitDate");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.ApplyUser).HasColumnName("ApplyUser");
            this.Property(t => t.ApplyDate).HasColumnName("ApplyDate");
            this.Property(t => t.Aduited).HasColumnName("Aduited");
            this.Property(t => t.Passed).HasColumnName("Passed");
            this.Property(t => t.Used).HasColumnName("Used");
            this.Property(t => t.UseDate).HasColumnName("UseDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.VIP_ID).HasColumnName("VIP_ID");
            this.Property(t => t.ReNewID).HasColumnName("ReNewID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.MemberName).HasColumnName("MemberName");
            this.Property(t => t.MobiPhone).HasColumnName("MobiPhone");
            this.Property(t => t.IDCard_ID).HasColumnName("IDCard_ID");
            this.Property(t => t.IDCard).HasColumnName("IDCard");
            this.Property(t => t.CurrentValidity).HasColumnName("CurrentValidity");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.VIPType).HasColumnName("VIPType");
            this.Property(t => t.CardTypeName).HasColumnName("CardTypeName");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.RenewMoney).HasColumnName("RenewMoney");
            this.Property(t => t.RenewTypeName).HasColumnName("RenewTypeName");
            this.Property(t => t.RenewValidity).HasColumnName("RenewValidity");
            this.Property(t => t.RenewPoint).HasColumnName("RenewPoint");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RenewDate).HasColumnName("RenewDate");
            this.Property(t => t.NewEndTime).HasColumnName("NewEndTime");
            this.Property(t => t.OldStartTime).HasColumnName("OldStartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.AduitMoney).HasColumnName("AduitMoney");
            this.Property(t => t.AduitPoint).HasColumnName("AduitPoint");
            this.Property(t => t.BackValidity).HasColumnName("BackValidity");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.OldEndDate).HasColumnName("OldEndDate");
        }
    }
}
