const fillTable = function (satName, isEnabled, isFavorite, country) {
    let addToTable = `<tr>
                        <td>
                            <img src="https://flagcdn.com/16x12/${country}.png">
                        </td>
                        <td>
                            ${satName}
                        </td>
                        <td>
                            ${isEnabled}
                        </td>
                        <td>
                            ${isFavorite}
                        </td>
                    </tr>`;
    document.querySelector(".js-table").innerHTML += addToTable;
}

const addFavoriteApi = function (satId) {
    var requestOptions = {
        method: 'POST',
        redirect: 'follow'
    };

    fetch(`http://localhost:7071/api/v1/favorite/${satId}`, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));
}

const deleteFavoriteApi = function (satId) {
    console.error(satId);
}

const setEventListners = function () {
    planetFavList = document.querySelectorAll(".js-fav-icon");
    planetEnableList = document.querySelectorAll(".js-enabled-checkbox");

    planetFavList.forEach(element => {
        element.addEventListener("click", function () {
            if (this.getAttribute("data-satellite-isfavorite") == "false") {
                addFavoriteApi(this.getAttribute("data-satellite-name"));
            }
            else {
                deleteFavoriteApi(this.getAttribute("data-satellite-name"));
            }
        })
    });

    planetEnableList.forEach(element => {
        element.addEventListener("click", function () {
            var requestOptions = {
                method: 'PUT',
                redirect: 'follow'
              };
              
              fetch(`http://localhost:7071/api/v1/favorite/${this.getAttribute("data-satellite-name")}`, requestOptions)
                .then(response => response.text())
                .then(result => console.log(result))
                .catch(error => console.log('error', error));
        })
    });
}

const parseData = function (json) {
    console.log(json);

    json.forEach(element => {
        var fav = "";
        var en = "";

        if (element.properties.isfavorite) {
            fav = `<svg xmlns="http://www.w3.org/2000/svg" data-satellite-name="${element.properties.satelliteid}" data-satellite-isfavorite="${element.properties.isfavorite}" class="c-settings_fav-icon js-fav-icon" enable-background="new 0 0 24 24" viewBox="0 0 24 24"><g><path d="M0,0h24v24H0V0z" fill="none"/><path d="M0,0h24v24H0V0z" fill="none"/></g><g><path d="M12,17.27L18.18,21l-1.64-7.03L22,9.24l-7.19-0.61L12,2L9.19,8.63L2,9.24l5.46,4.73L5.82,21L12,17.27z"/></g></svg>`;
        }
        else {
            fav = `<svg xmlns="http://www.w3.org/2000/svg" data-satellite-name="${element.properties.satelliteid}" data-satellite-isfavorite="${element.properties.isfavorite}" class="c-settings_fav-icon js-fav-icon u-settings_fav-icon--no" enable-background="new 0 0 24 24" viewBox="0 0 24 24"><path d="M0 0h24v24H0V0z" fill="none"/><path d="M12 7.13l.97 2.29.47 1.11 1.2.1 2.47.21-1.88 1.63-.91.79.27 1.18.56 2.41-2.12-1.28-1.03-.64-1.03.62-2.12 1.28.56-2.41.27-1.18-.91-.79-1.88-1.63 2.47-.21 1.2-.1.47-1.11.97-2.27M12 2L9.19 8.63 2 9.24l5.46 4.73L5.82 21 12 17.27 18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2z"/></svg>`;
        }

        if (element.properties.isshown) {
            en = `<input class="js-enabled-checkbox" data-satellite-name="${element.properties.satelliteid}" data-satellite-isfavorite="${element.properties.isshown}" checked type="checkbox" />`;
        }
        else {
            en = `<input class="js-enabled-checkbox" data-satellite-name="${element.properties.satelliteid}" data-satellite-isfavorite="${element.properties.isshown}" type="checkbox" />`;
        }



        fillTable(element.properties.name, en, fav, element.properties.country);
        setEventListners();
    });
}

const getAllFavorites = function () {

}

const showSatelliteInfo = function () {
    var requestOptions = {
        method: 'GET',
        redirect: 'follow'
    };

    fetch("http://localhost:7071/api/v1/favorite", requestOptions)
        .then(response => response.text())
        .then(result => parseData(JSON.parse(result).features))
        .catch(error => console.log('error', error));
}