
using System;

namespace Zhihu.Common.HtmlAgilityPack
{
    /// <summary>
    /// Represents an HTML text node.
    /// </summary>
    public class HtmlTextNode : HtmlNode
    {
        #region Fields

        private String _text;

        #endregion

        #region Constructors

        internal HtmlTextNode(HtmlDocument ownerdocument, Int32 index)
            :
                base(HtmlNodeType.Text, ownerdocument, index)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to OuterHtml.
        /// </summary>
        public override String InnerHtml
        {
            get { return OuterHtml; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or Sets the object and its content in HTML.
        /// </summary>
        public override String OuterHtml
        {
            get
            {
                if (_text == null)
                {
                    return base.OuterHtml;
                }
                return _text;
            }
        }

        /// <summary>
        /// Gets or Sets the text of the node.
        /// </summary>
        public String Text
        {
            get
            {
                if (_text == null)
                {
                    return base.OuterHtml;
                }
                return _text;
            }
            set { _text = value; }
        }

        #endregion
    }
}
