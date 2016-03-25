
namespace Zhihu.Common.HtmlAgilityPack
{
    internal enum ParseState
    {
        Text,
        WhichTag,
        Tag,
        BetweenAttributes,
        EmptyTag,
        AttributeName,
        AttributeBeforeEquals,
        AttributeAfterEquals,
        AttributeValue,
        Comment,
        QuotedAttributeValue,
        ServerSideCode,
        PcData
    }
}
