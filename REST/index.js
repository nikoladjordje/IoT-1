const grpc = require('@grpc/grpc-js');
const express = require('express');
const protoLoader = require('@grpc/proto-loader');
const swaggerUi = require('swagger-ui-express');
const swaggerJsdoc = require('swagger-jsdoc');
const YAML = require('yamljs');
const { loadSync, loadPackageDefinition } = require('@grpc/proto-loader');

const app = express();
const PORT = 3000;

const packageDefinition = loadSync(__dirname + '/Protos/microservice.proto');

const protoDescriptor = grpc.loadPackageDefinition(packageDefinition);

const myService = protoDescriptor.microservice.Beehive;

const client = new myService('localhost:5240', grpc.credentials.createInsecure());

const swaggerDocument = YAML.load('./openAPI.yaml');

app.use(express.json());

app.get('/GetBeehiveValueById/:id', (req, res) => {
    const request = { _id: req.params.id };
    client.GetBeehiveValueById(request, (error, response) => {
        if (error) {
            if (error.code === grpc.status.INVALID_ARGUMENT) {
                res.status(400).json({ error: 'Invalid ID format' });
            } else if (error.code === grpc.status.NOT_FOUND) {
                res.status(404).json({ error: 'Beehive not found' });
            } else {
                res.status(500).json({ error: 'Internal Server Error' });
            }
            return;
        }
        res.json(response);
    });
});

app.post('/addBeehive', (req, res) => {

    const request = {
        Device: req.body.Device,
        HiveNumber: req.body.HiveNumber,
        Date: req.body.Date,
        HiveTemp: req.body.HiveTemp,
        HiveHumidity: req.body.HiveHumidity,
        HivePressure: req.body.HivePressure,
        WeatherTemp: req.body.WeatherTemp,
        WeatherHumidity: req.body.WeatherHumidity,
        WeatherPressure: req.body.WeatherPressure,
        WindSpeed: req.body.WindSpeed,
        WeatherID: req.body.WeatherID,
        CloudCoverage: req.body.CloudCoverage,
        Rain: req.body.Rain,
        Lat: req.body.Lat,
        Lon: req.body.Lon,
        FileName: req.body.FileName,
        QueenPresence: req.body.QueenPresence,
        QueenAcceptance: req.body.QueenAcceptance,
        Frames: req.body.Frames,
        Target: req.body.Target,
        Time: req.body.Time,
        QueenStatus: req.body.QueenStatus
    };

    client.AddBeehiveValue(request, (error, response) => {
        if (error) {
            res.status(500).json({ error: 'Internal Server Error' });
            return;
        }
        res.json(response);
    });
});

app.delete('/deleteBeehive/:id', (req, res) => {
    const id = req.params.id;
    const request = { _id: id };
    client.DeleteBeehiveValueById(request, (error, response) => {
        if (error) {
            if (error.code === grpc.status.INVALID_ARGUMENT) {
                res.status(400).json({ error: 'Invalid ID format' });
            } else if (error.code === grpc.status.NOT_FOUND) {
                res.status(404).json({ error: 'Beehive not found' });
            } else {
                res.status(500).json({ error: 'Internal Server Error' });
            }
            return;
        }

        res.json(response);
    });
});

app.put('/updateBeehive', (req, res) => {
    const request = {
        _id: req.body.id,
        Device: req.body.Device,
        HiveNumber: req.body.HiveNumber,
        Date: req.body.Date,
        HiveTemp: req.body.HiveTemp,
        HiveHumidity: req.body.HiveHumidity,
        HivePressure: req.body.HivePressure,
        WeatherTemp: req.body.WeatherTemp,
        WeatherHumidity: req.body.WeatherHumidity,
        WeatherPressure: req.body.WeatherPressure,
        WindSpeed: req.body.WindSpeed,
        WeatherID: req.body.WeatherID,
        CloudCoverage: req.body.CloudCoverage,
        Rain: req.body.Rain,
        Lat: req.body.Lat,
        Lon: req.body.Lon,
        FileName: req.body.FileName,
        QueenPresence: req.body.QueenPresence,
        QueenAcceptance: req.body.QueenAcceptance,
        Frames: req.body.Frames,
        Target: req.body.Target,
        Time: req.body.Time,
        QueenStatus: req.body.QueenStatus
    };

    client.UpdateBeehiveValueById(request, (error, response) => {
        if (error) {
            if (error.code === grpc.status.INVALID_ARGUMENT) {
                res.status(400).json({ error: 'Invalid ID format' });
            } else if (error.code === grpc.status.NOT_FOUND) {
                res.status(404).json({ error: 'Beehive not found' });
            } else {
                res.status(500).json({ error: 'Internal Server Error' });
            }
            return;
        }

        res.json(response);
    });
});

app.use('/api-docs', swaggerUi.serve, swaggerUi.setup(swaggerDocument));

app.listen(PORT, () => {
    console.log(`Server is running on port ${PORT}`);
});

