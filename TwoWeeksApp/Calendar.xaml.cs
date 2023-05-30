using Firebase.Storage;
using MauiAudio;
using Plugin.Maui.Audio;
//using MauiAudio.Platforms.Android;
using System.Globalization;
using System.Runtime.Serialization;
//using static Android.Icu.Text.Transliterator;
//using Android.Media;

namespace TwoWeeksApp;

public partial class Calendar : ContentPage
{
    //private NativeAudioService audioService;
    private AudioManager am;
    private AudioPlayer ap;

    public Calendar()
	{
		InitializeComponent();

        Label lblSunday = new Label();
        lblSunday.Text = "Sun";
        MainGrid.Add(lblSunday, 0, 0);

        Label lblMonday = new Label();
        lblMonday.Text = "Mon";
        MainGrid.Add(lblMonday, 1, 0);

        Label lblTuesday = new Label();
        lblTuesday.Text = "Tues";
        MainGrid.Add(lblTuesday, 2, 0);

        Label lblWednesday = new Label();
        lblWednesday.Text = "Wed";
        MainGrid.Add(lblWednesday, 3, 0);

        Label lblThursday = new Label();
        lblThursday.Text = "Thurs";
        MainGrid.Add(lblThursday, 4, 0);

        Label lblFriday = new Label();
        lblFriday.Text = "Fri";
        MainGrid.Add(lblFriday, 5, 0);

        Label lblSaturday = new Label();
        lblSaturday.Text = "Sat";
        MainGrid.Add(lblSaturday, 6, 0);


        DateTime dt = DateTime.Now;
        DayOfWeek dOfW = dt.DayOfWeek;
        int dw = ((int)dOfW);
        //int start = dw
        int dNum = dt.Day;

        monthLabel.Text = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(dt.Month); 

        for (int i = dw; i < (20 + dw); i++)
        {
            Label lbl = new Label();
            //lbl.FontSize = experienceEntry.FontSize;
            int mod = DateTime.DaysInMonth(dt.Year, dt.Month) + 1;
            int d2 = (dNum % mod) + (dNum / mod);
            lbl.Text = String.Format("{0}", d2);
            if ((dNum - dt.Day) < 3)
            {
                lbl.BackgroundColor = Colors.LightSalmon;
            }
            else if ((dNum - dt.Day) < 17)
            {
                lbl.BackgroundColor = Colors.LightGray;
            }
            else if ((dNum - dt.Day) < 18)
            {
                lbl.BackgroundColor = Colors.Gold;
            }

            int r = i / 7;

            if (dNum == dt.Day) 
            {
                ColAnim(lbl);
                var lbl_tap = new TapGestureRecognizer();
                lbl_tap.Tapped += (s, e) =>
                {
                    //
                    PlayAudio();
                    //
                };
                lbl.GestureRecognizers.Add(lbl_tap);
            }

            MainGrid.Add(lbl, i % 7, r + 1);
            dNum++;
        }

        //PlayAudio();
    }

    private void Today_Clicked(object sender, EventArgs e)
    {

    }

    public async void PlayAudio()
    {
        await PlayAsync("");
    }

    public async Task PlayAsync(string url)
    {

        if (ap != null)
        {
            ap.Stop();
        }

        FirebaseStorage storage = new FirebaseStorage("twoweeks-89c57.appspot.com");//file_example_MP3_700KB
        FirebaseStorageReference starsRef = storage.Child("file_example_MP3_700KB.mp3");

        string link = await starsRef.GetDownloadUrlAsync();
        if (link != null)
        {
            var client = new HttpClient();
            var uri = new Uri(link);
            var response = await client.GetAsync(link);
            response.EnsureSuccessStatusCode();

            am = new AudioManager();
            ap = (AudioPlayer)am.CreatePlayer(await response.Content.ReadAsStreamAsync());
            ap.Play();
        }
    }

    public async Task DoWork1()
    {
        await monthLabel.ColorTo(Colors.Green, Colors.Black, c => monthLabel.BackgroundColor = c, 5000);
    }

    public async Task DoWork2()
    {
        await monthLabel.ColorTo(Colors.Red, Colors.White, c => monthLabel.TextColor = c, 5000);
    }

    public async void ColAnim(Label lbl)
    {
        do
        {
            await lbl.ColorTo(Colors.White, Colors.LightSalmon, c => lbl.BackgroundColor = c, 500);
            await lbl.ColorTo(Colors.LightSalmon, Colors.LightBlue, c => lbl.BackgroundColor = c, 500);
            await lbl.ColorTo(Colors.LightBlue, Colors.LightGreen, c => lbl.BackgroundColor = c, 500);
            await lbl.ColorTo(Colors.LightGreen, Colors.Yellow, c => lbl.BackgroundColor = c, 500);
            await lbl.ColorTo(Colors.Yellow, Colors.White, c => lbl.BackgroundColor = c, 500);
        } while (true);

        //var tasks = new List<Task<long>>();
        //tasks.Add(Task.Run(monthLabel.ColorTo(Colors.Green, Colors.Black, c => monthLabel.BackgroundColor = c, 5000)));
        //await Task.WhenAll(DoWork1(), DoWork2()).Wait();

        //await DoWork1();
        //await DoWork2();

        //        await Task.WhenAll(
        //  DoWork1(),
        //  DoWork2());


        //        await Task.WhenAll(
        //  monthLabel.ColorTo(Colors.Green, Colors.Black, c => monthLabel.BackgroundColor = c, 5000),
        //  monthLabel.ColorTo(Colors.Red, Colors.White, c => monthLabel.TextColor = c, 5000));
        // await this.ColorTo(Color.FromRgb(0, 0, 0), Color.FromRgb(255, 255, 255), c => BackgroundColor = c, 5000);
        //await boxView.ColorTo(Colors.Blue, Colors.Red, c => boxView.Color = c, 4000);
    }
}


public static class ViewExtensions
{
    public static Task<bool> ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
    {
        Func<double, Color> transform = (t) =>
            Color.FromRgba(fromColor.Red + t * (toColor.Red - fromColor.Red),
                           fromColor.Green + t * (toColor.Green - fromColor.Green),
                           fromColor.Blue + t * (toColor.Blue - fromColor.Blue),
                           fromColor.Alpha + t * (toColor.Alpha - fromColor.Alpha));
        return ColorAnimation(self, "ColorTo", transform, callback, length, easing);
    }

    public static void CancelAnimation(this VisualElement self)
    {
        self.AbortAnimation("ColorTo");
    }

    static Task<bool> ColorAnimation(VisualElement element, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
    {
        easing = easing ?? Easing.Linear;
        var taskCompletionSource = new TaskCompletionSource<bool>();

        element.Animate<Color>(name, transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
        return taskCompletionSource.Task;
    }
}



