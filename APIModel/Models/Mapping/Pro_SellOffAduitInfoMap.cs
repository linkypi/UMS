using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellOffAduitInfoMap : EntityTypeConfiguration<Pro_SellOffAduitInfo>
    {
        public Pro_SellOffAduitInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ApplyUserID)
                .HasMaxLength(50);

            this.Property(t => t.AduitUserID)
                .HasMaxLength(50);

            this.Property(t => t.ApplyNote)
                .HasMaxLength(50);

            this.Property(t => t.AduitNote)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellOffAduitInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.BackID).HasColumnName("BackID");
            this.Property(t => t.ApplyUserID).HasColumnName("ApplyUserID");
            this.Property(t => t.ApplyDate).HasColumnName("ApplyDate");
            this.Property(t => t.AduitUserID).HasColumnName("AduitUserID");
            this.Property(t => t.AduitDate).HasColumnName("AduitDate");
            this.Property(t => t.IsPass).HasColumnName("IsPass");
            this.Property(t => t.IsAduited).HasColumnName("IsAduited");
            this.Property(t => t.ApplyNote).HasColumnName("ApplyNote");
            this.Property(t => t.AduitNote).HasColumnName("AduitNote");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.NextPrice).HasColumnName("NextPrice");

            // Relationships
            this.HasRequired(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_SellOffAduitInfo)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Pro_SellBack)
                .WithMany(t => t.Pro_SellOffAduitInfo)
                .HasForeignKey(d => d.BackID);
            this.HasOptional(t => t.Pro_SellInfo)
                .WithMany(t => t.Pro_SellOffAduitInfo)
                .HasForeignKey(d => d.SellID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_SellOffAduitInfo)
                .HasForeignKey(d => d.ApplyUserID);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.Pro_SellOffAduitInfo1)
                .HasForeignKey(d => d.AduitUserID);

        }
    }
}
