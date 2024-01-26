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


    var $txtSDT = $("#sdt");
    var $txtEDT = $("#edt");
    var $CalendarS = $('#sdt');
    var $CalendarE = $('#edt');

    var $ddPU = $('#PU');
    var $ddType = $('#Type');
    var $ddCustomer = $('#Customer')
    var $ddLine = $('#Line')
    var $ddModel = $('#Model')

 
    $btnQuery.click(function () {
        var PU, Type, Customer, Line, Model, FromDate, EndDate;
       
        PU = $ddPU.val();
        Type = $ddType.val();
        //Customer = $ddCustomer.val();
        Line = $ddLine.val();
        Model = $ddModel.val();
        FromDate = ""+ $txtSDT.val();
        EndDate = "" + $txtEDT.val();

        if (PU == "") {
            ErrorMsg("PU 不能为空");
            return;
        }

        if (Type == "") {
            ErrorMsg("Type 不能为空");
            return;
        }
         
   
        if (FromDate == "") {
            ErrorMsg("FromDate 不能为空");
            return;
        }
        if (EndDate == "") {
            ErrorMsg("EndDate 不能为空");
            return;
        }

        var url = baseUrl + "/Query/AssyReport_QueryData";
        var jsonData = { "PU": PU, "Type": Type, "SDate": FromDate, "EDate": EndDate }
        ajaxJsonRequestDerived(url, 'post', jsonData, AssyReport_QueryData, AjaxFailResult);

    })
    

    function AssyReport_QueryData(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
          
            $tbResultTable.bootstrapTable('removeAll');
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
            fileName: 'AssyReport',
            tableName: 'AssyReport',
            ignoreColumn: [0],
            worksheetName: ['AssyReport']
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
                field: 'Line',
                title: 'Line',

            }, {
                field: 'Input',
                title: 'Input',
                
            }, {
                field: 'Output',
                title: 'Output',
               

            } 
             ],
    });
});