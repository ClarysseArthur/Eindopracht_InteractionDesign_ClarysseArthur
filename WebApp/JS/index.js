"use strict"

var map, geoJSONLayer, geo, satS, ren;
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
        geo = GeoJSONLayer;

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

        ren = renderer;

        geoJSONLayer = new GeoJSONLayer({
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

        geoJSONLayer.on("click", function () {
            console.error("end");
        })

        map.add(geoJSONLayer);  // adds the layer to the map
    });
}


const refresh = function(){
    console.log("click");
    map.remove(geoJSONLayer);

    const geoJSONLayer2 = new geo({
        url: satS,
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
        renderer: ren
    });

    map.add(geoJSONLayer2);
}


document.addEventListener("DOMContentLoaded", function () {
    navigator.geolocation.getCurrentPosition(function (position) {
        let lat = String(position.coords.latitude);
        let long = String(position.coords.longitude);

        if (document.querySelector(".js-index")) {
            satS = "https://satellite-id.azurewebsites.net/api/v1/favorites";
            initMap(lat, long, satS);
            showSatelliteInfo(satS, true);
        }
        else {
            satS = `https://satellite-id.azurewebsites.net/api/v1/above`;
            initMap(lat, long, satS);
            showSatelliteInfo(satS, false);
        }
    });
})