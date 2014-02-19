using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_RepaireRetListMap : EntityTypeConfiguration<View_RepaireRetList>
    {
        public View_RepaireRetListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProCount, t.NeedIMEI, t.ISdecimals, t.BackCount, t.RepairListID, t.IsReturn });

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            this.Property(t => t.ProCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.BackCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RepairListID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IsReturn)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("View_RepaireRetList");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.NeedIMEI).HasColumnName("NeedIMEI");
            this.Property(t => t.ISdecimals).HasColumnName("ISdecimals");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.BackCount).HasColumnName("BackCount");
            this.Property(t => t.RepairListID).HasColumnName("RepairListID");
            this.Property(t => t.RepairID).HasColumnName("RepairID");
            this.Property(t => t.AduitCount).HasColumnName("AduitCount");
            this.Property(t => t.IsReturn).HasColumnName("IsReturn");
        }
    }
}
