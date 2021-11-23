'use strict'

let map;
var markers = [];
var nssdcIdSateliteIdList = ['1998-067A', '2013-008A', '2009-005A', '1999-068A', '2002-022A', '2006-044A', '2011-061A', '2016-025A', '2018-076A', '2020-086A', '2020-061K'];

//! SET MARKERS
const placeMarkers = function (iconBase, icons, features, name, satId) {
  for (let i = 0; i < features.length; i++) {
    // Set marker
    const markerShadow = new google.maps.MarkerImage(
      "/IMG/Belgium.png",
      new google.maps.Size(20, 20)
    );

    const sateliteMarker = new google.maps.MarkerImage(
      "/IMG/satelite_big.png",
      new google.maps.Size(50, 50)
    );

    const marker = new google.maps.Marker({
      position: features[i].position,
      // icon: sateliteMarker,
      icon: icons[features[i].type].icon,
      //shadow: markerShadow,
      map: map,
      label: name,
      id: satId
    });

    // Add eventlistner marker
    marker.addListener("click", function () {
      showSateliteInfo(marker.id);
    })

    // markers.push(marker);
  }
}

//! INIT MAP
function initMap() {
  // 1. CALCULATE LOCATION
  // 2. INIT MAP
  // 3. Load sqtellites

  navigator.geolocation.getCurrentPosition(function (position) {
    let lat = String(position.coords.latitude);
    let long = String(position.coords.longitude);

    map = new google.maps.Map(document.getElementById("map"), {
      center: new google.maps.LatLng(lat, long),
      zoom: 5,
    });
  });

  loadSatellites();
}

const showSateliteInfo = function (dateliteId) {

}

//! SHOW SATELITE DATA ON MAP
const showSateliteOnMap = function (lat, long, name, satId) {
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

  console.log(satelite);

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

//! UPDATE MARKERS
const updateMarkers = function () {
  var requestOptions = {
    method: 'GET',
    redirect: 'follow'
  };

  nssdcIdSateliteIdList.forEach(element => {
    fetch(`http://aviation-edge.com/v2/public/satelliteDetails?key=15f8f4-e023f5&intldes=${element}`, requestOptions)
      .then(response => response.text())
      .then(result => function (result) {
        let satellite = JSON.parse(result)[0];

        for (let i = 0; i < markers.length; i++) {
          if (markers[i] == satellite.intldes) {
            markers[i].position = new google.maps.LatLng(satelite.result.geography.lat, satelite.result.geography.lon);
            console.log(`Update ${satelite.name}`)
          }
        }
      })
      .catch(error => console.log('error', error));
  });


}

const loadSatellites = function () {
  fetchApiData();

  // setInterval(updateMarkers, 30000);
}