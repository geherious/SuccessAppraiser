using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;

namespace SuccessAppraiser.BLL.Common.Util
{
    public class MockUtil
    {
        public static DbSet<T> CreateMockDbSet<T>(params T[] items) where T : class
        {
            var mock = items.AsQueryable().BuildMockDbSet();
            return mock;
        }
    }
}
