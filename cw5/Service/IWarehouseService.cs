using System;
using System.Threading.Tasks;
using cw5.Models;

namespace cw5.Service
{
    public interface IWarehouseService
    {
        public Task<bool> DoesProductExist(int productId);
        public Task<bool> DoesWarehouseExist(int productId);
        public Task<int> GetTheValidOrderId(int warehouseId, int productId, int amount, DateTime createdAt);
        public Task<bool> CompeleteTheOrder(int orderId, int warehouseId, int productId, int amount, DateTime createdAt);
        public Task<double> GetTheProductPrice(int productId);
        public Task<int> StoredProcedure(int warehouseId, int productId, int amount, DateTime createdAt);
        public Task<bool> WasOrderFullfilled(int idOrder);
        
        public Task<int> AddProductToWarehouse(int warehouseId, int productId, int amount, int orderId);
        
    }
}