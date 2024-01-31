using System.ComponentModel.DataAnnotations;

namespace SuccessAppraiser.Entities
{
    public class GoalTemplate
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public Guid? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public List<DayState> States { get; set; } = new List<DayState>();
    }
}
