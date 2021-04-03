---
title: "FireTV mittels Kodi (XBMC) zum vollwertigen und genügsamen Mediaplayer machen (kein root erforderlich)"
date: "2015-01-24"
categories: 
  - "allgemein"
tags: 
  - "android"
  - "firetv"
  - "kodi"
  - "xbmc"
---
# FireTV mittels Kodi (XBMC) zum vollwertigen und genügsamen Mediaplayer machen (kein root erforderlich)
_Published:_ 24.01.2015 00:00:00

_Categories_:[allgemein](/dotnetwork/de/categories#allgemein)

_Tags_:[android](/dotnetwork/de/tags#android) - [firetv](/dotnetwork/de/tags#firetv) - [kodi](/dotnetwork/de/tags#kodi) - [xbmc](/dotnetwork/de/tags#xbmc)


**Update (24.07.2015):**

Um Kodi zu aktualisieren, ohne die Einstellungen zu verlieren, reicht es folgenden Befehl mit der neuen APK auszuführen:

`adb install -r D:\\downloads\\kodi-15.0-Isengard-armeabi-v7a.apk`

(Das -r steht dabei für "reinstall")

Wenn beim ersten Start etwas schief geht (oder man versehentlich die Home-Taste gedrückt hat, während Kodi noch initialisiert) kann es vorkommen, dass Kodi nicht mehr startet. Dann in der SQL-Datenbank einfach die Datenbanken MyVideos93 und MyMusic52 löschen. Die Initialisierung startet dann beim nächsten Kodi-Aufruf erneut.



### **Worum gehts?**

Ein paar Sachen vorweg: Das Rad habe ich keineswegs erfunden, sondern alles, was hier steht, haben andere vor mir herausgefunden. Ich möchte nur versuchen das alles auf eine nachvollziehbare Zusammenfassung zu reduzieren. Diese Anleitung besteht aus drei Blöcken, die wahlweise komplett nacheinander durchgearbeitet werden können. Jedes Themengebiet ist aber auch für sich abgeschlossen.

Folgendes werde ich erklären:

##### 1\. Wie bereite ich mein Netzwerkspeicher (NAS) für Kodi vor

Ich werde anhand der Synology erklären, wie die Freigaben eingerichtet werden, Berechtigungen gesetzt und ggf. die Dateinamen angepasst werden

##### 2\. Installation und Konfiguration von Kodi auf der FireTV

Die Installation benötigt KEIN root auf der FireTV. Ein paar DOS-Befehle sind einzugeben. Aber keine Sorge: Das ist einfach nur Copy & Paste und dann klappt das schon.

##### 3\. Speichern der Kodi-Bibliothek auf dem NAS

Wenn man viele Serien und Filme hat, wird auch die Bibliothek von Kodi deutlich größer. Ich hatte schnell ein Gigabyte verbraten. Auf einem normalen Rechner kaum zu merken, aber die FireTV hat gerade gut 5 GB an freiem Speicher. Wäre doch blöd das alles für den Medienplayer zu verbraten.

Wer das ganze lieber im Bewegtbild mag, kann sich mein Video auf YouTube anschauen:

[Kodi installation auf FireTV](https://www.youtube.com/watch?v=bUo6xYiEvFM&feature=youtu.be)

## 1\. Wie bereite ich mein Netzwerkspeicher (NAS) für Kodi vor

Für den ersten Schritt benötigen wir nicht viel. Erst einmal natürlich einen Netzwerkspeicher mit Filmen, Musik oder Serien. Sowie einen Benutzer für die Freigaben. Ich beschreibe den Vorgang hier für den Synology NAS, so oder so ähnlich wird es aber auf den meisten Netzwerkspeichern durchgeführt.

Fangen wir mit dem Erstellen der Netzwerkfreigaben an. Obwohl die Synology viele Optionen des Zugriffs bietet, fallen einige von vornherein schon einmal flach. DLNA/UPnP fällt beispielsweise flach. Zwar kann Kodi diese auch als Quellen verwenden, da hier aber der Server die Arbeit übernimmt, wird keine Bibliothek angelegt. Am einfachsten lassen sich Windows-Freigaben (SMB) einrichten.

Wenn es noch gar keine Daten gibt kann einfach in der Systemsteuerung über “_Gemeinsame Ordner – Erstellen”_ eine neue Freigabe eingerichtet werden. Wichtig ist hier lediglich der Name. Alle anderen Einstellungen kann man nach dem persönlichem Geschmack einstellen. Erstellt für alle drei Kategorien (Filme, TV-Shows und Musik) jeweils eine Freigabe. Leider gibt es derzeit keine Möglichkeit, bereits bestehende Inhalte in eine Freigabe umzuwandeln. Daher meine Empfehlung: Neue Freigabe wie oben beschrieben erstellen und den existierenden Ordner dort hinein verschieben. Das sollte aus Geschwindigkeitsgründen über den Dateimanager geschehen.

![[cml<em>media</em>alt id='1532']createFolder[/cml<em>media</em>alt]](images/createFolder_oftsar.png)

Nachdem die Freigaben angelegt sind, wird es Zeit, einen Nutzer anzulegen. Ich habe für das Mediencenter einen eigenen Nutzer angelegt. Wenn bereits ein Benutzer mit Zugriffsrechten extistiert, kann dieser Schritt übersprungen werden. Ich habe aber gerne pro Dienst einen eigenen Benutzer, den ich auch entsprechend individuell konfigurieren kann. Mit der Synology ist das schnell gemacht. Nach der Anmeldung direkt in die Systemsteuerung, dann auf Benutzer und dann auf Erstellen klicken. Es öffnet sich ein neues Fenster.

![[cml<em>media</em>alt id='1533']createUser[/cml<em>media</em>alt]](images/createUser_uu4ct8.png)

Hier den Benutzernamen eintragen und – sofern gewünscht  – das Passwort hinterlegen. Ob ein Passwort sinnvoll ist oder nicht hängt natürlich auch von den Berechtigungen ab und ob ein Zugriff von Außen erfolgen kann.

Im Anschluss wird automatisch gefragt, zu welcher Gruppe der User zugeordnet werden soll. Lasst hier die Voreinstellung wie sie ist (users) und klickt auf Weiter. Es folgt das Fenster für die Ordnerberechtigungen:

![[cml<em>media</em>alt id='1543']selectPermissions[/cml<em>media</em>alt]](images/selectPermissions_afk0ms.png)

Setzt hier den Haken für “Nur Lesen” bei den vorher erzeugten Freigaben für Filme, Musik und TV-Shows. Möchtet Ihr, dass Kodi auch Bilder (Fanart u.ä.) direkt in den Verzeichnissen auf Eurem NAS ablegt, nehmt stattdessen Lesen/Schreiben. Klickt dann solange auf Weiter (ohne irgendetwas anzuhaken), bis das Fenster wieder schließt.

Damit ist das NAS vorbereitet. Ihr solltet jetzt von Eurem PC aus bereits die Freigaben sehen können.

## 2\. Installation und Konfiguration von Kodi auf der FireTV

Jetzt kommt der “Nerd”-Part. Es gibt zwar auch einige Tools, die eine grafische Oberfläche bieten um Kodi auf der FireTV zu installieren, aber keine davon hat wirklich bei mir funktioniert. Zudem kann man im Fehlerfall nur raten, was schief gelaufen ist. Aber keine Sorge. Per Copy&Paste ist es einfach die Handvoll Befehle auszuführen.

Als erstes benötigen wir “ADB”. Das steht für “Android Debug Bridge” und ist – knapp formuliert – eine Verbindung zu Eurem Android-Gerät (und beispielsweise mit der FireTV). Vergleichbar am ehesten mit einem Telnet- oder SSH-Programm wie Putty. ADB ist bestandteil des AndroidSDKs. Da dieses jedoch relativ groß ist, empfiehlt sich stattdessen “Minimal ADB” von xda Developers herunterzuladen:

[http://forum.xda-developers.com/showthread.php?t=2317790](http://forum.xda-developers.com/showthread.php?t=2317790)

Installiert bzw. Entpackt ADB und merkt Euch das Installationsverzeichnis.

Zusätzlich brauchen wir natürlich auch noch Kodi selbst:

[http://kodi.tv/download/](http://kodi.tv/download/)

Nehmt einfach die aktuellste Version für Android/ARM.

Im Netz wird hin und wieder die speziell auf FireTV optimierte SBMC-Version empfohlen, aber nach meinen tests ist diese Version bei weitem nicht so aktuell. wie Kodi. Und da Kodi in der aktuellen Version keinerlei Probleme macht, sollte man die aktuellste Version verwenden.

Wechselt dann in die Eingabeaufforderung (Start – CMD bei Windows) und mit CD in das Verzeichnis von ADB:

cd AndroidSDK\\sdk\\platform-tools

Um zu schauen, welche Geräte bereits verbunden sind und den Server zu initialisieren

adb devices

eintippen. Es sollte nun folgende Ausgabe erscheinen:

![[cml<em>media</em>alt id='1529']adb<em>devices[/cml</em>media_alt]](images/adb_devices_uvl4to.png)

Wenn nach “List of devices attached” keine Leerzeile steht, sondern eine Buchstaben-Zeichen-Kombination, habt ihr schon eine Verbindung am PC. In der Regel bedeutet dies schlicht, dass Ihr gerade Euer Smartphone per USB mit dem PC verbunden habt. Entfernt das Smartphone, bevor ihr weiter macht.

Jetzt benötigt Ihr die IP-Adresse Eurer FireTV. Um sie herauszufinden, geht in der FireTV auf Einstellungen – System – Info – Netzwerk:

Merkt Euch die Adresse und tragt im DOS folgendes ein:

adb connect 192.168.178.44

(Ersetzt die IP-Adresse durch die Eurer FireTV)

Kontrolliert nun erneut über

adb devices

ob die Verbindung funktioniert hat. Es sollte jetzt die IP-Adresse der FireTV angezeigt werden. Ist das der Fall kann bereits Kodi installiert werden über adb install:

adb install D:\\downloads\\kodi-14.0-Helix-armeabi-v7a.apk

Der Dateiname und natürlich auch der Ort kann etwas abweichen, je nachdem welches die derzeit aktuellste Version ist.

Die Installation dauert eine ganze Weile. Am Ende wird dann angezeigt, dass die Datei erfolgreich übertragen wurde:![[cml<em>media</em>alt id='1535']iinst<em>success[/cml</em>media<em>alt]](images/iinst_success_wfs45l.png) Schaut jetzt in der FireTV nach, ob Kodi installiert wurde. Unter “Einstellungen – Anwendungen – Alle installierten Apps verwalten” sollte nun auch Kodi zu finden sein. Startet die App und es sollte Euch Kodi begrüßen:

![[cml</em>media<em>alt id='1536']kodi</em>welcome[/cml<em>media</em>alt]](images/kodi_welcome_qpsirn.png)

Sollte etwas von der Anzeige abgeschnitten sein, wechselt in den Menüpunkt “System”, dann erneut auf “System”, stellt ganz unten den “Settings Level” auf “Expert” und dann auf “Video Output”. Dort könnt Ihr unter “Video Calibration” mit den Pfeiltasten der FireTV – Fernbedienung die richtige Auflösung einstellen. Stellt – wenn Ihr wollt – am Ende den “Settings Level” wieder zurück.

![[cml<em>media</em>alt id='1531']calibration[/cml<em>media</em>alt]](images/calibration_vj8gln.png)

Fügt jetzt noch keine Quellen hinzu, wenn ihr die Datenbank auf eurem Netzwerkspeicher hinterlegen wollt. Man kann Kodi auch mit einem lokalen Speicher verwenden, aber das verbraucht relativ viel Platz. Ein GB ist schnell verbraucht und Speicherplatz ist beschränkt auf der FireTV.

## 3\. Speichern der Kodi-Bibliothek auf dem NAS

Kodi benötigt eine MySQL-Datenbank, um die Daten im Netzwerk zu speichern. Bei der Synology existiert die sog. MariaDB. Dies ist eine relativ normale MySQL-Datenbank. Installiert sie einfach über den Paketmanager. Und wenn ihr schon mal da seid: PhpMyAdmin gleich mit. (Letzteres kann man Ende wieder entfernt werden. Es wird nur zur Konfiguration benötigt).

Nach der Installation ist die MariaDB erst einmal komplett ungeschützt. Der Standarduser heißt “root” und benötigt KEIN Passwort. Deshalb am besten als Erstes in der Systemsteuerung der Synology auf MariaDB klicken und “Change MariaDB password” auswählen. Das leere Passwort dann mit “OK” bestätigen und im Anschluss ein neues (gutes) Passwort vergeben.

![[cml<em>media</em>alt id='1539']mariadbpass[/cml<em>media</em>alt]](images/mariadbpass_vh5qag.png)

Wechselt nun in den Paketmanager zu phpMyAdmin und klickt auf den Link unter “URL”. Meldet Euch dort nun mit den Zugangsdaten an, die Ihr vorher bei der MariaDB eingegeben habt. Geht dort nun auf “Benutzer” und legt einen neuen User an. Dieser muss zunächst ALLE Rechte bekommen, die man vergeben kann. Diese Rechte können nach der Installation wieder eingeschränkt werden. Außerdem kann bei “Host” die IP-Adresse der FireTV eingetragen werden, um andere Zugriffe zu verhindern. Klickt also bei “Globale Rechte” auf “Alle Auswählen” und dann auf “OK” um den Kodi-Superuser zu erstellen. ![[cml<em>media</em>alt id='1534']dbuser[/cml<em>media</em>alt]](images/dbuser_wpz5po.png)

Um nun auch die Thumbnails zentral zu speichern muss zunächst eine Freigabe erstellt werden (Beschrieben unter Punkt 1). Hier muss der User für die FireTV jedoch Schreibberechtigungen erhalten.

Jetzt muss Kodi natürlich noch mitgeteilt werden, dass die MySQL-Datenbank auch verwendet werden soll. Erstellt dazu eine Datei “advancedsettings.xml” mit folgendem Inhalt:

```
<advancedsettings> 
 <videodatabase> 
 <type>mysql</type>
 <host>192.168.178.23</host> 
 <port>3306</port> 
 <user>fire</user> 
 <pass>AwesomePassword</pass>
 </videodatabase> 
 <musicdatabase> 
 <type>mysql</type> 
 <host>192.168.178.23</host> 
 <port>3306</port> 
 <user>fire</user> 
 <pass>AwesomePassword</pass> 
 </musicdatabase> 
 <videolibrary> 
 <importwatchedstate>true</importwatchedstate> 
 <importresumepoint>true</importresumepoint> 
 </videolibrary> 
 <pathsubstitution> 
 <substitute>
 <from>special://masterprofile/Thumbnails</from> 
 <to>smb://fire@sauger/thumbs</to> 
 </substitute> 
 </pathsubstitution> 
 </advancedsettings>
```
Ersetzt (jeweils zweimal) den Wert bei “host” durch die IP-Adresse Eures Netzwerkspeichers, und die Werte bei “user” und “pass” durch die Zugangsdaten des soeben erstellten Benutzers. Der letzte Block (“pathsubstitution”)  ist für die Thumbnails. Hier muss ebenfalls der Pfad angepasst werden. Er muss immer mit “smb://” beginnen. Hier sind sowohl hostname, als auch IP-Adresse erlaubt.

Jetzt muss diese Datei noch auf die FireTV übertragen werden. Wechselt dazu wieder ins DOS und ruft erneut adb auf mit folgendem Befehl:

adb push d:advancedsettings.xml /sdcard/Android/data/org.xbmc.kodi/files/.kodi/userdata/advancedsettings.xml

Da es sich um ein Linux-System handelt ist natürlich auf Groß/Kleinschreibung zu achten. Auf der sicheren Seite seit Ihr, indem Ihr per Copy/Paste den Befehl übertragt. Wenn es geklappt hat wird angezeigt, wieviel Bytes übertragen wurden.

Um sicher zu gehen, könnt Ihr direkt auf das Android-System wechseln und Euch den Verzeichnisinhalt mit

```
adb shell
 ls /sdcard/Android/data/org.xbmc.kodi/files/.kodi/userdata/
 exit
```

anzeigen lassen. Es sollte die eben erstellte advancedsettings.xml angezeigt werden.

![[cml<em>media</em>alt id='1538']listadvancedsettings[/cml<em>media</em>alt]](images/listadvancedsettings_ovqfce.png)

Das wars schon in Sachen “gefrickel”. Geht nun wieder zur FireTV unter “Einstellungen – Anwendungen – Alle installierten Apps verwalten – Kodi” und wählt dort zunächst “Stoppen erzwingen” und dann “App starten”. Dadurch wird die neue Konfiguration eingelesen.

Jetzt könnt Ihr Eure Videos, Serien und Musik hinzufügen. Wechselt dazu in Kodi auf “Videos – Files – Add Videos”. Im folgenden Fenster dann auf “Browse” und ganz unten auf “Add Network Location”. Tragt in der folgenden Seite die IP oder den Namen Eures NAS  bei “Server name” ein. Tragt dann bei “Username” und “Password” den Benutzernamen ein, den Ihr ganz am Anfang auf dem NAS angelegt habt (nicht den SQL-Benutzer).

![[cml<em>media</em>alt id='1530']addshare[/cml<em>media</em>alt]](images/addshare_hdbznu.png)

Wenn die Daten korrekt waren, sollte nach Auswahl von “OK” in der Liste ein neuer Eintrag erscheinen, der mit “smb://” und dem Namen Eures Netzwerkspeichers beginnt. ![[cml<em>media</em>alt id='1541']newshare[/cml<em>media</em>alt]](images/newshare_tsqb4t.png)

Wählt diesen nun aus und navigiert zu dem Verzeichnis mit Eurer Freigabe für die Filme. Zwei “OKs” später wählt bei “This directory contains” “Movies” aus. Für deutsche Beschreibungstexte dann zunächst in “Settings” wechseln und bei “Preferred Language” “de” auswählen.

![[cml<em>media</em>alt id='1540']movies[/cml<em>media</em>alt]](images/movies_l7a7jc.png)

Nun noch ein zweimal”OK”. Kodi fragt nun, ob für das neue Verzeichnis Informationen gesucht werden sollen. Wählt “YES” und seht, wie Kodi anfängt Daten zu sammeln. Je nach Menge der Filme kann das eine Weile dauern. Auf jeden Fall wird in der oben rechten Ecke angezeigt, dass Kodi die Daten sammelt.

![[cml<em>media</em>alt id='1542']scanning[/cml<em>media</em>alt]](images/scanning_wrkjod.png)

Sollte dieses Fenster nicht oder nur sehr kurz aufflackern, haut was mit Eurer Konfiguration (advancedsettings.xml) nicht hin. Vermutlich ist dann die Datenbank nicht erreichbar. (Falsche IP oder Zugangsdaten in der XML, Firewall blockt, o.ä.)

War alles erfolgreich könnt Ihr nun ebenfalls unter Videos mit den TV-Serien so verfahren und unter “Musik” in gleicher Weise Eure MP3s hinzufügen. Falls etwas schief gegangen ist, schaut ins Log von Kodi. Dazu am Besten unter “System – Debugging” das Debug einschalten. Kodi loggt lokal alle Einstellungen. Da es etwas mühsam ist, das Log direkt in der FireTV zu betrachten (und auch elementare Editoren wie “vi” fehlen) sollte man die Logdatei mittels “pull” von der FireTV herunterladen und dann in Notepad unter Windows öffnen:

adb pull /sdcard/Android/data/org.xbmc.kodi/files/.kodi/temp/kodi.log

In der MySQL/MariaDB-Datenbank sind nun zwei neue Datenbanken angelegt: MyVideos.. und MyMusic.. Für diese Datenbanken muss der MySQL-Nutzer weiterhin Berechtigungen besitzen, die globalen Datenbankberechtigungen können aber entfernt werden.

Ich wünsche viel Spaß mit dem neuen Kodi-Mediacenter.
