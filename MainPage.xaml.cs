using System;
using System.Runtime.InteropServices;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
            Window.Current.Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
        }

        private void CurrentView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
            else
            {
                webView.Source = new Uri("https://www.youtube.com/tv#/");
            }
        }

        async void loadYoutube()
        {
            await webView.EnsureCoreWebView2Async();
            // Set some TV user agent to get the TV version of the website https://deviceatlas.com/blog/list-smart-tv-user-agent-strings
            webView.CoreWebView2.Settings.UserAgent = "Mozilla/5.0 (WebOS; SmartTV) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5283.0 Safari/537.36";
            // Disable the context menu
            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webView.CoreWebView2.Settings.AreDevToolsEnabled = false;
            webView.Source = new Uri("https://www.youtube.com/tv#/");
            webView.CoreWebView2.NavigationCompleted += (s, e) => {
                // Show the back button when WebView is loaded
                currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                // Focus the WebView when it is loaded
                webView.Focus(FocusState.Programmatic);
            };
            // Close app when Exit YouTube button is clicked
            webView.CoreWebView2.WindowCloseRequested += (s, e) => { Application.Current.Exit(); };
        }
        private void Dispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.F11 && args.EventType == CoreAcceleratorKeyEventType.KeyUp)
            {
                if (ApplicationView.GetForCurrentView().IsFullScreenMode)
                {
                    ApplicationView.GetForCurrentView().ExitFullScreenMode();
                }
                else
                {
                    ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                }

                args.Handled = true;
            }
        }

    }

}
