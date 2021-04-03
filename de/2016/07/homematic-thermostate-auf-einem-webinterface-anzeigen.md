---
createnav: "0.0.2"
title: "HomeMatic - Thermostate auf einem WebInterface anzeigen"
date: "2016-07-11"
categories: 
  - "javascript-programmierung"
  - "linux"
  - "os"
  - "php"
  - "programmierung"
tags: 
  - "fhem-de"
  - "home-automation-de"
  - "homematic-de"
  - "raspberry-pi-de"
---
■ [.net.work](/) » [de](/de) » [2016](/de#2016)  » 7 » HomeMatic - Thermostate auf einem WebInterface anzeigen

# HomeMatic - Thermostate auf einem WebInterface anzeigen
_Published:_ 11.07.2016 00:00:00

_Categories_: [javascript-programmierung](/de/categories#javascript-programmierung) - [linux](/de/categories#linux) - [os](/de/categories#os) - [php](/de/categories#php) - [programmierung](/de/categories#programmierung)

_Tags_: [fhem-de](/de/tags#fhem-de) - [home-automation-de](/de/tags#home-automation-de) - [homematic-de](/de/tags#homematic-de) - [raspberry-pi-de](/de/tags#raspberry-pi-de)


Dieses Tutorial setzt voraus, dass FHEM bereits installiert und konfiguriert ist und der Umgang mit AngularJS bekannt ist

Ich habe in vorherigen Beiträgen bereits beschrieben, wie ich den RaspBerry PI zur Heimautomation verwende. Ich [erkenne bereits das SmartPhone](http://dotnet.work/2016/02/smartphone-erkennung-unter-linux-mittels-bluetooth-raspberry-pi-2/), das [FitBit](http://dotnet.work/2016/02/fitbit-erkennung-unter-linux-raspberry-pi-2/) und kontrolliere [Webcam](http://dotnet.work/2016/01/synology-surveillance-station-mit-einem-einfachen-shell-script-automatisieren/) sowie die [Harmony Fernbedienung](http://dotnet.work/2016/01/logitech-harmony-hub-mit-dem-raspberry-linux-steuern/) mittels einiger Shell-Scripte und einem Webinterface.

Als nächsten Schritt wollte ich Heizungsthermostate von HomeMatic (z.B. erhältlich bei ELV) ansteuern und schick darstellen.

Genutzte Hardware:

- HomeMatic Funk-Konfigurationsadapter LAN (HM-CFG-LAN)
- HomeMatic Thermostat HM-CC-RT-DN
- Raspberry Pi V2.

Verwendete Software:

- HTML/CSS
- JavaScript
- AngularJS
- FHEM
- PHP (optional)

Bevor wir anfangen, solltest du sicherstellen, dass FHEM bereits korrekt funktioniert und die Thermostate ansteuert. Ich werde diese Schritte nicht genauer erläutern, da dafür bereits gute Tutorials existieren im [FHEM Wiki](http://www.fhemwiki.de/wiki/HM-CFG-LAN_LAN_Konfigurations-Adapter). 

Ich habe ein "universelle" PHP-Script zur Kommunikation mit FHEM geschrieben. Der PHP-Part ist nicht zwingend notwendig; Da ausschließlich GET-Requests verwendet werden ließe sich das auch komplett per JavaScript oder sogar per HTML-Links erledigen. Der Grund PHP zu verwenden war für mich, dass das Webinterface von "außen" - also über das Internet erreichbar ist, die eigentliche Steuerung aber nicht. Das ist ein gewolltes Verhalten, so dass ein Zugriff vom Client (JavaScript) nicht möglich ist. Innerhalb meines LANs über den Server (PHP) jedoch schon.

Der langen Rede kurzer Sinn, das (simple) PHP-Script
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
Um das Script anzupassen muss lediglich die IP-Adresse und evtl. der Port in der $host-Variable angepasst werden. Ob das Script funktioniert kann leicht getestet werden:

http://yourserver/fhem.php?action=get&sensor={sensorname}\_Clima&param=measured-temp

Ersetze "Sensorname" durch den namen des Sensors, wie er in FHEM angezeigt wird. Das Ergebnis sollte die aktuelle Temperatur des Sensors sein.

Zeigen wir nun die Daten auf der Website an. Damit es schick aussieht verwende ich die Gauges von [ng-Google-Chart](https://github.com/angular-google-chart/angular-google-chart)

Die HTML-Datei ist wie folgt:
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
Ein klein bisschen Erklärung:

getStats() lädt zu Beginn die Sensoren über FHEM.php. Die HTML-Seite verwendet Bootstrap (für das Responsive Design) und Font Awesome um das Batteriesymbol anzuzeigen. Beide verwenden CSS und die Seite sollte zunächst auch ohne diese Bibliotheken funktionieren.

Nun zum Angular-Controller:
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
In $scope.heaterSettings.allHeaters Müssen die Namen sämtlicher anzuzeigender Sensoren eingetragen werden.

 

Das war es bereits. Viel Spaß damit :)
