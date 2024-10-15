using System.Text.Json.Serialization;

namespace GloboClimaAPI.Models
{
    public class Country
    {
        [JsonPropertyName("name")]
        public Name? Name { get; set; }

        [JsonPropertyName("cca2")]
        public string? Cca2 { get; set; }

        [JsonPropertyName("currencies")]
        public Dictionary<string, Currency>? Currencies { get; set; }

        [JsonPropertyName("capital")]
        public List<string>? Capital { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }

        [JsonPropertyName("subregion")]
        public string? Subregion { get; set; }

        [JsonPropertyName("languages")]
        public Dictionary<string, string>? Languages { get; set; }

        [JsonPropertyName("translations")]
        public Translations? Translations { get; set; }

        [JsonPropertyName("latlng")]
        public List<double>? Latlng { get; set; }

        [JsonPropertyName("area")]
        public double Area { get; set; }

        [JsonPropertyName("population")]
        public long Population { get; set; }

        [JsonPropertyName("flags")]
        public Flags? Flags { get; set; }
    }

    public class Name
    {
        [JsonPropertyName("common")]
        public string? Common { get; set; }

        [JsonPropertyName("official")]
        public string? Official { get; set; }
    }

    public class Currency
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
    }

    public class Translations
    {
        [JsonPropertyName("por")]
        public Translation? Por { get; set; }
    }

    public class Translation
    {
        [JsonPropertyName("official")]
        public string? Official { get; set; }

        [JsonPropertyName("common")]
        public string? Common { get; set; }
    }

    public class Flags
    {
        [JsonPropertyName("png")]
        public string? Png { get; set; }

        [JsonPropertyName("svg")]
        public string? Svg { get; set; }

        [JsonPropertyName("alt")]
        public string? Alt { get; set; }
    }
}
