namespace Tutorial8.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<(bool exists, int orderId)> OrderExistsWithProductId(int productId, int amount, DateTime createdAt, CancellationToken cancellationToken);
    
    Task<bool> IsOrderFulfilled(int orderId, CancellationToken cancellationToken);
    
    Task UpdateOrderFulfilled(int orderId, CancellationToken cancellationToken);
}