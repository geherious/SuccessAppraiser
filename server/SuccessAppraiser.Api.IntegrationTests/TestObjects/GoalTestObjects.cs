using SuccessAppraiser.Data.Entities;
using System;

namespace SuccessAppraiser.Api.IntegrationTests.TestObjects
{
    public class GoalTestObjects
    {
        public static DayState GetAState()
        {
            DayState dayState = new DayState();
            dayState.Name = "A";
            dayState.Color = "#537263";
            return dayState;
        }

        public static DayState GetBState()
        {
            DayState dayState = new DayState();
            dayState.Name = "B";
            dayState.Color = "#5c7264";
            return dayState;
        }

        public static GoalTemplate GetABTemplate()
        {
            GoalTemplate goalTemplate = new GoalTemplate();
            goalTemplate.Name = "AB";
            goalTemplate.States.Add(GetAState());
            goalTemplate.States.Add(GetBState());

            return goalTemplate;
        }

        public static GoalItem GetBaseGoal()
        {
            GoalItem goalItem = new GoalItem();
            goalItem.Name = "Test Goal";
            goalItem.DaysNumber = 12;
            goalItem.Description = "Description";
            goalItem.DateStart = new DateOnly(2024, 1, 1);
            goalItem.Template = GetABTemplate();
            return goalItem;
        }

        public static GoalDate GetGoalDate()
        {
            GoalDate goalDate = new GoalDate();
            goalDate.Date = new DateOnly(2024, 1, 1);
            goalDate.Comment = "Comment";
            
            return goalDate;
        }
    }
}
