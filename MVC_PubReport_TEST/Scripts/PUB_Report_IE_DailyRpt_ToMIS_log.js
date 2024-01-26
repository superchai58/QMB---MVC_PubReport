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


    var $txtBeginDT = $("#BeginDT");
    var $txtEndDT = $("#EndDT");

    var $BeginDTCalendar = $('#ddBeginDT');
    var $EndDTCalendar = $('#ddEndDT');

    //var dt = showdate(-1);
    //$txtBeginDT.val(dt);

    //绑定日历
 
    $BeginDTCalendar.calendar({
        trigger: '#BeginDT',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected')
        },
        onClose: function (view, date, data) {
            var Date;
            Date = $txtBeginDT.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            } 
        }
    });

 
    $EndDTCalendar.calendar({
        trigger: '#EndDT',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected')
        },
        onClose: function (view, date, data) {
            var Date;
            Date = $txtEndDT.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            } 
        }
    });

 

 
    $btnQuery.click(function () {
        var BeginDT = "";
        var EndDT = "";
        BeginDT = $txtBeginDT.val().trim();
        EndDT = $txtEndDT.val().trim(); 
      
        if (BeginDT == "") {
            ErrorMsg("请选择BeginDT");
            return;
        }
        if (EndDT == "") {
            ErrorMsg("请选择EndDT");
            return;
        } 
        var url = baseUrl + "/Query/IE_DailyRpt_ToMIS_log_QueryByDT";
        jsonData = { "BeginDT": BeginDT, "EndDT": EndDT};
        ajaxJsonRequestDerived(url, 'post', jsonData, funIE_DailyRpt_ToMIS_log_QueryByDT, AjaxFailResult);
    });


    ///////////////////////////////////////////////////////////
    //var url = baseUrl + "/Query/IE_DailyProductQty_Query";    
    //jsonData = { "Date": dt }
    //ajaxJsonRequestDerived(url, 'post', jsonData, IE_DailyProductQty_Query, AjaxFailResult);

    function funIE_DailyRpt_ToMIS_log_QueryByDT(data) {
        OKMsg("查询成功，文档已发送到您邮箱！");

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
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href= ' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350> 请重新登录 </a>')


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
            fileName: '产出工时',
            tableName: '产出工时',
            ignoreColumn: [0],
            worksheetName: ['产出工时']
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
                field: 'TransDate',
                title: 'TransDate'
            }, {
                field: 'Depart_No',
                title: 'Depart_No'
            }, {
                field: 'Mode',
                title: 'Mode',
                sortable: true
            }, {
                field: 'Line',
                title: 'Line',
                sortable: true,

            }, {
                field: 'Line_NO',
                title: 'Line_NO',
            }, {
                field: 'Shift',
                title: 'Shift',
            }, {
                field: 'Model',
                title: 'Model',
            }, {
                field: 'Stage',
                title: 'Stage',
 
            }, {
                field: 'OutPut',
                title: 'OutPut',
            } 
            , {
                field: 'Manhour',
                title: 'Manhour',
            }, {
                field: 'Out',
                title: 'Out',
            } , {
                field: 'BUTYPE',
                title: 'BUTYPE',
            }, {
                field: 'PN',
                title: 'PN',
            }, {
                field: 'WO',
                title: 'WO',
            }
        ],
    });
});