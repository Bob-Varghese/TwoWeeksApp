//using AndroidX.Lifecycle;
using Microsoft.Maui.Controls;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Database.Query;

namespace TwoWeeksApp;

public partial class UserQuestionaire : ContentPage
{
    GoalsAndQuestions selectedGoal;

    Dictionary<string, Entry> Entries = new Dictionary<string, Entry>();
    Dictionary<string, RadioButton> Radios = new Dictionary<string, RadioButton>();
    Dictionary<string, string> Answers = new Dictionary<string, string>();

    FirebaseClient firebaseClient = new FirebaseClient("https://twoweeks-89c57-default-rtdb.firebaseio.com");

    int pagenum = 0;
    int maxControls = 16;

    string[] tmp = new string[14];


    public UserQuestionaire()
	{
		InitializeComponent();
	}

    public UserQuestionaire(GoalsAndQuestions goal)
	{
        InitializeComponent();
        selectedGoal = goal;
        goalLabel.Text = goal.Name;

        LoadForm();
    }


    private void LoadForm()
    {

        int cntr = 0;
        int startForm = 0;
        bool blnDone = false;

        foreach (var item in selectedGoal.Questions)
        {
            int totalControls = cntr + 1 + item.InputControlTypes.Count;
            startForm += 1 + item.InputControlTypes.Count;

            if (totalControls > maxControls)
            {
                cntr = maxControls;
                blnDone = true;
            }
            else if (startForm < (pagenum * maxControls))
            {

            }
            else
            {

                Label lbl = new Label();
               // lbl.FontSize = goalLabel.FontSize;
                lbl.Text = item.Question;

                MainGrid.Add(lbl, 0, cntr);
                cntr++;

                foreach (var itm in item.InputControlTypes)
                {
                    char[] delimiterChars = { ';' };
                    string[] words = itm.Split(delimiterChars);

                    if (words[0] == "Entry")
                    {
                        Entry entry = new Entry();
                        string key = item.Question + words[1];
                        if (!Entries.ContainsKey(key))
                        {
                            entry.Placeholder = words[1];
                            entry.HorizontalOptions = LayoutOptions.Center;

                            //Label label = new Label();
                            //label.SetBinding(label.textpro)
                            //label.BindingContext = viewmodel = new PersonViewModel();
                            //label.SetBinding<PersonViewModel>(Label.TextProperty, vm => vm.Name, mode: BindingMode.OneWay);
                            //Answers.Add(item.Question + words[1], "");
                            Answers[key] = "";
                            //tmp[cntr] = ""
                            Binding bnd = new Binding();
                            //tmp[cntr - 1] = "";
                            bnd.Source = Answers[key];

                            bnd.Mode = BindingMode.TwoWay;
                            
                            //bnd.
                            //entry.BindingContext = Answers[key];
                            entry.SetBinding(Entry.TextProperty, bnd);
                            
                            //entry.BindingContextChanged +=  new EventHandler(BindingContext_Changed);
                           

                            Entries.Add(item.Question + words[1], entry);
                        }
                        else
                        {
                            entry = Entries[key];
                        }
                        MainGrid.Add(entry, 0, cntr);
                        cntr++;
                    }

                    if (words[0] == "Radio")
                    {
                        RadioButton rb = new RadioButton();
                        rb.Content = words[1];
                        rb.GroupName = item.Question;
                        if (!Radios.ContainsKey(words[1]))
                        {
                            Radios.Add(words[1], rb);
                        }
                        MainGrid.Add(rb, 0, cntr);
                        cntr++;
                    }
                }

                Label lbl2 = new Label();
                //lbl2.FontSize = goalLabel.FontSize;
                lbl2.Text = " ";

                MainGrid.Add(lbl2, 0, cntr);
                cntr++;
            }
        }

        if (pagenum > 0)
        {
            Button button = new Button();
            button.Text = "Prev";
            button.WidthRequest = 100;
            button.HorizontalOptions = LayoutOptions.Start;
            button.Clicked += PrevPage;
            MainGrid.Add(button, 0, cntr);
        }

        if (blnDone)
        {
            Button button2 = new Button();
            button2.Text = "Next";
            button2.WidthRequest = 100;
            button2.HorizontalOptions = LayoutOptions.End;
            button2.Clicked += NextPage;
            MainGrid.Add(button2, 0, cntr);
        }
    }

    private void BindingContext_Changed(object sender, EventArgs e)
    {
        Console.WriteLine("BindingContext changed");
    }

    public void NextPage(object sender, EventArgs e)
    {
        pagenum++;
        MainGrid.Clear();
        LoadForm();
    }
    public void PrevPage(object sender, EventArgs e)
    {
        pagenum--;
        MainGrid.Clear();
        LoadForm();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {

        foreach (KeyValuePair<string, Entry> ent in Entries)
        {
            Answers[ent.Key] = ent.Value.Text.ToString();
        }

        foreach (KeyValuePair<string, RadioButton> rad in Radios)
        {
            if (rad.Value.IsChecked) 
            {
                Answers.Add(rad.Value.GroupName.ToString(), rad.Key);
            }
        }

        Dictionary<string, Dictionary<string, string>> db = new Dictionary<string, Dictionary<string, string>>();
        db.Add(MainPage.username.Replace(".", "_"), Answers);

        await firebaseClient.Child("Users").PostAsync(db);
        //await firebaseClient.Child("Users").Child(MainPage.username).PostAsync(Answers);

        await Navigation.PushAsync(new Calendar());
    }


}