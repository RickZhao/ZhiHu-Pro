using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using Zhihu.Common.Helper;
using Zhihu.Common.HtmlAgilityPack;
using Zhihu.Common.Model.Html;



namespace Zhihu.Helper
{
    public sealed class RichTextBlockBuilder
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached(
            "Content", typeof (String), typeof (RichTextBlockBuilder),
            new PropertyMetadata(default(String), OnContentChangedCallback));

        public static void SetContent(DependencyObject element, String value)
        {
            element.SetValue(ContentProperty, value);
        }

        public static String GetContent(DependencyObject element)
        {
            return (String) element.GetValue(ContentProperty);
        }

        private static void OnContentChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var richTextBlock = dependencyObject as RichTextBlock;
            var content = dependencyPropertyChangedEventArgs.NewValue as String;

            if ((null == richTextBlock || String.IsNullOrEmpty(content))) return;

            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(content);

            //Debug.WriteLine("HAP完成解析：{0}", DateTime.Now);

            //Debug.WriteLine("RichText开始构建：{0}", DateTime.Now);

            var paras = HtmlHelper.Instance.GetParagraphs(htmlDoc.DocumentNode.ChildNodes);

            if (paras == null) return;

            richTextBlock.Blocks.Clear();

            foreach (var para in paras)
            {
                var paragraph = new Paragraph();

                foreach (var run in para.Runs)
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
                                span.Inlines.Add(new Run() { Text = Decode(textRun.Text) });
                                break;
                            case TextType.H1:
                            case TextType.H2:
                            case TextType.H3:
                            case TextType.Bold:
                                var bold = new Bold();

                                bold.Inlines.Add(new Run() { Text = Decode(textRun.Text) });

                                span.Inlines.Add(bold);
                                break;
                            case TextType.Italic:
                                var italic = new Italic();

                                italic.Inlines.Add(new Run() { Text = Decode(textRun.Text) });

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

                        Image img = null;

#if DEBUG
                        if (Theme.Instance.NoImage)
#else
                        if (Theme.Instance.NoImage && Utility.Instance.IsUsingWifi == false)
#endif
                        {
                            img = new Image()
                            {
                                Margin = new Thickness(0, 12, 0, 12),
                                Source = new BitmapImage(new Uri("ms-appx:///Resource/Images/Public/play-icon.png")),
                                Stretch = Stretch.Fill,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                            };
                        }
                        else
                        {
                            img = new Image()
                            {
                                Margin = new Thickness(0, 12, 0, 12),
                                Source = new BitmapImage(new Uri(imageRun.Image)),
                                Stretch = imageRun.Width > 400 ? Stretch.Fill : Stretch.None,
                                HorizontalAlignment = HorizontalAlignment.Left,
                                VerticalAlignment = VerticalAlignment.Center,
                            };
                        }

                        paragraph.Inlines.Add(new InlineUIContainer()
                        {
                            Child = img,
                        });

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

                            bold.Inlines.Add(new Run() { Text = Decode(textHrefRun.Text) });

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
                                Margin = new Thickness(0, 0, 0, 0),
                                Padding = new Thickness(0, 0, 0, 0),
                                FontSize = 16,
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
