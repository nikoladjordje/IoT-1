import { connect } from 'mqtt';
import { MongoClient } from 'mongodb';

const mqttClient = connect('mqtt://host.docker.internal:1883');
const mongoUrl = 'mongodb://host.docker.internal:27017';
const dbName = 'Beehive'; 
const collectionName = 'Beehive';


async function readAndPublishData() {
  const client = new MongoClient(mongoUrl);
  
  try {
    await client.connect();
    console.log("Connected to MongoDB");
    
    const db = client.db(dbName);
    const collection = db.collection(collectionName);
    
    // const data = await collection.findOne();
    const data = await collection.aggregate([{ $sample: { size: 1 } }]).next();
    
    if (data) {
      mqttClient.publish('sensor-data', JSON.stringify(data));
      console.log('Data sent:', data);
    }
  } catch (err) {
    console.error(err);
  } finally {
    await client.close();
  }
}

mqttClient.on('connect', () => {
  setInterval(readAndPublishData, 5000);
});
