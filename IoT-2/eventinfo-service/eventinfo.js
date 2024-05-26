const express = require('express');
const bodyParser = require('body-parser');
const mqtt = require('mqtt');

const app = express();
const port = 3000;
let events = [];

// Middleware
app.use(bodyParser.json());

// MQTT Client
const mqttClient = mqtt.connect('mqtt://host.docker.internal:1883');

mqttClient.on('connect', () => {
  mqttClient.subscribe('analytics-events', (err) => {
    if (!err) {
      console.log('Subscribed to analytics-events');
    } else {
      console.error('Failed to subscribe to analytics-events', err);
    }
  });
});

mqttClient.on('message', (topic, message) => {
  if (topic === 'analytics-events') {
    const event = JSON.parse(message.toString());
    events.push(event);
    console.log('Received event:', event);
  }
});

// REST API Endpoints
app.get('/api/events', (req, res) => {
  res.json(events);
});

app.listen(port, () => {
  console.log(`EventInfo service listening at http://localhost:${port}`);
});
