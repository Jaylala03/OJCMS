this.customMap = null;
this.companyBranchMap = null;
this.jobMarkerList = new Array();
var mapOption = 0;// 1 means display current location in map in small area.
var geocoder;
var paramLocation = "";
var paramSearchtype = "";
var jobHtml = "";
var mapControlId = '';
var markersArray = [];
//Initialize google api
function initLoader() {
    //var script = document.createElement("script");
    //script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyCMp1LqvP3kXWxsezGnL1ZX_8RPZ_StEww&sensor=true";
    //script.type = "text/javascript";
    //document.getElementsByTagName("head")[0].appendChild(script);
    google.maps.event.addDomListener(window, 'load', mapsLoaded);
}

//Completed Loading Maps
function mapsLoaded() {
    if (mapControlId == '') {
        mapControlId = 'map';
    }

    var mapDiv = document.getElementById(mapControlId);
    var currentLocation = new google.maps.LatLng(37.790234970864, -122.39031314844);
    var lat = $("#Latitude").val();
    var lon = $("#Longitude").val();
    if (lat != '' && lon != '' && lat != 'undefined' && lon != 'undefined' && lat != null && lon != null) {
        currentLocation = new google.maps.LatLng(lat, lon);
    }
    else {
        var geocoder = new google.maps.Geocoder();
        var stateName = "";
      //  console.log($('#StateID'));
        //var cityName = $('#CityName').val();


       
        //geocoder.geocode({ 'address': cityName }, function (results, status) {
        //    if (status == google.maps.GeocoderStatus.OK) {
        //        lat = results[0].geometry.location.lat();
        //        lon = results[0].geometry.location.lng();
        //      //  alert("city Name "+stateName+" lat: "+lat+" lon: "+lon);
        //    }
        //    else {
        //        lat = 37.790234970864;
        //        lon = -122.39031314844;
        //    }
        //});

        //lat = 37.790234970864;
        //lon = -122.39031314844;
    }
    customMap = new google.maps.Map(mapDiv, {
        center: currentLocation,
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });
    var markerLabel = $("#MarkerLabel").val();
    if (markerLabel == null || markerLabel == '') {
        markerLabel = 'Candidate Location';
    }

    if (lat != '' && lon != '') {
        MarkLocation(lat, lon, markerLabel);
    }
}

$('#CityName').live("focusout", function () {
    //alert($(this).val());
    var lat = $("#Latitude").val();
    var lon = $("#Longitude").val();
    var address = $('#Address').val();
    var currentLocation = new google.maps.LatLng(37.790234970864, -122.39031314844);
    var mapDiv = document.getElementById(mapControlId);
    var geocoder = new google.maps.Geocoder();
    var cityName = $(this).val();
    var location = address + "," + cityName;
    geocoder.geocode({ 'address': location }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            lat = results[0].geometry.location.lat();
            lon = results[0].geometry.location.lng();
            //  alert("city Name "+stateName+" lat: "+lat+" lon: "+lon);
            var markerLabel = $("#MarkerLabel").val();
            if (markerLabel == null || markerLabel == '') {
                markerLabel = 'Candidate Location';
            }
            if (lat != '' && lon != '') {
                MarkLocation(lat, lon, markerLabel);
            }
        }
        else {
            lat = 37.790234970864;
            lon = -122.39031314844;
        }
    });
   
});

function CustomMapApplication() {
    initLoader();
}

function InitializeCustomMap(option,map_control_id) {
    mapOption = option;
    mapControlId = map_control_id;
    var customMapApplication = new CustomMapApplication();
}


$('#btnHideCandidateOnGMap').live("click", function (e) {
    $('#btnHideCandidateOnGMap').hide();
    $('#btnShowCandidateOnGMap').show();
    $('#show-map-demo').hide();
});

$('#btnShowCandidateOnGMap').live("click", function (e) {
    $('#btnHideCandidateOnGMap').show();
    $('#btnShowCandidateOnGMap').hide();
    if ($('#show-map-demo').is(":visible")) {
        $('#show-map-demo').hide();
    }
    else {
        $('#show-map-demo').show();
        var myLatlng = new google.maps.LatLng(-25.363882, 131.044922);
        var mapOptions = {
            zoom: 13,
            center: myLatlng
        };


        customMap = new google.maps.Map(document.getElementById('map'), mapOptions);

        var oms = new OverlappingMarkerSpiderfier(customMap);
        var iw = new google.maps.InfoWindow();
        oms.addListener('click', function (marker, event) {
            iw.setContent(marker.desc);
            iw.open(customMap, marker);
        });

        var candidateListView = $("#CandidateListView").data("kendoListView");
        if (candidateListView != null) {
            var candidateListViewDataSource = candidateListView.dataSource;
            for (var i = candidateListViewDataSource._data.length-1; i >= 0 ; i--) {
                try {
                    var lan = candidateListViewDataSource._data[i].Latitude;
                    var long = candidateListViewDataSource._data[i].Longitude;
                    var candidateName = candidateListViewDataSource._data[i].FirstName + ' ' + candidateListViewDataSource._data[i].LastName;
                    if (lan != null && long != null && lan != undefined && long != undefined) {
                        var gm = google.maps;
                        var loc = new gm.LatLng(lan, long);
                        var marker = new gm.Marker({
                            position: loc,
                            title: candidateName,
                            map: customMap
                        });
                        marker.desc = candidateName;
                        if (marker != null && marker != undefined) {
                            oms.addMarker(marker);
                        }
                        customMap.setCenter(loc, 8);
                    }
                }
                catch (inext) { }
            }
        }
        customMap.setZoom(8);
    }
});

function onCandidateListViewDataBound(arg) {

    listViewLoadCount = listViewLoadCount + 1;
    if (listViewLoadCount > 1) {
        $("#AjaxProgressDialogue").dialog("close");
        listViewLoadCount = 0;
        buttonClicked = false;
        $('html,body').animate({ scrollTop: 0 }, 'slow', function () {
        });
    }
    $('#divCandidateOnMap').hide();
    $('#btnShowCandidateOnGMap').html('<i class="fa fa-map-marker orange-color"></i>&nbsp;&nbsp;Show Google Map');
    $("#btnShowCandidateOnGMap").removeAttr("disabled");
    $('#lblTotalCandidate').html(arg.sender.dataSource._total);

    var myLatlng = new google.maps.LatLng(-25.363882, 131.044922);
    var mapOptions = {
        zoom: 13,
        center: myLatlng
    };

    customMap = new google.maps.Map(document.getElementById('map'), mapOptions);

    var oms = new OverlappingMarkerSpiderfier(customMap);
    var iw = new google.maps.InfoWindow();
    oms.addListener('click', function (marker, event) {
        iw.setContent(marker.desc);
        iw.open(customMap, marker);
    });

    //if ($('#searchBy_PageNo').val() > 1)
    //    $('#CandidateListView_pager  .k-textbox').val($('#searchBy_PageNo').val());

    var sessionCadidateId = $('#searchBy_cadidateId').val();
    if (sessionCadidateId != null && sessionCadidateId != 0) {
        $('#divHeader_' + sessionCadidateId).css("background-color", "#FCE2B1");
        $('#divCandidate_' + sessionCadidateId).css("background-color", "#FCE2B1");
        $('#divCandidate_' + sessionCadidateId).show();
        $('a[id=' + sessionCadidateId + ']').addClass("icon-chevron-up");
        $('a[id=' + sessionCadidateId + '] i:first').removeClass("fa fa-chevron-circle-down orange-color").addClass("fa fa-chevron-circle-up orange-color");
    }
    var currentPageNo = 1;
    var candidateListView = $("#CandidateListView").data("kendoListView");
    if (candidateListView != null) {
        var candidateListViewDataSource = candidateListView.dataSource;
        for (var i = candidateListViewDataSource._data.length-1; i >= 0 ; i--) {
            try {
                var lan = candidateListViewDataSource._data[i].Latitude;
                var long = candidateListViewDataSource._data[i].Longitude;
                var candidateName = candidateListViewDataSource._data[i].FirstName + ' ' + candidateListViewDataSource._data[i].LastName;
                if (lan != null && long != null && lan != undefined && long != undefined) {
                    var gm = google.maps;
                    var loc = new gm.LatLng(lan, long);
                    var marker = new gm.Marker({
                        position: loc,
                        title: candidateName,
                        map: customMap
                    });
                    marker.desc = candidateName;
                    if (marker != null && marker != undefined) {
                        oms.addMarker(marker);
                    }
                    customMap.setCenter(loc, 8);
                }
            }
            catch (inext) { }
        }
    }
    var newPageNo = $('#searchBy_PageNo').val();
    var currentPage = $('#CandidateListView_pager  input.k-textbox').val();
    if (newPageNo > 1 && (currentPage == 1 || currentPage=="")) {
        candidateListView.dataSource.page(newPageNo);
    }
    $('#searchBy_PageSize').val(candidateListView.dataSource.pageSize());
    customMap.setZoom(8);
}

function onWebCandidateListViewDataBound(arg) {
    listViewLoadCount = listViewLoadCount + 1;
    if (listViewLoadCount > 1) {
        $("#AjaxProgressDialogue").dialog("close");
        listViewLoadCount = 0;
        buttonClicked = false;
        $('html,body').animate({ scrollTop: 0 }, 'slow', function () {
        });
    }
    $('#lblTotalCandidate').html(arg.sender.dataSource._total);
    //var candidateListView = $("#WebCandidateListView").data("kendoListView");
    //if (candidateListView != null) {
    //    var candidateListViewDataSource = candidateListView.dataSource;
    //    $('#lblTotalCandidate').html(arg.sender.dataSource._total);
    //}
    //listViewLoadCount = listViewLoadCount + 1;
    //if (listViewLoadCount > 1) {
    //    $("#AjaxProgressDialogue").dialog("close");
    //    listViewLoadCount = 0;
    //    buttonClicked = false;
    //    $('html,body').animate({ scrollTop: 0 }, 'slow', function () {
    //    });
    //}
}

$('#btnHideJobOnGMap').live("click", function (e) {
    $('#btnShowJobOnGMap').show();
    $('#btnHideJobOnGMap').hide();
    $('#show-map-demo').hide();
});

$('#btnShowJobOnGMap').live("click", function (e) {
    $('#btnShowJobOnGMap').hide();
    $('#btnHideJobOnGMap').show();
    if ($('#show-map-demo').is(":visible")) {
        $('#show-map-demo').hide();
    }
    else {
        $('#show-map-demo').show();
        var myLatlng = new google.maps.LatLng(-25.363882, 131.044922);
        var mapOptions = {
            zoom: 13,
            center: myLatlng
        };


        customMap = new google.maps.Map(document.getElementById('map'), mapOptions);

        var oms = new OverlappingMarkerSpiderfier(customMap);
        var iw = new google.maps.InfoWindow();
        oms.addListener('click', function (marker, event) {
            iw.setContent(marker.desc);
            iw.open(customMap, marker);
        });

        var jobListView = $("#ListViewJob").data("kendoListView");
        if (jobListView != null) {
            var jobListViewDataSource = jobListView.dataSource;
            for (var i = jobListViewDataSource._data.length-1; i >= 0 ; i--) {
                try {
                    var lan = jobListViewDataSource._data[i].Latitude;
                    var long = jobListViewDataSource._data[i].Longitude;
                    var jobName = jobListViewDataSource._data[i].PositionName;
                    if (lan != null && long != null && lan != undefined && long != undefined) {
                        var gm = google.maps;
                        var loc = new gm.LatLng(lan, long);
                        var marker = new gm.Marker({
                            position: loc,
                            title: jobName,
                            map: customMap
                        });
                        marker.desc = jobName;
                        if (marker != null && marker != undefined) {
                            oms.addMarker(marker);
                        }
                        customMap.setCenter(loc, 8);
                    }
                }
                catch (inext) { }
            }
        }
        customMap.setZoom(8);
    }
});

function onJobListViewDataBound(arg) {

    listViewLoadCount = listViewLoadCount + 1;
    if (listViewLoadCount > 1) {
        $("#AjaxProgressDialogue").dialog("close");
        listViewLoadCount = 0;
        buttonClicked = false;
        $('html,body').animate({ scrollTop: 0 }, 'slow', function () {
        });
    }
    $('#divJobOnMap').hide();
    $('#btnShowJobOnGMap').html('<i class="fa fa-map-marker orange-color"></i>&nbsp;&nbsp;Show Google Map');
    $("#btnShowJobOnGMap").removeAttr("disabled");
    $('#lblTotalJob').html(arg.sender.dataSource._total);

    var myLatlng = new google.maps.LatLng(-25.363882, 131.044922);
    var mapOptions = {
        zoom: 13,
        center: myLatlng
    };

    customMap = new google.maps.Map(document.getElementById('map'), mapOptions);

    var oms = new OverlappingMarkerSpiderfier(customMap);
    var iw = new google.maps.InfoWindow();
    oms.addListener('click', function (marker, event) {
        iw.setContent(marker.desc);
        iw.open(customMap, marker);
    });

    var jobListView = $("#ListViewJob").data("kendoListView");
    if (jobListView != null) {
        var jobListViewDataSource = jobListView.dataSource;
        for (var i = jobListViewDataSource._data.length-1; i >= 0 ; i--) {
            try {
                var lan = jobListViewDataSource._data[i].Latitude;
                var long = jobListViewDataSource._data[i].Longitude;
                var jobName = jobListViewDataSource._data[i].PositionName;
                if (lan != null && long != null && lan != undefined && long != undefined) {
                    var gm = google.maps;
                    var loc = new gm.LatLng(lan, long);
                    var marker = new gm.Marker({
                        position: loc,
                        title: jobName,
                        map: customMap
                    });
                    marker.desc = jobName;
                    if (marker != null && marker != undefined) {
                        oms.addMarker(marker);
                    }
                    customMap.setCenter(loc, 8);
                }
            }
            catch (inext) { }
        }
    }
    customMap.setZoom(8);
}

$('#btnHideCompanyOnGMap').live("click", function (e) {
    $('#btnShowCompanyOnGMap').show();
    $('#btnHideCompanyOnGMap').hide();
    $('#show-map-demo').hide();
});

$('#btnShowCompanyOnGMap').live("click", function (e) {
    $('#btnShowCompanyOnGMap').hide();
    $('#btnHideCompanyOnGMap').show();
    if ($('#show-map-demo').is(":visible")) {
        $('#show-map-demo').hide();
    }
    else {
        $('#show-map-demo').show();
        var myLatlng = new google.maps.LatLng(-25.363882, 131.044922);
        var mapOptions = {
            zoom: 13,
            center: myLatlng
        };

        customMap = new google.maps.Map(document.getElementById('map'), mapOptions);

        var oms = new OverlappingMarkerSpiderfier(customMap);
        var iw = new google.maps.InfoWindow();
        oms.addListener('click', function (marker, event) {
            iw.setContent(marker.desc);
            iw.open(customMap, marker);
        });
                
        var companyListView = $("#CompanyListView").data("kendoListView");
        if (companyListView != null) {
            var companyListViewDataSource = companyListView.dataSource;
            for (var i = companyListViewDataSource._data.length - 1; i >= 0 ; i--) {
                try {
                    var lan = companyListViewDataSource._data[i].Latitude;
                    var long = companyListViewDataSource._data[i].Longitude;
                    var companyName = companyListViewDataSource._data[i].Name;
                    if (lan != null && long != null && lan != undefined && long != undefined) {
                        var gm = google.maps;
                        var loc = new gm.LatLng(lan, long);
                        var marker = new gm.Marker({
                            position: loc,
                            title: companyName,
                            map: customMap
                        });
                        marker.desc = companyName;
                        if (marker != null && marker != undefined) {
                            oms.addMarker(marker);
                        }
                        customMap.setCenter(loc, 8);
                    }
                }
                catch (inext) { }
            }
        }
        customMap.setZoom(8);
    }
});

function onCompanyListViewDataBound(arg) {

    listViewLoadCount = listViewLoadCount + 1;
    if (listViewLoadCount > 1) {
        $("#AjaxProgressDialogue").dialog("close");
        listViewLoadCount = 0;
        buttonClicked = false;        
    }

    $('#divCompanyOnMap').hide();
    $('#btnShowCompanyOnGMap').html('<i class="fa fa-map-marker orange-color"></i>&nbsp;&nbsp;Show Google Map');
    $("#btnShowCompanyOnGMap").removeAttr("disabled");
    $('#lblTotalCompany').html(arg.sender.dataSource._total);

    var myLatlng = new google.maps.LatLng(-25.363882, 131.044922);
    var mapOptions = {
        zoom: 13,
        center: myLatlng
    };

    customMap = new google.maps.Map(document.getElementById('map'), mapOptions);

    var oms = new OverlappingMarkerSpiderfier(customMap);
    var iw = new google.maps.InfoWindow();
    oms.addListener('click', function (marker, event) {
        iw.setContent(marker.desc);
        iw.open(customMap, marker);
    });

    var sessionCompanyId = $('#searchBy_companyId').val();
    if (sessionCompanyId != null && sessionCompanyId != 0) {
        $('#divHeader_' + sessionCompanyId).css("background-color", "#FCE2B1");
        $('#divCompany_' + sessionCompanyId).css("background-color", "#FCE2B1");
        $('#divCompany_' + sessionCompanyId).show();
        $('a[id=' + sessionCompanyId + ']').addClass("icon-chevron-up");
        $('a[id=' + sessionCompanyId + '] i:first').removeClass("fa fa-chevron-circle-down orange-color").addClass("fa fa-chevron-circle-up orange-color");
    }
    var companyListView = $("#CompanyListView").data("kendoListView");
    if (companyListView != null) {
        var companyListViewDataSource = companyListView.dataSource;
       
        for (var i = companyListViewDataSource._data.length-1; i >= 0 ; i--) {
            try {
                var lan = companyListViewDataSource._data[i].Latitude;
                var long = companyListViewDataSource._data[i].Longitude;
                var companyName = companyListViewDataSource._data[i].Name;
                if (lan != null && long != null && lan!=undefined && long!=undefined) {
                    var gm = google.maps;
                    var loc = new gm.LatLng(lan, long);
                    var marker = new gm.Marker({
                        position: loc,
                        title: companyName,
                        map: customMap
                    });
                    marker.desc = companyName;
                    if (marker != null && marker != undefined) {
                        oms.addMarker(marker);
                    }
                    customMap.setCenter(loc, 8);
                }
            }
            catch (inext) { }            
        }
    }
    var newPageNo = $('#searchBy_PageNo').val();
    var currentPage = $('#CompanyListView_pager  input.k-textbox').val();
    //$('#searchBy_PageNo').val(currentPage);
    if (newPageNo > 1 && (currentPage == 1 || currentPage == "")) {
        companyListView.dataSource.page(newPageNo);
        //$('#CompanyListView_pager  input.k-textbox').val(newPageNo);
    }
    //$('#searchBy_PageNo').val(companyListView.dataSource.page());
    $('#searchBy_PageSize').val(companyListView.dataSource.pageSize());

    customMap.setZoom(8);    
}

function MarkLocation(lat, lon, message) {
    if (lat && lon) {
        var point = new google.maps.LatLng(lat, lon);
        if (point) {
            var marker = new google.maps.Marker({
                position: point,
                map: customMap,
                title: message
            });
            var infowindow = new google.maps.InfoWindow({
                content: message
            });
            google.maps.event.addListener(marker, 'mouseover', function () {
                infowindow.open(customMap, marker);
            });
            google.maps.event.addListener(marker, 'mouseout', function () {
                infowindow.close(customMap, marker);
            });

            customMap.setCenter(point, 13);
            markersArray.push(marker);
        }
    }
    else if (lat) {
        this.customMap.setCenter(lat, 13);
        if (paramSearchtype == "") {
            this.customMap.addOverlay(new GMarker(lat));
        }
    }
}

function LoadMap(mapControlId) {
    var mapDiv = document.getElementById(mapControlId);
    var currentLocation = new google.maps.LatLng(37.790234970864, -122.39031314844);
    var lat = $("#Latitude").val();
    var lon = $("#Longitude").val();
    if (lat != '' && lon != '' && lat != 'undefined' && lon != 'undefined' && lat != null && lon != null) {
        currentLocation = new google.maps.LatLng(lat, lon);
    }
    else {
        lat = 37.790234970864;
        lon = -122.39031314844;
    }
    var newMap = new google.maps.Map(mapDiv, {
        center: currentLocation,
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });
    var markerLabel = $("#MarkerLabel").val();
    if (markerLabel == null || markerLabel == '') {
        markerLabel = 'Candidate Location';
    }
    if (lat != '' && lon != '') {
        var point = new google.maps.LatLng(lat, lon);
        if (point) {
            var marker = new google.maps.Marker({
                position: point,
                map: newMap,
                title: markerLabel
            });
            newMap.setCenter(point, 13);
        }
    }
}

function LoadMapWithLatLon(mapControlId,latControlId,lonControlId,caption) {
    var mapDiv = document.getElementById(mapControlId);
    var currentLocation = new google.maps.LatLng(37.790234970864, -122.39031314844);
    var lat = $(latControlId).val();
    var lon = $(lonControlId).val();
    if (lat != '' && lon != '' && lat != 'undefined' && lon != 'undefined' && lat != null && lon != null) {
        currentLocation = new google.maps.LatLng(lat, lon);
    }
    else {
        lat = 37.790234970864;
        lon = -122.39031314844;
    }
    var newMap = new google.maps.Map(mapDiv, {
        center: currentLocation,
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });
    var markerLabel = $("#MarkerLabel").val();
    if (markerLabel == null || markerLabel == '') {
        markerLabel = caption;
    }
    if (lat != '' && lon != '') {
        var point = new google.maps.LatLng(lat, lon);
        if (point) {
            var marker = new google.maps.Marker({
                position: point,
                map: newMap,
                title: markerLabel
            });
            newMap.setCenter(point, 13);
        }
    }
}


function LoadCompanyBranchMaps(ID, getUrl, mapControlID) {

    var postData = "id=" + ID;

    $.ajax({
        type: "GET",
        url: getUrl,
        data: postData,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

            if (result.length != 0) {

                var myLatlng = new google.maps.LatLng(-25.363882, 131.044922);
                var mapOptions = {
                    zoom: 10,
                    center: myLatlng
                    //mapTypeId: google.maps.MapTypeId.ROADMAP
                };
                var mapControl = document.getElementById(mapControlID);
                companyBranchMap = new google.maps.Map(mapControl, mapOptions);

                var oms = new OverlappingMarkerSpiderfier(companyBranchMap);
                var iw = new google.maps.InfoWindow();
                oms.addListener('click', function (marker, event) {
                    iw.setContent(marker.desc);
                    iw.open(companyBranchMap, marker);
                });
                oms.addListener(marker, 'mouseover', function () {
                    iw.setContent(marker.desc);
                    iw.open(companyBranchMap, marker);
                });
                oms.addListener(marker, 'mouseout', function () {
                    iw.setContent(marker.desc);
                    iw.close(companyBranchMap, marker);
                });

                for (var i = 0; i < result.length; i++) {
                    try {
                        var lan = result[i].Latitude;
                        var long = result[i].Longitude;
                        var branchName = result[i].BranchName;

                        if (lan != null && long != null && lan != undefined && long != undefined) {
                            var gm = google.maps;
                            var loc = new gm.LatLng(lan, long);
                            var marker = new gm.Marker({
                                position: loc,
                                title: branchName,
                                map: companyBranchMap
                            });
                            marker.desc = branchName;
                            if (marker != null && marker != undefined) {
                                oms.addMarker(marker);
                            }
                            companyBranchMap.setCenter(loc, 8);
                        }
                    }
                    catch (inext) { }
                }
                companyBranchMap.setZoom(8);

            } else {
                var html = "<div class='alert alert-error' id='divAlertError'>There is no branch found</div>";
                $("#dvCompanyBranchMapAjaxAlertContainer").html('');
                $("#dvCompanyBranchMapAjaxAlertContainer").html(html);
            }
        },
        complete: function () {

        },
        error: function (req, status, error) {
            alert('error code-[' + status + "] and error desc-[" + error + "]");
        }
    });

}


//function setCookie(cname, cvalue, exdays) {
//    //var d = new Date();
//    //d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
//    //var expires = "expires=" + d.toUTCString();
//    document.cookie = cname + "=" + cvalue;// + "; " + expires;
//}
//function getCookie(cname) {
//    var name = cname + "=";
//    var ca = document.cookie.split(';');
//    for (var i = 0; i < ca.length; i++) {
//        var c = ca[i];
//        while (c.charAt(0) == ' ')
//            c = c.substring(1);
//        if (c.indexOf(name) == 0)
//            return c.substring(name.length, c.length);
//    }
//    return "";
//}

//function MarkCompanyBranchLocation(lat, lon, message) {

//    if (lat && lon) {
//        var point = new google.maps.LatLng(lat, lon);
//        if (point) {
//            var marker = new google.maps.Marker({
//                position: point,
//                map: companyBranchMap,
//                title: message
//            });
//            var infowindow = new google.maps.InfoWindow({
//                content: message
//            });
//            google.maps.event.addListener(marker, 'mouseover', function () {
//                infowindow.open(companyBranchMap, marker);
//            });
//            google.maps.event.addListener(marker, 'mouseout', function () {
//                infowindow.close(companyBranchMap, marker);
//            });

//            companyBranchMap.setCenter(point, 13);
//        }
//    }
//    else if (lat) {
//        this.customMap.setCenter(lat, 13);
//        if (paramSearchtype == "") {
//            this.customMap.addOverlay(new GMarker(lat));
//        }
//    }
//}