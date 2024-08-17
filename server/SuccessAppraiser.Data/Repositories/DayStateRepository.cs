using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;

namespace SuccessAppraiser.Data.Repositories
{
    public class DayStateRepository : BaseRepository<DayState>, IDayStateRepository
    {
        public DayStateRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
