using Amazon.DynamoDBv2.DataModel;

namespace GloboClimaAPI.Models
{
    [DynamoDBTable("user")]
    public class User
    {
        [DynamoDBHashKey("id")]
        public string? Id { get; set; }

        [DynamoDBProperty("login")]
        public string? Login { get; set; }

        [DynamoDBProperty("password")]
        public string? Password { get; set; }
    }
}
