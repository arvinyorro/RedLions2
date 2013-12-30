function simpleSearch()
{
    $.ajax({
        url: url.filterMembers,
        data: {
            "fullName": fullName 
            },
        type: "POST",
        cache: false,
        success: function (partialView) {
            // $("#global-workcodes-partial").html(data);
        },
        error: function (xhr, ajaxOptions, thrownError) { 
        }
    });
}