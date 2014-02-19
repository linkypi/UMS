using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellAduit_bakMap : EntityTypeConfiguration<Pro_SellAduit_bak>
    {
        public Pro_SellAduit_bakMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

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

            this.Property(t => t.CustName)
                .HasMaxLength(50);

            this.Property(t => t.CustPhone)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellAduit_bak");
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
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.CustName).HasColumnName("CustName");
            this.Property(t => t.CustPhone).HasColumnName("CustPhone");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_SellAduit_bak)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_SellAduit_bak)
                .HasForeignKey(d => d.ApplyUser);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.Pro_SellAduit_bak1)
                .HasForeignKey(d => d.ApplyUser);

        }
    }
}
