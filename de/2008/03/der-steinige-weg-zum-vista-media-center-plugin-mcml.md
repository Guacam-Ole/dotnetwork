---
title: "Der steinige Weg zum Vista Media Center PlugIn: MCML"
date: "2008-03-18"
categories: 
  - "c"
  - "windows"
tags: 
  - "mcml"
---

Nachdem ich leidvoll erfahren musste, dass XAML/WPF/XBAP in zukünftigen Media Center Editionen nicht mehr unterstützt wird, müssen wir wohl in den sauren Apfel beißen und mit MCML vorlieb nehmen. Doch was ist MCML eigentlich?

## MCML

MCML (Media Center Markup Language) ist eine Beschreibungssprache, die – wie etwa auch XAML – auf XML aufbaut. Eine sehr einfache MCML sieht z.B. so aus:

```
<Mcml xmlns="[http://schemas.microsoft.com/2006/mcml](http://schemas.microsoft.com/2006/mcml)"xmlns:cor="assembly://MSCorLib/System"><UI Name="Default"><Properties><Color Name="MyColor" Color="White"/>Properties><Locals><cor:StringName="MyString" String="Machts gut und Danke für den Fisch"/>Locals><Rules><Default Target="\[MyText.Content\]" Value="\[MyString\]"/>Rules><Content><TextName="MyText" Color="\[MyColor\]" Font="Calibri, 24"/>Content>UI<Mcml>
```

Am Anfang steht der XML Namespace. (xmlns). Der erste ([http://schemas](http://schemas/)…) muss immer so lauten und identifiziert das Dokument als MCML-Dokument. Der zweite angegebene Namespace mit der (im Dokument) internen Bezeichnung “cor” verweist auf _MSCorLib/System_. Dies sind ist der Namespace für die grundlegenden Klassen, wie etwa _String, int_, etc. also – vereinfacht ausgedrückt – all das, was man in C# zur Verfügung hat, ohne weitere Namespaces einzubinden.Als nächstes folgt das UI Element: UI steht für “User Interface”. Im Prinzip besteht eine MCML-Datei immer aus einem User Interface. Es gibt auch noch ein paar wenige, andere mögliche Elemente, aber diese sind erst einmal uninteressant. Das User Interface wiederum ist nur ein Container für die eigentlich darzustellenden Elemente. Vergleichbar wäre dies etwa mit dem _BODY_\-Tag auf einer HTML-Seite oder _WINDOW_ in einer XAML-Datei.

Da wir nicht nur malen wollen, sondern irgendwann auch auf die Elemente zugreifen wollen sollten alle Elemente einen eindeutigen Namen bekommen. In diesem Fall hat das UI den Namen “_Default_“.

Es folgen nun die Properties. Properties sind **globale** Variablen. Sie gelten nicht nur in der aktuellen MCML, sondern in der gesamten Instanz. Änderungen an Properties werden automatisch abgespeichert und sind beim nächsten Start wieder verfügbar.

In diesem Beispiel setzen wir die Property “MyColor” auf “White” Es folgen die “Locals”. Der Locals-Bereich definiert alle **lokalen** Variablen, die für die UI gelten. Im obigen Beispiel ist dies der String “MyString”.

Im Anschluss folgen die “Rules”, die Regeln. Im Prinzip ist dies der (vereinfachte) Programmiercode, der hinter dem Fenster steckt. Eine Regel ist im Prinzip immer vom Format bzw. _IF (Bedingung) THEN (Aktion)._ Es gibt einige Arten von Bedingungen, die uns noch im Laufe des Projekts über den Weg laufen werden. Eine dieser Standardbedingungen ist “_Default_“: Diese Bedingung besagt, dass diese Bedingung immer erfüllt ist.  Sie ließe sich z.B. mit _IF (1==1)_ übersetzen. Im obigen Beispiel weise ich also standardmässig _MyText.Content_ den Wert von_MyString_ zu.

Im C# – Code wäre die Entsprechnung für die bisherigen Markup-Elemente also folgende:

```
partialclass UI\_Default { 
  public Color MyColor=new System.Color.White;
  private string MyString="Machts gut und Danke für den Fisch"; 
  If (1==1) { 
    MyText.Content=MyString; 
  } 
}
```

Man sollte sich nicht dadurch verwirren lassen, dass “MyText” an dieser Position eigentlich nicht deklariert ist. Wir befinden uns schließlich nicht wirklich in einer Klasse oder Methode, sondern in der MCML-Definition. Vergleichbar ist dies mit einer WinForms-Anwendung. Auch hier kann ich im Quellcode auf die GUI-Element zugreifen, weil die Elemente “versteckt” in der Datei klasse.Designer.cs vorhanden sind. (Um dies zu verdeutlichen habe ich auch als Beispiel die “**_partial_** _class UI\_Default_” verwendet.Natürlich kann man alles, was im “Rules”-Bereich steht stattdessen auch direkt im Code erledigen. Allerdings hat die Verwendung von “Rules” drei klare Vorteile:

1. können Änderungen ohne Neucompilierung des Projekts durchgeführt werden
2. kann dem (versierten) Anwender erlaubt werden, einfache Änderungen an der MCML durchzuführen, um das UI-Objekt seinen Wünschen anzupassen.
3. bieten sich noch viel mehr Möglichkeiten, als hier (bisher) aufgezeigt, z.B. sehr angenehmes DataBinding, welches sich kaum vom DataBinding in XAML unterscheidet.

Bleiben nur noch die sichtbaren Elemente, hier bisher nur der Text MyText mit den Eigenschaften MyColor und der festen Fonteigenschaft. (Der Content wurde ja bereits im Rules-Bereich festgelegt).

Deutlich zu erkennen ist, dass der Text ohne unser Zutun zunächst horizontal und vertikal zentriert wurde. Wird das Fenster verkleinert fallen weitere Sachen auf, z.B. dass am Rand der Text automatisch “verblasst”.

So. Das war doch mal was. Eine ganze Seite lesen, nur für ein popeliges “Hallo Welt”
