

$(document).ready(function () {
    var $tbResultTable = $('#ResultTable');
    var $btnToExcel = $('#ToExcel');
    var $btnSave = $('#save');
    var $btnDelete = $('#delete');
    var $btnReset = $('#reset');
    var $btnUploadExcel = $('#UploadExcel');
    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
    var $prog_in = $('#prog_in');
    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');
    var $ModalDoing = $('#doing');
    var $txtFilePath = $('#UploadExcelFilePath');

    var $ddMode = $('#Mode');
    var $ddDepartNo = $('#DepartNo');
    var $ddQShift = $('#FQAShift');
    var $ddShift = $('#Shift');
    var $txtQLineNo = $('#FQA_LineNo');
    var $txtLineNo = $('#LineNo');
    var $txtQLine = $('#FQALine');
    var $txtLine = $('#Line');


    $ddDepartNo.change(function () {
        var DepartNo;

        DepartNo = $ddDepartNo.val();
        if (DepartNo == "") {
            ErrorMsg("请选择DepartNo");
            return;
        }
        var url = baseUrl + "/IE/IE_FQALineMapping_GetMode";
        jsonData = { "DepartNo": DepartNo };
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_FQALineMapping_GetMode, AjaxFailResult);

        Reset();
        
    });

    function IE_FQALineMapping_GetMode(data)
    {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $ddMode.empty();
            var optionStr = "<option value='' >--Please Select--</option>";
            var tableData = data.tableData;

            for (var i = 0; i < tableData.length; i++)
            {
                optionStr = optionStr + "<option value='" + tableData[i] + "' >" + tableData[i] + "</option>";
            }
            var option = $(optionStr);
            $ddMode.append(option);

            var DepartNo;
            var Mode;
            var QLineNo;
            var LineNo;

            DepartNo = $ddDepartNo.val();
            Mode = $ddMode.val();
            QLineNo = $txtQLineNo.val().trim();
            LineNo = $txtLineNo.val().trim();

            var url = baseUrl + "/IE/IE_FQALineMapping_GetData";
            //jsonData = { "DepartNo": DepartNo, "Mode": Mode, "QLineNo": QLineNo, "LineNo": LineNo };
            jsonData = { "DepartNo": DepartNo };
            ajaxJsonRequestDerived(url, 'post', jsonData, IE_FQALineMapping_ShowData, AjaxFailResult);
        }
       
    }

    $ddMode.change(function () {
        var DepartNo;
        var Mode;
        var QLineNo;
        var LineNo;

        DepartNo = $ddDepartNo.val();
        Mode = $ddMode.val();
        QLineNo = $txtQLineNo.val().trim();
        LineNo = $txtLineNo.val().trim();

        if (DepartNo == "") {
            ErrorMsg("请选择[DepartNo]");
            return;
        }
        if (Mode == "")
        {
            ErrorMsg("请选择[Mode]");
            return;
        }
        var url = baseUrl + "/IE/IE_FQALineMapping_GetData";
        jsonData = { "DepartNo": DepartNo, "Mode": Mode};
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_FQALineMapping_ShowData, AjaxFailResult);
    });
    function Reset() {
        
        $ddMode.val("");
        $ddQShift.val("");
        $ddShift.val("");

        $txtQLineNo.val("");
        $txtLineNo.val("");
        $txtQLine.val("");
        $txtLine.val("");
         
    }
    $btnReset.click(function () {
        $ddDepartNo.val("");
        Reset();
    });
    
    $btnSave.click(function () {
        var DepartNo;
        var Mode;
        var QShift;
        var Shift;
        var QLineNo;
        var LineNo;
        var QLine;
        var Line;
        var Msg;

        DepartNo = $ddDepartNo.val();
        Mode = $ddMode.val();
        QShift = $ddQShift.val();
        Shift = $ddShift.val();
        
        QLineNo = $txtQLineNo.val().trim();
        LineNo = $txtLineNo.val().trim();
        QLine = $txtQLine.val().trim();
        Line = $txtLine.val().trim();
        Msg = "";

        if (DepartNo == "")
        {
            Msg = Msg+ "请选择[DepartNo] ||";
        }
        if (Mode == "") {
            Msg = Msg + "请选择[Mode] ||";
        }
        if (QShift == "") {
            Msg = Msg + "请选择[QShift] ||";
        }
        if (Shift == "") {
            Msg = Msg + "请选择[Shift] ||";
        }
        if (QLineNo == "") {
            Msg = Msg + "[QLineNo]不能为空 ||";
        }
        if (LineNo == "") {
            Msg = Msg + "[LineNo]不能为空 ||";
        }
        if (QLine == "") {
            Msg = Msg + "[QLine]不能为空 ||";
        }
        if (Line == "") {
            Msg = Msg + "[Line]不能为空 ||";
        }

        if (Msg != "")
        {
            ErrorMsg(Msg);
            return
        }
        var url = baseUrl + "/IE/IE_FQALineMapping_SaveData";
        jsonData = { "DepartNo": DepartNo, "Mode": Mode, "QLineNo": QLineNo, "LineNo": LineNo, "QShift": QShift, "Shift": Shift, "QLine": QLine, "Line": Line };
 
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_FQALineMapping_ShowData, AjaxFailResult);

    });

    function IE_FQALineMapping_ShowData(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            OKMsg("一共(" + data.tableData.length + ")行");
             
        }
        EnableBtns();
    }
     


    function DisableBtns() {
        $btnSave.attr("disabled", "disabled");
        $btnDelete.attr("disabled", "disabled");
        $btnToExcel.attr("disabled", "disabled");
        $btnUploadExcel.attr("disabled", "disabled");
        $btnReset.attr("disabled", "disabled");
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

        $btnReset.attr("disabled", false);



    }
    function OKMsg(msg) {
        $lblMessageError.html("");
        $lblMessageSuccess.html(msg);
    }
    function ErrorMsg(msg) {
        if (msg == "用户登录已失效，请重新登录后操作") {

            if ($UserLogin.length != 0) {
                $UserLogOut.html("");
                $UserLogin.html("");
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href=' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350 > 请重新登录 </a>')


                tb_init('a.thickbox, area.thickbox, input.thickbox');
                imgLoader = new Image();// preload image
                imgLoader.src = tb_pathToImage;
            }

        }
        $lblMessageError.html(msg);
        $lblMessageSuccess.html("");
    }


    $btnDelete.click(function () {
        var selectRows = $tbResultTable.bootstrapTable('getSelections');
        if (selectRows.length == 0) {
            ErrorMsg("请选择要删除的记录！");
            return;
        }
        var msg = "您确定要删除吗？"
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
            var url = baseUrl + "/IE/IE_FQALineMapping_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, Data_Delete, AjaxFailResult);
        }



    });
    function Data_Delete(data) {
        if (data.result != "OK") {

            ErrorMsg(data.result);

            $ModalDoing.modal('hide');


        } else {
            //移除选中的行
            var ids = $.map($tbResultTable.bootstrapTable('getSelections'), function (row) {
                return row.ID;
            });
            $tbResultTable.bootstrapTable('remove', {
                field: 'ID',
                values: ids
            });
            //$lblMessageError.html("");
            //$lblMessageSuccess.html("(" + ids.length + ")行资料已删除");
            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');
            Reset();
        }
    }

    $btnToExcel.click(function () {

        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: 'IE_FQALineMapping',
            tableName: 'IE_FQALineMapping',
            ignoreColumn: [0],
            worksheetName: ['IE_FQALineMapping']
        })
    });


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
                title: 'ID',
                visible: false,
            }, {
                field: 'Depart_No',
                title: 'DepartNo'
            }, {
                field: 'Mode',
                title: 'Mode'
            }, {
                field: 'FQA_Shift',
                title: 'QShift',
               
            }, {
                field: 'Shift',
                title: 'Shift',
            }, {
                field: 'FQA_Line_No',
                title: 'QLineNo',
            }, {
                field: 'Line_No',
                title: 'LineNo',
            }, {
                field: 'FQA_Line',
                title: 'QLine',
            }, {
                field: 'Line',
                title: 'Line',
            }, {
                field: 'UserID',
                title: '操作人',
            }, {
                field: 'TransDateTime',
                title: '日期',
            }],

        onClickRow: function (row, $element) {
            $ddMode.val(row.Mode);
            $ddDepartNo.val(row.Depart_No);
            $ddQShift.val(row.FQA_Shift);
            $ddShift.val(row.Shift);
            $txtQLineNo.val(row.FQA_Line_No);
            $txtLineNo.val(row.Line_No);
            $txtQLine.val(row.FQA_Line);
            $txtLine.val(row.Line);

        },
    });

})