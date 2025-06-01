using Microsoft.Data.SqlClient;
using Tutorial8.Entities;
using Tutorial8.Repositories.Interfaces;

namespace Tutorial8.Repositories;

public class WarehouseRepository(IConfiguration configuration) : IWarehouseRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("Default") ??
                                                throw new ArgumentException("Connection string not found");


    public Task<bool> TestConnection(CancellationToken cancellationToken)
    {
        const string query = """
                              SELECT 1
                              """;

        return Task.Run(async () =>
        {
            await using SqlConnection con = new SqlConnection(_connectionString);
            await using SqlCommand cmd = new SqlCommand(query, con);
            await con.OpenAsync(cancellationToken);
            var result = await cmd.ExecuteScalarAsync(cancellationToken);
            return result is not null;
        }, cancellationToken);
    }

    public async Task<int> AddProductToWarehouse(ProductWarehouse productWarehouse, int orderId, CancellationToken cancellationToken)
    {
        const string query = """
                              INSERT INTO dbo.Product_Warehouse (IdProduct, IdWarehouse, IdOrder, Amount, Price, CreatedAt)
                              OUTPUT inserted.IdProductWarehouse
                              VALUES (@IdProduct, @IdWarehouse, @IdOrder, @Amount, 
                                      (SELECT Price FROM Product WHERE Product.IdProduct = @IdProduct), GETDATE());
                              """;

        await using SqlConnection con = new SqlConnection(_connectionString);
        await using var transaction = await con.BeginTransactionAsync(cancellationToken);
        try
        {
            await using (SqlCommand cmd = new SqlCommand(query, con, (SqlTransaction)transaction))
            {
                cmd.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
                cmd.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
                cmd.Parameters.AddWithValue("@IdOrder", orderId);
                cmd.Parameters.AddWithValue("@Amount", productWarehouse.Amount);

                await con.OpenAsync(cancellationToken);
                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                transaction.Commit();

                return result is not null ? Convert.ToInt32(result) : -1;
            }
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
        }

        return -1;
    }
}