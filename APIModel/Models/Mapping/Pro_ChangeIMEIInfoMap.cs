using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_ChangeIMEIInfoMap : EntityTypeConfiguration<Pro_ChangeIMEIInfo>
    {
        public Pro_ChangeIMEIInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Pro_ChangeIMEIInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ChangeListID).HasColumnName("ChangeListID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_ChangeProListInfo)
                .WithMany(t => t.Pro_ChangeIMEIInfo)
                .HasForeignKey(d => d.ChangeListID);

        }
    }
}
