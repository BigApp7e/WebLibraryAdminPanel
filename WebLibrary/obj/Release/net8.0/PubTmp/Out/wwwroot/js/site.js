$(document).ready(function () {
    $('.delbtn').click(function () {

        let rowData = $(this).closest('tr').find('td').map(function () {
            return $(this).text();
        }).get();

        localStorage.setItem("deleteId", rowData[0]);
        $("#deleteModalCenter").modal('show');
    });

    $('.dellNo').click(function () {
        $("#deleteModalCenter").modal('hide');
    });

    $('.dellOk').click(function () {
        $("#deleteModalCenter").modal('hide');
        sendToDelete(localStorage.getItem("deleteId"))
    });


    $('.editBtn').click(function () {

        let rowData = $(this).closest('tr').find('td').map(function () {
            return $(this).text();
        }).get();

        $('#edit-id').val(rowData[0])
        $('#edit-аuthor').val(rowData[1]);
        $('#edit-title').val(rowData[2]);

        $("#edit-type option[value='" + rowData[3] + "']").attr('selected', 'selected');
       // $('select[name="edit-type"]').find  ('edit-type:contains("Book")').attr("selected", true);
      //  $('#edit-type').val(rowData[3]);

        $("#edithModalCenter").modal('show');
    });

    $('.btnSaveChanges').click(function () {

        var data = {
            "Id": $('#edit-id').val(),
            "Author": $('#edit-аuthor').val(),
            "Title": $('#edit-title').val(),
            "Type": $('#edit-type').val()
        }
        $("#edithModalCenter").modal('hide');
        sendToEdit(data);
    });
});


function sendToEdit(data) {

    $.ajax({
        type: 'POST',
        data: JSON.stringify(data),
        url: "/Home/ReceiveDataToEdit",
        contentType: "application/json",
        success: function (data) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            console.error("Error adding person:", xhr.responseText);
        }
    });

}
function sendToDelete(id) {

    var data = { "Id": id };

    $.ajax({
        type: 'POST',
        data: JSON.stringify(data),
        url: "/Home/ReceiveDataToDelete",
        contentType: "application/json",  
        success: function (data) {
            window.location.reload();
        },
        error: function (xhr, status, error) {
            console.error("Error adding person:", xhr.responseText);
        }
    });
}