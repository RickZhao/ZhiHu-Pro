using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;



namespace Zhihu.Controls
{
    public sealed class FlipViewIndicator : ListBox
    {
        private TextBlock _numberIndicator;
        private ScrollViewer _scrollViewer;

        public FlipViewIndicator()
        {
            DefaultStyleKey = typeof(FlipViewIndicator);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _numberIndicator = GetTemplateChild("Indicator") as TextBlock;
            _scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;

            UpdateNumberMode(NumberMode);
        }

        public Boolean NumberMode
        {
            get { return (Boolean)GetValue(NumberModeProperty); }
            set
            {
                SetValue(NumberModeProperty, value);
            }
        }

        public static readonly DependencyProperty NumberModeProperty = DependencyProperty.Register("NumberMode", typeof(Boolean), typeof(FlipViewIndicator), new PropertyMetadata(false));
        
        private void UpdateNumberMode(Boolean numberModeOn)
        {
            if (_numberIndicator != null)
            {
                _numberIndicator.Text = String.IsNullOrEmpty(IndicatorText) ? String.Empty : IndicatorText;
            }
            if (_numberIndicator != null)
            {
                _numberIndicator.Visibility = numberModeOn ? Visibility.Visible : Visibility.Collapsed;
            }
            if (_scrollViewer != null)
            {
                _scrollViewer.Visibility = numberModeOn ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public String IndicatorText
        {
            get;
            set;
        }
        
        public FlipView FlipView
        {
            get { return GetValue(FlipViewProperty) as FlipView; }
            set
            {
                SetValue(FlipViewProperty, value);
            }
        }

        public static readonly DependencyProperty FlipViewProperty = DependencyProperty.Register("FlipView", typeof(FlipView), typeof(FlipViewIndicator), new PropertyMetadata(null, OnFlipViewProperty_Changed));

        private static void OnFlipViewProperty_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var indicator = sender as FlipViewIndicator;
            var flipView = args.NewValue as FlipView;

            indicator?.HookUpSelectionChanged();

            var srcBinding = new Binding()
            {
                Mode = BindingMode.TwoWay,
                Source = flipView,
                Path = new PropertyPath("ItemsSource")
            };

            indicator.SetBinding(FlipViewIndicator.ItemsSourceProperty, srcBinding);

            var binding = new Binding()
            {
                Mode = BindingMode.TwoWay,
                Source = flipView,
                Path = new PropertyPath("SelectedItem")
            };
            indicator.SetBinding(FlipViewIndicator.SelectedItemProperty, binding);
        }

        private void HookUpSelectionChanged()
        {
            this.SelectionChanged -= FlipViewIndicator_SelectionChanged;
            this.SelectionChanged += FlipViewIndicator_SelectionChanged;
        }

        private void FlipViewIndicator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemsSource = this.ItemsSource as IList;
            var selectedIndex = itemsSource?.IndexOf(SelectedItem) + 1;
            var total = itemsSource?.Count;

            IndicatorText = String.Format("{0}/{1}", selectedIndex, total);

            if (_numberIndicator != null)
            {
                _numberIndicator.Text = String.IsNullOrEmpty(IndicatorText) ? String.Empty : IndicatorText; ;
            }
        }
    }
}
