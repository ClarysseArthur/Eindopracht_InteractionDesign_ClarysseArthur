const fillTable = function (satName, isEnabled, isFavorite, id) {
    let addToTable = `<div class="c-settings_child c-setting_child-name">
                            ${satName}
                        </div>
                        <div class="c-settings_child">
                            ${isEnabled}
                            <label class="c-label c-label--option c-custom-toggle" for="toggle${id}">
                                <span class="c-custom-toggle__fake-input js-toggle">
                                </span>
                            </label>
                        </div>
                        <div class="c-settings_child c-settings_child-star">
                            ${isFavorite}
                        </div>`;

    document.querySelector(".js-settings").innerHTML += addToTable;
}

const addFavoriteApi = function (satId) {
    var requestOptions = {
        method: 'POST',
        redirect: 'follow'
    };

    fetch(`https://satellite-id.azurewebsites.net/api/v1/favorites/${satId}`, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));

    let element = document.querySelector(`svg[data-satellite-name="${satId}"]`);
    element.classList.toggle("c-settings_fav-icon_fav");
    element.setAttribute("data-satellite-isfavorite", true);
}

const deleteFavoriteApi = function (satId) {
    console.error(satId);

    var requestOptions = {
        method: 'DELETE',
        redirect: 'follow'
      };

      fetch(`https://satellite-id.azurewebsites.net/api/v1/favorites/${satId}`, requestOptions)
        .then(response => response.text())
        .then(result => console.log(result))
        .catch(error => console.log('error', error));

    let element = document.querySelector(`svg[data-satellite-name="${satId}"]`);
    element.classList.toggle("c-settings_fav-icon_fav");
    element.setAttribute("data-satellite-isfavorite", false);

    console.log();
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

            fetch(`https://satellite-id.azurewebsites.net/api/v1/favorites/${this.getAttribute("data-satellite-name")}`, requestOptions)
                .then(response => response.text())
                .then(result => console.log(result))
                .catch(error => console.log('error', error));
        })
    });
}

const parseData = function (json, isFavPage) {
    console.log(json);

    let i = 0;

    json.forEach(element => {
        var fav = "";
        var en = "";

        if (element.properties.isfavorite) {
            fav = `<svg xmlns="http://www.w3.org/2000/svg" data-satellite-name="${element.properties.satID}" data-satellite-isfavorite="${element.properties.isfavorite}" class="c-settings_fav-icon c-settings_fav-icon_fav js-fav-icon" enable-background="new 0 0 24 24" viewBox="0 0 24 24"><g><path d="M0,0h24v24H0V0z" fill="none"/><path d="M0,0h24v24H0V0z" fill="none"/></g><g><path d="M12,17.27L18.18,21l-1.64-7.03L22,9.24l-7.19-0.61L12,2L9.19,8.63L2,9.24l5.46,4.73L5.82,21L12,17.27z"/></g></svg>`;
        }
        else {
            fav = `<svg xmlns="http://www.w3.org/2000/svg" data-satellite-name="${element.properties.satID}" data-satellite-isfavorite="${element.properties.isfavorite}" class="c-settings_fav-icon js-fav-icon" enable-background="new 0 0 24 24" viewBox="0 0 24 24"><g><path d="M0,0h24v24H0V0z" fill="none"/><path d="M0,0h24v24H0V0z" fill="none"/></g><g><path d="M12,17.27L18.18,21l-1.64-7.03L22,9.24l-7.19-0.61L12,2L9.19,8.63L2,9.24l5.46,4.73L5.82,21L12,17.27z"/></g></svg>`;
        }

        if (element.properties.isshown) {
            en = `<input id="toggle${i}" class="js-enabled-checkbox c-option c-option--hidden c-option-toggle o-hide-accessible" data-satellite-name="${element.properties.satID}" data-satellite-isfavorite="${element.properties.isshown}" checked type="checkbox" />`;
        }
        else {
            en = `<input id="toggle${i}" class="js-enabled-checkbox c-option c-option--hidden c-option-toggle o-hide-accessible" data-satellite-name="${element.properties.satID}" data-satellite-isfavorite="${element.properties.isshown}" type="checkbox" />`;
        }

        fillTable(element.properties.satName, en, fav, i);
        setEventListners();

        i++;
    });

    if (!isFavPage){
        document.querySelectorAll(".js-toggle").forEach(element => {
            element.classList.add("u-disabled");
        });
    
    
        document.querySelectorAll(".js-enabled-checkbox").forEach(element => {
            element.disabled = true;
        });
    }   
}

const showSatelliteInfo = function (satSource, isFavPage) {
    var requestOptions = {
        method: 'GET',
        redirect: 'follow'
    };

    fetch(satSource, requestOptions)
        .then(response => response.text())
        .then(result => parseData(JSON.parse(result).features, isFavPage))
        .catch(error => console.log('error', error));
}