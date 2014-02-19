using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_Sell_ServiceMap : EntityTypeConfiguration<Pro_Sell_Service>
    {
        public Pro_Sell_ServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.ProClass)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.VIPService_ProID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_Sell_Service");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.ProClass).HasColumnName("ProClass");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.isVIPService).HasColumnName("isVIPService");
            this.Property(t => t.VIPService_ProID).HasColumnName("VIPService_ProID");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_Sell_Service)
                .HasForeignKey(d => d.VIPService_ProID);
            this.HasRequired(t => t.Pro_SellListInfo)
                .WithMany(t => t.Pro_Sell_Service)
                .HasForeignKey(d => d.SellListID);

        }
    }
}
