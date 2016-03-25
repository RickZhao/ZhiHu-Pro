using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;
using Zhihu.Common.Net;


namespace Zhihu.Common.Service.Runtime
{
// ReSharper disable once ClassNeverInstantiated.Global
    public sealed class LoginService : ILogin
    {
        public async Task<LoginResult> LoginAsync(String email, String password)
        {
            var api = new UserApi();
            var result = await api.LoginAsync(email, password);

            return result;
        }

        public async Task<OperationResult> RegisterAsync(String email, String password, String lastName,
            String firstName, Int32 gender, String clientId)
        {
            var api = new UserApi();
            var result = await api.RegisterAsync(email, password, lastName, firstName, gender, clientId);

            return result;
        }
    }
}
