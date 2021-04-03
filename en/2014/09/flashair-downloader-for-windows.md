---
createnav: "0.0.1"
title: "FlashAir Downloader for Windows"
date: "2014-09-25"
categories: 
  - "applications"
  - "csharp"
tags: 
  - "cs"
  - "flashair-en"
  - "flashairdownloader-en"
---
# FlashAir Downloader for Windows
_Published:_ 25.09.2014 00:00:00

_Categories_: [applications](/en/categories#applications) - [csharp](/en/categories#csharp)

_Tags_: [cs](/en/tags#cs) - [flashair-en](/en/tags#flashair-en) - [flashairdownloader-en](/en/tags#flashairdownloader-en)


\[bs\_notification type="warning"\]**Development stopped!** [Please read this](http://dotnet.work/2017/01/flashairdownloader-development-now-its-your-turn/) before downloading FAD\[/bs\_notification\]

\[bs\_notification type="info"\]**New Version available!**This version is obsolete. I would strongly suggest to use the FAD2 instead. You can download it here: http://dotnet.work/fad2.\[/bs\_notification\]

Toshibas FlashAir is an SD-Card with built-in WiFi. It allows you to download Pictures from your DigiCam through WiFi. Sadly there is no working Windows-Application.

The FlashAir-Downloader solves that issue. It allowes Bulk-Downloading of your images. Your pictures can be saved in folders created automaticly containing date-information.

Installation:

Unpack the Zip and start the Setup.Exe

Start the Application and go to Settings:

![flash1](images/flash1_qn247z.png)

Adjust the Settings if needed. Especially for “local Path”.

To use the Delete-Option you have to adjust the configuration of the SD-Card. Insert the SD-Card into your computer and open the file “CONFIG” file in the hidden “SD\_WLAN”-folder with a text editor. Add the line
```
UPLOAD=1
```

to the CONFIG-File.

Thats it. Close that window, turn on your camera and click onto “CONNECT”. If the connection works, you should see the folders and images of your SD-Card. Enter a folder or preview an image by double-clicking onto the icon. Mark the images you want to download and start downloading by clicking onto “Download”.

I give no guarantee about anything. This application shouldn’t do anything bad. But if your Camera explodes: Don’t blame me.This tool is and always will be free. But I don’t mind donations, of course :)

— Update V 1.1 —

I just updated the FlashAirDownloader to Version 1.1.

The following has changed:

- Bugfix: If the file date is quite old (<2000) it could not be downloaded. This has been fixed. \[\*Yeah! Year 2000 bug in 2014. Well done, Toshiba \*![:roll:](images/icon_rolleyes.gif)  ![:mrgreen:](images/icon_mrgreen.gif)  \]
- New Feature: You can now change the contents of the CONFIG – File directly. Just insert the SD-Card into your computer and Select “Card Properties [![fadConfig](images/fadConfig_nvxiz4.png)](http://res.cloudinary.com/dyzmmxp1s/image/upload/v1422648202/fadConfig_nvxiz4.png)
- New Feature: If you use more than just One FlashAir-Card you can now automatically create subfolders for each card. You can use the internal “CID” ID for that or the Application ID which may contain any custom name. ![fadconfig2](images/fadconfig2_arr8bm.png)

(Tested with Windows 7 and Windows 8.1)

A new Version is available: http://dotnet.work/fad2
