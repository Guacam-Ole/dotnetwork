---
title: "Installation einer Softwareumgebung für PS3 homebrew für Linux-N00bs"
date: "2011-06-27"
categories: 
  - "linux"
tags: 
  - "linux"
  - "ps3"
---

Es gibt bereits einige Anleitungen im Internet, wie man eine eigene Linux-Distribution mit den nötigen Tools zum Entwickeln eigener Homebrew-Anwendungen erstellt. Leider hat bisher keine dieser Anleitungen bei mir wirklich funktioniert und auch die fertigen VMs wollten nicht so wirklich, wie sie sollten. Da ich selber eher ein Linux-Anfänger bin habe ich mal eine Anleitung geschrieben, die anderen Linux-Neulingen helfen soll nicht die gleichen Fehler wie ich zu machen.

Diese Anleitung funktioniert mit der derzeit aktuellen Ubuntu-Version und den derzeit erhältlichen aktuellsten ps3toolchains. Die Basis dieser Anleitung ist folgende: [Scognito](http://scognito.wordpress.com/2010/11/04/setup-a-build-environment-for-compiling-and-running-homebrew-for-ps3/ "Scognito")

Ich habe das Ganze in einer VM durchgeführt, ein echtes Linux sollte sich aber nicht deutlich unterscheiden.

Schritt 1: Download und Installation Ubunutu Derzeit ist die Version 11.04 aktuell, mit der ich alles folgende durchgeführt habe.  Bei langsamen Kisten oder in der VM (beides traf bei mir zu) sollte man im Anmeldebildschirm im unteren Bereich Ubuntu Classic (no effects) auswählen, zumal wir ohnehin fast nur in der Bash arbeiten.

Der nächste Schritt ist es die korrekte Tastatur einzustellen, wenn man kein amerikanisches Tastaturlayout benutzt. Dies erfolgt über_System/Preferences/Keyboard_.

Bevor es nun endlich losgeht habe ich alle installierten Module auf den neuesten Stand gebracht über _System/Administration/Update Manager_.

Nun gehts aber erst einmal los mit den notwendigen Paketen. Viele Anleitungen verlangen hier Pakete, die Ubunutu in der aktuellen Version nicht oder anders kennt. Ich habe die Pakete ab genannte Version angepasst. Wechselt aber erst einmal in die Bash über _Applications/Accessoires/Terminal_. (Ihr könnt auch ein Icon direkt per Drag/Drop auf den Desktop ziehen)

sudo apt-get install autoconf automake bison flex gcc make wget git libppl0.10-dev libcloog-ppl-dev libelf-dev libncurses5-dev texinfo build-essential libgmp3-dev python zlib1g-dev pkg-config libtool python-dev

(Das ist ein EINZEILER)

Merkt Euch schonmal, dass man hier ständig mit sudo arbeitet anstatt einmalig per su als root einzuloggen. Nach einer Weile mit Downloads und Installationen sollte dieser Schritt abgeschlossen sein. Wenn das geklappt hat freut Euch: Eure Ubuntu-Version scheint mit diesem Tutorial kompatibel zu sein ![:)](images/icon_smile.gif)

Als nächstes brauchen wir ein Verzeichnis für das ganze Playstation-Gelumpe. Einige Anleitungen empfehlen `~/dev/ps3` und so werde ich es auch tun:

`mkdir -p ~/dev/ps3`

Die Tilde am Anfang ist wichtig! Somit landet dieses Verzeichnis automatisch unter \*/home/(deinanmeldename)/dev/ps3 \*(Ich musste übrigens immer die Tilde (~) zweimal drücken bei der Installation in der VM)

Als nächstes werden ein paar Einstellungen in der Datei bash.rc fällig. Nehmt dazu Euren Lieblingseditor. Wer sich mit vi auskennt tippt folgendes ein:

`vi ~/.bashrc`

Wer sich **nicht** mit dem vi auskennt sollte dringendst die Finger davon lassen und entweder den DOS-ähnlichen Editor nano verwenden:

`nano ~/.bashrc` 

oder gleich auf den Grafischen Editor gedit ausweichen:

`gedit ~/.bashrc`

Wieder sind hier Tilde und . wichtig. Sollte sich eine neue Datei öffnen habt ihr Euch vertippt.

Fügt nun ganz am Ende folgende Zeilen ein:

```
export PS3DEV=$HOME/dev/ps3 
export PSL1GHT=$PS3DEV/psl1ght 
export PATH=$PATH:$PS3DEV/bin:$PS3DEV/ppu/bin:$PS3DEV/spu/bin:$PSL1GHT/bin 
export PS3LOAD=tcp:192.168.0.10 
```

In der letzten Zeile steht die IP-Adresse Eurer Playstation 3. Wenn Ihr sie derzeit nicht wisst: Das brauchen wir erst ganz am Ende. Lasst die letzte Zeile dann erstmal weg.

Damit diese ganzen schönen Einstellungen auch übernommen werden müssen sie aufgerufen werden:

`. .bashrc`

Ja. Da steht (Punkt)(Lehrzeichen)(Punkt)

Testet dies, indem Ihr \[bash\]echo $PSL1GHT\[/bash\]

eingebt. Dort sollte dann der Pfad _/home/(name)/dev/ps3/psl1ght_ auftauchen

Bisher haben wir nur Standard-Linux-Pakete installiert und ein paar Einstellungen vorgenommen. Jetzt aber holen wir uns endlich die ps3toolchain.

```
cd $PS3DEV 
git clone git://github.com/ooPo/ps3toolchain.git
```

Normalerweise würde man jetzt sicherstellen, dass genug Kaffee im Haus ist. In diesem Fall bleibt aber genug Zeit, neuen zu kaufen. Nach dem download wechselt Ihr ins neu erstellte ps3toolchain-Verzeichnis und führt das dortige Script aus:

```
cd ps3toolchain 
./toolchain.sh
```

Bei meiner alten Kiste dauerte dieser Schritt dann satte 12 Stunden. Bei schnelleren Rechnern wird es wohl eher um die 5-6 Stunden dauern. Hier darf eigentlich nix schieflaufen. Falls doch (bei mir lief z.B. die Festplatte voll) unbedingt das komplette ps3toolchain-Verzeichnis löschen, bevor Ihr das Script erneut startet.

Das Lange Warten hat einen Vorteil: Im Anschluß ist eigentlich alles komplett fertig. Testet am besten, ob alles korrekt läuft an den Beispielen. Wir fangen mal mit einem profanen Grafiktest an:

cd $PS3DEV/ps3toolchain/build/psl1ght/samples/graphics/videoTest 
make videoTest.pkg 

Im Anschluß liegen dann zwei Dateien im Verzeichnis: videoTest.pkg und videoTest.geohot.pkg. Installiert diese Dateien auf Eure PS3, Startet sie und betrachtet den wunderschönen Sonnenuntergang Farbverlauf.

Das wars eigentlich schon!
