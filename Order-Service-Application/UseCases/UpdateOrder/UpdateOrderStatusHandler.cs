using MediatR;
using Order_Service_Application.Interfaces;
using Order_Service_Application.Outbox;
using Order_Service_Domain.Interfaces;

namespace Order_Service_Application.UseCases.UpdateOrder
{
    public class UpdateOrderStatusHandler(IOrderRepository repository, IProcessedMessageRepository processedRepository) : IRequestHandler<UpdateOrderStatusCommand, Unit>
    {
        private readonly IOrderRepository _repository = repository;
        private readonly IProcessedMessageRepository _processedRepository = processedRepository;

        public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            if (await _processedRepository.Exists(request.MessageId))
                return Unit.Value;

            await _processedRepository.AddAsync(new ProcessedMessage
            {
                Id = request.MessageId,
                ProcessedAt = DateTime.UtcNow
            });

            var order = await _repository.GetByIdAsync(request.OrderId);
            if (order is not null)
            {
                order.SetOrderStatus(request.OrderStatus);

                _repository.UpdateOrder(order);

                await _repository.SaveChangesAsync();

                return Unit.Value;
            }

            return Unit.Value;
        }
    }
}
