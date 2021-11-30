"use strict"

var map;
var nssdcIdSateliteIdList = ['1998-067A', '2013-008A', '2009-005A', '1999-068A', '2002-022A', '2006-044A', '2011-061A', '2016-025A', '2018-076A', '2020-086A', '2020-061K'];
var satellites = {
    type: "FeatureCollection",
    metadata: {
        generated: 1637515625,
        title: "Satelites",
        status: 200
    },
    features: []
};
var satelliteJson;

const initMap = function (lat, long) {
    console.log(long);
    console.log(lat);

    require([
        "esri/config",
        "esri/Map",
        "esri/views/SceneView",
        "esri/layers/GeoJSONLayer",
        "esri/PopupTemplate"
    ], function (esriConfig, Map, SceneView, GeoJSONLayer) {

        esriConfig.apiKey = "AAPK8bcad090be7b4becbb6c3bdeb563bd5eUjQg07m8XSZNFJbkx7WLxPK9BoPB_avWjSsxJM2DZeXMPqhSD2DgkO5IsAP-c18v";

        map = new Map({
            // basemap: "osm-light-gray", //Basemap layer service
            basemap: "arcgis-imagery",
            ground: "world-elevation", //Elevation service
        });

        const view = new SceneView({
            container: "viewDiv",
            map: map,
            camera: {
                position: {
                    x: long, //Longitude
                    y: lat, //Latitude
                    z: 20000000 //Meters
                },
                tilt: 0
            }
        });

        const renderer = {
            type: "simple",
            field: "mag",
            symbol: {
                type: "simple-marker",
                color: "orange",
            },
            visualVariables: [{
                type: "size",
                field: "mag",
                stops: [{
                    value: 2.5,
                    size: "20px"
                },
                ]
            }]
        };

        const geoJSONLayer = new GeoJSONLayer({
            // url: 'https://satellite-id.azurewebsites.net/api/v1/satellitelocation/{"satellites": [{"name": "1998-067A"},{"name": "2013-008A"},{"name": "2009-005A"},{"name": "1999-068A"},{"name": "2002-022A"},{"name": "2002-022A"},{"name": "2006-044A"},{"name": "2011-061A"},{"name": "2016-025A"},{"name": "2018-076A"},{"name": "2020-086A"},{"name": "2020-061K"}]}',
            url: `http://localhost:7071/api/v1/favoriteenabled`,
            popupTemplate: {
                title: "Satelite info • {name}",
                content: `<div class="c-detail_master">
                            <div class="c-detail_child u-detail_child-title">Latitude:</div>
                            <div class="c-detail_child u-detail_child-data">{lat}</div>

                            <div class="c-detail_child u-detail_child-title">Longitude:</div>
                            <div class="c-detail_child u-detail_child-data">{lon}</div>

                            <div class="c-detail_child u-detail_child-title">Country:</div>
                            <div class="c-detail_child u-detail_child-data">{country} <img src="https://flagcdn.com/16x12/{country}.png"></div>

                            <div class="c-detail_child u-detail_child-title">Launch date:</div>
                            <div class="c-detail_child u-detail_child-data">{launchdate}</div>

                            <div class="c-detail_child u-detail_child-title">Satellite ID:</div>
                            <div class="c-detail_child u-detail_child-data">{satelliteid}</div>
                        </div>`
            },
            renderer: renderer
        });
        map.add(geoJSONLayer);  // adds the layer to the map
        console.warn("DONE");
    });
}

document.addEventListener("DOMContentLoaded", function () {
    navigator.geolocation.getCurrentPosition(function (position) {
        let lat = String(position.coords.latitude);
        let long = String(position.coords.longitude);

        initMap(lat, long);
        showSatelliteInfo();
    });
})