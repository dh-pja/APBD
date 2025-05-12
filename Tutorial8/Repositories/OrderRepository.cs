using Microsoft.Data.SqlClient;
using Tutorial8.Repositories.Interfaces;

namespace Tutorial8.Repositories;

public class OrderRepository(IConfiguration configuration) : IOrderRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("Default") ??
                                                 throw new ArgumentException("Connection string not found");
    
    public async Task<(bool exists, int orderId)> OrderExistsWithProductId(int productId, int amount, DateTime createdAt, CancellationToken cancellationToken)
    {
        const string query = """
                              SELECT IdOrder FROM
                              dbo.Order
                              WHERE IdProduct = @ProductId AND Amount = @Amount AND CreatedAt < @CreatedAt;
                              """;

        await using SqlConnection con = new SqlConnection(_connectionString);
        await using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@CreatedAt", createdAt);

            await con.OpenAsync(cancellationToken);
            
            var order = (exists: false, orderId: 0);
            
            var reader = await cmd.ExecuteReaderAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                order = (true, reader.GetInt32(0));
            }

            await reader.CloseAsync();
            await con.CloseAsync();
            
            return order;
        }
    }

    public async Task<bool> IsOrderFulfilled(int orderId, CancellationToken cancellationToken)
    {
        const string query =
            """
            SELECT Count(*) FROM dbo.Product_Warehouse
                WHERE IdOrder = @OrderId;
            """;

        await using SqlConnection con = new SqlConnection(_connectionString);
        await using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@OrderId", orderId);

            await con.OpenAsync(cancellationToken);
            await using SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken);
            
            return await reader.ReadAsync(cancellationToken);
        }
    }

    public async Task UpdateOrderFulfilled(int orderId, CancellationToken cancellationToken)
    {
        
        const string query = """
                              UPDATE dbo.Order
                              SET FulfilledAt = GETDATE()
                              WHERE IdOrder = @OrderId;
                              """;

        await using SqlConnection con = new SqlConnection(_connectionString);
        await using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@OrderId", orderId);
            await con.OpenAsync(cancellationToken);
            
            await cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}