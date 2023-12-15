using DR_TI_GIT.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace DR_TI_GIT.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}