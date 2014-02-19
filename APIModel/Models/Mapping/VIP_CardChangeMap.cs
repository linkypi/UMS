using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_CardChangeMap : EntityTypeConfiguration<VIP_CardChange>
    {
        public VIP_CardChangeMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("VIP_CardChange");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.OLD_VIP_ID).HasColumnName("OLD_VIP_ID");
            this.Property(t => t.NEW_VIP_ID).HasColumnName("NEW_VIP_ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_SellInfo)
                .WithMany(t => t.VIP_CardChange)
                .HasForeignKey(d => d.SellID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.VIP_CardChange)
                .HasForeignKey(d => d.UserID);
            this.HasOptional(t => t.VIP_VIPInfo)
                .WithMany(t => t.VIP_CardChange)
                .HasForeignKey(d => d.NEW_VIP_ID);
            this.HasOptional(t => t.VIP_VIPInfo1)
                .WithMany(t => t.VIP_CardChange1)
                .HasForeignKey(d => d.OLD_VIP_ID);

        }
    }
}
