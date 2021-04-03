---
createnav: "0.0.2"
title: "It's because they at Microsoft hate web developers"
date: "2016-06-14"
categories: 
  - "others"
tags: 
  - "office365"
  - "sharepoint-en"
---
■ [.net.work](/) » [en](/en) » [2016](/en#2016)  » 6 » It's because they at Microsoft hate web developers

# It's because they at Microsoft hate web developers
_Published:_ 14.06.2016 00:00:00

_Categories_: [others](/en/categories#others)

_Tags_: [office365](/en/tags#office365) - [sharepoint-en](/en/tags#sharepoint-en)


That remark isn't mine, it is from StackOverflow. :)

I had an issue that my Office365-App did not work correctly on the machine of ONE user. Sadly though that user was the Microsoft validation crew. I was unable to reproduce the error and did not even see any clue in the Server logs. Everything seemed to work fine.

Finally I managed to reproduce the error. It only happened under the following conditions: 1. Internet Explorer 11; IE - what else :) 2. Caching in IE has to be on "Automatic" 3. Developer Console MUST NOT be open 3.1. In fact: Even if you open it and close it afterwards, the error does not appear anymore 4. I had to retract/reinstall the app each time to check this behaviour which took about 10 minutes every time

I had issues with console on older IEs before, but at least angular uses some magic, so that issue is resolved. Nearly all questions in the web combining IE and console repeat the solution that you may not use "console.log()".

But the real issue was: Caching. IE - in its wisdom - decided that the API-Call that returns the installation state should be cached. That cached value always responded that the app is not installed, even if it was. So the user kept receiving the "please install" - Page.

Any other browser (Chrome, Firefox, ...) doesn't cache these calls. And the IE doesn't either when you open the Developer Console (what you do if you want to debug an application) which makes it really hard to track that error.

So the solution is to make sure the call isn't cached using No-Cache-Headers or just by adding a random value to the target address. And of course resist the urge to display a reminder to use another browser. That could be bad for the validation results :)
