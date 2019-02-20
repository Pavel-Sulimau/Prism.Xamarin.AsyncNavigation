using Prism.Navigation;

namespace Prism.Xamarin.AsyncNavigation
{
    internal static class NavigationParametersExtensions
    {
        internal static INavigationParametersInternal GetNavigationParametersInternal(this INavigationParameters parameters)
        {
            return (INavigationParametersInternal)parameters;
        }
    }
}
