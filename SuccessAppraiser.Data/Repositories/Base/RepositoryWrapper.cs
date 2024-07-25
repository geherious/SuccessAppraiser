using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Repositories.Interfaces;
using System;

namespace SuccessAppraiser.Data.Repositories.Base
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _dbContext;

        public RepositoryWrapper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}
