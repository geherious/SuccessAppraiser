using Microsoft.AspNetCore.Identity;

namespace SuccessAppraiser.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public List<GoalItem> Goals { get; set; } = new List<GoalItem>();
        public List<GoalTemplate> Templates { get; set; } = new List<GoalTemplate>();
    }
}
