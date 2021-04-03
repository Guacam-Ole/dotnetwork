---
createnav: "0.0.1"
title: "Fitbit-Erkennung unter Linux (Raspberry Pi 2)"
date: "2016-02-27"
categories: 
  - "linux"
tags: 
  - "fitbit-de"
  - "raspberry-pi-de"
---
# Fitbit-Erkennung unter Linux (Raspberry Pi 2)
_Published:_ 27.02.2016 00:00:00

_Categories_: [linux](/dotnetwork/de/categories#linux)

_Tags_: [fitbit-de](/dotnetwork/de/tags#fitbit-de) - [raspberry-pi-de](/dotnetwork/de/tags#raspberry-pi-de)


Wie ich bereits [vorher beschrieben habe](http://dotnet.work/2016/01/synology-surveillance-station-mit-einem-einfachen-shell-script-automatisieren/), nutze ich den Raspberry. um mein Smartphone mittels WiFi zu erkennen. Seit Android Marshmellow geht das leider nicht mehr, denn im Schlafmodus wird das Wlan jetzt deaktiviert. Die einfachste Möglichkeit, dieses Problem zu umgehen, wäre die Verwendung von Bluetooth. Und das hatte ich ursprünglich auch vor. Aber während ich noch auf den Dongle gewartet hatte, hat mein Arbeitgeber mir ein "[Fitbit Charge HR](http://amzn.to/1VKm1OJ)" geschenkt. Da mir glaubhaft versichert wurde, dass das "HR" nicht für Human Resources steht und das Ding nicht genutzt wird um zu prüfen, wie viele Zeilen Code ich pro Tag so schreibe, habe ich es mir mal umgebunden und ausprobiert.

Als Nerd passe ich nun nicht wirklich in die Fitness-Zielgruppe. Aber als kleine dezente Uhr verrichtet das Ding einen guten Dienst und die Vibrations-Alarmfunktion gefällt mir richtig gut. Ich werde sicher noch ein Addon für Outlook schreiben, damit mir Termine zukünftig dezent übermittelt werden. Zusätzliche Informationen, wie viele Schritte ich benötige um das Bier aus dem Keller oder die Chips aus der Küche zu holen sind auch ganz nett. Nicht wirklich hilfreich, aber nerdig :)

Da ich mich nun entschieden habe, das Ding eine Weile zu tragen, sollte es doch auch gut dazu geeignet zu sein zu erkennen, ob ich nun zu Hause oder unterwegs bin und es so in meine Überwachungs-Mechanismen einbauen. Zunächst habe ich es mit dem (mittlerweile eingetrudelten) USB-Dongle versucht. Da aber das Armband im sog. "Bluetooth Low Energy (BLE)"-Profil arbeitet, war es mit dem Dongle nicht zu ermitteln. Weder ein

```
hcitool scan
```

noch ein

```
hcitool lescan
```

listeten das Gerät auf.

Glücklicherweise kommt das Armband mit einem USB-Dongle. Und zum Glück gibt es das "[Galileo](https://bitbucket.org/benallard/galileo)"-Package, dass eigentlich für die Synchronisation mittels Linux gedacht ist. Die Installation ist einfach:

```
sudo apt-get install python-usb python-requests
sudo pip install galileo
```

Jetzt noch den USB-Dongle anstecken und das Raspberry neu starten. (Bestimmt gibt es auch die Möglichkeit, nur den USB-Stack neu zu starten, aber ein Neustart ist schlicht einfacher. Und schließlich ist das hier ein Raspberry und kein Hochverfügbarkeits-Server).

Nach der Eingabe von
```
sudo galileo -v
```

sollte in etwa folgendes ausgegeben werden:
```
2016-02-27 16:28:21,875:INFO: 1 trackers discovered
```

Um das nun in ein Script einzubauen braucht es nur noch etwas übliche Bash-Magie:

``` 
if galileo -v | grep -q '1 trackers'; then
  // Do whatever you like if you found it
fi
```

Fertig. :)
