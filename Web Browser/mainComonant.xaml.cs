using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HtmlAgilityPack;
using System.Windows.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace Web_Browser
{
    public partial class mainComonant : Page
    {
        public static mainComonant mainComonant_;

        public mainComonant()
        {
            mainComonant_ = this;
            InitializeComponent();
            bgtext.Width = MainWindow.window.windowWidth - 400;
            MainWindow.window.cond = true;
        }
        public TabItem tabrecoitem = null;
        int condi = 0;
        private void documentTitleChangeweb(object? sender, object e)
        {
            if (condi == 0)
            {
                if (!webengin.Source.AbsoluteUri.Contains("accounts.google.com"))
                {
                    usercond = false;
                }
                condi++;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("https://t3.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url=" + webengin.Source.AbsoluteUri + "&size=16");
                image.EndInit();
               
                       
                    var ith = tabrecoitem.Header as withCloseBtnHeader;
                    ith.title.Text = webengin.CoreWebView2.DocumentTitle.ToString().Trim() + "                                ";
                    ith.iconlogo.Source = image;
                    if (webengin.Source.AbsoluteUri != "https://www.google.com/")
                    {
                        dataBase1 dataBase1 = new dataBase1();
                        dataBase1.HaddData(webengin.Source.AbsoluteUri, webengin.CoreWebView2.DocumentTitle.ToString().Trim());
                    }
                
            }
        }

        private void backweb(object sender, RoutedEventArgs e)
        {
            webengin.GoBack();
        }

        private void forwordweb(object sender, RoutedEventArgs e)
        {
            webengin.GoForward();
        }

        private void refreshweb(object sender, RoutedEventArgs e)
        {
            webengin.Reload();

        }

        private void mousedown(object sender, MouseButtonEventArgs e)
        {
            webengin.Stop();
        }
        string userAgent_ = "";
        bool usercond = true;
        private void NavigationStart(object sender, CoreWebView2NavigationStartingEventArgs e)
        {

            if (new Uri(e.Uri).Host.Contains("accounts.google.com"))
            {
                if (webengin.CoreWebView2 != null)
                {
                    var setting = webengin.CoreWebView2.Settings;
                    if (setting.UserAgent != "chrome")
                    {
                        userAgent_ = setting.UserAgent;
                    }
                    setting.UserAgent = "chrome";
                }
                usercond = true;
            }
            else
            {
                var setting = webengin.CoreWebView2.Settings;
                if (setting.UserAgent == "chrome")
                {
                    webengin.CoreWebView2.Settings.UserAgent = userAgent_;
                    usercond = true;
                }
            }
            refbtn.Visibility = Visibility.Collapsed;
            refbtn_Copy.Visibility = Visibility.Visible;
        }
        bool condition = true;
        private void newwindowopen(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            if (condition)
            {
                MainWindow.window.Add_Tab(e.Uri.ToString());
                condition = false;
            }
            else
            {
                condition = true;
            }
            e.Handled = true;
        }
        string accoountname = "";

        private async void NavigationComplete(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (webengin.Source.AbsoluteUri == "https://www.google.com/")
            {
                string linklogo = null;
                linklogo = await webengin.CoreWebView2.ExecuteScriptAsync("document.querySelector('.gbii').getAttribute('src');");
                accoountname = "Google Account \n " + await webengin.CoreWebView2.ExecuteScriptAsync("document.querySelector('.gb_rb').innerText;") + "\n " + await webengin.CoreWebView2.ExecuteScriptAsync("document.querySelector('.gb_be').querySelectorAll('div')[3].innerText;");

                Thread.Sleep(500);
                linklogo = linklogo.Substring(1, linklogo.Length - 2);

                try
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(linklogo);

                    image.EndInit();
                  
                Thread.Sleep(500);

                   ImageBrush imageb = new ImageBrush();
                  imageb.ImageSource = image;
                    profileimage.Background = imageb;
                    profileimage.ToolTip = accoountname;
                    

                }
                catch (Exception p)
                {

                }
            }
            usercond = false;
            webengin.CoreWebView2.NewWindowRequested += newwindowopen;
            refbtn.Visibility = Visibility.Visible;
            refbtn_Copy.Visibility = Visibility.Collapsed;
        }

        private void sourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            if (usercond)
            {
                webengin.Reload();
                usercond = false;
            }
            condi = 0;
            webengin.CoreWebView2.DocumentTitleChanged += documentTitleChangeweb;
            textEdit.Text = webengin.Source.AbsoluteUri;
        }

        private async void maincomPageload(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                bookmarkAdd_fromDatabase();
            });
            t.ApartmentState = ApartmentState.STA;
            t.Start();
        }
        public Button bookmarkbtn;
        public void bookmarkAdd_fromDatabase()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
    (Action)(() =>
    {
        dataBase1 dataBase = new dataBase1();
        List<List<string>> list;
        list = dataBase.getData();
        var context = this.FindResource("BookmarkExtramenue") as RibbonContextMenu;
        var btn = this.FindResource("Buttoneffect") as Style;
        Bookmarkstacpanel.Children.Clear();
        context.Items.Clear();
        for (int i = 0; i < list[0].Count; i++)
        {
            Button butn = new Button();
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            int ind = i;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("https://t3.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url=" + list[0][i] + "&size=50");
            image.EndInit();
            

                butn.Width = 22;
                butn.Height = 22;
                butn.Margin = new Thickness(10, 0, 0, 0);
                butn.VerticalAlignment = VerticalAlignment.Center;
                img.Source = image;
                butn.Content = img;
                butn.ToolTip = "" + list[1][ind] + " , " + list[0][ind] + "";
                butn.Click += (o, e) =>
                {
                    webengin.Source = new Uri(list[0][ind]);
                };
                butn.PreviewMouseRightButtonDown += (o, e) =>
                {
                    bookmarkbtn = o as Button;
                    var context1 = this.FindResource("bookmarkDeletemenu") as RibbonContextMenu;
                    context1.IsOpen = true;
                };
                butn.Style = btn;

                if (((butn.Width + 10) * Bookmarkstacpanel.Children.Count) < MainWindow.window.windowWidth - (32 * 3))
                {
                    Bookmarkstacpanel.Children.Add(butn);
                    bookmarkaddBtn.Visibility = Visibility.Hidden;
                }
                else
                {
                    context.Items.Add(butn);
                    bookmarkaddBtn.Visibility = Visibility.Visible;
                }
            
        }
    }));
        }
        private void bookmarkmoclick(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var context = this.FindResource("bookmarkAddmenu") as RibbonContextMenu;
                context.IsOpen = true;
            }
        }

        private async void BookmarkclickAdd(object sender, RoutedEventArgs e)
        {
            dataBase1 dataBase = new dataBase1();
            dataBase.addData(webengin.Source.AbsoluteUri, webengin.CoreWebView2.DocumentTitle.ToString());
            Thread t = new Thread(() =>
            {
                bookmarkAdd_fromDatabase();
            });
            t.ApartmentState = ApartmentState.STA;
            t.Start();
        }

        private void bookmarkdeleteClick(object sender, RoutedEventArgs e)
        {
            int ind = Bookmarkstacpanel.Children.IndexOf(bookmarkbtn);
            if (ind == -1)
            {
                var context = this.FindResource("BookmarkExtramenue") as RibbonContextMenu;
                ind = context.Items.IndexOf(bookmarkbtn) + Bookmarkstacpanel.Children.Count;
            }
            dataBase1 dataBase = new dataBase1();
            Thread t = new Thread(() =>
            {
                dataBase.deleteData(ind + 1);
                bookmarkAdd_fromDatabase();
            });

            t.Start();
        }

        private void bookmarkExtraBtnClick(object sender, RoutedEventArgs e)
        {
            var context = this.FindResource("BookmarkExtramenue") as RibbonContextMenu;
            context.IsOpen = true;
        }

        private void keyDownevent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string textUri = textEdit.Text;
                if (textUri != "")
                {

                    textUri = textUri.Replace("https://", "");
                    textUri = textUri.Replace("https://www.", "");
                    textUri = textUri.Replace("www.", "");
                    webengin.Source = new Uri("https://www." + textUri);
                }
            }
        }

        private void HistoryShowbtnClick(object sender, RoutedEventArgs e)
        {
            var context = this.FindResource("Historydatashow") as RibbonContextMenu;
            context.IsOpen = true;
            context.MaxWidth = 500;
            context.Items.Clear();
            List<List<string>> strings;
            dataBase1 dataBase = new dataBase1();
            strings = dataBase.HgetData();
            for (int i = 0; i < strings[0].Count; i++)
            {
                int ind = i;
                withCloseBtnHeader withCloseBtn = new withCloseBtnHeader();
                RibbonMenuItem rmi = new RibbonMenuItem();
                BitmapImage btim = new BitmapImage();
                btim.BeginInit();
                btim.UriSource = new Uri("https://t3.gstatic.com/faviconV2?client=SOCIAL&type=FAVICON&fallback_opts=TYPE,SIZE,URL&url=" + strings[0][ind] + "&size=50");
                btim.EndInit();
                

                    withCloseBtn.title.Text = strings[1][ind];
                    withCloseBtn.iconlogo.Source = btim;
                    withCloseBtn.Height = 30;
                    withCloseBtn.MaxWidth = 400;
                    rmi.Header = withCloseBtn;
                    withCloseBtn.CloseBtn.Click += async (o, e) =>
                    {
                        int i = context.Items.IndexOf(rmi);
                        context.Items.Remove(rmi);
                        await Task.Run(() =>
                         {

                             dataBase.HdeleteData(i + 1);
                         });

                    };
                    rmi.Click += (o, e) =>
                    {
                        webengin.Source = new Uri(strings[0][ind]);
                    };
                    context.Items.Add(rmi);
                
            }

        }
        private void login_pageClick(object sender, MouseButtonEventArgs e)
        {
            if (profileimage.ToolTip == accoountname)
            {
                MainWindow.window.Add_Tab("https://myaccount.google.com/");
            }
            else
            {

                MainWindow.window.Add_Tab("https://accounts.google.com/signin/v2/identifier?hl=bn&passive=true&continue=https%3A%2F%2Fwww.google.com%2F&ec=GAZAmgQ&flowName=GlifWebSignIn&flowEntry=ServiceLogin");


            }
        }

        private void historybtnmouseup(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void delallHistorybtnclick(object sender, RoutedEventArgs e)
        {
            dataBase1 d = new dataBase1();
            d.hdeleteallhistory();
        }

        private void historyDeletebtnclick(object sender, MouseButtonEventArgs e)
        {

            if (e.RightButton == MouseButtonState.Pressed)
            {

                var context1 = this.FindResource("HistoryDeletemenu") as RibbonContextMenu;
                context1.IsOpen = true;
            }
        }

        private void clearprofilemenubtnclick(object sender, RoutedEventArgs e)
        {
            webengin.CoreWebView2.Profile.ClearBrowsingDataAsync();
        }

        private void login_pagemusedown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {

                var context1 = this.FindResource("clearprofilemenue") as RibbonContextMenu;
                context1.IsOpen = true;
            }
        }
    }



}
