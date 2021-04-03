---
createnav: "0.0.2"
title: "Emptying SharePoint-List the (quite) fast way"
date: "2012-08-10"
categories: 
  - "development"
  - "sharepoint-en"
tags: 
  - "sharepoint-en"
  - "splist-en"
---
■ [.net.work](/) » [en](/en) » [2012](/en#2012)  » 8 » Emptying SharePoint-List the (quite) fast way

# Emptying SharePoint-List the (quite) fast way
_Published:_ 10.08.2012 00:00:00

_Categories_: [development](/en/categories#development) - [sharepoint-en](/en/categories#sharepoint-en)

_Tags_: [sharepoint-en](/en/tags#sharepoint-en) - [splist-en](/en/tags#splist-en)


I already offered a solution to [empty a SharePoint list at light speed](http://www.stammtischphilosoph.com/2012/08/emptying-sharepoint-lists-the-ultra-fast-way/ "Emptying SharePoint lists the ultra fast way")

But this solution hast two big disadvantages:

1. You cannot empty a list that has more entries thatn the allowed list view threshold (default is 5000)
2. All Lookups will be lost

This solution has non of those drawbacks. It is much slower than that solution but also much faster than “normal” deletion of SPListItems or using a Caml-Query to do that.

Just be asure that this is not thread-safe. So make sure noone is writing into that list in the same time or that lines might not be deleted.
```
private int rest;
public static void TruncateList(Guid listId, int limit)
{
SPSecurity.RunWithElevatedPrivileges(delegate()
{
using (SPSite site = new SPSite(SPContext.Current.Site.ID))
{
using (SPWeb web = SPContext.Current.Web)
{
try
{
\_rest = limit;
web.AllowUnsafeUpdates = true;
SPList listCopy = web.Lists\[listId\];

ContentIterator iterator = new ContentIterator();
iterator.ProcessListItems(listCopy, ProcessItem, ProcessError);

web.Update();
web.AllowUnsafeUpdates = false;
}
catch (Exception ex)
{
if (!ex.Message.Contains("Limit reached"))
{
Debug.WriteLine(ex);
}

}
}
}
}
);
}

public static bool ProcessError(SPListItem item, Exception ex)
{
if (!ex.Message.Contains("Limit reached"))
{
Debug.WriteLine(ex);
}

return true;
}

public static void ProcessItem(SPListItem item)
{
if (rest<=0)
{
Debug.WriteLine("Limit reached");
throw new Exception("Limit reached");
return;
}
item.Delete();
\_rest–;

}
```
