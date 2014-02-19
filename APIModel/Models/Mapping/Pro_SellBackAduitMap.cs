using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellBackAduitMap : EntityTypeConfiguration<Pro_SellBackAduit>
    {
        public Pro_SellBackAduitMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.AduitUser)
                .HasMaxLength(50);

            this.Property(t => t.ApplyUser)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            this.Property(t => t.CusName)
                .HasMaxLength(50);

            this.Property(t => t.CusPhone)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellBackAduit");
            this.Property(t => t.ID).HasColumnName("ID");
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
            this.Property(t => t.AduitMoney).HasColumnName("AduitMoney");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.ApplyMoney).HasColumnName("ApplyMoney");
            this.Property(t => t.CusName).HasColumnName("CusName");
            this.Property(t => t.CusPhone).HasColumnName("CusPhone");
            this.Property(t => t.VIPID).HasColumnName("VIPID");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_SellBackAduit)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_SellBackAduit)
                .HasForeignKey(d => d.AduitUser);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.Pro_SellBackAduit1)
                .HasForeignKey(d => d.ApplyUser);
            this.HasOptional(t => t.Pro_SellInfo)
                .WithMany(t => t.Pro_SellBackAduit)
                .HasForeignKey(d => d.SellID);
            this.HasOptional(t => t.VIP_VIPInfo)
                .WithMany(t => t.Pro_SellBackAduit)
                .HasForeignKey(d => d.VIPID);

        }
    }
}
