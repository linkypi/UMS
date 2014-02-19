using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_OffListAduitMap : EntityTypeConfiguration<VIP_OffListAduit>
    {
        public VIP_OffListAduitMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            this.Property(t => t.UpdUser)
                .HasMaxLength(50);

            this.Property(t => t.discountPic)
                .HasMaxLength(50);

            this.Property(t => t.discountSynopsis)
                .HasMaxLength(250);

            this.Property(t => t.discountInfo)
                .HasMaxLength(300);

            this.Property(t => t.discountPicbigid__)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_OffListAduit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.ArriveMoney).HasColumnName("ArriveMoney");
            this.Property(t => t.OffMoney).HasColumnName("OffMoney");
            this.Property(t => t.ArriveCount).HasColumnName("ArriveCount");
            this.Property(t => t.OffRate).HasColumnName("OffRate");
            this.Property(t => t.OffPoint).HasColumnName("OffPoint");
            this.Property(t => t.OffPointMoney).HasColumnName("OffPointMoney");
            this.Property(t => t.MaxPoint).HasColumnName("MaxPoint");
            this.Property(t => t.MinPoint).HasColumnName("MinPoint");
            this.Property(t => t.SendPoint).HasColumnName("SendPoint");
            this.Property(t => t.HaveTop).HasColumnName("HaveTop");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.UpdDate).HasColumnName("UpdDate");
            this.Property(t => t.UpdUser).HasColumnName("UpdUser");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.UseLimit).HasColumnName("UseLimit");
            this.Property(t => t.SendTicket).HasColumnName("SendTicket");
            this.Property(t => t.VIPTicketMaxCount).HasColumnName("VIPTicketMaxCount");
            this.Property(t => t.discountPic).HasColumnName("discountPic");
            this.Property(t => t.discountSynopsis).HasColumnName("discountSynopsis");
            this.Property(t => t.discountInfo).HasColumnName("discountInfo");
            this.Property(t => t.UnOver).HasColumnName("UnOver");
            this.Property(t => t.discountPicbigid__).HasColumnName("discountPicbigid  ");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.HeadID).HasColumnName("HeadID");

            // Relationships
            this.HasRequired(t => t.VIP_OffListAduitHeader)
                .WithMany(t => t.VIP_OffListAduit)
                .HasForeignKey(d => d.HeadID);

        }
    }
}
