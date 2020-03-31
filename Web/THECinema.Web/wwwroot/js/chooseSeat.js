$(".seatStructure *").prop("disabled", true);

$(":checkbox").click(function () {
    if ($("input:checked").length == ($("#numseats").val())) {
        $(":checkbox").prop('disabled', true);
        $(':checked').prop('disabled', false);
        $(".taken *").prop("disabled", true);
    } else {
        $(":checkbox").prop('disabled', false);
        $(".taken *").prop("disabled", true);
    }
});

function getChosenSeats() {
    if ($("input:checked").length == ($("#numseats").val())) {
        $(".seatStructure *").prop("disabled", true);

        var allSeatsVals = [];

        $('#hall :checked').each(function () {
            allSeatsVals.push($(this).val());
        });

        $("#selectedSeats").val(allSeatsVals.toString());
        $("#priceReady").val($("#price").val());

    } else {
        alert("Please select " + ($("#numseats").val()) + " seats")
    }
}
