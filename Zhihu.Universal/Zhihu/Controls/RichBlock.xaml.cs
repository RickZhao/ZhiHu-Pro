using System;
using System.Collections.Generic;
using System.ComponentModel;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Zhihu.Common.DataSource;
using Zhihu.Common.Model.Html;
using Zhihu.Controls.ItemView;


namespace Zhihu.Controls
{
    public sealed partial class RichBlock : UserControl, INotifyPropertyChanged
    {
        private LazyLoading<ParagraphModel> _paras;

        public LazyLoading<ParagraphModel> Paras
        {
            get { return _paras; }
            set
            {
                _paras = value;
                RaisePropertyChanged("Paras");
            }
        }

        public RichBlock()
        {
            this.InitializeComponent();

            var binding = new Binding()
            {
                Path = new PropertyPath("Paras"),
                Source = this,
                Mode = BindingMode.OneWay,
            };

            this.ContentView.SetBinding(ItemsControl.ItemsSourceProperty, binding);
        }

        #region Dependency Properties

        #region Footer

        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }
        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(RichBlock), new PropertyMetadata(null, FooterTemplateChangedCallback));

        private static void FooterTemplateChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var richBlock = sender as RichBlock;

            if (richBlock == null) return;

            var headerTemplate = args.NewValue as DataTemplate;

            richBlock.UpdateFooterTemplate(headerTemplate);
        }

        private void UpdateFooterTemplate(DataTemplate footerTemplate)
        {
            ContentView.FooterTemplate = footerTemplate;
        }

        #endregion

        #region Header Template

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(RichBlock), new PropertyMetadata(null, HeaderTemplateChangedCallback));

        private static void HeaderTemplateChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var richBlock = sender as RichBlock;

            if (richBlock == null) return;

            var headerTemplate = args.NewValue as DataTemplate;

            richBlock.UpdateHeaderTemplate(headerTemplate);
        }

        private void UpdateHeaderTemplate(DataTemplate headerTemplate)
        {
            ContentView.HeaderTemplate = headerTemplate;
        }

        #endregion

        #region ParagraphModels

        public List<ParagraphModel> Paragraphs
        {
            get { return (List<ParagraphModel>)GetValue(ParagraphsProperty); }
            set { SetValue(ParagraphsProperty, value); }
        }

        public static readonly DependencyProperty ParagraphsProperty = DependencyProperty.Register(
            "Paragraphs", typeof(List<ParagraphModel>), typeof(RichBlock), new PropertyMetadata(default(List<ParagraphModel>), ParagraphsChangedCallback));

        private static void ParagraphsChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var richBlock = sender as RichBlock;

            if (richBlock == null) return;

            var paras = args.NewValue as List<ParagraphModel>;

            if (paras == null) return;

            richBlock.UpdateParas(paras);
        }

        private void UpdateParas(List<ParagraphModel> paraModels)
        {
            if (paraModels == null)
            {
                this.Paras.Clear();
            }
            else
            {
                this.Paras = new LazyLoading<ParagraphModel>(paraModels);
            }
        }

        #endregion
        
        #endregion

        #region Content Changing

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> CotentChanging
        {
            get
            {
                return _cotentChanging ??
                       (_cotentChanging =
                           new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(
                               AnswerContent_OnContainerContentChanging));
            }
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _cotentChanging;

        private void AnswerContent_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var view = args.ItemContainer.ContentTemplateRoot as ParagraphView;

            if (view == null) return;

            if (args.InRecycleQueue == true)
            {
                view.Clear();
            }
            else if (args.Phase == 0)
            {
                view.ShowPlaceholder(args.Item as ParagraphModel);

                args.RegisterUpdateCallback(CotentChanging);
            }
            else if (args.Phase == 1)
            {
                view.Show();
            }
            args.Handled = true;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(String propertyName)
        {
            if (PropertyChanged == null) return;

            var handler = PropertyChanged;
            handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}