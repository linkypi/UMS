using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_BorowInfoMap : EntityTypeConfiguration<Pro_BorowInfo>
    {
        public Pro_BorowInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.BorowID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Dept)
                .HasMaxLength(50);

            this.Property(t => t.Deleter)
                .HasMaxLength(50);

            this.Property(t => t.Borrower)
                .HasMaxLength(50);

            this.Property(t => t.BorrowType)
                .HasMaxLength(50);

            this.Property(t => t.AduitID)
                .HasMaxLength(50);

            this.Property(t => t.MobilPhone)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_BorowInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.BorowID).HasColumnName("BorowID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.BorowDate).HasColumnName("BorowDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Dept).HasColumnName("Dept");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.Deleter).HasColumnName("Deleter");
            this.Property(t => t.Borrower).HasColumnName("Borrower");
            this.Property(t => t.BorrowType).HasColumnName("BorrowType");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
            this.Property(t => t.MobilPhone).HasColumnName("MobilPhone");
            this.Property(t => t.EstimateReturnTime).HasColumnName("EstimateReturnTime");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_BorowInfo)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_BorowInfo)
                .HasForeignKey(d => d.UserID);
            this.HasRequired(t => t.Pro_BorowInfo2)
                .WithOptional(t => t.Pro_BorowInfo1);

        }
    }
}
