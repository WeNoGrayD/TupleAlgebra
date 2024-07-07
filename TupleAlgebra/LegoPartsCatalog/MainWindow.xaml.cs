using LegoPartsCatalogApp.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LegoPartsCatalogApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ColorQueryModel _model;

        public MainWindow()
        {
            InitializeComponent();

            lstvColors.DataContext = new ColorQueryViewModel(
                _model = new ColorQueryModel());

            return;
        }

        public void btn_click(object sender, RoutedEventArgs e)
        {
            var vm = lstvColors.DataContext as ColorQueryViewModel;

            vm.MakeQuery();
            var query = _model.Query;

            return;
        }
    }
}