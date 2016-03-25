using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class ChatView : UserControl
    {
        private Chat _chat;
        private RelayCommand<Chat> _itemTapped;

        public ChatView()
        {
            this.InitializeComponent();
            this.Tapped += ChatView_Tapped;
        }

        internal void ShowPlaceholder(Chat notify)
        {
            _chat = notify;

            this.Participant.Opacity = 0;

            this.Avator.Opacity = 0;

            this.Summary.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<Chat> itemTapped)
        {
            this._itemTapped = itemTapped;
        }

        internal void ShowAvatar()
        {
            this.Avator.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_chat.Participant.AvatarUrl))),
                Stretch = Stretch.Fill,
            };
            this.Avator.Opacity = 1;
        }

        internal void ShowTitle()
        {
            this.Participant.Text = _chat.Participant.Name;
            this.Participant.Opacity = 1;
        }

        internal void ShowSummary()
        {
            this.Summary.Text = _chat.Snippet;
            this.Summary.Opacity = 1;
        }

        internal void Clear()
        {
            this._chat = null;

            this.Participant.ClearValue(TextBlock.TextProperty);
            this.Summary.ClearValue(TextBlock.TextProperty);
            this.Avator.ClearValue(Shape.FillProperty);

            this._itemTapped = null;
        }

        private void ChatView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_itemTapped != null)
            {
                _itemTapped.Execute(_chat);
            }
        }
    }
}
