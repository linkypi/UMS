using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class VIP_OffTicketMap : EntityTypeConfiguration<VIP_OffTicket>
    {
        public VIP_OffTicketMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.TicketID)
                .HasMaxLength(50);

            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Source)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VIP_OffTicket");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.TicketID).HasColumnName("TicketID");
            this.Property(t => t.OffID).HasColumnName("OffID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.VIP_ID).HasColumnName("VIP_ID");
            this.Property(t => t.Used).HasColumnName("Used");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.UseDate).HasColumnName("UseDate");
            this.Property(t => t.Source).HasColumnName("Source");
            this.Property(t => t.Note).HasColumnName("Note");

            // Relationships
            this.HasOptional(t => t.VIP_OffList)
                .WithMany(t => t.VIP_OffTicket)
                .HasForeignKey(d => d.OffID);
            this.HasRequired(t => t.VIP_VIPInfo)
                .WithMany(t => t.VIP_OffTicket)
                .HasForeignKey(d => d.VIP_ID);

        }
    }
}
