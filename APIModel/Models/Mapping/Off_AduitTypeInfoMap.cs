using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Off_AduitTypeInfoMap : EntityTypeConfiguration<Off_AduitTypeInfo>
    {
        public Off_AduitTypeInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            this.Property(t => t.DeleteUserID)
                .HasMaxLength(50);

            this.Property(t => t.AddUserID)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Off_AduitTypeInfo");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.DeleteDate).HasColumnName("DeleteDate");
            this.Property(t => t.DeleteUserID).HasColumnName("DeleteUserID");
            this.Property(t => t.AddDate).HasColumnName("AddDate");
            this.Property(t => t.AddUserID).HasColumnName("AddUserID");
            this.Property(t => t.OldAduitType).HasColumnName("OldAduitType");
        }
    }
}
