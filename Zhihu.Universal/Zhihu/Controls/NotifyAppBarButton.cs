using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Zhihu.Controls
{
    public sealed class NotifyAppBarButton : AppBarButton
    {
        public NotifyAppBarButton()
        {
            this.DefaultStyleKey = typeof(NotifyAppBarButton);
        }

        public static readonly DependencyProperty NotifyMarkVisiableProperty = DependencyProperty.Register("NotifyMarkVisiable", typeof(Visibility), typeof(NotifyAppBarButton), new PropertyMetadata(Visibility.Collapsed));

        public Visibility NotifyMarkVisiable
        {
            get { return (Visibility)GetValue(NotifyMarkVisiableProperty); }
            set { SetValue(NotifyMarkVisiableProperty, value); }
        }
    }
}
