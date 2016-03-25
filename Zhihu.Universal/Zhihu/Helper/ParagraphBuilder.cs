using System;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using UmengSDK;

using Zhihu.Controls;
using Zhihu.Common.Model.Html;
using Zhihu.Common.Helper;


namespace Zhihu.Helper
{
    public sealed class ParagraphBuilder
    {
        #region ImageViewer DependencyProperty

        private static ImageViewer _viewer;

        public static readonly DependencyProperty ViewerProperty = DependencyProperty.RegisterAttached(
            "Viewer", typeof(ImageViewer), typeof(ParagraphBuilder), new PropertyMetadata(default(ImageViewer), ViewerChangedCallback));

        public static void SetViewer(DependencyObject element, ImageViewer value)
        {
            element.SetValue(ViewerProperty, value);
        }

        public static ImageViewer GetViewer(DependencyObject element)
        {
            return (ImageViewer)element.GetValue(ViewerProperty);
        }

        private static void ViewerChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var richTextBlock = dependencyObject as RichTextBlock;
            var imageViewer = dependencyPropertyChangedEventArgs.NewValue as ImageViewer;

            if (null == richTextBlock || null == imageViewer) return;

            _viewer = imageViewer;
        }

        #endregion

        #region Content DependencyProperty

        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
            "Content", typeof (ParagraphModel), typeof (ParagraphBuilder),
            new PropertyMetadata(default(ParagraphModel), ParagraphChangedCallback));

        public static void SetContent(DependencyObject element, ParagraphModel value)
        {
            element.SetValue(ContentProperty, value);
        }

        public static ParagraphModel GetContent(DependencyObject element)
        {
            return (ParagraphModel) element.GetValue(ContentProperty);
        }

        private static void ParagraphChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var richTextBlock = sender as RichTextBlock;
            var paragraphModel = args.NewValue as ParagraphModel;

            if ((null == richTextBlock || null == paragraphModel || paragraphModel.Runs == null || paragraphModel.Runs.Count == 0)
                || (paragraphModel.Runs.Count == 1 && paragraphModel.Runs[0] is LineBreakRun)) return;

            richTextBlock.Blocks.Clear();

            UpdateRichTextBox(paragraphModel, richTextBlock);
        }

        #endregion
        
        private static async void UpdateRichTextBox(ParagraphModel paragraphModel, RichTextBlock richTextBlock)
        {
            try
            {
                if (paragraphModel == null || (paragraphModel.Runs.Count == 1 && paragraphModel.Runs.FirstOrDefault() is LineBreakRun))
                    return;

                var paragraph = new Paragraph();
                foreach (var run in paragraphModel.Runs)
                {
                    #region Paragraph Build

                    if (run is LineBreakRun)
                    {
                        #region LineBreak

                        var span = new Span();
                        span.Inlines.Add(new LineBreak());

                        paragraph.Inlines.Add(span);

                        #endregion
                    }
                    else if (run is TextRun)
                    {
                        #region TextRun

                        var textRun = run as TextRun;
                        var span = new Span();

                        switch (textRun.Type)
                        {
                            case TextType.Plain:
                                span.Inlines.Add(new Run() {Text = Decode(textRun.Text)});
                                break;
                            case TextType.H1:
                            case TextType.H2:
                            case TextType.H3:
                            case TextType.Bold:
                                var bold = new Bold();

                                bold.Inlines.Add(new Run() {Text = Decode(textRun.Text)});

                                span.Inlines.Add(bold);
                                break;
                            case TextType.Italic:
                                var italic = new Italic();

                                italic.Inlines.Add(new Run() {Text = Decode(textRun.Text)});

                                span.Inlines.Add(italic);
                                break;
                        }

                        paragraph.Inlines.Add(span);

                        #endregion
                    }
                    else if (run is ImageRun)
                    {
                        #region ImageRun

                        var imageRun = run as ImageRun;

                        if (imageRun.Image.Contains("equation"))
                        {
                            #region 公式

                            var img = new Image()
                            {
                                Margin = new Thickness(6, 6, 6, 6),
                                Source = new BitmapImage(new Uri(imageRun.Image)),
                                Stretch = Stretch.None,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                Tag = imageRun,
                            };

                            paragraph.Inlines.Add(new InlineUIContainer()
                            {
                                Child = img,
                            });

                            #endregion
                        }
                        else
                        {
                            #region 普通图片

                            paragraph.Inlines.Add(new InlineUIContainer()
                            {
                                Child = new PlaceholderImage()
                                {
                                    ImageContent = imageRun,
                                },
                            });

                            #endregion
                        }

                        #endregion
                    }
                    else if (run is ImageHrefRun)
                    {
                        #region ImageHrefRun

                        var imgHrefRun = run as ImageHrefRun;

                        var hyperLink = new HyperlinkButton()
                        {
                            Tag = new Uri(imgHrefRun.Href),
                            Content = new Image()
                            {
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                                Source = new BitmapImage(new Uri(imgHrefRun.Image)),
                                Stretch = imgHrefRun.Width > 400 ? Stretch.Fill : Stretch.UniformToFill,
                            }
                        };
                        
                        hyperLink.Click -= HyperLink_Click;
                        hyperLink.Click += HyperLink_Click;

                        paragraph.Inlines.Add(new InlineUIContainer()
                        {
                            Child = hyperLink,
                        });

                        #endregion
                    }
                    else if (run is TextHrefRun)
                    {
                        #region TextHrefRun

                        var textHrefRun = run as TextHrefRun;

                        if (textHrefRun.IsRelative)
                        {
                            var bold = new Bold();

                            bold.Inlines.Add(new Run() {Text = Decode(textHrefRun.Text)});

                            paragraph.Inlines.Add(bold);

                            continue;
                        }

                        var hyperLink = new HyperlinkButton()
                        {
                            Foreground = new SolidColorBrush(Theme.Instance.LinkForeColor),
                            Content = new TextBlock()
                            {
                                Text = Decode(textHrefRun.Text),
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(0, 0, 0, -10),
                                Padding = new Thickness(0, 0, 0, 0),
                                FontSize = Theme.Instance.FeedSummary,
                            },
                            Tag = textHrefRun.Href,
                        };

                        hyperLink.Click -= HyperLink_Click;
                        hyperLink.Click += HyperLink_Click;

                        paragraph.Inlines.Add(new InlineUIContainer()
                        {
                            Child = hyperLink
                        });

                        #endregion
                    }
                    else if (run is VideoRun)
                    {
                        #region VideoRun

                        var videoRun = run as VideoRun;
                        var videoThumb = new Grid();

                        videoThumb.Children.Add(new Image()
                        {
                            Source = new BitmapImage(new Uri(videoRun.Thumb)),
                            Stretch = Stretch.UniformToFill,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(0, 12, 0, 12),
                        });
                        videoThumb.Children.Add(new Image()
                        {
                            Source = new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/play-icon.png")),
                            Width = 88,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        });

                        var hyperLink = new HyperlinkButton()
                        {
                            Tag = new Uri(videoRun.HostPage),
                            Content = videoThumb
                        };

                        hyperLink.Click -= HyperLink_Click;
                        hyperLink.Click += HyperLink_Click;

                        var uiContainer = new InlineUIContainer()
                        {
                            Child = hyperLink
                        };

                        paragraph.Inlines.Add(uiContainer);

                        #endregion
                    }

                    #endregion
                }
                richTextBlock.Blocks.Add(paragraph);
            }
            catch (Exception ex)
            {
                await UmengAnalytics.TrackException(ex, ex.Message);
            }
        }

        private static void HyperLink_Click(object sender, RoutedEventArgs e)
        {
            var hyperLinkButton = sender as HyperlinkButton;

            if (hyperLinkButton == null) return;

            var hyperLinkUrl = hyperLinkButton.Tag.ToString();

            Utility.Instance.RaiseEvent(hyperLinkUrl);
        }
        
        private static String Decode(String formal)
        {
            var text = formal.Replace("&lt;", "<");
            
            text = text.Replace("&gt;", ">");
            text = text.Replace("&amp;", "&");
            text = text.Replace("&quot;", "\"");

            return text;
        }
    }
}