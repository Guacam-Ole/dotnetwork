---
title: "Setting single Panels on NanoLeaf using Home Assistant"
date: "2024-1-20"
categories: 
  - "homeautomation"
tags:
  - "homeassistant"
  - "homeautomation"
  - "nanoleaf"
---
■ [.net.work](/) » [en](/en) » [2023](/en#2023)  » 12 » How to track a squirrel

# Setting single Panels on Nanoleaf using Home Assistant

I went deep into Home Assistant automation currently and - as everybod knows who ever went this road - more ideas came up. So my newest idea was to use the Hex-Panels from Nanoleaf to display some information. For example if the washing machine is ready, I have an appointment, the Smartphone Battery gets low and so on.

![NanoLeaf with different Colors on the wall](https://github.com/Guacam-Ole/dotnetwork/assets/4692497/eaa1c25f-8b92-420c-b71d-0eee8d6758ab)


This entry describes how I managed to do that.

_I collected all this information from different sources. THe most important was from sygys who posted the Home Assistant configuration: https://community.home-assistant.io/t/nanoleaf-per-tile-light-control-build-in-ha/442720
By the API documentation alone I would not have been able to achieve that._

## The hardware
I mostly used Nanoleaf Elements in the wooden design. These cannot display any colors but just lighten up. I like the descent appearance of those. Do not want to create an all-rgb-gaming-experience in my living room. I also added three dark black hexagon tiles to it. Those where meant to display the status if needed.
BTW: The nanoleaf app does not offer any color scenes in this setup because the wooden panels are the "main" panels. But that does not matter for this scenario.

## Integrating in Home Assistant
For the main features I used the standard integration for Home Assistant. That allows detecing swipe controls (another advantage of the wooden panels because you don't see fingerprints there) and start mostly anything what the app allows. Which does NOT include adressing single panels

## Using the API 
Currently this can only be done by a REST-Api. Initially you need any tool that can send POST - Commands. You can use Curl if you like to work on the shell, or UI-Tools like Postman. 

### Retrieving a secret
The first manual step is to obtain a secret. This is done exactly like when installing the Home Assistant integration. You just pair the Panels by pressing the power button for a few seconds until the LEDs start blnking/moving. Don't worry, this will NOT remove any previous pairings. 
Now while the pairing is active call the following URL:

`POST http://192.168.178.65:16021/api/v1/new`

(replace the IP by the Nanoleaf IP. Should be visible in your Router for example)

This will return just a simple value which contains your secret token. This token will remain valid even if you unpower the Nanoleaf. Make sure you store this secret somewhere, you will need it. 

### Retrieve the Configuration
Now it is time to test your secret and retrieve the configuration from your Nanoleaf panels. You even can do this in your browser, or still in Postman/Curl:

`http://192.168.178.65:16021/api/v1/thisisthemagicsecretreturnedbefore/`

The most interesting part is the layout/positionData-Part in that response:
```
      "layout": {
            "numPanels": 46,
            "sideLength": 0,
            "positionData": [
                {
                    "panelId": 8172,
                    "x": 301,
                    "y": 747,
                    "o": 360,
                    "shapeType": 15
                },
                {
                    "panelId": 36653,
                    "x": 276,
                    "y": 703,
                    "o": 300,
                    "shapeType": 15
                },
[..]
```
I honestly was a bit surprised, because I do NOT have 46 panels. Far from it. The simple truth is: THe wooden Panels have 6 LEDs. each of them has their own ID. So having 7 wooden panels that makes 42 Ids, additional 3 Ids for the three black panels and the control buttons is an additional panel (with id 0)

If you do not need to adress all Panels but just a single one, a good hint is to just rotate one of the panels, make that call again and see which panel has a new orientation ("o" in the json)

Now you have to write down all PanelIds. At least those you want to modifiy. Because I mostly want to use the black panels I split the following part in two, but you can also work with one (or even more).

### Configure Rest in Home Assistant
Go to your beloved Home Assistant Configuration (configuration.yaml) and add the following entries:
```
rest_command:
  nanoleaf_wood:
    url: http://192.168.178.65:16021/api/v1/thisisthemagicsecretreturnedbefore/effects
    method: PUT
    payload: >
      { "write" : {"command": "display", "animType": "static", "animData":
      {%- set all = [ 8172,36653,  32365,61100,6889,35368,57498,28763,33051,4570,9112,45913,61802,25003,37099,42,14434,43171,34741,29680,58161,4721,33456,45298,4960,8482,45539,16547,53346,9255,5361,58801,30064,33077,4596,57524,54769,50577,21840,42000,13521,1683] %}
      {%- set ns = namespace(panels=[panels | count | string]) %}
      {%- for panel in panels %}
      {%- set ns.panels = ns.panels + [ '{} 1 {} {} {} 0 20'.format(all[panel.number-1], panel.r, panel.g, panel.b) ] %}
      {%- endfor %}
      "{{ ns.panels | join(' ') }}",
      "loop": false, "palette": [], "colorType": "HSB"}
      }
    content_type: "application/json"
  nanoleaf_black:
    url: http://192.168.178.65:16021/api/v1/thisisthemagicsecretreturnedbefore/effects
    method: PUT
    payload: >
      { "write" : {"command": "display", "animType": "static", "animData":
      {%- set all = [ 52732, 44680, 17782 ] %}      
      {%- set ns = namespace(panels=[panels | count | string]) %}
      {%- for panel in panels %}
      {%- set ns.panels = ns.panels + [ '{} 1 {} {} {} 0 20'.format(all[panel.number-1], panel.r, panel.g, panel.b) ] %}
      {%- endfor %}
      "{{ ns.panels | join(' ') }}",
      "loop": false, "palette": [], "colorType": "HSB"}
      }
    content_type: "application/json"
``` 
Replace the IP with your Nenoleaf - IP and the secret with your secret (before "/effects"). It is up to you if you want one or more different calls. The numbers in the "set all" - part have to be replaced by the `panelId`s you retrieved earlier.

What comes next is of course testing your config and then restarting Home Assistant.

### Automation Examples

Now you can use this in any automation. The number is the number in the array, starting at 1(!) Example for the simple ones:
```
alias: Nano_BlackTest
description: ""
trigger: []
condition: []
action:
  - service: rest_command.nanoleaf_black
    data:
      panels:
        - number: 1
          r: 255
          g: 0
          b: 0
        - number: 2
          r: 0
          g: 255
          b: 0
        - number: 3
          r: 0
          g: 0
          b: 255
mode: single
```

and an example to light up the three wooden panels (remember. Each panel has 6 LEDs):
```
alias: Nano_WoodTest
description: ""
trigger: []
condition: []
action:
  - service: rest_command.nanoleaf_wood
    data:
      panels:
        - number: 31
          r: 255
          g: 255
          b: 255
        - number: 32
          r: 255
          g: 255
          b: 255
        - number: 33
          r: 255
          g: 255
          b: 255
        - number: 34
          r: 255
          g: 255
          b: 255
        - number: 35
          r: 255
          g: 255
          b: 255
        - number: 36
          r: 255
          g: 255
          b: 255
        - number: 19
          r: 255
          g: 255
          b: 255
        - number: 20
          r: 255
          g: 255
          b: 255
        - number: 21
          r: 255
          g: 255
          b: 255
        - number: 22
          r: 255
          g: 255
          b: 255
        - number: 23
          r: 255
          g: 255
          b: 255
        - number: 24
          r: 255
          g: 255
          b: 255
        - number: 1
          r: 255
          g: 255
          b: 255
        - number: 2
          r: 255
          g: 255
          b: 255
        - number: 3
          r: 255
          g: 255
          b: 255
        - number: 4
          r: 255
          g: 255
          b: 255
        - number: 5
          r: 255
          g: 255
          b: 255
        - number: 6
          r: 255
          g: 255
          b: 255
mode: single

```

That's it. Have fun with your panels-





