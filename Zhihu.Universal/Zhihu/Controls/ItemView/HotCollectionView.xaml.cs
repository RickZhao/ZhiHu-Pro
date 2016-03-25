using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Helper;
using Zhihu.Common.Model;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class HotCollectionView : UserControl
    {
        private HotCollection _item;
        private RelayCommand<HotCollection> _collectionTapped;

        public HotCollectionView()
        {
            this.InitializeComponent();

            this.Tapped += HotCollectionView_Tapped;
        }

        void HotCollectionView_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (_collectionTapped != null && _collectionTapped.CanExecute(_item))
            {
                _collectionTapped.Execute(_item);
            }
        }

        internal void ShowPlaceHolder(HotCollection item)
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
            this.Summary.Text = _item.Description;
            this.Summary.Opacity = 1;
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

        internal void RegistEventHandler(RelayCommand<HotCollection> collectionTapped)
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
