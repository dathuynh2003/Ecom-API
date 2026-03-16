using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Shared.Utils
{
    public class EmailUtils
    {
        public static string GenerateVerificationEmailBody(string name, string verificationLink)
        {
            return $@"
                <html>
                <body>
                    <h2>Email Verification</h2>
                    <p>Hi {name},</p>
                    <p>Thank you for registering! Please click the link below to verify your email address:</p>
                    <a href='{verificationLink}' style='display:inline-block;padding:10px 20px;font-size:16px;color:#fff;background-color:#007bff;text-decoration:none;border-radius:5px;'>Verify Email</a>
                    <p>If you did not create an account, please ignore this email.</p>
                </body>
                </html>";
        }

        public static string GeneratePasswordResetEmailBody(string name, string resetLink)
        {
            return $@"
                <html>
                <body>
                    <h2>Password Reset Request</h2>
                    <p>Hi {name},</p>
                    <p>We received a request to reset your password. Please click the link below to set a new password:</p>
                    <a href='{resetLink}' style='display:inline-block;padding:10px 20px;font-size:16px;color:#fff;background-color:#dc3545;text-decoration:none;border-radius:5px;'>Reset Password</a>
                    <p>If you did not request a password reset, please ignore this email.</p>
                </body>
                </html>";
        }
    }
}
