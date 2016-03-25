
using System;

namespace Zhihu.Common.HtmlAgilityPack
{
    public class NameValuePair
    {
        #region Fields

        internal readonly String Name;
        internal String Value;

        #endregion

        #region Constructors

        internal NameValuePair()
        {
        }

        internal NameValuePair(String name)
            :
                this()
        {
            Name = name;
        }

        internal NameValuePair(String name, String value)
            :
                this(name)
        {
            Value = value;
        }

        #endregion
    }
}
