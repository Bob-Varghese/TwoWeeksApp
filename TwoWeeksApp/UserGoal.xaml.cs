namespace TwoWeeksApp;

public partial class UserGoal : ContentPage
{
	public UserGoal()
	{
		InitializeComponent();

        List<string> data = new List<string>();
        foreach (var item in App.AllAvailableGoals)
        {
            data.Add(item.Name);
        }
        goalListView.ItemsSource = data;

    }

    private void Item_Clicked(object sender, EventArgs e)
    {
        var selectedPost = goalListView.SelectedItem as String;
        //Navigation.PushAsync(new PostDetailPage(selectedPost));
        GoalsAndQuestions goal = new GoalsAndQuestions()
        {
            Name = selectedPost // goalEntry.Text
        };

        foreach (var item in App.AllAvailableGoals)
        {
            if (item.Name == selectedPost)
            {
                goal = item;
            };
        }

        //Navigation.PushAsync(new PostDetailPage(post));
        App.Current.MainPage = new NavigationPage(new UserQuestionaire(goal));


    }


}