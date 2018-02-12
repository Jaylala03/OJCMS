var fancyBoxOpened = false;
var listViewLoadCount = 0;
var buttonClicked = false;
$(document).ready(function () {

    $.ajaxSetup({ cache: false });

    var confirmDelete = function () {
        return confirm(' Are you sure you want to delete this record?');
    };

    PassSearchParameter = function () {
        if (!buttonClicked) {
            listViewLoadCount = 1;
        }
        //if (buttonClicked) {
        //    $("#AjaxProgressDialogue").dialog("open");
        //}
        //if (localStorage.hasOwnProperty)
        //    $('#ProgramID').val(localStorage.getItem("ProgramID"));

        //$('#RegionID').val(localStorage.getItem("RegionID"));

        //$('#CaseStatusID').val(localStorage.getItem("CaseStatusID"));
        //$('#LastName').val(localStorage.getItem("LastName"));

        //$('#FirstName').val(localStorage.getItem("FirstName"));
        //$('#DisplayID').val(localStorage.getItem("DisplayID"));
        //$('#CreatedByWorkerName').val(localStorage.getItem("CreatedByWorkerName"));
        //$('#PhoneNumber').val(localStorage.getItem("PhoneNumber"));
        //$('#JamatkhanaID').val(localStorage.getItem("JamatkhanaID"));
        //$('#CaseNumber').val(localStorage.getItem("CaseNumber"));
        //$('#SubProgramID').val(localStorage.getItem("SubProgramID"));

        setTimeout(function () {

            var dropdownlist = $("#SubProgramID").data("kendoDropDownList");
            var index = parseInt(localStorage.getItem("SubProgramID"));
            if (dropdownlist != null && dropdownlist != undefined) {
                dropdownlist.value(index);
            }




            // $('#lnkSearchCase').click();
        }, 6000);
        var o = {};
        try {
            var frmSearch = $('#frmSearch');
            if (frmSearch != null && frmSearch != undefined) {
                var data = frmSearch.serializeArray();
                $.each(data, function () {
                    if (o[this.name] !== undefined) {
                        if (!o[this.name].push) {
                            o[this.name] = [o[this.name]];
                        }
                        o[this.name].push(this.value || '');
                    } else {
                        o[this.name] = this.value || '';
                    }
                });
            }

            var hfParantId = $("#hfParentId");
            if (hfParantId) {
                var name = $(hfParantId).attr('name');
                o[name] = hfParantId.val();
            }
        }
        catch (ex) {
        }
        console.log(o);
        return o;
    };


    PassSearchParameterWorker = function () {
        if (!buttonClicked) {
            listViewLoadCount = 1;
        }



        var o = {};
        try {
            var frmSearch = $('#frmSearch');
            if (frmSearch != null && frmSearch != undefined) {
                var data = frmSearch.serializeArray();
                $.each(data, function () {
                    if (o[this.name] !== undefined) {
                        if (!o[this.name].push) {
                            o[this.name] = [o[this.name]];
                        }
                        o[this.name].push(this.value || '');
                    } else {
                        o[this.name] = this.value || '';
                    }
                });
            }

            var hfParantId = $("#hfParentId");
            if (hfParantId) {
                var name = $(hfParantId).attr('name');
                o[name] = hfParantId.val();
            }
        }
        catch (ex) {
        }
        console.log(o);
        return o;
    };


    PassSearchParameterProvider = function () {
        if (!buttonClicked) {
            listViewLoadCount = 1;
        }



        var o = {};
        try {
            var frmSearch = $('#frmEditorServiceProvider');
            if (frmSearch != null && frmSearch != undefined) {
                var data = frmSearch.serializeArray();
                $.each(data, function () {
                    if (o[this.name] !== undefined) {
                        if (!o[this.name].push) {
                            o[this.name] = [o[this.name]];
                        }
                        o[this.name].push(this.value || '');
                    } else {
                        o[this.name] = this.value || '';
                    }
                });
            }

            var hfParantId = $("#hfParentId");
            if (hfParantId) {
                var name = $(hfParantId).attr('name');
                o[name] = hfParantId.val();
            }
        }
        catch (ex) {
        }
        return o;
    };

    PassSearchParameterService = function () {
        if (!buttonClicked) {
            listViewLoadCount = 1;
        }



        var o = {};
        try {
            var frmSearch = $('#frmEditorService');
            if (frmSearch != null && frmSearch != undefined) {
                var data = frmSearch.serializeArray();
                $.each(data, function () {
                    if (o[this.name] !== undefined) {
                        if (!o[this.name].push) {
                            o[this.name] = [o[this.name]];
                        }
                        o[this.name].push(this.value || '');
                    } else {
                        o[this.name] = this.value || '';
                    }
                });
            }

            var hfParantId = $("#hfParentId");
            if (hfParantId) {
                var name = $(hfParantId).attr('name');
                o[name] = hfParantId.val();
            }
        }
        catch (ex) {
        }
        return o;
    };

    //WS.jQuery.notific8 = {
    //    displayMessage: function (message, color, life) {
    //        var settings = {
    //            life: life,
    //            theme: color,
    //            sticky: false,
    //            verticalEdge: 'right',
    //            horizontalEdge: 'top'
    //        };
    //        $.notific8('zindex', 11500);
    //        $.notific8(message, settings);
    //    }
    //};
    var clearForm = function (id) {
        try {

            $('#' + id).find(':input').each(function () {
                var id = $(this).attr('id');

                switch (this.type || id) {
                    case 'password':
                    case 'select-multiple':

                        if (this.dataset && this.dataset['role'] == 'multiselect') {

                            var dropdownlist = $("#" + id).data("kendoMultiSelect");

                            dropdownlist.value(0);
                            $('#divEthnicityOther').addClass('hide');
                            $('#divEthnicity').addClass('col-md-8');
                        }
                        break;
                    case 'select-one':
                    case 'select':
                    case 'text': try {
                        var dropdownlist = $("#" + id).data("kendoDropDownList");

                        if (dropdownlist) {
                            dropdownlist.options.optionLabel = "Please Select";
                            dropdownlist.refresh();
                            dropdownlist.select(0);
                        }

                        $(this).val('');
                        $(this).text('');

                    }
                        catch (e) { }
                        break;
                    case 'textarea':
                        try {
                            if (this.dataset && this.dataset['role'] == 'dropdownlist') {
                                var dropdownlist = $("#" + id).data("kendoDropDownList");

                                dropdownlist.select(0);
                            }
                            else {
                                $(this).val('');
                                $(this).text('');
                            }
                        }
                        catch (e) { }
                        break;
                    case 'checkbox':
                    case 'radio':
                        this.checked = false;
                    case 'hidden':
                        try {
                            if (id == 'ID') {
                                $(this).val('0');
                            }
                            else if (id.indexOf('Date') >= 0) {
                                $(this).val('01/01/0001');
                            }
                            //else if (id.indexOf('Is') >= 0) {
                            //    $(this).val('false');
                            //}
                            //else {
                            //    $(this).val('');
                            //}
                        }
                        catch (ex) { }
                }
            });
        }
        catch (err) {
        }
    };

    setVisiblityFilter = function () {
        var filterArray = [];
        var filterArrayData = localStorage.getItem('filterArray');

        if (filterArrayData != null && filterArrayData != undefined) {
            filterArray = filterArrayData.split(',');
            $(filterArray).each(function (i, item) {
                $('.mutliSelect input[value=' + item + ']').prop('checked', true);
                var grid = $("#GridCase").data("kendoGrid");

                grid.hideColumn(parseInt(item));
            });
        }
    };

    var displayMessage = function (message, color, life) {
        var settings = {
            life: life,
            theme: color,
            sticky: false,
            verticalEdge: 'right',
            horizontalEdge: 'top'
        };
        $.notific8('zindex', 11500);
        $.notific8(message, settings);
    };

    $('#divRegion').live("click", function (e) {
        var programID = $('#ProgramID').val();
        if (!programID) {
            alert('Please select a program first');
        }
    });

    GetprogressNoteByCaseID = function () {
        $('#GridCase').find('table').find('tbody').find('tr').each(function () {
            var $obj = $(this);
            var caseId = $(this).find('td:last-child').find('input').val();
            $.ajax({
                url: '/CaseProgressNote/GetprogressNoteByCaseID',
                data: { 'CaseID': caseId },
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                async: true,
                // contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $obj.find('td').eq(8).html(data);
                },
                failue: function (data) {
                    console.log(data);
                }
            });
            // $.post("/CaseProgressNote/GetprogressNoteByCaseID", { CaseID: caseId },
            //function (res) {
            //    if (res) {
            //        $obj.find('td').eq(8).html(res);
            //    }
            //});
        });
    }

    $('#divSubProgram,#divJamatkhana').live("click", function (e) {
        var regionID = $('#RegionID').val();
        if (!regionID) {
            alert('Please select a region first');
        }
    });

    $('#btnSaveAndRefresh').live("click", function (e) {
        debugger
        e.preventDefault();
        var entityName = $(this).attr('name').replace('btnSaveAndRefresh', '');
        var gridId = '#Grid' + entityName;
        var listviewId = '#ListView' + entityName;
        var formID = 'frmEditor' + entityName;
        var $form = $('#' + formID);

        var url = $form.attr('action');
        var searchString = window.location.search.substring(1);
        if (searchString != null) {
            if (url.indexOf('?') == -1) {
                url = url + "?";
                url = url + searchString;
            }
            else {
                url = url + "&" + searchString;
            }
        }
        if (!$form.valid || $form.valid()) {

            $.post(url, $form.serializeArray(),
            function (res) {
                if (res.success) {

                    clearForm(formID);
                    if (res.data != null)
                        displayMessage(res.data, 'lime', 5000);
                    if (res.url != null && res.url != "undefined") {
                        location.href = res.url;
                        return;
                    }
                    if (gridId.indexOf("Grid") > 0) {
                        var gridDynamic = $(gridId).data("kendoGrid");
                        if (gridDynamic != null) {
                            gridDynamic.dataSource.read();
                            gridDynamic.refresh();
                            if (gridId == '#GridCaseAction') {
                                gridId = gridId + 'Completed';
                                if (gridId.indexOf("Grid") > 0) {
                                    var gridDynamic = $(gridId).data("kendoGrid");
                                    if (gridDynamic != null) {
                                        gridDynamic.dataSource.read();
                                        gridDynamic.refresh();
                                        return false;
                                    }
                                }
                            }
                            return false;
                        }
                    }

                    if (listviewId.indexOf("ListView") > 0) {
                        var listviewDynamic = $(listviewId).data("kendoListView");
                        if (listviewDynamic != null) {
                            listviewDynamic.dataSource.read();
                            listviewDynamic.refresh();
                            return false;
                        }
                    }
                    //for refreshing two grid of same model

                }
                else {
                    if (res.data != null)
                        displayMessage(res.data, 'ruby', 10000);
                }
                return false;
            });
        }

        return false;
    });

    $('#btnSaveAndRefreshPopUp').live("click", function (e) {
        ;
        e.preventDefault();
        var entityName = $(this).attr('name').replace('btnSaveAndRefreshPopUp', '');
        var gridId = '#Grid' + entityName;
        var listviewId = '#ListView' + entityName;
        var $form = $('#frmEditorPopUp' + entityName);
        var url = $form.attr('action');

        if (!$form.valid || $form.valid()) {

            $.post(url, $form.serializeArray(),
            function (res) {
                if (res.success) {
                    if (res.data != null)
                        displayMessage(res.data, 'lime', 5000);
                    if (res.url != null && res.url != "undefined") {
                        location.href = res.url;
                        return;
                    }
                    if (gridId.indexOf("Grid") > 0) {
                        var gridDynamic = $(gridId).data("kendoGrid");
                        if (gridDynamic != null) {
                            gridDynamic.dataSource.read();
                            gridDynamic.refresh();
                        }
                    }
                    if (listviewId.indexOf("ListView") > 0) {
                        var listviewDynamic = $(listviewId).data("kendoListView");
                        if (listviewDynamic != null) {
                            listviewDynamic.dataSource.read();
                            listviewDynamic.refresh();
                        }
                    }
                    parent.$.fancybox.close();
                }
                else {
                    if (res.data != null)
                        displayMessage(res.data, 'ruby', 10000);
                    parent.$.fancybox.close();
                }
                return false;
            });
        }
    });

    $('#btnSaveAndRefreshCircle').live("click", function (e) {
        ;
        e.preventDefault();
        var $form = $('#circleFrmEditor');
        var url = $form.attr('action');
        if (!$form.valid || $form.valid()) {

            $.post(url, $form.serializeArray(),
            function (res) {
                if (res.success) {
                    ClearSupportCircle();
                    if (res.data != null)
                        displayMessage(res.data, 'lime', 5000);
                    if (res.url != null && res.url != "undefined") {
                        location.href = res.url;
                        return;
                    }
                    var gridDynamic = $('#GridCaseSupportCircle').data("kendoGrid");
                    if (gridDynamic != null) {
                        gridDynamic.dataSource.read();
                        gridDynamic.refresh();
                        return;
                    }
                }
                else {
                    if (res.data != null)
                        displayMessage(res.data, 'ruby', 10000);
                }
                return false;
            });
        }
    });

    $('#btnSave').live("click", function (e) {
        ;
        e.preventDefault();
        var entityName = $(this).attr('name').replace('btnSave', '');
        var $form = $('#frmEditor' + entityName);
        var url = $form.attr('action');
        if (entityName == 'CaseMember') {
            url = url + '&isredirect=true';
        }
        if (!$form.valid || $form.valid()) {
            $.post(url, $form.serializeArray(),
            function (res) {
                console.log(res);
                if (res.success) {
                    if (res.url != null && res.url != "undefined") {
                        location.href = res.url;
                        return;
                    }
                    if (res.data != null)
                        displayMessage(res.data, 'lime', 5000);
                }
                else {
                    if (res.data != null)
                        displayMessage(res.data, 'ruby', 10000);
                }
                return false;
            });
        }
    });

    $("#btnSearch").live("click", function (e) {
        e.preventDefault();
        var gridId = '#' + $(this).attr('name').replace('btnSearch', 'Grid');
        var listviewId = '#' + $(this).attr('name').replace('btnSearch', 'ListView');
        if (gridId.indexOf("Grid") > 0) {
            var gridDynamic = $(gridId).data("kendoGrid");
            if (gridDynamic != null) {
                gridDynamic.dataSource.read();
                gridDynamic.refresh();
                return;
            }
        }
        if (listviewId.indexOf("ListView") > 0) {
            var listviewDynamic = $(listviewId).data("kendoListView");
            if (listviewDynamic != null) {
                listviewDynamic.dataSource.read();
                listviewDynamic.refresh();
                return;
            }
        }
    });

    $('a[name="lnkSearch"]').live("click", function (e) {
        //localStorage.setItem('ProgramID', $('#ProgramID').val());
        //localStorage.setItem('RegionID', $('#RegionID').val());
        //localStorage.setItem('SubProgramID', $('#SubProgramID').val());
        //localStorage.setItem('CaseStatusID', $('#CaseStatusID').val());
        //localStorage.setItem('LastName', $('#LastName').val());
        //localStorage.setItem('FirstName', $('#FirstName').val());
        //localStorage.setItem('DisplayID', $('#DisplayID').val());
        //localStorage.setItem('CreatedByWorkerName', $('#CreatedByWorkerName').val());
        //localStorage.setItem('PhoneNumber', $('#PhoneNumber').val());
        //localStorage.setItem('JamatkhanaID', $('#JamatkhanaID').val());
        //localStorage.setItem('CaseNumber', $('#CaseNumber').val());
        e.preventDefault();
        var gridID = '#Grid' + $(this).attr('id').replace('lnkSearch', '');
        var listViewID = '#ListView' + $(this).attr('id').replace('lnkSearch', '');
        buttonClicked = true;
        var grid = $(gridID).data("kendoGrid");
        if (grid != null) {
            grid.dataSource.read();
            grid.refresh();
        }
        var lstView = $(listViewID).data("kendoListView");
        if (lstView != null) {
            lstView.dataSource.read();
            lstView.refresh();
        }
    });


    $('#btnServiceproviderSearch').live("click", function (e) {


        var gridID = '#GridServiceProvider';
        //var listViewID = '#ListView' + $(this).attr('id').replace('lnkSearch', '');
        buttonClicked = true;
        var grid = $(gridID).data("kendoGrid");
        if (grid != null) {
            grid.dataSource.read();
            grid.refresh();
        }

    });


    $('#btnServiceSearch').live("click", function (e) {


        var gridID = '#GridService';
        //var listViewID = '#ListView' + $(this).attr('id').replace('lnkSearch', '');
        buttonClicked = true;
        var grid = $(gridID).data("kendoGrid");
        if (grid != null) {
            grid.dataSource.read();
            grid.refresh();
        }

    });


    $('a[name="lnkDelete"]').live("click", function (e) {
        e.preventDefault();
        if (confirmDelete()) {
            var gridID = '#Grid' + $(this).attr('id').replace('lnkDelete', '');
            var listViewID = '#ListView' + $(this).attr('id').replace('lnkDelete', '');

            var postUrl = $(this).attr('href');
            $.post(postUrl, null,
            function (res) {
                if (res.success) {
                    //parent.$.fancybox.close();
                    // displayMessage(res.data, 'lime', 5000);
                    if (res.url != null && res.url != "undefined") {
                        location.href = res.url;
                        return;
                    }
                    var grid = $(gridID).data("kendoGrid");
                    if (grid != null) {
                        grid.dataSource.read();
                        grid.refresh();
                        if (gridID == '#GridCaseAction') {
                            gridID = gridID + 'Completed';
                            if (gridID.indexOf("Grid") > 0) {
                                var gridDynamic = $(gridID).data("kendoGrid");
                                if (gridDynamic != null) {
                                    gridDynamic.dataSource.read();
                                    gridDynamic.refresh();
                                    return false;
                                }
                            }
                        }
                    }

                    var lstView = $(listViewID).data("kendoListView");
                    if (lstView != null) {
                        lstView.dataSource.read();
                        lstView.refresh();
                    }
                }
                else {
                    displayMessage(res.data, 'ruby', 10000);
                    //parent.$.fancybox.close();
                }
                return false;
            });
        }
        return false;
    });

    $('a[name="lnkEditor"]').live("click", function (e) {

        var getUrl = $(this).attr('href');

        var containerID = $(this).attr('data-container');
        $.get(getUrl, function (res) {

            $('#' + containerID).html(res);
            $('.ckeditor').ckeditor();
        });
        return false;
    });

    $('a[name="lnkEdit"]').live("click", function (e) {

        var getUrl = $(this).attr('href');
        var containerID = 'div' + $(this).attr('id').replace('lnkEdit', '');
        $.get(getUrl, function (res) {
            $('#' + containerID).html(res);
        });
        return false;
    });


    $('#btnClearForm').live("click", function (e) {
        var entityName = $(this).attr('name').replace('btnClearForm', '');
        var formID = 'frmEditor' + entityName;
        clearForm(formID);
        return false;
    });

    $('a[name="lnkLivingSituation"]').live("click", function (e) {
        $('#divImmediateNeeds').addClass("hide");
        $('#divEducation').addClass("hide");
        $('#divHealth').addClass("hide");
        $('#divHousing').addClass("hide");
        $('#divIncomeLivelihood').addClass("hide");
        $('#divAssetsProduction').addClass("hide");
        $('#divDignitySelfRespect').addClass("hide");
        $('#divSocialSupport').addClass("hide");
        var containerID = '#' + $(this).attr('id').replace('lnk', 'div');
        $('.menu-item').removeClass("active");
        $(this).parent().addClass("active");
        $(containerID).removeClass("hide");
        return false;
    });

    $('a[name="lnkServices"]').live("click", function (e) {
        $('#divServicesCurrentyUsed').addClass("hide");
        $('#divServicesCurrentyProposed').addClass("hide");
        var containerID = '#' + $(this).attr('id').replace('lnk', 'div');
        $('.menu-item').removeClass("active");
        $(this).parent().addClass("active");
        $(containerID).removeClass("hide");
        return false;
    });

    $('input[name="QualityOfLifeIDs"]').live("change", function (e) {
        var containerID = '#divQualityOfLife' + $(this).attr('id').replace('chkQualityOfLifeIDs', '');
        if ($(this).is(":checked")) {
            $(containerID).show();
        }
        else {
            $(containerID).hide();
        }
        return false;
    });

    $('input[name="QualityOfLifeCategoryIDs"]').live("change", function (e) {
        var containerID = '#divQualityOfLifeCategoryIDs_' + $(this).attr('id').replace('chkQualityOfLifeCategoryIDs_', '');
        if ($(this).is(":checked")) {
            $(containerID).show();
        }
        else {
            $(containerID).hide();
        }
        return false;
    });

    $('#btnCloseAction').live("click", function (e) {
        var gridID = '#GridCaseAction';
        var postUrl = "/CaseAction/CloseAjax";
        var selectedActions = "";
        $("input[name='chkCaseAction']").each(function () {
            if ($(this).attr('checked') == 'checked') {
                if (selectedActions == "") {
                    selectedActions = $(this).val();
                }
                else {
                    selectedActions = selectedActions + "," + $(this).val();
                }
            }
        });

        if (selectedActions.length <= 0) {
            alert("Please select at least one case action");
        }
        else {
            $.post(postUrl, { ids: selectedActions },
              function (res) {
                  if (res.success) {
                      var grid = $(gridID).data("kendoGrid");
                      if (grid != null) {
                          grid.dataSource.read();
                          grid.refresh();
                      }
                      var gridCaseActionClose = $("#GridCaseActionClose").data("kendoGrid");
                      if (gridCaseActionClose != null) {
                          gridCaseActionClose.dataSource.read();
                          gridCaseActionClose.refresh();
                      }
                      var alertContainer = $("#dvAjaxAlertContainer");
                      $(alertContainer).html(res.data);
                  }
                  else {
                      var alertContainer = $("#dvAjaxAlertContainer");
                      $('input[type=checkbox]').prop('checked', false);
                      var grid = $(gridID).data("kendoGrid");
                      if (grid != null) {
                          grid.dataSource.read();
                          grid.refresh();
                      }
                      var gridCaseActionClose = $("#GridCaseActionClose").data("kendoGrid");
                      if (gridCaseActionClose != null) {
                          gridCaseActionClose.dataSource.read();
                          gridCaseActionClose.refresh();
                      }
                      var alertContainer = $("#dvAjaxAlertContainer");
                      $(alertContainer).html(res.data);
                  }
                  return false;
              });
        }
    });

    onRequestEnd = function () {

        var ddl = $("#MemberStatusID").data("kendoDropDownList");

        ddl.options.optionLabel = "";
        ddl.refresh();

    }

    onRequestEnd1 = function () {

        var ddl = $("#RelationshipStatusID").data("kendoDropDownList");

        ddl.options.optionLabel = "";
        ddl.refresh();

    }
    onRequestEnd2 = function () {

        var ddl = $("#LanguageID").data("kendoDropDownList");

        ddl.options.optionLabel = "";
        ddl.refresh();

    }
    onRequestEnd3 = function () {

        var ddl = $("#CommunicationLanguageID").data("kendoDropDownList");

        ddl.options.optionLabel = "";
        ddl.refresh();

    }

    onRequestEnd4 = function () {

        var ddl = $("#GenderID").data("kendoDropDownList");

        ddl.options.optionLabel = "";
        ddl.refresh();


    }
    onRequestEnd5 = function () {

        var ddl = $("#MaritalStatusID").data("kendoDropDownList");

        ddl.options.optionLabel = "";
        ddl.refresh();

    }
    onRequestEnd6 = function () {

        var ddl = $("#CaseEthinicity").data("kendoDropDownList");

        ddl.options.optionLabel = "";
        ddl.refresh();

    }
    $('input[name="moduletype"]').live('change', function () {

        if ($(this).val() == 1) {
            $('#live').show();
            $('#local').hide();
            $('#filenameError').html('');
            $('#fileLocationError').html('');
            $('#fileName1Error').html('');
            $('#moduleFileError').html('');
        }
        else {
            $('#live').hide();
            $('#local').show();
        }
    });

    $('#submitModule').live('click', function () {

        if ($('input[name="moduletype"]:checked').val() == 1) {
            if ($('input[name="fileName"]').val().length == 0) {
                $('#filenameError').html('File name is required.');
            }
            if ($('input[name="fileLocation"]').val().length == 0) {
                $('#fileLocationError').html('File location is required.');
            }
            if ($('input[name="fileName"]').val().length > 0 && $('input[name="fileLocation"]').val().length > 0) {
                return true;
            }
            return false;
        }
        else {

            if ($('#localFile').val().length == 0) {

                $('#fileName1Error').html('File name is required.');
            }
            if ($('input[name="moduleFile"]').val().length == 0) {
                $('#moduleFileError').html('File is required.');
            }
            if ($('input[name="fileName1"]').val().length > 0 && $('input[name="moduleFile"]').val().length > 0) {
                return true;
            }
            return false;

        }
    });

    $('input[name="fileName"]').live('keyup', function () {
        if ($(this).val().length > 0) {
            $('#filenameError').html('');
        }
    });

    $('input[name="fileName1"]').live('keyup', function () {
        if ($(this).val().length > 0) {
            $('#fileName1Error').html('');
        }
    });
    $('input[name="fileLocation"]').live('keyup', function () {
        if ($(this).val().length > 0) {
            $('#fileLocationError').html('');
        }
    });
    $('input[name="moduleFile"]').live('change', function () {
        if ($(this).val().length > 0) {
            $('#moduleFileError').html('');
        }
    });
    $('.liveFile').live('click', function () {
        window.open('' + $(this).attr('data-location') + '', 'PoP_Up', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=1366,height=768');
    });
    GridCaseAssessmentLivingConditionParameter = function (e) {
        var o = {};
        try {
            o["CaseMemberID"] = $("#CaseMemberID").val();
            o["QualityOfLifeCategoryID"] = $("#QualityOfLifeCategoryID").val();
        }
        catch (ex) {
        }

        return o;
    };

    ListViewSmartGoalParameter = function (e) {
        var o = {};
        try {
            o["categoryId"] = $("#QualityOfLifeCategoryID").val();
        }
        catch (ex) {
        }

        return o;
    };

    GridCaseActionParameter = function (e) {
        var o = {};
        try {
            o["casesmartgoalserviceproviderid"] = $("#CaseActionServiceProviderID").val();
        }
        catch (ex) {
        }

        return o;
    };
});

//shamim-for manage Case 

function DropDownList_OnChanged(e) {
    var imgTickId = '#img' + e.sender.element[0].name;
    var img = $(imgTickId);
    if (img != null) {
        $(img).attr('src', '/images/tick.png');
    }
    if ((e.sender.element.val() > 0 && e.sender.element.val() != ' ') || (e.sender.element.val().length >= 2)) {
        $(imgTickId).show();
    }
    else {
        $(imgTickId).hide();
    }
}

function SmartGoalQualityOfLifeCategoryDropDownList_OnChanged(e) {
    var url = window.location.href;
    var indexOfParameter = url.indexOf('?');
    url = url.substring(0, indexOfParameter);
    url = url + '?CaseID=' + $('#CaseID').val() + '&CaseGoalID=' + $('#CaseGoalID').val() + '&CaseMemberID=' + $('#CaseMemberID').val() + '&QualityOfLifeCategoryID=' + e.sender.element.val();
    window.location.href = url;
}

function QualityOfLifeCategoryDropDownList_OnChanged(e) {
    var grid = $('#GridCaseAssessmentLivingCondition').data("kendoGrid");
    if (grid != null) {
        grid.dataSource.read();
        grid.refresh();
    }

    var lstViewSmartGoal = $('#ListViewSmartGoal').data("kendoListView");
    if (lstViewSmartGoal != null) {
        lstViewSmartGoal.dataSource.read();
        lstViewSmartGoal.refresh();
    }
}

function WorkerProgramDropDownList_OnChanged(e) {
    $("input:checkbox").attr('checked', false);
    var workerID = $('#ID').val();
    $.ajax({
        type: "GET",
        url: '/Worker/LoadSubProgramEditorAjax?programID=' + e.sender.element.val() + '&workerID=' + workerID,
        data: null,
        contentType: 'application/json; charset=utf-8',
        success: function (res) {
            $('#divSubProgram').html(res);
        },
        complete: function () {

        },
        error: function (req, status, error) {
            alert('error code-[' + status + "] and error desc-[" + error + "]");
        }
    });
}
function WorkerRoleForPermissionDropDownList_OnChanged(e) {
    $.ajax({
        type: "GET",
        url: '/WorkerInRoleNew/LoadRolePermissionsAjax?workerroleID=' + e.sender.element.val(),
        data: null,
        contentType: 'application/json; charset=utf-8',
        success: function (res) {
            $('#divRolePermissions').html(res);
        },
        complete: function () {

        },
        error: function (req, status, error) {
            alert('error code-[' + status + "] and error desc-[" + error + "]");
        }
    });
}
function PermissionProgramDropDownList_OnChanged(e) {
    //$("input:checkbox").attr('checked', false);
    var permissionID = $('#ID').val();
    $.ajax({
        type: "GET",
        url: '/Permission/LoadSubProgramEditorAjax?programID=' + e.sender.element.val() + '&permissionID=' + permissionID,
        data: null,
        contentType: 'application/json; charset=utf-8',
        success: function (res) {
            $('#divSubProgram').html(res);
        },
        complete: function () {

        },
        error: function (req, status, error) {
            alert('error code-[' + status + "] and error desc-[" + error + "]");
        }
    });
}
function PermissionRegionDropDownList_OnChanged(e) {
    //$("input:checkbox").attr('checked', false);
    var permissionID = $('#ID').val();
    $.ajax({
        type: "GET",
        url: '/Permission/LoadJamatkhanaEditorAjax?regionID=' + e.sender.element.val() + '&permissionID=' + permissionID,
        data: null,
        contentType: 'application/json; charset=utf-8',
        success: function (res) {
            $('#divJamatkhana').html(res);
        },
        complete: function () {

        },
        error: function (req, status, error) {
            alert('error code-[' + status + "] and error desc-[" + error + "]");
        }
    });
}


function SmartGoalDropDownList_OnChanged(e) {
    var CaseSmartGoalID = $('#CaseSmartGoalID').val();
    $.ajax({
        type: "GET",
        url: '/CaseSmartGoalProgress/GetCaseSmartGoalAssignmentAjax?SmartGoalID=' + e.sender.element.val() + '&CaseSmartGoalID=' + CaseSmartGoalID,
        data: null,
        contentType: 'application/json; charset=utf-8',
        success: function (res) {
            $('#SmartGoalStartDate').val(res.SmartGoalStartDate);
            $('#SmartGoalEndDate').val(res.SmartGoalEndDate);
        },
        complete: function () {

        },
        error: function (req, status, error) {
            alert('error code-[' + status + "] and error desc-[" + error + "]");
        }
    });
}

function WorkerRoleDropDownList_OnChanged(e) {
    $("input:checkbox").attr('checked', false);
    $.ajax({
        type: "GET",
        url: '/Lookup/RegionRole/LoadRegionByWorkerRoleIDAjax?workerRoleID=' + e.sender.element.val(),
        data: null,
        contentType: 'application/json; charset=utf-8',
        success: function (res) {
            $.each(res, function () {
                $('input[value="' + this.ID + '"]').attr('checked', true);
            });
        },
        complete: function () {

        },
        error: function (req, status, error) {
            alert('error code-[' + status + "] and error desc-[" + error + "]");
        }
    });
}

function SubProgramDropDownList_OnChanged(e) {
    $("input:checkbox").attr('checked', false);
    $.ajax({
        type: "GET",
        url: '/Lookup/RegionRole/LoadRegionBySubProgramIDAjax?subProgramID=' + e.sender.element.val(),
        data: null,
        contentType: 'application/json; charset=utf-8',
        success: function (res) {
            $.each(res, function () {
                $('input[value="' + this.ID + '"]').attr('checked', true);
            });
        },
        complete: function () {

        },
        error: function (req, status, error) {
            alert('error code-[' + status + "] and error desc-[" + error + "]");
        }
    });
}
function CaseSmartWorkerDropDownList_OnChanged(e) {
    var selectedValue = e.sender.element.val();
    $('#WorkerName').val('');
    if (selectedValue == 0) {
        $('#divOtherWorker').show();
    }
    else {
        $('#divOtherWorker').hide();
    }
}

function ServiceTypeDropDownList_OnChanged(e) {
    var ddlWorkerID = $('#WorkerID').data("kendoDropDownList");
    if (ddlWorkerID != null) {
        ddlWorkerID.dataSource.read();
        ddlWorkerID.refresh();
    }
}


function ServiceProviderDropDownList_OnChanged(e) {
    var provider = $('#ServiceProviderID :selected').text();

}

function CaseMemberDropDownList_OnChanged(e)
{
    var url = window.location.href;
    var indexOfParameter = url.indexOf('?');
    url = url.substring(0, indexOfParameter);
    url = url + '?CaseID=' + $('#CaseID').val() + '&CaseMemberID=' + e.sender.element.val();
    window.location.href = url;
}

function CaseActionServiceProviderDropDownList_OnChanged(e) {
    $("#CaseSmartGoalServiceProviderID").val(e.sender.element.val());
    var gridCaseAction = $("#GridCaseAction").data("kendoGrid");
    if (gridCaseAction != null) {
        gridCaseAction.dataSource.read();
        gridCaseAction.refresh();
    }
}

function ProgramDropDownList_OnSelect(e) {
    var varProgramID = $("#ProgramID").val();
    if (!varProgramID) {
        varProgramID = 0;
    }
    return {
        programID: varProgramID
    };
}
function CaseMemberDropDownList_OnSelect(e) {
    var varCaseMemberID = $("#CaseMemberIds").val();
    if (!varCaseMemberID) {
        varCaseMemberID = 0;
    }
    return {
        CaseMemberIds: varCaseMemberID
    };
}

function RegionAndProgramDropDownList_OnSelect(e) {
    var varProgramID = $("#ProgramID").val();
    if (!varProgramID) {
        varProgramID = 0;
    }
    var varRegionID = $("#RegionID").val();
    if (!varRegionID) {
        varRegionID = 0;
    }
    return {
        programID: varProgramID,
        regionID: varRegionID
    };
}
function WorkerRegionDropDownList_OnSelect(e) {
    return {
        workerRoleIDs: $("#TempWorkerRoleIDs").val()
    };
}
function onGridWorkerInRoleDataBound(arg) {
    var workerInRoleGrid = $("#GridWorkerInRole").data("kendoGrid");
    var workerRoleIDs = "";
    if (workerInRoleGrid != null) {
        var workerInRoleGridDataSource = workerInRoleGrid.dataSource;
        for (var i = workerInRoleGridDataSource._data.length - 1; i >= 0 ; i--) {
            try {
                if (workerRoleIDs != "") {
                    workerRoleIDs = workerRoleIDs + "," + workerInRoleGridDataSource._data[i].WorkerRoleID;
                }
                else {
                    workerRoleIDs = workerInRoleGridDataSource._data[i].WorkerRoleID;
                }
            }
            catch (inext) { }
        }
    }
    $('#TempWorkerRoleIDs').val(workerRoleIDs);
}
function onGridWorkerProgramDataBound(arg) {
    var workerInRoleGrid = $("#GridWorkerProgram").data("kendoGrid");
    var programIDs = "";
    if (workerInRoleGrid != null) {
        var workerInRoleGridDataSource = workerInRoleGrid.dataSource;
        for (var i = workerInRoleGridDataSource._data.length - 1; i >= 0 ; i--) {
            try {
                if (programIDs != "") {
                    programIDs = programIDs + "," + workerInRoleGridDataSource._data[i].ProgramID;
                }
                else {
                    programIDs = workerInRoleGridDataSource._data[i].ProgramID;
                }
            }
            catch (inext) { }
        }
    }
    $('#TempProgramIDs').val(programIDs);
}
function onGridWorkerRegionDataBound(arg) {
    var workerInRoleGrid = $("#GridWorkerRegion").data("kendoGrid");
    var regionIDs = "";
    if (workerInRoleGrid != null) {
        var workerInRoleGridDataSource = workerInRoleGrid.dataSource;
        for (var i = workerInRoleGridDataSource._data.length - 1; i >= 0 ; i--) {
            try {
                if (regionIDs != "") {
                    regionIDs = regionIDs + "," + workerInRoleGridDataSource._data[i].RegionID;
                }
                else {
                    regionIDs = workerInRoleGridDataSource._data[i].RegionID;
                }
            }
            catch (inext) { }
        }
    }
    $('#TempRegionIDs').val(regionIDs);
}
function WorkerSubProgramDropDownList_OnSelect(e) {
    return {
        regionIDs: $("#TempRegionIDs").val(),
        programIDs: $("#TempProgramIDs").val()
    };
}
function RegionDropDownList_OnSelect(e) {
    return {
        RegionID: $("#RegionID").val()
    };
}
function CaswWorkerRoleDropDownList_OnSelect(e) {

    return {
        RoleID: $("#RoleID").val()
    };
}
function FinancialAssistanceCategoryDropDownList_OnSelect(e) {
    return {
        categoryID: $("#FinancialAssistanceCategoryID").val(),
        regionID: $("#RegionID").val()
    };
}
function RoleDropDownList_OnSelect(e) {

    return {
        RoleID: $("#RoleID").val()

    };
}
function ServiceTypeDropDownList_OnSelect(e) {
    return {
        serviceTypeID: $("#ServiceTypeID").val()
    };
}

function RegionAndServiceTypeDropDownList_OnSelect(e) {

    return {
        serviceTypeID: $("#ServiceTypeID").val(),
        regionID: $("#RegionID").val()
    };
}
function PermissionAndWorkerRoleDropDownList_OnSelect(e) {

    return {

        workerroleID: $("#WorkerRoleID").val(),
        permissionID: $("#PermissionID").val()
    };
}
function ServiceProviderDropDownList_OnSelect(e) {
    return {
        serviceProviderID: $("#ServiceProviderID").val()
    };
}
function QualityOfLifeCategoryDropDownList_OnSelect(e) {
    return {
        categoryId: $("#QualityOfLifeCategoryID").val()
    };
}
function AssessmentTypeDropDownList_OnChange(e) {
    var caseID = $('#CaseID').val();
    var caseMemberID = $("#CaseMemberID").val();
    var assessmentTypeID = e.sender.element.val();
    if (assessmentTypeID == 3) {
        if (caseMemberID && caseMemberID > 0) {
            location.href = '/CaseManagement/CaseAssessment/Termination?CaseID=' + caseID + '&CaseMemberID=' + caseMemberID;
        }
        else {
            alert('Please select a family or family member to discharge');
        }
    }
}


function DropDownListEthnicity_OnChange(e) {
    console.log(e);
    var data = [];
    data = $('#CaseEthinicity').val();
    $(data).each(function (i, item) {
        console.log(item);
        var selectedText = $('#CaseEthinicity option[value="' + item + '"]').text();
        if (selectedText == 'Other') {


            $('#divEthnicityOther').removeClass('hide');
            $('#divEthnicity').removeClass('col-sm-8');
            $('#divEthnicity').addClass('col-sm-4');
        }
        else {
            $('#divEthnicityOther').addClass('hide');
            $('#divEthnicityOther').find('input').val('');
            $('#divEthnicity').removeClass('col-sm-4');
            $('#divEthnicity').addClass('col-sm-8');
        }
    });
    console.log($('#CaseEthinicity').val());
}

function DropDownListOther_OnChange(e) {
    var ddlName = e.sender.element[0].name;
    var ddlText = $("#" + ddlName).data("kendoDropDownList").text();;
    var divOther = '#div' + ddlName.replace('ID', '') + 'Other';
    var divDdlContainer = '#div' + ddlName.replace('ID', '');
    if (ddlText.indexOf("Other") >= 0) {
        $(divOther).removeClass('hide');
        $(divDdlContainer).removeClass('col-sm-8');
        $(divDdlContainer).addClass('col-sm-4');
    }
    else {
        $(divOther).addClass('hide');
        $(divDdlContainer).removeClass('col-sm-4');
        $(divDdlContainer).addClass('col-sm-8');
    }

    if (ddlName == 'ServiceProviderID' && ddlText.indexOf("Other") >= 0) {
        $('#divOtherProviderLink').show();
    }
    else {
        $('#divOtherProviderLink').hide();
    }
}

function ProgramDropDownList_OnChanged(e) {
    var ddlRegionID = $('#RegionID').data("kendoDropDownList");
    if (ddlRegionID != null) {
        ddlRegionID.dataSource.read();
        ddlRegionID.refresh();
    }
    var ddlSubProgramID = $('#SubProgramID').data("kendoDropDownList");
    if (ddlSubProgramID != null) {
        //ddlSubProgramID.dataSource.data({}); // clears dataSource
        //ddlSubProgramID.text(""); // clears visible text
        //ddlSubProgramID.value(""); // clears invisible value
        ddlSubProgramID.dataSource.read();
        ddlSubProgramID.refresh();
    }
}
function MultipleProgramDropDownList_OnChanged(e)
{
    var ddlRegionID = $('#RegionID').data("kendoMultiSelect");
    var ddlJamatkhanaID = $('#JamatkhanaID').data("kendoMultiSelect");
    var ddlSubProgramID = $('#SubProgramID').data("kendoMultiSelect");

    if (ddlJamatkhanaID != null) {
        ddlJamatkhanaID.value("");
        ddlJamatkhanaID.input.blur();
    }
    if (ddlSubProgramID != null) {
        ddlSubProgramID.value("");
        ddlSubProgramID.input.blur();
    }

    if (ddlRegionID != null)
    {
        ddlRegionID.value("");
        ddlRegionID.input.blur();

        ddlRegionID.dataSource.read();
        ddlRegionID.refresh();
    }
    //var ddlSubProgramID = $('#SubProgramID').data("kendoMultiSelect");
    //if (ddlSubProgramID != null) {
    //    //ddlSubProgramID.dataSource.data({}); // clears dataSource
    //    //ddlSubProgramID.text(""); // clears visible text
    //    //ddlSubProgramID.value(""); // clears invisible value
    //    ddlSubProgramID.dataSource.read();
    //    ddlSubProgramID.refresh();
    //}

    //var ddlJamatkhanaID = $('#JamatkhanaID').data("kendoMultiSelect");
    //if (ddlJamatkhanaID != null) {
    //    ddlJamatkhanaID.dataSource.read();
    //    ddlJamatkhanaID.refresh();
    //}
}
function MultipleRegionDropDownList_OnChanged(e) {
    var ddlJamatkhanaID = $('#JamatkhanaID').data("kendoMultiSelect");
    var ddlSubProgramID = $('#SubProgramID').data("kendoMultiSelect");

    if (ddlJamatkhanaID != null) {
        ddlJamatkhanaID.value("");
        ddlJamatkhanaID.input.blur();
    }
    if (ddlSubProgramID != null) {
        ddlSubProgramID.value("");
        ddlSubProgramID.input.blur();
    }
    if ($('#ProgramID').val() != null && $('#RegionID').val() != null) {
        ddlJamatkhanaID.dataSource.read();
        ddlJamatkhanaID.refresh();

        ddlSubProgramID.dataSource.read();
        ddlSubProgramID.refresh();
    }
}
function MultipleProgramDDL_OnSelect(e) {
    var data = [];
    data = $('#ProgramID').val();
    var programids = "";
    if (data != null) {
        programids = data.join(', ');
    }
    return {
        programIDs: programids
    };
}

function MultipleRegionDDL_OnSelect(e) {
    var data = [];
    data = $('#RegionID').val();
    var regionids = "";
    if (data != null) {
        regionids = data.join(', ');
    }
    return {
        regionIDs: regionids
    };
}

function MultiRegionAndProgramDDL_OnSelect(e) {
    debugger
    var dataprogram = [];
    dataprogram = $('#ProgramID').val();
    var programids = "";
    if (dataprogram != null) {
        programids = dataprogram.join(', ');
    }
    var dataregion = [];
    dataregion = $('#RegionID').val();
    var regionids = "";
    if (dataregion != null) {
        regionids = dataregion.join(', ');
    }
    return {
        programIDs: programids,
        regionIDs: regionids
    };
}
function ServiceRegionDropDownList_OnChanged(e) {
    if ($('#ServiceTypeID').val() > 0) {
        var ddlRegionID = $('#ServiceProviderID').data("kendoDropDownList");
        if (ddlRegionID != null) {
            ddlRegionID.dataSource.read();
            ddlRegionID.refresh();
        }
    }

}

function CaseMemberIdsDropDownList_OnChanged(e) {
    var ddlCaseWorkerID = $('#CaseWorkerID').data("kendoDropDownList");
    if (ddlCaseWorkerID != null) {
        ddlCaseWorkerID.dataSource.read();
        ddlCaseWorkerID.refresh();
    }

}

function RegionDropDownList_OnChanged(e) {
    var ddlSubProgramID = $('#SubProgramID').data("kendoDropDownList");
    if (ddlSubProgramID != null) {
        ddlSubProgramID.dataSource.read();
        ddlSubProgramID.refresh();
    }
}



function RoleDropDownList_OnChanged(e) {
    var ddlSubProgramID = $('#WorkerID').data("kendoDropDownList");
    if (ddlSubProgramID != null) {
        ddlSubProgramID.dataSource.read();
        ddlSubProgramID.refresh();
    }
}

//function RelationStatusDropDownList_OnChanged(e) {
//    if (e.sender.text() == "self" || e.sender.text() == "Self" || e.sender.text() == "SELF")
//        $('#selfDiv').css('display', 'block');
//    else
//    {
//        $('#LanguageID').val('');
//        $('#DateOfBirth').val('');
//        $('#GenderID').val('');
//        $('#EthnicityID').val('');
//        $('#MaritalStatusID').val('');
//        $('#selfDiv').css('display', 'none');
//    }
//}



function CheckOtherClick(cb) {
    if (cb.checked) {
        $('#divSmartGoalOther').removeClass('hide');
    }
    else {
        $('#divSmartGoalOther').addClass('hide');
    }
}

$(function () {
    $("#offset").spinner({
        step: 5,
        min: 0,
        stop: function (event, ui) {

            //var offsetValue = $("#offset").spinner("value");
            //var limitValue = $("#limit").spinner("value");

            //if (offsetValue >= limitValue) {
            //    $("#limit").spinner("value", offsetValue + 10);
            //}
        }

    });
    $('#limit').spinner({
        step: 5,
        min: 0,
        stop: function (event, ui) {

            //var offsetValue = $("#offset").spinner("value");
            //var limitValue = $("#limit").spinner("value");

            //if (offsetValue >= limitValue) {
            //    $("#limit").spinner("value", offsetValue + 10);
            //}
        }
    });
    //$('#generateCSV').live('click', function () {
    //    var offsetValue = $("#offset").spinner("value");
    //    var limitValue = $("#limit").spinner("value");
    //    if (offsetValue > 0 && limitValue > 0) {
    //        return true;
    //    }
    //    else {
    //        alert('Start From & Number of Records are required.');
    //        return false;
    //    }
    //});
});
//end of document.ready funtion
