using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GrpcServer.Models
{
    public class BeehiveModel
    {
        public ObjectId _id { get; set; }
        [BsonElement("device")]
        public int Device { get; set; }
        [BsonElement("hive number")]
        public int HiveNumber { get; set; }
        [BsonElement("date")]
        public String Date { get; set; }
        [BsonElement("hive temp")]
        public double HiveTemp { get; set; }
        [BsonElement("hive humidity")]
        public double HiveHumidity { get; set; }
        [BsonElement("hive pressure")]
        public double HivePressure { get; set; }
        [BsonElement("weather temp")]
        public double WeatherTemp { get; set; }
        [BsonElement("weather humidity")]
        public int WeatherHumidity { get; set; }
        [BsonElement("weather pressure")]
        public int WeatherPressure { get; set; }
        [BsonElement("wind speed")]
        public double WindSpeed { get; set; }
        [BsonElement("weatherID")]
        public int WeatherID { get; set; }
        [BsonElement("cloud coverage")]
        public int CloudCoverage { get; set; }
        [BsonElement("rain")]
        public int Rain { get; set; }
        [BsonElement("lat")]
        public double Lat { get; set; }
        [BsonElement("long")]
        public double Lon { get; set; }
        [BsonElement("file name")]
        public string FileName { get; set; }
        [BsonElement("queen presence")]
        public int QueenPresence { get; set; }
        [BsonElement("queen acceptance")]
        public int QueenAcceptance { get; set; }
        [BsonElement("frames")]
        public int Frames { get; set; }
        [BsonElement("target")]
        public int Target { get; set; }
        [BsonElement("time")]
        public double Time { get; set; }
        [BsonElement("queen status")]
        public int QueenStatus { get; set; }
    }
}
