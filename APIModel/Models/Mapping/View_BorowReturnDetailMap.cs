using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_BorowReturnDetailMap : EntityTypeConfiguration<View_BorowReturnDetail>
    {
        public View_BorowReturnDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IsReturn, t.BorowListID, t.RetCount, t.UnRetCount });

            // Properties
            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.IsReturn)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.BorowListID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.BID)
                .HasMaxLength(50);

            this.Property(t => t.RetCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UnRetCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_BorowReturnDetail");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.BorowListID).HasColumnName("BorowListID");
            this.Property(t => t.BorowID).HasColumnName("BorowID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.NeedIMEI).HasColumnName("NeedIMEI");
            this.Property(t => t.BID).HasColumnName("BID");
            this.Property(t => t.RetCount).HasColumnName("RetCount");
            this.Property(t => t.UnRetCount).HasColumnName("UnRetCount");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
        }
    }
}
