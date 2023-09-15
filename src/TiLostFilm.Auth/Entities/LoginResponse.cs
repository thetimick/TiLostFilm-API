using Newtonsoft.Json;

namespace TiLostFilm.Auth.Entities;

public record LoginResponse
{
    [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
    public string? Name { get; set; }
    
    [JsonProperty(PropertyName = "success", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Success { get; set; }
    
    [JsonProperty(PropertyName = "result", NullValueHandling = NullValueHandling.Ignore)]
    public string? Result { get; set; }
};