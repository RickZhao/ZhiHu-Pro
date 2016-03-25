using System;
using System.Threading.Tasks;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;


namespace Zhihu.Controls
{
    public sealed partial class ToasteBar : UserControl
    {
        public ToasteBar()
        {
            this.InitializeComponent();
        }
    }

    internal sealed class ToasteBarInfo
    {
        public Uri Image { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public Object Tag { get; set; }
    }

    internal sealed class ToasteIndicator
    {
        #region Singleton

        private static ToasteIndicator _instance;

        public static ToasteIndicator Instance
        {
            get
            {
                if (_instance == null) _instance = new ToasteIndicator();

                return _instance;
            }
        }

        #endregion

        private readonly Popup _popup;

        private readonly ToasteBar _toasteBar;

        private ToasteIndicator()
        {
            _toasteBar = new ToasteBar();
            _popup = new Popup { Child = _toasteBar };
        }

        public void Show(String title, String content, Object tag, Int32 iSeconds)
        {
            Show(null, title, content, tag, iSeconds);
        }

        public void Show(Uri image, String title, String content, Object tag, Int32 iSeconds)
        {
            _toasteBar.DataContext = new ToasteBarInfo()
            {
                Image = image,
                Title = title,
                Content = content,
                Tag = tag,
            };

            var task = new Task(async () =>
            {
                try
                {
                    var dispatcher = Window.Current.Dispatcher;

                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        _popup.IsOpen = true;
                    });

                    await Task.Delay(iSeconds*1000);

                    await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        _popup.IsOpen = false;
                    });
                }
                catch (Exception exc)
                {
                }
            });

            task.Start(TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}