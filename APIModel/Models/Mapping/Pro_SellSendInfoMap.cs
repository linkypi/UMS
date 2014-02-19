using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellSendInfoMap : EntityTypeConfiguration<Pro_SellSendInfo>
    {
        public Pro_SellSendInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.InOrderList)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellSendInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.SepecialID).HasColumnName("SepecialID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.SendProOffID).HasColumnName("SendProOffID");
            this.Property(t => t.ProCost).HasColumnName("ProCost");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.InOrderList).HasColumnName("InOrderList");
            this.Property(t => t.BackID).HasColumnName("BackID");

            // Relationships
            this.HasOptional(t => t.Pro_ProInfo)
                .WithMany(t => t.Pro_SellSendInfo)
                .HasForeignKey(d => d.ProID);
            this.HasOptional(t => t.Pro_SellBack)
                .WithMany(t => t.Pro_SellSendInfo)
                .HasForeignKey(d => d.BackID);
            this.HasOptional(t => t.Pro_SellInfo)
                .WithMany(t => t.Pro_SellSendInfo)
                .HasForeignKey(d => d.SellID);
            this.HasOptional(t => t.Pro_SellListInfo)
                .WithMany(t => t.Pro_SellSendInfo)
                .HasForeignKey(d => d.SellListID);
            this.HasOptional(t => t.Pro_SellSpecalOffList)
                .WithMany(t => t.Pro_SellSendInfo)
                .HasForeignKey(d => d.SepecialID);
            this.HasOptional(t => t.VIP_SendProOffList)
                .WithMany(t => t.Pro_SellSendInfo)
                .HasForeignKey(d => d.SendProOffID);

        }
    }
}
