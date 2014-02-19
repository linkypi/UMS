using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_OutOrderListMap : EntityTypeConfiguration<View_OutOrderList>
    {
        public View_OutOrderListMap()
        {
            // Primary Key
            this.HasKey(t => t.OutListID);

            // Properties
            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.Aduit)
                .HasMaxLength(8000);

            this.Property(t => t.FromHallName)
                .HasMaxLength(50);

            this.Property(t => t.FromUserName)
                .HasMaxLength(50);

            this.Property(t => t.ToUserName)
                .HasMaxLength(50);

            this.Property(t => t.Pro_HallName)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.DeleterName)
                .HasMaxLength(50);

            this.Property(t => t.OldID)
                .HasMaxLength(50);

            this.Property(t => t.OutDate)
                .HasMaxLength(100);

            this.Property(t => t.OutOrderID)
                .HasMaxLength(50);

            this.Property(t => t.OutListID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_OutOrderList");
            this.Property(t => t.OutID).HasColumnName("OutID");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.Aduit).HasColumnName("Aduit");
            this.Property(t => t.FromHallName).HasColumnName("FromHallName");
            this.Property(t => t.FromUserName).HasColumnName("FromUserName");
            this.Property(t => t.ToUserName).HasColumnName("ToUserName");
            this.Property(t => t.Pro_HallName).HasColumnName("Pro_HallName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.DeleterName).HasColumnName("DeleterName");
            this.Property(t => t.OldID).HasColumnName("OldID");
            this.Property(t => t.OutDate).HasColumnName("OutDate");
            this.Property(t => t.OutOrderID).HasColumnName("OutOrderID");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.NeedIMEI).HasColumnName("NeedIMEI");
            this.Property(t => t.OutListID).HasColumnName("OutListID");
        }
    }
}
