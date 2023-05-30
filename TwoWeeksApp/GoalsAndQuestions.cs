using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoWeeksApp
{
    public class GoalsAndQuestions
    {    
        public int Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<UserQuestions> Questions { get; set; }

        public static void LoadAllGoalsAndQuestions()
        {
            GoalsAndQuestions tmpGoal = CreateNewPost("Lose Weight, Feel Great");
            UserQuestions tmpQ = tmpGoal.CreateNewQuestion("WHAT IS YOUR CURRENT AGE: ");
            tmpQ.InputControlTypes.Add("Entry;AGE");

            tmpQ = tmpGoal.CreateNewQuestion("WHAT IS YOUR CURRENT WEIGHT: ");
            tmpQ.InputControlTypes.Add("Entry;WEIGHT");

            tmpQ = tmpGoal.CreateNewQuestion("WHAT IS YOUR CURRENT HEIGHT: ");
            tmpQ.InputControlTypes.Add("Entry;FEET");
            tmpQ.InputControlTypes.Add("Entry;INCHES");

            tmpQ = tmpGoal.CreateNewQuestion("HOW MUCH WEIGHT ARE YOU HOPING TO LOSE: ");
            tmpQ.InputControlTypes.Add("Radio;0-10 lbs");
            tmpQ.InputControlTypes.Add("Radio;10-20 lbs");
            tmpQ.InputControlTypes.Add("Radio;20-30 lbs");
            tmpQ.InputControlTypes.Add("Radio;30-40 lbs");
            tmpQ.InputControlTypes.Add("Radio;40-50 lbs");

            tmpQ = tmpGoal.CreateNewQuestion("HOW MUCH TIME ARE YOU WILLING TO COMMIT: ");
            tmpQ.InputControlTypes.Add("Radio;1-2 weeks");
            tmpQ.InputControlTypes.Add("Radio;2-3 weeks");
            tmpQ.InputControlTypes.Add("Radio;1 month");


            App.AllAvailableGoals.Add(tmpGoal);

            JakeCreateGoal();
        }


        public static void JakeCreateGoal()
        {
            GoalsAndQuestions tmpGoal = CreateNewPost("Quit Smoking Idiot");
            UserQuestions tmpQ = tmpGoal.CreateNewQuestion("Cigarettes per day ");
            tmpQ.InputControlTypes.Add("Entry;3");

            tmpQ = tmpGoal.CreateNewQuestion("Are you an indoor smoker ");
            tmpQ.InputControlTypes.Add("Entry;Yes/No");

            tmpQ = tmpGoal.CreateNewQuestion("Will you meet the app half way ");
            tmpQ.InputControlTypes.Add("Entry;Yes");
            tmpQ.InputControlTypes.Add("Entry;No");

            tmpQ = tmpGoal.CreateNewQuestion("How many weeks will you give yourself before you give up on this app ");
            tmpQ.InputControlTypes.Add("Radio;1 week");
            tmpQ.InputControlTypes.Add("Radio;TWO WEEKS");
            tmpQ.InputControlTypes.Add("Radio;More");
            tmpQ.InputControlTypes.Add("Radio;I already gave up");


            App.AllAvailableGoals.Add(tmpGoal);

            //FirebaseClient firebaseClient = new FirebaseClient("https://twoweeks-89c57-default-rtdb.firebaseio.com");
            //irebaseClient.Child("GoalsAndQuestions").PostAsync(tmpGoal);




        }

        public static GoalsAndQuestions CreateNewPost(string goal)
        {
            GoalsAndQuestions tmpGoal = new GoalsAndQuestions()
            {
                Name = goal,
                Questions = new List<UserQuestions>()
            };
            return tmpGoal;
        }

        public UserQuestions CreateNewQuestion(string question)
        {
            UserQuestions tmpQ = new UserQuestions
            {
                Question = question,
                InputControlTypes = new List<string>()
            };
            Questions.Add(tmpQ);
            return tmpQ;
        }
    }

    public class UserQuestions
    {
        public string Question { get; set; }

        public List<string> InputControlTypes { get; set; }

    }


}
