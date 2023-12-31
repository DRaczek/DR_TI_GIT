﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DR_TI_GIT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(FileInfo), "fileinfo")]
    public partial class GemPuzzleGame : ContentPage
    {
        private string fileInfo;
        private string fileInfoName;
        public string FileInfo
        {
            set
            {
                if (!string.IsNullOrEmpty(fileInfo) && value == null)
                {
                    Intialize();
                    fileInfoName = null;
                }
                fileInfo = value;
                // split na / a pozniej na . zeby pobrac nazwe pliku bez rozszerzenia
                if (fileInfo != null) fileInfoName = (new List<string>(fileInfo.Split('/')).Last()).Split('.').First();
                if (value != null) LoadFromFile(value);
            }
        }

        TapGestureRecognizer gestureRecognizer = new TapGestureRecognizer();
        int size = 4;
        int emptyRow = 0, emptyCol = 0;
        private uint animationDurationMS = 200;
        List<Element> elements = new List<Element>();
        public GemPuzzleGame()
        {
            InitializeComponent();
            gestureRecognizer.Tapped += GestureRecognizer_Tapped;
            Intialize();
        }

        private void Intialize()
        {
            grid.Children?.Clear();
            emptyCol = emptyRow = size - 1;
            CreateGame();
        }

        private void CreateGame()
        {
            DefineRowColDefinitons();

            List<int> tilesNumbers = new List<int>();
            for (int j = 0; j < (size * size) - 1; j++) tilesNumbers.Add(j + 1);

            //x sprawdza czy już koniec pętli, tj liczy od 0 do 15 elementu
            //row i col to indexy w gridzie 4x4
            for (int x = 0, col = 0, row = 0; x < (size * size) - 1; x++)
            {
                Random rand = new Random();
                int index = rand.Next(0, tilesNumbers.Count);
                string value = tilesNumbers[index].ToString();
                tilesNumbers.RemoveAt(index);

                Element e = Element.create(ref elements, gestureRecognizer, col, row, value);
                grid.Children.Add(e);

                col++;
                if (col == size)
                {
                    row++; col = 0;
                }
            }
        }

        private async void GestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Element clicked = sender as Element;
            
            if ((Math.Abs(clicked.GridCol - emptyCol) == 1 && Math.Abs(clicked.GridRow - emptyRow) == 0) ||
                (Math.Abs(clicked.GridCol - emptyCol) == 0 && Math.Abs(clicked.GridRow - emptyRow) == 1))
            {
                await clicked.FadeTo(0, animationDurationMS, Easing.SpringIn);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay((int)animationDurationMS);
                    Grid.SetColumn(clicked, emptyCol);
                    Grid.SetRow(clicked, emptyRow);
                    int tmp = emptyRow;
                    emptyRow = clicked.GridRow;
                    clicked.GridRow = tmp;
                    tmp = emptyCol;
                    emptyCol = clicked.GridCol;
                    clicked.GridCol = tmp;
                    await clicked.FadeTo(1, animationDurationMS, Easing.SpringOut);
                });
                IsDone();
            }
        }

        private void DefineRowColDefinitons()
        {
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            for (int i = 0; i < size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }
        }

        private bool IsDone()
        {
            var elements = grid.Children;
            if (elements.Where(e => !(e as Element).IsInCorrectPosition(size)).Count() == 0)
            {
                DisplayAlert("", "Wygrałeś !", "OK");
                return true;
            }
            return false;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            string Title = await DisplayPromptAsync("Tytuł", "Podaj nazwe pliku", initialValue: fileInfoName ?? "");
            if (string.IsNullOrEmpty(Title)) return;
            try
            {
                var save = new GameSave()
                {
                    EmptyCol = emptyCol,
                    EmptyRow = emptyRow,
                    Size = size,
                    Tiles = elements.Select(element =>
                    {
                        var tile = new Tile()
                        {
                            GridCol = element.GridCol,
                            GridRow = element.GridRow,
                            Number = Convert.ToInt32(element.Label.Text)
                        };
                        return tile;
                    }).ToList()
                };
                string path = Path.Combine(App.savesPath, Title + ".txt");
                File.Create(path).Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(save));
                await DisplayAlert("Sukces", "Poprawnie zapisano do pliku", "Ok");
            }
            catch (Exception)
            {
                await DisplayAlert("Błąd", "Podczas zapisywania wystąpił błąd", "Ok");
            }
        }

        private void LoadFromFile(string fileName)
        {
            GameSave save = JsonConvert.DeserializeObject<GameSave>(File.ReadAllText(fileName));
            grid.Children.Clear();
            emptyCol = save.EmptyCol;
            emptyRow = save.EmptyRow;
            size = save.Size;
            elements = save.Tiles.Select(tile =>
            {
                Element element = Element.create(ref elements, gestureRecognizer, tile.GridCol, tile.GridRow, tile.Number.ToString());
                grid.Children.Add(element);
                return element;
            }).ToList();
        }
    }
}