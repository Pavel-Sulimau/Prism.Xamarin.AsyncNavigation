using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;

namespace Prism.Xamarin.AsyncNavigation.Common
{
    internal static class PageUtilitiesExtended
    {
        public static async Task InvokeViewAndViewModelActionAsync<T>(object view, Func<T, Task> asyncAction) where T : class
        {
            if (view is T viewAsT)
                await asyncAction(viewAsT);

            if (view is BindableObject element)
            {
                if (element.BindingContext is T viewModelAsT)
                {
                    await asyncAction(viewModelAsT);
                }
            }

            if (view is Page page)
            {
                var partials = (List<BindableObject>)page.GetValue(Prism.Xamarin.AsyncNavigation.Mvvm.ViewModelLocator.PartialViewsProperty)
                    ?? new List<BindableObject>();
                foreach (var partial in partials)
                {
                    await InvokeViewAndViewModelActionAsync(partial, asyncAction);
                }
            }
        }

        internal static Task OnNavigatingToAsync(object page, INavigationParameters parameters)
        {
            if (page != null)
                return InvokeViewAndViewModelActionAsync<INavigatingAsyncAware>(page, async v => await v.OnNavigatingToAsync(parameters));

            return Task.FromResult(0);
        }

        internal static bool HasDirectNavigationPageParent(Page page)
        {
            return page?.Parent != null && page?.Parent is NavigationPage;
        }

        internal static bool HasNavigationPageParent(Page page)
        {
            if (page?.Parent != null)
            {
                if (page.Parent is NavigationPage)
                {
                    return true;
                }
                else if (page.Parent is TabbedPage || page.Parent is CarouselPage)
                {
                    return page.Parent.Parent != null && page.Parent.Parent is NavigationPage;
                }
            }

            return false;
        }

        internal static bool IsSameOrSubclassOf<T>(Type potentialDescendant)
        {
            if (potentialDescendant == null)
                return false;

            Type potentialBase = typeof(T);

            return potentialDescendant.GetTypeInfo().IsSubclassOf(potentialBase)
                || potentialDescendant == potentialBase;
        }

        internal static void SetAutowireViewModelOnPage(Page page)
        {
            var vmlResult = Prism.Mvvm.ViewModelLocator.GetAutowireViewModel(page);
            if (vmlResult == null)
                Prism.Mvvm.ViewModelLocator.SetAutowireViewModel(page, true);
        }
    }
}
