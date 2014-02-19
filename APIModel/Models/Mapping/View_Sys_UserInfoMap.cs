using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Sys_UserInfoMap : EntityTypeConfiguration<View_Sys_UserInfo>
    {
        public View_Sys_UserInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserID, t.RoleID, t.IsLogin, t.Aduit, t.ID, t.IsBoss, t.HasDefault });

            // Properties
            this.Property(t => t.UserID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.UserPwd)
                .HasMaxLength(50);

            this.Property(t => t.RealName)
                .HasMaxLength(50);

            this.Property(t => t.UserIP)
                .HasMaxLength(50);

            this.Property(t => t.UpdUserID)
                .HasMaxLength(50);

            this.Property(t => t.RoleID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.SysTime)
                .HasMaxLength(100);

            this.Property(t => t.DtpName)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.IsLogin)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.OpUpdUser)
                .HasMaxLength(50);

            this.Property(t => t.Aduit)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LeaveTime)
                .HasMaxLength(100);

            this.Property(t => t.RoleName)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.HasDefault)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("View_Sys_UserInfo");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.UserPwd).HasColumnName("UserPwd");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.UserIP).HasColumnName("UserIP");
            this.Property(t => t.DtpID).HasColumnName("DtpID");
            this.Property(t => t.UpdUserID).HasColumnName("UpdUserID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.CanLogin).HasColumnName("CanLogin");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.SysTime).HasColumnName("SysTime");
            this.Property(t => t.CancelLimit).HasColumnName("CancelLimit");
            this.Property(t => t.AduitLimit).HasColumnName("AduitLimit");
            this.Property(t => t.DtpName).HasColumnName("DtpName");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.OpID).HasColumnName("OpID");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.LeaveDate).HasColumnName("LeaveDate");
            this.Property(t => t.IsLogin).HasColumnName("IsLogin");
            this.Property(t => t.OpUpdUser).HasColumnName("OpUpdUser");
            this.Property(t => t.UserFlag).HasColumnName("UserFlag");
            this.Property(t => t.Aduit).HasColumnName("Aduit");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.LeaveTime).HasColumnName("LeaveTime");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.AuditOffPrice).HasColumnName("AuditOffPrice");
            this.Property(t => t.IsBoss).HasColumnName("IsBoss");
            this.Property(t => t.HasDefault).HasColumnName("HasDefault");
            this.Property(t => t.BorowAduitPrice).HasColumnName("BorowAduitPrice");
        }
    }
}
