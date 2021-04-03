---
title: "Abfallprodukt"
date: "2008-10-28"
categories: 
  - "anwendungen"
  - "c"
  - "windows"
tags: 
  - "tor"
  - "torsignal"
---
# Abfallprodukt
_28.10.2008 00:00:00_
|Categories|
|-|
|[anwendungen](/dotnetwork/de/categories#anwendungen) :black_small_square: [c](/dotnetwork/de/categories#c) :black_small_square: [windows](/dotnetwork/de/categories#windows)|

|Tags|
|-|
|[tor](/dotnetwork/de/tags#tor) :black_small_square: [torsignal](/dotnetwork/de/tags#torsignal)|



Auf der Suche nach einer Möglichkeit, TOR per Software anzusprechen bin ich mehrmals über das sog. “TorSignal4Windows” gestolpert. Leider scheinen alle Links darauf tot zu sein. Also habe ich – quasi als Abfallprodukt eines anderen Projekts – selber ein TorSignalForWindows entwickelt.

Das Programm ist in c# (NET 2.0) programmiert und verwendet die Telnet-Bibliothek von[Klaus Basan](http://www.klausbasan.de/misc/telnet/index.html).

Die Funktionsweise ist simpel:

```
TorSignalForWindows ip port signal
```

Folgendes Beispiel ändert die Identität:

```
TorSignalForWindows localhost 9051 NEWNYM
```

Für SourceForge ist es doch etwas simpel, ... :)

[TorSignalForWindows (Sourcecode)](http://files.oles-cloud.de/TorSignalForWindowsSource.zip)

[TorSignalForWindows (Ausführbare Datei)](http://files.oles-cloud.de/TorSignalForWindows.zip)

Das Programm darf gerne für Nicht-Kommerzielle Zwecke verwendet und auch Weiterentwickelt werden.
