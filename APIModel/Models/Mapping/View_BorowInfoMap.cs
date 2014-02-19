using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_BorowInfoMap : EntityTypeConfiguration<View_BorowInfo>
    {
        public View_BorowInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IsReturn, t.ID });

            // Properties
            this.Property(t => t.IsReturn)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            this.Property(t => t.BorrowType)
                .HasMaxLength(50);

            this.Property(t => t.Borrower)
                .HasMaxLength(50);

            this.Property(t => t.Dept)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.BorowDate)
                .HasMaxLength(100);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.BorowID)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_BorowInfo");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.BorrowType).HasColumnName("BorrowType");
            this.Property(t => t.Borrower).HasColumnName("Borrower");
            this.Property(t => t.Dept).HasColumnName("Dept");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.BorowDate).HasColumnName("BorowDate");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.BorowID).HasColumnName("BorowID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
        }
    }
}
