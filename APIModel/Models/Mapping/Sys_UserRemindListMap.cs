using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_UserRemindListMap : EntityTypeConfiguration<Sys_UserRemindList>
    {
        public Sys_UserRemindListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_UserRemindList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Remind).HasColumnName("Remind");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Count).HasColumnName("Count");

            // Relationships
            this.HasOptional(t => t.Sys_RemindList)
                .WithMany(t => t.Sys_UserRemindList)
                .HasForeignKey(d => d.Remind);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Sys_UserRemindList)
                .HasForeignKey(d => d.UserID);

        }
    }
}
