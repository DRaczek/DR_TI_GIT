using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DR_TI_GIT
{
    public partial class App : Application
    {

        public static string savesPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public App()
        {
            InitializeComponent();

            MainPage = new  AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
