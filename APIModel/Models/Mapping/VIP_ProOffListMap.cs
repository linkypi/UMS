using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_ProOffListMap : EntityTypeConfiguration<VIP_ProOffList>
    {
        public VIP_ProOffListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_ProOffList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SellTypeID).HasColumnName("SellTypeID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.AfterOffPrice).HasColumnName("AfterOffPrice");
            this.Property(t => t.Salary).HasColumnName("Salary");
            this.Property(t => t.Rate).HasColumnName("Rate");
            this.Property(t => t.ReduceMoney).HasColumnName("ReduceMoney");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.OffMoney).HasColumnName("OffMoney");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.VIP_ProOffList)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_SellType)
                .WithMany(t => t.VIP_ProOffList)
                .HasForeignKey(d => d.SellTypeID);
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.VIP_ProOffList)
                .HasForeignKey(d => d.OffID);

        }
    }
}
