using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.UnitTests.Common
{
    public class GoalTestObjects
    {
        public static GoalTemplate GetHabbitTemplate()
        {
            GoalTemplate template = new GoalTemplate();
            template.Name = "Habbit";
            template.Id = Guid.Parse("213efabc-8997-43f4-bcf1-056ff716ca31");

            template.States.Add(GetEasyDayState());
            template.States.Add(GetAverageDayState());
            template.States.Add(GetHardDayState());

            return template;
        }

        public static DayState GetEasyDayState()
        {
            DayState easy = new DayState();
            easy.Id = Guid.Parse("544014f4-5aa6-4987-ac5a-30ebb3593153");
            easy.Name = "Easy";
            easy.Color = "#58E000";
            return easy;
        }

        public static DayState GetAverageDayState()
        {
            DayState average = new DayState();
            average.Id = Guid.Parse("b347dbf9-98eb-473b-b7a1-4babde51c9a0");
            average.Name = "Average";
            average.Color = "#FFF800";
            return average;
        }

        public static DayState GetHardDayState()
        {
            DayState hard = new DayState();
            hard.Id = Guid.Parse("c334ec91-b37e-4c0c-b4ba-0b92a6304480");
            hard.Name = "Hard";
            hard.Color = "#FF2C00";
            return hard;
        }

        public static GoalItem GetHabbitGoal()
        {
            GoalItem item = new GoalItem();
            item.Id = Guid.Parse("86ddc570-a18d-4535-908f-132b485c2a42");
            item.Name = "Running";
            item.Description = "Description";
            item.DaysNumber = 12;

            item.DateStart = new DateOnly(2024, 06, 12);
            item.UserId = Guid.Parse("cffaea27-8a8f-471d-aa12-39913ffbbda3");

            var template = GetHabbitTemplate();
            item.Template = template;
            item.TemplateId = template.Id;

            GoalDate date = GetHabbitDate11();
            date.Goal = item;
            date.GoalId = item.Id;

            item.Dates.Add(date);

            return item;
        }

        public static GoalDate GetHabbitDate11()
        {
            GoalDate date = new GoalDate();
            date.Id = Guid.Parse("7648b821-6102-4420-8a46-751150b545d3");
            date.Date = new DateOnly(2024, 06, 13);
            date.Comment = "Comment";
            date.State = GetAverageDayState();
            date.StateId = GetAverageDayState().Id;


            return date;
        }
    }
}
