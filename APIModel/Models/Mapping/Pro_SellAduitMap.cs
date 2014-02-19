using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellAduitMap : EntityTypeConfiguration<Pro_SellAduit>
    {
        public Pro_SellAduitMap()
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

            this.Property(t => t.CustName)
                .HasMaxLength(50);

            this.Property(t => t.CustPhone)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser2)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser3)
                .HasMaxLength(50);

            this.Property(t => t.Note1)
                .HasMaxLength(250);

            this.Property(t => t.Note2)
                .HasMaxLength(250);

            this.Property(t => t.Note3)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("Pro_SellAduit");
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
            this.Property(t => t.Aduited2).HasColumnName("Aduited2");
            this.Property(t => t.Passed2).HasColumnName("Passed2");
            this.Property(t => t.AduitDate2).HasColumnName("AduitDate2");
            this.Property(t => t.AduitUser2).HasColumnName("AduitUser2");
            this.Property(t => t.Aduited3).HasColumnName("Aduited3");
            this.Property(t => t.Passed3).HasColumnName("Passed3");
            this.Property(t => t.AduitDate3).HasColumnName("AduitDate3");
            this.Property(t => t.AduitUser3).HasColumnName("AduitUser3");
            this.Property(t => t.Aduited1).HasColumnName("Aduited1");
            this.Property(t => t.Passed1).HasColumnName("Passed1");
            this.Property(t => t.Note1).HasColumnName("Note1");
            this.Property(t => t.Note2).HasColumnName("Note2");
            this.Property(t => t.Note3).HasColumnName("Note3");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_SellAduit)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_SellAduit)
                .HasForeignKey(d => d.ApplyUser);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.Pro_SellAduit1)
                .HasForeignKey(d => d.ApplyUser);

        }
    }
}
