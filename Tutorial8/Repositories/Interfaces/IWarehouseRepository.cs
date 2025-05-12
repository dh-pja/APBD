using Tutorial8.Entities;

namespace Tutorial8.Repositories.Interfaces;

public interface IWarehouseRepository
{
    Task<int> AddProductToWarehouse(ProductWarehouse productWarehouse, int orderId, CancellationToken cancellationToken);
}