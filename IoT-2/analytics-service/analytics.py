import paho.mqtt.client as mqtt
import json
from datetime import datetime

# Postavke MQTT brokera
MQTT_BROKER_HOST = "host.docker.internal"  # Adresa MQTT brokera
MQTT_BROKER_PORT = 1883  # Port MQTT brokera
TOPIC_SUBSCRIBE = "sensor-data"  # Topic na koji se pretplaćuje za dobijanje podataka
TOPIC_PUBLISH = "analytics-events"  # Topic na koji šalje rezultate analize

# Funkcija koja se poziva prilikom uspostavljanja konekcije sa MQTT brokerom
def on_connect(client, userdata, flags, rc, properties=None):
    print("Connected to MQTT broker with result code "+str(rc))
    # Pretplata na odgovarajući topic za dobijanje podataka
    client.subscribe(TOPIC_SUBSCRIBE)

# Funkcija koja se poziva prilikom primanja poruke sa MQTT brokera
def on_message(client, userdata, message):
    try:
        # Pretvaranje JSON podataka u Python dictionary
        data = json.loads(message.payload.decode())
        
        # Debugging poruka
        print("Received message:", data)
        
        # Detekcija anomalija
        anomalies = detect_anomalies(data)
        if anomalies:
            # Debugging poruka
            print("Anomalies detected:", anomalies)
            
            # Slanje rezultata analize na novi topic
            analysis_result = {
                'event_type': 'AnomalyDetected',
                'anomalies': anomalies,
                'data': data,
                'timestamp': datetime.now().isoformat()
            }
            client.publish(TOPIC_PUBLISH, json.dumps(analysis_result))
            # Debugging poruka
            print("Analysis result published:", analysis_result)
        else:
            analysis_result = {
                'event_type': 'Regular',
                'data': data,
                'timestamp': datetime.now().isoformat()
            }
            client.publish(TOPIC_PUBLISH, json.dumps(analysis_result))
            # Debugging poruka
            print("Analysis result published:", analysis_result)
    except Exception as e:
        print("Error processing message:", e)


def detect_anomalies(data):
    anomalies = []
    try:
        # Check for high hive temperature
        if 'hive temp' in data and float(data['hive temp']) > 35:
            anomalies.append('HighHiveTemperature')
        
        # Check for low hive humidity
        if 'hive humidity' in data and float(data['hive humidity']) < 30:
            anomalies.append('LowHiveHumidity')
        
        # Check for high hive humidity
        if 'hive humidity' in data and float(data['hive humidity']) > 70:
            anomalies.append('HighHiveHumidity')

        # Check for extreme weather temperature
        if 'weather temp' in data:
            if float(data['weather temp']) < 5 or float(data['weather temp']) > 40:
                anomalies.append('ExtremeWeatherTemperature')
        
        # Check for extreme weather humidity
        if 'weather humidity' in data:
            if float(data['weather humidity']) < 20 or float(data['weather humidity']) > 90:
                anomalies.append('ExtremeWeatherHumidity')
        
        # Check for extreme wind speed
        if 'wind speed' in data and float(data['wind speed']) > 15:
            anomalies.append('HighWindSpeed')
        
        # Check for unexpected queen presence or absence
        if 'queen presence' in data:
            if data['queen presence'] not in [0, 1]:
                anomalies.append('UnexpectedQueenPresence')

    except Exception as e:
        print("Error detecting anomalies:", e)
    
    return anomalies

# Kreiranje MQTT klijenta
client = mqtt.Client()

# Postavljanje funkcija za obradu konekcije i poruka
client.on_connect = on_connect
client.on_message = on_message

# Povezivanje sa MQTT brokerom
client.connect(MQTT_BROKER_HOST, MQTT_BROKER_PORT, 60)

# Beskonačna petlja koja osluškuje poruke sa MQTT brokera
client.loop_forever()
