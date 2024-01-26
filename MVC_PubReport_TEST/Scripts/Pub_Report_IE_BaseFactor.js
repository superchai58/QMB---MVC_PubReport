

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
    var $ddSection = $('#Section');
    var $txtLine = $('#Line');
    var $ddStage = $('#Stage');
    var $txtBase = $('#Base'); 
    var $txtFactor = $('#Factor');
    var $txtCountTimes = $('#CountTimes');


    $ddMode.change(function () {
        var Mode;

        Mode = $ddMode.val();
        if (Mode == "")
        {
            ErrorMsg("请选择Mode");
            return;
        }
        var modeLength = Mode.length;
        var modeSMT = Mode.substring(modeLength - 3, modeLength);
        var modeFA = Mode.substring(modeLength - 2, modeLength)

        $ddStage.empty();
        var optionStr = "<option value='' >--Please Select--</option>";
        if (modeSMT == "SMT") {
            
            optionStr = optionStr + "<option value='ATEST_1' >ATEST_1</option>";
            optionStr = optionStr + "<option value='ATEST_2' >ATEST_2</option>";
            optionStr = optionStr + "<option value='BTEST_1' >BTEST_1</option>";
            optionStr = optionStr + "<option value='BTEST_2' >BTEST_2</option>";
            optionStr = optionStr + "<option value='CTEST_1' >CTEST_1</option>";
            optionStr = optionStr + "<option value='CTEST_2' >CTEST_2</option>";
            optionStr = optionStr + "<option value='PROTO_1' >PROTO_1</option>";
            optionStr = optionStr + "<option value='PROTO_2' >PROTO_2</option>";
            optionStr = optionStr + "<option value='Input' >Input</option>";
        }
        if (modeFA == "FA") {

            
            optionStr = optionStr + "<option value='ATEST_1' >ATEST_1</option>";
            optionStr = optionStr + "<option value='BTEST_1' >BTEST_1</option>";
            optionStr = optionStr + "<option value='CTEST_1' >CTEST_1</option>";
            optionStr = optionStr + "<option value='PROTO_1' >PROTO_1</option>";
            optionStr = optionStr + "<option value='Input'>Input</option>";
            optionStr = optionStr + "<option value='Output'>Output</option>";
        }  
        var option = $(optionStr);
        $ddStage.append(option);

        Reset();

        var url = baseUrl + "/IE/IE_BaseFactor_GetData";
        jsonData = { "Mode":Mode };
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_BaseFactor_ShowData, AjaxFailResult);
    });

    function Reset()
    {
        $ddSection.val("");
        $txtLine.val("");
        $ddStage.val("");
        $txtBase.val("");
        $txtFactor.val("");
        $txtCountTimes.val("");
    }
    $btnReset.click(function () {
        $ddMode.val("");
        Reset();
    });
    $btnSave.click(function () {
        var Mode;
        var Line;
        var Base;
        var CountTimes;
        var Section;
        var Stage;
        var Factor;
        var Msg;

        Mode = $ddMode.val();
        Section = $ddSection.val();
        Line = $txtLine.val().trim();
        Stage = $ddStage.val();
        Base = $txtBase.val().trim();
        Factor = $txtFactor.val().trim();
        CountTimes = $txtCountTimes.val().trim();

        Msg = "";
        if (Mode == "")
        {
            Msg = Msg+ "[Mode] 不能为空";
        }

        if (Section == "")
        {
            Msg = Msg + "[Section] 不能为空";
        }
        if (Line == "")
        {
            Msg = Msg + "[Line] 不能为空";
        }
        if (Stage == "")
        {
            Msg = Msg + "[Stage] 不能为空";
        }
        if (checkNumber(Base) == false)
        {
            Msg = Msg + "[Base] 必须是数字";
        }
        if (checkNumber(Factor) == false) {
            Msg = Msg + "[Factor] 必须是数字";
        }
        if (checkNumberInt(CountTimes) == false) {
            Msg = Msg + "[CountTimes] 必须是整数";
        }
        if (Msg != "")
        {
            ErrorMsg(Msg);
            return;
        }

        DisableBtns();
        var url = baseUrl + "/IE/IE_BaseFactor_SavaData";
        jsonData = { "Mode": Mode,"Section":Section,"Line":Line,"Stage":Stage,"Base":Base,"Factor":Factor,"CountTimes":CountTimes };
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_BaseFactor_ShowData, AjaxFailResult);


    });


    function IE_BaseFactor_ShowData(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            OKMsg("一共(" + data.tableData.length + ")行");
            Reset();
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
            var url = baseUrl + "/IE/IE_BaseFactor_Delete";
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
            fileName: 'IE_BaseFactor',
            tableName: 'IE_BaseFactor',
            ignoreColumn: [0],
            worksheetName: ['IE_BaseFactor']
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
                field: 'Mode', 
                title: 'Mode'
            }, {
                field: 'Section',
                title: 'Section'
            }, {
                field: 'Line',
                title: 'Line',
                sortable: true,
            }, {
                field: 'Stage',
                title: 'Stage',
            }, {
                field: 'Base',
                title: 'Base',
            }, {
                field: 'Factor',
                title: 'Factor',
            }, {
                field: 'CountTimes',
                title: 'Times',
            }, {
                field: 'UserName',
                title: '操作人',
            }, {
                field: 'TransDateTime',
                title: '日期',
            }],

        onClickRow: function (row, $element) {
            $ddMode.val(row.Mode);
            $ddSection.val(row.Section); 
            $txtLine.val(row.Line);
            $ddStage.val(row.Stage);
            $txtBase.val(row.Base);
            $txtFactor.val(row.Factor);
            $txtCountTimes.val(row.CountTimes); 

        },
    });
    
})