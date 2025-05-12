namespace Tutorial8.Contracts.Requests;

public record struct ProductWarehouseRequest(
    int IdProduct,
    int IdWarehouse,
    int Amount,
    string CreatedAt
    );