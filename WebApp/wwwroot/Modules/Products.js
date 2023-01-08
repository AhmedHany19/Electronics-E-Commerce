$(document).ready(function () {
    $('#tableProducts').DataTable({
        "autoWidth": false,
        "responsive": true
    });
    $('#tableLogProducts').DataTable({
        "autoWidth": false,
        "responsive": true
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
            window.location.href = `/Admin/Products/Delete?Id=${id}`;
            Swal.fire(
                lbTitleDeletedOk,
                lbMsgDeletedOkProduct,
                lbSuccess
            )
        }
    })
}

function DeleteLog(id) {

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
            window.location.href = `/Admin/Products/DeleteLog?Id=${id}`;
            Swal.fire(
                lbTitleDeletedOk,
                lbMsgDeletedOkProduct,
                lbSuccess
            )
        }

    })
}


Edit = (id, name, description, category, brand, color, price, image, discount) => {
    document.getElementById("title").innerHTML = lbAddNewProduct;
    document.getElementById("btnSave").value = lbEdit;
    document.getElementById("productId").value = id;
    document.getElementById("productName").value = name;
    document.getElementById("description").value = description;
    document.getElementById("ddlCategory").value = category;
    document.getElementById("ddlBrand").value = brand;
    document.getElementById("color").value = color;

    var Active = document.getElementById("productAvailable");
    if (active == "True")
        Active.checked = true;
    else
        Active.checked = false;

    document.getElementById("price").value = price;
    document.getElementById("discount").value = discount;
    document.getElementById("image").hidden = false;
    document.getElementById("image").src = PathImageProduct + image;
    document.getElementById("imgeHide").value = image;
}

Rest = () => {
    document.getElementById("title").innerHTML = lbAddNewCategory;
    document.getElementById("btnSave").value = "";
    document.getElementById("productId").value = "";
    document.getElementById("productName").value = "";
    document.getElementById("description").value = "";
    document.getElementById("ddlCategory").value = "";
    document.getElementById("ddlBrand").value = "";
    document.getElementById("color").value = "";

    var Active = document.getElementById("productAvailable");
    if (active == "True")
        Active.checked = true;
    else
        Active.checked = false;

    document.getElementById("price").value = "";
    document.getElementById("discount").value = "";

    document.getElementById("image").hidden = false;
    document.getElementById("image").src = "" + "";
    document.getElementById("imgeHide").value = "";
}

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