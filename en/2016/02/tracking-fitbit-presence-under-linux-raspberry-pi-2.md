---
title: "Tracking fitbit presence under Linux (Raspberry Pi 2)"
date: "2016-02-27"
categories: 
  - "linux-en"
tags: 
  - "fitbit"
  - "home-automation"
  - "raspberry-pi"
---

[As I wrote before](http://dotnet.work/2016/01/automate-your-synology-surveillancestation-with-some-simple-linux-commands/) I track the presence of my smartphone using WiFi. Since Android Marshmellow this doesn't work anymore, because when switching to idle mode, the WiFi is disconnected. The easiest way to get around this issue would be to use Bluetooth instead. And in fact: I already decided to go that way. But then my company gave me a nice present: A "Fitbit Charge HR". While my company assured, that the HR is not for Human Resource and does not try to track if I am programming enough I decided to give this a try.

Being a nerd I am not that much into that "fitness"-area. But I like the device as a small watch and the vibration alarm is nice too. I will surely sync this with my Appointments lateron. Getting some information about how many steps it takes to get the beer out of the cellar or the chips out of the kitchen is a nice benefit. Not really useful. But at least nerdy :)

So I plan to wear this device for a while. So it should be a good idea to use it for tracking my presence. Maybe in combination with a phone-detection as a fall-back. First I tried to use a standard USB-dongle to detect it. But as this device works on Bluetooth Low Energy (BLE) it cannot be detected with normal Bluetooth-Dongles.  So
```
hcitool scan
```

or even
```
hcitool lescan
```

did not receive any results. Luckily the Fitbit ships with an USB-dongle. And even better, there is a package called "[galileo](https://bitbucket.org/benallard/galileo)" which purpose is to sync the fitbit. Installation is easy:
```
sudo apt-get install python-usb python-requests
sudo pip install galileo
```

Now install the dongle and restart the raspberry.

If you enter the following command
```
sudo galileo -v
```

you should see something like

```
2016-02-27 16:28:21,875:INFO: 1 trackers discovered
```

All you have to do now to check if the fitbit is in range automaticly is some bash-magic:

```
if galileo -v | grep -q '1 trackers'; then
  // Do whatever you like if you found it
fi
```
