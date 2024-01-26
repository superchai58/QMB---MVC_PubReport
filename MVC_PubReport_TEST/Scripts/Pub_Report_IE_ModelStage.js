


$(document).ready(function () {
    //页面上的控件变量，方便修改，且只读一次。
    var $ddMode = $('#IE_ModelStageForm #Mode');
    var $ddStage = $("#IE_ModelStageForm #Stage");
    var $ddSection = $('#IE_ModelStageForm #Section')
    var $txtModel = $('#IE_ModelStageForm #Model');
    var $txtFilePath = $('#IE_ModelStageForm #UploadExcelFilePath');

    var $btnDelete = $('#IE_ModelStageForm #delete');
    var $btnSave = $('#IE_ModelStageForm #save');
    var $btnToExcel = $('#IE_ModelStageForm #ToExcel');
    var $btnUploadExcel = $('#IE_ModelStageForm #UploadExcel');

    var $lblMessageError = $('#IE_ModelStageForm #message-error');
    var $lblMessageSuccess = $('#IE_ModelStageForm #message-success');

    var $tbResultTable = $('#IE_ModelStageForm #ResultTable');
    
    var $ModalDoing = $('#doing');
    var $UserLogOut = $('#UserLogOut');
    var $UserLogin = $('#UserLogin');

    var $prog_in = $('#prog_in');

  

    //页面上的变量 end 
    //IE_ModelStage 
    $ddMode.change(function () {
        var mode = $ddMode.val();
        var modeLength = mode.length;
        var modeSMT = mode.substring(modeLength - 3, modeLength);
        var modeFA = mode.substring(modeLength - 2, modeLength)

        $ddStage.empty();
        if (modeSMT == "SMT") {
            var optionStr = "<option value='' >Please Select</option>";
            optionStr = optionStr + "<option value='ATEST_1' >ATEST_1</option>";
            optionStr = optionStr + "<option value='ATEST_2' >ATEST_2</option>";
            optionStr = optionStr + "<option value='BTEST_1' >BTEST_1</option>";
            optionStr = optionStr + "<option value='BTEST_2' >BTEST_2</option>";
            optionStr = optionStr + "<option value='CTEST_1' >CTEST_1</option>";
            optionStr = optionStr + "<option value='CTEST_2' >CTEST_2</option>";
            optionStr = optionStr + "<option value='PROTO_1' >PROTO_1</option>";
            optionStr = optionStr + "<option value='PROTO_2' >PROTO_2</option>";
            optionStr = optionStr + "<option value='Input' >Input</option>";

            var option = $(optionStr);
            $ddStage.append(option);
        }
        if (modeFA == "FA") {

            var optionStr = "<option value='' >Please Select</option>";
            optionStr = optionStr + "<option value='ATEST_1' >ATEST_1</option>";
            optionStr = optionStr + "<option value='BTEST_1' >BTEST_1</option>";
            optionStr = optionStr + "<option value='CTEST_1' >CTEST_1</option>";
            optionStr = optionStr + "<option value='PROTO_1' >PROTO_1</option>";
            optionStr = optionStr + "<option value='Input'>Input</option>";
            optionStr = optionStr + "<option value='Output'>Output</option>";

            var option = $(optionStr);
            $ddStage.append(option);
        }

        //从后台根据Mode 得到结果集
        ResetMessage();
        opType = 3;
        var path = baseUrl + "/IE/IE_ModelStage_ByOpType";

        $tbResultTable.bootstrapTable('removeAll');

        IE_ModelStage_ByOpType(opType, path);
        $tbResultTable.bootstrapTable('refresh');


    });

    $ddSection.change(function () {
        opType = 3;
        var path = baseUrl + "/IE/IE_ModelStage_ByOpType";
        $tbResultTable.bootstrapTable('removeAll');
        IE_ModelStage_ByOpType(opType, path);
    });

    $ddStage.change(function () {
        opType = 3;
        var path = baseUrl + "/IE/IE_ModelStage_ByOpType";
        $tbResultTable.bootstrapTable('removeAll');

        IE_ModelStage_ByOpType(opType, path);
  

    });

    $txtModel.keypress(function (event) {
        if (event.which == 13) {
            var Mode = $ddMode.val();
            if (Mode == "" || Mode == undefined) {
                //$lblMessageError.html("Mode 不能为空，请选择!");
                //$lblMessageSuccess.val("");
                ErrorMsg("Mode 不能为空，请选择!");
                return false;
            }
            opType = 3;
            var path = baseUrl + "/IE/IE_ModelStage_ByOpType";
            IE_ModelStage_ByOpType(opType, path);
            //$lblMessageError.html("");
            //$lblMessageSuccess.val("查询中...");
            OKMsg("查询中...");
            DisableBtns();
        }

    });

    $btnDelete.click(function () {
        var selectRows = $tbResultTable.bootstrapTable('getSelections');

        //console.log(selectRows);
        if (selectRows.length == 0) {

            //$lblMessageError.html("请选择要删除的记录！");
            //$lblMessageSuccess.html("");
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
            var url = baseUrl + "/IE/IE_ModelStage_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, IE_ModelStage_DeleteData, AjaxFailResult);
        }



    });
    function IE_ModelStage_DeleteData(data) {
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
        }
    }

    function OKMsg(msg)
    {
        $lblMessageError.html("");
        $lblMessageSuccess.html(msg);
    }
    function ErrorMsg(msg) {
        if (msg == "用户登录已失效，请重新登录后操作") {
            //location.reload();
            if ($UserLogin.length != 0)
        {
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

    $btnSave.click(function () {

        ResetMessage();
        var dataCheck = IE_ModelStage_Check();
        if (dataCheck == true) {
            opType = 2;
            var path = baseUrl + "/IE/IE_ModelStage_ByOpType";
            IE_ModelStage_ByOpType(opType, path);
            $btnSave.val("保存...");
            DisableBtns();

        }
    });


    $btnToExcel.click(function () {

        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: 'IE_ModelStage',
            tableName: 'IE_ModelStage',
            ignoreColumn: [0],
            worksheetName: ['IE_ModelStage']
        })
    });
    var sitv,uploadOK
    $btnUploadExcel.click(function () {
        var name = $txtFilePath.val();
        var formaData = new FormData();

        uploadOK = 0;
        formaData.append("file", $txtFilePath[0].files[0])
        formaData.append("name", name); 
        if (name == "" || name == undefined)
        {
            ErrorMsg("请选择需要上传的Excel文件");
            return;
        }
        $btnUploadExcel.val("资料正在上传...");
        DisableBtns();
        sitv = setInterval(function () {
            var prog_url = baseUrl + "/IE/ProcessGetStatus";
            $.getJSON(prog_url, function (res) {
                //$('#prog_in').width(res + '%');
               
                //alert(res.tableData);
                if (uploadOK == 1) {
                    $prog_in.css("width", '100%')
                }
                else {
                    $prog_in.css("width", res.tableData + '%')
                }
            }
                , 6000);
        });

        var url = baseUrl + "/IE/IE_ModelStage_UploadExcel";
        ajaxJsonRequestFile(url, 'post', formaData, UploadSuceess, AjaxFailResult);


    });
    function UploadSuceess(data)
    {
        if (data.result != "OK") {
            //if (msg == "用户登录已失效，请重新登录后操作") {
            //    //location.reload();
            //    $('#UserLogOut').html("");
            //    $UserLogin.html('<a class=" thickbox" href="/User/Login?keepThis=True&amp;TB_iframe=True&amp;height=430&amp;width=350&modal=true"    title="登录PUB Report"  > 请登录 </a>')
            //    tb_init('a.thickbox, area.thickbox, input.thickbox');
            //}
            ErrorMsg(data.result);
            clearInterval(sitv);
            uploadOK = 0;
            $prog_in.css("width", '0%')
            EnableBtns(); 
            return;

        } else {
            clearInterval(sitv);
            uploadOK = 1;
            $prog_in.css("width", '100%')
            EnableBtns();
            return;
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
        $txtModel.val("");
        $txtModel.focus();


    }
    function IE_ModelStage_ByOpType(opType, url) {
        var Mode = "", Model = "", Section = "", Stage = "";
        Mode = $ddMode.val();
        Model = $txtModel.val().trim();
        Section = $ddSection.val();
        Stage = $ddStage.val();
        jsonData = { "Mode": Mode, "Model": Model, "Section": Section, "Stage": Stage, "opType": opType };
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_ModelStage_ShowData, AjaxFailResult);
    }

    function IE_ModelStage_ShowData(data) {
        if (data.result != "OK") {
            
            //if (data.result == "用户登录已失效，请重新登录后操作") {
            //    $('#UserLogOut').html("");
            //    //location.reload();
            //    $('#UserLogin').html('<a class="thickbox" href="/User/Login?keepThis=True&amp;TB_iframe=True&amp;height=430&amp;width=350&modal=true"    title="登录PUB Report"  > 请登录 </a>')
            //    tb_init('a.thickbox, area.thickbox, input.thickbox');
            //}
            ErrorMsg(data.result);

        }
        else {
            
            $tbResultTable.bootstrapTable('load', data.tableData);
            EnableBtns();  
            if (opType == 3) {
                OKMsg("一共(" + data.tableData.length + ")行资料");
                return;
            } else {
                OKMsg("数据操作成功");
                return;
            }
        }

    }



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
                 title: '删除',
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
                field: 'Item',
                title: 'Item',
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
                field: 'Stage',
                title: 'Stage',
                sortable: true
            }, {
                field: 'Model',
                title: 'Model',
                sortable: true,
 
            }
        , {
            field: 'UserName',
            title: '操作人'
        }
        , {
            field: 'TransDateTime',
            title: '日期',
            sortable: true
        }
 
        ],
 
        onClickRow: function (row, $element) {
            
            $ddMode.val(row.Mode);
            $txtModel.val(row.Model);
            $ddSection.val(row.Section);
            $ddStage.val(row.Stage);

        },
    });

 


    function IE_ModelStage_Check() {
        var Mode = "", Model = "", Section = "", Stage = "", UserName = "";

        Mode = $ddMode.val();
        Model = $txtModel.val();
        Section = $ddSection.val();
        Stage = $ddStage.val();

        if (Mode == "" || Mode == undefined) {
            //$lblMessageError.html("Mode 不能为空，请选择!");
            ErrorMsg("Mode 不能为空，请选择!");
            return false;
        }

        if (Section == "" || Section == undefined) {
            //$lblMessageError.html("Section 不能为空，请选择!");
            ErrorMsg("Section 不能为空，请选择!");
            return false;
        }
        if (Stage == "" || Stage == undefined) {
            //$lblMessageError.html("Stage 不能为空，请选择!");
            ErrorMsg("Stage 不能为空，请选择!");
            return false;
        }
        if (Model == "" || Model == undefined) {
            //$lblMessageError.html("Model 不能为空，请填写!");
            ErrorMsg("Model 不能为空，请选择!");
            return false;
        }

        return true;
    }
});