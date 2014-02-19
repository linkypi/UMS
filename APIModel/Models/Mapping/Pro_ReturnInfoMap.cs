using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ReturnInfoMap : EntityTypeConfiguration<Pro_ReturnInfo>
    {
        public Pro_ReturnInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ReturnID)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Deleter)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ReturnInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ReturnID).HasColumnName("ReturnID");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.ReturnDate).HasColumnName("ReturnDate");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.BorowID).HasColumnName("BorowID");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.Deleter).HasColumnName("Deleter");

            // Relationships
            this.HasOptional(t => t.Pro_BorowInfo)
                .WithMany(t => t.Pro_ReturnInfo)
                .HasForeignKey(d => d.BorowID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_ReturnInfo)
                .HasForeignKey(d => d.UserID);

        }
    }
}
