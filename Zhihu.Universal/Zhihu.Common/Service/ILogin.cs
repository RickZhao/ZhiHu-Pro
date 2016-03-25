using System;
using System.Threading.Tasks;

using Zhihu.Common.Model.Result;


namespace Zhihu.Common.Service
{
    public interface ILogin
    {
        Task<LoginResult> LoginAsync(String email, String password);

        Task<OperationResult> RegisterAsync(String email, String password, String lastName, String firstName,
            Int32 gender, String clientId);
    }
}
