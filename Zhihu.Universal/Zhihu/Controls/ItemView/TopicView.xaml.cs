using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Helper;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class TopicView : UserControl
    {
        private Topic _item;
        private RelayCommand<Topic> _topicTapped;

        public TopicView()
        {
            this.InitializeComponent();

            this.Tapped += TopicView_Tapped;
        }

        private void TopicView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            _topicTapped?.Execute(_item);
        }

        internal void ShowPlaceHolder(Topic item)
        {
            _item = item;

            this.Title.Opacity = 0;
            this.Summary.Opacity = 0;
            this.Avator.Opacity = 0;
        }

        internal void ShowTitle()
        {
            this.Title.Text = _item.Name;
            this.Title.Opacity = 1;
        }

        internal void ShowDescription()
        {
            this.Summary.Text = _item.Excerpt;
            this.Summary.Opacity = 1;
        }

        internal void ShowAvator()
        {
            this.Avator.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.AvatarUrl))),
                Stretch = Stretch.Fill,
            };
            this.Avator.Opacity = 1;
        }

        internal void RegistEventHandler(RelayCommand<Topic> topicTapped)
        {
            this._topicTapped = topicTapped;
        }

        internal void Clear()
        {
            _item = null;

            this.Title.ClearValue(TextBlock.TextProperty);
            this.Summary.ClearValue(TextBlock.TextProperty);
            this.Avator.ClearValue(Ellipse.FillProperty);
        }
    }
}
