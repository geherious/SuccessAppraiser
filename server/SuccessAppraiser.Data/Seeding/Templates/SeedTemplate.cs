using SuccessAppraiser.Data.Context;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.Data.Seeding.Templates
{
    public static class SeedTemplate
    {
        public static void Seed(ApplicationDbContext dbContext)
        {
            CreateHabbitTemplate(dbContext);

        }

        private static void CreateHabbitTemplate(ApplicationDbContext dbContext)
        {
            GoalTemplate template = new GoalTemplate();
            template.Name = "Habbit";

            DayState easy = new DayState();
            easy.Name = "Easy";
            easy.Color = "#58E000";

            DayState average = new DayState();
            average.Name = "Average";
            average.Color = "#FFF800";

            DayState hard = new DayState();
            hard.Name = "Hard";
            hard.Color = "#FF2C00";

            template.States.Add(easy);
            template.States.Add(average);
            template.States.Add(hard);

            GoalTemplate existingTemplate = dbContext.GoalTemplates.FirstOrDefault(x => x.Name == template.Name)!;
            if (existingTemplate != null)
            {
                return;
            }
            else
            {
                dbContext.GoalTemplates.Add(template);
                dbContext.SaveChanges();
            }
        }
    }
}
