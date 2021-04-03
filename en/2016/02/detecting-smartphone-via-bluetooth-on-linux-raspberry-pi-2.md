---
title: "Detecting Smartphone via Bluetooth on Linux (Raspberry Pi 2)"
date: "2016-02-27"
categories: 
  - "others"
tags: 
  - "bluetooth"
  - "raspberry-pi"
---

After [detecting my phone using WiFi](http://dotnet.work/2016/01/automate-your-synology-surveillancestation-with-some-simple-linux-commands/) (which does only work under Marshmallow while the phone is loading or the display is enabled) and [detecting the fitbit device](http://dotnet.work/2016/02/tracking-fitbit-presence-under-linux-raspberry-pi-2/) I came back to my original task to detect my phone using Bluetooth.

 

While there are many documents out there that describe this on different Linux and even raspberry variants none of those really worked for me. The raspberry 2 (Debian Jessie) seem to be a bit different there.

First of all you need a Bluetooth USB dongle. I buyed the Logilink BT00015 (available at [amazon.com](http://www.amazon.com/gp/product/B0096Y2HFW/ref=as_li_tl?ie=UTF8&camp=1638&creative=19454&creativeASIN=B0096Y2HFW&linkCode=as2&tag=derstammti-21), [amazon.de](http://www.amazon.de/gp/product/B0096Y2HFW/ref=as_li_tl?ie=UTF8&camp=1638&creative=19454&creativeASIN=B0096Y2HFW&linkCode=as2&tag=derstammti-21) or elsewhere). I would expect that others work, too. But as always: On Linux, drivers can be tricky and it's always good to know working hardware.

Before installing drivers it is always a good idea to update everything:

```
sudo apt-get update
sudo apt-get upgrade
```

Then install the libraries needed for bluetooth

sudo apt-get install --no-install-recommends bluetooth

I received an error that some dependencies could not be resolved. I fixed that by entering the following commands:

```
sudo dpkg --configure -a
sudo apt-get install -f
```

Now insert the USB dongle and restart the raspberry

```
sudo shutdown -r now
```

To check if the USB-dongle is detected,  enter the following command:

lsusb

This lists all USB devices. There should be one offering Bluetooth - services

Now lets check if the Bluetooth itself works:

```
/etc/init.d/bluetooth status
```

The response should now contain the word "Running":
```
pi@raspberrypi:~ $ /etc/init.d/bluetooth status
● bluetooth.service - Bluetooth service
 Loaded: loaded (/lib/systemd/system/bluetooth.service; enabled)
 Active: active (running) since Sat 2016-02-27 19:31:45 UTC; 17min ago
 Docs: man:bluetoothd(8)
 Main PID: 7841 (bluetoothd)
 Status: "Running"
 CGroup: /system.slice/bluetooth.service
 └─7841 /usr/lib/bluetooth/bluetoothd
```

Now it's time to check for devices. Do nothing with your phone right now and enter:
```
hcitool scan
```

This should display all Bluetooth-devices in Range. As you haven't done anything with your phone yet it should not be displayed there. The output should be something like this:

```
Scanning ...
 7C:2F:BE:EF:FA:CE Ole
 00:04:BE:EF:FA:CE SHIELD
 B8:86:BE:EF:FA:CE Fette Glotze
 88:53:BE:EF:FA:CE TOMATO-PC
 48:44:BE:EF:FA:CE TVBluetooth
```
 

(I modified the MAC addresses. I am not absolutely sure if anyone could do evil things using the MACs, but I prefer not to find that out :) )

Now put your phone into discovery mode. On Android Marshmallow you just have to enter your Bluetooth-settings for this. Then re-scan your devices. There should be one more, now:
```
hcitool scan

7C:2F:BE:EF:FA:CE Ole
00:04:BE:EF:FA:CE SHIELD
B8:86:BE:EF:FA:CE Fette Glotze
88:53:BE:EF:FA:CE TOMATO-PC
48:44:BE:EF:FA:CE TVBluetooth

F8:95:C7:H0:0H:0H G4
```

Now just note your Mac-Address. We will **not** pair our device. That takes a few steps and additional packages and additional problems. Pairing is not needed once we know the mac-address.

Now we cam just check the presence by using the info command. Simply leave your Bluetooth-settings on the phone and enter
```
sudo hcitool info F8:95:C7:H0:0H:0H
```

Event though your phone isn't in discovery-mode anymore you will get some status information like the device name:
```
Requesting information ...
 BD Address: F8:95:C7:H0:0H:0H
 Device Name: G4
 LMP Version: 4.1 (0x7) LMP Subversion: 0x6109
 Manufacturer: Broadcom Corporation (15)
```

if you turn off Bluetooth on your phone and re-enter that command you will receive a different response:

```
Requesting information ...
Can't create connection: Input/output error
```

So that's it. Just use the good old grep in your bash-script to check whether the phone is there or not:
```
if hcitool info F8:95:C7:H0:0H:0H | grep -q 'Device Name'; then
 // Do whatever you like if you found it
fi
```

Again: Have fun :)
