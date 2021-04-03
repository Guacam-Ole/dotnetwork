---
createnav: "0.0.2"
title: "Raspberry Pi 4: usB first!"
date: "2020-05-27"
categories: 
  - "others"
---
■ [.net.work](/) » [en](/en) » [2020](/en#2020)  » 5 » Raspberry Pi 4: usB first!

# Raspberry Pi 4: usB first!
_Published:_ 27.05.2020 00:00:00

_Categories_: [others](/en/categories#others)


At the time of this writing the boot-from-usb has just been made available as a Beta feature for the Raspberry Pi 4.

There are a few tutorials on how to activate that feature but I had an additional requirement: Boot from USB first, then boot from SD which requires a few additional steps.

This document might still be useful if you just want the SD first approach. Just ignore the second last part in that case.

But before we begin:

Use at your own risk. We are writing data to the EEPROM here using beta contents and even manipulate configs for that. If something fails your pi might be bricked. Not even reinstall Linux will repair that.

Additional warning: Don't try this on a N00bs install.

Still here? So let's begin. (I will asume you use Raspbian Buster)

### Updating the Pi

Before we start we should always have the most current Linux version installed. So make sure your internet connection works and enter

`sudo apt-get update`

followed by

`sudo apt-get upgrade`

and in addition

`sudo rpi-update`

to update the firmware on the sdcard

### Switching to beta

now change rpi to switch to beta - state. (_If you are reading this from the future: Hi there! Cars still a thing? And maybe you don't need to switch to beta anymore_)

do this by using your favourite editor like vi or nano to change the rpi-config:

`sudo nano /etc/default/rpi-eeprom-update`

change the release-status from `critical` to `beta` and save the file.

### Flashing the EEPROM

Now check the most current version on your pi:

`ls /lib/firmware/raspberrypi/bootloader/beta/pieeprom*.bin`

At time of this writing the most current version there is 2020-05-15. So flash the eeprom with that version:

`sudo rpi-eeprom-update -d -f /lib/firmware/raspberrypi/bootloader/beta/pieeprom-2020-05-15.bin`

You will be thrown back to the prompt a while after. Now reboot your pi and hope nothing went wrong:

`sudo shutdown - r now`

now check if the version has been installed by typing

`sudo vcgencmd bootloader_version`

the response should display the date corresponding to your file. So something like `May 15`

### Changing the boot order

By default now the boot order will be SD first, USB second. So if you just want to throw away your SD and boot grom USB: Ignore this chapter. If you need a "hardware multiboot" (boot from USB if attached but still have a fallback SD in place if not) follow these instructions:

First make sure I did not lie about the default order and display the current config:

`sudo vcgencmd bootloader_config`

this should display some information about the configuration. Most interesting for us is the last part called `BOOT_ORDER`which will contain the entry

`BOOT_ORDER=0xf41`

where 4=USB and 1=SD. More details like network bo**ot can be found at the bootloader documentation.**

While this might look like just how we want it you have to read this right to left. So it is SD (1) first and USB (4) second. But we are about to change that.

First it is highly recommend to not fiddle with the official ROM files but make a copy of that. Let's do that:

`sudo cp /lib/firmware/respberrypi/bootloader/beta/pieeprom-2015-05-15.bin pieeprom.bin`

Now create a config from that rom:

`sudo rpi-eeprom-config pieeprom-usbfirst.bin >bootconf.txt`

You now created a config file containing the config-output which you can edit by nano again:

`sudo nano bootconf.txt`

and change the last line to:

`BOOT_ORDER=0xf14`

At the last step we now apply our changed config und update the eeprom again:

`  
sudo rpi-eeprom-config --out pieeprom-usbfirst.bin --config bootconf.txt pieeprom.bin  
sudo rpi-eeprom-update -d -f ./pieeprom-usbfirst.bin  
sudo shutdown -r now  
`

### Finalizing the USB drive

Now our raspberry can boot from USB (and boot from there first) but the linux on our USB does not really know how to handle that. This part is simple:

First you need to flash a Raspbian image onto your USB-drive (just like you would to with an SD-card) . Then attach and mount the usb drive and copy the most current firmware files from the /boot - directory of the sdcard to the Usb drive. Alternatively you can select those files from the [master-branch on github.](https://github.com/raspberrypi/firmware/tree/master/boot)

Have fun :)
