---
title: "Nextcloud anstatt Google Drive/One auf der Synology. Schritt 1: OCR"
date: "2018-11-03"
categories: 
  - "allgemein"
  - "anwendungen"
---

Mein Entschluss stand fest: Anstatt Google (oder ein anderer Anbieter) wollte ich meine Daten selbst in der Cloud hosten. Die Lösung sollte da nicht (NUR) auf dem NAS selbst laufen, sondern ich wollte auch weiterhin direkt im Internet eine Kopie besitzen die einerseits Datenredundanz bietet und selbst dann verfügbar sein, wenn mein NAS explodiert und auch schnelleren und unkomplizierteren Zugriff von unterwegs erlauben als über den lokalen Internetprovider auf meine Synology.

Eines der Hauptgründe für Drive war bisher das automatische OCR: Ich scanne bereits jetzt sämtliche Dokumente ein, synchronisiere sie dann zu Drive und bekomme OCR automatisch. Das hilft ungemein auch alte Rechnungen, etwa für Garantiebelege oder die Lohnsteuererklärung zu finden.

Das bedeutet aber natürlich auch, dass Google exakt weiß, was in meinen Dokumenten steht. Sicher sein, dass damit kein Schindluder getrieben wird, kann man nicht. Ebenso wie bei anderen Anbietern. Die OCR-Funktion ist also einerseits ein wichtiges Kriterium, andererseits macht es mehr Sinn, das selbst zu erledigen. Optimalerweise genauso bequem wie bisher.

Als ersten Schritt brauche ich also ein OCR für die PDF-Dateien. Das soll von nun an direkt beim Einscannen auf dem NAS passieren. Zum Glück gibt es da einen fertigen DockerContainer. Einfach nach "OCRmyPDF" suchen und das Image herunterladen. Es muss KEIN Container erstellt werden. Dies geschieht durch das Script automatisch. Der Container ist unter dem Namen "ocr" zu finden. Dies wird nur bei Bedarf für jeden einzelnen OCR-Auftrag erledigt.

 

Und dafür ist ein kleines aber feines PHP-Script zuständig namens "[FileBasedMiniDMS](https://github.com/stweiss/FileBasedMiniDMS/blob/master/FileBasedMiniDMS.php)". Zugegeben: Das könnte man vermutlich auch über Shellscripte anstatt PHP machen, aber einem geschenkten Gaul und so 😎. Also flugs die beiden PHP-Dateien irgendwo auf das NAS gepackt. Auch wenn es PHP-Dateien sind, werden sie übrigens NICHT über den Webserver ausgeliefert. Sie sollten also nicht im Web-Verzeichnis liegen.

Die Konfiguration ist relativ simpel und erfolgt über die config.php. Folgende Werte sind dabei interessant:

\[bs\_row class="row"\]  
\[bs\_col class="col-md-6"\]$doRenameAfterOCR\[/bs\_col\]  
\[bs\_col class="col-md-6"\]Sorgt dafür, dass die Dateien nach der Regel "datum name tags" umbenannt werden. \[/bs\_col\]  
\[/bs\_row\]

\[bs\_row class="row"\]  
\[bs\_col class="col-md-6"\]$doTagging\[/bs\_col\]  
\[bs\_col class="col-md-6"\]Sorgt \*\*zusätzlich\*\* zum OCR dafür, dass Dokumente nach Tags einsortiert werden. Dafür muss der Dateiname aber bereits Tags enthalten. Nur in Kombi mit $tagsfolder . \[/bs\_col\]  
\[/bs\_row\]

\[bs\_row class="row"\]  
\[bs\_col class="col-md-6"\]$matchWithoutOCR\[/bs\_col\]  
\[bs\_col class="col-md-6"\]Gibt den Filter an, welche Dateien überprüft werden sollen. "\*" für alle Dateien eintragen.\[/bs\_col\]  
\[/bs\_row\]

\[bs\_row class="row"\]  
\[bs\_col class="col-md-6"\]$dockercontainer\[/bs\_col\]  
\[bs\_col class="col-md-6"\]Muss genauso lauten wie das Image. I.d.R. nicht anzupassen\[/bs\_col\]  
\[/bs\_row\]

\[bs\_row class="row"\]  
\[bs\_col class="col-md-6"\]$inboxfolder\[/bs\_col\]  
\[bs\_col class="col-md-6"\]Verzeichnis, in dem sich die zu prüfenden PDF-Dateien befinden. Unterverzeichnisse werden rekursiv durchsucht\[/bs\_col\]  
\[/bs\_row\]

\[bs\_row class="row"\]  
\[bs\_col class="col-md-6"\]$OCRPrefix\[/bs\_col\]  
\[bs\_col class="col-md-6"\]Wird nach dem OCR vor den Dateinamen geschrieben\[/bs\_col\]  
\[/bs\_row\]

\[bs\_row class="row"\]  
\[bs\_col class="col-md-6"\]$recyclebin\[/bs\_col\]  
\[bs\_col class="col-md-6"\]Hier werden nach dem OCR die Original-PDF dateien hin verschoben (inkl. der Verzeichnisstruktur). Wichtig: Bloß nicht als Unterverzeichnis der zu scannenden Dokumente, sonst wird es von dort erneut versucht zu OCRen. \[/bs\_col\]  
\[/bs\_row\]

Die Felder $renamerules und $tagrules sollte man anpassen, bzw. einfach leeren, dass dort nur noch "=array();" steht.

zum Testen kann man nun am Besten

php FileBasedMiniDMS.php -d -t >> ocr.log 2>&1

aufrufen. Das "tut so", als würde es alle Dokumente Scannen und loggt die Ausgabe. Wenn nichts gescanned wird, zusätzlich mit "-o" prüfen. Tauchen jetzt die Dateien auf, ist der Wert von $matchWithoutOCR nicht in Ordnung.

Läuft alles, können die Parameter (-d -o -t) entfernt werden. Neben einem Backup (sowieso) bietet es sich an, erst einmal in einem kleinen Verzeichnis zu testen. War auch das erfolgreich, muss das Script nur noch per Cronjob (oder über die Oberfläche als Scheduler) eingetragen werden und fertig.

Das Dateidatum der PDF-Datei bleibt übrigens erhalten.

Ein kleiner Hinweis zur Performance:

Auf meiner Synology DS218+ (Intel Celeron J3355/DualCore 2Ghz), die im Idle-Modus ca. 5% CPU verbraucht und 800MB von 10GB dauerte eine 5MB PDF-Datei (Dann unter CPU-Vollast bei kaum erhöhtem Speicherverbrauch) ca. 4 Minuten. Beim initialen OCR wird das also eine ganze Weile brauchen.
