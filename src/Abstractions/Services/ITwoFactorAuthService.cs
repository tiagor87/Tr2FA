using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TRTwoFactorAuth.Abstractions.Models;

namespace TRTwoFactorAuth.Abstractions.Services;

public interface ITwoFactorAuthService
{
    Task<Stream> GenerateQrCodeAsync(TwoFactorAuthIssuer issuer, TwoFactorAuthLabel label, TwoFactorAuthSecret secret, CancellationToken cancellationToken = default);
    Task ValidateCodeAsync(TwoFactorAuthCode code, TwoFactorAuthSecret secret, TwoFactorVerificationRange? range = null, CancellationToken cancellationToken = default);
    Task<TwoFactorAuthCode> GetNextCodeAsync(TwoFactorAuthSecret secret, CancellationToken cancellationToken = default);
}
