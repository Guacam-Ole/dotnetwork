---
createnav: "0.0.1"
title: "Smartphone-Erkennung unter Linux mittels Bluetooth (Raspberry Pi 2)"
date: "2016-02-27"
categories: 
  - "linux"
tags: 
  - "bluetooth-de"
  - "raspberry-pi-de"
---
# Smartphone-Erkennung unter Linux mittels Bluetooth (Raspberry Pi 2)
_Published:_ 27.02.2016 00:00:00

_Categories_: [linux](/dotnetwork/de/categories#linux)

_Tags_: [bluetooth-de](/dotnetwork/de/tags#bluetooth-de) - [raspberry-pi-de](/dotnetwork/de/tags#raspberry-pi-de)


Nachdem ich mein Smartphone mittels WiFi erkenne (was seit Android Marshmallow nicht mehr wie gewünscht funktioniert) und auch das Fitness-Armband Fitbit erkenne, bin ich jetzt zurück zum ursprünglichen Plan gekehrt, das Smartphone über Bluetooth zu erkennen.

Ich habe im Netz viele verschiedene Anleitungen für diesen Zweck entdeckt, aber keine schien mit meiner Konstellation zu funktionieren. Ob es daran liegt, dass die Anleitungen für die alte Pi (1) geschrieben wurden, oder Debian Jessie hier anders funktioniert, kann ich nicht mit Bestimmtheit sagen. Ich weiß nur, dass diese Lösung hier damit bei mir funktioniert. "Works on my machine" sozusagen :)

Zunächst einmal braucht es dafür einen USB-Dongle. Theoretisch sollte der Hersteller egal sein, aber wie das nun mal so ist mit Treibern unter Linux: Hat man mal eine funktionierende Variante herausgefunden, sollte man lieber keine andere ausprobieren. Ich habe den [Logilink BT0015](http://amzn.to/2axiYtW) verwendet.

Bevor es los geht, die üblichen Vorkehrungen bei Linux. Erst einmal alles aktualisieren:

 
```
sudo apt-get update
 sudo apt-get upgrade
```

Dann werden die benötigten Bluetooth-Bibliotheken installiert:

sudo apt-get install --no-install-recommends bluetooth

Ich habe dabei eine Fehlermeldung erhalten, dass einige Abhängigkeiten nicht aufgelöst werden konnten. Die folgenden Befehle haben dies behoben:

```
sudo dpkg --configure -a
 sudo apt-get install -f
 ```

Jetzt kann der USB-Stick eingesteckt werden und der Raspberry neu gestartet:

```
sudo shutdown -r now
```

Um zu prüfen, ob die USB-Hardware erkannt wurde, folgenden Befehl ausführen:

```
lsusb
```

Dies listet alle Geräte auf. Eines davon sollte der Bluetooth-Dongle sein. Prüfen wir nun, ob auch die Funktionalität da ist. Folgender Befehl sollte eine Ausgabe verursachen, in der das wichtige Wort "Running" vorkommt:

```
$ /etc/init.d/bluetooth status
● bluetooth.service - Bluetooth service
 Loaded: loaded (/lib/systemd/system/bluetooth.service; enabled)
 Active: active (running) since Sat 2016-02-27 19:31:45 UTC; 17min ago
 Docs: man:bluetoothd(8)
 Main PID: 7841 (bluetoothd)
 Status: "Running"
 CGroup: /system.slice/bluetooth.service
 └─7841 /usr/lib/bluetooth/bluetoothd
```
Soweit, so gut. Zeit, die Bluetooth-Geräte in der näheren Umgebung zu erfassen. Jetzt am Telefon **noch nichts einstellen.**
```
Scanning ...
 7C:2F:BE:EF:FA:CE Ole
 00:04:BE:EF:FA:CE SHIELD
 B8:86:BE:EF:FA:CE Fette Glotze
 88:53:BE:EF:FA:CE TOMATO-PC
 48:44:BE:EF:FA:CE TVBluetooth
 ```

(Ich habe die Mac-Adressen leicht verändert. Keine Ahnung, ob man mit deren Kenntnis etwas schlimmes anstellen kann, aber warum ein Risiko eingehen? :) )

Jetzt am Smartphone dafür sorgen, dass es über Bluetooth sichtbar ist. Bei Android Marshmallow reicht es, dafür in die Einstellungen zu gehen. Ein erneuter Scan sollte jetzt ein zusätzliches Gerät (Das Smartphone) darstellen:
```
hcitool scan
7C:2F:BE:EF:FA:CE Ole
00:04:BE:EF:FA:CE SHIELD
B8:86:BE:EF:FA:CE Fette Glotze
88:53:BE:EF:FA:CE TOMATO-PC
48:44:BE:EF:FA:CE TVBluetooth


F8:95:C7:H0:0H:0H G4
```

Die Mac-Adresse des Telefons sollte jetzt notiert werden. Ein pairen des Geräts ist nicht notwendig. Nach meinen Erfahrung benötigt dass zusätzliche Pakete und macht vor allem zusätzliche Probleme. Darauf kann und sollte man verzichten.

Mit Kenntnis der Mac-Adresse kann überprüft werden, ob das Gerät in Reichweite ist:
```
sudo hcitool info F8:95:C7:H0:0H:0H
```

Auch ungepaart verrät das Telefon nun einige Informationen:
```
Requesting information ...
 BD Address: F8:95:C7:H0:0H:0H
 Device Name: G4
 LMP Version: 4.1 (0x7) LMP Subversion: 0x6109
 Manufacturer: Broadcom Corporation (15)
 ```

Ist das Telefon hingegen nicht in Reichweite ist die Antwort eine andere:
```
Requesting information ...
Can't create connection: Input/output error
```

Das wars. Um das ganze jetzt automatisiert zu verwenden benötigt man nur noch etwas Bash-gescripte:
```
if hcitool info F8:95:C7:H0:0H:0H | grep -q 'Device Name'; then
 // Do whatever you like if you found it
fi
```

Viel Spaß damit
