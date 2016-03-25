
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using GalaSoft.MvvmLight.Command;

using Zhihu.Common.Model;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class TableQuestionView : UserControl
    {
        private TableQuestion _item;
        private RelayCommand<TableQuestion> _titleTapped;

        public TableQuestionView()
        {
            this.InitializeComponent();

            this.Tapped += Title_Tapped;  
        }

        private void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _titleTapped?.Execute(_item);
        }
        

        internal void ShowPlaceHolder(TableQuestion item)
        {
            _item = item;

            Title.Opacity = 0;
            Followers.Opacity = 0;
            this.Answers.Opacity = 0;
        }

        internal void RegistEventHandler(RelayCommand<TableQuestion> titleTapped)
        {
            this._titleTapped = titleTapped;
        }

        internal void ShowTitle()
        {
            this.Title.Text = _item.Title;
            this.Title.Opacity = 1;
        }

        internal void ShowSummary()
        {
            Followers.Text = _item.FollowerCount.ToString();
            Followers.Opacity = 1;
            Answers.Text = _item.AnswerCount.ToString();
            Answers.Opacity = 1;
        }
        
        public void Clear()
        {
            this._item = null;
            this._titleTapped = null;

            this.Title.ClearValue(TextBlock.TextProperty);
            this.Followers.ClearValue(TextBlock.TextProperty);
            this.Answers.ClearValue(TextBlock.TextProperty);
        }
    }
}
