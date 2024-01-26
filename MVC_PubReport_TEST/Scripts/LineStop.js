$(document).ready(function () {


    var KeyName = $("#KeyName").text().trim();

    var $btnRest = $('#rest');
    var $btnSave = $('#save');
    var $btnToExcel = $('#ToExcel');
    var $btnDelete = $('#delete');
    var $btnQuery = $('#Query');
    var $btnUploadExcel = $('#UploadExcel')

    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');

    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');

    var $tbResultTable = $('#ResultTable');

    var $ModalDoing = $('#doing');
    var $UploadExcelFilePath = $('#UploadExcelFilePath');
    var $ddSheetName = $('#SheetName');

    var $ddPU = $('#PU');
    var $ddType = $('#Type');
    var $ddTableName = $('#TableName');
    var $selErrType = $('#ErrType');
    var $selRepCode = $('#RepCode');

    var Auth = KeyName;
    var chkFlag = "Y";


    // DisableBtns();

    //$ddPU.change(function () {
    //    DisableBtns();
    //});

    //$ddType.change(function () {
    //    var PU, Type;
    //    //DisableBtns();
    //    PU = $ddPU.val();
    //    Type = $ddType.val();

    //    if (PU == "") {
    //        ErrorMsg("请选择PU");
    //        return;
    //    }
    //    if (Type == "") {
    //        ErrorMsg("请选择Type");
    //        return;
    //    }


    //    var url = baseUrl + "/Upload/CheckRight";
    //    jsonData = { "PU": PU, "Type": Type, "Auth": Auth }
    //    ajaxJsonRequestDerived(url, 'post', jsonData, CheckRight, AjaxFailResult);

    //});
    //function chkRight(PU,Type,Auth) {
    //    var chkurl = baseUrl + "/Upload/CheckRight";
    //    jsonData = { "PU": PU, "Type": Type, "Auth": Auth }
    //    ajaxJsonRequestDerived(chkurl, 'post', jsonData, CheckRight, AjaxFailResult);
    //    return chkFlag;
    //}
    //function CheckRight(data) {
    //    if (data.result != "OK") {
    //        ErrorMsg(data.result);
    //        chkFlag = "N";
    //    } else {
    //        chkFlag = "Y";
    //    }

    //}

    var wb;//读取完成的数据
    var rABS = false; //是否将文件读取为二进制字符串
    $UploadExcelFilePath.change(function (e) {
        var importfile_maxsize = 1 * 1024;

        var f = e.target.files
        var reader = new FileReader();
        var SheetNames;
        var suffix = f[0].name.split(".")[1];
        if (suffix != "xls" && suffix != "xlsx") {
            ErrorMsg("导入文件格式不是xls/xlsx 格式");
            return;
        }
        if (f[0].size / 1024 > importfile_maxsize) {
            ErrorMsg("导入的表格文件不能大于1M");
            return;
        }
        wb = undefined;
        reader.onload = function (e) {
            var data = e.target.result;
            if (rABS) {
                wb = XLSX.read(btoa(fixdata(data)), {//手动转化
                    type: 'base64'
                });
            } else {
                wb = XLSX.read(data, {
                    type: 'binary'
                });
            }
            SheetNames = wb.SheetNames;
            // alert("SheetNames:" + SheetNames.length)

            $ddSheetName.empty();
            var optionStr = "<option value='' >--Please Select--</option>";
            for (var i = 0; i < SheetNames.length; i++) {
                optionStr = optionStr + "<option value='" + SheetNames[i] + "' >" + SheetNames[i] + "</option>";
            }
            //alert("optionStr:" + optionStr);
            var option = $(optionStr);
            $ddSheetName.append(option);
            //alert(KeyName);
            if (KeyName == "RepGrade") {
                optionStr = "<option value='ALL' >ALL</option>";
                option = $(optionStr);
                $ddSheetName.append(option);
            }
            $ddSheetName.comboSelect();
            //alert(JSON.stringify(XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]])));
        };
        if (rABS) {
            reader.readAsArrayBuffer(f[0]);
        } else {
            reader.readAsBinaryString(f[0]);
        }
    })
    function fixdata(data) { //文件流转BinaryString
        var o = "",
            l = 0,
            w = 10240;
        for (; l < data.byteLength / w; ++l) o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w, l * w + w)));
        o += String.fromCharCode.apply(null, new Uint8Array(data.slice(l * w)));
        return o;
    }


    //var sitv, uploadOK
    $btnUploadExcel.click(function (e) {
        var filePath = "";
        var SheetName = "";
        var PU, Type

        PU = $ddPU.val();
        Type = $ddType.val();

        if (PU == "") {
            ErrorMsg("请选择PU");
            return;
        }
        if (Type == "") {
            ErrorMsg("请选择Type");
            return;
        }
        //chkRight(PU, Type, Auth);
        //if (chkFlag == "N") {
        //    return;
        //}

        filePath = $UploadExcelFilePath.val();
        SheetName = $ddSheetName.val();

        if (filePath == "" || filePath == undefined) {
            ErrorMsg("请选择文件");
            return;
        }
        if (SheetName == "" || SheetName == undefined) {
            ErrorMsg("请选择SheetName");
            return;
        }
        //var excelContext = JSON.stringify(XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]));
        var excelJson = XLSX.utils.sheet_to_json(wb.Sheets[SheetName]);

        //alert(SheetName);
        if (excelJson.length == 0 && SheetName != "ALL") {
            ErrorMsg("没有对应的资料");
            return;
        }
        if (excelJson.length > 1000) {
            ErrorMsg("最多上传1000 行资料");
            return;

        }
        OKMsg("资料正在上传...");
        DisableBtns();
        var formaData = new FormData();
        formaData.append("file", $UploadExcelFilePath[0].files[0])
        formaData.append("name", name);
        formaData.append("SheetName", SheetName);
        formaData.append("PU", PU);
        formaData.append("Type", Type);
        formaData.append("Auth", Auth);

        var url = baseUrl + "/SF/" + KeyName + "_UploadExcel";
        ajaxJsonRequestFile(url, 'post', formaData, ErrorCode_UploadExcel, AjaxFailResult);
        $ModalDoing.modal('show').css({
            width: 'auto',
            'margin-left': function () {
                return ($(this).width() / 10);
            },
            'margin-top': function () {
                return ($(this).height() / 2);
            },

        });

    })

    function ErrorCode_UploadExcel(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            EnableBtns();

        } else {
            EnableBtns();
            OKMsg(data.rowMsg);
            $tbResultTable.bootstrapTable('load', data.tableData);
        }

        $ModalDoing.modal('hide')
    }

    //$btnQuery.click(function () {
    //    var PU, Type;

    //    PU = $ddPU.val();
    //    Type = $ddType.val();
    //    TableName = $ddTableName.val();
    //    //alert(TableName);
    //    //alert(KeyName.trim());
    //    if (PU == "" || Type == "") {
    //        ErrorMsg("PU/Type 不能为空");
    //        return;
    //    }
    //    if (KeyName == "RepGrade") {
    //        if (TableName == "") {
    //            ErrorMsg("查询时TableName不能为空");
    //            return;
    //        }
    //        var url = baseUrl + "/SF/Upload_Query";
    //        jsonData = { "PU": PU, "Type": Type, "Auth": Auth, "Table": TableName }
    //        ajaxJsonRequestDerived(url, 'post', jsonData, ShowData, AjaxFailResult)
    //    }
    //    else {
    //        //chkRight(PU, Type, Auth);
    //        //if (chkRight(PU, Type, Auth) == "N") {
    //        //    return;
    //        //}
    //        //alert(KeyName);
    //        var url = baseUrl + "/SF/Upload_Query";
    //        if (KeyName == "ErrorCode" || KeyName == "KeyPartSupplier") {
    //            var ID = "";
    //            if (KeyName == "ErrorCode") {
    //                ID = $selErrType.val();
    //            }
    //            if (KeyName == "KeyPartSupplier") {
    //                ID = $selRepCode.val();
    //            }
    //            jsonData = { "PU": PU, "Type": Type, "Auth": Auth, "Table": KeyName, "ID": ID }
    //        }
    //        else {
    //            jsonData = { "PU": PU, "Type": Type, "Auth": Auth, "Table": KeyName }
    //        }
    //        ajaxJsonRequestDerived(url, 'post', jsonData, ShowData, AjaxFailResult)
    //    };

    //});

    function ShowData(data) {
        if (data.result != "OK") {

            ErrorMsg(data.result);
            $tbResultTable.bootstrapTable('removeAll');
        }
        else {

            $tbResultTable.bootstrapTable('load', data.tableData);

            OKMsg("一共(" + data.tableData.length + ")行资料");
        }

    }

    $btnDelete.click(function () {
        var PU, Type;

        PU = $ddPU.val();
        Type = $ddType.val();
        TableName = $ddTableName.val();
        //alert(TableName);
        if (PU == "" || Type == "") {
            ErrorMsg("PU/Type 不能为空");
            return;
        }
        //chkRight(PU, Type, Auth);
        //if (chkFlag == "N") {
        //    return;
        //}
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
            
            
            var url = baseUrl + "/SF/LineStop_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows), "PU": PU, "Type": Type, "Auth": Auth };
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
            //Reset();
        }
    }

    $btnToExcel.click(function () {

        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: KeyName,
            tableName: KeyName,
            ignoreColumn: [0, 1],
            worksheetName: [KeyName]
        })
    });


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
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href="' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350"  > 请重新登录 </a>')


                tb_init('a.thickbox, area.thickbox, input.thickbox');
                imgLoader = new Image();// preload image
                imgLoader.src = tb_pathToImage;
            }

        }
        $lblMessageError.html(msg);
        $lblMessageSuccess.html("");
    }
    function DisableBtns() {
        $btnSave.attr("disabled", "disabled");
        $btnDelete.attr("disabled", "disabled");
        $btnToExcel.attr("disabled", "disabled");
        $btnQuery.attr("disabled", "disabled");
        $btnUploadExcel.attr("disabled", "disabled")

    }
    function EnableBtns() {
        //$btnSave.val("添加/更新");
        $btnSave.attr("disabled", false);


        //$btnDelete.val("删除");
        $btnDelete.attr("disabled", false);


        //$btnToExcel.val("导出数据Excel");
        $btnToExcel.attr("disabled", false);
        $btnQuery.attr("disabled", false);
        $btnUploadExcel.attr("disabled", false);


    }

})