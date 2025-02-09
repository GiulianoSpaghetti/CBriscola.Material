# CBriscola

:it: Made in Italy. Il primo prodotto serio pubblicato in avalonia col dialetto material ma senza il foglio di stile di google ed internazionalizzato.

Questo gioco dimostra che la teoria dei giochi è vera: l'algorimo brevettato funziona su tutti i giochi di carte senza piatto.

![Napoli-Logo](https://github.com/user-attachments/assets/8163c808-62d3-40d3-bce3-0957e57bc26a)
![made in parco grifeo](https://github.com/user-attachments/assets/fadbf046-aeae-4f11-bda4-eb332c701d56)


## CBriscola.Material
Quello che avete davanti non è il gioco della briscola come si intende oggi, perché oggi tutti i simulatori di briscola dicono "hai preso l'asso, bravo" e finisce lì. Quello che avete davanti è un simulatore equo e professionale, con punteggio aggiornato in tempo reale, in modo da poter decidere se "rischiare" o meno coscientemente, scritto in avalonia col dialetto material. Sembra strano a dirsi, ma è Hard Core (i vecchietti che urlano "guarda che sto piombo a denari" davanti al monitor nella variante normale, oppure i bambini nella variante bussata), perché si ha il potere di cambiare in ogni istante l'andamento della parita coscientemente con le proprie scelte.

Dal momento che avalonia ha i timer che vengono blacklistati, c'è il pulsante per continuare a giocare.

Permette di usare la variante "bussata" nella quale bisogna rispondere al seme (il "poker" nella teoria dei giochi).

E' in avalonia, ma col dialetto material di google, ma senza usare il foglio di stile di google, ma usando la reactive ui (si legge multithreaded)

# Come installare

## Su Windows

[![winget](https://user-images.githubusercontent.com/49786146/159123313-3bdafdd3-5130-4b0d-9003-40618390943a.png)](https://marticliment.com/wingetui/share?pid=GiulioSorrentino.CBriscola.Avalonia&pname=CBriscola.Avalonia&psource=Winget:%20winget)

Prerequisiti:

https://winstall.app/apps/Microsoft.DotNet.DesktopRuntime.9

Note: i package sono system indpedent ed IL, per cui in teoria è sufficiente reinstallare il pacchetto ad ogni aggiornamento del desktop runtime ed avviare il programma una volta per ottenere il codice binario aggiornato.
Ovviamente se aggiornano avalonia bisogna ricompilare.


## Su GNU/linux
Seguite le istruzioni su http://numeronesoft.ddns.net:8080

NOTA BENE: la connessione a numeronesoft.ddns.net non e https

Poi installate cbriscola.avalonia

Prerequisiti:

https://learn.microsoft.com/en-us/linux/packages

Note: i package sono system indpedent ed IL, per cui in teoria è sufficiente reinstallare il pacchetto ad ogni aggiornamento del desktop runtime ed avviare il programma una volta per ottenere il codice binario aggiornato.
Ovviamente se aggiornano avalonia bisogna ricompilare.


ATTENZIONE:

Avalonia si basa su due librerie native libharfbuzzsharp e libskiasharp che non sono libere, quindi avalonia non è libero.

## Per compilare

Bisogna scaricarsi da nuget il package CardFramework.avalonia

## Come funziona
Per festeggiare, vi spiego come funziona il mio algoritmo brevettato:
i punti in totale sono 120, ossia 4 assi che valgono 11 punti ciascuno, 4 3 che valgono 10 punti ciascuno, 4 10 che valgono 4 punti ciascuno, 4 9 che valgono 3 punti ciascuno, 4 8 che valgono 2 punti ciascuno.
Dal momento che la matematica non è una opinione:
4x11+4x10=84.
4x4+4x3+4x2=16+12+8=36

84+36=120 punti totali

120/2 = 60, servono 61 punti per vincere

basandosi solo sui carichi si rischia di perdere, perché

84-61=23, bisogna prenderli quasi tutti e lasciare solo 23 punti di carichi

60-36=24, prendendo tutte le altre carte bastano solo 3 carichi per vincere.

Per cui non metto i livelli, ma vi lascio imparare la teoria delle carte a lungo, da me inventata a 18 anni, con la wxbriscola, che mi ha portato l'amore di Francesca.                                                                                                                                              
## Internazionalizzazione
Aprire il file MainWindows.axaml, all'interno del tag MainWindow.Resources ci sono qulli che vengono chiamati dizionari.
BIsogna copiare un dizionario ed aggiungerlo alla fine dei dizionari, chiamarlo con la denominazione internazionale a due carattri ella lingua (it per italiano, pt per portoghese, es per spagnolo e via dicendo) e bisogna tradurre tutto qullo che è il contenuto del tag x:string, non il parametro.

Infine compilare.

## Dove recuperare i mazzi aggiuntivi

I mazzi aggiuntivi sono quelli della wxbriscola, si possono scaricare sulle relative home page dei progetti, per windows e linux e per GNU/Limux dal mio repository.

## Bug noti

Se si usa un mazzo non completo all'avvio del programma, verrà caricato il mazzo napoletano e l'avviso non è garantito che esca.



## Screenshot

## Su debian

![Schermata del 2025-01-25 18-24-13](https://github.com/user-attachments/assets/c8023243-8986-406e-95d9-a77626095277)
![Schermata del 2025-01-25 18-24-22](https://github.com/user-attachments/assets/be187f6a-9479-4af2-b17b-39df3b27c9ce)
![Schermata del 2025-01-25 18-24-29](https://github.com/user-attachments/assets/ed19d824-3a6a-44e9-afce-30f5b68ce085)
![Schermata del 2025-01-25 18-24-36](https://github.com/user-attachments/assets/a9445ffc-cfed-4938-9ff1-e161f1ec3468)

## Su Windows

<img width="1920" alt="2025-01-25 (8)" src="https://github.com/user-attachments/assets/fc2644ff-6b18-4675-8179-29075884240e" />
<img width="1920" alt="2025-01-25 (7)" src="https://github.com/user-attachments/assets/00424793-b22d-43ac-8955-c68ae501c862" />
<img width="1920" alt="2025-01-25 (6)" src="https://github.com/user-attachments/assets/f4b77c31-b324-4e59-b310-1c68854d36aa" />

## Bug noti

In questa versione i mazzi aggiuntivi non vengono riletti ad ogni esecuzione della pagina opzioni, perché è una e viene caricata solo la prima volta. E' necessario chiudere e riaprire il programma una volta installati i mazzi aggiuntivi.

In questa versione non è possibile vedere più di due avvisi nella notifica, a volte ce ne possono essere anche tre.


## Donazione

http://numerone.altervista.org/donazioni.php
