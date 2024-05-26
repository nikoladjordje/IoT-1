# Koristimo osnovnu Python sliku
FROM python:3.8-slim

# Postavljamo radni direktorijum
WORKDIR /app

# Kopiramo requirements fajl i instaliramo zavisnosti
COPY requirements.txt ./
RUN pip install --no-cache-dir -r requirements.txt

# Kopiramo sve fajlove iz lokalnog direktorijuma u radni direktorijum u kontejneru
COPY . .

# Pokrećemo skriptu
CMD ["python", "analytics.py"]
