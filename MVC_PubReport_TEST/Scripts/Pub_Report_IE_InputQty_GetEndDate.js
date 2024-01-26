$(document).ready(function () {
    var $txtDT1 = $("#dt1")

    var dt = showdate(-1);
    $txtDT1.val(dt);
    var $ddPU = $('#PU');

    $('#dd1').calendar({
        trigger: '#dt1',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected')
        },
        onClose: function (view, date, data) {
            var Date;            
            Date = document.getElementById("dt").value + ";" + $txtDT1.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            }
            
            var url = baseUrl + "/Query/IE_InputQty_GetPU";
            jsonData = { "Date": Date }
            ajaxJsonRequestDerived(url, 'post', jsonData, IE_InputQty_GetPU, AjaxFailResult);
        }
    })

    function IE_InputQty_GetPU(data) {
        $ddPU.empty();
        var optionStr = "<option value='' >--Please Select--</option>";
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {

            var puList = data.tableData;

            for (var i = 0; i < puList.length; i++) {
                optionStr = optionStr + "<option value='" + puList[i] + "' >" + puList[i] + "</option>";
            }
        }
        var option = $(optionStr);
        $ddPU.append(option);
    }
});