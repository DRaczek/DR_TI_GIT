using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DR_TI_GIT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<FileInfo> Files { get; set; }
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Update();
        }
        public void Update()
        {
            var directories = new DirectoryInfo(App.savesPath);
            Files=new ObservableCollection<FileInfo>(directories.GetFiles());
            Files.OrderBy(f=>f.LastWriteTime).ToList();
            listView.ItemsSource = Files;
        }

        private void WczytajPrzycisk_Clicked(object sender, EventArgs e)
        {
            FileInfo fileInfo = (sender as Button).CommandParameter as FileInfo;
            AppShell.Current.GoToAsync("//GemPuzzleGame?fileinfo=" + fileInfo.FullName);
        }
    }
}