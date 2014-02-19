using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_tianyadiscountPicMap : EntityTypeConfiguration<VIP_tianyadiscountPic>
    {
        public VIP_tianyadiscountPicMap()
        {
            // Primary Key
            this.HasKey(t => t.tianYidiscountPicId);

            // Properties
            this.Property(t => t.tianYidiscountPicId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.tianYidiscountPic)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_tianyadiscountPic");
            this.Property(t => t.tianYidiscountPicId).HasColumnName("tianYidiscountPicId");
            this.Property(t => t.tianYidiscountId).HasColumnName("tianYidiscountId");
            this.Property(t => t.tianYidiscountPic).HasColumnName("tianYidiscountPic");
            this.Property(t => t.tianYidiscountPicIndex).HasColumnName("tianYidiscountPicIndex");
        }
    }
}
