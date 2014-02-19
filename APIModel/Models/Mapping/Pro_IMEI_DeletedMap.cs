using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_IMEI_DeletedMap : EntityTypeConfiguration<Pro_IMEI_Deleted>
    {
        public Pro_IMEI_DeletedMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.IMEI)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_IMEI_Deleted");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.IMEI).HasColumnName("IMEI");
        }
    }
}
