---
createnav: "0.0.1"
title: "LEDs einer Überwachungskamera per Linux ein- und ausschalten"
date: "2016-01-15"
categories: 
  - "linux"
  - "os"
tags: 
  - "raspberry-pi-de"
  - "synology-de"
---
# LEDs einer Überwachungskamera per Linux ein- und ausschalten
_Published:_ 15.01.2016 00:00:00

_Categories_: [linux](/dotnetwork/de/categories#linux) - [os](/dotnetwork/de/categories#os)

_Tags_: [raspberry-pi-de](/dotnetwork/de/tags#raspberry-pi-de) - [synology-de](/dotnetwork/de/tags#synology-de)


Da ich bereits per Shell-Script auf dem Raspberry automatisch die Kamera [ja nach Anwesenheit aktiviere](http://dotnet.work/2016/01/synology-surveillance-station-mit-einem-einfachen-shell-script-automatisieren/), wollte ich noch einen Schritt weiter gehen. Da es mir nicht um heimliche Aufnahmen geht, sondern um eine Sicherheitsüberwachung wollte ich deutlich sehen können, ob die Kamera aktiv ist oder nicht. Das Webinterface erlaubt es zwar generell, die LEDs zu deaktivieren oder zu aktivieren, dies ist aber immer unabhängig von der Aufnahme.

Aus diesem Grund habe ich das Script erweitert, dass die LEDs bei der Überwachung automatisch aktiviert werden und ohne Aktivierung aus sind. So habe ich ganz nebenbei auch eine schnelle Prüfung, ob das Script korrekt arbeitet.

Ich verwende die Robocam IV, die unter anderem von Pearl über den Namen 7Links vertrieben wird. Sie ist aber auch unter vielen anderen Namen erhältlich.

Das Vorgehen war dabei recht simpel: Die Einstellungen im Webinterface vornehmen und in Chrome mit F12 in der Entwicklungskonsole nim "Network"-Reiter nachschauen, was denn so passiert:

[![konsole](images/konsole.png)](http://dotnet.work/wp-content/uploads/2016/01/konsole.png)

Interessant ist hier vor allem die "Request URL" und die "Form Data". Bevor man sich nun an die Arbeit macht, das Ganze im Linux zu bearbeiten, kann man mit der Chrome-Extension "[Postman](https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop)" prüfen, ob ein manueller Aufruf genau so funktioniert. In Postman dazu die Einstellung "x-www-form-urlencoded" verwendet:

[![postman](images/postman.png)](http://dotnet.work/wp-content/uploads/2016/01/postman.png)

 

Klappt es, kann es eigentlich schon los gehen. In diesem Fall war es sogar besonders einfach, weil nicht nur ein POST von der Kamera akzeptiert wird, sondern auch ein GET-Request
```
http://192.168.0.88/admin/system.cgi?led=2
```
problemlos funktionierte.

Fehlt nur noch die Authentifizierung. Die erfolgt mit einem klassischen Login-Screen, wie man ihn etwa auch von einer .htaccess - Passwortabfrage kennt. Damit war der Befehl schnell gefunden.
```
wget http://username:password@192.168.0.88/admin/system.cgi?led=2
```
schaltet die LED an,
```
wget http://username:password@192.168.0.88/admin/system.cgi?led=0
```
schaltet sie aus.

(0=aus, 1=an, 2=blinken)

 

Diese Zeilen also ins bestehende Script eingefügt und alle Klarheiten sind beseitigt :)
