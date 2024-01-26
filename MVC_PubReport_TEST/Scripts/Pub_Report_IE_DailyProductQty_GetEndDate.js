$(document).ready(function () {
    var $txtDT1 = $("#dt1");
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


            url = baseUrl + "/Query/IE_DailyProductQty_GetPU";
            jsonData = { "Date": Date }
            ajaxJsonRequestDerived(url, 'post', jsonData, IE_DailyProductQty_GetPU, AjaxFailResult);
        }
    });


    function IE_DailyProductQty_GetPU(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $ddPU.empty();
            var puList = data.tableData;
            var optionStr = "<option value='' >--Please Select--</option>";
            for (var i = 0; i < puList.length; i++) {
                optionStr = optionStr + "<option value='" + puList[i] + "' >" + puList[i] + "</option>";
            }
            var option = $(optionStr);
            $ddPU.append(option);

        }
    }
})