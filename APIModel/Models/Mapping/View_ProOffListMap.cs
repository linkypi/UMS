using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_ProOffListMap : EntityTypeConfiguration<View_ProOffList>
    {
        public View_ProOffListMap()
        {
            // Primary Key
            this.HasKey(t => new { t.OffID, t.OffFlag, t.ProCount, t.NeedIMEI, t.OffType, t.SendPoint });

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

            this.Property(t => t.ProID)
                .HasMaxLength(50);

            this.Property(t => t.OffProNote)
                .HasMaxLength(50);

            this.Property(t => t.ProCount)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProName)
                .HasMaxLength(50);

            this.Property(t => t.ClassName)
                .HasMaxLength(50);

            this.Property(t => t.TypeName)
                .HasMaxLength(50);

            this.Property(t => t.OffUpdUser)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.OffType)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SendPoint)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("View_ProOffList");
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
            this.Property(t => t.ProID).HasColumnName("ProID");
            this.Property(t => t.OffProNote).HasColumnName("OffProNote");
            this.Property(t => t.ProCount).HasColumnName("ProCount");
            this.Property(t => t.SellTypeID).HasColumnName("SellTypeID");
            this.Property(t => t.AfterOffPrice).HasColumnName("AfterOffPrice");
            this.Property(t => t.ProName).HasColumnName("ProName");
            this.Property(t => t.ClassName).HasColumnName("ClassName");
            this.Property(t => t.TypeName).HasColumnName("TypeName");
            this.Property(t => t.NeedIMEI).HasColumnName("NeedIMEI");
            this.Property(t => t.ProFormat).HasColumnName("ProFormat");
            this.Property(t => t.UpdDate).HasColumnName("UpdDate");
            this.Property(t => t.OffUpdUser).HasColumnName("OffUpdUser");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.OffType).HasColumnName("OffType");
            this.Property(t => t.SendPoint).HasColumnName("SendPoint");
            this.Property(t => t.Salary).HasColumnName("Salary");
            this.Property(t => t.Rate).HasColumnName("Rate");
            this.Property(t => t.Point).HasColumnName("Point");
            this.Property(t => t.ProOffMoney).HasColumnName("ProOffMoney");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Note).HasColumnName("Note");
        }
    }
}
