using Ecom.Application.Abstractions.Persistence;
using Ecom.Application.Models.Responses.User;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Shared.Common;

namespace Ecom.Application.UseCases.Implementations
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;

        public UserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Asynchronously retrieves user information for the specified user identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose information is to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see
        /// cref="ApiResponse{GetUserInfoResponse}"/> with the user's information if found; otherwise, an appropriate
        /// error response.</returns>
        public async Task<ApiResponse<GetUserInfoResponse>> GetByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return ApiResponse<GetUserInfoResponse>.Fail("User not found!");
            }

            if (user.IsDeleted || !user.IsActive)
            {
                return ApiResponse<GetUserInfoResponse>.Fail("User not found!");
            }

            var result = new GetUserInfoResponse
            {
                Id = user.Id,
                FullName = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Dob = user.Dob,
                AvatarUrl = user.AvatarUrl,
                RoleName = user.Role.ToString(),
                Addresses = user.Addresses,
                CartId = user.CartId
            };

            return ApiResponse<GetUserInfoResponse>.Ok(result, "Get user info successfully!");

        }
    }
}
