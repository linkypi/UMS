using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VIPBackMap : EntityTypeConfiguration<VIP_VIPBack>
    {
        public VIP_VIPBackMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_VIPBack");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.VIP_ID).HasColumnName("VIP_ID");
            this.Property(t => t.Return_Money).HasColumnName("Return_Money");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.AduitID).HasColumnName("AduitID");

            // Relationships
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.VIP_VIPBack)
                .HasForeignKey(d => d.UserID);
            this.HasOptional(t => t.VIP_VIPInfo)
                .WithMany(t => t.VIP_VIPBack)
                .HasForeignKey(d => d.VIP_ID);

        }
    }
}
