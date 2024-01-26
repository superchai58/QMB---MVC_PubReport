

$(document).ready(function () {
    var $tbResultTable = $('#ResultTable');
    var $btnToExcel = $('#ToExcel');
    var $btnSave = $('#save');
    var $btnDelete = $('#delete');
    var $btnUploadExcel = $('#UploadExcel');
    var $btnReset = $('#reset');
    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
    var $prog_in = $('#prog_in');
    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');
    var $ModalDoing = $('#doing');
    var $txtFilePath = $('#UploadExcelFilePath');

    var $ddPU = $('#PU')
    var $ddType = $('#Type');
    var $txtLine = $('#Line');
    var $ddShiftNo = $('#ShiftNo');

    $ddPU.change(function () {
        $ddType.val("");
        //$txtLine.val("");
        //$ddShiftNo.val("");
    });
    
    $btnReset.click(function () {
        $ddPU.val("");
        $ddType.val("");
        $txtLine.val("");
        $ddShiftNo.val("");
    });
    $ddType.change(function () {
        var PU;
        var Type;
        var Msg;

        PU = $ddPU.val();
        Type = $ddType.val();
        Msg = "";
        if (PU == "") {
            Msg = Msg + "[PU] 不能为空 || ";
        }
        if (Type == "") {
            Msg = Msg + "[Type] 不能为空 || ";
        }
        if (Msg != "") {
            ErrorMsg(Msg);
            return;
        }

        var url = baseUrl + "/IE/IE_Line_Shift_View";

        jsonData = { "PU": PU, "Type": Type }
        ajaxJsonRequestDerived(url, 'post', jsonData, showData, AjaxFailResult);

    });
     
    $btnSave.click(function () {
        var PU;
        var Type;
        var Line;
        var ShiftNo; 
        var Msg;

        PU = $ddPU.val();
        Type = $ddType.val();
        Line = $txtLine.val().trim();
        ShiftNo = $ddShiftNo.val();
            

        Msg = "";
        if (PU == "") {
            Msg = Msg + "[PU] 不能为空 || ";
        }
        if (Type == "") {
            Msg = Msg + "[Type] 不能为空 || ";
        }
        if (Line == "") {
            Msg = Msg + "[Line] 不能为空 || ";
        }
        if (ShiftNo == "") {
            Msg = Msg + "[ShiftNo] 不能为空 || ";
        }
    
        if (Msg != "") {
            ErrorMsg(Msg);
            return;
        }

        DisableBtns();
        var url = baseUrl + "/IE/IE_Line_Shift_SavaData";

        jsonData = { "PU": PU, "Type": Type, "Line": Line, "ShiftNo": ShiftNo }
        ajaxJsonRequestDerived(url, 'post', jsonData, showData, AjaxFailResult);

    });

 
    $btnDelete.click(function () {
        var Type;
        var PU;
        var msg;

        PU = $ddPU.val();
        Type = $ddType.val();

        msg = "";
        if (PU == "") {
            msg = Msg + "[PU] 不能为空 || ";
        }
        if (Type == "") {
            msg = Msg + "[Type] 不能为空 || ";
        }

        if (msg != "")
        {
            ErrorMsg(msg);
            return;
        }

        var selectRows = $tbResultTable.bootstrapTable('getSelections');

        //console.log(selectRows);
        if (selectRows.length == 0) {

            //$lblMessageError.html("请选择要删除的记录！");
            //$lblMessageSuccess.html("");
            ErrorMsg("请选择要删除的记录！");
            return;
        }
        msg = "您确定要删除吗？"
        if (confirm(msg) == true) {
            $ModalDoing.modal('show').css({
                width: 'auto',
                'margin-left': function () {
                    return ($(this).width() / 10);
                },
                'margin-top': function () {
                    return ($(this).height() / 2);
                },

            });
            var url = baseUrl + "/IE/IE_Line_Shift_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows),"PU":PU,"Type":Type };
            ajaxJsonRequestDerived(url, 'post', jsonData, DeleteData, AjaxFailResult);
        }

    });
    function showData(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            OKMsg("一共(" + data.tableData.length + ")行");
        }
        EnableBtns();
    }

    function DeleteData(data)
    {
        if (data.result != "OK") {

            ErrorMsg(data.result);

            $ModalDoing.modal('hide');


        } else {
            //移除选中的行
            var ids = $.map($tbResultTable.bootstrapTable('getSelections'), function (row) {
                return row.Line;
            });
            $tbResultTable.bootstrapTable('remove', {
                field: 'Line',
                values: ids
            });
            //$lblMessageError.html("");
            //$lblMessageSuccess.html("(" + ids.length + ")行资料已删除");
            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');
        }

    }

    function DisableBtns() {
        $btnSave.attr("disabled", "disabled");
        $btnDelete.attr("disabled", "disabled");
        $btnToExcel.attr("disabled", "disabled");
        $btnUploadExcel.attr("disabled", "disabled");
    }
    function EnableBtns() {
        $btnSave.val("添加/更新");
        $btnSave.attr("disabled", false);


        $btnDelete.val("删除");
        $btnDelete.attr("disabled", false);


        $btnToExcel.val("导出数据Excel");
        $btnToExcel.attr("disabled", false);

        $btnUploadExcel.val("Excel资料上传");
        $btnUploadExcel.attr("disabled", false); 

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
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href=' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350     > 请重新登录 </a>')


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
            fileName: 'IE_Line_Shift',
            tableName: 'IE_Line_Shift',
            ignoreColumn: [0],
            worksheetName: ['IE_Line_Shift']
        })
    });

     
    //初始化table end
    $tbResultTable.bootstrapTable({
        editable: true, //开启编辑模式 
        clickToSelect: true,
        search: true,
        datatype: "json",
        toolbar: '#toolbar',//工具按钮用哪个容器
        striped: true,                      //是否显示行间隔色
        columns: [
             {
                 field: 'Checked',
                 title: '删除<br />',
                 checkbox: true,
                 formatter: function (value, row, index) {
                     if (row.state == true)
                         return {
                             disable: true,
                             checked: true

                         };
                     return value;
                 }
             },

            {
                //field: 'Item',
                title: 'Item<br />',

                formatter: function (value, row, index)
                { return index + 1 }
            }, {
                field: 'ID',
                align: 'center',
                title: 'ID',
                visible: false,
            }, {
                field: 'PU',
 
                title: 'PU'
            }, {
                field: 'Line',
                title: 'Line',
    
            }, {
                field: 'ShiftNo',
                title: 'ShiftNo',
   

            }, {
                field: 'Trans_DateTime',
                title: 'Trans_DateTime',
            }, {
                field: 'UserID',
                title: '操作人',
            } ],

        onClickRow: function (row, $element) {
 

            $ddPU.val(row.PU);
            
            $txtLine.val(row.Line);
            $ddShiftNo.val(row.ShiftNo)




        },
    });
})