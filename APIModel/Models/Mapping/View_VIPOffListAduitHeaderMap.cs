using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_VIPOffListAduitHeaderMap : EntityTypeConfiguration<View_VIPOffListAduitHeader>
    {
        public View_VIPOffListAduitHeaderMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Aduited, t.Passed, t.Aduited2, t.Passed2, t.Aduited3, t.Passed3, t.Aduited1, t.Passed1, t.ExtNote, t.HasAduited, t.HasPassed, t.HasAduited1, t.HasPassed1, t.HasAduited2, t.HasPassed2, t.HasAduited3, t.HasPassed3 });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Destination)
                .HasMaxLength(500);

            this.Property(t => t.SaleTarget)
                .HasMaxLength(500);

            this.Property(t => t.StartDate)
                .HasMaxLength(100);

            this.Property(t => t.Applyer)
                .HasMaxLength(50);

            this.Property(t => t.ApplyDate)
                .HasMaxLength(100);

            this.Property(t => t.Aduited)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Passed)
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

            this.Property(t => t.Aduited1)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Passed1)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.AduitNote1)
                .HasMaxLength(100);

            this.Property(t => t.AduitDate1)
                .HasMaxLength(100);

            this.Property(t => t.AduitNote2)
                .HasMaxLength(50);

            this.Property(t => t.AduitDate2)
                .HasMaxLength(100);

            this.Property(t => t.ApplyNote)
                .HasMaxLength(50);

            this.Property(t => t.EndDate)
                .HasMaxLength(100);

            this.Property(t => t.Creater)
                .HasMaxLength(50);

            this.Property(t => t.DeadLine)
                .HasMaxLength(206);

            this.Property(t => t.Scope)
                .HasMaxLength(1000);

            this.Property(t => t.ExtNote)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.AduitNote3)
                .HasMaxLength(150);

            this.Property(t => t.AduitDate3)
                .HasMaxLength(100);

            this.Property(t => t.AduitUser3)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_VIPOffListAduitHeader");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Destination).HasColumnName("Destination");
            this.Property(t => t.SaleTarget).HasColumnName("SaleTarget");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Applyer).HasColumnName("Applyer");
            this.Property(t => t.ApplyDate).HasColumnName("ApplyDate");
            this.Property(t => t.Aduited).HasColumnName("Aduited");
            this.Property(t => t.Passed).HasColumnName("Passed");
            this.Property(t => t.Aduited2).HasColumnName("Aduited2");
            this.Property(t => t.Passed2).HasColumnName("Passed2");
            this.Property(t => t.Aduited3).HasColumnName("Aduited3");
            this.Property(t => t.Passed3).HasColumnName("Passed3");
            this.Property(t => t.Aduited1).HasColumnName("Aduited1");
            this.Property(t => t.Passed1).HasColumnName("Passed1");
            this.Property(t => t.AduitNote1).HasColumnName("AduitNote1");
            this.Property(t => t.AduitDate1).HasColumnName("AduitDate1");
            this.Property(t => t.AduitNote2).HasColumnName("AduitNote2");
            this.Property(t => t.AduitDate2).HasColumnName("AduitDate2");
            this.Property(t => t.ApplyNote).HasColumnName("ApplyNote");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Creater).HasColumnName("Creater");
            this.Property(t => t.DeadLine).HasColumnName("DeadLine");
            this.Property(t => t.Scope).HasColumnName("Scope");
            this.Property(t => t.ExtNote).HasColumnName("ExtNote");
            this.Property(t => t.AduitNote3).HasColumnName("AduitNote3");
            this.Property(t => t.AduitDate3).HasColumnName("AduitDate3");
            this.Property(t => t.AduitUser3).HasColumnName("AduitUser3");
            this.Property(t => t.HasAduited).HasColumnName("HasAduited");
            this.Property(t => t.HasPassed).HasColumnName("HasPassed");
            this.Property(t => t.HasAduited1).HasColumnName("HasAduited1");
            this.Property(t => t.HasPassed1).HasColumnName("HasPassed1");
            this.Property(t => t.HasAduited2).HasColumnName("HasAduited2");
            this.Property(t => t.HasPassed2).HasColumnName("HasPassed2");
            this.Property(t => t.HasAduited3).HasColumnName("HasAduited3");
            this.Property(t => t.HasPassed3).HasColumnName("HasPassed3");
        }
    }
}
