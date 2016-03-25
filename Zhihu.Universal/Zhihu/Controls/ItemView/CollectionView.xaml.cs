using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;
using Zhihu.Common.Helper;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class CollectionView : UserControl
    {
        private Collection _item;
        private RelayCommand<Collection> _collectionTapped;

        public CollectionView()
        {
            this.InitializeComponent();

            this.Tapped += CollectionView_Tapped;
        }

        private void CollectionView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_collectionTapped != null && _collectionTapped.CanExecute(_item))
            {
                _collectionTapped.Execute(_item);
            }
        }

        internal void ShowPlaceHolder(Collection item)
        {
            _item = item;

            this.Title.Opacity = 0;
            this.Summary.Opacity = 0;
            this.Avator.Opacity = 0;
            this.Author.Opacity = 0;
            this.QueAnswerCount.Opacity = 0;
        }

        internal void ShowTitle()
        {
            this.Title.Text = _item.Title;
            this.Title.Opacity = 1;
        }

        internal void ShowDescription()
        {
            this.Summary.Text = String.IsNullOrEmpty(_item.Description) ? String.Empty : _item.Description;
            this.Summary.Opacity = 1;
            this.Summary.Visibility = String.IsNullOrEmpty(_item.Description) ? Visibility.Collapsed : Visibility;
        }

        internal void ShowAvator()
        {
            this.Avator.Fill = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(AvartarHelper.GetLarge(_item.Creator.AvatarUrl))),
                Stretch = Stretch.Fill,
            };
            this.Avator.Opacity = 1;
        }

        internal void ShowAuthor()
        {
            this.Author.Text = _item.Creator.Name;
            this.Author.Opacity = 1;
        }

        internal void ShowCount()
        {
            this.QueAnswerCount.Text = _item.AnswerCount.ToString();
            this.QueAnswerCount.Opacity = 1;
        }

        internal void RegistEventHandler(RelayCommand<Collection> collectionTapped)
        {
            this._collectionTapped = collectionTapped;
        }

        internal void Clear()
        {
            _item = null;
            this.Title.ClearValue(TextBlock.TextProperty);
            this.Summary.ClearValue(TextBlock.TextProperty);
            this.Avator.ClearValue(Ellipse.FillProperty);
            this.Author.ClearValue(TextBlock.TextProperty);
            this.QueAnswerCount.ClearValue(TextBlock.TextProperty);
        }
    }
}
