using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Prism.Xamarin.AsyncNavigation;
using Xamarin.Forms;

namespace AsyncNavigationDemo.ViewModels
{
    public class FirstViewModel : ViewModelBase, INavigatingAsyncAware
    {
        private string _result = "Loading";
        private bool _isBusy;

        private static bool ShouldLoadingFail { get; set; } = true;

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

        public ICommand NavigateToSecondPageCommand { get; }

        public FirstViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "First Page";

            NavigateToSecondPageCommand = new Command(async () => await NavigateToSecondPageAsync());
        }

        public Task OnNavigatingToAsync(INavigationParameters parameters)
        {
            return InitializeAsync(parameters);
        }

        private async Task InitializeAsync(INavigationParameters parameters)
        {
            Debug.WriteLine($"{nameof(FirstViewModel)} -> {nameof(OnNavigatingTo)} is invoked.");

            await Task.Delay(2000); // simulate a network activity (data loading)

            ShouldLoadingFail = !ShouldLoadingFail;
            if (ShouldLoadingFail)
                throw new Exception("Simulated Navigation Exception");

            Result = "Success";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Debug.WriteLine($"{nameof(FirstViewModel)} -> {nameof(OnNavigatedTo)} is invoked.");
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            Debug.WriteLine($"{nameof(FirstViewModel)} -> {nameof(OnNavigatedFrom)} is invoked.");

            base.OnNavigatedFrom(parameters);
        }

        private async Task NavigateToSecondPageAsync()
        {
            INavigationResult navigationResult;
            try
            {
                IsBusy = true;
                navigationResult = await NavigationService.NavigateAsync("SecondPage");
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
