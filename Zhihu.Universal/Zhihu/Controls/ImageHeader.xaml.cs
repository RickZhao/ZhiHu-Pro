using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;


namespace Zhihu.Controls
{
    public sealed partial class ImageHeader : UserControl
    {
        public static readonly DependencyProperty NormalProperty = DependencyProperty.Register(
            "Normal", typeof (BitmapImage), typeof (ImageHeader), new PropertyMetadata(default(String)));

        public BitmapImage Normal
        {
            get { return (BitmapImage) GetValue(NormalProperty); }
            set { SetValue(NormalProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
            "Selected", typeof (BitmapImage), typeof (ImageHeader), new PropertyMetadata(default(String)));

        public BitmapImage Selected
        {
            get { return (BitmapImage) GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty IsCurrentProperty = DependencyProperty.Register(
            "IsCurrent", typeof (Boolean), typeof (ImageHeader), new PropertyMetadata(default(Boolean), HeaderChangedCallback));

        private static void HeaderChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var imgHeader = sender as ImageHeader;

            var isCurrent = args.NewValue != null && Boolean.TrueString == args.NewValue.ToString();

            imgHeader?.UpdateCurrent(isCurrent);
        }

        private void UpdateCurrent(Boolean isCurrent)
        {
            this.Logo.Source = isCurrent ? Selected : Normal;
        }

        public Boolean IsCurrent
        {
            get { return (Boolean) GetValue(IsCurrentProperty); }
            set { SetValue(IsCurrentProperty, value); }
        }

        public ImageHeader()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }
    }
}
