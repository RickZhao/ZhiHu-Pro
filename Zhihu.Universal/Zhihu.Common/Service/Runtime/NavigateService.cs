using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Zhihu.Common.Service.Runtime
{
    public sealed class NavigateService : INavigate
    {
        private Frame _frame;

        public event NavigatingCancelEventHandler Navigating;

        public void NavigateTo(Type sourcePageType)
        {
            if (EnsureMainFrame())
            {
                _frame.Navigate(sourcePageType);
            }
        }

        public void GoBack()
        {
            if (EnsureMainFrame() && _frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }

        private bool EnsureMainFrame()
        {
            if (_frame != null)
            {
                return true;
            }

            _frame = Window.Current.Content as Frame;
            if (_frame != null)
            {
                _frame.Navigating += (s, e) =>
                {
                    if (Navigating != null)
                    {
                        Navigating(s, e);
                    }
                };
                return true;
            }
            return false;
        }
    }
}
