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
    var $ddMode = $('#Mode');
    var $txtLine = $('#Line');
    var $ddShift = $('#Shift');
    
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
            var url = baseUrl + "/Query/Efficiency_GetMode";
            jsonData = { "Date": Date }
            ajaxJsonRequestDerived(url, 'post', jsonData, Efficiency_GetMode, AjaxFailResult);
        }
    });


    function Efficiency_GetMode(data) {
        $ddMode.empty();
        var optionStr = "<option value='' >--Please Select--</option>";
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            
            var ModelList = data.tableData;
         
            for (var i = 0; i < ModelList.length; i++) {
                optionStr = optionStr + "<option value='" + ModelList[i] + "' >" + ModelList[i] + "</option>";
            } 
        }
        var option = $(optionStr);
        $ddMode.append(option);
    }

    $btnReset.click(function () {

        $ddMode.val("");
        $txtLine.val("");
        $ddShift.val(""); 
    });
    $btnQuery.click(function () {
        var Date = "";
        var Mode = "";
        var Line = "";
        var Shift = "";
        var Model = "";
        var WO = "";
        Date = $txtDT.val().trim();
        Mode = $ddMode.val();
        Line = $txtLine.val().trim();
        Shift = $ddShift.val();
        

        if (Date == "") {
            ErrorMsg("请选择日期");
            return;
        }
        var url = baseUrl + "/Query/Efficiency_Query";
        jsonData = { "Date": Date, "Mode": Mode, "Line": Line, "Shift": Shift };
        ajaxJsonRequestDerived(url, 'post', jsonData, Show_Data, AjaxFailResult);
    });

 

    function Show_Data(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            //var sum = 0;
            //for (var i = 0; i < dataLen; i++) {
            //    sum = sum + data.tableData[i].QTY;
            //}
            OKMsg("一共(" + dataLen + ")行");
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
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href="' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350  > 请重新登录 </a>')


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
            fileName: 'Eff',
            tableName: 'Eff',
            ignoreColumn: [0],
            worksheetName: ['Eff']
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
        showFooter: true,     
        columns: [
            {
                title: 'Item<br />',
                formatter: function (value, row, index)
                { return index + 1 }
            }, {
                field: 'Mode',
                title: 'Mode',
                sortable: true,
            }, {
                field: 'WorkDate',
                title: 'WorkDate'
            }, {
                field: 'Line',
                title: 'Line',
                sortable: true
            }, {
                field: 'Shift',
                title: 'Shift',
                

            }, {
                field: 'WKTM',
                title: '实勤工时',
                footerFormatter: function (value) {
                    var count = 0;
                    for (var i in value)
                    {
                        count += value[i].WKTM;
                    }
                    return count;
                }
            }, {
                field: 'ExCTM',
                title: '异常工时',
                footerFormatter: function (value) {
                    var count = 0;
                    for (var i in value) {
                        count += value[i].ExCTM;
                    }
                    return count;
                }
            }, {
                field: 'DOTM',
                title: '实作工时',
                footerFormatter: function (value) {
                    var count = 0;
                    for (var i in value) {
                        count += value[i].DOTM;
                    }
                    return count;
                }
            }, {
                field: 'OutPut',
                title: '产出数量',
                footerFormatter: function (value) {
                    var count = 0;
                    for (var i in value) {
                        count += value[i].OutPut;
                    }
                    return count;
                }
            }, {
                field: 'OutPutHour',
                title: '产出工时',
                footerFormatter: function (value) {
                    var count = 0;
                    for (var i in value) {
                        count += value[i].OutPutHour;
                    }
                    return count;
                }
            }, {
                field: 'InsertDateTime',
                title: 'InsertDateTime',
            }],
    });
});