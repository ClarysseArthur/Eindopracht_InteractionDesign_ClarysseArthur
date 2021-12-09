"use strict"

var map;
var settingsState = false;

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

const initMap = function (lat, long, satSource) {
    console.log(long);
    console.log(lat);

    require([
        "esri/config",
        "esri/Map",
        "esri/views/SceneView",
        "esri/layers/GeoJSONLayer",
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
            url: satSource,
            popupTemplate: {
                title: "Satelite info • {satName}",
                content: `
                        <div class="c-detail_master">
                            <div class="c-detail_child u-detail_child-title">Latitude:</div>
                            <div class="c-detail_child u-detail_child-data">{satLatitude}</div>

                            <div class="c-detail_child u-detail_child-title">Longitude:</div>
                            <div class="c-detail_child u-detail_child-data">{satLongitude}</div>

                            <div class="c-detail_child u-detail_child-title">Satellite ID:</div>
                            <div class="c-detail_child u-detail_child-data">{satID}</div>
                        </div>`
            },
            renderer: renderer
        });
        
        geoJSONLayer.on("click", function(){
            console.error("end");
        })
        
        map.add(geoJSONLayer);  // adds the layer to the map

    });
}



document.addEventListener("DOMContentLoaded", function () {
    document.querySelector(".js-burger").addEventListener("click", function(){
        console.log("Iets?!");
        document.querySelector(".js-settings").classList.toggle("c-settings_closed");
    })

    navigator.geolocation.getCurrentPosition(function (position) {
        let lat = String(position.coords.latitude);
        let long = String(position.coords.longitude);

        if (document.querySelector(".js-index")) {
            initMap(lat, long, "https://satellite-id.azurewebsites.net/api/v1/favorites");
            showSatelliteInfo("https://satellite-id.azurewebsites.net/api/v1/favorites", true);
        }
        else {
            initMap(lat, long, `https://satellite-id.azurewebsites.net/api/v1/above`);
            showSatelliteInfo(`https://satellite-id.azurewebsites.net/api/v1/above`, false);
        }
    });
})