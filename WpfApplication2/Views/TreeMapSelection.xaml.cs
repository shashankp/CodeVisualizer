﻿using System;
using System.Collections.Generic;
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
}
