$(document).ready(function () {
    var $btnReset = $('#reset');
    var $btnSave = $('#save');
    var $btnToExcel = $('#ToExcel');
    var $btnDelete = $('#delete');
    var $btnAdd = $('#Add');
    var $btnQuery = $('#Query');
    var $btnClose = $('#close');
    //var $btnUploadExcel = $('#UploadExcel')

    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
    var $lblMessageErrorModal = $('#message-error-modal');
    var $lblMessageSuccessModal = $('#message-success-modal');

    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');

    var $tbResultTable = $('#ResultTable');

    var $ModalDoing = $('#doing');
    var $ModalAddInfo = $('#AddInfo');

    var $txtCompanyCode = $('#CompanyCode')
    var $txtCompanyName = $('#CompanyName')
    var $txtCompayLocal = $('#CompayLocal')




    if (QueryStr == null || QueryStr == "") {
        var url = baseUrl + "/QA/QITS_CompanyCodeViewAll";
        ajaxJsonRequestDerived(url, 'post', "", Show_Data, AjaxFailResult);
    } else {
        var url = baseUrl + "/QA/QITS_CompanyCodeView";
        var CompanyCode = getQueryString("CompanyCode")
        var jsondata = { "CompanyCode": CompanyCode };
       
        ajaxJsonRequestDerived(url, 'post', jsondata, Show_Data, AjaxFailResult);
    }

   
    function Show_Data(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);

        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            BtnsControl();
            OKMsg("一共(" + dataLen + ")行");
        }
    }


    $btnAdd.click(function () {
        ShowModal(1);

    });
    $btnReset.click(function () {

        ResetTxt();
    });

    function ResetTxt() {
        $txtCompanyCode.val("");
        $txtCompanyName.val("");
        $txtCompayLocal.val("");
        $lblMessageErrorModal.html("");
        $lblMessageSuccessModal.html("");
    }
    $btnClose.click(function () {
        ResetTxt();
        $ModalAddInfo.modal('hide');
    });
    $btnSave.click(function () {

        var CompanyCode, CompanyName, CompayLocal

        CompanyCode = $txtCompanyCode.val();
        CompanyName = $txtCompanyName.val();
        CompayLocal = $txtCompayLocal.val();

        if (CompanyCode == "") {
            ErrorMsgModal("公司代码 不能为空");
            return;
        }
        if (CompanyName == "") {
            ErrorMsgModal("公司名称 不能为空");
            return;
        }
        if (CompanyCode == "") {
            ErrorMsgModal("单位 不能为空");
            return;
        }

        var url = baseUrl + "/QA/QITS_CompanyCodeSave";
        var jasondata = { "CompanyCode": CompanyCode, "CompanyName": CompanyName, "CompayLocal": CompayLocal }
        ajaxJsonRequestDerived(url, 'post', jasondata, Show_DataModal, AjaxFailResult);

    });

    function Show_DataModal(data) {
        if (data.result != "OK") {
            ErrorMsgModal(data.result);

        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            EnableBtns();
            OKMsg("一共(" + dataLen + ")行");
            OKMsgModal("保存成功");
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
            var url = baseUrl + "/QA/QITS_CompanyCode_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, QITS_CompanyCode_Delete, AjaxFailResult);
        }
    });
    function QITS_CompanyCode_Delete(data) {
        if (data.result != "OK") {

            ErrorMsg(data.result);
            $ModalDoing.modal('hide');
        } else {
            //移除选中的行
            var ids = $.map($tbResultTable.bootstrapTable('getSelections'), function (row) {
                return row.CompanyCode;
            });
            $tbResultTable.bootstrapTable('remove', {
                field: 'CompanyCode',
                values: ids
            });

            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');

        }
    }

    $btnToExcel.click(function () {
        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: 'QITS_CompanyCode',
            tableName: 'QITS_CompanyCode',
            ignoreColumn: [0, 1],
            worksheetName: ['QITS_CompanyCode']
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

    function OKMsgModal(msg) {
        $lblMessageErrorModal.html("");
        $lblMessageSuccessModal.html(msg);
    }
    function ErrorMsgModal(msg) {
        if (msg == "用户登录已失效，请重新登录后操作") {
            //location.reload();
            if ($UserLogin.length != 0) {
                $ModalAddInfo.modal('hide');
                $UserLogOut.html("");
                $UserLogin.html("");
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href="' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350"  > 请重新登录 </a>')


                tb_init('a.thickbox, area.thickbox, input.thickbox');
                imgLoader = new Image();// preload image
                imgLoader.src = tb_pathToImage;
            }

        }
        $lblMessageErrorModal.html(msg);
        $lblMessageSuccessModal.html("");
    }
    function BtnsControl() {
        if (UserRole != "助理") {
            if (UserRole != "Leader") {

                DisableBtns();
            }
            else
            {

                EnableBtns();
            }
        }
        else
        {

            DisableBtns();
        }
    }
    function DisableBtns() {
        $btnAdd.attr("disabled", "disabled")
        $btnSave.attr("disabled", "disabled");
        $btnDelete.attr("disabled", "disabled");
        $btnToExcel.attr("disabled", "disabled");
        $btnQuery.attr("disabled", "disabled");

    }
    function EnableBtns() {
        $btnAdd.attr("disabled", false)
        $btnSave.attr("disabled", false);
        $btnDelete.attr("disabled", false);
        $btnToExcel.attr("disabled", false);
        $btnQuery.attr("disabled", false);


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
                field: 'CompanyCode',
                title: '公司代码', 

            }, {
                field: 'CompanyName',
                title: '公司名称',


            }, {
                field: 'CompayLocal',
                title: '单位(所属区域或产品别)',


            }, {
                field: 'UserID',
                title: '操作人',


            },
            //{

            //    field: "Action",
            //    title: "编辑",
            //    align: "center",
            //    formatter: function (value, row, index) {
            //        var strHtml = "<a href='javascript:void(0);' onclick='ShowModal(2)'  style='margin-left:5px; color:#003399'><li class='glyphicon glyphicon-pencil'></li></a> "
            //        return strHtml;

            //    },
            //    edit: false
            //}
        ],
        onDblClickRow: onDblClickRow

    });
    function onDblClickRow(selectRow) {
        $txtCompanyCode.attr("disabled", "disabled");

        $txtCompanyCode.val(selectRow.CompanyCode);
        $txtCompanyName.val(selectRow.CompanyName);
        $txtCompayLocal.val(selectRow.CompayLocal);

        ShowModal(2);
    }

    function ShowModal(type) //type = 1 表示新增，2表示编辑
    {
  
       
        if (type == 1) {
            $txtCompanyCode.attr("disabled", false);
        } else {
            $txtCompanyCode.attr("disabled", "disabled");
        }
        $ModalAddInfo.modal('show').css({
            width: 'auto',
            'margin-left': function () {
                return ($(this).width() / 10);
            },
            'margin-top': function () {
                return ($(this).height() - 1500 / 2);
            },

        });
    }

})


