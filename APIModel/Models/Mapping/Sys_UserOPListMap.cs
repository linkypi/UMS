using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Sys_UserOPListMap : EntityTypeConfiguration<Sys_UserOPList>
    {
        public Sys_UserOPListMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.UserID)
                .HasMaxLength(50);

            this.Property(t => t.UpdUserID)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Sys_UserOPList");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.UpdUserID).HasColumnName("UpdUserID");
            this.Property(t => t.OpID).HasColumnName("OpID");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.LeaveDate).HasColumnName("LeaveDate");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");

            // Relationships
            this.HasOptional(t => t.Pro_HallInfo)
                .WithMany(t => t.Sys_UserOPList)
                .HasForeignKey(d => d.HallID);
            this.HasOptional(t => t.Sys_UserInfo)
                .WithMany(t => t.Sys_UserOPList)
                .HasForeignKey(d => d.UserID);
            this.HasOptional(t => t.Sys_UserInfo1)
                .WithMany(t => t.Sys_UserOPList1)
                .HasForeignKey(d => d.UpdUserID);
            this.HasOptional(t => t.Sys_UserOp)
                .WithMany(t => t.Sys_UserOPList)
                .HasForeignKey(d => d.OpID);

        }
    }
}
