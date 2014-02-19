using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_ChangeListMap : EntityTypeConfiguration<View_Pro_ChangeList>
    {
        public View_Pro_ChangeListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.HasPrice, t.ID, t.STPID });

            // Properties
            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.HasPrice)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.ID)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.ClassID)
                .HasMaxLength(100);

            this.Property(t => t.SellTypeName)
                .HasMaxLength(50);

            this.Property(t => t.STPID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TypeID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Pro_ChangeList");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.OldPrice).HasColumnName("OldPrice");
            this.Property(t => t.LowPrice).HasColumnName("LowPrice");
            this.Property(t => t.OldLowPrice).HasColumnName("OldLowPrice");
            this.Property(t => t.MinPrice).HasColumnName("MinPrice");
            this.Property(t => t.OldMinPrice).HasColumnName("OldMinPrice");
            this.Property(t => t.MaxPrice).HasColumnName("MaxPrice");
            this.Property(t => t.OldMaxPrice).HasColumnName("OldMaxPrice");
            this.Property(t => t.IsTicketUseful).HasColumnName("IsTicketUseful");
            this.Property(t => t.OldIsTicketUseful).HasColumnName("OldIsTicketUseful");
            this.Property(t => t.ProCost).HasColumnName("ProCost");
            this.Property(t => t.OldProCost).HasColumnName("OldProCost");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.HasPrice).HasColumnName("HasPrice");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellTypeID).HasColumnName("SellTypeID");
            this.Property(t => t.UpdateFlag).HasColumnName("UpdateFlag");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.SellTypeName).HasColumnName("SellTypeName");
            this.Property(t => t.STPID).HasColumnName("STPID");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
        }
    }
}
