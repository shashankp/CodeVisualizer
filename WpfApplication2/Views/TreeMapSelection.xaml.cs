using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication2.Models;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for TreeMapSelection.xaml
    /// </summary>
    public partial class TreeMapSelection : UserControl
    {
        private List<TfsItemViewModel> SubItems;
         
        public TreeMapSelection()
        {
            InitializeComponent();
            this.DataContext = TfsHelper.GetData();
            SubItems = TfsHelper.GetData();
        }
    }

    public class ScoreToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value as TfsItemViewModel;
            if (result != null)
            {
                var score = result.BugCount;
                SolidColorBrush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF0C6BBF"));
                if (score >= 7)
                {
                    brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE10000"));
                } else if (score >= 2 && score < 7)
                {
                    brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFE100"));
                }

                return brush;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
