using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_ReturnInfoMap : EntityTypeConfiguration<View_ReturnInfo>
    {
        public View_ReturnInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ReturnID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.ReturnDate)
                .HasMaxLength(100);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.DeleteDate)
                .HasMaxLength(100);

            this.Property(t => t.Deleter)
                .HasMaxLength(50);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Borrower)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_ReturnInfo");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ReturnID).HasColumnName("ReturnID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.ReturnDate).HasColumnName("ReturnDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.BorowID).HasColumnName("BorowID");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.Deleter).HasColumnName("Deleter");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Borrower).HasColumnName("Borrower");
        }
    }
}
