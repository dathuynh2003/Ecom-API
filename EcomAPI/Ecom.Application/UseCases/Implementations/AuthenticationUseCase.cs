using Ecom.Application.Abstractions.Auth;
using Ecom.Application.Abstractions.Mail;
using Ecom.Application.Abstractions.Persistence;
using Ecom.Application.Models.Requests.Auth;
using Ecom.Application.Models.Responses.Auth;
using Ecom.Application.UseCases.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Domain.Enums;
using Ecom.Shared.Common;
using Ecom.Shared.Utils;

namespace Ecom.Application.UseCases.Implementations
{
    public class AuthenticationUseCase : IAuthenticationUseCase
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IUserTokenRepository _userTokenRepo;

        public AuthenticationUseCase(IUserRepository userRepo, IJwtService jwtService, IEmailService emailService, IUserTokenRepository userToken)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _emailService = emailService;
            _userTokenRepo = userToken;
        }

        /// <summary>
        /// Requests a new access token using the specified token request parameters.
        /// </summary>
        /// <param name="request">The parameters required to request a new access token. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an ApiResponse with the token
        /// response data if the request is successful.</returns>
        public async Task<ApiResponse<TokenResponse>> GetNewAccessToken(TokenRequest request)
        {
            var isValid = await _jwtService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (isValid == false)
            {
                return ApiResponse<TokenResponse>.Fail("Invalid refresh token!");
            }

            var user = await _userRepo.GetByIdAsync(request.UserId);
            if (user == null)
                return ApiResponse<TokenResponse>.Fail("User not found.");

            var reponseToken = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.ToString()),
                RefreshToken = request.RefreshToken
            };
            return ApiResponse<TokenResponse>.Ok(reponseToken, "New access token generated successfully.");
        }

        /// <summary>
        /// Authenticates a user asynchronously using the specified login request and returns a token response.
        /// </summary>
        /// <param name="request">The login request containing user credentials and any additional authentication parameters. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an ApiResponse with the token
        /// response if authentication is successful.</returns>
        public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
        {
            // Validate the login request parameters
            if (request is null)
                return ApiResponse<TokenResponse>.Fail("Login request cannot be null.");
            if (string.IsNullOrEmpty(request.Account) || string.IsNullOrEmpty(request.Password))
                return ApiResponse<TokenResponse>.Fail("Account and password must be provided.");

            var user = await _userRepo.LoginAsync(request.Account, request.Password);
            if (user == null)
                return ApiResponse<TokenResponse>.Fail("Invalid account or password.");
            if (user.IsActive == false)
                return ApiResponse<TokenResponse>.Fail("Email not verified.");
            var tokenResponse = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(user.Id, user.Role.ToString()),
                RefreshToken = await _jwtService.GenerateRefreshToken(user.Id)
            };
            return ApiResponse<TokenResponse>.Ok(tokenResponse, "Login successful.");
        }

        /// <summary>
        /// Logs out the specified user by revoking their refresh token asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to log out.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an ApiResponse indicating the
        /// outcome of the logout operation. If successful, the response contains a success message; otherwise, it
        /// contains an error message.</returns>
        public async Task<ApiResponse<string>> LogoutAsync(Guid userId)
        {
            var result = await _jwtService.RevokeRefreshTokenAsync(userId);
            if (result == false)
                return ApiResponse<string>.Fail("Logout failed or user already logged out");
            return ApiResponse<string>.Ok(null, "Logout successful");
        }

        /// <summary>
        /// Registers a new user account asynchronously using the provided registration details.
        /// </summary>
        /// <remarks>An email verification link is sent to the user's email address upon successful
        /// registration. The link is valid for 5 minutes. The method does not create the user if the email is already
        /// registered or if the password and confirm password do not match.</remarks>
        /// <param name="request">The registration information for the new user, including name, email, phone number, date of birth, password,
        /// and confirm password. The password and confirm password must match. The email must not already be
        /// registered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an ApiResponse with registration
        /// outcome details. If registration is successful, the response includes the user's email and indicates that a
        /// confirmation email has been sent. If registration fails, the response contains an error message.</returns>
        public async Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest request)
        {
            // Validate phone number
            if (await _userRepo.IsPhoneInUseByAnotherUserAsync(request.Email, request.PhoneNumber))
            {
                return ApiResponse<RegisterResponse>.Fail("Phone number already in use");
            }
            // Validate password
            if (request.Password.Equals(request.ConfirmPassword) == false)
                return ApiResponse<RegisterResponse>.Fail("Password and confirm password do not match");
            // Check existing user
            var existing = await _userRepo.GetByEmailAsync(request.Email);
            if (existing is not null)
            {
                if (existing.IsActive)
                {
                    // Case 1: Email already registered and verified
                    return ApiResponse<RegisterResponse>.Fail("Email already registered");
                }
                // Case 2: Existing but not verfied
                await HandleExistingInactiveUserAsync(existing, request);
            }
            else
            {
                // Case 3: New user registration
                existing = await HandleNewUserRegistrationAsync(request);
            }

            // Generate email verification token
            var tokenValue = TokenUtils.GenerateToken();
            var emailToken = UserToken.Create(existing.Id, tokenValue, DateTime.UtcNow.AddMinutes(5), UserTokenType.EmailVerification);
            await _userTokenRepo.AddAsync(emailToken);
            await _userTokenRepo.SaveChangesAsync();

            await SendVerificationEmailAsync(existing, tokenValue);

            return ApiResponse<RegisterResponse>.Ok(new RegisterResponse
            {
                Email = existing.Email,
                IsConfirmEmailSent = "true"
            }, "Registration successful. Please check your email to verify your account.");
        }

        /// <summary>
        /// Resends a verification email to the specified user email address, generating a new verification token.
        /// </summary>
        /// <remarks>The verification email includes a new token valid for a limited time. This method
        /// should be used when a user requests a new verification email, such as after failing to receive the
        /// original.</remarks>
        /// <param name="email">The email address of the user to whom the verification email will be resent. Cannot be null or empty.</param>
        /// <returns>An ApiResponse containing a status message indicating the result of the resend operation. The response data
        /// is null.</returns>
        public async Task<ApiResponse<string>> ResendVerificationEmailAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);
            if (user is null)
                return ApiResponse<string>.Fail("User not found");
            if (user.IsActive)
                return ApiResponse<string>.Ok(null, "Account is already verified");

            var newToken = TokenUtils.GenerateToken();

            var existingToken = await _userTokenRepo.GetByUserAsync(user.Id);
            if (existingToken is not null)
            {
                existingToken.Refresh(newToken, DateTime.UtcNow.AddMinutes(5));
                await _userTokenRepo.UpdateAsync(existingToken);
            }
            else
            {
                var emailToken = UserToken.Create(user.Id, newToken, DateTime.UtcNow.AddMinutes(5), UserTokenType.EmailVerification);
                await _userTokenRepo.AddAsync(emailToken);
            }
            await _userTokenRepo.SaveChangesAsync();
            await SendVerificationEmailAsync(user, newToken);
            return ApiResponse<string>.Ok(null, "Verification email resent successfully.");
        }

        /// <summary>
        /// Verifies a user's email address using the provided verification token asynchronously.
        /// </summary>
        /// <remarks>The method returns an error response if the token is invalid, expired, or the
        /// associated user cannot be found. Upon successful verification, the user's account is activated and the token
        /// is deleted.</remarks>
        /// <param name="token">The email verification token to validate. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an ApiResponse with a success
        /// message if the email is verified; otherwise, an error message indicating the reason for failure.</returns>
        public async Task<ApiResponse<string>> VerifyEmailAsync(string token)
        {
            var emailToken = await _userTokenRepo.GetByToken(token);
            if (emailToken == null)
            {
                return ApiResponse<string>.Fail("Invalid token.");
            }
            if (emailToken.Type != UserTokenType.EmailVerification)
            {
                return ApiResponse<string>.Fail("Invalid token.");
            }
            var user = await _userRepo.GetByIdAsync(emailToken.UserId);
            if (user == null)
            {
                return ApiResponse<string>.Fail("User not found.");
            }
            if (user.IsActive)
            {
                return ApiResponse<string>.Ok(null, "Email already verified.");
            }
            if (emailToken.IsUsed || emailToken.ExpiresAt < DateTime.UtcNow)
            {
                return ApiResponse<string>.Fail("Expired token.");
            }
            user.Activate();
            emailToken.MarkAsUsed();
            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChangesAsync();
            // Optionally, you can delete the token after successful verification
            //await _userTokenRepo.HardDeleteAsync(emailToken);
            await _userTokenRepo.UpdateAsync(emailToken);
            await _userTokenRepo.SaveChangesAsync();
            return ApiResponse<string>.Ok(null, "Email verified successfully.");
        }

        // ==========================================================
        private async Task SendVerificationEmailAsync(User user, string token)
        {
            var verifyLink = $"http://localhost:3101/verify-email?token={token}";
            //var verifyLink = $"{_appSettings.FrontendBaseUrl}/verify-email?token={token}";
            var subject = "Verify your email";
            var body = EmailUtils.GenerateVerificationEmailBody(user.Name, verifyLink);
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        private async Task<User> HandleExistingInactiveUserAsync(User existingUser, RegisterRequest reuqest)
        {
            // Update existing user info
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(reuqest.Password);
            existingUser.UpdateInfo(reuqest.Name, reuqest.PhoneNumber, reuqest.Dob, hashedPassword, existingUser.AvatarUrl);
            await _userRepo.UpdateAsync(existingUser);
            await _userRepo.SaveChangesAsync();

            return existingUser;
        }
        private async Task<User> HandleNewUserRegistrationAsync(RegisterRequest request)
        {
            var hasdedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = User.Create(request.Name, request.Email, request.PhoneNumber, request.Dob, hasdedPassword, Domain.Enums.UserRole.Customer);
            var cart = Cart.CreateForUser(user.Id);
            user.Cart = cart;
            user.CartId = cart.Id;
            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();
            return user;
        }

        public async Task<ApiResponse<string>> ForgotPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return ApiResponse<string>.Fail("Email is required");

            var user = await _userRepo.GetByEmailAsync(email);

            // Always return success response to prevent email enumeration, but only send email if user exists and is active
            if (user is not null && user.IsActive)
            {
                await CreatePasswordResetTokenAndSendEmailAsync(user);
            }
            return ApiResponse<string>.Ok(null, "Password reset link has been sent");

        }

        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            if (request is null)
                return ApiResponse<string>.Fail("Request cannot be null");

            if (string.IsNullOrWhiteSpace(request.Token))
                return ApiResponse<string>.Fail("Invalid token.");

            if (string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.ConfirmPassword))
                return ApiResponse<string>.Fail("Password and confirm password are required.");

            if (!request.NewPassword.Equals(request.ConfirmPassword))
                return ApiResponse<string>.Fail("Password and confirm password do not match.");

            var resetToken = await _userTokenRepo.GetValidTokenByToken(request.Token);
            if (resetToken is null || resetToken.Type != UserTokenType.PasswordReset)
                return ApiResponse<string>.Fail("Invalid token.");

            var user = await _userRepo.GetByIdAsync(resetToken.UserId);
            if (user is null)
                return ApiResponse<string>.Fail("User not found.");
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.ChangePassword(hashedPassword);
            resetToken.MarkAsUsed();
            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveChangesAsync();
            await _userTokenRepo.UpdateAsync(resetToken);
            await _userTokenRepo.SaveChangesAsync();

            await _jwtService.RevokeRefreshTokenAsync(user.Id);

            return ApiResponse<string>.Ok(null, "Password reset successful.");
        }

        private async Task CreatePasswordResetTokenAndSendEmailAsync(User user)
        {
            var tokenValue = TokenUtils.GenerateToken();
            var passwordResetToken = UserToken.Create(user.Id, tokenValue, DateTime.UtcNow.AddMinutes(15), UserTokenType.PasswordReset);
            await _userTokenRepo.AddAsync(passwordResetToken);
            await _userTokenRepo.SaveChangesAsync();
            var resetLink = $"http://localhost:3101/reset-password?token={tokenValue}";
            //var resetLink = $"{_appSettings.FrontendBaseUrl}/reset-password?token={tokenValue}";
            var subject = "Password Reset Request";
            var body = EmailUtils.GeneratePasswordResetEmailBody(user.Name, resetLink);
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
    }
}
