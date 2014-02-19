using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_PersonSalaryMap : EntityTypeConfiguration<View_PersonSalary>
    {
        public View_PersonSalaryMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Seller)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("View_PersonSalary");
            this.Property(t => t.salary).HasColumnName("salary");
            this.Property(t => t.Seller).HasColumnName("Seller");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.SellDate).HasColumnName("SellDate");
            this.Property(t => t.ID).HasColumnName("ID");
        }
    }
}
