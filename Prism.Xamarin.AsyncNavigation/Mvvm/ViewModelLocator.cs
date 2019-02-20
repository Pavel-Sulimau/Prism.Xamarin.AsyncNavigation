using System.Collections.Generic;
using Xamarin.Forms;

namespace Prism.Xamarin.AsyncNavigation.Mvvm
{
    /// <summary>
    /// This class defines the attached property and related change handler that calls the <see cref="Prism.Mvvm.ViewModelLocationProvider"/>.
    /// </summary>
    internal static class ViewModelLocator
    {
        internal static readonly BindableProperty PartialViewsProperty =
            BindableProperty.CreateAttached("PrismPartialViews", typeof(List<BindableObject>), typeof(Prism.Mvvm.ViewModelLocator), null);
    }
}
