---
title: "How to track a squirrel"
date: "2023-12-17"
categories: 
  - "csharp"
  - "development"
tags:
  - "mastodon"
  - "bot"
  - "synology"
  - "socialmedia"
---



Let's start by telling you that this whole project ist utter nonsense. Noone really *needs* this feature. I decided to create this nonetheless.

## The Goal
I own a little feeding box for squirrels of the neighbourhood. And it works as designed: Squirrels get here, steal their food and run away. 

I can see them through my glass door and it just looks amazing, especially in the typical "Hamburger Wetter" (Hamburg weather, means: raining horizontally). Sometimes I take a picture or even a video, but through the door that just does not look very great.

So I decided to add a surveillance cam to the mix, and while I am on it: Automatically publish whenever a squirrel appears

### Hardware
I use a dlink - outdoor webcam with WiFi. Not really worth looking up the model number here, just the cheapest WiFi-Cam I could find which is compatible with Synology Surveillance Station (but nearly all are)

In addition the Synology NAS with installed surveillance station. I own a DS920+, but any should do

### Privacy
I do not want to track any persons. Not me, not others. This is not meant to be surveillance, it is meant to film squirrels. TBH I did not ask the squirrels how they think about privacy and GDPR, but I assume they are fine with it. After all they accepted all Cookies until now, and not only the required ones.

To make sure only squirrels (or other animals) get caught I wanted to do a few conditions: 
* Only track movement directly in front of the feeder. 
  * So I can make sure not Humans or Traffic in the background trigger the motion sensor
* Put the Camera in "Squirrel"-height.
  * So even if someone (me or my guests) manages to trigger the alarm no faces can be seen
* Have a simple to use approval workflow
  * Just in case. If someone ends up lying there drunk I do not want to publish a video of that person anywhere

#### Only Track movement directly in front of the feeder
That is one of the reasons why I use the Surveillance Station of Synology. The Camera itself has movement detection and (according to the documentation) even a fancy AI "this is a person" algortihm. I do not want the last for obvious reasons and especially can not decide which area of the image should be used for detection. In addition this seems to require a cloud feature.
I do not want my camera feed in the cloud from anyone. This should never leave my house without my consent. The surveillance station can select very small pieces of the image to be selected as alert regions. I put it directly in front of the feeder where no background movement can be detected. This way I can also adjust the sensibility of the detection to quite a high value because no noise becaues of branches moving in the wind etc. is to be expected.

#### Put the camera in squirrel-height
That was easy as there are some flowerboxes directly before the feeder where I mounted the cam

[IMAGE HERE]

#### Have a simple approval workflow
Because I mostly am at Mastodon at time of this writing (these sentences sometimes age very bad. A few months ago that would have been the artist formerly known as Twitter, which itself was just a replacement for Google+ which, well, you know the sad story). Let's just hope that was the last Social Media switch for me.

One of the advantages of Mastodon is that it has a very nice, simple API that allows sending toots, getting Notifications and so on. So my idea for the workflow is to notify me (and only me) that a new movement has been detected. Attach an image of that movement and if I approve this (e.g. by adding a "fav" to that toot) the bot knows that this detection was a valid Squirrel video and posts it. So AI without the "A". (some might argue even without the "I"...)

## Synology
Adding the Webcam is quite simple. It nearly is found automatically in Surveillance station. Finding out how to connect was a bit harder. Username is always "admin" and the required password is a PIN. Printed onto the Power Plug, not the device...
After that the App of the camera does not like the Camera anymore, which is bonus for  me as I do not want images to be uploaded to dlink, anyways.

The detection area can be set in the Camera options. Fiddling with the thresholds takes a while, especially as I do not have a trained squirrel to assist me on that. I then changed the time plan to always motion detection (so disabling the constant recording) and updated the minimum record time to five minutes. A good comprimise between getting everything and not creating videos that are too big for mastodon/youtube/wherever I wanted to upload this, which I had not decided at this point

Synology has multiple ways to notify about movements: Email, SMS, Push notifications and Webhooks. Obviously the last one was the way to go as it does not spam me in any way and I have to program stuff anyways.

## Writing a Webhook
A webhook is just an HTTP-call essentially. Synology allows POST and PUT in Surveillance Station. So I created a simple Web Api in Visual Studio and was ready to debug the incoming data. 

### Firewalls, Firewalls everywhere!
Synology allows you to send a test-notification which just resulted in an error. WHAT exactly went wrong is nowhere to be seen. Not in surveillance station and not in the logs from the synology. So I had to fly blind at this point.

I knew it worked when using my local machine, either via localhost or the ipadress from my development machine.

So the next step was to connect to my synology via SSH and try to access my API throuh a simple webcall via GET:
`wget https://192.168.178.23:6789/ping`

This simple call already did not work. So must have been a firewall issue. I checked everything in my Synology configuration FOR A VERY LONG TIME and did find nothing. Finally I put my attention to Windows, disabled the Defender Firewall and it went through. Because I do not want to have the firewall open I added an exception through the "allow an app through windows firewall" and finally it did work with the firewall still enabled.

### Receiving data from the webhook

The Webhook arrived as expected, has some informations in it and can even provide an url to a preview image. That image URL is a generated "public" (within my LAN) Address I am able to receive and download.




Nur ein Test
