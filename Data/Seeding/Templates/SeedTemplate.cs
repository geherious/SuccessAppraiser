using Microsoft.EntityFrameworkCore.Diagnostics;
using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;
using System.Text.Json;

namespace SuccessAppraiser.Data.Seeding.Templates
{
    public static class SeedTemplate
    {
        public static void Seed(ApplicationDbContext dbContext)
        {
            string jsonString = File.ReadAllText("../Data/Seeding/Templates/BaseTemplates.json");
            List<TemplateSeedBase>? templates = JsonSerializer.Deserialize<List<TemplateSeedBase>>(jsonString);

            if (templates != null && !dbContext.GoalTemplates.Any() )
            {
                foreach (TemplateSeedBase template in templates)
                {
                    GoalTemplate goalTemplate = new GoalTemplate();
                    goalTemplate.Name = template.Name;

                    List<DayState> dayStatesList = new List<DayState>();
                    foreach(KeyValuePair<string, string> kvp in template.States)
                    {
                        DayState dayState = new DayState();
                        dayState.Name = kvp.Key;
                        dayState.Color = kvp.Value;
                        dayState.Templates.Add(goalTemplate);

                        dbContext.DayStates.Add(dayState);
                        dayStatesList.Add(dayState);
                    }

                    goalTemplate.States = dayStatesList;
                    dbContext.GoalTemplates.Add(goalTemplate);

                    dbContext.SaveChanges();
                }
            }
        }
    }
}
