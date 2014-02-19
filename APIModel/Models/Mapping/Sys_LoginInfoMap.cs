using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_LoginInfoMap : EntityTypeConfiguration<Sys_LoginInfo>
    {
        public Sys_LoginInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.LoginID);

            // Properties
            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.LoginIP)
                .HasMaxLength(50);

            this.Property(t => t.LoginOutIP)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_LoginInfo");
            this.Property(t => t.LoginID).HasColumnName("LoginID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.LoginDate).HasColumnName("LoginDate");
            this.Property(t => t.LoginState).HasColumnName("LoginState");
            this.Property(t => t.QuitDate).HasColumnName("QuitDate");
            this.Property(t => t.LoginIP).HasColumnName("LoginIP");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.LoginOutID).HasColumnName("LoginOutID");
            this.Property(t => t.LoginOutIP).HasColumnName("LoginOutIP");
        }
    }
}
