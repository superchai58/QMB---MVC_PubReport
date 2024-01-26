

function getJsonRequest(url, done) {
    $.get(url)
    .done(function (data, status, jqXHR) { if (done != null) done(data, status, jqXHR); });
}

function ajaxJsonRequestNative(url, json, done, always) {
    $.post('../../api/ApiPool/OrderApi/', json)
    .done(function (data) { if (done != null) done(data); })
    .always(function (xhr, status, err) { if (always != null) always(xhr, status, err); });
}

function ajaxJsonRequestDerived(url, type, json, success, always) {
    $.ajax({
        url: url,
        cache: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        dataType: 'json',
        //async: false,
        
        success: function (data) {   if (success != null) success(data) ; } 
        //error: function () { alert("系统出现异常，请联系QMS!"); }

    })
    .always(function (xhr, status, err) { if (always != null) always(xhr, status, err); });
}

function ajaxJsonRequestFile(url, type, sendData, success, always) {
    $.ajax({
        url: url,
        cache: false,
        type: 'POST',
        contentType: false,
        data: sendData,
      
        processData:false,
   

        success: function (data) { if (success != null) success(data); }
        //error: function () { alert("系统出现异常，请联系QMS!"); }

    })
    .always(function (xhr, status, err) { if (always != null) always(xhr, status, err); });
}


function formValidation() {
    var validation = true;

    $("input[type='text']").each(function () {
        if ($(this).attr("required") == "required") {
            if ($(this).val().trim().length == 0) {
                alert($(this).attr("aria-label") + " is required!!");
                $(this).focus();
                validation = false;
                return false;
            }
            else if ($(this).attr("itemtype") == "DECIMAL" && !$.isNumeric($(this).val())) {
                alert($(this).attr("aria-label") + " must be an integer!!");
                $(this).focus();
                validation = false;
                return false;
            }
        }
    });

    return validation;
}

function formReset(callBack) {
    $("input[type='text']").each(function () {
        $(this).val("");
    });

    if (callBack != null) callBack();
}

function msgbox(response, role) {
    if (role == "Z") alert(response);
    else alert("System got unexpected exception; please try again later!!");
}

