---
createnav: "0.0.1"
title: "Datensicherung mit Rapidshare"
date: "2008-09-19"
categories: 
  - "linux"
tags: 
  - "rapidshare"
---
# Datensicherung mit Rapidshare
_Published:_ 19.09.2008 00:00:00

_Categories_: [linux](//de/categories#linux)

_Tags_: [rapidshare](//de/tags#rapidshare)


Es ist deutlich einfacher RapidShare als Backup-Medium zu nutzen, als erwartet. Meine Variante will ich dem geneigten Linux-Admin hier mal kurz näher bringen. Ausgehend für dieses mini-Howto ist Debian Linux (Etch). In diesem Beispiel sichere ich meine MySQL-Datenbank nach Rapidshare.

1. Ich hab da mal was vorbereitet MySQL und Perl sollte bereits installiert sein. Fehlt eigentlich nur noch rar. Unter Debian kann man es einfach per apt-get installieren:

`apt-get install rar unrar`

1. Vorbereitung Zunächst lege ich einen Ordner für die Backups an:

`mkdir /down/rsbackup`

1. MySql-Backup

`mysqldump --user= --password= >$(date +"%Y-%m-%e")\_backup.sql`

Hiermit wird die komplette DB gesichert und in einer Datei mit Datumsinformationen im Dateinamen gespeichert.

1. RAR Diese Datei packen wir nun mit rar und vergeben auch gleich ein Passwort:

`rar a -p backup$(date +"%Y-%m-%e").rar  \*.sql`

Wichtig: nach dem “-p” KEIN Leerzeichen

1. Upload Zum Schluß muß die Datei nur noch hochgeladen werden. Hierfür gibt es ein fertiges Perl-Script bei Rapidshare. Am einfachsten direkt mit wget herunterladen:

`wget [http://images.rapidshare.com/software/rsapi.pl](http://images.rapidshare.com/software/rsapi.pl)`

Im Anschluss wird das Script dann ausgeführt:

`perl rsapi.pl backup$(date +"%Y-%m-%e").rar prem`

Und fertig.

Komplett läßt sich das ganze dann in ein Shell-Script packen (Berechtigungen setzen nicht vergessen) und dann z.B. per Cronjob aufgerufen werden:

`cd /home/rsbackup mysqldump --user= --password= >$(date +"%Y-%m-%e")\_backup.sql rar a -p backup$(date +"%Y-%m-%e").rar  \*.sql perl rsapi.pl backup$(date +"%Y-%m-%e").rar prem`

Viel Spaß beim Sichern
