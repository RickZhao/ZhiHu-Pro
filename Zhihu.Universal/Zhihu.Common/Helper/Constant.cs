using System;
using Windows.Foundation;

namespace Zhihu.Common.Helper
{
    public class Constant
    {
        public class DefaultFontSize
        {
            public const double PageTitle = 32.0;
            public const double FeedTitle = 19.0;
            public const double FeedVerb = 14.0;
            public const double FeedSummary = 15.0;
            public const double VoteCount = 11.0;
        }
        public class DefaultLayout
        {
            public const Boolean LowerVotingButtonVisible = true;
            public const Boolean UpperVotingButtonVisible = false;
            public const Boolean LowerVotingButtonPairVisible = false;
            public const Boolean ThanksButtonVisible = true;
            public const Boolean NotHelpfulButtonVisible = true;
            public const Boolean AddToCollectionButtonVisible = false;
            public const Boolean CommentButtonVisible = true;
            public const Boolean ShareToWechatButtonVisible = false;
            public const Boolean OpenWithEdgeButtonVisible = false;
        }
    }
}