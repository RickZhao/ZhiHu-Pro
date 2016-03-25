using System;
using System.Collections.Generic;


namespace Zhihu.Common.Model.Html
{
    public sealed class ParagraphModel
    {
        public ParagraphModel()
        {
            Runs = new List<RunBase>();
        } 

        public IList<RunBase> Runs { get; set; }
    }

    public class RunBase
    {
    }

    public enum TextType
    {
        Plain = 0,
        Bold,
        Italic,
        H1,
        H2,
        H3
    }

    public sealed class LineBreakRun : RunBase
    {
        public override String ToString()
        {
            return "LineBreakRun";
        }
    }

    public sealed class TextRun : RunBase
    {
        public String Text { get; set; }
        public TextType Type { get; set; }

        public override String ToString()
        {
            return String.Format("TextRun: {0} {1}", Type.ToString(), Text);
        }
    }

    public sealed class ImageRun : RunBase
    {
        public String Image { get; set; }

        public Double Width { get; set; }

        public Double Height { get; set; }

        public override String ToString()
        {
            return String.Format("ImageRun: {0} ", Image);
        }
    }

    public sealed class VideoRun : RunBase
    {
        public String Thumb { get; set; }
        public String Title { get; set; }
        public String Video { get; set; }
        public String HostPage { get; set; }

        public override String ToString()
        {
            return String.Format("VideoRun: {0} {1} {2}", Title, Thumb, HostPage);
        }
    }

    public sealed class ImageHrefRun : RunBase
    {
        public String Image { get; set; }
        public String Href { get; set; }

        public Double Width { get; set; }

        public Double Height { get; set; }

        public override String ToString()
        {
            return String.Format("ImageHrefRun: {0} ", Image);
        }
    }

    public sealed class TextHrefRun : RunBase
    {
        public String Text { get; set; }
        public String Href { get; set; }
        public Boolean IsRelative { get; set; }
        public override String ToString()
        {
            return String.Format("TextHrefRun: {0} ", Text);
        }
    }
}