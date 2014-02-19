using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellBackAduitListMap : EntityTypeConfiguration<Pro_SellBackAduitList>
    {
        public Pro_SellBackAduitListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Pro_SellBackAduitList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AduitID).HasColumnName("AduitID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.BackPrice).HasColumnName("BackPrice");
            this.Property(t => t.AduitBackPrice).HasColumnName("AduitBackPrice");

            // Relationships
            this.HasRequired(t => t.Pro_SellBackAduit)
                .WithMany(t => t.Pro_SellBackAduitList)
                .HasForeignKey(d => d.AduitID);
            this.HasRequired(t => t.Pro_SellListInfo)
                .WithMany(t => t.Pro_SellBackAduitList)
                .HasForeignKey(d => d.SellListID);

        }
    }
}
