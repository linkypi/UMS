using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_SellOffAduitInfoMap : EntityTypeConfiguration<View_SellOffAduitInfo>
    {
        public View_SellOffAduitInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.NextPrice, t.HallID, t.IsAduited, t.IsPass, t.ID, t.UserID });

            // Properties
            this.Property(t => t.NextPrice)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.HallName)
                .HasMaxLength(50);

            this.Property(t => t.HallID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IsAduited)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.IsPass)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.ApplyDate)
                .HasMaxLength(100);

            this.Property(t => t.ApplyUser)
                .HasMaxLength(50);

            this.Property(t => t.ApplyNote)
                .HasMaxLength(50);

            this.Property(t => t.ID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AduitNote)
                .HasMaxLength(50);

            this.Property(t => t.UserID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastAduiter)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_SellOffAduitInfo");
            this.Property(t => t.NextPrice).HasColumnName("NextPrice");
            this.Property(t => t.HallName).HasColumnName("HallName");
            this.Property(t => t.HallID).HasColumnName("HallID");
            this.Property(t => t.IsAduited).HasColumnName("IsAduited");
            this.Property(t => t.IsPass).HasColumnName("IsPass");
            this.Property(t => t.ApplyDate).HasColumnName("ApplyDate");
            this.Property(t => t.ApplyUser).HasColumnName("ApplyUser");
            this.Property(t => t.ApplyNote).HasColumnName("ApplyNote");
            this.Property(t => t.SellID).HasColumnName("SellID");
            this.Property(t => t.BackID).HasColumnName("BackID");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.AduitNote).HasColumnName("AduitNote");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.LastAduiter).HasColumnName("LastAduiter");
        }
    }
}
