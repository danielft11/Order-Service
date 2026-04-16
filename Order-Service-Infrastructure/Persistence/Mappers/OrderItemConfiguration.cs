using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Service_Domain.Entities;

namespace Order_Service_Infrastructure.Persistence.Mappers
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProductId)
                .IsRequired();

            builder.Property(i => i.ProductName)
                .IsRequired();

            builder.Property(i => i.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Quantity)
                .IsRequired();
        }
    }
}
