$(function () {
    var uri = 'api/values';

    function clearData() {
        $('.geocache_items_wrapper').css('display', '');
        $('.geocache_info').val('');
        $(".responseMSG").removeClass("success");
        $(".responseMSG").html("");

        $('#save_geocache').css('display', 'none');
        $('#delete_geocache').css('display', 'none');
    }

    function setupForNew() {
        clearData();

        $('input.editable').removeAttr('readonly');
        $('#save_geocache').css('display', '');
    }

    function setupForDisplay(data) {
        clearData();

        if (data) {
            if (data.GeocacheID){
                $('input[name="GeocacheID"]').val(data.GeocacheID);
            }
            if (data.Name){
                $('input[name="Name"]').val(data.Name);
            }
            if(data.Latitude){
                $('input[name="Latitude"]').val(data.Latitude);
            }
            if(data.Longitude){
                $('input[name="Longitude"]').val(data.Longitude);
            }
        }

        $('input.editable').attr('readonly', 'readony');
        $('#delete_geocache').css('display', '');
    }

    function currentGeocacheItem() {
        return {
            GeocacheID: $('input[name="GeocacheID"]').val(),
            Name: $('input[name="Name"]').val(),
            Latitude: $('input[name="Latitude"]').val(),
            Longitude: $('input[name="Longitude"]').val()
        };
    }

    $('#geocache_items').change(function () {
        var geocacheID = $(this).find(':selected').val();
        //ajax the info for the geocache selected.
        if (geocacheID) {
            $.getJSON(uri + "/" + geocacheID, function (data) {
                if (data) {
                    setupForDisplay(data);
                }
            });
        }
    });

    $('#create_geocache').click(function () {
        setupForNew();
    });

    $('#save_geocache').click(function () {
        $("span.error").html('');
        $.ajax({
            url: uri,
            data: $("#saveContactForm").serialize(),
            type: "post",
            statusCode: {
                200: function (value) {
                    if (value > 0) {
                        var geoItem = currentGeocacheItem();
                        geoItem.GeocacheID = value;
                        setupForDisplay(geoItem);

                        $('#geocache_items').append('<option value="' + value + '" >' + $('input[name="Name"]').val() + "</option>");
                        $(".responseMSG").html("Saved Geocache Items Successfully.");
                        $(".responseMSG").addClass("success");
                    }
                },
                400: function (response) {
                    var errors = $.parseJSON(response.responseText);
                    $.each(errors.ModelState, function (i, ival) {
                        var errorName = "error" + i.split(".")[1];
                        $("span." + errorName).html(ival);
                    });
                },
                406: function (response) {
                    var msg = $.parseJSON(response.responseText);
                    $(".responseMSG").html(msg.Message);
                },
            }
        });
    });

    /*$('#delete_geocache').click(function () {
        var geocacheID = $('input[name="GeocacheID"]').val()
        $.ajax({
            url: uri + "/" + geocacheID,
            type: "delete",
            statusCode: {
                200: function (value) {
                    $("select[name='geocache_items']").find("option[value='" + geocacheID + "']").remove();
                    clearData();
                    $(".responseMSG").addClass("success");
                    $(".responseMSG").html("Removed Geocache Item");
                },
                406: function (response) {
                    var msg = $.parseJSON(response.responseText);
                    $(".responseMSG").removeClass("success");
                    $(".responseMSG").html(msg.Message);
                },
            }
        });
    });*/
});