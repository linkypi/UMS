using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_Sell_CarMap : EntityTypeConfiguration<Pro_Sell_Car>
    {
        public Pro_Sell_CarMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.CarName)
                .HasMaxLength(50);

            this.Property(t => t.CarID)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_Sell_Car");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.CarName).HasColumnName("CarName");
            this.Property(t => t.CarID).HasColumnName("CarID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Desc).HasColumnName("Desc");
            this.Property(t => t.IsOther).HasColumnName("IsOther");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SellListID).HasColumnName("SellListID");

            // Relationships
            this.HasOptional(t => t.Pro_SellListInfo)
                .WithMany(t => t.Pro_Sell_Car)
                .HasForeignKey(d => d.SellListID);

        }
    }
}
