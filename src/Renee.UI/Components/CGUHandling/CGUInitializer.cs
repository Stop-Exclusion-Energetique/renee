using System.Security.Claims;
using Renee.Application.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace Renee.UI.Components.CGUHandling;

public class CguInitializer
{
    private readonly AuthenticationStateProvider _authProvider;
    private readonly ICguValidationService _cguValidationService;
    private readonly CguContext _cguContext = new CguContext();

    private bool _isInitialized;

    public CguContext Context => _cguContext;

    public CguInitializer(
        AuthenticationStateProvider authProvider,
        ICguValidationService cguValidationService)
    {
        _authProvider = authProvider;
        _cguValidationService = cguValidationService;
    }

    public async Task InitializeAsync(Guid userId)
    {
        if (_isInitialized || _cguContext.IsInitialized)
            return;

        _cguContext.UserId = userId;
        var cguVersionResult = await _cguValidationService.InitializeCGUVerificationAsync(userId);
        if (cguVersionResult.IsSuccess)
        {
            _cguContext.UserVersion = cguVersionResult.Value?.LastValidatedCGUVersionByUser;
            _cguContext.LatestVersion = cguVersionResult.Value?.LastCGUVersion;
            _cguContext.LatestVersionLabelFile = cguVersionResult.Value?.LastCGULabel;
        }
        _cguContext.IsInitialized = true;
        _isInitialized = true;
    }

    public async Task MarkCGUAsAcceptedAsync()
    {
        if (_cguContext.UserId == Guid.Empty || _cguContext.LatestVersion == null)
            return;

        await _cguValidationService.MarkCGUAsAcceptedAsync(_cguContext.UserId, _cguContext.LatestVersion);
        _cguContext.UserVersion = _cguContext.LatestVersion;
        _cguContext.IsInitialized = true;
        _isInitialized = true;
    }
}
