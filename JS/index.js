'use strict'

let map;
var nssdcIdSateliteIdList = ['1998-067A', '2013-008A', '2018-013A', '2009-005A', '2006-018A', '1999-068A', '2002-022A', '2006-044A', '2011-061A', '2015-058D'];

//! SET MARKERS
const placeMarkers = function (iconBase, icons, features, name, satId) {
  for (let i = 0; i < features.length; i++) {
    // Set marker
    const marker = new google.maps.Marker({
      position: features[i].position,
      icon: icons[features[i].type].icon,
      map: map,
      title: name,
      id: satId
    });

    // Add eventlistner marker
    marker.addListener("click", function() {
      console.log(marker.id);
    })
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
      zoom: 5,
    });
  });
}

//! SHOW SATELITE DATA
const showSateliteOnMap = function(lat, long, name, satId){
  const iconBase = "/IMG/";

  const icons = {
    satelite: {
      icon: iconBase + "satelite.png"
    }
  };

  const features = [
    {
      position: new google.maps.LatLng(lat, long),
      type: "satelite",
    }
  ];

  placeMarkers(iconBase, icons, features, name, satId);
}

//! SHOW SATELITE DATA
const showSatelite = function (jsonData) {
  let satelite = jsonData[0]

  // console.log(satelite);

  showSateliteOnMap(satelite.result.geography.lat, satelite.result.geography.lon, satelite.name, satelite.intldes);
}

//! FETCH API
const fetchApiData = function () {
  //TODO Vergeet niet ook nog "eigen" satelieten in de cookies te steken
  var requestOptions = {
    method: 'GET',
    redirect: 'follow'
  };

  nssdcIdSateliteIdList.forEach(element => {
    fetch(`http://aviation-edge.com/v2/public/satelliteDetails?key=15f8f4-e023f5&intldes=${element}`, requestOptions)
      .then(response => response.text())
      .then(result => showSatelite(JSON.parse(result)))
      .catch(error => console.log('error', error));
  });
}

//! DOM CONTENT LOADED
document.addEventListener("DOMContentLoaded", function () {
  fetchApiData();
})