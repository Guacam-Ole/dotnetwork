---
title: "PS3 Media Server"
date: "2009-03-08"
categories: 
  - "windows"
tags: 
  - "mediathek"
  - "ps3"
---

Ich habe heute mal wieder versucht MKV-Dateien auf meine PS3 zu streamen. Nach einer frischen Installation sollte es endlich klappen über TVersity die HD-Filme auf der Konsole zu schauen. Um es kurz zu machen: Es klappte nicht, obwohl ich mir wortwörtlich an die Anleitungen gehalten habe.

Bei der Suche nach Problemlösungen bin ich nun auf den “PS3 Media Server” gestossen. Es handelt sich um ein Java-Projekt unter der GNU-Lizenz. Es gibt den Server für Windows, Linux und sogar für den angebissenen Apfel. (Wer kommt auf die Idee, einen Mac als Streaming Server einzurichten?..)

Also downgeloaded, installiert und – das Teil läuft auf anhieb. 720p 1080p,Dolby Digital: Alles funktionierte sofort, ohne auch nur ein bißchen zu konfigurieren. Selbst Filme, die auf meinem PC Bild/Ton-Synchronisationsprobleme haben laufen gestreamt auf der PS3 plötzlich flüssig.

Das Programm bietet zwar extrem viele Einstellmöglichkeiten für die einzelnen Codecs und Co. da bei mir aber alles auf Anhieb funktionierte habe ich tunlichst die Finger davon gelassen

Standardmässig werden alle Laufwerke freigegeben, man kann dies aber natürlich beschräken. Grösstes Manko ist wohl, dass man ausser der Freigabe nicht viel Einstellen kann: Eine Unterscheidung nach Video, Bildern und Sound sind ebenso wenig möglich (es werden in allen Bereichen immer alle Ordner angezeigt), wie das Vergeben individueller Bezeichnungen für die Ordner, wie dies etwa TVersity anbietet.

Die Benutzeroberfläche sieht recht “technisch” aus, mir gefällt das aber besser als der Flash-Style der TVersity-Oberfläche. Da aber ohnehin alles problemlos funktioniert werde ich das Interface wohl sicher nicht allzu oft anzeigen. Recht unscheinbar erscheint übrigens die Lupe über der Anzeige der freigegebenen Ordner:

Hier versteckt sich eine indizierungsfunktion, die etwa die Interpreten von MP3 erstellt. Diese Tags können dann in der Playstation genutzt werden.

Unterm Strich bin ich also endlich auf eine funktionierende Streaming-Lösung gestossen, die ich nur jedem Empfehlen kann. Einziges Manko sind die noch etwas knappen konfigurationsmöglichkeiten und die Tatsache, dass Webinhalte noch per Editieren von Textdateien eingepflegt werden müssen. Die üblichen Verdächtigen wie etwa youtube, oder gametrailers sind aber schon bereits fertig konfiguriert.

Da der Kern aber stimmt gehe ich davon aus, dass diese “Kleinigkeiten” bald nachgereicht werden.

Ein gutes Howto ist übrigens in diesem [Blog](http://otmanix.de/2009/01/30/howto-ps3-media-server-auf-windows-xp/) versteckt

Am Ende noch ein kleines Howto für Videos aus der ZDF Mediathek. Das ist leider noch etwas umständlich, aber immerhin funktioniert es.

Step 1: VLC installieren. Step 2: Einsammeln der URL. Dazu einfach auf die entspechende Seite gehen, gewünschte Qualität einstellen und als Player den VLC einstellen. Die Url steht am unteren Ende Step 3: URL in VLC öffnen (Medien/Netzwerk öffnen) und unter (Netzwerk/Medieninformationen) die Original URL heraussuchen: Step 4:Die URL in die Datei (C:Program FilesPS3 Media Server) WEB.Conf eintragen:

```
videostream.Web,TVs=<em>Neues aus der Anstalt</em>,<strong>mms://ondemand.msmedia.zdf.newmedia.nacamar.net/zdf/data/msmedia/zdf/09/02/090217_anstalt_nad_vh.wmv</strong><br></br>
```

(Titel schräg hervorgehoben, URL Fett)
