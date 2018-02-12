var dataids = [];
$(document).ready(function () {
    $("#ZipCode").live("focusout",function () {
        var id = $(this).attr('id');
        var imgId = '#img' + id;
        var img = $(imgId);
        if (img != null) {
            $(img).attr('src', '/images/tick.png');
        }
        var value = $(this).val();
        var valControl = $('span[data-valmsg-for="' + id + '"]');
        var valid = true;
        if (valControl != null) {
            if (valControl.is(':hidden')) {
                valid = true;
            }
            else {
                valid = false;
            }
        }
        if (value != null && value != '') {
            if (img != null && valid) {
                $(img).show();
            }
        }
        else {
            $(img).hide();
        }
        dataids = [0, 0, 0];
        $("#CityName").val("");
        setIdsToDropdown();
        var zip = $(this).val();
        if (zip != "" && zip.length >= 5) {
            var geocoder = new google.maps.Geocoder();

            geocoder.geocode({ 'address': zip }, function (result, status) {
                if (status == "OK") {
                    for (var ind = 0; ind < result.length; ind++) {
                        if (result[ind].types[0] == "postal_code") {
                            var country_short_name = "";
                            var country_long_name = "";
                            var state_short_name = "";
                            var state_long_name = "";
                            var city_short_name = "";
                            var city_long_name = "";
                            var lat;
                            var long;
                            for (var component in result[ind]['address_components']) {
                                for (var i in result[ind]['address_components'][component]['types']) {
                                    var typeaddress = result[ind]['address_components'][component]['types'][i];
                                    switch (typeaddress) {
                                        case "sublocality":
                                        case "locality":
                                            city_long_name = result[ind]['address_components'][component]['long_name'];
                                            city_short_name = result[ind]['address_components'][component]['short_name'];
                                            break;
                                        case "administrative_area_level_1":
                                            state_long_name = result[ind]['address_components'][component]['long_name'];
                                            state_short_name = result[ind]['address_components'][component]['short_name'];
                                            break;
                                        case "country":
                                            country_long_name = result[ind]['address_components'][component]['long_name'];
                                            country_short_name = result[ind]['address_components'][component]['short_name'];
                                            break;
                                    }
                                }
                            }
                            lat = result[0].geometry.location.lat();
                            long = result[0].geometry.location.lng();
                            //lat = result[ind]['geometry']['location']['b'];
                            //long = result[ind]['geometry']['location']['d'];
                            $("#Latitude").val(lat);
                            $("#Longitude").val(long);
                            try
                            {
                                if (this.customMap != null) {
                                    customMap.setCenter(new google.maps.LatLng(lat, long), 13);
                                    MarkLocation(lat, long);
                                }
                            }
                            catch (err) {
                            }
                            // $("#Latitude").val(lat);
                            //  $("#Longitude").val(long);
                            $("#CityName").val(city_long_name);
                            var getUrl = $("#hfZipCodeUrl").val();
                            if (getUrl == null || getUrl == "") {
                                getUrl = "GetCityStateCountryIDAjax";
                            }
                            if (country_long_name != "" && state_long_name != "" && state_short_name != "") {
                                jQuery.ajax({
                                    type: "POST",
                                    url: getUrl,
                                    data: { country_long_name: country_long_name, country_short_name: country_short_name, state_long_name: state_long_name, state_short_name: state_short_name, city_long_name: city_long_name, city_short_name: city_short_name },
                                    dataType: "json",
                                    //async: false,
                                    success: function (data) {
                                        // alert(dataids);
                                        if (data.length > 0) {
                                            dataids = data;
                                            setIdsToDropdown();
                                        }
                                    }
                                });
                            }
                        }
                    }
                }
            });
        }
    });
});
function setIdsToDropdown() {
    setkendodropdownvalue("#CountryID", dataids[0]);
    setkendodropdownvalue("#StateID", dataids[1]);
    if (dataids.length >= 3) {
        $("#CityID").val(dataids[2]);
    }
    if (dataids.length >= 4) {
        $("input[name='AreaCode']").each(function () {
            $(this).val(dataids[3]);
        });
    }
}
function setkendodropdownvalue(controlid, value) {
    var dropDownList = $(controlid).data("kendoDropDownList");
    if (dropDownList != null && dropDownList != 'undefined') {
        dropDownList.value(value);
    }
}