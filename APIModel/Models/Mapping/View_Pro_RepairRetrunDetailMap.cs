using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Pro_RepairRetrunDetailMap : EntityTypeConfiguration<View_Pro_RepairRetrunDetail>
    {
        public View_Pro_RepairRetrunDetailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TypeID, t.ClassID, t.ProID, t.ID });

            // Properties
            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.TypeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ClassID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NEW_IMEI)
                .HasMaxLength(50);

            this.Property(t => t.OLD_IMEI)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.InListID)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Pro_RepairRetrunDetail");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.ClassID).HasColumnName("ClassID");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.NEW_IMEI).HasColumnName("NEW_IMEI");
            this.Property(t => t.OLD_IMEI).HasColumnName("OLD_IMEI");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.InListID).HasColumnName("InListID");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.HallID).HasColumnName("HallID");
        }
    }
}
