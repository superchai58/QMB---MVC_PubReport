$(document).ready(function () {
    //每个query网页都有的控件
    var $tbResultTable = $('#ResultTable');
    var $btnToExcel = $('#ToExcel');
    var $btnQuery = $('#Query');
    var $btnReset = $('#Reset');
    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');
    //


    var $txtDT = $("#dt");
    var $Calendar = $('#dd');
    var $ddPU = $('#PU');
    var $txtLine = $('#Line');
    var $ddShift = $('#Shift');
    var $txtModel = $('#Model');
    var $txtWo = $('#WO');

    var dt = showdate(-1);
    $txtDT.val(dt);

    //绑定日历
    var $Calendar = $('#dd');
    $Calendar.calendar({
        trigger: '#dt',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected')
        },
        onClose: function (view, date, data) {
            var Date;
            Date = $txtDT.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            }

            //url = baseUrl + "/QueryController/IE_InputQty_GetPU";
            //jsonData = { "Date": Date }
            //ajaxJsonRequestDerived(url, 'post', jsonData, IE_InputQty_GetPU, AjaxFailResult);
        }
    });

    function IE_DailyProductQty_GetPU(data) {
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

    $btnReset.click(function () {

        $ddPU.val("");
        $txtLine.val("");
        $ddShift.val("");
        $txtModel.val("");
        $txtWo.val("");
    });
    $btnQuery.click(function () {
        var Date = "";
        var PU = "";
        var Line = "";
        var Shift = "";
        var Model = "";
        var WO = "";
        Date = document.getElementById("dt").value + ";" + document.getElementById("dt1").value;
        //Date = $txtDT.val().trim();
        PU = $ddPU.val();
        Line = $txtLine.val().trim();
        Shift = $ddShift.val();
        Model = $txtModel.val().trim();
        WO = $txtWo.val().trim();

        if (Date == "") {
            ErrorMsg("请选择日期");
            return;
        }
        var url = baseUrl + "/Query/IE_DailyProductQty_Query";
        jsonData = { "Date": Date, "PU": PU, "Line": Line, "Shift": Shift, "Model": Model, "WO": WO };
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_DailyProductQty_Query, AjaxFailResult);
    });


    ///////////////////////////////////////////////////////////
    //var url = baseUrl + "/Query/IE_DailyProductQty_Query";    
    //jsonData = { "Date": dt }
    //ajaxJsonRequestDerived(url, 'post', jsonData, IE_DailyProductQty_Query, AjaxFailResult);

    function IE_DailyProductQty_Query(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            var sum = 0;
            for (var i = 0; i < dataLen; i++) {
                sum = sum + data.tableData[i].QTY;
            }
            OKMsg("一共(" + dataLen + ")行,总计[ " + sum + " ]");
        }
    }
    function OKMsg(msg) {
        $lblMessageError.html("");
        $lblMessageSuccess.html(msg);
    }
    function ErrorMsg(msg) {
        if (msg == "用户登录已失效，请重新登录后操作") {
            //location.reload();
            if ($UserLogin.length != 0) {
                $UserLogOut.html("");
                $UserLogin.html("");
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href=' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350> 请重新登录 </a>')


                tb_init('a.thickbox, area.thickbox, input.thickbox');
                imgLoader = new Image();// preload image
                imgLoader.src = tb_pathToImage;
            }

        }
        $lblMessageError.html(msg);
        $lblMessageSuccess.html("");
    }
    $btnToExcel.click(function () {

        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: '输出',
            tableName: '输出',
            ignoreColumn: [0],
            worksheetName: ['输出']
        })
    });
    //初始化table:
    $tbResultTable.bootstrapTable({
        editable: true, //开启编辑模式

        clickToSelect: true,
        search: true,
        datatype: "json",
        toolbar: '#toolbar',//工具按钮用哪个容器
        striped: true,                      //是否显示行间隔色
        //showFooter: true,
        columns: [
            {
                title: 'Item<br />',
                formatter: function (value, row, index)
                { return index + 1 }
            }, {
                field: 'Mode',
                title: 'Mode'
            }, {
                field: 'TransDate',
                title: 'TransDate'
            }, {
                field: 'Line',
                title: 'Line',
                sortable: true
            }, {
                field: 'Shift',
                title: 'Shift',
                sortable: true,
            }, {
                field: 'Model',
                title: 'Model',
            }, {
                field: 'WO',
                title: 'WO',
            }, {
                field: 'PN',
                title: 'PN',
            }, {
                field: 'QTY',
                title: 'QTY',
            }, {
                field: 'SourcePU',
                title: 'SourcePU',
            },
            {
                field: 'InsertDateTime',
                title: 'InsertDateTime',
            }],
    });
});