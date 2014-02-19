using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_BorowReturnInfoMap : EntityTypeConfiguration<View_BorowReturnInfo>
    {
        public View_BorowReturnInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IsReturn, t.ID });

            // Properties
            this.Property(t => t.BorowID)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.Dept)
                .HasMaxLength(50);

            this.Property(t => t.Borrower)
                .HasMaxLength(50);

            this.Property(t => t.BorrowType)
                .HasMaxLength(50);

            this.Property(t => t.IsReturn)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.BorowDate)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("View_BorowReturnInfo");
            this.Property(t => t.BorowID).HasColumnName("BorowID");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.Dept).HasColumnName("Dept");
            this.Property(t => t.Borrower).HasColumnName("Borrower");
            this.Property(t => t.BorrowType).HasColumnName("BorrowType");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.BorowDate).HasColumnName("BorowDate");
        }
    }
}
