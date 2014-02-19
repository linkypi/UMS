using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_SellOffAduitInfoListMap : EntityTypeConfiguration<Pro_SellOffAduitInfoList>
    {
        public Pro_SellOffAduitInfoListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_SellOffAduitInfoList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.IsPass).HasColumnName("IsPass");
            this.Property(t => t.AduitDate).HasColumnName("AduitDate");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.AduitID).HasColumnName("AduitID");

            // Relationships
            this.HasRequired(t => t.Pro_SellOffAduitInfo)
                .WithMany(t => t.Pro_SellOffAduitInfoList)
                .HasForeignKey(d => d.AduitID);
            this.HasRequired(t => t.Sys_UserInfo)
                .WithMany(t => t.Pro_SellOffAduitInfoList)
                .HasForeignKey(d => d.UserID);

        }
    }
}
