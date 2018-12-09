//import { json } from "d3";

var guestID = -1;

$(".eventGuest").click(function () {
    if (!$(this).hasClass("is-seated")) {
        guestID = ($(this).attr('id'));
    }
});



$(".EventSeat").click(function () {

    if (guestID != -1) {
        var selectedID = ($(this).attr('id'));
        var data = {
            "seatid": selectedID,
            "guestid": guestID,
        }

        $.ajax({
            type: "POST",
            dataType: 'json',
            data: data,
            url: '/Events/seatperson/' + selectedID,
            success: function (data) {
                if (data['guestid'] != -1 && data['seatid'] != -1) {
                    alert(data['sponsorshipLevel'])
                    //here we check the sponsorship level attached to the eventguest and then assign it to the event seat to add the correct classes.
                    if (data['sponsorshipLevel'] == "Gold") {
                        $("#" + selectedID + ".EventSeat").addClass("is-gold").addClass("has-guest");
                        $("#" + guestID + ".eventGuest").addClass("is-seated");
                        guestID = -1;
                    }
                    if (data['sponsorshipLevel'] = "Bronze") {
                        $("#" + selectedID + ".EventSeat").addClass("is-bronze").addClass("has-guest");
                        $("#" + guestID + ".eventGuest").addClass("is-seated");
                        guestID = -1;
                    }
                    if (data['sponsorshipLevel'] = "Silver") {
                        $("#" + selectedID + ".EventSeat").addClass("is-silver").addClass("has-guest");
                        $("#" + guestID + ".eventGuest").addClass("is-seated");
                        guestID = -1;
                    }
                    if (data['sponsorshipLevel'] = "Platinum") {
                        $("#" + selectedID + ".EventSeat").addClass("is-platinum").addClass("has-guest");
                        $("#" + guestID + ".eventGuest").addClass("is-seated");
                        guestID = -1;
                    }
                    if (data['sponsorshipLevel'] = "SuperSpecial") {
                        $("#" + selectedID + ".EventSeat").addClass("is-super").addClass("has-guest");
                        $("#" + guestID + ".eventGuest").addClass("is-seated");
                        guestID = -1;
                    }
                  else {
                        $("#" + selectedID + ".EventSeat").addClass("no-sponsor").addClass("has-guest");
                        $("#" + guestID + ".eventGuest").addClass("is-seated");
                        guestID = -1;
                    }


                }
                window.location = location.href;
            },
            error: function (xhr) {
                console.log(xhr.statusCode);
            }

        });
    }
});

$("body").on("click", "[name='LundSeat'].has-guest", function () {
    var seatID = $(this).attr('id');
    console.log(seatID);
    $.ajax({
        type: "POST",
        dataType: 'json',
        data: {
            seatID: seatID
        },
        url: '/Events/onHover/',
        success: function (data) {
            $(".modal-body").html("Guest Name: " + data[2] + "<br>Sponsor Name: " + data[3] + "<br>Seat Section: " + data[4] + "<br>Seat Row: " + data[5]);
            $("#seated-person").modal("show");

            $(".modal-extra-footer").html("<input type='hidden' name='gst-id' value='" + data[1] + "' id='gst-id'><input type='hidden' name='set-id' value='" + data[0] + "' id='set-id'> ");

            
        },
        
        error: function (xhr) {
            console.log(xhr.statusCode);
        }
    });
});



$("body").on("click", "#modal-remove-guest", function () {
    var seatID = $("#set-id").val();
    var guestID = $("#gst-id").val();


    $.ajax({
        type: "POST",
        dataType: 'json',
        data: {
            seatID: seatID
        },
        url: '/Events/unseatGuest/',
        success: function () {

            $("#seated-person").modal("hide");
            $("#" + seatID + ".has-guest").removeClass("has_guest")
            
            $("p#" + guestID + ".is-seated").removeClass("is-seated")
            window.location = location.href;
        },
        error: function (xhr) {
            console.log(xhr.statusCode);
        }
    });
});



$('#Check_all_guests').click(function () {
    $('input:checkbox').not(this).prop('checked', this.checked);
});

$('#Check_all_sponsors').click(function () {
    $('input:checkbox').not(this).prop('checked', this.checked);
});

$('#Check_all_guests_for_sponsor').click(function () {
    $('input:checkbox').not(this).prop('checked', this.checked);
});




