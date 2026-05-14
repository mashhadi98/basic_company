using Company.Application.Features.Customers.Repositories;
using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<List<Customer>> GetAllAsync(bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Customer> query = _context.Customers.OrderBy(c => c.SortOrder).ThenBy(c => c.CompanyName);

        if (isPublished.HasValue)
            query = query.Where(c => c.IsPublished == isPublished.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        customer.CreatedAt = DateTime.UtcNow;
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _context.Customers.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers.AnyAsync(c => c.Id == id, cancellationToken);
    }
}
