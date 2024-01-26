$(document).ready(function () {



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
    //var $txtMonth = $('#Month');

    var $txtDT = $("#dt");
    var $Calendar = $('#dd');
    var dt = showdate(0);

    var Auth = "CapacityPMC";
    var Department = "PMC";
    $txtDT.val(dt.substr(0, 6));

   // DisableBtns();

    //$ddPU.change(function () {
    //    DisableBtns();
    //});

    $ddType.change(function () {
        var PU, Type;
        //DisableBtns();
        PU = $ddPU.val();
        Type = $ddType.val();

        if (PU == "")
        {
            ErrorMsg("请选择PU");
            return;
        }
        if (Type == "")
        {
            ErrorMsg("请选择Type");
            return;
        }

 
        var url = baseUrl + "/Upload/CheckRight";
        jsonData = { "PU": PU, "Type": Type, "Auth": Auth }
        ajaxJsonRequestDerived(url, 'post', jsonData, CheckRight, AjaxFailResult);

    });
    function CheckRight(data)
    {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            DisableBtns();
        } else {
            EnableBtns();
        }

    }
    
    //绑定日历
    var $Calendar = $('#dd');
    $Calendar.calendar({
        trigger: '#dt',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected');

        },
        onClose: function (view, date, data) {
            var Date;
            Date = $txtDT.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            }


            //url = baseUrl + "/Query/IE_DailyProductQty_GetPU";
            //jsonData = { "Date": Date }
            //ajaxJsonRequestDerived(url, 'post', jsonData, IE_DailyProductQty_GetPU, AjaxFailResult);
        }
    });



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
            //wb.SheetNames[0]是获取Sheets中第一个Sheet的名字
            //wb.Sheets[Sheet名]获取第一个Sheet的数据  JSON.stringify(w.SheetNames);
            //document.getElementById("demo").innerHTML = JSON.stringify(XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]));
            //document.getElementById("demo").innerHTML = JSON.stringify(wb.SheetNames);
            //document.getElementById("demo").innerHTML = wb.SheetNames;
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
        var PU, Type, Month;

        PU = $ddPU.val();
        Type = $ddType.val();
        Month = $txtDT.val();

        if (PU == "")
        {
            ErrorMsg("请选择PU");
            return;
        }
        if (Type == "") {
            ErrorMsg("请选择Type");
            return;
        }
        if (Month == "")
        {
            ErrorMsg("请选择Month");
            return;
        }

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
        if (excelJson.length == 0) {
            ErrorMsg("没有对应的资料");
            return;
        }
        if (excelJson.length > 1000) {
            ErrorMsg("最多上传1000 行资料");
            return;

        }
        OKMsg("资料正在上传...");
        DisableBtns();
        //sitv = setInterval(function () {
        //    var prog_url = baseUrl + "/IE/ProcessGetStatus";
        //    $.getJSON(prog_url, function (res) {
        //        //$('#prog_in').width(res + '%');

        //        //alert(res.tableData);
        //        if (uploadOK == 1) {
        //            $prog_in.css("width", '100%')
        //        }
        //        else {
        //            $prog_in.css("width", res.tableData + '%')

        //        }

        //    }
        //        , 6000);
        //});
        var formaData = new FormData();
        formaData.append("file", $UploadExcelFilePath[0].files[0])
        formaData.append("name", name);
        formaData.append("SheetName", SheetName);
        formaData.append("PU", PU);
        formaData.append("Type", Type);
        formaData.append("Month", Month);

        var url = baseUrl + "/Upload/Capacity_Plan_UploadExcel";
        //alert(SheetName);
        //var jsonData = { "SheetName": SheetName };
        //alert(JSON.stringify( jsonData));
        //ajaxJsonRequestFile(url, 'post', jsonData, Capacity_Plan_UploadExcel, AjaxFailResult);
        //ajaxJsonRequestDerived(url, 'post', jsonData, Capacity_Plan_UploadExcel, AjaxFailResult);
        ajaxJsonRequestFile(url, 'post', formaData, Capacity_Plan_UploadExcel, AjaxFailResult);
        $ModalDoing.modal('show').css({
            width: 'auto',
            'margin-left': function () {
                return ($(this).width() / 10);
            },
            'margin-top': function () {
                return ($(this).height() / 2);
            },

        });

        //循环检查资料是否合法
        //var newExcelJson = new Array();
        //for (var i = 0; i < excelJson.length; i++) {
        //    if (checkData(excelJson[i]) == true) {
        //        newExcelJson.push(excelJson[i]);

        //    }
        //}
        //alert(excelContext);
        //var excelContext = JSON.stringify(newExcelJson);
        //var url = baseUrl + "/SMT/PCBA_CPU_MaterialSave";
        //var jsonData = { "jsonData": excelContext };
        //ajaxJsonRequestDerived(url, 'post', jsonData, Show_Data, AjaxFailResult);

    })

    function Capacity_Plan_UploadExcel(data) {
        if (data.result != "OK") {

            ErrorMsg(data.result);
            //clearInterval(sitv);
            //uploadOK = 0;
            //$prog_in.css("width", '0%')
            EnableBtns();
            

        } else {
            //clearInterval(sitv);
            uploadOK = 1;
            //$prog_in.css("width", '100%')
            EnableBtns();
            OKMsg("资料上传成功");
            var UploadFailQty = data.tableData.length;
            if (UploadFailQty > 0) {
                ErrorMsg("(" + UploadFailQty + ")行资料上传失败");
                $tbResultTable.bootstrapTable('load', data.tableData);
            }

            
        }

        $ModalDoing.modal('hide')
    }

    $btnQuery.click(function () {
        var PU, Type,Month;
        
        PU = $ddPU.val();
        Type = $ddType.val();
        Month = $txtDT.val();

        if (PU == "" || Type == "" || $txtDT == "")
        {
            ErrorMsg("PU/Type/Month 都不能为空");
            return;
        }

        var url = baseUrl + "/Upload/Capacity_Plan_Query";
        jsonData = { "PU": PU, "Type": Type, "Month": Month, "Department": Department }
        ajaxJsonRequestDerived(url, 'post', jsonData, ShowData, AjaxFailResult);

    });

    function ShowData(data)
    {
        if (data.result != "OK") {

            ErrorMsg(data.result);

        }
        else {

            $tbResultTable.bootstrapTable('load', data.tableData);
            
            OKMsg("一共(" + data.tableData.length + ")行资料");
        }

    }
    $btnRest.click(function () {
        $ddPU.val("");
          $ddType.val("");
      


        $tbResultTable.bootstrapTable('removeAll');
    });
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
            var url = baseUrl + "/Upload/Capacity_Plan_Delete";
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
            //Reset();
        }
    }

    $btnToExcel.click(function () {

        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: 'Capacity_Plan',
            tableName: 'Capacity_Plan',
            ignoreColumn: [0,1],
            worksheetName: ['Capacity_Plan']
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
        $btnUploadExcel.attr("disabled","disabled")

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
                field: 'Department',
                title: 'Department',


            }, {
                field: 'PU',
                title: 'PU',


            }, {
                field: 'Type',
                title: 'Type',


            }, {
                field: 'DateTime',
                title: 'DateTime',


            }, {
                field: 'Factory',
                title: 'Factory',


            }, {
                field: 'Line',
                title: 'Line',


            }, {
                field: 'HRLine',
                title: 'HRLine',


            }, {
                field: 'Shift',
                title: 'Shift',


            }, {
                field: 'IsDocking',
                title: 'IsDocking',


            }, {
                field: 'Quantity',
                title: 'Quantity',


            }, {
                field: 'IsDisplay',
                title: 'IsDisplay',


            }, {
                field: 'Buffer_Qty',
                title: 'Buffer_Qty', 
            }, {
                field: 'Remark',
                title: 'Remark',

            }, {
                field: 'TransDateTime',
                title: '操作日期',


            }, {
                field: 'UserID',
                title: '操作人',


            }],

    });
})