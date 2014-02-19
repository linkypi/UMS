using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace APIModel.Models.Mapping
{
    public class Pro_CashTicketMap : EntityTypeConfiguration<Pro_CashTicket>
    {
        public Pro_CashTicketMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.TicketID)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Pro_CashTicket");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SellListID).HasColumnName("SellListID");
            this.Property(t => t.TicketID).HasColumnName("TicketID");
            this.Property(t => t.IsBack).HasColumnName("IsBack");

            // Relationships
            this.HasOptional(t => t.Pro_SellListInfo)
                .WithMany(t => t.Pro_CashTicket)
                .HasForeignKey(d => d.SellListID);

        }
    }
}
