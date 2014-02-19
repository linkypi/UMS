using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_YanBoPriceStepInfoMap : EntityTypeConfiguration<View_YanBoPriceStepInfo>
    {
        public View_YanBoPriceStepInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.ISdecimals });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.ProTypeName)
                .HasMaxLength(50);

            this.Property(t => t.ProClassName)
                .HasMaxLength(50);

            this.Property(t => t.OldName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_YanBoPriceStepInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProPrice).HasColumnName("ProPrice");
            this.Property(t => t.StepPrice).HasColumnName("StepPrice");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.LowPrice).HasColumnName("LowPrice");
            this.Property(t => t.ProCost).HasColumnName("ProCost");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ProTypeName).HasColumnName("ProTypeName");
            this.Property(t => t.ProClassName).HasColumnName("ProClassName");
            this.Property(t => t.UpdateFlag).HasColumnName("UpdateFlag");
            this.Property(t => t.OldProPrice).HasColumnName("OldProPrice");
            this.Property(t => t.OldStepPrice).HasColumnName("OldStepPrice");
            this.Property(t => t.OldLowPrice).HasColumnName("OldLowPrice");
            this.Property(t => t.OldProCost).HasColumnName("OldProCost");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.OldName).HasColumnName("OldName");
            this.Property(t => t.ISdecimals).HasColumnName("ISdecimals");
        }
    }
}
