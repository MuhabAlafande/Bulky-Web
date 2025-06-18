using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class CompanyRepository(ApplicationDbContext dbContext) : Repository<Company>(dbContext), ICompanyRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public void Update(Company company) => _dbContext.Update(company);
}