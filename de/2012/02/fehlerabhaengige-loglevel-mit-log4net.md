---
createnav: "0.0.2"
title: "Fehlerabhängige Logdetails mit Log4net"
date: "2012-02-27"
categories: 
  - "c"
  - "programmierung"
tags: 
  - "log4net"
---
■ [.net.work](/) » [de](/de) » [2012](/de#2012)  » 2 » Fehlerabhängige Logdetails mit Log4net

# Fehlerabhängige Logdetails mit Log4net
_Published:_ 27.02.2012 00:00:00

_Categories_: [c](/de/categories#c) - [programmierung](/de/categories#programmierung)

_Tags_: [log4net](/de/tags#log4net)


Als Entwickler kennt man das Problem: In der Startphase will man wirklich jede Variable loggen, um zu sehen,wie das System arbeitet. Nach einer Weile läuft das System stabil und man reduziert die Logausgaben auf Info oder sogar auf Warn.

Kommt es nun zu einem Fehler, wäre es dann doch wieder ganz nett kurz vor dem Fehler zu sehen, welche Debug-Ausgaben es denn gegeben hätte. Log4Net schafft dies bereits mit Bordmitteln; Überraschenderweise scheint niemand dies bisher genutzt zu haben, oder dies niedergeschrieben zu haben. Also hole ich dies hiermit mal nach 

Folgende Konfiguration sorgt dafür, dass im Normalfall nur Info-Meldungen in die Logfile geschrieben werden. Tritt ein Fehler auf, werden zusätzlich Debug-Ausgaben geloggt:
```
 <appender name="RollingFile" type="log4net.Appender.RollingFileAppender"> 
   <file value="D:Logscodelog.log"></file> 
   <appendtofile value="true"></appendtofile> 
   <maximumfilesize value="10000KB"></maximumfilesize> 
   <maxsizerollbackups value="50"></maxsizerollbackups> 
   <layout type="log4net.Layout.PatternLayout"> 
     <conversionpattern value="%d \[%t\] %-5p %c %L – %m%n"></conversionpattern> 
   </layout> 
 </appender> 
 <appender name="BufferedFileInfo" type="log4net.Appender.BufferingForwardingAppender">
 <bufferSize value="1" />
 <lossy value="true"/>
 <evaluator type="log4net.Core.LevelEvaluator">
 <threshold value="INFO" />
 </evaluator>
 <appender-ref ref="RollingFile" />
 </appender>
 
 <appender name="BufferedFileError" type="log4net.Appender.BufferingForwardingAppender">
 <bufferSize value="20" />
 <lossy value="true"/>
 <evaluator type="log4net.Core.LevelEvaluator">
 <threshold value="ERROR" />
 </evaluator>
 <appender-ref ref="RollingFile" />
 </appender>
 <root> 
   <appender-ref ref="BufferedFileInfo"> 
   <appender-ref ref="BufferedFileError"> 
</root> 
```
Zur Erklärung: Der BufferFileInfo hat einen Puffer von “1”, speichert also nur eine Zeile und schreibt alles vom Status “INFO” in die Logfile. Der BufferedFileError reagiert nur bei Fehlern und speichert die letzten 20 Log-Ausgaben, unabhängig vom Level. Beide senden die Ausgaben an “RollingFile”, der für das eigentliche Schreiben in die Logfile sorgt.
