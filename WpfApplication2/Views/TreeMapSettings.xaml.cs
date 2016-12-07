using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfApplication2.Views
{
    /// <summary>
    /// Interaction logic for TreeMapSettings.xaml
    /// </summary>
    public partial class TreeMapSettings : UserControl
    {
        private List<TfsItemViewModel> data; 
        private readonly BackgroundWorker worker = new BackgroundWorker();

        public TreeMapSettings()
        {
            InitializeComponent();

            pbStatus.Visibility = Visibility.Hidden;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        private void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                pbStatus.Visibility = Visibility.Visible;
                worker.RunWorkerAsync(new
                {
                    SlnPath = tbSolutionPath.Text,
                    TfsPath = tbTfsPath.Text
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // run all background tasks here
            data = TfsHelper.SetPathParams(((dynamic)(e.Argument)).SlnPath, 
                                            ((dynamic)(e.Argument)).TfsPath);
        }

        private void worker_RunWorkerCompleted(object sender,
                                       RunWorkerCompletedEventArgs e)
        {
            //update ui once worker complete his work
            pbStatus.Visibility = Visibility.Hidden;
            dataGrid.ItemsSource = data;
        }

    }
}
