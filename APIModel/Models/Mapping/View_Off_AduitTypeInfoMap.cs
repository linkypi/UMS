using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_Off_AduitTypeInfoMap : EntityTypeConfiguration<View_Off_AduitTypeInfo>
    {
        public View_Off_AduitTypeInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ID, t.Name, t.Price, t.StartDate, t.EndDate, t.Flag, t.AddDate, t.AddUserID });

            // Properties
            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.Price)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AddUserID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.StartTime)
                .HasMaxLength(100);

            this.Property(t => t.EndTime)
                .HasMaxLength(100);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.RealName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_Off_AduitTypeInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.AddDate).HasColumnName("AddDate");
            this.Property(t => t.AddUserID).HasColumnName("AddUserID");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.RealName).HasColumnName("RealName");
        }
    }
}
