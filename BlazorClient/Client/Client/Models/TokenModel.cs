using System.Text.Json.Serialization;

namespace Client.Models;

public class TokenModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int LifeTime { get; set; }
    
    [JsonIgnore]
    public DateTime ReleasedAt { get; set; }

    [JsonIgnore]
    public DateTime ExpiredAt => ReleasedAt.AddSeconds(LifeTime);
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}