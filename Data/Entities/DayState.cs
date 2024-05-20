using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SuccessAppraiser.Data.Entities
{
    public class DayState
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [RegularExpression(@"#(?i)[0-9A-Z]{6}")]
        public string? Color { get; set; }
        [JsonIgnore]
        public List<GoalTemplate> Templates { get; set; } = new List<GoalTemplate>();
    }
}
