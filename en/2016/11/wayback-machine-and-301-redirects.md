---
createnav: "0.0.1"
title: "Wayback Machine and 301 redirects"
date: "2016-11-15"
categories: 
  - "others"
---
# Wayback Machine and 301 redirects
_Published:_ 15.11.2016 00:00:00

_Categories_: [others](/en/categories#others)


As you might know: the wayback machine from archive.org saves snapshots of websites. It does this automatically or can be forced to do so. However. If you ever had a 301 redirect on your site you are very much doomed.

Like browsers also the wayback-machine caches the redirect and does not continue to take snapshots from your sites. Even if you enter your url into the wayback-machine, you still will be redirected.

You can however force it to ignore the cache by manually store your site again:

https://web.archive.org/save/\[YOURSITE\]

for example:

https://web.archive.org/save/http://blathering.de

Et voílà: Everything works nice from now on.

 

\[Image is CC-BY-NC-ND © David Baldingerhttp://www.dbaldinger.com/opinion\_cartoons/second\_page/wayback.html\]
