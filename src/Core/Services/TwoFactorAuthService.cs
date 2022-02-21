﻿using OtpNet;
using TRTwoFactorAuth.Abstractions.Models;
using TRTwoFactorAuth.Abstractions.Services;
using TRTwoFactorAuth.Abstractions.Utilities;
using TRTwoFactorAuth.Core.Exceptions;

namespace TRTwoFactorAuth.Core.Services;

public class TwoFactorAuthService : ITwoFactorAuthService
{
    private readonly IQrCodeGenerator _qrCodeGenerator;

    public TwoFactorAuthService(IQrCodeGenerator qrCodeGenerator)
    {
        _qrCodeGenerator = qrCodeGenerator;
    }

    public async Task<Stream> GenerateQrCodeAsync(TwoFactorAuthIssuer twoFactorAuthIssuer, TwoFactorAuthLabel twoFactorAuthLabel, TwoFactorAuthSecret twoFactorAuthSecret, CancellationToken cancellationToken = default)
    {
        var twoFactorAuth = new TwoFactorAuth(TwoFactorAuthType.TimeBasedOneTimePassword, twoFactorAuthIssuer, twoFactorAuthLabel, twoFactorAuthSecret);
        var uri = twoFactorAuth.GenerateUri();
        var stream = await _qrCodeGenerator.GenerateAsync(uri, cancellationToken);
        return stream;
    }

    public Task ValidateCodeAsync(TwoFactorAuthCode code, TwoFactorAuthSecret twoFactorAuthSecret, TwoFactorVerificationRange? range = null, CancellationToken cancellationToken = default)
    {
        range ??= TwoFactorVerificationRange.Default;
        var totp = new Totp(Base32Encoding.ToBytes(twoFactorAuthSecret.Value));
        var isValid = totp.VerifyTotp(code.Value, out _, new VerificationWindow(Math.Abs(range.Start), Math.Abs(range.End)));
        if (!isValid)
        {
            throw new TwoFactorAuthCodeInvalidException($"The code {code} is invalid");
        }
        return Task.CompletedTask;
    }

    public Task<TwoFactorAuthCode> GetNextCodeAsync(TwoFactorAuthSecret twoFactorAuthSecret, CancellationToken cancellationToken = default)
    {
        var totp = new Totp(Base32Encoding.ToBytes(twoFactorAuthSecret.Value));
        return Task.FromResult(new TwoFactorAuthCode(totp.ComputeTotp()));
    }
}