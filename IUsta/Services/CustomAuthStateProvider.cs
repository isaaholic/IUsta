using Blazored.LocalStorage;
using System.Security.Claims;
using System.Text.Json;

namespace IUsta.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJDdXN0b21lciIsInJvbGUiOiJDdXN0b21lciIsImlhdCI6MTYyNjE0NjY2MCwiZXhwIjoxNjI2MTQ2NzYwfQ.nVXzN_rB1D97vq8HGB4qLPH5Wy4VrZ5i4oPXG-9It0LJOk6gbgNkGzFp5b-LIjxdHhOYWeTrs8B0qEDyDvtZmw";       
        string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJMZXlsYSIsIm5hbWUiOiJMZXlsYSIsImlhdCI6MTYyNjE0NjY2MCwiZXhwIjoxNjI2MTQ2NzYwfQ.dgZWhYHcJUv2as2TAL16fX9DfgaaR6HX0-3KjX8toSP3Nk-Qtaoj-F6_eWYIgT8GqIaWt20z1UowmugavAnrmA";

        var identity = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(token))
            identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }


    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
