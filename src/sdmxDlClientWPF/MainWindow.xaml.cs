﻿using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
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

namespace sdmxDlClientWPF
{
    public partial class MainWindow : Window, IViewFor<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = ViewModelLocator.Main;
        }

        public MainViewModel? ViewModel { get; set; }
        object? IViewFor.ViewModel { get => ViewModel; set => ViewModel = (MainViewModel?) value; }
    }
}