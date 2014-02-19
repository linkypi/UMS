using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_VIPType_BakMap : EntityTypeConfiguration<VIP_VIPType_Bak>
    {
        public VIP_VIPType_BakMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.UpdUser)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_VIPType_Bak");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Cost_production).HasColumnName("Cost_production");
            this.Property(t => t.SPoint).HasColumnName("SPoint");
            this.Property(t => t.SBalance).HasColumnName("SBalance");
            this.Property(t => t.Validity).HasColumnName("Validity");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.UpdUser).HasColumnName("UpdUser");
            this.Property(t => t.OldID).HasColumnName("OldID");

            // Relationships
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.VIP_VIPType_Bak)
                .HasForeignKey(d => d.UpdUser);
            this.HasOptional(t => t.VIP_VIPType)
                .WithMany(t => t.VIP_VIPType_Bak)
                .HasForeignKey(d => d.OldID);
            this.HasRequired(t => t.VIP_VIPType_Bak2)
                .WithOptional(t => t.VIP_VIPType_Bak1);

        }
    }
}
