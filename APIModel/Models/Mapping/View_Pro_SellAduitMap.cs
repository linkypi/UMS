using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_SellAduitMap : EntityTypeConfiguration<View_Pro_SellAduit>
    {
        public View_Pro_SellAduitMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Aduited1, t.Passed1, t.Passed, t.Aduited, t.Aduited2, t.Passed2, t.Aduited3, t.Passed3, t.Used, t.Money, t.HasUsed, t.HasAduited2, t.HasAduited3, t.HasAduited, t.HasAduited1, t.HasPassed2, t.HasPassed1, t.HasPassed3, t.HasPassed });

            // Properties
            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.AduitDate)
                .HasMaxLength(100);

            this.Property(t => t.ApplyDate)
                .HasMaxLength(100);

            this.Property(t => t.UseDate)
                .HasMaxLength(100);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser)
                .HasMaxLength(50);

            this.Property(t => t.ApplyUser)
                .HasMaxLength(50);

            this.Property(t => t.Aduited1)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Passed1)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Passed)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Aduited)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Aduited2)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Passed2)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Aduited3)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Passed3)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.AduitDate2)
                .HasMaxLength(100);

            this.Property(t => t.AduitDate3)
                .HasMaxLength(100);

            this.Property(t => t.AduitUser2)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser3)
                .HasMaxLength(50);

            this.Property(t => t.Used)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Money)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.CustName)
                .HasMaxLength(50);

            this.Property(t => t.CustPhone)
                .HasMaxLength(50);

            this.Property(t => t.Note1)
                .HasMaxLength(250);

            this.Property(t => t.Note2)
                .HasMaxLength(250);

            this.Property(t => t.Note3)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("View_Pro_SellAduit");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.AduitDate).HasColumnName("AduitDate");
            this.Property(t => t.ApplyDate).HasColumnName("ApplyDate");
            this.Property(t => t.UseDate).HasColumnName("UseDate");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.AduitUser).HasColumnName("AduitUser");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.ApplyUser).HasColumnName("ApplyUser");
            this.Property(t => t.Aduited1).HasColumnName("Aduited1");
            this.Property(t => t.Passed1).HasColumnName("Passed1");
            this.Property(t => t.Passed).HasColumnName("Passed");
            this.Property(t => t.Aduited).HasColumnName("Aduited");
            this.Property(t => t.Aduited2).HasColumnName("Aduited2");
            this.Property(t => t.Passed2).HasColumnName("Passed2");
            this.Property(t => t.Aduited3).HasColumnName("Aduited3");
            this.Property(t => t.Passed3).HasColumnName("Passed3");
            this.Property(t => t.AduitDate2).HasColumnName("AduitDate2");
            this.Property(t => t.AduitDate3).HasColumnName("AduitDate3");
            this.Property(t => t.AduitUser2).HasColumnName("AduitUser2");
            this.Property(t => t.AduitUser3).HasColumnName("AduitUser3");
            this.Property(t => t.Used).HasColumnName("Used");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Money).HasColumnName("Money");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.CustName).HasColumnName("CustName");
            this.Property(t => t.CustPhone).HasColumnName("CustPhone");
            this.Property(t => t.Note1).HasColumnName("Note1");
            this.Property(t => t.Note2).HasColumnName("Note2");
            this.Property(t => t.Note3).HasColumnName("Note3");
            this.Property(t => t.HasUsed).HasColumnName("HasUsed");
            this.Property(t => t.HasAduited2).HasColumnName("HasAduited2");
            this.Property(t => t.HasAduited3).HasColumnName("HasAduited3");
            this.Property(t => t.HasAduited).HasColumnName("HasAduited");
            this.Property(t => t.HasAduited1).HasColumnName("HasAduited1");
            this.Property(t => t.HasPassed2).HasColumnName("HasPassed2");
            this.Property(t => t.HasPassed1).HasColumnName("HasPassed1");
            this.Property(t => t.HasPassed3).HasColumnName("HasPassed3");
            this.Property(t => t.HasPassed).HasColumnName("HasPassed");
        }
    }
}
