using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace Persistence.Externals;

public class AuthTokenServiceProvider( HttpClient httpClient )
{
    private readonly HttpClient _httpClient = httpClient;
    private string _cachedToken = string.Empty;
    private static readonly SemaphoreSlim s_lock = new( 1, 1 );

    public async Task<string> GetTokenAsync( ServiceProviderParams serviceParams )
    {
        if ( !string.IsNullOrEmpty( _cachedToken ) )
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken( _cachedToken );
            var expiryTimeText = jwt.Claims.Single( claim => claim.Type == "exp" ).Value;
            var expiryDateTime = UnixTimeStampToDateTime( int.Parse( expiryTimeText ) );

            if ( expiryDateTime > DateTime.UtcNow )
            {
                return _cachedToken;
            }
        }

        await s_lock.WaitAsync();
        var response = await _httpClient.PostAsJsonAsync( serviceParams.ServiceUrl, new
        {
            userid = serviceParams.UserID,
            email = serviceParams.UserEmail,
            customClaims = new Dictionary<string, object>
            {
                { "admin", true },
                { "trusted_member", true }
            }
        } );

        var newToken = await response.Content.ReadAsStringAsync();
        _cachedToken = newToken;

        s_lock.Release();
        return newToken;
    }

    private static DateTime UnixTimeStampToDateTime( int unixTimeStamp )
    {
        var dateTime = new DateTime( 1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc );
        dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
        return dateTime;
    }
}

public sealed record ServiceProviderParams
{
    public string? ServiceUrl { get; private set; }
    public string? UserID { get; private set; }
    public string? UserEmail { get; private set; }

    private ServiceProviderParams() { }

    public ServiceProviderParams( string serviceUrl, string userID, string userEmail )
    {
        ServiceUrl = serviceUrl;
        UserID = userID;
        UserEmail = userEmail;
    }
}