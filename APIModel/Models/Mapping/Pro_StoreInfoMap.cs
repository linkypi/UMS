using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_StoreInfoMap : EntityTypeConfiguration<Pro_StoreInfo>
    {
        public Pro_StoreInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_StoreInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProID).HasColumnName("ProID");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Pro_StoreInfo)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Pro_InOrderList)
                .WithMany(t => t.Pro_StoreInfo)
                .HasForeignKey(d => d.InListID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_StoreInfo)
                .HasForeignKey(d => d.ProID);

        }
    }
}
