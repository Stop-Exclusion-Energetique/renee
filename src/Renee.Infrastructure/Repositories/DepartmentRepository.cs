using Microsoft.EntityFrameworkCore;
using Renee.Domain.Entity;
using Renee.Domain.Repositories;
using Renee.Infrastructure.Data;

namespace Renee.Infrastructure.Repositories;

public class DepartmentRepository(ReneeDbContext dbContext) : IDepartmentRepository
{
	public async Task<List<Department>> GetAllAsync() => await dbContext.Departments.ToListAsync();
}