using TaskCBT.Domain.Entities;

namespace TaskCBT.Domain.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(string email);
        Task<Customer> GetByICNumberAsync(string icNumber);
        Task<Customer> GetByMobileNumberAsync(string mobileNumber);
    }
}
