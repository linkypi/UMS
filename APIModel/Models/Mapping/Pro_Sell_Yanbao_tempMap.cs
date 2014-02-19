using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_Sell_Yanbao_tempMap : EntityTypeConfiguration<Pro_Sell_Yanbao_temp>
    {
        public Pro_Sell_Yanbao_tempMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.YanBaoName)
                .HasMaxLength(50);

            this.Property(t => t.BillID)
                .HasMaxLength(50);

            this.Property(t => t.MobileType)
                .HasMaxLength(50);

            this.Property(t => t.MobileName)
                .HasMaxLength(50);

            this.Property(t => t.MobileIMEI)
                .HasMaxLength(50);

            this.Property(t => t.FatureNum)
                .HasMaxLength(50);

            this.Property(t => t.BateriNum)
                .HasMaxLength(50);

            this.Property(t => t.NgarkuesNum)
                .HasMaxLength(50);

            this.Property(t => t.KufjeNum)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.UserName)
                .HasMaxLength(20);

            this.Property(t => t.UserPhoneNum)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Pro_Sell_Yanbao_temp");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.YanBaoName).HasColumnName("YanBaoName");
            this.Property(t => t.BillID).HasColumnName("BillID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.MobileType).HasColumnName("MobileType");
            this.Property(t => t.MobileName).HasColumnName("MobileName");
            this.Property(t => t.MobilePrice).HasColumnName("MobilePrice");
            this.Property(t => t.MobileIMEI).HasColumnName("MobileIMEI");
            this.Property(t => t.MobileDate).HasColumnName("MobileDate");
            this.Property(t => t.FatureNum).HasColumnName("FatureNum");
            this.Property(t => t.BateriNum).HasColumnName("BateriNum");
            this.Property(t => t.NgarkuesNum).HasColumnName("NgarkuesNum");
            this.Property(t => t.KufjeNum).HasColumnName("KufjeNum");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.BackListID).HasColumnName("BackListID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.UserPhoneNum).HasColumnName("UserPhoneNum");
            this.Property(t => t.SellType).HasColumnName("SellType");

            // Relationships
            this.HasOptional(t => t.Pro_SellBackList)
                .WithMany(t => t.Pro_Sell_Yanbao_temp)
                .HasForeignKey(d => d.BackListID);
            this.HasOptional(t => t.Pro_SellListInfo_Temp)
                .WithMany(t => t.Pro_Sell_Yanbao_temp)
                .HasForeignKey(d => d.SellListID);

        }
    }
}
