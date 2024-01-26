

$(document).ready(function () {
    var $ddFactory = $('#Factory');
    var $ddPU = $('#PU');
    var $ddMode = $('#Mode');
    var $ddLine = $('#Line');
    var $txtModel = $('#Model');
    var $txtCycleTime = $('#CycleTime');
    var $txtManHour = $('#ManHour');
    var $txtOnlineMan = $('#OnlineMan');
    var $txtOfflineMan = $('#OfflineMan');
    var $txtShareRate = $('#ShareRate');
    var $txtRemarks = $('#Remarks');
    var $txtFilePath = $('#UploadExcelFilePath');

    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
    var $prog_in = $('#prog_in');

    var $tbResultTable = $('#ResultTable');
    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');
    var $ModalDoing = $('#doing');

    var $btnToExcel = $('#ToExcel');
    var $btnSave = $('#save');
    var $btnDelete = $('#delete');
    var $btnUploadExcel = $('#UploadExcel');
 
    var tableDataRows = 0;

    $ddFactory.change(function () {
    
        var Factory = $ddFactory.val();
        var url = baseUrl + "/IE/IE_STD_ManHour_GetPU";
        if (Factory != "")
        {
            jsonData = { "Factory": Factory }

            ajaxJsonRequestDerived(url, 'post', jsonData, IE_STD_ManHour_GetPU, AjaxFailResult);
        }
       
    });

    function IE_STD_ManHour_GetPU(data)
    {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            return;
        } else {
            var PUList = data.tableData;
            var optionStr = "<option value='' >--Please Select--</option>";

            for(var i=0;i< PUList.length;i++)
            {
                optionStr = optionStr + "<option value='" + PUList[i] + "' >" + PUList[i] + "</option>";
            }
            var option = $(optionStr);
            $ddPU.html("");
            $ddPU.append(option);

            optionStr = "<option value='' >--Please Select--</option>";
            option = $(optionStr);
            $ddMode.html("");
            $ddMode.append(option);

            optionStr = "<option value='' >--Please Select--</option>";
            option = $(optionStr);
            $ddLine.html("");          
            $ddLine.append(option);

          
        }
    }

 


    $ddPU.change(function () {

        var Factory = $ddFactory.val();
        var PU = $ddPU.val();
        if (Factory == "")
        {
            ErrorMsg("请选择Factory");
            return;
        }
        if (PU == "") {
            ErrorMsg("请选择PU");
            return;
        }
        var url = baseUrl + "/IE/IE_STD_ManHour_GetMode";
     
        jsonData = { "Factory": Factory, "PU": PU }
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_STD_ManHour_GetMode, AjaxFailResult);
       
    });
    function IE_STD_ManHour_GetMode(data)
    {
        if (data.result != "OK") { 
            ErrorMsg(data.result);
            return;
        } else {
            var ModeList = data.tableData;
            var optionStr = "<option value='' >--Please Select--</option>";

            for (var i = 0; i < ModeList.length; i++) {
                optionStr = optionStr + "<option value='" + ModeList[i] + "' >" + ModeList[i] + "</option>";
            }
            var option = $(optionStr);
            $ddMode.html("");
            $ddMode.append(option);

            optionStr = "<option value='' >--Please Select--</option>";
            option = $(optionStr);            
            $ddLine.html("");           
            $ddLine.append(option);

        }
    }


    $ddMode.change(function () {

        var Factory = $ddFactory.val();
        var PU = $ddPU.val();
        var Mode = $ddMode.val();
        if (Factory == "") {
            ErrorMsg("请选择Factory");
            return;
        }
        if (PU == "") {
            ErrorMsg("请选择PU");
            return;
        }
        if (Mode == "")
        {
            ErrorMsg("请选择Mode");
            return;
        }
        var url = baseUrl + "/IE/IE_STD_ManHour_GetLine";
    
        jsonData = { "Factory": Factory, "PU": PU,"Mode":Mode }
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_STD_ManHour_GetLine, AjaxFailResult);
       
    });
    function IE_STD_ManHour_GetLine(data) {
        if (data.result != "OK") { 
            ErrorMsg(data.result);
            return;
        } else {
            var LineList = data.tableData;
            var optionStr = "<option value='' >--Please Select--</option>";

            for (var i = 0; i < LineList.length; i++) {
                optionStr = optionStr + "<option value='" + LineList[i] + "' >" + LineList[i] + "</option>";
            }
            var option = $(optionStr);
            $ddLine.html("");
            $ddLine.append(option);
        }
    }

    $ddLine.change(function () {
        var Factory = $ddFactory.val();
        var PU = $ddPU.val();
        var Mode = $ddMode.val();
        var Line = $ddLine.val();
        var Model = $txtModel.val();

 
        if (Line == "") {
            ErrorMsg("请选择Line");
            return;
        }
        var url = baseUrl + "/IE/IE_STD_ManHour_GetData";

        jsonData = { "Factory": Factory, "PU": PU, "Mode": Mode, "Line": Line, "Model": Model };
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_STD_ManHour_GetData, AjaxFailResult);
    });

    function IE_STD_ManHour_GetData(data)
    {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            return;
        } else {
            var tableData = data.tableData;
            tableDataRows = tableData.length;

            $tbResultTable.bootstrapTable('load', tableData);
            OKMsg(" 一共(" + tableDataRows + ")行资料");
        }
        
    }

    $txtModel.keypress(function (e) {
        if (e.which == 13)
        {
            var Factory = $ddFactory.val();
            var PU = $ddPU.val();
            var Mode = $ddMode.val();
            var Line = $ddLine.val();
            var Model = $txtModel.val();
            var url = baseUrl + "/IE/IE_STD_ManHour_GetData";
            if (Model == "")
            {
                return;
            }
            jsonData = { "Factory": Factory, "PU": PU, "Mode": Mode, "Line": Line, "Model": Model };
            ajaxJsonRequestDerived(url, 'post', jsonData, IE_STD_ManHour_GetData, AjaxFailResult);
 
        }
    });

    $btnSave.click(function () {
        //check 数据是否都有填写
        var Factory = $ddFactory.val();
        var PU = $ddPU.val();
        var Mode = $ddMode.val();
        var Line = $ddLine.val();
        var Model = $txtModel.val().trim();
        var CycleTime = $txtCycleTime.val().trim();
        var ManHour = 0.0000;
        var OnlineMan = $txtOnlineMan.val().trim();
        var OfflineMan = $txtOfflineMan.val().trim();
        var ShareRate = $txtShareRate.val().trim();
        var Remarks = $txtRemarks.val().trim();

        var checkMsg = ""
        if (Factory == "") {
            checkMsg =checkMsg + " 请选择Factory ||";
             
        }
        if (PU == "") {
            
            checkMsg =checkMsg + "  请选择PU ||";
        }
        if (Mode == "") {
          
            checkMsg =checkMsg + " 请选择Mode ||";
        }
        if (Line == "")
        {
            checkMsg = checkMsg + " 请选择Line ||";
        }
        if (Model == "") {
            checkMsg = checkMsg + " 请填写Model ||";
        }
        if (Remarks == "") {
            checkMsg = checkMsg + " 请填写Remarks ||";
        }

        if (checkMsg != "")
        {
            ErrorMsg(checkMsg);
            return;
        }

        //检查填写的格式是否正确
        checkMsg = "";
        if (checkNumber(CycleTime) == false)
        {
            checkMsg = checkMsg + " CycleTime请填写数字格式 ||";
        }

 

        if (checkNumberInt(OnlineMan) == false) {
            checkMsg = checkMsg + " OnlineMan请填写整数格式 ||";
        }

        if (checkNumber(OfflineMan) == false) {
            checkMsg = checkMsg + " OfflineMan请填写数字格式 ||";
        }

        if (checkNumber(ShareRate) == false) {
            checkMsg = checkMsg + " ShareRate请填写数字格式 ||";
        }

        if (checkMsg != "") {
            ErrorMsg(checkMsg);
            return;
        }

        if (Mode.substr(Mode.length - 3 , 3).toUpperCase() == "SMT") {
            ManHour = ((Number(OnlineMan) + Number(OfflineMan)) * (1 + Number(ShareRate)) * Number(CycleTime) / 3600) * 0.9198;
        } else {
            ManHour = ((Number(OnlineMan) + Number(OfflineMan)) * (1 + Number(ShareRate)) * Number(CycleTime) / 3600);
        }
        //$txtManHour.attr("disabled", false);
        //$txtManHour.attr("value", String(ManHour));
        $txtManHour.val(ManHour);
        //$txtManHour.attr("disabled", "disabled");

        var url = baseUrl + "/IE/IE_STD_ManHour_GetSaveData1";
       
        jsonData = {
            "Factory": Factory, "PU": PU, "Mode": Mode, "Line": Line, "Model": Model, "CycleTime": Number(CycleTime),
            "ManHour": Number(ManHour), "OnlineMan": Number(OnlineMan), "OfflineMan":Number(OfflineMan), "ShareRate":(ShareRate),
            "Remarks": Remarks

        };
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_STD_ManHour_GetSaveData1, AjaxFailResult);
        
    });
    function IE_STD_ManHour_GetSaveData1(data)
    {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            return;
        } else {
            var tableData = data.tableData;
            tableDataRows = tableData.length;

            $tbResultTable.bootstrapTable('load', tableData);
            OKMsg(" 一共(" + tableDataRows + ")行资料受影响");
        }

    }


    var sitv, uploadOK
    $btnUploadExcel.click(function () {
        var name = $txtFilePath.val();
        var formaData = new FormData();

        uploadOK = 0;
        formaData.append("file", $txtFilePath[0].files[0])
        formaData.append("name", name);

        
        if (name == "" || name == undefined) {
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

        var url = baseUrl + "/IE/IE_STD_ManHour_UploadExcel";
        ajaxJsonRequestFile(url, 'post', formaData, IE_STD_ManHour_UploadExcel, AjaxFailResult);


    });
    function IE_STD_ManHour_UploadExcel(data) {
        if (data.result != "OK") {
 
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
            OKMsg("资料上传成功");
            var UploadFailQty = data.tableData.length;
            if (UploadFailQty > 0) {
                ErrorMsg("(" + UploadFailQty + ")行资料上传失败");
                $tbResultTable.bootstrapTable('load', data.tableData);
            }
          
            return;
        }

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
            var url = baseUrl + "/IE/IE_STD_ManHour_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, IE_STD_ManHour_Delete, AjaxFailResult);
        } 
    });

    function IE_STD_ManHour_Delete(data)
    {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            $ModalDoing.modal('hide');
        } else {
            //移除选中的行
            var ids = $.map($tbResultTable.bootstrapTable('getSelections'), function (row) {
                return (row.ID);
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





    $btnToExcel.click(function () {

        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: 'IE_STD_ManHour',
            tableName: 'IE_STD_ManHour',
            ignoreColumn: [0],
            worksheetName: ['IE_STD_ManHour']
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
                align: 'center',
                title: 'ID',
                visible: false,
            }, {
                field: 'Factory',
                align:'center',
                title: '厂区<br />'
            }, {
                field: 'BU',
              
                title: 'PU<br />'
            }, {
                field: 'Mode',
                title: 'Mode<br />',
                sortable: true
            }, {
                field: 'Line',
                
                title: 'Line<br />',
                sortable: true,

            }, {
                field: 'Model',
                title: 'Model',
               
                sortable: true,

            }, {
                field: 'CycleTime',
                title: 'Cycle<br />Time',
                
            }  , {
                field: 'ManHour',
                title: 'Man<br />Hour',
                 
                sortable: true,

            } , {
                 field: 'Online_Man',
                 title: 'Online<br />Man',
             }, {
                 field: 'Offline_Man',
                 title: 'Offline<br />Man',
             }, {
                 field: 'Share_Man',
                 title: 'Share<br />Man',

             }, {
                 field: 'Share_Rate',
                 title: 'Share<br />Rate',

             }, {
                 field: 'STD_Manpower',
                 title: 'STD<br />Manpower',

             }, {
                 field: 'Output_8Hrs',
                 title: 'Output<br />8H',

             }, {
                 field: 'Output_12Hrs',
                 title: 'Output<br />12H',

             }, {
                 field: 'Balance_Efficiency',
                 title: 'Balance<br />Eff',
                 

             }, {
                 field: 'Issue_Date',
                 title: 'Issue<br />Date',
                 

             }, {
                 field: 'Remarks',
                 title: '说明',
                 
             }, {
                 field: 'UserName',
                 title: '操作人',
                
             }, {
                field: 'TransDateTime',
                title: '日期',
                sortable: true,
               
        }],

        onClickRow: function (row, $element) {
            $ddFactory.val(row.Factory);
            $ddPU.val(row.BU);
            $ddMode.val(row.Mode);
            $ddLine.val(row.Line);
            $txtModel.val(row.Model);
            $txtCycleTime.val(row.CycleTime);
            $txtManHour.val(row.ManHour);
            $txtOnlineMan.val(row.Online_Man);
            $txtOfflineMan.val(row.Offline_Man);
            $txtShareRate.val(row.Share_Rate);
            $txtRemarks.val(row.Remarks);
 

        },
    });

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
})
