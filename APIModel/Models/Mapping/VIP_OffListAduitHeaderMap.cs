using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_OffListAduitHeaderMap : EntityTypeConfiguration<VIP_OffListAduitHeader>
    {
        public VIP_OffListAduitHeaderMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Destination)
                .HasMaxLength(500);

            this.Property(t => t.SaleTarget)
                .HasMaxLength(500);

            this.Property(t => t.Applyer)
                .HasMaxLength(50);

            this.Property(t => t.AduitNote1)
                .HasMaxLength(100);

            this.Property(t => t.AduitNote2)
                .HasMaxLength(50);

            this.Property(t => t.ApplyNote)
                .HasMaxLength(50);

            this.Property(t => t.Scope)
                .HasMaxLength(1000);

            this.Property(t => t.Creater)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser1)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser2)
                .HasMaxLength(50);

            this.Property(t => t.Deleter)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser3)
                .HasMaxLength(50);

            this.Property(t => t.AduitNote3)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("VIP_OffListAduitHeader");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Destination).HasColumnName("Destination");
            this.Property(t => t.SaleTarget).HasColumnName("SaleTarget");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Applyer).HasColumnName("Applyer");
            this.Property(t => t.ApplyDate).HasColumnName("ApplyDate");
            this.Property(t => t.Aduited).HasColumnName("Aduited");
            this.Property(t => t.Passed).HasColumnName("Passed");
            this.Property(t => t.Aduited1).HasColumnName("Aduited1");
            this.Property(t => t.Passed1).HasColumnName("Passed1");
            this.Property(t => t.AduitNote1).HasColumnName("AduitNote1");
            this.Property(t => t.AduitDate1).HasColumnName("AduitDate1");
            this.Property(t => t.Aduited2).HasColumnName("Aduited2");
            this.Property(t => t.Passed2).HasColumnName("Passed2");
            this.Property(t => t.AduitNote2).HasColumnName("AduitNote2");
            this.Property(t => t.AduitDate2).HasColumnName("AduitDate2");
            this.Property(t => t.ApplyNote).HasColumnName("ApplyNote");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Scope).HasColumnName("Scope");
            this.Property(t => t.Creater).HasColumnName("Creater");
            this.Property(t => t.AduitUser1).HasColumnName("AduitUser1");
            this.Property(t => t.AduitUser2).HasColumnName("AduitUser2");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.Deleter).HasColumnName("Deleter");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.Aduited3).HasColumnName("Aduited3");
            this.Property(t => t.Passed3).HasColumnName("Passed3");
            this.Property(t => t.AduitUser3).HasColumnName("AduitUser3");
            this.Property(t => t.AduitDate3).HasColumnName("AduitDate3");
            this.Property(t => t.AduitNote3).HasColumnName("AduitNote3");
        }
    }
}
