$(function () {
    $('.typeButton').on('click', function myFunction() {
        event.preventDefault();  
        const workTypeName = $(this).data('work-type'); // Get the work type name
        console.log("Selected WorkType:", workTypeName);

        $.ajax({
            url: "/services/FilterByWorkType",
            method: "GET",
            data: { type: workTypeName },
            contentType: "application/json",
            success: function (response) {
                $('#workRow').html(response)
            }
        })
    })
})
