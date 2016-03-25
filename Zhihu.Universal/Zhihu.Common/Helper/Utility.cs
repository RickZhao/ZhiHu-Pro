using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Windows.Networking.Connectivity;

namespace Zhihu.Common.Helper
{
    /// <summary>
    /// 随机码生成类
    /// </summary>
    public sealed class Utility
    {
        #region Singleton

        private static Utility _instance;

        private Utility()
        {
        }

        /// <summary>
        /// LazyLoad的单例模式
        /// </summary>
        public static Utility Instance
        {
            get
            {
                if (null == _instance)
                {
                    _instance = new Utility();
                }
                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// 获取一个随机码
        /// </summary>
        /// <returns>以字符串形式返回</returns>
        public String NextRand
        {
            get
            {
                var random = new Random();
                var result = random.Next(320000000, 330000000);
                return result.ToString();
            }
        }

        public String BaseUri
        {
            get { return "https://api.zhihu.com"; }
        }
     
        #region Keys

        public String CurrentThemeKey
        {
            get { return "CurrentTheme"; }
        }

        public String UserTokenKey
        {
            get { return "UserTokenKey"; }
        }

        public String ExpiresDateKey
        {
            get { return "ExpiresDate"; }
        }

        public String NoImageKey
        {
            get { return "NoImageKey"; }
        }

        public String AllowTextSelectionKey
        {
            get { return "AllowTextSelectionKey"; }
        }

        public String StatusBarIsOpenKey
        {
            get { return "StatusBarIsOpenKey"; }
        }

        public String HasReviewedKey
        {
            get { return "HasReviewedKey"; }
        }

        public String LastRemindKey
        {
            get { return "LastRemindKey"; }
        }

        public String LastPromptContributeKey
        {
            get { return "LastPromptContributeKey"; }
        }

        #endregion

        public String EmailRegex
        {
            get
            {
                return
                    @"^([\!#\$%&'\*\+/\=?\^`\{\|\}~a-zA-Z0-9_-]+[\.]?)+[\!#\$%&'\*\+/\=?\^`\{\|\}~a-zA-Z0-9_-]+@{1}((([0-9A-Za-z_-]+)([\.]{1}[0-9A-Za-z_-]+)*\.{1}([A-Za-z]){1,6})|(([0-9]{1,3}[\.]{1}){3}([0-9]{1,3}){1}))$";
            }
        }

        public String PasswordRegex
        {
            get { return @"^[0-9a-zA-Z`~!@#$%\^&*()_+-={}|\[\]:"";'<>?,.]{6,20}$"; }
        }

        public String ClientId
        {
            get { return "a09343e8e67e44b29e0d850c14c7bf"; }
        }

        public String ClientSecret
        {
            get { return "9a161c4fe6c84609ae301017a61afc"; }
        }

        public String UmengAppKey
        {
            get
            {
                return "56277ad867e58e354e00364c";
            }
        }

        public String WeChatAppId { get { return "wxf7a78a2f45efc55d"; } }
        public String WeChatAppSecret { get { return "7ed72fc0fe00ba9027c43843b22c4c25"; } }

        public String DemoFeeds
        {
            get
            {
                return
                    @"{'paging':{'next':'https:\/\/api.zhihu.com\/feeds?limit=20&after_id=a10944456_104_0','previous':'https:\/\/api.zhihu.com\/feeds?before_id=a10938774_104_0&limit=20'},'data':[{'count':1,'target':{'author':{'headline':'\u5fae\u4fe1\u53f7\uff1aread01 , 2011\u5e74\u5b8c\u6210\u6bcf\u5929\u4e00\u672c\u4e66\u4e60\u60ef','avatar_url':'http:\/\/pic2.zhimg.com\/4bae5257d_s.jpg','name':'warfalcon','url':'https:\/\/api.zhihu.com\/people\/d9adc24d8a761eebf51e9e87a635fa87','gender':1,'type':'people','id':'d9adc24d8a761eebf51e9e87a635fa87'},'url':'https:\/\/api.zhihu.com\/answers\/38867145','question':{'url':'https:\/\/api.zhihu.com\/questions\/27967203','type':'question','id':27967203,'title':'\u5982\u4f55\u8bc4\u4ef7\u5fae\u4fe1\u60e9\u7f5a\u516c\u4f17\u53f7\u6284\u88ad\u4fb5\u6743\uff1f'},'excerpt':'\u975e\u5e38\u9ad8\u5174\u80fd\u770b\u5230\u8fd9\u4e2a\u901a\u77e5\u51fa\u53f0\uff0c\u4f1a\u7acb\u523b\u6539\u53d8\u76ee\u524d\u5fae\u4fe1\u516c\u4f17\u53f7\u7684\u6574\u4e2a\u751f\u6001\uff0c\u800c\u4e14\u4e5f\u662f\u6211\u77e5\u9053\u7684\u4e92\u8054\u7f51\u4e0a\u76ee\u524d\u4e3a\u6b62\u9488\u5bf9\u4fb5\u72af\u6587\u7ae0\u7248\u6743\u6700\u4e3a\u4e25\u91cd\u7684\u5904\u7f5a\u65b9\u5f0f\uff0c\u559c\u95fb\u4e50\u89c1\uff0c\u5168\u529b\u652f\u2026','updated_time':1423035520,'comment_count':17,'created_time':1423020883,'voteup_count':313,'type':'answer','id':38867145,'thanks_count':46},'updated_time':1423056921,'verb':'ANSWER_VOTE_UP','actors':[{'headline':'','avatar_url':'http:\/\/pic4.zhimg.com\/f050c87ad_s.jpg','name':'talich','url':'https:\/\/api.zhihu.com\/people\/02885f65b7e37ea368f7bd05d8c6e424','gender':1,'type':'people','id':'02885f65b7e37ea368f7bd05d8c6e424'}],'type':'feed','id':'a10938774_104_0'},{'count':1,'target':{'follower_count':179,'title':'\u6709\u54ea\u4e9b\u503c\u5f97\u63a8\u8350\u7684\u65e5\u672c\u5e74\u8f7b\u4f5c\u5bb6\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27983557','type':'question','id':27983557,'answer_count':13},'updated_time':1423044407,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic1.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q3376101_59_0'},{'count':1,'target':{'author':{'headline':'\u8036\u9c81\u6cd5\u5f8b\u535a\u58eb\u5728\u8bfb\uff5c\u725b\u6d25\u54f2\u5b66\u653f\u6cbb\u7ecf\u6d4e(PPE)\u4e00\u7b49\u8363\u8a89\u6587\u5b66\u58eb','avatar_url':'http:\/\/pic4.zhimg.com\/0f569eaea6584a276e4ff5bfd3759199_s.jpg','name':'Youlin Yuan','url':'https:\/\/api.zhihu.com\/people\/7b29640d18571e07eed671f520a18b4f','gender':1,'type':'people','id':'7b29640d18571e07eed671f520a18b4f'},'url':'https:\/\/api.zhihu.com\/answers\/38883830','question':{'url':'https:\/\/api.zhihu.com\/questions\/27971171','type':'question','id':27971171,'title':'\u5728\u5e38\u9752\u85e4\u505a\u5b66\u6e23\u662f\u600e\u6837\u4e00\u756a\u4f53\u9a8c\uff1f'},'excerpt':'\u725b\u6d25\u672c \u8036\u9b6fJD\u3002\u5c31\u9019\u5006\u5b78\u6821\u800c\u8a00\uff0c\u4ec0\u9ebc\u300c\u78be\u58d3\u300d\u6211\u9084\u771f\u662f\u5f88\u5c11\u611f\u89ba\u5230\u7684\u3002\u6211\u8a8d\u70ba\u7279\u5225\u5728\u83c1\u82f1\u5b78\u6821\uff0c\u5b78\u751f\u6108\u52a0\u8a8d\u8b58\u5230\u4eba\u5404\u6709\u5fd7\u3002\u5927\u5bb6\u90fd\u5f88\u8070\u660e\uff0c\u4f60\u611b\u82b1\u6642\u9593\u5728\u5b78\u7fd2\u4e0a\uff0c\u5c31\u2026','updated_time':1423032460,'comment_count':34,'created_time':1423032460,'voteup_count':502,'type':'answer','id':38883830,'thanks_count':95},'updated_time':1423041662,'verb':'ANSWER_VOTE_UP','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic4.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'a10945485_104_0'},{'count':1,'target':{'author':{'headline':'\u84dd\u6e56\u8d44\u672c\u5408\u4f19\u4eba','avatar_url':'http:\/\/pic4.zhimg.com\/df1cf7e41_s.jpg','name':'\u80e1\u535a\u4e88','url':'https:\/\/api.zhihu.com\/people\/7bea8db8bf20eabf9c2895f7a20784f0','gender':0,'type':'people','id':'7bea8db8bf20eabf9c2895f7a20784f0'},'url':'https:\/\/api.zhihu.com\/answers\/14352849','question':{'url':'https:\/\/api.zhihu.com\/questions\/20136949','type':'question','id':20136949,'title':'\u300c\u4ea7\u54c1\u662f\u4e00\u4e2a\u4eba\u7684\u300d\u8fd9\u53e5\u8bdd\u662f\u5426\u6b63\u786e\uff1f\u8be5\u600e\u4e48\u7406\u89e3\uff1f'},'excerpt':'\u8fd9\u53e5\u8bdd\u91cc\u9762\u5305\u542b\u4e86\u5f88\u591a\u9053\u7406\uff1a\u4ea7\u54c1\u901a\u5e38\u90fd\u6709\u4e00\u4e2a\u7075\u9b42\u4eba\u7269\uff0c\u5c31\u662f\u4ea7\u54c1\u7ecf\u7406\u3002\u4ea7\u54c1\u7ecf\u7406\u662f\u6700\u4e86\u89e3\u7528\u6237\u7684\u4eba\u3002\u5982\u679c\u6709\u67d0\u4e9b\u516c\u53f8\u9ad8\u5c42\u6ca1\u6709\u81ea\u77e5\u4e4b\u660e\uff0c\u5728\u5177\u4f53\u4ea7\u54c1\u8bbe\u8ba1\u7684\u5c42\u9762\u4e0a\u778e\u2026','updated_time':1336123352,'comment_count':3,'created_time':1336016030,'voteup_count':122,'type':'answer','id':14352849,'thanks_count':20},'updated_time':1423041267,'verb':'ANSWER_VOTE_UP','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic1.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'a862052_104_0'},{'count':1,'target':{'follower_count':87,'title':'\u4e3a\u4ec0\u4e48\u73a9\u4e00\u5929\u4fc4\u7f57\u65af\u65b9\u5757\u540e\u7684\u665a\u4e0a\u8111\u5b50\u91cc\u6ee1\u662f\u5792\u65b9\u5757\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27950533','type':'question','id':27950533,'answer_count':14},'updated_time':1423041097,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic1.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q3362853_59_0'},{'count':1,'target':{'follower_count':72,'title':'\u5218\u770b\u5c71\u600e\u4e48\u505a\u597d\u5403\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27981080','type':'question','id':27981080,'answer_count':18},'updated_time':1423041041,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic1.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q3375114_59_0'},{'count':1,'target':{'follower_count':88,'title':'\u600e\u4e48\u770b\u5f85\u4eba\u4eba\u5f71\u89c6\u56de\u5f52\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27976164','type':'question','id':27976164,'answer_count':6},'updated_time':1423040967,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic1.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q3373143_59_0'},{'count':1,'target':{'follower_count':122,'title':'\u4e3a\u4ec0\u4e48\u670b\u53cb\u5708\u8d8a\u5237\u8d8a\u7a7a\u865a\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27905434','type':'question','id':27905434,'answer_count':35},'updated_time':1423040937,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic2.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q3344984_59_0'},{'count':1,'target':{'follower_count':52,'title':'\u6709\u54ea\u4e9b\u6f2b\u753b\u7684BOSS\u6218\u8fbe\u5230\u4e86\u704c\u7bee\u9ad8\u624b\u5c71\u738b\u4e4b\u6218\u7684\u6c34\u51c6\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27968793','type':'question','id':27968793,'answer_count':11},'updated_time':1423040926,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic3.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q3370190_59_0'},{'count':1,'target':{'follower_count':80,'title':'Amazon Prime \u5341\u5e74\u4e86\uff0c\u5b83\u662f\u5982\u4f55\u4e00\u6b65\u6b65\u8d70\u5230\u4eca\u5929\u8fd9\u4e2a\u5730\u6b65\u7684\uff1f\u90fd\u7ecf\u5386\u4e86\u4ec0\u4e48\u53d8\u5316\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27969837','type':'question','id':27969837,'answer_count':0},'updated_time':1423040880,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic1.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q3370594_59_0'},{'count':1,'target':{'follower_count':61,'title':'\u8fd9\u4e9b\u5feb\u4e50\uff0c\u662f\u4e0a\u5e1d\u7684bug\uff1f\u8fd8\u662f\u4e0a\u5e1d\u7684gift\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27979004','type':'question','id':27979004,'answer_count':14},'updated_time':1423040866,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic4.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q3374276_59_0'},{'count':1,'target':{'follower_count':145,'title':'\u652f\u4ed8\u5b9d\u51ed\u4ec0\u4e48\u662f\u963f\u91cc\u5df4\u5df4\u7684\u6838\u5fc3\u7ade\u4e89\u529b\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27982265','type':'question','id':27982265,'answer_count':7},'updated_time':1423040853,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic3.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q3375593_59_0'},{'count':1,'target':{'author':{'headline':'\u5982\u6897\u5728\u5589\uff0c\u4e0d\u5410\u4e0d\u5feb','avatar_url':'http:\/\/pic3.zhimg.com\/2f06d75f5_s.jpg','name':'\u7941\u5cf0','url':'https:\/\/api.zhihu.com\/people\/a1fd735961b060f7454340d79c3ed050','gender':1,'type':'people','id':'a1fd735961b060f7454340d79c3ed050'},'url':'https:\/\/api.zhihu.com\/answers\/33005066','question':{'url':'https:\/\/api.zhihu.com\/questions\/26500297','type':'question','id':26500297,'title':'\u865a\u6e0a\u7384\u4e3a\u4ec0\u4e48\u53eb\u300c\u7231\u7684\u6218\u58eb\u300d\uff1f'},'excerpt':'\u56e0\u4e3a\u4ed6\u4e0d\u8981\u8138\u5730\u627f\u8ba4\u4e86\u2014\u2014\u65e9\u5e74\u300a\u767d\u8c8c\u4f20\u9053\u5e08\u300b\u7684\u540e\u8bb0\uff1a\u5f53\u5e74\u6211\u56e0\u4e3a\u300cThe Warlock of Firetop Mountain\u300d\u800c\u5931\u53bb\u5947\u5e7b\u7cfb\u8d1e\u64cd\uff0c\u5e76\u4e14\u4ee5\u300cSorcery !\u300d\u56db\u90e8\u66f2\u5ea6\u8fc7\u7cdc\u70c2\u81f3\u2026','updated_time':1415252212,'comment_count':4,'created_time':1415249054,'voteup_count':33,'type':'answer','id':33005066,'thanks_count':8},'updated_time':1423039740,'verb':'ANSWER_VOTE_UP','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic4.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'a8588791_104_0'},{'count':1,'target':{'author':{'name':'\u533f\u540d\u7528\u6237','url':'','headline':'','avatar_url':'http:\/\/pic3.zhimg.com\/aadd7b895_s.jpg','type':'people','id':'0'},'url':'https:\/\/api.zhihu.com\/answers\/38876510','question':{'url':'https:\/\/api.zhihu.com\/questions\/26500297','type':'question','id':26500297,'title':'\u865a\u6e0a\u7384\u4e3a\u4ec0\u4e48\u53eb\u300c\u7231\u7684\u6218\u58eb\u300d\uff1f'},'excerpt':'Fate\/Zero \u7b2c\u4e00\u5377 \u7b2c\u56db\u6b21\u5723\u676f\u6218\u4e89\u79d8\u8bdd \u540e\u8bb0\u865a\u6e0a\u7384\u865a\u6e0a\u7384\u60f3\u5199\u80fd\u591f\u6e29\u6696\u4eba\u5fc3\u7684\u6545\u4e8b\u3002 \u4e86\u89e3\u6211\u8fc7\u53bb\u521b\u4f5c\u7ecf\u5386\u7684\u4eba\uff0c\u6050\u6015\u4f1a\u76b1\u7740\u7709\u5934\u89c9\u5f97\u8fd9\u662f\u4e00\u4e2a\u51b7\u7b11\u8bdd\u5427\u3002\u5176\u5b9e\u5c31\u7b97\u662f\u2026','updated_time':1423027487,'comment_count':0,'created_time':1423027487,'voteup_count':5,'type':'answer','id':38876510,'thanks_count':2},'updated_time':1423039737,'verb':'ANSWER_VOTE_UP','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic3.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'a10942550_104_0'},{'count':1,'target':{'follower_count':66,'title':'\u65e5\u672c\u52a8\u6f2b\u7f16\u5267\u865a\u6e0a\u7384\u7684\u7ae5\u5e74\u662f\u5426\u7ecf\u5386\u8fc7\u4e25\u91cd\u7684\u5fc3\u7406\u521b\u4f24\uff1f','url':'https:\/\/api.zhihu.com\/questions\/24652795','type':'question','id':24652795,'answer_count':12},'updated_time':1423039558,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic1.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'q2043162_59_0'},{'count':1,'target':{'follower_count':78,'title':'\u82f1\u8bed\u4e2d\uff0c\u300c\u81ea\u4ece\u300d\u4e00\u610f\uff0c\u8fd8\u6709\u54ea\u4e9b\u8bcd\u80fd\u6bd4\u300csince\u300d\u663e\u5f97\u66f4\u4e66\u9762\u8bed\u6216\u8005\u66f4\u9ad8\u7ea7\uff1f','url':'https:\/\/api.zhihu.com\/questions\/27978995','type':'question','id':27978995,'answer_count':11},'updated_time':1423036554,'verb':'QUESTION_FOLLOW','actors':[{'headline':'\u548c\u77e5\u4e4e\u5728\u4e00\u8d77','avatar_url':'http:\/\/pic4.zhimg.com\/0626f4164009f291b26a79d96c6962c5_s.jpg','name':'\u9ec4\u7ee7\u65b0','url':'https:\/\/api.zhihu.com\/people\/b6f80220378c8b0b78175dd6a0b9c680','gender':1,'type':'people','id':'b6f80220378c8b0b78175dd6a0b9c680'}],'type':'feed','id':'q3374273_59_0'},{'count':1,'target':{'author':{'headline':'Coursera\u8f6f\u4ef6\u5de5\u7a0b\u5e08','avatar_url':'http:\/\/pic2.zhimg.com\/9a6fd5b91c9d0a5c384e81f608a5d72e_s.jpg','name':'\u8463\u98de','url':'https:\/\/api.zhihu.com\/people\/17fe26c7cbd996dac6f7e9e415883d42','gender':1,'type':'people','id':'17fe26c7cbd996dac6f7e9e415883d42'},'url':'https:\/\/api.zhihu.com\/answers\/38886033','question':{'url':'https:\/\/api.zhihu.com\/questions\/27966110','type':'question','id':27966110,'title':'\u5982\u4f55\u8bc4\u4ef7\u73b0\u9636\u6bb5\u5728\u7ebf\u6559\u80b2\uff1f'},'excerpt':'\u8981\u7406\u89e3\u5728\u7ebf\u6559\u80b2\uff0c\u9996\u5148\u8981\u7406\u89e3\u6559\u80b2\u662f\u4ec0\u4e48\u3002\u56e0\u4e3a\u5728\u7ebf\u6559\u80b2\u9996\u5148\u662f\u6559\u80b2\uff0c\u5176\u6b21\u624d\u662f\u5728\u7ebf\u3002\u7f8e\u56fd\u6559\u80b2\u5fc3\u7406\u5b66\u5bb6 Benjamin Bloom \u628a\u8ba4\u77e5\u9886\u57df\u7684\u6559\u80b2\u76ee\u6807\u5206\u4e3a 6 \u4e2a\u5c42\u6b21\uff1a\u57fa\u672c\u2026','updated_time':1423036688,'comment_count':4,'created_time':1423033877,'voteup_count':32,'type':'answer','id':38886033,'thanks_count':13},'updated_time':1423034445,'verb':'ANSWER_VOTE_UP','actors':[{'headline':'\u77e5\u4e4e 001 \u53f7\u5458\u5de5','avatar_url':'http:\/\/pic3.zhimg.com\/8da264fea16f71917c8ecce0965143ae_s.jpg','name':'\u5468\u6e90','url':'https:\/\/api.zhihu.com\/people\/6733f12c60e7e98ea7491f20de46f79e','gender':1,'type':'people','id':'6733f12c60e7e98ea7491f20de46f79e'}],'type':'feed','id':'a10946380_104_0'},{'count':1,'target':{'author':{'headline':'\u6c6a\uff01','avatar_url':'http:\/\/pic2.zhimg.com\/dd09229e2_s.jpg','name':'\u826f\u8a00\u5915','url':'https:\/\/api.zhihu.com\/people\/5a403886d11acbf0c718ede230d46853','gender':1,'type':'people','id':'5a403886d11acbf0c718ede230d46853'},'url':'https:\/\/api.zhihu.com\/answers\/38420949','question':{'url':'https:\/\/api.zhihu.com\/questions\/26495738','type':'question','id':26495738,'title':'\u6709\u54ea\u4e9b\u5bb6\u5177\u5176\u5b9e\u6839\u672c\u6ca1\u5fc5\u8981\u4e70\uff1f'},'excerpt':'\u8fd9\u4e2a\u6211\u6bd4\u8f83\u53d7\u5bb3\u5176\u4e2d\uff0c\u6211\u8bf4\u4e00\u8bf4\u5427\uff1a1.HIFI\u8bbe\u5907\uff1a\u4f60\u4ee5\u4e3a\u4f1a\u662f\u8fd9\u6837\u5b50\uff1a\u5b9e\u9645\u5374\u662f\u8fd9\u6837\u5b50\uff1a\u5360\u5730\u65b9\uff0c\u4e00\u5e74\u4e0d\u77e5\u9053\u4f1a\u4e0d\u4f1a\u7528\u4e24\u4e09\u6b21\uff0c\u4e00\u5e73\u65b9\u591a\u5c11\u94b1\u6765\u8fd9\u73b0\u5728\uff1f2.\u7535\u89c6\u67dc\u50cf\u8fd9\u79cd\u2026','updated_time':1422724891,'comment_count':92,'created_time':1422551633,'voteup_count':699,'type':'answer','id':38420949,'thanks_count':166},'updated_time':1423032273,'verb':'ANSWER_VOTE_UP','actors':[{'headline':'DineHQ.com','avatar_url':'http:\/\/pic4.zhimg.com\/4c1196773d9b8baae3cdf6f5067f22de_s.jpg','name':'\u675c\u6f47','url':'https:\/\/api.zhihu.com\/people\/fbe647152af631c248aaaa65e319fd7e','gender':1,'type':'people','id':'fbe647152af631c248aaaa65e319fd7e'}],'type':'feed','id':'a10759961_104_0'},{'count':2,'target':{'author':{'headline':'You are what you fuck','avatar_url':'http:\/\/pic3.zhimg.com\/c8b1d1d7c_s.jpg','name':'\u9f99\u837b','url':'https:\/\/api.zhihu.com\/people\/95d6cad20926b2db3243f8237b777c18','gender':0,'type':'people','id':'95d6cad20926b2db3243f8237b777c18'},'url':'https:\/\/api.zhihu.com\/answers\/38845886','question':{'url':'https:\/\/api.zhihu.com\/questions\/27941775','type':'question','id':27941775,'title':'\u7f8e\u56fd\u6587\u5316\u662f\u5982\u4f55\u8fd0\u884c\u7684\uff1f'},'excerpt':'\u6211\u89c9\u5f97\u9996\u5148\u5e94\u8be5\u5b9a\u4e49\u95ee\u9898\u91cc\u7684\u7f8e\u56fd\u6587\u5316\u6240\u6307\uff0c\u800c\u8fd9\u91cc\u6307\u7684\u5927\u6982\u5c31\u662f\u6240\u8c13\u7f8e\u56fd\u5927\u4f17\u6d41\u884c\u6587\u5316\u548c\u8fd9\u79cd\u793e\u4f1a\u6587\u5316\u73af\u5883\u4e0b\u7f8e\u56fd\u4eba\u7684\u4e00\u4e9b\u7279\u8d28\uff0c\u5305\u62ec\u4ed6\u4eec\u7279\u8d28\u5728\u6211\u4eec\u5bf9\u4ed6\u4eec\u7684\u8ba4\u8bc6\u4e0a\u2026','updated_time':1422992475,'comment_count':1,'created_time':1422988174,'voteup_count':5,'type':'answer','id':38845886,'thanks_count':3},'updated_time':1423032256,'verb':'ANSWER_VOTE_UP','actors':[{'headline':'DineHQ.com','avatar_url':'http:\/\/pic4.zhimg.com\/4c1196773d9b8baae3cdf6f5067f22de_s.jpg','name':'\u675c\u6f47','url':'https:\/\/api.zhihu.com\/people\/fbe647152af631c248aaaa65e319fd7e','gender':1,'type':'people','id':'fbe647152af631c248aaaa65e319fd7e'},{'headline':'014\u53f7\u77e5\u53cb\uff0c\u77e5\u4e4e\u4e13\u680f\u52aa\u529b\u66f4\u65b0\u4e2d','avatar_url':'http:\/\/pic2.zhimg.com\/e0591bdfe_s.jpg','name':'\u8d75\u963f\u840c','url':'https:\/\/api.zhihu.com\/people\/95c5ad48d0bef82c84ff134e1798d76e','gender':0,'type':'people','id':'95c5ad48d0bef82c84ff134e1798d76e'}],'type':'feed','id':'a10930263_104_0'},{'count':1,'target':{'author':{'headline':'\u6bd2\u96fb\u6ce2\u4f7f\u3044','avatar_url':'http:\/\/pic1.zhimg.com\/d9d48964e_s.jpg','name':'\u53f6\u4f73\u6850','url':'https:\/\/api.zhihu.com\/people\/24899f12dd48bb86191ab79febb1668c','gender':1,'type':'people','id':'24899f12dd48bb86191ab79febb1668c'},'url':'https:\/\/api.zhihu.com\/answers\/38881269','question':{'url':'https:\/\/api.zhihu.com\/questions\/27970610','type':'question','id':27970610,'title':'\u5982\u679c\u4f60\u662f Type Moon \u8001\u677f\uff0c\u4f60\u8981\u5982\u4f55\u632f\u5174 Fate \u7cfb\u5217\uff1f'},'excerpt':'\u6cfb\u836f\uff0c\u5176\u5b9e\u578b\u6708\u7684\u91ce\u5fc3\u65e9\u5df2\u66b4\u9732\uff0c\u6240\u8c13\u5927\u5c40\u89c2\uff0c\u8be5\u6587\u5316\u5e03\u5c40\u7684\u5730\u65b9\u4e5f\u90fd\u5e03\u5c40\u5b8c\u6bd5\u4e86\u3002\u5148\u8bf4\u8bf4\u4e0a\u6e38\u539f\u521b\u5185\u5bb9\uff08\u5c0f\u8bf4\u3001avg\uff09\u8fd9\u4e2a\u6bd4\u8f83\u91cd\u8981\uff0c\u6574\u4e2a\u578b\u6708\u4e16\u754c\u89c2\u7531\u5948\u865a\u8611\u83c7\u4e00\u624b\u2026','updated_time':1423032042,'comment_count':7,'created_time':1423030811,'voteup_count':30,'type':'answer','id':38881269,'thanks_count':11},'updated_time':1423031939,'verb':'ANSWER_VOTE_UP','actors':[{'headline':'\u8010\u5fc3\u662f\u7f8e\u5fb7','avatar_url':'http:\/\/pic4.zhimg.com\/465513667_s.jpg','name':'\u5f20\u4eae','url':'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f','gender':1,'type':'people','id':'8b68876001197b3b9cd605b20814616f'}],'type':'feed','id':'a10944456_104_0'}]}";
            }
        }

        public Int64 Timestamp
        {
            get
            {
                var timestamp = (DateTime.Now.Ticks - new DateTime(1970, 1, 1).Ticks) / TimeSpan.TicksPerSecond;
                return timestamp;
            }
        }

        public DateTime GetFromeTimestamp(Int64 timestamp)
        {
            var from = new DateTime(1970, 1, 1);

            return from.AddSeconds(timestamp);
        }

        public String GetEasyInt32(Int32 count)
        {
            if (count < 0) return "0";

            if (count > 1000) return (count / 1000.0).ToString("F1") + "k";
            else return count.ToString();
        }

        public DateTime GetExpiresInDate(Int64 expiresIn)
        {
            return DateTime.Now.AddSeconds(expiresIn);
        }

        public Boolean IsUsingWifi
        {
            get
            {
                var netWorkAvailable = NetworkInterface.GetIsNetworkAvailable();

                if (netWorkAvailable == false) return false;

                var connection = NetworkInformation.GetInternetConnectionProfile();

                if (connection == null) return true;

                return connection.ProfileName == "Ethernet" || connection.ProfileName == "以太网"
                    || connection.IsWlanConnectionProfile || !connection.IsWwanConnectionProfile;

            }
        }

        public Boolean IsNetworkAvailable
        {
            get
            {
                var netWorkAvailable = NetworkInterface.GetIsNetworkAvailable();
                return netWorkAvailable;
            }
        }

        public String GetImageRequest(String imageUri)
        {
            var imgUrl = new Uri(imageUri);

            return imgUrl.AbsolutePath;
        }

        public String GetImageHost(String imageUri)
        {
            var imgUrl = new Uri(imageUri);

            return imgUrl.Host;
        }

        public String GetPlaintText(String request)
        {
            var hashString = MD5Core.GetHashString(Encoding.UTF8.GetBytes(request));

            var plaintText =
                new String((from c in hashString.ToCharArray() where char.IsLetterOrDigit(c) select c).ToArray());

            return plaintText;
        }
        
        public event Action<String> HyperlinkTapped;

        public void RaiseEvent(String url)
        {
            if (HyperlinkTapped == null) return;

            var handler = HyperlinkTapped;
            handler.Invoke(url);
        }
    }
}
