using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ChangeProListInfoMap : EntityTypeConfiguration<Pro_ChangeProListInfo>
    {
        public Pro_ChangeProListInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ChangeListID);

            // Properties
            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.OldProID)
                .HasMaxLength(50);

            this.Property(t => t.NewProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.NewInListID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_ChangeProListInfo");
            this.Property(t => t.ChangeListID).HasColumnName("ChangeListID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.OldProID).HasColumnName("OldProID");
            this.Property(t => t.NewProID).HasColumnName("NewProID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.ChangeID).HasColumnName("ChangeID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.NewInListID).HasColumnName("NewInListID");
            this.Property(t => t.Flag).HasColumnName("Flag");

            // Relationships
            this.HasOptional(t => t.Pro_ChangeProInfo)
                .WithMany(t => t.Pro_ChangeProListInfo)
                .HasForeignKey(d => d.ChangeID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_ChangeProListInfo)
                .HasForeignKey(d => d.NewProID);
            this.HasOptional(t => t.Pro_ProInfo1)
                .WithMany(t => t.Pro_ChangeProListInfo1)
                .HasForeignKey(d => d.OldProID);

        }
    }
}
