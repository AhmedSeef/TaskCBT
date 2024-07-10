using Microsoft.EntityFrameworkCore;
using TaskCBT.Domain.Entities;
using TaskCBT.Domain.Interfaces;
using TaskCBT.Infrastructure.Data;

namespace TaskCBT.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(TaskDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer> GetByICNumberAsync(string icNumber)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.ICNumber == icNumber);
        }

        public async Task<Customer> GetByMobileNumberAsync(string mobileNumber)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.MobileNumber == mobileNumber);
        }
    }
}
