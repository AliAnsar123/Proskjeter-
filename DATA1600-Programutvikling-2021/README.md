# Eksamen DATA1600 Programutvikling, VÅR 2021
## Innlevering
Besvarelsen skal inneholde et Java kodeprosjekt som kan kjøres fra en Java IDE.

Anbefalt oppsett er et modul-basert Maven Java prosjekt, opprettet og utviklet i Intellij. Eventuelle eksterne biblioteker må legges til i Maven konfigurasjonsfilen, inkludert JavaFX.

Sensor vil også kunne kjøre Java 8 Intellij prosjekteruten Maven. Med dette oppsettet er det anbefalt å ikke bruke eksterne biblioteker utover bibliotek for testing. (Hvis det inkluderes eksterne biblioteker er det en risiko for at sensor ikke får kjørt prosjektet.)
## Tekniske krav og krav til funksjonalitet
Det skal implementeres et registreringssystem for gårdsprodukter. Programmet må inneholde funksjonalitetene beskrevet under. Dere blir utelukkende evaluert basert på disse funksjonalitetene. Det vil si, dere kan gjerne legge til andre funksjonaliteter til programmet, men slik ekstra funksjonalitet blir ikke evaluert.
## Registrering av nye produkter
Produktene skal organiseres i to nivåer: et nivå for ulike typer produktkategorier(som gjødsel, korn, traktorer, og arbeidsklær)og et nivå for konkrete produkter(som «John Deere 5100R traktor» innen produktkategori «traktor»).

Bruker skal ha muligheten til å registrere individuelle elementer for begge disse to nivåene fra det grafiske brukergrensesnittet.

Hvis brukeren taster inn ugyldig data, skal brukeren få beskjed om dette. Det skal dermed ikke være mulig å legge til elementer med ugyldig data.

Det grafiske brukergrensesnittet for å legge til elementer skal være designet slik at det er enkelt for brukeren å forstå hvordan å legge til elementer. Det blir lagt noe vekt på det grafiske designet, men ikke mye. Det vil si, det skal se bra ut, men det forventes ikke noe ekstraordinært her som animasjoner eller andre dynamiske GUI funksjonaliteter.
## Opplisting av eksisterende elementer
De elementene som er lagt til fra bruker og lest inn fra fil skal listes opp i det grafiske brukergrensesnittet. Det skal være mulig for bruker å kunne sortere elementene etter hver datakolonne. I tillegg forventes det at elementer kan filtreres ut ifra filtreringsmuligheter som passer for den spesifikke applikasjonen.

Bruker skal kunne velge et individuelt element, gjøre endringer på elementets data, og slette elementet. Det grafiske grensesnittet skal være designet slik at det kommer tydelig frem hvilket element som er valgt (for eksempel, ved å markere valgt rad med mørkere bakgrunn).

## Filbehandling
Programmet skal støtte filbehandling slik at programmets data ikke går tapt når programmet avsluttes. Det anbefales at besvarelsen allerede inneholder eksempeldata slik at sensor har noe å gå ut ifra.

Lagring av data som representerer produktkategorier skal lagres med binære filer, mens data for konkrete produkter skal lagres med tekstfiler. Tekstfilene skal være kompatible med Excel, slik at produkter kan prosesseres med andre programmer.

Sensor tester programmet ved å injisere ugyldig data i filene, så husk å inkludere håndtering av ugyldig data fra fil.

Metoder som laster inn data fra fil skal gjennomføres i en egen tråd. Legg til noen sekunder med venting slik at tråd-løsningen kan verifiseres av sensor. Det skal ikke være mulig å legge til eller endre på elementer i brukergrensesnittet mens tråden arbeider.