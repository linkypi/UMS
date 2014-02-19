using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_BorowAduitMap : EntityTypeConfiguration<Pro_BorowAduit>
    {
        public Pro_BorowAduitMap()
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

            this.Property(t => t.Borrower)
                .HasMaxLength(50);

            this.Property(t => t.BorrowType)
                .HasMaxLength(50);

            this.Property(t => t.Dept)
                .HasMaxLength(50);

            this.Property(t => t.MobilPhone)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser2)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser3)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser4)
                .HasMaxLength(50);

            this.Property(t => t.Note1)
                .HasMaxLength(100);

            this.Property(t => t.Note2)
                .HasMaxLength(100);

            this.Property(t => t.Note3)
                .HasMaxLength(100);

            this.Property(t => t.Note4)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Pro_BorowAduit");
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
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Borrower).HasColumnName("Borrower");
            this.Property(t => t.BorrowType).HasColumnName("BorrowType");
            this.Property(t => t.Dept).HasColumnName("Dept");
            this.Property(t => t.MobilPhone).HasColumnName("MobilPhone");
            this.Property(t => t.EstimateReturnTime).HasColumnName("EstimateReturnTime");
            this.Property(t => t.AduitUser2).HasColumnName("AduitUser2");
            this.Property(t => t.AduitDate2).HasColumnName("AduitDate2");
            this.Property(t => t.Aduited2).HasColumnName("Aduited2");
            this.Property(t => t.Passed2).HasColumnName("Passed2");
            this.Property(t => t.AduitUser3).HasColumnName("AduitUser3");
            this.Property(t => t.AduitDate3).HasColumnName("AduitDate3");
            this.Property(t => t.Aduited3).HasColumnName("Aduited3");
            this.Property(t => t.Passed3).HasColumnName("Passed3");
            this.Property(t => t.InternalBorow).HasColumnName("InternalBorow");
            this.Property(t => t.Aduited1).HasColumnName("Aduited1");
            this.Property(t => t.Passed1).HasColumnName("Passed1");
            this.Property(t => t.TotalMoney).HasColumnName("TotalMoney");
            this.Property(t => t.Aduited4).HasColumnName("Aduited4");
            this.Property(t => t.Passed4).HasColumnName("Passed4");
            this.Property(t => t.AduitUser4).HasColumnName("AduitUser4");
            this.Property(t => t.AduitDate4).HasColumnName("AduitDate4");
            this.Property(t => t.Note1).HasColumnName("Note1");
            this.Property(t => t.Note2).HasColumnName("Note2");
            this.Property(t => t.Note3).HasColumnName("Note3");
            this.Property(t => t.Note4).HasColumnName("Note4");
        }
    }
}
