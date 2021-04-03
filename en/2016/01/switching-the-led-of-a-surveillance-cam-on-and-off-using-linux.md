---
createnav: "0.0.1"
title: "Switching the LED of a surveillance cam on and off using Linux"
date: "2016-01-15"
categories: 
  - "linux-en"
  - "operating-systems"
tags: 
  - "raspberry-pi"
  - "synology"
---
# Switching the LED of a surveillance cam on and off using Linux
_Published:_ 15.01.2016 00:00:00

_Categories_: [linux-en](//en/categories#linux-en) - [operating-systems](//en/categories#operating-systems)

_Tags_: [raspberry-pi](//en/tags#raspberry-pi) - [synology](//en/tags#synology)


I already wrote a script on my Raspberry [that starts and stops the surveillance mode](http://dotnet.work/2016/01/automate-your-synology-surveillancestation-with-some-simple-linux-commands/) of my camera using the "Surveillance Station" on the Synology DiskStation. Now I wanted to extend that tool and light the LEDs of the cam when it is recording and turning them off if it isn't.

This has two reasons: First: I do not want to use the cam for "secret recordings", damaging the private spheres of my visitors. Second: I can easily see if the cam is working as expected. Now my cam has the option to activate the LEDs or deactivate from a web interface, but it is all-or-nothing. It does not depend if it is recording or not. Which isn't a surprise as the camera does not trigger the recordings itself but this is done by the Surveillance Station.

I own (and use) the "Robocam 4", sold by 7Links. But this hardware is quite common and used under different names, too.

Finding out the right commands is quite straight-forward: Open the webinterface and listen what happens there using the developer console in Chrome:

[![konsole](images/konsole.png)](http://dotnet.work/wp-content/uploads/2016/01/konsole.png)

The "Request URL" and "Form Data" are the most interesting informations here. Before entering that stuff into the Linux-shell let's test this with a more "comfortable" interface. The chrome extension "[Postman](https://chrome.google.com/webstore/detail/postman/fhbjgbiflinjbdggehcddcbncdddomop)" offers a nice way to play around with POST-Requests. The setting should be "x-www-form-urlencoded" here:

[![postman](images/postman.png)](http://dotnet.work/wp-content/uploads/2016/01/postman.png)

 

If everything works as desired it is time to put that command to Linux using shell commands. Especially easy if the come does not only accept POST-requests, but GET-requests, too.

http://192.168.0.88/admin/system.cgi?led=2

worked like a charm in my case.

The only thing that's left is the authentifivcation. If you receive a classic login-screen like from a .htaccess password-protection it can be easily added to the url. So
```
wget http://username:password@192.168.0.88/admin/system.cgi?led=2
```

turns on the LEDs
```
wget http://username:password@192.168.0.88/admin/system.cgi?led=0
```

turns them off

(0=off, 1=on, 2=blinking)

 

Just add these lines to your existing script and the LEDs should be work just like needed :)
