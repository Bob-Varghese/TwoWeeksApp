namespace TwoWeeksApp;

public partial class App : Application
{

    public static List<GoalsAndQuestions> AllAvailableGoals = new List<GoalsAndQuestions>();

    public App()
	{
		InitializeComponent();

        GoalsAndQuestions.LoadAllGoalsAndQuestions();

		MainPage = new AppShell();
    }
}
