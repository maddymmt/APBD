using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using cw5.Models;
using Microsoft.Extensions.Configuration;

namespace cw5.Service
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IConfiguration _configuration;

        public WarehouseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> DoesProductExist(int productId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM Product WHERE ProductId = @productId";
                command.Parameters.AddWithValue("productId", productId);
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                return reader.HasRows;
            }
        }

        public async Task<bool> DoesWarehouseExist(int warehouseId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM Warehouse WHERE WarehouseId = @warehouseId";
                command.Parameters.AddWithValue("warehouseId", warehouseId);
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                return reader.HasRows;
            }
        }

        public async Task<int> GetTheValidOrderId(int warehouseId, int productId, int amount, DateTime createdAt)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText =
                    $"SELECT idOrder FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount AND CreatedAt < @CreatedAt";
                command.Parameters.AddWithValue("IdProduct", productId);
                command.Parameters.AddWithValue("Amount", amount);
                command.Parameters.AddWithValue("CreatedAt", createdAt);

                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }

                return -1;
            }
        }


        public async Task<bool> CompeleteTheOrder(int orderId, int warehouseId, int productId, int amount,
            DateTime createdAt)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText =
                    $"INSERT INTO Product_warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
                command.Parameters.AddWithValue("IdOrder", orderId);
                command.Parameters.AddWithValue("IdWarehouse", warehouseId);
                command.Parameters.AddWithValue("IdProduct", productId);
                command.Parameters.AddWithValue("Amount", amount);
                command.Parameters.AddWithValue("Price", await GetTheProductPrice(productId) * amount);
                command.Parameters.AddWithValue("CreatedAt", createdAt);

                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();

                return reader.RecordsAffected > 0;
            }
        }

        public async Task<double> GetTheProductPrice(int productId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $"SELECT Price FROM Product WHERE IdProduct = @IdProduct";

                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();
                return double.Parse(reader.ToString());
            }
        }

        public async Task<int> StoredProcedure(int warehouseId, int productId, int amount, DateTime createdAt)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "AddProductToWarehouse";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdWarehouse", warehouseId);
                command.Parameters.AddWithValue("@IdProduct", productId);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@CreatedAt", createdAt);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> WasOrderFullfilled(int idOrder)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM Product_warehouse WHERE IdOrder = @IdOrder";
                command.Parameters.AddWithValue("IdOrder", idOrder);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }

        public async Task<int> AddProductToWarehouse(int warehouseId, int productId, int amount, int orderId)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "INSERT INTO Product_warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@IdWarehouse, @IdProduct,@IdOrder, @Amount, @Price, @CreatedAt)";
                command.Parameters.AddWithValue("@IdWarehouse", warehouseId);
                command.Parameters.AddWithValue("@IdProduct", productId);
                command.Parameters.AddWithValue("@IdOrder", orderId);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@Price", await GetTheProductPrice(productId)*amount);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
            
        }

    }
}