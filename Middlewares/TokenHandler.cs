

namespace SSO_Backend.Middlewares;

public class TokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TokenHandler> _logger;

    public TokenHandler(IHttpContextAccessor httpContextAccessor, ILogger<TokenHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {

        var token = ExtractToken();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private string ExtractToken()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            return httpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", "");
        }
        return string.Empty;
    }
}
