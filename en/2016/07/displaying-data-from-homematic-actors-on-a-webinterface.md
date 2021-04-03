---
createnav: "0.0.2"
title: "Displaying Data from HomeMatic Actors on a WebInterface"
date: "2016-07-11"
categories: 
  - "others"
tags: 
  - "fhem"
  - "homematic"
  - "raspberry-pi"
---
■ [.net.work](/) » [en](/en) » [2016](/en#2016)  » 7 » Displaying Data from HomeMatic Actors on a WebInterface

# Displaying Data from HomeMatic Actors on a WebInterface
_Published:_ 11.07.2016 00:00:00

_Categories_: [others](/en/categories#others)

_Tags_: [fhem](/en/tags#fhem) - [homematic](/en/tags#homematic) - [raspberry-pi](/en/tags#raspberry-pi)


This tutorial assumes that you already installed and configured FHEM and know how to use AngularJS.

 

As you might know I use my Raspberry PI to automate tasks at home. I already [detect my SmartPhone](http://dotnet.work/2016/02/detecting-smartphone-via-bluetooth-on-linux-raspberry-pi-2/), and my [FitBit](http://dotnet.work/2016/02/tracking-fitbit-presence-under-linux-raspberry-pi-2/) and also [control my Webcam](http://dotnet.work/2016/01/automate-your-synology-surveillancestation-with-some-simple-linux-commands/) and [Harmony remote](http://dotnet.work/2016/01/control-your-harmony-hub-with-a-raspberry-pi-linux/) using a raspberry and a web interface.

Now I also wanted to include actors from HomeMatic thermostats into my configuration and display it in a nice way.

Hardware used:

- HomeMatic Wireless Configuration Adapter LAN (HM-CFG-LAN)
- HomeMatic Thermostate HM-CC-RT-DN
- Raspberry Pi V2.

Software used:

- HTML/CSS
- JavaScript
- AngularJS
- FHEM
- PHP (optional)

Before we begin make sure you have already installed FHEM and the hardware for it. I will not discuss that here as there are a lot of good tutorials for that. A good (german) tutorial can be found in the [FHEM Wiki](http://www.fhemwiki.de/wiki/HM-CFG-LAN_LAN_Konfigurations-Adapter). (If you got any english documentation, feel free to post it in the comments.)

I wrote a "general" PHP script to send commands and receive commands from FHEM. This can also be done without PHP and pure JavaScript-Code. I chose this solution because my configuration website is accessable through the internet, but my FHEM isn't. So JS-Could (running on the client) would not work, but PHP (running on the server) does.

The PHP-Script:
```
<?php
 $host="http://192.168.178.36:8083/fhem?";

 $sensor= $\_GET\['sensor'\];
 $param= $\_GET\['param'\];
 $action= $\_GET\['action'\];

 if ($action=="get") {
 $get="detail=SENSOR&dev.getSENSOR=SENSOR&cmd.getSENSOR=get&arg.getSENSOR=param&val.getSENSOR=PARAM&XHR=1&addLinks=1";
 } else {
 $get="cmd=set%20SENSOR%20desired-temp%20PARAM&XHR=1";
 }

 $replaced=str\_replace("SENSOR",$sensor,$get);
 $replaced=str\_replace("PARAM",$param,$replaced);
 $total=$host.$replaced;
 
 $ch = curl\_init();
 curl\_setopt($ch, CURLOPT\_URL, $total);
 curl\_setopt($ch, CURLOPT\_RETURNTRANSFER, 1);
 $return = curl\_exec($ch);

 echo $return;

?>
```
The only thing you need to modify here is the IP-Adress (or hostname) and maybe the port from the $host - variable. As you can see, this is a simple GET-Request which could easily used through JavaScript (or even just inside a href) also.

You can test this script by simply calling it with some parameters:

http://yourserver/fhem.php?action=get&sensor={sensorname}\_Clima&param=measured-temp

Replace "Sensorname" by the name of your sensor. You should receive the current temperature of the sensor as a result.

Now it is time to display this data in a fancy way in html. Create any HTML - Page using AngularJS. To use these nice Gauges you need [ng-Google-Chart](https://github.com/angular-google-chart/angular-google-chart)

Now lets see the HTML-Part containing the AngularJS-Directives to display and set the thermostate values:
```
<html><body ng-app="piApp">
  <div class="container" ng-Controller="heatController" ng-init="getStats()">
        <div class="row">
            <div class="box">
                <div class="col-lg-12">
                    <hr>
                    <h2 class="intro-text text-center">Heizungs
                        <strong>Einstellungen</strong>
                    </h2>
                    <hr>
                </div>
                <div class="col-md-12">
                    <div ng-repeat="heater in heaterSettings.allHeaters" class="heater">
                        <div class="battery"><i class="fa fa-4x fa-{{heater.battery.icon}} {{heater.battery.color}}"></i> </div>
                        <div google-chart chart="heater.gauge"  ></div>
                        <div class="caption">{{heater.name}}<br/>
                            <select ng-model="heater.ui.wish" ng-change="setTemperature(heater.id,heater.ui.wish)">
                                <option ng-repeat="option in availableTemperatures()" value="{{option}}">{{option}}</option>
                            </select>
                        </div>
                    </div>
                </div>

        </div>
    </div>
</div></body></html>
```

Some explanation:

piApp is the AngularJS - App for the website, getStats() loads the current Sensors using FHEM.php mentioned above directly from the start. This html uses Bootstrap (for repsonsive design) and Font Awesome to display the Battery indicator. Both use CSS to achieve this. (The Gauge-Display works without these libraries)

The Angular-Controller looks like this:
```
angular.module('piApp', \['googlechart'\])
    .controller('heatController', function($scope, $http) {
        $scope.url='fhem.php?';
        $scope.heaterSettings={
            allHeaters:\[
                {name:'Wohnzimmer', id:'Wohnzimmer\_Clima', ui:{},battery:{},gauge:{}},
                {name:'Schlafzimmer',id:'Schlafzimmer\_Clima',ui:{},battery:{},gauge:{}},
                {name:'Bad',id:'Bad\_Clima',ui:{},battery:{},gauge:{}}
            \]
        };

        $scope.getParamUrl=function(sensor,param) {
            return "action=get&sensor="+sensor+"&param="+param;
        };
        $scope.setParamUrl=function(sensor,param) {
            return "action=set&sensor="+sensor+"&param="+param;
        };

        $scope.getBattery=function(sensor, target) {
            $http.get($scope.url+$scope.getParamUrl(sensor,'batteryLevel'))
                .success(function(data) {
                    $scope.setBattery(target,data);
                });
        };

        $scope.getStats=function() {
            $scope.heaterSettings.allHeaters.forEach(function (heater) {
                // gauge:
                heater.gauge={};
                heater.gauge.type="Gauge";
                heater.gauge.options = {
                    width: 250,
                    height: 250,
                    redFrom: 20,
                    redTo: 30,
                    greenFrom: 0,
                    greenTo: 10,
                    yellowFrom: 10,
                    yellowTo: 20,
                    minorTicks: 10,
                    majorTicks: \['0','5','10','15','20','25','30'\],
                    max:30,
                    animation:{
                        startup: true,
                        duration: 1000,
                        easing: 'inAndOut'
                    }
                };

                // Gemessene Temperatur:
                $http.get($scope.url+$scope.getParamUrl(heater.id,'measured-temp'))
                    .success(function(data) {
                        $scope.setData(heater.gauge,data);
                    });
                $http.get($scope.url+$scope.getParamUrl(heater.id,'desired-temp'))
                    .success(function(data) {
                        $scope.setWish(heater.ui,data);
                    });
                $scope.getBattery(heater.name,heater);
            });
        };

        $scope.setTemperature=function(sensor, temperature) {
            $http.get($scope.url+$scope.setParamUrl(sensor,temperature))
                .success(function(data) {
                    console.log("Temperatur von "+sensor+" auf "+temperature+" Grad eingestellt");
                    //$scope.getStats();
                })
                .error(function(data) {
                    alert("Fehlgeschlagen");
                }
            );
        };

        $scope.availableTemperatures=function() {
            var available=\[\];
            for(var i=5;i<=30;i=i+0.5)  {
                var iasString= i.toString();
                if (Math.round(i)===i) {
                    iasString+='.0'
                }
                available.push(iasString);
            }
            available.push("on");
            available.push("off");
            return available;
        };

        $scope.setData=function(target,data) {
            var value=parseFloat(data.replace(/(\\r\\n|\\n|\\r)/gm,""));
            target.data=  \[ \['Label', 'Value'\], \['', value\]\];
        };

        $scope.setWish=function(target,data) {
            var value=data.replace(/(\\r\\n|\\n|\\r)/gm,"").trim();
            target.wish=value;
        };
     
        $scope.setBattery=function(target,value) {
            var value=parseFloat(value.replace(/(\\r\\n|\\n|\\r)/gm,"").trim());
            var color='green';
            var icon='battery-full';
            if (value<2.1) {
                color="red";
                icon='battery-empty';
            } else if (value<2.3) {
                color="red";
                icon='battery-quarter';
            } else if (value<2.6) {
                color="yellow";
                icon='battery-half';
            }
            target.battery={
                value:value,
                color:color,
                icon:icon
            }
        };

    })
```

Explanation:

"googlechart" in the module-configuration allows to paint those fancy gauges.

in $scope.heaterSettings.allHeaters you have to modify the Names of your Sensors. Enter as many sensors as you want.

All other functions communicate with fhem.php or display values.
