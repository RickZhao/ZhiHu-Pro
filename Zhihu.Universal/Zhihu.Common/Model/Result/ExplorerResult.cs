using System;


namespace Zhihu.Common.Model.Result
{
    public sealed class ExplorePeopleResult : ListResultBase
    {
        public ExplorePeopleResult(ExplorePeople result)
            : base(result)
        {
        }

        public ExplorePeopleResult(Exception error)
            : base(error)
        {
        }
    }

    public sealed class EditorRecommendsResult : ListResultBase
    {
        public EditorRecommendsResult(EditorRecommends result)
            : base(result)
        {
        }

        public EditorRecommendsResult(Exception error)
            : base(error)
        {
        }
    }

    public sealed class BannerResult
    {
        public Banner Result { get; private set; }
        public Exception Error { get; private set; }

        private BannerResult()
        {

        }

        public BannerResult(Banner result)
        {
            Result = result;
        }

        public BannerResult(Exception error)
        {
            Error = error;
        }
    }

    public sealed class HotTopicsCollectionsResult
    {
        public HotTopicsCollections Result { get; private set; }
        public Exception Error { get; private set; }

        private HotTopicsCollectionsResult()
        {

        }

        public HotTopicsCollectionsResult(HotTopicsCollections result)
        {
            Result = result;
        }

        public HotTopicsCollectionsResult(Exception error)
        {
            Error = error;
        }
    }
}
