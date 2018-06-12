$('#filterText').keypress(function (e) {
    var key = e.which;
    if (key === 13)  // the enter key code
    {
        $('#searchButton').trigger('click');
        return false;
    }
});

$("#searchButton").click(function (e) {
    var url = "./api/items";

    $.ajax({
        type: "GET",
        url: url,
        data: $('#searchForm').serialize(),
        dataType: "json",
        contentType: "application/json",
        success: function (response) {
            console.log("Got here");
            console.log(response);
            //var json = $.parseJSON(response);
            outputData(response);
        },
        error: function (jqXHR, textStatus, errorThrown) {

            $('#result').html('<p>status code: ' + jqXHR.status + '</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>' + jqXHR.responseText + '</div>');
            console.log('jqXHR:');
            console.log(jqXHR);
            console.log('textStatus:');
            console.log(textStatus);
            console.log('errorThrown:');
            console.log(errorThrown);
        },
        timeout: 3000
    });
});

$("#outputDiv").on('click', '.pageButton', function () {
    console.log("IN link click");
    var dataId = $(this).data("id");
    console.log(dataId);

    $.ajax({
        type: "GET",
        url: dataId,
        dataType: "json",
        contentType: "application/json",
        success: function (response) {
            console.log("Got here");
            console.log(response);
            //var json = $.parseJSON(response);
            outputData(response);
        },
        error: function (jqXHR, textStatus, errorThrown) {

            $('#result').html('<p>status code: ' + jqXHR.status + '</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>' + jqXHR.responseText + '</div>');
            console.log('jqXHR:');
            console.log(jqXHR);
            console.log('textStatus:');
            console.log(textStatus);
            console.log('errorThrown:');
            console.log(errorThrown);
        },
        timeout: 3000
    });
});

function outputData(response) {
    $('#outputDiv').empty();
    $.each(response.items,
        function (index, value) {
            var thumbNailUrl = getThumbnailTag(value.standardThumbnail);
            if (!thumbNailUrl) {
                thumbNailUrl = getThumbnailTag(value.mediumThumbnail);
            }
            if (!thumbNailUrl) {
                thumbNailUrl = getThumbnailTag(value.highResThumbnail);
            }
            if (!thumbNailUrl) {
                thumbNailUrl = getThumbnailTag(value.maxResThumbnail);
            }
            console.log(thumbNailUrl);


            var line = '<div class="card mx-auto" style="width:400px">';
            line += '<div class="card-header text-center">' + value.title + '</div>';
            line += '<div class="card-body text-center">' + thumbNailUrl + '</div>';
            line += '</div>';
            $('#outputDiv').append(line);
        });

    var finalLine = '<div class="row">';
    finalLine += '<div class="col"/>';

    var previousPageUrl = response.previousPageUrl;
    if (previousPageUrl) {
        finalLine += '<div class="col"><a class="pageButton col" href="#" data-id="' + previousPageUrl + '">Previous Page</a></div>';
    }

    var nextPageUrl = response.nextPageUrl;
    if (nextPageUrl) {
        finalLine += '<div class="col"><a class="pageButton col" href="#" data-id="' + nextPageUrl + '">Next Page</a></div>';
    }

    finalLine += '<div class="col"/>';
    finalLine += '</div>';

    $('#outputDiv').append(finalLine);
}

function getThumbnailTag(thumbNail) {
    console.log(thumbNail);
    if (thumbNail) {
        var output = '<img src="' + thumbNail.url + '"';
        if (thumbNail.height) {
            output += ' height="' + thumbNail.height + '"';
        }
        if (thumbNail.width) {
            output += ' width = "' + thumbNail.width + '"';
        }
        output += '/>';
        return output;
    }
    return "";
}