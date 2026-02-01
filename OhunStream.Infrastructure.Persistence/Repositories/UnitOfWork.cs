using OhunStream.Infrastructure.Persistence.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhunStream.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork(OhunStreamDbContext dbContext) : IUnitOfWork
    {

        public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
