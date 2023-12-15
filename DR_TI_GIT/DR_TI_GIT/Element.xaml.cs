using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DR_TI_GIT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Element : Grid
    {
        public int GridCol { get; set; }
        public int GridRow { get; set; }

        public Label Label
        {
            get { return labelControl; }
            set { labelControl = value; }
        }

        public ImageButton ImageButton
        {
            get { return imgButton; }
            set { imgButton = value; }
        }
        public bool IsInCorrectPosition(int boardSize)
        {
            return Convert.ToInt32(Label.Text) == GridCol + 1 + (GridRow * boardSize);
        }

        public Element()
        {
            InitializeComponent();
        }

        public static Element create(ref List<Element> elements,
            TapGestureRecognizer tapGestureRecognizer,
            int col,
            int row,
            string value)
        {
            Element e = new Element();
            elements.Add(e);
            e.GestureRecognizers.Add(tapGestureRecognizer);
            e.GridCol = col;
            e.GridRow = row;
            e.SetBinding(Grid.HeightRequestProperty, new Binding(".Width", source: e));
            Grid.SetRow(e, row);
            Grid.SetColumn(e, col);
            e.labelControl.Text = value;
            return e;
        }
    }
}