

$(document).ready(function () {
    var $tbResultTable = $('#ResultTable');   
    var $btnToExcel = $('#ToExcel');
    var $btnSave = $('#save');
    var $btnDelete = $('#delete');
    var $btnUploadExcel = $('#UploadExcel');   
    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
    var $prog_in = $('#prog_in');
    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');
    var $ModalDoing = $('#doing');
    var $txtFilePath = $('#UploadExcelFilePath');

    var $txtLine = $('#Line');
    var $ddShift = $('#Shift');
    var $txtLineNO = $('#LineNO');
    var $txtDepartNO = $('#DepartNO');
    var $txtMode = $('#Mode');




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

        var url = baseUrl + "/IE/IE_DepartMent_UploadExcel";
        ajaxJsonRequestFile(url, 'post', formaData, UploadSuceess, AjaxFailResult);


    });
    function UploadSuceess(data) {
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
            OKMsg("资料上传完成");
            var UploadFailQty = data.tableData.length;
            if (UploadFailQty > 0)
            {
                ErrorMsg("(" + UploadFailQty + ")行资料上传失败");
                $tbResultTable.bootstrapTable('load', data.tableData);
            }
            
            return;
        }

    }

    $txtLine.blur(function () {
        var Line;
        Line = $txtLine.val().trim();
        if (Line != "")
        {
            var url = baseUrl + "/IE/IE_DepartMent_GetData";

            jsonData = { "Line": Line}
            ajaxJsonRequestDerived(url, 'post', jsonData, IE_DepartMent_GetData, AjaxFailResult);
        }
    })

    $btnSave.click(function () {
        var Line;
        var Shift;
        var LineNo;
        var DepartNo;
        var Mode;
        var Msg;

        Line = $txtLine.val().trim();
        Shift = $ddShift.val();
        LineNo = $txtLineNO.val().trim();
        DepartNo = $txtDepartNO.val().trim();
        Mode = $txtMode.val().trim();

        Msg = "";
        if (Line == "")
        {
            Msg = Msg+ "[Line] 不能为空 || ";
        }
        if (Shift == "") {
            Msg = Msg + "[Shift] 不能为空 || ";
        }
        if (LineNo == "") {
            Msg = Msg + "[LineNo] 不能为空 || ";
        }
        if (DepartNo == "") {
            Msg = Msg + "[DepartNo] 不能为空 || ";
        }
        if (Mode == "") {
            Msg = Msg + "[Mode] 不能为空 || ";
        }
        if (Msg != "")
        {
            ErrorMsg(Msg);
            return;
        }
        
        DisableBtns();
        var url = baseUrl + "/IE/IE_DepartMent_SavaData";

        jsonData = { "Line": Line, "Shift": Shift, "LineNo": LineNo, "DepartNo": DepartNo, "Mode": Mode }
        ajaxJsonRequestDerived(url, 'post', jsonData, IE_DepartMent_SavaData, AjaxFailResult);

    });

    function IE_DepartMent_SavaData(data)
    {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            OKMsg("一共(" + data.tableData.length + ")行资料受影响");
        }
        EnableBtns();
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
            var url = baseUrl + "/IE/IE_DepartMent_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, IE_DepartMent_Delete, AjaxFailResult);
        }



    });
    function IE_DepartMent_Delete(data) {
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

    $btnToExcel.click(function () {

        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: 'IE_DepartMent',
            tableName: 'IE_DepartMent',
            ignoreColumn: [0],
            worksheetName: ['IE_DepartMent']
        })
    });

    //初始化table
    var url = baseUrl + "/IE/IE_DepartMent_GetData"; 
    ajaxJsonRequestDerived(url, 'post', "", IE_DepartMent_GetData, AjaxFailResult);
    function IE_DepartMent_GetData(data)
    {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            OKMsg("一共(" + data.tableData.length + ")行");
        }
       
    }
    //初始化table end
    $tbResultTable.bootstrapTable({
        editable: true, //开启编辑模式
 
        //url: '../IE/IE_DepartMent_GetData',
     
       // sidePagination: "server",
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
                field: 'Line',
                align: 'center',
                title: 'Line'
            }, {
                field: 'Shift',
                title: 'Shift'
            }, {
                field: 'Line_No',
                title: 'LineNo',
                sortable: true
            }, {
                field: 'Depart_No',
                title: 'DepartNo',
                sortable: true,

            }, {
                field: 'Mode',
                title: 'Mode', 
            }, {
                field: 'UserID',
                title: '操作人',
            }, {
                field: 'TransDateTime',
                title: '日期',
            }],

        onClickRow: function (row, $element) {
            $txtLine.val(row.Line);
            $ddShift.val(row.Shift);
            $txtMode.val(row.Mode);
            $txtLineNO.val(row.Line_No);
            $txtDepartNO.val(row.Depart_No);
           
           


        },
    });
})