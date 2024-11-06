using HtmlAgilityPack;
using Microsoft.Web.WebView2.Core;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using System.Windows.Threading;

namespace Web_Browser
{
    public partial class MainWindow : Window
    {
        //  ########## for all file access ##########
        public static MainWindow window;

        public MainWindow()
        {
            window = this;
            InitializeComponent();
        }
        //  ########### current window size #######
        public int windowWidth = 0;
        public int windowHeight = 0;
        //  ########### main Window loading event ##########

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.Close();
        }

        private void MaximizeRestoreClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            if (window.WindowState == System.Windows.WindowState.Normal)
            {
                window.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                window.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.WindowState = System.Windows.WindowState.Minimized;
        }

        private async void windowLoad(object sender, RoutedEventArgs e)
        {
            windowWidth = (int)Width;
            windowHeight = (int)Height;
            //  ########### for Add_Tab function call other thread #############
            await Task.Run(() => Add_Tab("https://www.google.com"));
        }

        public  void Add_Tab(string link)
        {
            //  ########### because this function another thread so use Invoke ############## 
            Application.Current.Dispatcher.Invoke((Action)async delegate
            {
                TabItem tab = new TabItem();
                withCloseBtnHeader closeBtnHeader = new withCloseBtnHeader();
                Frame frame = new Frame();
                mainComonant comonant = new mainComonant();
                mainComonant.mainComonant_.tabrecoitem = tab;
                 comonant.webengin.Source = new Uri(link);

                //  ########### create closeBtn click event like tab_close #############
                tab.Width = 200;
                tab.Height = 30;
                closeBtnHeader.CloseBtn.Click += (o, e) =>
                {
                    tabControl.Items.Remove(tab);
                    comonant.webengin.Dispose();
                    int count = tabControl.Items.Count;
                    if(windowWidth-200 > 200*count)
                    {

                    addBtn.Margin = new Thickness(addBtn.Margin.Left - tab.Width, addBtn.Margin.Top, addBtn.Margin.Right, addBtn.Margin.Bottom);
                    }
                    else
                    {

                    foreach (TabItem item in tabControl.Items)
                    {
                        item.Width = ((addBtn.Margin.Left) / count);
                    }
                    } 
                };
                tab.IsSelected = true;
                //  first add mainComonant as comonant on frame.content
                frame.Content = comonant;
                // then add withCloseBtnHeader as closeBtnHeader on tab.header
                tab.Header = closeBtnHeader;
                // then add frame on tab.contant
                tab.Content = frame;
                // then add tab on tabControl
                tabControl.Items.Add(tab);
                if (addBtn.Margin.Left < windowWidth - 210)
                {
                    addBtn.Margin = new Thickness(addBtn.Margin.Left + 200, addBtn.Margin.Top, addBtn.Margin.Right, addBtn.Margin.Bottom);
                }
                else
                {
                    int count = tabControl.Items.Count;
                    foreach (TabItem item in tabControl.Items)
                    {
                        item.Width = ((addBtn.Margin.Left) / count);
                    }
                }
            });
        }

        private async void addBtnClick(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => Add_Tab("https://www.google.com"));
        }
        public bool cond = false;
        public void mainWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            windowHeight = (int)e.NewSize.Height;
            windowWidth = (int)e.NewSize.Width;
            if (cond)
            {
                mainComonant.mainComonant_.bookmarkAdd_fromDatabase();
                mainComonant.mainComonant_.bgtext.Width = windowWidth - 400;
            }
        }

        private void windowClosed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }


    }
}
