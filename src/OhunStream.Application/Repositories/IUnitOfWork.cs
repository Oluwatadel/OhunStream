using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhunStream.Infrastructure.Persistence.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
