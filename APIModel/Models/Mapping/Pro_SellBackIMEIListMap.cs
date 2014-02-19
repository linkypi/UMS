using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellBackIMEIListMap : EntityTypeConfiguration<Pro_SellBackIMEIList>
    {
        public Pro_SellBackIMEIListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellBackIMEIList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellBackListID).HasColumnName("SellBackListID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.Pro_SellBackList)
                .WithMany(t => t.Pro_SellBackIMEIList)
                .HasForeignKey(d => d.SellBackListID);

        }
    }
}
