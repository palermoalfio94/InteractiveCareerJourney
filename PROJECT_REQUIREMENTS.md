# Interactive Career Journey — requisiti del progetto

## Scopo

Realizzare un portfolio personale interattivo che racconti il percorso di Alfio Palermo come un breve viaggio narrativo in un mondo 2.5D. Il mondo stesso rappresenta il CV: scuole, università, luoghi di lavoro e progetti compaiono lungo una strada e restano visibili dopo la loro introduzione.

Il portfolio deve comunicare competenze e percorso in modo memorabile, mantenendo le informazioni professionali facili da raggiungere per recruiter, docenti, ricercatori e aziende.

## Piattaforma e strumenti

- Motore: Unity **6.3 LTS**.
- Rendering: **Universal Render Pipeline (URP)**.
- Destinazione principale: browser tramite **WebGL Build Support**.
- I moduli di build per Android, iOS e altre piattaforme non sono necessari al prototipo. Windows Build Support sarà installato solo se servirà distribuire anche un eseguibile Windows.
- Stile: 3D leggero con composizione e movimento prevalentemente laterali, per un effetto 2.5D.
- Contenuti narrativi: progettati per essere aggiunti tramite dati, senza incorporare tutta la biografia nella logica della scena.

## Esperienza utente

La modalità iniziale è **Play Journey**: il personaggio avanza automaticamente e la storia si svolge in pochi minuti. Il visitatore può mettere in pausa e riprendere, saltare direttamente all’esplorazione e selezionare luoghi o oggetti importanti.

La modalità **Explore** permette di fermare la narrazione, spostarsi nel mondo e regolare lo zoom. È disponibile tramite il comando di salto e al termine del viaggio. La vista finale deve mostrare l’intero percorso come una mappa/diorama.

Se il visitatore seleziona un punto d’interesse durante la narrazione, il viaggio si mette in pausa. Si apre una scheda con i dettagli e, chiudendola, si può riprendere dal punto raggiunto.

Controlli minimi: pausa/ripresa, esplorazione/salto e audio attivo/disattivo. La velocità di riproduzione è facoltativa. L’esperienza non deve includere prove o sfide obbligatorie.

## Mondo e narrazione

- Il protagonista rappresenta Alfio in diverse fasi della vita con un’identità visiva coerente. Età, abiti, proporzioni e oggetti di scena comunicano la fase; non è richiesta una resa fotorealistica.
- Le tappe vengono mostrate nello spazio: scuola, università, aziende, attività di insegnamento e progetti sono luoghi o oggetti interattivi.
- Le tappe già introdotte rimangono nel mondo. La scena deve poter crescere lungo la strada e consentire una vista d’insieme.
- Attività parallele, come studio e lavoro contemporanei, devono poter essere rappresentate visivamente. Un percorso universitario temporaneamente interrotto resta presente e può riconnettersi alla storia quando riprende.
- Dialoghi e didascalie della narrazione sono brevi. Le informazioni estese compaiono nelle schede interattive.
- Tono professionale e amichevole, con momenti di ironia discreta.

### Traccia narrativa attuale

1. Primo incontro con l’informatica, intorno ai 13 anni.
2. Scuola superiore tecnica.
3. Prime competenze: C++, PHP, HTML, CSS e basi di database.
4. Primi progetti di programmazione.
5. Studi universitari in Informatica e progetti/corsi.
6. Durante gli studi, un ramo parallelo mostra il lavoro da Game Programmer presso eyePlus Tecnowork con Unity e Blender; dopo un anno questa esperienza si chiude.
7. Alfio resta all'università e si laurea con 110/110 e una tesi sulla segmentazione semantica con Deep Learning; seguono crescita professionale e successivo cambio di azienda.
8. Pausa temporanea degli studi e attività di insegnamento.
9. Ritorno all’università per la laurea magistrale in Informatica.
10. Machine learning, deep learning, computer vision e progetti di AI.
11. Progetti di dati sintetici e VQA, ricerca e sviluppi futuri.

Date e contenuti biografici precisi saranno completati in seguito.

## Interazione e schede

I punti d’interesse devono supportare evidenziazione al passaggio del mouse, etichetta sintetica, selezione e apertura della scheda. La selezione può spostare brevemente la camera sul punto e sospendere la narrazione.

Le schede possono contenere titolo, data, descrizione breve, tecnologie, competenze, immagini/video, repository, pubblicazioni, link esterni e CV scaricabile quando pertinente. I link devono essere facili da individuare e aprire.

### Tappa universitaria — prototipo

- L’edificio **UNICT Informatica Triennale** è cliccabile e apre un pannello con l’elenco dei corsi.
- Il prototipo parte con circa 15 corsi e alcuni progetti di esempio, esplicitamente marcati come contenuti dimostrativi da sostituire.
- Selezionando un corso si leggono descrizione e, quando presenti, progetti associati.
- Dialoghi, didascalie, durata della transizione temporale, corsi e progetti si modificano nell’asset Unity `JourneyContentCatalog` tramite Inspector, senza intervenire sulla logica della scena.
- Il generatore crea l’asset di contenuto solo se non esiste; le successive rigenerazioni della scena non devono cancellare le modifiche ai testi e ai corsi.

## Direzione artistica e prestazioni

- Look stilizzato, low-poly, pulito e coerente, simile a un piccolo diorama.
- Riutilizzare famiglie di asset compatibili e componenti modulari; preferire asset esistenti o assemblaggio/procedura. Evitare di richiedere modellazione manuale significativa in Blender.
- Usare animazioni umanoidi riutilizzabili quando disponibili; animazioni di edifici, oggetti e camera devono essere procedurali e riutilizzabili.
- Limitare luci realtime, draw call, risoluzione delle texture e caricamento iniziale dei media. Riutilizzare materiali e valutare illuminazione baked.
- Considerare caricamento, dimensione WebGL, navigazione semplice e accessibilità delle informazioni. Prevedere in seguito un fallback per dispositivi mobili.

## Architettura attesa

L’implementazione deve essere modulare e orientata ai dati. I concetti principali includono:

- definizione dati per eventi, date, personaggio, destinazione, dialogo, durata, oggetti generati, animazioni, camera e contenuti associati;
- regia del viaggio e movimento del personaggio;
- gestione della camera per inseguimento, selezione e ritorno;
- sistema riutilizzabile di comparsa per edifici e oggetti;
- dialoghi/didascalie;
- punti d’interesse e relative schede;
- gestione delle modalità Journey ed Explore;
- persistenza degli elementi già introdotti nella scena.

I nomi e la forma definitiva dei componenti restano decisioni implementative; l’obiettivo è aggiungere nuove tappe principalmente tramite dati e asset riutilizzati.

## Primo traguardo: vertical slice

Prima di costruire l’intera biografia, realizzare una sequenza rifinita di circa **10–15 secondi**:

1. Strada semplice e camera laterale.
2. Alfio adolescente entra e cammina.
3. Compaiono scuola media e insegnante.
4. Breve dialogo sullo studio dell’informatica.
5. Transizione temporale di cinque anni e passaggio alla variante più grande.
6. Compaiono riferimenti visivi a C++, PHP, HTML/CSS e database.
7. Appare il primo progetto scolastico come punto d’interesse.
8. Selezionandolo, la narrazione si ferma e si apre una scheda; chiudendola, si può riprendere.
9. Si può mettere in pausa/riprendere ed effettuare zoom per vedere la porzione di mondo costruita.

Questa fetta serve a valutare stile, personaggio, asset, camera, ritmo, dialoghi, animazioni, interazione e fattibilità WebGL prima di estendere il progetto.

## Criteri di qualità

- La narrazione può essere avviata, messa in pausa, ripresa e saltata.
- Le tappe introdotte restano esplorabili.
- Selezionare un elemento apre informazioni e link senza perdere la posizione narrativa.
- L’esplorazione non richiede abilità da videogiocatore.
- Le informazioni professionali restano rapide da consultare.
- La scena mantiene uno stile coerente e prestazioni adeguate alla destinazione WebGL.
- Una nuova tappa può essere aggiunta senza riscrivere la regia centrale.

## Decisioni ancora aperte

- Nome e percorso definitivi della cartella Unity appena creata.
- Selettore preciso di asset/personaggio e relative licenze.
- Dati biografici, date, testi, immagini e collegamenti definitivi.
- Implementazione specifica dei contenuti dati e della navigazione Explore.
- Requisiti di fallback mobile e hosting del build WebGL.

Il brief completo e originario è conservato in `INTERACTIVE_CAREER_JOURNEY_CODEX_BRIEF.md`.
