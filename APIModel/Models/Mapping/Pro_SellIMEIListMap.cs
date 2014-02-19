using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellIMEIListMap : EntityTypeConfiguration<Pro_SellIMEIList>
    {
        public Pro_SellIMEIListMap()
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
            this.ToTable("Pro_SellIMEIList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.CashPrice).HasColumnName("CashPrice");
            this.Property(t => t.SellSpecalID).HasColumnName("SellSpecalID");

            // Relationships
            this.HasOptional(t => t.Pro_SellListInfo)
                .WithMany(t => t.Pro_SellIMEIList)
                .HasForeignKey(d => d.SellListID);
            this.HasOptional(t => t.Pro_SellSpecalOffList)
                .WithMany(t => t.Pro_SellIMEIList)
                .HasForeignKey(d => d.SellSpecalID);

        }
    }
}
