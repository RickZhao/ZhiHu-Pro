
using Windows.UI.Xaml.Controls;

using Zhihu.Common.Model.Html;
using Zhihu.Helper;


namespace Zhihu.Controls.ItemView
{
    public sealed partial class ParagraphView : UserControl
    {
        private ParagraphModel _paragraph;

        public ParagraphView()
        {
            this.InitializeComponent();
        }

        internal void ShowPlaceholder(ParagraphModel para)
        {
            _paragraph = para;
        }

        internal void Show()
        {
            HtmlHelper.Instance.UpdateRichTextBox(_paragraph, this.TextBlock);
        }

        public void Clear()
        {
            this.TextBlock.Blocks.Clear();
        }
    }
}
