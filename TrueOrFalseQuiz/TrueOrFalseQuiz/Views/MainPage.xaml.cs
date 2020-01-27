using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueOrFalseQuiz.Views;
using Xamarin.Forms;

namespace TrueOrFalseQuiz
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void StartQuiz(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QuizPage());
        }
    }
}
