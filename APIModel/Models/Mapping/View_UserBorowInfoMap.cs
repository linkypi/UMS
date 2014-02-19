using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_UserBorowInfoMap : EntityTypeConfiguration<View_UserBorowInfo>
    {
        public View_UserBorowInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.UnReturnCount);

            // Properties
            this.Property(t => t.BorowDate)
                .HasMaxLength(100);

            this.Property(t => t.ReturnDate)
                .HasMaxLength(100);

            this.Property(t => t.BorowID)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.Borrower)
                .HasMaxLength(50);

            this.Property(t => t.UnReturnCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_UserBorowInfo");
            this.Property(t => t.BorowDate).HasColumnName("BorowDate");
            this.Property(t => t.ReturnDate).HasColumnName("ReturnDate");
            this.Property(t => t.BorowID).HasColumnName("BorowID");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.BorowCount).HasColumnName("BorowCount");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.BorowDays).HasColumnName("BorowDays");
            this.Property(t => t.Borrower).HasColumnName("Borrower");
            this.Property(t => t.UnReturnCount).HasColumnName("UnReturnCount");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
        }
    }
}
