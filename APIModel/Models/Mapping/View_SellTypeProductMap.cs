using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SellTypeProductMap : EntityTypeConfiguration<View_SellTypeProduct>
    {
        public View_SellTypeProductMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.IsTicketUseful, t.IsAduit, t.ISdecimals });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.ClassID)
                .HasMaxLength(50);

            this.Property(t => t.TypeID)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("View_SellTypeProduct");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.SellType).HasColumnName("SellType");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.LowPrice).HasColumnName("LowPrice");
            this.Property(t => t.MinPrice).HasColumnName("MinPrice");
            this.Property(t => t.MaxPrice).HasColumnName("MaxPrice");
            this.Property(t => t.IsTicketUseful).HasColumnName("IsTicketUseful");
            this.Property(t => t.IsAduit).HasColumnName("IsAduit");
            this.Property(t => t.ProCost).HasColumnName("ProCost");
            this.Property(t => t.UpdateFlag).HasColumnName("UpdateFlag");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.NewPrice).HasColumnName("NewPrice");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.ISdecimals).HasColumnName("ISdecimals");
        }
    }
}
