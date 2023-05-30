//using static Android.Content.ClipData;

namespace TwoWeeksApp;

public partial class CreateGoals : ContentPage
{
    Button createButton;
    Button finishButton;
    Editor enterGoal;
    Editor enterQuestion;
    Entry enterLabel;
    Picker pikType;

    GoalsAndQuestions tmpGoal;
    UserQuestions tmpQ;


    int rowCounter = 0;

    public CreateGoals()
	{
		InitializeComponent();

        createButton = new Button();
        createButton.Text = "Create Goal";
        createButton.WidthRequest = 120;
        createButton.BackgroundColor = Colors.DeepSkyBlue;
        createButton.HorizontalOptions = LayoutOptions.Start;
        createButton.Clicked += createButton_Clicked;
        MainGrid.Add(createButton, 0, rowCounter);

        finishButton = new Button();
        finishButton.Text = "Finish Goal";
        finishButton.WidthRequest = 120;
        finishButton.BackgroundColor = Colors.DeepSkyBlue;
        finishButton.HorizontalOptions = LayoutOptions.End;
        finishButton.Clicked += finishButton_Clicked;
        MainGrid.Add(finishButton, 0, rowCounter);
        rowCounter++;

        enterGoal = new Editor();
        enterGoal.Placeholder = "TYPE GOAL";
        //enterGoal.WidthRequest = 300;
        //enterGoal.HeightRequest = 100;
        enterGoal.BackgroundColor = Colors.White;
        enterGoal.HorizontalOptions = LayoutOptions.Fill;
        enterGoal.HorizontalTextAlignment = TextAlignment.Center;
        enterGoal.AutoSize = EditorAutoSizeOption.TextChanges;
        MainGrid.Add(enterGoal, 0, rowCounter);
        rowCounter++;
    }

    private void createButton_Clicked(object sender, EventArgs e)
    {
        if (createButton.Text == "Create Goal")
        {
            tmpGoal = GoalsAndQuestions.CreateNewPost(enterGoal.Text);

            createButton.Text = "Create Question";
            finishButton.Text = "Finish Question";

            enterQuestion = new Editor();
            enterQuestion.Placeholder = "TYPE QUESTION";
            enterQuestion.AutoSize = EditorAutoSizeOption.TextChanges;
            enterQuestion.BackgroundColor = Colors.White;
            enterQuestion.HorizontalOptions = LayoutOptions.Fill;
            enterQuestion.HorizontalTextAlignment = TextAlignment.Center;
            //enterGoal.
            MainGrid.Add(enterQuestion, 0, rowCounter);
            rowCounter++;

        }
        else if (createButton.Text == "Create Question") 
        {
            createButton.Text = "Add Entry";
            finishButton.Text = "Finish Entry";

              tmpQ = tmpGoal.CreateNewQuestion(enterQuestion.Text);

              pikType = new Picker();
              pikType.Items.Add("Entry");
              pikType.Items.Add("Radio");
              //pikType.Items.Add("Editor");
              //pikType.Items.Add("Picker");
              pikType.BackgroundColor = Colors.White;
              pikType.HorizontalOptions = LayoutOptions.Fill;
              pikType.HorizontalTextAlignment = TextAlignment.Center;
              pikType.Title = "Select the User Entry type";
              //pikType.HeightRequest = 30;
              MainGrid.Add(pikType, 0, rowCounter);
              rowCounter++;

/*
            RadioButton rb = new RadioButton();
            rb.Content = "Entry";
            rb.GroupName = "InputType";
            rb.HorizontalOptions = LayoutOptions.Center;
            MainGrid.Add(rb, 0, rowCounter);
            rowCounter++;

            RadioButton rb2 = new RadioButton();
            rb2.Content = "Radio";
            rb2.GroupName = "InputType";
            rb2.HorizontalOptions = LayoutOptions.Center;
            MainGrid.Add(rb2, 0, rowCounter);
            rowCounter++;
*/

            enterLabel = new Entry();
            enterLabel.Placeholder = "TYPE LABEL";
            //enterLabel.WidthRequest = 300;
            //enterLabel.HeightRequest = 100;
            enterLabel.BackgroundColor = Colors.White;
            enterLabel.HorizontalOptions = LayoutOptions.Fill;
            enterLabel.HorizontalTextAlignment = TextAlignment.Center;
            MainGrid.Add(enterLabel, 0, rowCounter);
            rowCounter++;


        }
        else if (createButton.Text == "Add Entry")
        {
            //createButton.Text = "Entry Label";
            //finishButton.Text = "Finish Label";
            string tp = pikType.SelectedItem.ToString();
            string lbl = enterLabel.Text.ToString();
            tmpQ.InputControlTypes.Add(tp + ";" + lbl);

            enterLabel = new Entry();
            enterLabel.Placeholder = "TYPE LABEL";
            //enterLabel.WidthRequest = 300;
            //enterLabel.HeightRequest = 100;
            enterLabel.BackgroundColor = Colors.White;
            enterLabel.HorizontalOptions = LayoutOptions.Fill;
            enterLabel.HorizontalTextAlignment = TextAlignment.Center;
            MainGrid.Add(enterLabel, 0, rowCounter);
            rowCounter++;

        }

        /*     GoalsAndQuestions tmpGoal = CreateNewPost("Quit Smoking, Idiot");
             UserQuestions tmpQ = tmpGoal.CreateNewQuestion("Cigarettes per day: ");
             tmpQ.InputControlTypes.Add("Entry;3");

             tmpQ = tmpGoal.CreateNewQuestion("Are you an indoor smoker?: ");
             tmpQ.InputControlTypes.Add("Entry;Yes/No");

             tmpQ = tmpGoal.CreateNewQuestion("Will you meet the app half way?: ");
             tmpQ.InputControlTypes.Add("Entry;Yes");
             tmpQ.InputControlTypes.Add("Entry;No");
        */


    }

    private void finishButton_Clicked(object sender, EventArgs e)
    {

    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new Calendar());
    }


}