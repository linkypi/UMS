using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_VIPTypeServiceMap : EntityTypeConfiguration<View_VIPTypeService>
    {
        public View_VIPTypeServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.ServiceID);

            // Properties
            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.ServiceID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_VIPTypeService");
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.SCount).HasColumnName("SCount");
            this.Property(t => t.ServiceID).HasColumnName("ServiceID");
            this.Property(t => t.Flag).HasColumnName("Flag");
        }
    }
}
