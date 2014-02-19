using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class View_OffListAduitMap : EntityTypeConfiguration<View_OffListAduit>
    {
        public View_OffListAduitMap()
        {
            // Primary Key
            this.HasKey(t => new { t.OffID, t.OffFlag, t.Type, t.ArriveMoney, t.Flag, t.UseLimit, t.Aduited, t.Passed, t.Aduited1, t.Passed1, t.Aduited2, t.Passed2, t.IsDelete });

            // Properties
            this.Property(t => t.OffID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.OffName)
                .HasMaxLength(50);

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

            this.Property(t => t.UseLimit)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AduitUser2)
                .HasMaxLength(50);

            this.Property(t => t.AduitUser1)
                .HasMaxLength(50);

            this.Property(t => t.Aduited)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Passed)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Aduited1)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Passed1)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Aduited2)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Passed2)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.AduitNote2)
                .HasMaxLength(500);

            this.Property(t => t.AduitNote1)
                .HasMaxLength(500);

            this.Property(t => t.AduitDate2)
                .HasMaxLength(50);

            this.Property(t => t.AduitDate1)
                .HasMaxLength(50);

            this.Property(t => t.ApplyNote)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("View_OffListAduit");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.OffName).HasColumnName("OffName");
            this.Property(t => t.OffMoney).HasColumnName("OffMoney");
            this.Property(t => t.OffRate).HasColumnName("OffRate");
            this.Property(t => t.OffPoint).HasColumnName("OffPoint");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
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
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.UseLimit).HasColumnName("UseLimit");
            this.Property(t => t.SysDate).HasColumnName("SysDate");
            this.Property(t => t.AduitUser2).HasColumnName("AduitUser2");
            this.Property(t => t.AduitUser1).HasColumnName("AduitUser1");
            this.Property(t => t.Aduited).HasColumnName("Aduited");
            this.Property(t => t.Passed).HasColumnName("Passed");
            this.Property(t => t.Aduited1).HasColumnName("Aduited1");
            this.Property(t => t.Passed1).HasColumnName("Passed1");
            this.Property(t => t.Aduited2).HasColumnName("Aduited2");
            this.Property(t => t.Passed2).HasColumnName("Passed2");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.AduitNote2).HasColumnName("AduitNote2");
            this.Property(t => t.AduitNote1).HasColumnName("AduitNote1");
            this.Property(t => t.AduitDate2).HasColumnName("AduitDate2");
            this.Property(t => t.AduitDate1).HasColumnName("AduitDate1");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.ApplyNote).HasColumnName("ApplyNote");
        }
    }
}
