---
createnav: "0.0.1"
title: "Optimizer für Google+ - Anleitung"
date: "2014-02-02"
categories: 
  - "anwendungen"
  - "javascript-programmierung"
  - "jquery-programmierung"
tags: 
  - "chrome-extensions"
  - "javascript"
  - "jquery"
---
# Optimizer für Google+ - Anleitung
_Published:_ 02.02.2014 00:00:00

_Categories_: [anwendungen](/dotnetwork/de/categories#anwendungen) - [javascript-programmierung](/dotnetwork/de/categories#javascript-programmierung) - [jquery-programmierung](/dotnetwork/de/categories#jquery-programmierung)

_Tags_: [chrome-extensions](/dotnetwork/de/tags#chrome-extensions) - [javascript](/dotnetwork/de/tags#javascript) - [jquery](/dotnetwork/de/tags#jquery)


Diese Anleitung wurde für die Version 2.2.0 (\*Widgetmania)\* geschrieben. Aktuellere Versionen können unter Umständen optisch etwas abweichen

# Grundprinzip

Die Extension “Optimizer für Google+”(G+ – Optimizer)  bietet im Wesentlichen drei grundlegende Features:

- Filtern unerwünschter Inhalte
- Verbesserung der Usability
- Darstellen von hilfreichen Widgets

Da die Extension mittlerweile ziemlich umfangreich geworden ist und einige Features “versteckt” sind, folgt hier eine ausführliche Beschreibung der einzelnen Optionen.

Die aktuelle Version der Extension gibt es im \[Google Chrome Web Store\](https://chrome.google.com/webstore/detail/google%2B-filter/edknapjhmlocokbpbihilmjmfmmddhop?utm\_source=stammtischphilosoph)

# Konfiguration

Es gibt zwei Arten, den G+ – Optimizer zu konfigurieren. Zum einen im Einstellungsmenü unter den Chrome-Extensions unter “Tools – Erweiterungen” und anschließendem Klick auf “Optionen”, oder durch Aufrufen der Google+ – Seite und Rechtsklick auf das Symbol des Filters in der Adressleiste. Die meisten Features sind standardmässig deaktiviert und müssen erst im Optionsmenü freigeschaltet werden.

## Filter

Der G+ – Optimizer kann eine ganze Anzahl von Standard-Blöcken filtern. Diese können im Optionsmenü “Filter” ausgewählt werden. Die Filter sind jeweils aktiv, wenn sie farblich dargestellt werden. Schwarz/Weiß -Grafiken bedeuten einen deaktivierten Filter.

### Filtern von Standardblöcken

Standardblöcke sind Bereiche in Google+, die bestimmte Informationen darstellen. Die meisten dieser Blöcke lassen sich individuell filtern.

![filter+1](images/filter+1.png)

#### +1 – Filter

Dieser Filter entfernt Einträge, die automatisch erscheinen, wenn eine Person innerhalb der eigenen Kreise ein “+1″ vergibt.

\#### [![filteryt](images/filteryt.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/01/filteryt.png)YouTube – FilterWenn Du ein Video auf YouTube hochgeladen hast oder dort einen Kommentar hinterlassen hast, werden Antworten darauf auch in Google+ angezeigt. Über diesen Filter kannst Du dies verhindern.

\#### [![filterwham](images/filterwham.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/01/filterwham.png)Wham-FilterZur Weihnachtszeit kann die Dauerberieselung durch “Last Christmas” von Wham schon mal etwas viel werden. Über diesen Filter bleibt Ihr wenigstens bei Google+ davon verschont.

Über das Menü “Einstellungen” läßt sich der Filter noch genauer administrieren:

\- _“Last Christmas” in Beitrag filtern._\- Sobald irgendwo\* im Beitrag\* der Text “Last Christmas” im Text vorkommt, wird dieser entfernt. - _“Last Christmas” in Link filtern. \*- Sobald irgendwo \*in einem Link_ (z.B. YouTube-Video) der Text “Last Christmas” vorkommt, wird der Beitrag entfernt. - _“WHAM” in Beitrag filtern. \*- Sobald irgendwo \*im Beitrag_ der Text “WHAM” im Text vorkommt, wird dieser komplett entfernt. - _“WHAM” in Link filtern. \*- Sobald irgendwo \*in einem Link_ der Text “WHAM” im Link vorkommt, wird der Beitrag komplett entfernt.

\#### [![filtercomm](images/filtercomm.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filtercomm.png)Community-FilterGoogle+ empfiehlt regelmäßig Communities, von denen es glaubt, sie könnten Dich interessieren. Wenn Du lieber selber aktiv nach passenden Communities suchst, kannst Du mit diesem Filter sowohl Empfehlungen, als auch Einladungen deaktivieren.

\#### ![filltergeb](images/filltergeb.png)GeburtstagsfilterWenn ein Kreisling seinen Geburtstag hinterlegt hat, wirst Du aufgefordert, ihm wie alle anderen 24.592 Kontakte des Geburtstagskindes automatisierte Glückwünsche zu übermitteln. Willst Du nicht? Dann den Geburtstagsfilter auswählen.

\#### [![filterknow](images/filterknow.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filterknow.png)Vielleicht kennen Sie… – FilterAnhand der Leute in den Kreisen versucht Google+ mal mehr, mal weniger erfolgreich zu erahnen, wer denn sonst noch für Dich interessant sein könnte. Um das zu verhindern dient der “Vielleicht kennen Sie” – Filter.

\### InhaltsfilterInhaltsfilter können jeden beliebigen Beitrag anhand bestimmter Suchbegriffe filtern.

#### [![filterhash1](images/filterhash1.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filterhash1.png)

#### Hashtag-Filter

![filterhash2](images/filterhash2.png)

Google+ filtert teilweise Beiträge automatisch nach Hashtags. Zusätzlich kann man auch selbst in Beiträgen Hashtags eingeben. Der Hashtag-Filter kann diese Beiträge dann filtern.

Es gibt zwei Möglichkeiten, diese Hashtags einzugeben. Zum einen gibt es die Bearbeitungsmöglichkeit im Menü erweitert. Hier lassen sich auch einmal hinterlegte Hashtags wieder entfernen. Die Hashtags können wahlweise mit oder ohne “#” eingegeben werden. Um mehrere Werte einzugeben, diese mit einem Komma trennen.

Eine zusätzliche Möglichkeit besteht direkt im Google+ – Stream zu filtern. Dort einfach auf einen der grau hinterlegten Hashtags klicken. Die Anzeige des Hashtags wird automatisch um eine Mülltonne erweitert. Ein Klick hierauf fügt den Hashtag zur Filterliste hinzu.

Damit dieser Filter aktiv wird muss allerdings zunächst die Seite neu geladen werden.

#### [![filterfull](images/filterfull.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filterfull.png)Individueller Textfilter

Hashtag-Filter können effektiv bestimmte Themengebiete herausfiltern. Der Volltextfilter hingegen kann jeden beliebigen Text herausfiltern. Im Zweifelsfall sollte aber der Hashtag-Filter verwendet werden. Zum einen, weil der Volltext-Filter deutlich mehr Resourcen benötigt und zum Anderen, weil damit auch versehentlich ungewollt Inhalte gefiltert werden. So bewirkt ein Volltextfilter auf “_icke_” auch, dass “_Zicke_” herausgefiltert wird. Der Volltextfilter wird im Bereich “Erweitert” angepasst und funktioniert dort wie der Hashtag-Filter.

Der Volltextfilter unterscheidet zwischen Groß- und Kleinschreibung.

#### [![bild](images/bild.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/03/bild.png)Bilder-Filter

Bilder lassen sich generell, oder nur für GIF-Formate (animierte Bilder) ausblenden. Dieser Filter löscht keine Inhalte, sondern blendet sie lediglich aus. Sie können Live wieder zum Vorschein gebracht werden

![video](images/video.png)

#### Video-Filter

Der Video-Filter funktioniert technisch ebenso, wie der Bilder-Filter. Hier werden jedoch eingebettete Videos herausgefiltert

\#### [![links](images/links.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/03/links.png)

#### Link-Filter

Der Link-Filter blendet generell sämtliche, verknüpften Website-Links aus dem Stream

\### Sonstige Einstellungen

#### [![filterpause](images/filterpause.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filterpause.png)Pause

Google+ lädt regelmäßig Inhalte nach, z.B. wenn ans Ende gescrollt wird, oder auf den blauen Kasten mit neuen Nachrichten geklickt wird. Der Filter erkennt dies und gibt Google+ zunächst etwas Zeit, den Inhalt nachzuladen, bevor er aktiv wird. Standardmäßig ist dies eine halbe Sekunde. Auf schnellen Rechnern kann man jedoch auch kleinere Werte einstellen. Umgekehrt auf leistungsschwachen Rechnern (oder Internetverbindungen) lässt sich der Wert auch erhöhen.

## Widgets

Der Google+ Optimizer bietet die Möglichkeit, zusätzliche Inhalte in die Google+ – Kacheln einzublenden. Diese werden jeweils am oberen Rand angezeigt und wandern wie normale Beiträge nach unten. Um sie erneut ans obere Ende zu befördern, muss Google+ neu geladen werden. Sämtliche Widgets funktionieren nur beim dreispaltigen Layout von Google+.

#### [![filtersport](images/filtersport.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filtersport.png)Sport-Widget

Es lassen sich bis zu drei Sport-Widgets anzeigen. Diese zeigen aktuelle Ergebnisse in Tabellenform. Durch einen Klick auf die jeweilige Zeile lassen sich weitere Details darstellen. Die Informationen für diese Tabellen stammen von [OpenLigaDB](http://www.openligadb.de/). Sie werden direkt von der Community gepflegt. Die Aktualität, insbesondere Abseits der populären Sportarten kann also schwanken. Wer das Ändern möchte kann jedoch selbst leicht zum Erfolg beitragen, indem er auf OpenLigaDB eigene Ergebnisse einträgt.

Es wird automatisch immer der aktuelle Spieltag der gewählten Liga dargestellt.

Um ein Sportwidget zu aktivieren muss zunächst beim Eintrag “Position” eine der drei Spalten ausgewählt werden. Im Anschluss kann die gewünschte Sportart ausgewählt werden. Eine Änderung der Sportart füllt automatisch die Dritte Auswahl “Liga” mit gültigen Werten.

[![filtersport2](images/filtersport21.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filtersport21.png)Nachdem bis zu drei Sportligen ausgewählt wurden, müssen die Einstellungen über den darunter liegenden Button gespeichert werden.

\#### [![filterwetter1](images/filterwetter1.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filterwetter1.png)Wetter-WidgetDas Wetter-Widget verhindert, dass Du aus dem Fenster schauen musst… Zusätzlich wird eine Wettervorhersage der nächsten 5 Tage dargestellt. Wie beim Sportwidget betimmt die Auswahl “Positon”, in welcher Spalte das Widget dargestellt werden soll.

[![filterwetter2](images/filterwetter2.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filterwetter2.png)Um einen Ort anzugeben, reicht es diesen im Suchfeld einzutragen und auf Suchen zu klicken. Eine Länderangabe, o.ä. ist nicht notwendig. Wird der Ort gefunden, so werden im Auswahlfeld “Ort” die gefundenen Übereinstimmungen dargestellt. Ebenso wie beim Sport müssen diese Einstellungen durch den blauen Button gespeichert werden.

\#### [![filterstopp](images/filterstopp.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/02/filterstopp.png)StoppuhrNie wieder die Pizza anbrennen lassen, wenn Du im Google+ – Stream verloren bist!

Die Stoppuhr zählt die Zeit abwärts, so wie es nunmal eine Stoppuhr üblicherweise macht…

Du stellst Die Zeit in Minuten über den äußeren Ring ein. “Stoppuhr starten” startet dann den Countdown. Der Innere Zeige zeigt die Minuten und am Ende wirst Du durch einen unangenehmen Ton daran erinnert, dass Du die Pizza aus dem Ofen holen wolltest.

Von diesem Widget kann nur eines gleichzeitig geladen werden.

Erweiterungen

#### [![color1](images/color1.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/03/color1.png)Benutzer hervorheben

Wichtige Benutzer, besonders enge Freunde und andere Kontakte lassen sich farblich hervorheben, um sie im Stream nicht zu übersehen. Zusätzlich lassen sich Notizen für diese Benutzer hinterlegen, die ebenfalls bei jedem Beitrag angezeigt werden.

[![trophies](images/trophies.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/03/trophies.png)

#### Trophäen

Seien wir ehrlich: Kein Mensch braucht wirklich Trophäen, aber man kann so herrlich damit angeben… ![:D](images/icon_biggrin.gif)

Für verschiedene Aktionen bei Google+ kann man mit dieser Erweiterung Trophäen bekommen. Einige sind hier zu erkennen, eine Gesamtliste der Trophäen gibt es jedoch nicht. Lasst Euch überraschen, was Ihr verdient habt!

Die Trophäen werden (nur für Dich) im “Über mich” – Bereich angezeigt.

[![emoticons](images/emoticons.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/03/emoticons.png)

#### Emoticons

Aus Foren kennt man sie zur Genüge, aus dem Chat sowieso: Emoticons machen das Erlebnis etwas bunter. Mit diesem aktivierten Feature nun auch bei Google+

\#### [![qs](images/qs.png)](http://www.stammtischphilosoph.com/wp-content/uploads/2014/03/qs.png)QuickshareQuickshare-Icons erlauben es, Beiträge mit häufig genutzten Kreisen schnell zu teilen. Zusätzlich lassen sich “Bookmark-“Kreise definieren um sie als Favoritensammlung zu nutzen.
