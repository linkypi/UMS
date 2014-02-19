using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_RenewBackAduitMap : EntityTypeConfiguration<VIP_RenewBackAduit>
    {
        public VIP_RenewBackAduitMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser)
                .HasMaxLength(50);

            this.Property(t => t.ApplyUser)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_RenewBackAduit");
            this.Property(t => t.ID).HasColumnName("ID");
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
            this.Property(t => t.Money).HasColumnName("Money");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.Validity).HasColumnName("Validity");
            this.Property(t => t.VIP_ID).HasColumnName("VIP_ID");
            this.Property(t => t.ReNewID).HasColumnName("ReNewID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.NewDate).HasColumnName("NewDate");
            this.Property(t => t.UserID).HasColumnName("UserID");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.VIP_RenewBackAduit)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.VIP_RenewBackAduit)
                .HasForeignKey(d => d.ApplyUser);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.VIP_RenewBackAduit1)
                .HasForeignKey(d => d.AduitUser);
            this.HasOptional(t => t.Sys_UserInfo2)
                .WithMany(t => t.VIP_RenewBackAduit2)
                .HasForeignKey(d => d.UserID);
            this.HasOptional(t => t.VIP_Renew)
                .WithMany(t => t.VIP_RenewBackAduit)
                .HasForeignKey(d => d.ReNewID);
            this.HasRequired(t => t.VIP_RenewBackAduit2)
                .WithOptional(t => t.VIP_RenewBackAduit1);
            this.HasRequired(t => t.VIP_RenewBackAduit3)
                .WithOptional(t => t.VIP_RenewBackAduit11);

        }
    }
}
