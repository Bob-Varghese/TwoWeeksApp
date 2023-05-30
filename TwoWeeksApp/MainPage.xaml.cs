//using Android.Media;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using MauiAudio;
using Newtonsoft.Json;
//using Org.Apache.Http.Authentication;
using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
//using static Android.Icu.Text.Transliterator;

namespace TwoWeeksApp;

public partial class MainPage : ContentPage
{
	int count = 0;

    public Button button;
    public Button NewUser;
    public Entry entry;
    public Entry Pass;

    public static string username = "";

    FirebaseClient firebaseClient = new FirebaseClient("https://twoweeks-89c57-default-rtdb.firebaseio.com");
    public string webAPIKey = "AIzaSyBGH27ITcQyTadJO2zLx9H_tgwJ-xKoAFk\r\n";
    public ObservableCollection<UserRecord> DatabaseRecords { get; set; } = new ObservableCollection<UserRecord>();

    //public List<UserRecord> UserRecords2 { get; set; } = new List<UserRecord>();

    public MainPage()
	{
		InitializeComponent();

        img1.FadeTo(0, 3000, Easing.CubicIn);
        img3.FadeTo(0, 3000, Easing.CubicIn);

        entry = new Entry();
        entry.Placeholder = "Email";
        entry.BackgroundColor = Colors.White;
        entry.HorizontalOptions = LayoutOptions.Fill;
        entry.HorizontalTextAlignment = TextAlignment.Center;
        //MainGrid.Children.Add(entry);
        MainGrid.Add(entry, 0, 3);
        entry.Opacity = 0;
        entry.FadeTo(1, 4000, Easing.CubicIn);

        Pass = new Entry();
        Pass.Placeholder = "Password";
        Pass.HorizontalOptions = LayoutOptions.Fill;
        Pass.HorizontalTextAlignment = TextAlignment.Center;
        Pass.BackgroundColor = Colors.White;
        MainGrid.Add(Pass, 0, 4);
        Pass.Opacity = 0;
        Pass.FadeTo(1, 4000, Easing.CubicIn);

        button = new Button();
        button.Text = "Login";
        button.WidthRequest = 250;
        button.BackgroundColor = Colors.LightBlue;
        button.HorizontalOptions = LayoutOptions.Center;
        button.Clicked += loginButton_Clicked;
        MainGrid.Add(button, 0, 5);
        button.Opacity = 0;
        button.FadeTo(1, 5000, Easing.CubicIn);

        NewUser = new Button();
        NewUser.Text = "Create New User";
        NewUser.WidthRequest = 250;
        NewUser.BackgroundColor = Colors.DeepSkyBlue;
        NewUser.HorizontalOptions = LayoutOptions.Center;
        NewUser.Clicked += NewUserButton_Clicked;
        MainGrid.Add(NewUser, 0, 6);
        NewUser.Opacity = 0;
        NewUser.FadeTo(1, 5000, Easing.CubicIn);

    }


    private void NewUserButton_Clicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new CreateGoals());
        //return;

        if (button.IsEnabled)
        {
            button.IsEnabled = false;
            entry.Text = "";
            entry.Placeholder = "CREATE NEW USER";
            Pass.Text = "";
            Pass.Placeholder = "CREATE NEW PASSWORD";
        }
        else
        {
            bool isEmailEmpty = string.IsNullOrEmpty(entry.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(Pass.Text);
            if (isEmailEmpty || isPasswordEmpty)
            {

            }
            else
            {
                AuthenticateEmail(entry.Text.ToString(), Pass.Text.ToString());
                //AuthenticateNewUser(entry.Text.ToString(), Pass.Text.ToString());
            }
        }
    }

    private async void AuthenticateLogin(string user, string pass)
    {
        //await PlayAsync("");


        var authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        try
        {
            FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(user, pass);

            if (auth.User.IsEmailVerified)
            {
                var content = await auth.GetFreshAuthAsync();
                var serializedContent = JsonConvert.SerializeObject(content);
                Preferences.Set("FreshFirebaseToken", serializedContent);
                App.Current.MainPage = new UserGoal();
            }
            else
            {
                Pass.Text = "";
                Pass.Placeholder = "User Email not verified";
            }

            //await this._navigation.PushAsync(new Dashboard());
        }
        catch (Exception ex)
        {
            string reason = ex.Message.Split("Reason:").LastOrDefault<string>();
            entry.Text = "";
            string res2 = reason.Split("-->").FirstOrDefault<string>();
            entry.Placeholder = res2;


            //await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            //throw;
        }

    }

    private async void AuthenticateEmail(string user, string pass)
    {
        try
        {
            NewUser.IsEnabled = false;
            var AuthProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
            var auth = await AuthProvider.CreateUserWithEmailAndPasswordAsync(user, pass, "", true);
            string token = auth.FirebaseToken;

        if (token != null)
            {
                entry.Text = "";
                entry.Placeholder = "User Registered successfully";
                Pass.Text = "";
                Pass.Placeholder = "Verification email sent";
                button.IsEnabled = true;
                //NewUser.IsEnabled = false;

            }
            //await App.Current.MainPage.DisplayAlert("Alert", "User Registered successfully", "OK");

        }
        catch (Exception ex)
        {
            //await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "OK");
            //throw;
            string reason = ex.Message.Split("Reason:").LastOrDefault<string>();
            entry.Text = "";
            string res2 = reason.Split("-->").FirstOrDefault<string>();
            entry.Placeholder = res2;

        }

    }

    private void loginButton_Clicked(object sender, EventArgs e)
    {
        bool isEmailEmpty = string.IsNullOrEmpty(entry.Text);
        bool isPasswordEmpty = string.IsNullOrEmpty(Pass.Text);
        if (isEmailEmpty || isPasswordEmpty)
        {

        }
        else
        {
            username = entry.Text.ToString();
            AuthenticateLogin(entry.Text.ToString(), Pass.Text.ToString());
            //GetPassword(entry.Text.ToString(), Pass.Text.ToString());
            //Navigation.PushAsync(new NewTravelPage());
        }

    }

    private async void AuthenticateNewUser(string user, string pass)
    {
        IReadOnlyCollection<Firebase.Database.FirebaseObject<UserRecord>> items = await firebaseClient
            .Child("Users")
            .Child(user)
            .OnceAsync<UserRecord>();

        if (items.Count == 0)
        {
            _ = firebaseClient.Child("Users").Child(user).PostAsync(new UserRecord
            {
                MyProperty = pass
            });
            App.Current.MainPage = new UserGoal();
        }
        else
        {
            entry.Text = "";
            entry.Placeholder = "USERNAME ALREADY TAKEN";
            Pass.Text = "";
            Pass.Placeholder = "CREATE NEW PASSWORD";
        }

    }

    private async void GetPassword(string user, string pass)
    {
        IReadOnlyCollection<Firebase.Database.FirebaseObject<UserRecord>> items = await firebaseClient
            .Child("Users")
            .Child(user)
            .OnceAsync<UserRecord>();

        if (items.Count == 0)
        {
            //Xamarin.Forms.mMessageBox.Show("");
            entry.Text = "";
            entry.Placeholder = "USERNAME NOT FOUND";
            //Device.InvokeOnMainThreadAsync(() =>
            //// {
            // });
        }
        else
        {
            foreach (var job in items)
            {
                DatabaseRecords.Add(job.Object);
            }

            UserRecord tmp = DatabaseRecords.LastOrDefault<UserRecord>();
            if (tmp != null)
            {
                if (tmp.MyProperty == pass)
                {
                    //Navigation.PushAsync(new NewTravelPage());
                    //Application.Current.MainPage = new ProfilePage();
                    // = new NewTravelPage();
                }
                else
                {
                    Pass.Text = "";
                    Pass.Placeholder = "PASSWORD IS INCORRECT";
                }
            }
        }
    }



    private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;




		//SemanticScreenReader.Announce(CounterBtn.Text);
	}


    //private readonly INativeAudioService audioService;
    public async Task PlayAsync(string url)
    {
        try
        {

            FirebaseStorage storage = new FirebaseStorage("twoweeks-89c57.appspot.com");//file_example_MP3_700KB

            FirebaseStorageReference starsRef = storage.Child("file_example_MP3_700KB.mp3");
            FirebaseStorageReference img = storage.Child("icon.png");
            string link = await starsRef.GetDownloadUrlAsync();
            string ig = await img.GetDownloadUrlAsync();
            //link = "C:\\Users\\boaz8\\Downloads\\file_example_MP3_700KB.mp3";
            //NativeAudioService audioService = new NativeAudioService();

            MediaPlay mp = new MediaPlay();
            mp.Name = url;
            mp.Author = url;
            mp.URL = link;
            mp.Image = ig;


            //await audioService.InitializeAsync(mp);


            //await audioService.PlayAsync();
            var client = new HttpClient();
            var uri = new Uri(link);
            var response = await client.GetAsync(link);
            response.EnsureSuccessStatusCode();


            AudioManager am = new AudioManager();
            AudioPlayer ap = (AudioPlayer)am.CreatePlayer(await response.Content.ReadAsStreamAsync());
            //AudioPlayer ap = (AudioPlayer)am.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(link));
            ap.Play();

            // AudioPlayer ap = new AudioPlayer();response.Content.ReadAsStreamAsync()



        }
        catch (Exception ex)
        {
            string err = ex.Message;
        }


    }

}

public class MusicPlayerPageViewModel
{
    readonly IAudioManager audioManager;
    IAudioPlayer audioPlayer;

    public MusicPlayerPageViewModel(IAudioManager audioManager)
    {
        this.audioManager = audioManager;
    }

    public async Task Load()
    {
        // Load the audio stream from the application
        Stream audioStream = await FileSystem.OpenAppPackageFileAsync("s.mp3");

        // Create the player
        audioPlayer = audioManager.CreatePlayer(audioStream);

        audioPlayer.Play();
    }
}

