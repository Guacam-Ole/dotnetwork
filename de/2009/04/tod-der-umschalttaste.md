---
createnav: "0.0.2"
title: "Tod der Umschalttaste!"
date: "2009-04-15"
categories: 
  - "windows"
tags: 
  - "registry"
---
■ [.net.work](/) » [de](/de) » [2009](/de#2009)  » 4 » Tod der Umschalttaste!

# Tod der Umschalttaste!
_Published:_ 15.04.2009 00:00:00

_Categories_: [windows](/de/categories#windows)

_Tags_: [registry](/de/tags#registry)


Ich ärger mich immer wieder über die blöde Shift-Lock, bzw. Caps-Lock – Taste, auf deutsch auch “Feststelltaste” genannt, die ich eigentlich noch nie wirklich gebraucht habe. In Zeiten, an denen auf alten Schreibmaschinen fast schon ein Amboss nötig war, um Tasten herunterzudrücken, war diese Taste sicherlich noch sinnvoll, heute schleppt man dieses unnötige Überbleibsel mit wie die [Pferdearschbreite beim Bau der Space-Shuttles.](http://blog.b-o-f-h.net/index.php?/archives/37-Breit-wie-ein-Pferdearsch.html)

Doch zum Glück gibt es Abhilfe. Folgenden Text als _wechdamit.reg_ speichern und per Doppelklick in die Registrierung übertragen. Schon arbeitet die Umschalttaste so, als wäre sie eine normale Shift-Taste:

```
Windows Registry Editor Version 5.00
\[HKEYLOCALMACHINESYSTEMCurrentControlSetControlKeyboard Layout\] 
“Scancode Map”=hex:00,00,00,00,00,00,00,00,02,00,00,00,2a,00,3a,00,00,00,00,00
```
