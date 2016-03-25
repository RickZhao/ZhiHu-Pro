using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;

namespace Zhihu.Common.Service.Design
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class DesignLoginService : ILogin
    {
        public Task<LoginResult> LoginAsync(String email, String password)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> RegisterAsync(String email, String password, String lastName, String firstName,
            Int32 gender, String clientId)
        {
            throw new NotImplementedException();
        }
    }
}
