﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Web_Browser
{
    /// <summary>
    /// Interaction logic for withCloseBtnHeader.xaml
    /// </summary>
    public partial class withCloseBtnHeader : UserControl
    {
        public static withCloseBtnHeader withCloseBtnHeader_;
        public withCloseBtnHeader()
        {
            withCloseBtnHeader_ = this;
            InitializeComponent();
        }

    }
}
