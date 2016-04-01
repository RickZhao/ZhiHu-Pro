using System;
using System.Collections.Generic;

namespace Zhihu.Common.Helper
{
    public sealed class VerbModels
    {
        public static readonly List<VerbModel> ForNotify = new List<VerbModel>()
        {
            // Notify
            new VerbModel()
            {
                Verbs = new List<String>() {"ANSWER_CREATE",},
                Display = "回答了该问题",
            },
            new VerbModel()
            {
                Verbs = new List<String>() { "ARTICLE_PUBLISH", "ARTICLE_CREATE" },
                Display = "发表了文章",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"COMMENT_CREATE_IN_ANSWER",},
                Display = "评论了该答案",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"MENTION_IN_ANSWER_COMMENT",},
                Display = "在评论中提到了你",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"REPLY_IN_ANSWER_COMMENT",},
                Display = "回复了该答案的评论",
            },
            new VerbModel()
            {
                Verbs = new List<String>() { "ANSWER_VOTE_UP", },
                Display = "赞同了你的回答",
            },
            new VerbModel()
            {
                Verbs = new List<String>() { "ANSWER_THANKS", },
                Display = "感谢了你的回答",
            },
            new VerbModel()
            {
                Verbs = new List<String>() { "COMMENT_LIKE_IN_ANSWER", },
                Display = "赞了你的评论",
            },
            new VerbModel()
            {
                Verbs = new List<string>() { "QUESTION_SUGGEST_EDIT", },
                Display ="提问被建议重新编辑",
            },
            new VerbModel()
            {
                Verbs = new List<String>() { "REPLY_IN_ARTICLE_COMMENT",},
                Display = "回复了文章的评论",
            },
            new VerbModel()
            {
                Verbs = new List<string> () { "COMMENT_CREATE_IN_ARTICLE" },
                Display = "评论了文章"
            },
            new VerbModel()
            {
                Verbs = new List<String>() { "MENTION_IN_ARTICLE_COMMENT", },
                Display = "在评论中提到了你",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"QUESTION_ASK_PEOPLE_ANSWER"},
                Display = "回答了该问题",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"COLUMN_ADD_AUTHOR"},
                Display = "邀请您成为专栏作者",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"ARTICLE_VOTE_UP"},
                Display = "赞了您的文章",
            },
        };

        public static readonly List<VerbModel> ForFeed = new List<VerbModel>()
        {
            new VerbModel()
            {
                Verbs = new List<String>() {"ROUNDTABLE_FOLLOW", "MEMBER_FOLLOW_ROUNDTABLE",},
                Display = "关注了圆桌",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"MEMBER_CREATE_ARTICLE", "COLUMN_NEW_ARTICLE",},
                Display = "发表了文章",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"ARTICLE_CREATE"},
                Display = "在 {0} 发表文章",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"MEMBER_VOTEUP_ARTICLE", "ARTICLE_VOTE_UP",},
                Display = "赞同该文章",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"MEMBER_VOTEUP_ANSWER", "ANSWER_VOTE_UP",},
                Display = "赞同该回答",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"MEMBER_ANSWER_QUESTION", "ANSWER_CREATE"},
                Display = "回答了该问题",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"MEMBER_ASK_QUESTION", "MEMBER_CREATE_QUESTION", "QUESTION_CREATE",},
                Display = "提了一个问题",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"MEMBER_FOLLOW_QUESTION", "QUESTION_FOLLOW",},
                Display = "关注了该问题",
            },
            new VerbModel()
            {
                Verbs = new List<string>() {"MEMBER_FOLLOW_COLUMN"},
                Display = "关注了专栏"
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"COLUMN_POPULAR_ARTICLE"},
                Display = "中很多人赞同该文章",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"TOPIC_POPULAR_QUESTION", "TOPIC_ACKNOWLEDGED_ANSWER",},
                Display = "",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"PROMOTION_ANSWER",},
                Display = "热门问答",
            },
            new VerbModel()
            {
                Verbs = new List<String>() {"PROMOTION_ARTICLE",},
                Display = "热门问答",
            },

        };
    }
}