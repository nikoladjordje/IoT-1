using Grpc.Core;
using GrpcClientIoT;
using GrpcServer.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrpcClientIoT.Services
{
    public class Microservices : Beehive.BeehiveBase
    {
        private readonly IMongoCollection<BeehiveModel> _collection;
        public Microservices(IMongoDatabase database)
        {
            _collection = database.GetCollection<BeehiveModel>("Beehive");
        }

        public override async Task<ValueMessage> GetBeehiveValueById(BeehiveId request, ServerCallContext context)
        {
            var Id = request.Id;

            Console.WriteLine(Id);

            ObjectId objectId;
            try
            {
                objectId = ObjectId.Parse(Id);
            }
            catch (FormatException)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ObjectId format"));
            }

            var filter = Builders<BeehiveModel>.Filter.Eq(x => x._id, objectId);

            var beehive = await _collection.Find(filter).FirstOrDefaultAsync();

            if (beehive == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Beehive value not found"));
            }

            return await Task.FromResult(new ValueMessage
            {
                Id = beehive._id.ToString(),
                BeehiveValue = new BeehiveValue
                {
                    Device = beehive.Device,
                    HiveNumber = beehive.HiveNumber,
                    Date = beehive.Date.ToString(),
                    HiveTemp = beehive.HiveTemp,
                    HiveHumidity = beehive.HiveHumidity,
                    HivePressure = beehive.HivePressure,
                    WeatherTemp = beehive.WeatherTemp,
                    WeatherHumidity = beehive.WeatherHumidity,
                    WeatherPressure = beehive.WeatherPressure,
                    WindSpeed = beehive.WindSpeed,
                    WeatherID = beehive.WeatherID,
                    CloudCoverage = beehive.CloudCoverage,
                    Rain = beehive.Rain,
                    Lat = beehive.Lat,
                    Lon = beehive.Lon,
                    FileName = beehive.FileName,
                    QueenPresence = beehive.QueenPresence,
                    QueenAcceptance = beehive.QueenAcceptance,
                    Frames = beehive.Frames,
                    Target = beehive.Target,
                    Time = beehive.Time,
                    QueenStatus = beehive.QueenStatus
                },
                Message = "Beehive value found"
            });
        }

        public override async Task<ValueMessage> AddBeehiveValue(BeehiveValue request, ServerCallContext contextd)
        {
            var objectId = ObjectId.GenerateNewId();

            var filter = Builders<BeehiveModel>.Filter.Eq(x => x._id, objectId);

            var beehiveValue = await _collection.Find(filter).FirstOrDefaultAsync();

            if (beehiveValue != null)
            {
                return await Task.FromResult(new ValueMessage
                {
                    Id = beehiveValue._id.ToString(),
                    Message = "There is a beehive value with the same id in database"
                });
            }

            var newValue = new BeehiveModel
            {
                _id = objectId,
                Device = request.Device,
                HiveNumber = request.HiveNumber,
                Date = DateTime.Now,
                HiveTemp = request.HiveTemp,
                HiveHumidity = request.HiveHumidity,
                HivePressure = request.HivePressure,
                WeatherTemp = request.WeatherTemp,
                WeatherHumidity = request.WeatherHumidity,
                WeatherPressure = request.WeatherPressure,
                WindSpeed = request.WindSpeed,
                WeatherID = request.WeatherID,
                CloudCoverage = request.CloudCoverage,
                Rain = request.Rain,
                Lat = request.Lat,
                Lon = request.Lon,
                FileName = request.FileName,
                QueenPresence = request.QueenPresence,
                QueenAcceptance = request.QueenAcceptance,
                Frames = request.Frames,
                Target = request.Target,
                Time = request.Time,
                QueenStatus = request.QueenStatus
            };
            await _collection.InsertOneAsync(newValue);

            return await Task.FromResult(new ValueMessage
            {
                Id = newValue._id.ToString(),
                Message = "Beehive value added successfully"
            });
        }

        public override async Task<ValueMessage> DeleteBeehiveValueById(BeehiveId request, ServerCallContext context)
        {
            var Id = request.Id;
            ObjectId objectId;
            try
            {
                objectId = ObjectId.Parse(Id);
            }
            catch (FormatException)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ObjectId format"));
            }

            var filter = Builders<BeehiveModel>.Filter.Eq(x => x._id, objectId);

            var beehiveValue = await _collection.FindOneAndDeleteAsync(filter);

            if (beehiveValue == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Beehive value with the requested Id not found"));
            }

            return await Task.FromResult(new ValueMessage
            {
                Id = objectId.ToString(),
                Message = "Beehive value with the requested Id has been successfully deleted"
            });
        }

        public override async Task<ValueMessage> UpdateBeehiveValueById(BeehiveValue request, ServerCallContext context)
        {
            var Id = request.Id;
            ObjectId objectId;
            try
            {
                objectId = ObjectId.Parse(Id);
            }
            catch (FormatException)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid ObjectId format"));
            }

            var filter = Builders<BeehiveModel>.Filter.Eq(x => x._id, objectId);

            var beehiveValue = await _collection.Find(filter).FirstOrDefaultAsync();

            if (beehiveValue == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "There is no beehive value with the same Id in database"));
            }

            beehiveValue.Device = request.Device;
            beehiveValue.HiveNumber = request.HiveNumber;
            beehiveValue.Date = DateTime.Now;
            beehiveValue.HiveTemp = request.HiveTemp;
            beehiveValue.HiveHumidity = request.HiveHumidity;
            beehiveValue.HivePressure = request.HivePressure;
            beehiveValue.WeatherTemp = request.WeatherTemp;
            beehiveValue.WeatherHumidity = request.WeatherHumidity;
            beehiveValue.WeatherPressure = request.WeatherPressure;
            beehiveValue.WindSpeed = request.WindSpeed;
            beehiveValue.WeatherID = request.WeatherID;
            beehiveValue.CloudCoverage = request.CloudCoverage;
            beehiveValue.Rain = request.Rain;
            beehiveValue.Lat = request.Lat;
            beehiveValue.Lon = request.Lon;
            beehiveValue.FileName = request.FileName;
            beehiveValue.QueenPresence = request.QueenPresence;
            beehiveValue.QueenAcceptance = request.QueenAcceptance;
            beehiveValue.Frames = request.Frames;
            beehiveValue.Target = request.Target;
            beehiveValue.Time = request.Time;
            beehiveValue.QueenStatus = request.QueenStatus;

            await _collection.ReplaceOneAsync(filter, beehiveValue);

            return new ValueMessage
            {
                Id = objectId.ToString(),
                Message = "Beehive value with the requested ID has been successfully updated"
            };
        }
    }
}
