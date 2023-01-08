$(document).ready(function () {
    $('#tableCategory').DataTable({
        "autoWidth": false,
        "responsive":true
    });   
});

function Delete(id) {
    Swal.fire({
        title: lbTitleMsgDelete,
        text: lbTextMsgDelete,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: lbconfirmButtonText,
        cancelButtonText: lbcancelButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/Admin/Sliders/Delete?Id=${id}`;
            Swal.fire(
                lbTitleDeletedOk,
                lbMsgDeletedOkCategory,
                lbSuccess
            )
        }
    })
}



Edit = (id, image, name) => {
    document.getElementById("title").innerHTML = lbTitleEditCategory;
    document.getElementById("btnSave").value = lbEdit;
    document.getElementById("sliderId").value = id;
    document.getElementById("sliderName").value = name;

    document.getElementById("image").hidden = false;
    document.getElementById("image").src = PathImageSlider + image;
    document.getElementById("imgeHide").value = image;

    
}

Rest = () => {
    document.getElementById("title").innerHTML = lbAddNewCategory;
    document.getElementById("btnSave").value = lbbtnSave;

    document.getElementById("image").hidden = false;
    document.getElementById("image").src = "" + "";
    document.getElementById("imgeHide").value = "";
    document.getElementById("sliderName").value = "";



    document.getElementById("defaultOpen").click();


    function openCity(evt, cityName) {
        // Declare all variables
        var i, tabcontent, tablinks;

        // Get all elements with class="tabcontent" and hide them
        tabcontent = document.getElementsByClassName("tabcontent");
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }

        // Get all elements with class="tablinks" and remove the class "active"
        tablinks = document.getElementsByClassName("tablinks");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }

        // Show the current tab, and add an "active" class to the button that opened the tab
        document.getElementById(cityName).style.display = "block";
        evt.currentTarget.className += " active";
    }
}