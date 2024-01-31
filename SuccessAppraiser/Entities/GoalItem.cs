using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SuccessAppraiser.Entities
{
    public class GoalItem
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(48)]
        public string? Name { get; set; }
        [MaxLength(255)]
        public string? Description { get; set; }
        public int DaysNumber { get; set; }

        public DateOnly DateStart { get; set; }
        public List<GoalDate> Dates { get; set; } = new List<GoalDate>();
        public Guid UserId { get; set; }
        [Required]
        public ApplicationUser? User { get; set; }

        public Guid TemplateId { get; set; }
        [Required]
        public GoalTemplate? Template { get; set; }


    }
}
