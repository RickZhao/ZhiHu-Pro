using System;

using Windows.UI.Xaml.Navigation;


namespace Zhihu.Common.Service
{
    public interface INavigate
    {
        event NavigatingCancelEventHandler Navigating;
        void NavigateTo(Type sourcePageType);
        void GoBack(); 
    }
}
