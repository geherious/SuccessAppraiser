using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuccessAppraiser.Data.Entities
{
    public class GoalDate
    {
        [Key]
        public Guid Id { get; set; }

        public DateOnly Date { get; set; }

        [MaxLength(1024)]
        public string? Comment { get; set; }

        public Guid StateId { get; set; }
        [Required]
        public DayState? State { get; set; }

        public Guid GoalId { get; set; }
        [Required]
        public GoalItem? Goal { get; set; }

    }
}
