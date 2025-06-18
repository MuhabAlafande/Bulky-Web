using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class ApplicationUserRepository(ApplicationDbContext dbContext) : Repository<ApplicationUser>(dbContext), IApplicationUserRepository;