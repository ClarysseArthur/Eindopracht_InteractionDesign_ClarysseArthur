'use strict'

let map;
var noradSateliteIdList = [ 25544, 39084, 36516, 33591, 29155, 25994, 27424, 38771, 37849, 40967 ];

//! SET MARKERS
const placeMarkers = function (iconBase, icons, features) {
  for (let i = 0; i < features.length; i++) {
    const marker = new google.maps.Marker({
      position: features[i].position,
      icon: icons[features[i].type].icon,
      map: map,
    });
  }
}

//! INIT MAP
function initMap() {
  // 1. CALCULATE LOCATION
  // 2. INIT MAP
  navigator.geolocation.getCurrentPosition(function (position) {
    let lat = String(position.coords.latitude);
    let long = String(position.coords.longitude);

    map = new google.maps.Map(document.getElementById("map"), {
      center: new google.maps.LatLng(lat, long),
      zoom: 8,
    });
  });

  // VARS
  const iconBase = "/IMG/";

  const icons = {
    satelite: {
      icon: iconBase + "satelite.png"
    }
  };

  const features = [
    {
      position: new google.maps.LatLng(-33.91721, 151.2263),
      type: "satelite",
    }
  ];

  placeMarkers(iconBase, icons, features);
}

