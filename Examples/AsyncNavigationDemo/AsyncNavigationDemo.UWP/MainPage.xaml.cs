using Prism;
using Prism.Ioc;

namespace AsyncNavigationDemo.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            var xamarinFormsApp = new AsyncNavigationDemo.App(new UwpInitializer());
            xamarinFormsApp.StartAsync().Wait();
            LoadApplication(xamarinFormsApp);
        }
    }

    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
