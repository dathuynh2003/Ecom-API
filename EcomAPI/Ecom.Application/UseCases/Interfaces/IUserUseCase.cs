using Ecom.Application.Models.Responses.User;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Interfaces
{
    public interface IUserUseCase
    {
        Task<ApiResponse<GetUserInfoResponse>> GetByIdAsync(Guid userId);
    }
}
