using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order_Service_Domain.Entities;

namespace Order_Service_Infrastructure.Persistence.Mappers
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.UserId).IsRequired();

            builder.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status).IsRequired();

            builder.Property(o => o.CreatedAt).IsRequired();

            builder.HasMany(o => o.Items).WithOne().HasForeignKey(i => i.OrderId);
        }
    }
}
