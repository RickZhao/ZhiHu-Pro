using System;
using System.ComponentModel;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;
using Zhihu.Converter;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class CommentView : UserControl, INotifyPropertyChanged
    {
        private Comment _item;

        public Comment Item
        {
            get { return _item; }
            private set
            {
                _item = value;

                var handler = PropertyChanged;
                if (handler != null) handler.Invoke(this, new PropertyChangedEventArgs("Item"));
            }
        }

        private RelativeTimeConverter _converter = new RelativeTimeConverter();
        private RelayCommand<Comment> _authorTapped;

        public event PropertyChangedEventHandler PropertyChanged;

        public CommentView()
        {
            this.InitializeComponent();

            this.Avatar.Tapped += Author_Tapped;
            this.Author.Tapped += Author_Tapped;
            this.VoteupContainer.Tapped += Author_Tapped;

            this.Summary.Tapped += Answer_Tapped;
        }

        private void Author_Tapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            tappedRoutedEventArgs.Handled = true;

            if (_authorTapped != null)
                _authorTapped.Execute(Item);
        }

        private void Answer_Tapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
        }

        internal void ShowPlaceHolder(Comment item)
        {
            this.Item = item;
            this.DataContext = this;
        }

        internal void RegistEventHandler(RelayCommand<Comment> authorTapped)
        {
            this._authorTapped = authorTapped;
        }

        internal void ShowAvatar()
        {
            this.Avatar.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(Item.Author.AvatarUrl))),
                Stretch = Stretch.Fill,
            };
        }

        internal void ShowAuthor()
        {
            this.Author.Text = Item.Author.Name;
        }

        internal void ShowUpdatedTime()
        {
            this.CreatedTime.Text = _converter.Convert(Item.CreatedTime, null, null, null).ToString();
        }

        internal void ShowVoteup()
        {
            if (String.IsNullOrEmpty(this.VoteupCount.Text) == false) return;

            this.VoteupCount.Text = Utility.Instance.GetEasyInt32(Item.VoteCount);
        }

        internal void ShowSummary()
        {
            this.Summary.Text = Item.Content;
        }

        internal void Clear()
        {
            Item = null;

            this.Avatar.ClearValue(Ellipse.FillProperty);
            this.Author.ClearValue(TextBlock.TextProperty);
            this.CreatedTime.ClearValue(TextBlock.TextProperty);
            this.VoteupCount.ClearValue(TextBlock.TextProperty);
            this.Summary.ClearValue(TextBlock.TextProperty);

            this._authorTapped = null;
        }
    }
}
