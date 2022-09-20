# README Gruppeoppgave 2 Webapplikasjoner
 Ligger en PDF fil "ReadMeBilder" med bilder av programet
## Gruppemedlemmer
- Lavrans Bjerkestrand S341825
- Ali Ansar S345867
- Bjørnar Engeset Bang S333969

## Brukernavn: admin
## Passord: admin123


## Tekniske kommentarer
Ikke oppdater Newtonsof.Json pakken

## Hvordan kjøre programet
Vi har allerede generert filene så programmet funker som det skal selv om dere ikke gjør dette, bare kjøre Visual Studio prosjektet som vanlig

1. Må ha installert Node Js
2. Åpne terminal
3. Gå til mappen "frontend" 
4. Kjør "npm install", tar litt tid
5. Nå kan du kjøre programet fra Visual Studio, run -> start without debugging

Hvis dere vil generere frontend koden på nytt gjøres det slik:
Kjør "npm run build" etter "4.Kjør "npm install""



## TODO list opp alle API endpoints

### Login
- POST      - /api/login/
- DELETE    - /api/login/

### Company
- GET       - /api/companies/
- GET       - /api/companies/1
- POST      - /api/companies/   Restricted
- PUT       - /api/companies/   Restricted
- DELETE    - /api/companies/4  Restricted

### Port
- GET       - /api/ports/
- GET       - /api/ports/1
- POST      - /api/ports/       Restricted
- PUT       - /api/ports/       Restricted
- DELETE    - /api/ports/7      Restricted

### Customer
- GET       - /api/customers/   Restricted
- GET       - /api/customers/1  Restricted
- POST      - /api/customers/   Restricted
- PUT       - /api/customers/   Restricted
- DELETE    - /api/customers/   Restricted

### Route
- POST      - /api/routs/
- PUT       - /api/routs/
- DELETE    - /api/routs/7

Customer
- GET       - /api/customers/
- GET       - /api/customers/1
- POST      - /api/customers/
- PUT       - /api/customers/
- DELETE    - /api/customers/

Order
- GET       - /api/orders/
- GET       - /api/orders/1
- POST      - /api/orders/
- PUT       - /api/orders/
- DELETE    - /api/orders/

Route
- GET       - /api/routes/
- GET       - /api/routes/1
- POST      - /api/routes/      Restricted
- PUT       - /api/routes/      Restricted
- DELETE    - /api/routes/      Restricted

### RouteTime
- GET       - /api/routetimes/
- GET       - /api/routetimes/1 
- POST      - /api/routetimes/  Restricted
- PUT       - /api/routetimes/  Restricted
- DELETE    - /api/routetimes/  Restricted

RouteTime
Further testing needed for put/delete
- GET       - /api/routetimes/
- GET       - /api/routetimes/1
- POST      - /api/routetimes/
- PUT       - /api/routetimes/
- DELETE    - /api/routetimes/

User
- POST      - /api/users/
- DELETE    - /api/users/

ZipCode
- GET       - /api/zipcodes/
- GET       - /api/zipcodes/4790
- POST      - /api/zipcodes/
- PUT       - /api/zipcodes/
- DELETE    - /api/zipcodes/
