---
createnav: "0.0.1"
title: "Emptying SharePoint lists the ultra fast way"
date: "2012-08-09"
categories: 
  - "development"
  - "sharepoint-en"
tags: 
  - "sharepoint-en"
  - "splist-en"
---
# Emptying SharePoint lists the ultra fast way
_Published:_ 09.08.2012 00:00:00

_Categories_: [development](/en/categories#development) - [sharepoint-en](/en/categories#sharepoint-en)

_Tags_: [sharepoint-en](/en/tags#sharepoint-en) - [splist-en](/en/tags#splist-en)


SharePoint is damn slow when deleting lines from lists. Each entry can take up to some seconds which isn’t nice when you have to delete thousands of lines.

Also no Trunc-Methode is available.

Other solutions found on the net creating xml-files to bulk delete aren’t very fast either.

That’s why I decided to write my own fast truncate-method for SharePoint-Lists.

**All content of that list will be deleted. Use at your own risk!**

_What it does_:

First a template of that list is being created. After that the list is deleted completely. Than a new list using that template is being created and at the end that template is being deleted.

Works damn fast (just a few seconds even on big lists). IF anything goes wrong you can use the template to create a list manually.

 
```
public void TruncateList(Guid listID)
{
SPSecurity.RunWithElevatedPrivileges(delegate()
{
using (SPSite site = new SPSite(SPContext.Current.Site.ID))
{
using (SPWeb web = SPContext.Current.Web)
{
try
{
web.AllowUnsafeUpdates = true;
// alte Werte merken
SPList listCopy = web.Lists\[listID\];
string title = listCopy.Title;
string description = listCopy.Description;

string backupName = string.Format("BACKUP{0}{1:d} {1:HH}{1:mm}", title,DateTime.Now);

// save template
listCopy.SaveAsTemplate(backupName, backupName, string.Empty, false);
SPListTemplate template = web.Site.GetCustomListTemplates(web)\[backupName\];

// delete list
listCopy.Delete();
web.Lists.Add(title, description, template);

//  remove template
SPList gallery = web.Lists\["List Template Gallery"\];
foreach (SPListItem item in gallery.Items.Cast().Where(item => item.Title == backupName))
{
item.Delete();
break;
}

web.Update();
web.AllowUnsafeUpdates = false;
}
catch (Exception ex)
{
\_log.Error(ex);
throw;
}
}
}
}
);
}
```
