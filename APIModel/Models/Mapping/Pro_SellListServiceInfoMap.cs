using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellListServiceInfoMap : EntityTypeConfiguration<Pro_SellListServiceInfo>
    {
        public Pro_SellListServiceInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Pro_SellListServiceInfo");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
