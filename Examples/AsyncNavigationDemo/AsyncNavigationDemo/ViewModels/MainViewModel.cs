using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;

namespace AsyncNavigationDemo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _result;
        private bool _isBusy;

        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand NavigateToFirstPageCommand { get; }

        public MainViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            NavigateToFirstPageCommand = new Command(async () => await NavigateToFirstPageAsync());
        }

        private async Task NavigateToFirstPageAsync()
        {
            INavigationResult navigationResult;
            try
            {
                IsBusy = true;
                await Task.Delay(100);
                navigationResult = await NavigationService.NavigateAsync("FirstPage");
            }
            finally
            {
                IsBusy = false;
            }

            if (navigationResult?.Success ?? false)
            {
                Result = $"Success - {DateTime.Now.Ticks}";
            }
            else if (navigationResult?.Exception != null)
            {
                Result = $"Failed - With {navigationResult.Exception.Message} {DateTime.Now.Ticks}";
            }
            else
            {
                Result = "Unexpected navigation result.";
            }
        }
    }
}
