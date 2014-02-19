using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_OffListMap : EntityTypeConfiguration<View_OffList>
    {
        public View_OffListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.OffID, t.OffFlag, t.Type, t.ArriveMoney });

            // Properties
            this.Property(t => t.OffID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OffName)
                .HasMaxLength(50);

            this.Property(t => t.StartTime)
                .HasMaxLength(100);

            this.Property(t => t.EndTime)
                .HasMaxLength(100);

            this.Property(t => t.discountPic)
                .HasMaxLength(50);

            this.Property(t => t.discountSynopsis)
                .HasMaxLength(250);

            this.Property(t => t.discountInfo)
                .HasMaxLength(300);

            this.Property(t => t.Type)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UpdUser)
                .HasMaxLength(50);

            this.Property(t => t.RealName)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.ArriveMoney)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SalesName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_OffList");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.OffName).HasColumnName("OffName");
            this.Property(t => t.OffMoney).HasColumnName("OffMoney");
            this.Property(t => t.OffRate).HasColumnName("OffRate");
            this.Property(t => t.OffPoint).HasColumnName("OffPoint");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.OffFlag).HasColumnName("OffFlag");
            this.Property(t => t.UnOver).HasColumnName("UnOver");
            this.Property(t => t.VIPTicketMaxCount).HasColumnName("VIPTicketMaxCount");
            this.Property(t => t.discountPic).HasColumnName("discountPic");
            this.Property(t => t.discountSynopsis).HasColumnName("discountSynopsis");
            this.Property(t => t.discountInfo).HasColumnName("discountInfo");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.UpdDate).HasColumnName("UpdDate");
            this.Property(t => t.UpdUser).HasColumnName("UpdUser");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.ArriveMoney).HasColumnName("ArriveMoney");
            this.Property(t => t.SalesName).HasColumnName("SalesName");
        }
    }
}
