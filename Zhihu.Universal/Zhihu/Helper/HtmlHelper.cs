using System;
using System.Collections.Generic;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

using UmengSDK;

using Zhihu.Common.Helper;
using Zhihu.Common.HtmlAgilityPack;
using Zhihu.Common.Model.Html;
using Zhihu.Controls;


namespace Zhihu.Helper
{
    internal sealed class HtmlHelper
    {
        #region Singleton

        private static HtmlHelper _instance;

        private HtmlHelper()
        {
        }

        /// <summary>
        /// LazyLoad的单例模式
        /// </summary>
        public static HtmlHelper Instance
        {
            get { return _instance ?? (_instance = new HtmlHelper()); }
        }

        #endregion

        public void UpdateRichTextBox(ParagraphModel paragraphModel, RichTextBlock richTextBlock)
        {
            try
            {
                if (paragraphModel == null ||
                    (paragraphModel.Runs.Count == 1 && paragraphModel.Runs.FirstOrDefault() is LineBreakRun))
                    return;

                richTextBlock.Blocks.Clear();

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

                            paragraph.Inlines.Add(new LineBreak());

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
                                Margin = new Thickness(0, 0, 0, -12),
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
                UmengAnalytics.TrackException(ex, ex.Message);
            }
        }

        public IList<ParagraphModel> GetParagraphs(HtmlNodeCollection nodeCollection, Boolean topEndHolder = false)
        {
            var paragraphs = new List<ParagraphModel>();

            //var header = new ParagraphModel();
            //header.Runs.Add(new LineBreakRun());
            //paragraphs.Add(header);

            try
            {
                var paragraph = new ParagraphModel();

                foreach (var node in nodeCollection)
                {
                    if (node.Name == "#text")
                    {
                        #region #text

                        var run = new TextRun() { Text = node.InnerText, Type = TextType.Plain };

                        paragraph.Runs.Add(run);

                        #endregion
                    }
                    else if (node.Name == "div" &&
                             node.Attributes.Hashitems.ContainsKey("class") &&
                             node.Attributes["class"].Value == "video-box")
                    {
                        #region video-box-inner

                        var videoRun = GetVideobox(node);

                        if (videoRun != null)
                        {
                            paragraph = MergePhagraph(paragraphs, paragraph);

                            var para = new ParagraphModel();

                            para.Runs.Add(videoRun);

                            paragraphs.Add(para);
                        }

                        #endregion
                    }
                    else if (node.Name == "br")
                    {
                        #region br

                        paragraph.Runs.Add(new LineBreakRun());

                        #endregion
                    }
                    else if (node.Name == "b")
                    {
                        #region b

                        foreach (var child in node.ChildNodes)
                        {
                            foreach (var inline in GetInlines(child))
                            {
                                paragraph.Runs.Add(inline);
                            }
                        }

                        #endregion
                    }
                    else if (node.Name == "a")
                    {
                        #region a

                        foreach (var child in node.ChildNodes)
                        {
                            foreach (var inline in GetInlines(child))
                            {
                                paragraph.Runs.Add(inline);
                            }
                        }

                        #endregion
                    }
                    else if (node.Name == "h2" || node.Name == "p" || node.Name == "ol")
                    {
                        #region h2 p ol

                        paragraph = MergePhagraph(paragraphs, paragraph);

                        var para = new ParagraphModel();

                        foreach (var child in node.ChildNodes)
                        {
                            foreach (var inline in GetInlines(child))
                            {
                                para.Runs.Add(inline);
                            }
                        }

                        paragraphs.Add(para);

                        #endregion
                    }
                    else if (node.Name == "img")
                    {
                        #region img

                        var imageRun = new ImageRun()
                        {
                            Image = node.Attributes["src"].Value
                        };

                        if (true == node.Attributes.Hashitems.ContainsKey("data-rawwidth"))
                            imageRun.Width = Double.Parse(node.Attributes["data-rawwidth"].Value);

                        if (true == node.Attributes.Hashitems.ContainsKey("data-rawheight"))
                            imageRun.Height = Double.Parse(node.Attributes["data-rawheight"].Value);

                        if (imageRun.Width > 0)
                        {
                            paragraph = MergePhagraph(paragraphs, paragraph);

                            var para = new ParagraphModel();

                            para.Runs.Add(imageRun);

                            para.Runs.Add(new LineBreakRun());
                            paragraphs.Add(para);
                        }
                        else
                        {
                            paragraph.Runs.Add(imageRun);
                        }

                        #endregion
                    }
                    else if (node.ChildNodes.Count > 0)
                    {
                        #region Inner

                        paragraph = MergePhagraph(paragraphs, paragraph);

                        var para = new ParagraphModel();

                        foreach (var grandChildNode in node.ChildNodes)
                        {
                            var inlines = GetInlines(grandChildNode);

                            foreach (var inlineItem in inlines)
                            {
                                paragraph.Runs.Add(inlineItem);
                            }
                        }

                        paragraphs.Add(para);

                        #endregion
                    }
                }

                paragraphs.Add(paragraph);
            }
            catch (Exception ex)
            {
                UmengAnalytics.TrackException(ex, ex.Message);
            }
            
            return paragraphs;
        }

        public IList<ImageRun> GetAllImages(HtmlNodeCollection nodeCollection)
        {
            var paras = GetParagraphs(nodeCollection);

            if (paras == null || paras.Count == 0) return null;

            var imageRuns = new List<ImageRun>();

            foreach (var para in paras)
            {
                foreach (var run in para.Runs)
                {
                    if (run is ImageRun) imageRuns.Add(run as ImageRun);
                }
            }

            return imageRuns;
            //return paras.SelectMany(para => para.Runs).OfType<ImageRun>().Select(run => run as ImageRun).ToList();
        }

        #region Privates Methods
        
        private IEnumerable<RunBase> GetInlines(HtmlNode node)
        {
            try
            {
                var collection = new List<RunBase>();

                #region 遍历子节点

                if (node.Name == "#text" || node.Name == "span")
                {
                    #region #text

                    if (node.ParentNode.Name == "a")
                    {
                        var isRelative = false;
                        try
                        {
                            var uri = new Uri(node.ParentNode.Attributes["href"].Value, UriKind.Absolute);
                            isRelative = false;
                        }
                        catch (Exception)
                        {
                            isRelative = true;
                        }

                        collection.Add(new TextHrefRun()
                        {
                            Text = node.InnerText,
                            Href = node.ParentNode.Attributes["href"].Value,
                            IsRelative = isRelative,
                        });
                    }
                    else
                    {
                        switch (node.ParentNode.Name)
                        {
                            case "b":
                            case "u":
                            case "strong":
                                collection.Add(new TextRun() { Text = node.InnerText, Type = TextType.Bold });
                                break;
                            case "h2":
                                collection.Add(new TextRun() { Text = node.InnerText, Type = TextType.H2 });
                                break;
                            case "p":
                            case "ol":
                            default:
                                collection.Add(new TextRun() { Text = node.InnerText, Type = TextType.Plain });
                                break;
                        }
                    }

                    #endregion
                }
                else if (node.Name == "br")
                {
                    #region br

                    collection.Add(new LineBreakRun());

                    #endregion
                }
                else if (node.Name == "a")
                {
                    #region a

                    if (node.ChildNodes.Count > 0)
                    {
                        foreach (var child in node.ChildNodes)
                        {
                            foreach (var inline in GetInlines(child))
                            {
                                collection.Add(inline);
                            }
                        }
                    }

                    #endregion
                }
                else if (node.Name == "b" || node.Name == "u" || node.Name == "strong")
                {
                    #region b

                    foreach (var grandChild in node.ChildNodes)
                    {
                        collection.AddRange(GetInlines(grandChild));
                    }

                    #endregion
                }
                else if (node.Name == "ul")
                {
                    #region ul li

                    foreach (var grandChild in node.ChildNodes)
                    {
                        collection.AddRange(GetInlines(grandChild));
                    }
                    collection.Add(new LineBreakRun());

                    #endregion
                }
                else if (node.Name == "li")
                {
                    #region ul li

                    collection.Add(new TextRun()
                    {
                        Text = " • "
                    });

                    foreach (var grandChild in node.ChildNodes)
                    {
                        collection.AddRange(GetInlines(grandChild));
                    }

                    if (false == collection.LastOrDefault() is LineBreakRun)
                    {
                        collection.Add(new LineBreakRun());
                    }

                    #endregion
                }
                else if (node.Name == "img")
                {
                    #region img

                    if (node.ParentNode.Name == "a")
                    {
                        var imageHrefRun = new ImageHrefRun()
                        {
                            Image = node.InnerText,
                            Href = node.ParentNode.Attributes["href"].Value
                        };

                        if (true == node.Attributes.Hashitems.ContainsKey("width"))
                            imageHrefRun.Width = Double.Parse(node.Attributes["width"].Value);

                        collection.Add(imageHrefRun);

                        if (imageHrefRun.Width > 0)
                        {
                            collection.Add(new LineBreakRun());
                        }
                    }
                    else
                    {
                        var imageRun = new ImageRun()
                        {
                            Image = node.Attributes["src"].Value,
                        };

                        if (true == node.Attributes.Hashitems.ContainsKey("data-rawwidth"))
                            imageRun.Width = Double.Parse(node.Attributes["data-rawwidth"].Value);

                        if (true == node.Attributes.Hashitems.ContainsKey("data-rawheight"))
                            imageRun.Height = Double.Parse(node.Attributes["data-rawheight"].Value);

                        collection.Add(imageRun);

                        if (imageRun.Width > 0)
                        {
                            collection.Add(new LineBreakRun());
                        }
                    }

                    #endregion
                }
                else
                {
                    foreach (var childNode in node.ChildNodes)
                    {
                        collection.AddRange(GetInlines(childNode));
                    }
                }

                #endregion

                return collection;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private RunBase GetVideobox(HtmlNode node)
        {
            if (node.Name == "div" &&
                node.Attributes.Hashitems.ContainsKey("class") &&
                node.Attributes["class"].Value == "video-box")
            {
                var thumbSrc = String.Empty;
                var title = String.Empty;
                var hostPage = String.Empty;

                var video = node.Attributes["data-swfurl"].Value;

                var nodeVideoInner = node.ChildNodes.First(n => n.Name == "div" &&
                                                                n.Attributes.Hashitems.ContainsKey("class") &&
                                                                n.Attributes["class"].Value == "video-box-inner");

                foreach (var childNode in nodeVideoInner.ChildNodes)
                {
                    if (childNode.Name == "div" && childNode.Attributes.Hashitems.ContainsKey("class") &&
                        childNode.Attributes["class"].Value == "video-thumb")
                    {
                        var thumbNode = childNode.ChildNodes.FindFirst("img");
                        thumbSrc = thumbNode.Attributes["src"].Value;
                    }
                    else if (childNode.Name == "div" && childNode.Attributes.Hashitems.ContainsKey("class") &&
                             childNode.Attributes["class"].Value == "video-box-body")
                    {
                        var titleNode =
                            childNode.ChildNodes.First(
                                n =>
                                    n.Name == "div" && n.Attributes.Hashitems.ContainsKey("class") &&
                                    n.Attributes["class"].Value == "video-title");
                        title = titleNode.InnerText;

                        var videoNode =
                            childNode.ChildNodes.First(
                                n =>
                                    n.Name == "div" && n.Attributes.Hashitems.ContainsKey("class") &&
                                    n.Attributes["class"].Value == "video-url");

                        hostPage = videoNode.InnerText;
                    }
                }

                return new VideoRun() { Thumb = thumbSrc, Title = title, HostPage = hostPage, Video = video };
            }
            return null;
        }

        private ParagraphModel MergePhagraph(List<ParagraphModel> paragraphs, ParagraphModel paragraph)
        {
            if (paragraph.Runs.Count == 0) return paragraph;

            paragraphs.Add(paragraph);

            paragraph = new ParagraphModel();

            return paragraph;
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

        #endregion
    }
}
