using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zhihu.Common.Model;
using Zhihu.Common.Model.Result;

namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class DesignPersonService : IPerson
    {
        public Task<UnReadResult> CheckUnReadAsync(String access)
        {
            throw new NotImplementedException();
        }

        public async Task<ExplorePeopleResult> GetAmazingGuysAsync(String accessToken)
        {
            const String json = @"{\'paging\':{\'next\':\'https:\/\/api.zhihu.com\/explore\/people?limit=20&offset=20\',\'previous\':\'https:\/\/api.zhihu.com\/explore\/people?limit=20&offset=0\'},\'data\':[{\'headline\':\'\',\'avatar_url\':\'http:\/\/pic3.zhimg.com\/59bc2bdf4_s.jpg\',\'name\':\'yolfilm\',\'url\':\'https:\/\/api.zhihu.com\/people\/28bb2b6ff09a5072198351434ab2efff\',\'gender\':1,\'type\':\'people\',\'id\':\'28bb2b6ff09a5072198351434ab2efff\'},{\'headline\':\'V2EX\',\'avatar_url\':\'http:\/\/pic3.zhimg.com\/7469f202e_s.jpg\',\'name\':\'\u5218\u6615\',\'url\':\'https:\/\/api.zhihu.com\/people\/455b06074c9e37df07146f86294f6373\',\'gender\':-1,\'type\':\'people\',\'id\':\'455b06074c9e37df07146f86294f6373\'},{\'headline\':\'\u6240\u6709\u56de\u7b54\u90fd\u662f\u4e2a\u4eba\u5224\u65ad\uff0c\u8981\u662f\u518d\u56de\u590d\u8bc4\u8bba\u5c31\u8ba9\u6211\u51fa\u95e8\u88ab\u8f66\u649e\u6b7b\',\'avatar_url\':\'http:\/\/pic4.zhimg.com\/9a1f72f21_s.jpg\',\'name\':\'\u8d1f\u4e8c\',\'url\':\'https:\/\/api.zhihu.com\/people\/05cb7f4a0ddb9955c7e709161f779757\',\'gender\':1,\'type\':\'people\',\'id\':\'05cb7f4a0ddb9955c7e709161f779757\'},{\'headline\':\'\u300a\u592a\u91ab\u4f86\u4e86\u300b\u7b2c\u56db\u671f\u4e0a\u7dda\uff01http:\/\/taiyilaile.com\',\'avatar_url\':\'http:\/\/pic3.zhimg.com\/d747ac93c_s.jpg\',\'name\':\'Lawrence Li\',\'url\':\'https:\/\/api.zhihu.com\/people\/6bec872206d9884cd9535841b6a1f510\',\'gender\':0,\'type\':\'people\',\'id\':\'6bec872206d9884cd9535841b6a1f510\'},{\'headline\':\'\u300cIT \u516c\u8bba\u300d\u4e3b\u64ad www.itgonglun.com\',\'avatar_url\':\'http:\/\/pic1.zhimg.com\/ffc0a08ff_s.jpg\',\'name\':\'Rio\',\'url\':\'https:\/\/api.zhihu.com\/people\/0d81a29a497b91e0f374ae0508de779d\',\'gender\':1,\'type\':\'people\',\'id\':\'0d81a29a497b91e0f374ae0508de779d\'},{\'headline\':\'\u9b45\u65cf\u8425\u9500\u4e2d\u5fc3\u62db\u52df\u8bbe\u8ba1\u5e08\',\'avatar_url\':\'http:\/\/pic1.zhimg.com\/24f3a654b_s.jpg\',\'name\':\'\u674e\u6960\',\'url\':\'https:\/\/api.zhihu.com\/people\/29c3654588fd4246bb90cbd345242d65\',\'gender\':1,\'type\':\'people\',\'id\':\'29c3654588fd4246bb90cbd345242d65\'},{\'headline\':\'\u5468\u672b\u753b\u62a5\/\u5f6d\u535a\u5546\u4e1a\u5468\u520a\u4e2d\u6587\u7248\u3002\u6025\u9700TMT\u62a5\u9053\u4e13\u624d\uff0c\u6709\u5174\u8da3\u79c1\u4fe1\u6216\u8054\u7cfb liufengatwork # gmail.com\',\'avatar_url\':\'http:\/\/pic2.zhimg.com\/4d2dd4f2f_s.jpg\',\'name\':\'\u5218\u950b\',\'url\':\'https:\/\/api.zhihu.com\/people\/4e076dc635ff5e0ff87bb2f4bb38b699\',\'gender\':-1,\'type\':\'people\',\'id\':\'4e076dc635ff5e0ff87bb2f4bb38b699\'},{\'headline\':\'\u5e73\u9762\u8bbe\u8ba1\u5e08\',\'avatar_url\':\'http:\/\/pic4.zhimg.com\/197aef737_s.jpg\',\'name\':\'\u8303\u5708\u5708\',\'url\':\'https:\/\/api.zhihu.com\/people\/7e291c0bffbec3f9a6aa1b6003a6adf4\',\'gender\':0,\'type\':\'people\',\'id\':\'7e291c0bffbec3f9a6aa1b6003a6adf4\'},{\'headline\':\'Sociologist\',\'avatar_url\':\'http:\/\/pic2.zhimg.com\/34be89066_s.jpg\',\'name\':\'\u5927Joy\',\'url\':\'https:\/\/api.zhihu.com\/people\/7b9210d72c8fe5eb3a57bb774bd94ca5\',\'gender\':0,\'type\':\'people\',\'id\':\'7b9210d72c8fe5eb3a57bb774bd94ca5\'},{\'headline\':\'KnewOne.com \u91cd\u5ea6\u7528\u6237\',\'avatar_url\':\'http:\/\/pic4.zhimg.com\/3d4085b43_s.jpg\',\'name\':\'\u4e8e\u6b23\u70c8\',\'url\':\'https:\/\/api.zhihu.com\/people\/246e6cf44e94cefbf4b959cb5042bc91\',\'gender\':1,\'type\':\'people\',\'id\':\'246e6cf44e94cefbf4b959cb5042bc91\'},{\'headline\':\'DineHQ.com\',\'avatar_url\':\'http:\/\/pic4.zhimg.com\/4e506e6f8_s.jpg\',\'name\':\'\u675c\u6f47\',\'url\':\'https:\/\/api.zhihu.com\/people\/fbe647152af631c248aaaa65e319fd7e\',\'gender\':1,\'type\':\'people\',\'id\':\'fbe647152af631c248aaaa65e319fd7e\'},{\'headline\':\'\u6d2a\u6ce2 t.qq.com\/keso-me\',\'avatar_url\':\'http:\/\/pic4.zhimg.com\/c677f2847_s.jpg\',\'name\':\'keso\',\'url\':\'https:\/\/api.zhihu.com\/people\/17236dbda9d1e11286068ade7021c335\',\'gender\':1,\'type\':\'people\',\'id\':\'17236dbda9d1e11286068ade7021c335\'},{\'headline\':\'014\u53f7\u77e5\u53cb\',\'avatar_url\':\'http:\/\/pic1.zhimg.com\/e0591bdfe_s.jpg\',\'name\':\'\u8d75\u963f\u840c\',\'url\':\'https:\/\/api.zhihu.com\/people\/95c5ad48d0bef82c84ff134e1798d76e\',\'gender\':0,\'type\':\'people\',\'id\':\'95c5ad48d0bef82c84ff134e1798d76e\'},{\'headline\':\'\u77e5\u4e4e 001 \u53f7\u5458\u5de5\',\'avatar_url\':\'http:\/\/pic3.zhimg.com\/c9e094344_s.jpg\',\'name\':\'\u5468\u6e90\',\'url\':\'https:\/\/api.zhihu.com\/people\/6733f12c60e7e98ea7491f20de46f79e\',\'gender\':1,\'type\':\'people\',\'id\':\'6733f12c60e7e98ea7491f20de46f79e\'},{\'headline\':\'\u8010\u5fc3\u662f\u7f8e\u5fb7\',\'avatar_url\':\'http:\/\/pic2.zhimg.com\/465513667_s.jpg\',\'name\':\'\u5f20\u4eae\',\'url\':\'https:\/\/api.zhihu.com\/people\/8b68876001197b3b9cd605b20814616f\',\'gender\':1,\'type\':\'people\',\'id\':\'8b68876001197b3b9cd605b20814616f\'},{\'headline\':\'\u6ca1\u5934\u8111\u548c\u4e0d\u9ad8\u5174\',\'avatar_url\':\'http:\/\/pic1.zhimg.com\/327a6c851_s.jpg\',\'name\':\'\u5218\u6df3\',\'url\':\'https:\/\/api.zhihu.com\/people\/ac89219fc2558ad571dab9c0c14cba88\',\'gender\':1,\'type\':\'people\',\'id\':\'ac89219fc2558ad571dab9c0c14cba88\'},{\'headline\':\'\u91d1\u878d\',\'avatar_url\':\'http:\/\/pic3.zhimg.com\/c82591eb0_s.jpg\',\'name\':\'\u4f59\u4ea6\u591a\',\'url\':\'https:\/\/api.zhihu.com\/people\/148b0aa95b520d225d3435acc0c6ba57\',\'gender\':1,\'type\':\'people\',\'id\':\'148b0aa95b520d225d3435acc0c6ba57\'},{\'headline\':\'\u548c\u77e5\u4e4e\u5728\u4e00\u8d77\',\'avatar_url\':\'http:\/\/pic4.zhimg.com\/ee581ba52_s.jpg\',\'name\':\'\u9ec4\u7ee7\u65b0\',\'url\':\'https:\/\/api.zhihu.com\/people\/b6f80220378c8b0b78175dd6a0b9c680\',\'gender\':1,\'type\':\'people\',\'id\':\'b6f80220378c8b0b78175dd6a0b9c680\'},{\'headline\':\'leaming\',\'avatar_url\':\'http:\/\/pic4.zhimg.com\/2a8a5519f_s.jpg\',\'name\':\'\u5468\u4e2d\u77f3\',\'url\':\'https:\/\/api.zhihu.com\/people\/4541b3754c5968118105714fbdf7eb4f\',\'gender\':1,\'type\':\'people\',\'id\':\'4541b3754c5968118105714fbdf7eb4f\'},{\'headline\':\'\',\'avatar_url\':\'http:\/\/pic2.zhimg.com\/f050c87ad_s.jpg\',\'name\':\'talich\',\'url\':\'https:\/\/api.zhihu.com\/people\/02885f65b7e37ea368f7bd05d8c6e424\',\'gender\':1,\'type\':\'people\',\'id\':\'02885f65b7e37ea368f7bd05d8c6e424\'}]}";
            var obj = JsonConvert.DeserializeObject<ExplorePeople>(json);

            await Task.Delay(100);

            return new ExplorePeopleResult(obj);
        }

        public async Task<FollowingResult> FollowAsync(String accessToken, String person)
        {
            const String json = @"{\'is_following\':true}";
            var obj = JsonConvert.DeserializeObject<Following>(json);

            await Task.Delay(100);

            return new FollowingResult(obj);
        }

        public async Task<FollowingResult> UnFollowAsync(String accessToken, String person, String user)
        {
            const String json = @"{\'is_following\':false}";
            var obj = JsonConvert.DeserializeObject<Following>(json);

            await Task.Delay(100);

            return new FollowingResult(obj);
        }

        public async Task<ProfileResult> GetProfileAsync(String accessToken, String userId = "self", Boolean autoCache = false)
        {
            const String json = @"{\'following_count\':1140,\'shared_count\':854,\'ask_about_count\':0,\'education\':[{\'name\':\'\u534e\u5357\u7406\u5de5\u5927\u5b66\',\'url\':\'https:\/\/api.zhihu.com\/topics\/19599737\',\'excerpt\':\'\u534e\u5357\u7406\u5de5\u5927\u5b66\uff08\u539f\u534e\u5357\u5de5\u5b66\u9662\uff0c1952\u5e74\u5efa\u7acb\uff09\uff1a\u6559\u80b2\u90e8\u76f4\u5c5e\u7684\u91cd\u70b9\u5927\u5b66\uff0c\u6db5\u76d6\u7406\u3001\u5de5\u3001\u7ba1\u3001\u7ecf\u3001\u6587\u3001\u6cd5\u7b49\u591a\u5b66\u79d1\uff0c\u5148\u540e\u6210\u4e3a\u201c211\u5de5\u7a0b\u201d\u548c\u201c985\u5de5\u7a0b\u201d\u9662\u6821\uff0c\u88ab\u8a89\u4e3a\u4e2d\u56fd\u2026\',\'experience\':\'\',\'introduction\':\'\u534e\u5357\u7406\u5de5\u5927\u5b66\uff08\u539f\u534e\u5357\u5de5\u5b66\u9662\uff0c1952\u5e74\u5efa\u7acb\uff09\uff1a\u6559\u80b2\u90e8\u76f4\u5c5e\u7684\u91cd\u70b9\u5927\u5b66\uff0c\u6db5\u76d6\u7406\u3001\u5de5\u3001\u7ba1\u3001\u7ecf\u3001\u6587\u3001\u6cd5\u7b49\u591a\u5b66\u79d1\uff0c\u5148\u540e\u6210\u4e3a\u201c211\u5de5\u7a0b\u201d\u548c\u201c985\u5de5\u7a0b\u201d\u9662\u6821\uff0c\u88ab\u8a89\u4e3a\u4e2d\u56fd\u201c\u5357\u65b9\u5de5\u79d1\u5927\u5b66\u7684\u4e00\u9762\u65d7\u5e1c\u201d\uff0c\u201c\u5de5\u7a0b\u5e08\u7684\u6447\u7bee\u201d\uff0c\u201c\u4f01\u4e1a\u5bb6\u7684\u6447\u7bee\u201d\u3002\u6821\u56ed\u5206\u4e3a\u4e24\u4e2a\u6821\u533a\uff0c\u5317\u6821\u533a\u4f4d\u4e8e\u5e7f\u5dde\u5e02\u5929\u6cb3\u533a\u4e94\u5c71\u9ad8\u6821\u533a\uff0c\u5357\u6821\u533a\u4f4d\u4e8e\u5e7f\u5dde\u5e02\u756a\u79ba\u533a\u5e7f\u5dde\u5927\u5b66\u57ce\u5185\u3002\u5b66\u6821\u5360\u5730\u9762\u79ef4417\u4ea9(\u5176\u4e2d\u5357\u6821\u533a1677\u4ea9)\u3002\u5317\u6821\u533a\u6e56\u5149\u5c71\u8272\u4ea4\u76f8\u8f89\u6620\uff0c\u7eff\u6811\u7e41\u82b1\u9999\u98d8\u56db\u5b63\uff0c\u6c11\u65cf\u5f0f\u5efa\u7b51\u4e0e\u73b0\u4ee3\u5316\u697c\u7fa4\u9519\u843d\u6709\u81f4\uff0c\u73af\u5883\u4f18\u7f8e\u6e05\u65b0\uff0c\u6587\u5316\u5e95\u8574\u6df1\u539a\uff0c\u662f\u6559\u80b2\u90e8\u547d\u540d\u7684\u201c\u6587\u660e\u6821\u56ed\u201d\uff1b\u5357\u6821\u533a\u662f\u4e00\u4e2a\u73af\u5883\u4f18\u7f8e\u3001\u8bbe\u65bd\u5148\u8fdb\u3001\u7ba1\u7406\u5b8c\u5584\u3001\u5236\u5ea6\u521b\u65b0\u7684\u73b0\u4ee3\u5316\u6821\u56ed\uff0c\u662f\u8398\u8398\u5b66\u5b50\u6c42\u5b66\u7684\u7406\u60f3\u4e4b\u5730\u3002\',\'avatar_url\':\'http:\/\/pic1.zhimg.com\/4d0d193a9_s.jpg\',\'type\':\'topic\',\'id\':\'19599737\'}],\'employment\':[[{\'name\':\'Microsoft\',\'url\':\'https:\/\/api.zhihu.com\/topics\/19638573\',\'excerpt\':\'\',\'experience\':\'\',\'introduction\':\'\',\'avatar_url\':\'http:\/\/pic1.zhimg.com\/e82bab09c_s.jpg\',\'type\':\'topic\',\'id\':\'19638573\'},{\'name\':\'SDE\',\'url\':\'https:\/\/api.zhihu.com\/topics\/19785124\',\'excerpt\':\'\',\'experience\':\'\',\'introduction\':\'\',\'avatar_url\':\'http:\/\/pic4.zhimg.com\/e82bab09c_s.jpg\',\'type\':\'topic\',\'id\':\'19785124\'}]],\'id\':\'0970f947b898ecc0ec035f9126dd4e08\',\'favorite_count\':1,\'voteup_count\':142657,\'following_columns_count\':19,\'business\':{\'name\':\'\u8ba1\u7b97\u673a\u8f6f\u4ef6\',\'url\':\'https:\/\/api.zhihu.com\/topics\/19619368\',\'excerpt\':\'\u5fbc\',\'introduction\':\'\u5fbc\',\'avatar_url\':\'http:\/\/pic2.zhimg.com\/e82bab09c_s.jpg\',\'type\':\'topic\',\'id\':\'19619368\'},\'headline\':\'\u4e13\u4e1a\u9020\u8f6e\u5b50 www.gaclib.net\',\'favorited_count\':33394,\'location\':[{\'name\':\'\u897f\u96c5\u56fe\uff08Seattle\uff09\',\'url\':\'https:\/\/api.zhihu.com\/topics\/19583552\',\'excerpt\':\'\u897f\u96c5\u56fe\u662f\u7f8e\u56fd\u897f\u5317\u90e8\u6700\u5927\u7684\u57ce\u5e02\u3002\u591a\u5bb6\u9ad8\u79d1\u6280\u516c\u53f8\u7684\u603b\u90e8\uff08Microsoft, Amazon\uff0cBoeing \u7b49\u7b49\uff09\u5750\u843d\u4e8e\u6b64\u3002\',\'experience\':\'\',\'introduction\':\'\u897f\u96c5\u56fe\u662f\u7f8e\u56fd\u897f\u5317\u90e8\u6700\u5927\u7684\u57ce\u5e02\u3002\u591a\u5bb6\u9ad8\u79d1\u6280\u516c\u53f8\u7684\u603b\u90e8\uff08Microsoft, Amazon\uff0cBoeing \u7b49\u7b49\uff09\u5750\u843d\u4e8e\u6b64\u3002\',\'avatar_url\':\'http:\/\/pic4.zhimg.com\/053cff427_s.jpg\',\'type\':\'topic\',\'id\':\'19583552\'}],\'follower_count\':48785,\'type\':\'people\',\'email\':\'vczh@163.com\',\'following_topic_count\':11,\'description\':\'\u957f\u671f\u770b\u52a8\u753b\u7247\uff0c\u505a\u7f16\u8bd1\u5668\u56fe\u5f62\u5b66\u3001\u770b\u7740\u5c0floli\u53d1\u6296\uff0c\u5f00\u53d1http:\/\/www.gaclib.net\u3002 \',\'qq_weibo_name\':\'\',\'columns_count\':2,\'sina_weibo_url\':\'http:\/\/weibo.cn\/u\/1916825084\',\'is_active\':true,\'answer_count\':4755,\'question_count\':149,\'has_daily_recommend_permission\':false,\'name\':\'vczh\',\'url\':\'https:\/\/api.zhihu.com\/people\/0970f947b898ecc0ec035f9126dd4e08\',\'gender\':1,\'following_collection_count\':0,\'sina_weibo_name\':\'GeniusVczh\',\'avatar_url\':\'http:\/\/pic3.zhimg.com\/88db1114b_s.jpg\',\'following_question_count\':5284,\'thanked_count\':19581,\'qq_weibo_url\':\'\'}";

            var obj = JsonConvert.DeserializeObject<Profile>(json);

            await Task.Delay(100);

            return new ProfileResult(obj);
        }

        public Task<NotificationsResult> CheckNotificationsAsync(String accessToken)
        {
            throw new NotImplementedException();
        }

        public Task<ActivitiesResult> GetActivitiesAsync(String access, String request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public async Task<PeopleFollowingResult> CheckFollowingAsync(String accessToken, String userId, Boolean autoCache = false)
        {
            const String json = @"{\'is_followed\':false,\'is_following\':true}";

            var obj = JsonConvert.DeserializeObject<PeopleFollowing>(json);

            await Task.Delay(100);

            return new PeopleFollowingResult(obj);
        }


        public Task<AnswersResult> GetAnswersAsync(string access, string request, string orderBy, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }


        public Task<QuestionsResult> GetQuestionsAsync(string access, string request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }


        public Task<CollectionsResult> GetCollectionsAsync(string access, string request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }


        public Task<ColumnsResult> GetColumnsAsync(string access, string request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<ListResultBase> GetFollowingTopicsAsync(String access, String request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<ListResultBase> GetFollowersAsync(String access, String request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }

        public Task<ListResultBase> GetFolloweesAsync(String access, String request, Boolean autoCache = false)
        {
            throw new NotImplementedException();
        }
    }
}
