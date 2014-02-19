using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_InOrderListMap : EntityTypeConfiguration<Pro_InOrderList>
    {
        public Pro_InOrderListMap()
        {
            // Primary Key
            this.HasKey(t => t.InListID);

            // Properties
            this.Property(t => t.InListID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.InOrderID)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.InitInListID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_InOrderList");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.Pro_InOrderID).HasColumnName("Pro_InOrderID");
            this.Property(t => t.InOrderID).HasColumnName("InOrderID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.InitInListID).HasColumnName("InitInListID");
            this.Property(t => t.RetailPrice).HasColumnName("RetailPrice");

            // Relationships
            this.HasOptional(t => t.Pro_InOrder)
                .WithMany(t => t.Pro_InOrderList)
                .HasForeignKey(d => d.Pro_InOrderID);
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_InOrderList)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_InOrderList2)
                .WithMany(t => t.Pro_InOrderList1)
                .HasForeignKey(d => d.InitInListID);

        }
    }
}
