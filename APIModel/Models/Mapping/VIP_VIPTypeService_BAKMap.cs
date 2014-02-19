using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VIPTypeService_BAKMap : EntityTypeConfiguration<VIP_VIPTypeService_BAK>
    {
        public VIP_VIPTypeService_BAKMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.UpdUser)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_VIPTypeService_BAK");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.SCount).HasColumnName("SCount");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.UpdUser).HasColumnName("UpdUser");
            this.Property(t => t.OldID).HasColumnName("OldID");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.VIP_VIPTypeService_BAK)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.VIP_VIPTypeService_BAK)
                .HasForeignKey(d => d.UpdUser);
            this.HasOptional(t => t.VIP_VIPType)
                .WithMany(t => t.VIP_VIPTypeService_BAK)
                .HasForeignKey(d => d.TypeID);
            this.HasOptional(t => t.VIP_VIPType_Bak)
                .WithMany(t => t.VIP_VIPTypeService_BAK)
                .HasForeignKey(d => d.TypeID);
            this.HasOptional(t => t.VIP_VIPTypeService)
                .WithMany(t => t.VIP_VIPTypeService_BAK)
                .HasForeignKey(d => d.OldID);

        }
    }
}
