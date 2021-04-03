---
title: "Truncate für Sharepoint-Listen"
date: "2011-11-14"
categories: 
  - "c"
  - "programmierung"
  - "sharepoint"
tags: 
  - "sharepoint"
  - "splist"
---
# Truncate für Sharepoint-Listen
_Published:_ 14.11.2011 00:00:00

_Categories_:[c](/dotnetwork/de/categories#c) - [programmierung](/dotnetwork/de/categories#programmierung) - [sharepoint](/dotnetwork/de/categories#sharepoint)

_Tags_:[sharepoint](/dotnetwork/de/tags#sharepoint) - [splist](/dotnetwork/de/tags#splist)


Wir alle wissen, dass einige elementare SQL-Tasks in SharePoint schlicht und einfach nicht funktionieren. Eines dieser Dinge ist das leeren von Listen. Das löschen einzelner Zeilen dauert eine Ewigkeit.

Aus diesem Grund habe ich eine Methode geschrieben, die eine Liste komplett löscht und mit dem gleichen Schema unter gleichem Namen neu erstellt, also im Endeffect ein TRUNC durchführt:

```
///
/// Liste leeren
/// Liste
public static void TruncateList(SPWeb web, SPList list)
{
string title = list.Title;
string description = list.Description;
SPListTemplateType template = list.BaseTemplate;
list.Title = string.Format("BACKUP\_{0}", title);
list.Update();

// Neue erstellen:
Guid newListId = web.Lists.Add(title, description, template);
web.Update();
SPList newList = web.Lists\[newListId\];

// Felder übernehmen:
foreach (SPField field in list.Fields) {
if (!newList.Fields.ContainsFieldWithStaticName(field.StaticName))
{
newList.Fields.Add(field);
}
}
newList.Update();
foreach (string viewField in list.DefaultView.ViewFields.ToStringCollection())
{
newList.DefaultView.ViewFields.Add(viewField);
}
newList.DefaultView.Update();
// löschen alte Liste:
list.Delete();
}
```
