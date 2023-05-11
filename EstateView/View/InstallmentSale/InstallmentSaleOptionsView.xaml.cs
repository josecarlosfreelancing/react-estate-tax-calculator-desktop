﻿using EstateView.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace EstateView.View.InstallmentSale
{
    /// <summary>
    /// Interaction logic for InstallmentSaleOptionsView.xaml
    /// </summary>
    public partial class InstallmentSaleOptionsView : UserControl
    {
        public InstallmentSaleOptionsView()
        {
            InitializeComponent();
        }

        private void afr_lookup_Click(object sender, RoutedEventArgs e)
        {
            UrlHelper.LookupAFR();
        }
    }
}
