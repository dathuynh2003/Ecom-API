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
    }
}
