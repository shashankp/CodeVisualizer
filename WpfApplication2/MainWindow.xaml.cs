using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var data = TfsHelper.GetData(@"C:\tfs\Dev\Concert\Web\TRTA.Concert.EmulateProductService\TRTA.Concert.EmulateProductService\Controllers",
                @"$/WorkflowTools/Dev/Concert/Web/TRTA.Concert.EmulateProductService/TRTA.Concert.EmulateProductService/Controllers");
            this.DataContext = data;
        }

        //private void Button1_OnClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var data = TfsHelper.GetData(tbSolutionPath.Text, tbTfsPath.Text);
        //        dataGrid.ItemsSource = data;
        //        treeMap1.ItemsSource = data;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}
    }
}
