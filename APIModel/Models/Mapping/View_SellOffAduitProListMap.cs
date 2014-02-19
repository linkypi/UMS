using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SellOffAduitProListMap : EntityTypeConfiguration<View_SellOffAduitProList>
    {
        public View_SellOffAduitProListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProCount, t.OtherOff, t.ClassID, t.TypeID, t.ProPrice });

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.ProCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.OtherOff)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ClassID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_SellOffAduitProList");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.SellAduitID).HasColumnName("SellAduitID");
            this.Property(t => t.BackAduitID).HasColumnName("BackAduitID");
            this.Property(t => t.OtherOff).HasColumnName("OtherOff");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");
            this.Property(t => t.OldSellListID).HasColumnName("OldSellListID");
            this.Property(t => t.Preferent).HasColumnName("Preferent");
        }
    }
}
