using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_UserInfoMap : EntityTypeConfiguration<Sys_UserInfo>
    {
        public Sys_UserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.UserID);

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

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_UserInfo");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.UserPwd).HasColumnName("UserPwd");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.UserIP).HasColumnName("UserIP");
            this.Property(t => t.DtpID).HasColumnName("DtpID");
            this.Property(t => t.UpdUserID).HasColumnName("UpdUserID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.CanLogin).HasColumnName("CanLogin");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.CancelLimit).HasColumnName("CancelLimit");
            this.Property(t => t.AduitLimit).HasColumnName("AduitLimit");
            this.Property(t => t.UpTime).HasColumnName("UpTime");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.AuditOffPrice).HasColumnName("AuditOffPrice");
            this.Property(t => t.IsBoss).HasColumnName("IsBoss");
            this.Property(t => t.BrwLimit).HasColumnName("BrwLimit");
            this.Property(t => t.BorowAduitPrice).HasColumnName("BorowAduitPrice");

            // Relationships
            this.HasOptional(t => t.Sys_DeptInfo)
                .WithMany(t => t.Sys_UserInfo)
                .HasForeignKey(d => d.DtpID);
            this.HasRequired(t => t.Sys_RoleInfo)
                .WithMany(t => t.Sys_UserInfo)
                .HasForeignKey(d => d.RoleID);

        }
    }
}
