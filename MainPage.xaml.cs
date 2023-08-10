using System;
using System.Runtime.InteropServices;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace YouTube_TV_on_Windows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SystemNavigationManager currentView;
        public const uint GW_CHILD = 5;
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);


        public MainPage()
        {
            this.InitializeComponent();
            currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            currentView.BackRequested += CurrentView_BackRequested;
            loadYoutube();
            webView.Focus(FocusState.Programmatic);
        }

        private void CurrentView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
            else
            {
                webView.Source = new Uri("https://www.youtube.com/tv");
            }
            webView.Focus(FocusState.Programmatic);
        }

        async void loadYoutube()
        {
            await webView.EnsureCoreWebView2Async();
            // Set some TV user agent to get the TV version of the website
            // https://deviceatlas.com/blog/list-smart-tv-user-agent-strings for different user agents
            webView.CoreWebView2.Settings.UserAgent = "Mozilla/5.0 (compatible; U; NETFLIX) AppleWebKit/533.3 (KHTML, like Gecko) Qt/4.7.0 Safari/533.3 Netflix/3.2 (DEVTYPE=RKU-42XXX-; CERTVER=0) QtWebKit/2.2, Roku 3/7.0 (Roku, 4200X, Wireless)";
            webView.Source = new Uri("https://www.youtube.com/tv");
            webView.CoreWebView2.NavigationCompleted += (s, e) => { focusYoutube(); };
            webView.CoreWebView2.WindowCloseRequested += (s, e) => { Application.Current.Exit(); };
            webView.CoreWebView2.ContextMenuRequested += (s, e) => { };
            webView.Focus(FocusState.Programmatic);
        }

        void focusYoutube()
        {
            webView.Focus(FocusState.Programmatic);
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }

}
