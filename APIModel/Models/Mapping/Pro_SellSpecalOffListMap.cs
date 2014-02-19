using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellSpecalOffListMap : EntityTypeConfiguration<Pro_SellSpecalOffList>
    {
        public Pro_SellSpecalOffListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellSpecalOffList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.SpecalOffID).HasColumnName("SpecalOffID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.BackID).HasColumnName("BackID");
            this.Property(t => t.OffMoney).HasColumnName("OffMoney");
            this.Property(t => t.SellAduitID).HasColumnName("SellAduitID");
            this.Property(t => t.BackAduitID).HasColumnName("BackAduitID");

            // Relationships
            this.HasOptional(t => t.Pro_SellBack)
                .WithMany(t => t.Pro_SellSpecalOffList)
                .HasForeignKey(d => d.BackID);
            this.HasOptional(t => t.Pro_SellBackInfo_Aduit)
                .WithMany(t => t.Pro_SellSpecalOffList)
                .HasForeignKey(d => d.BackAduitID);
            this.HasOptional(t => t.Pro_SellInfo)
                .WithMany(t => t.Pro_SellSpecalOffList)
                .HasForeignKey(d => d.SellID);
            this.HasOptional(t => t.Pro_SellInfo_Aduit)
                .WithMany(t => t.Pro_SellSpecalOffList)
                .HasForeignKey(d => d.SellAduitID);
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.Pro_SellSpecalOffList)
                .HasForeignKey(d => d.SpecalOffID);

        }
    }
}
