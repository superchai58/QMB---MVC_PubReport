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

    var $txtModel = $('#Model');
    var $txtCustModel = $('#CustModel');    
    var $ddCompanyCode = $('#CompanyCode');
    var $txtDVTDay = $('#DVTDay');
    var $txtPVTDay = $('#PVTDay');
    var $txtMPDay = $('#MPDay');
    var $ddQAPIC = $('#QAPIC'); 

    //绑定日历
    var $Calendar = $('#ddDVTDay');
    $Calendar.calendar({
        trigger: '#DVTDay',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected');

        },
        onClose: function (view, date, data) {
            var Date;
            Date = $txtDVTDay.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            }
       
        }
    });

    $('#ddPVTDay').calendar({
        trigger: '#PVTDay',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected');

        },
        onClose: function (view, date, data) {
            var Date;
            Date = $txtPVTDay.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            }

        }
    });
    $('#ddMPDay').calendar({
        trigger: '#MPDay',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected');

        },
        onClose: function (view, date, data) {
            var Date;
            Date = $txtMPDay.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            }

        }
    });
 

    if (QueryStr == null || QueryStr == "") {

        var url = baseUrl + "/QA/QITS_ModelSettingViewAll";
      
        ajaxJsonRequestDerived(url, 'post', "", Show_Data, AjaxFailResult);
    } else {

        var url = baseUrl + "/QA/QITS_ModelSettingView";
        var Model = getQueryString("Model")
        var jsondata = { "Model": Model };
        ajaxJsonRequestDerived(url, 'post', jsondata, Show_Data, AjaxFailResult);
    }

    function Show_Data(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            if (data.result.substr(0, 9) == "您没有相关操作权限") {
                DisableBtns();
            }

        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            //EnableBtns();
            BtnsControl();
            OKMsg("一共(" + dataLen + ")行");

            //初始化Leader下拉框信息
            url = baseUrl + "/QA/User_ListViewGetQA"
            ajaxJsonRequestDerived(url, 'post', "", Show_QA, AjaxFailResult);
        }
    }

    function Show_QA(data) {
      

        var LeaderJason = data.tableData
        var rowLength = LeaderJason.length;
       
        $ddQAPIC.empty();
        var optionStr = "<option value='' >--Please Select--</option>";
        for (var i = 0; i < rowLength; i++) {
            optionStr = optionStr + "<option value='" + LeaderJason[i]["UserID"] + "' >" + LeaderJason[i]["UserID"] + "(" + LeaderJason[i]["UserName"] + ")</option>";
        }
        var option = $(optionStr);
        $ddQAPIC.append(option);
        $ddQAPIC.comboSelect();
    }

    $btnAdd.click(function () {
        ShowModal(1);

    })
    $btnReset.click(function () {

        ResetTxt();
    });

    function ResetTxt() {
        $txtModel.val("");
        $txtCustModel.val("");
        $ddCompanyCode.val("");
        $txtDVTDay.val("");
        $txtPVTDay.val("");
        $txtMPDay.val("");
        $ddQAPIC.val("");
        $('select').comboSelect();

        $lblMessageErrorModal.html("");
        $lblMessageSuccessModal.html("");
        
    }
    $btnClose.click(function () {
        ResetTxt();
        $ModalAddInfo.modal('hide');
    });
    $btnSave.click(function () {

        var Model, CustModel, CompanyCode, DVTDay, PVTDay, MPDay, QAPIC;

        Model = $txtModel.val();
        CustModel = $txtCustModel.val();
        CompanyCode = $ddCompanyCode.val();
        DVTDay = $txtDVTDay.val();
        PVTDay = $txtPVTDay.val();
        MPDay = $txtMPDay.val();
        QAPIC = $ddQAPIC.val();
        

        if (CompanyCode == "") {
            ErrorMsgModal("公司代码 不能为空");
            return;
        }
        if (CustModel == "") {
            ErrorMsgModal("CustModel 不能为空");
            return;
        }
        if (Model == "") {
            ErrorMsgModal("Model 不能为空");
            return;
        }
        if (QAPIC == "") {
            ErrorMsgModal("QAPIC 不能为空");
            return;
        }
     
        var url = baseUrl + "/QA/QITS_ModelSettingSave";
        var jsonSend = {
            "CompanyCode": CompanyCode, "Model": Model, "CustModel": CustModel,
            "DVTDay": DVTDay, "PVTDay": PVTDay, "MPDay": MPDay,
            "QAPIC": QAPIC 
        }
        var strJson = "[" + JSON.stringify(jsonSend) + "]";
        var jsonData = { "jsonData": strJson };
        ajaxJsonRequestDerived(url, 'post', jsonData, Show_DataModal, AjaxFailResult);

    });

    function Show_DataModal(data) {
        if (data.result != "OK") {
            ErrorMsgModal(data.result);

        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            //EnableBtns();
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
            var url = baseUrl + "/QA/QITS_ModelSettingDelete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, DeleteRow, AjaxFailResult);
        }



    });
    function DeleteRow(data) {
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

            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');

        }
    }

    $btnToExcel.click(function () {
        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: 'QITS_ModelSetting',
            tableName: 'QITS_ModelSetting',
            ignoreColumn: [0, 1],
            worksheetName: ['QITS_ModelSetting']
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

    function BtnsControl()
    {
        if (UserRole != "助理") {
            if (UserRole != "Leader") {
               
                DisableBtns();
            } else
            {
               
                EnableBtns();
            }
        } else
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
            } , {
                field: 'Model',
                title: '广达机种',


            }, {
                field: 'CustModel',
                title: '客户机种',


            }, {
                field: 'CompanyCode',
                title: '公司代码',
                formatter: function (value, row, index) {
                    var aHtml = baseUrl + "/QA/QITS_CompanyCode?Flag=viewQITS_CompanyCode";
                    var strHtml = "<a href='" + aHtml + "&CompanyCode=" + row.CompanyCode + "'   style='margin-left:5px; color:#003399' target='_blank'>" + row.CompanyCode + "</li></a> "
                    return strHtml;

                }

            }, {
                field: 'QAPIC',
                title: 'QA负责人',
                formatter: function (value, row, index) {
                    var aHtml = baseUrl + "/QA/User_List?Flag=User_ListView";
                    var strHtml = "<a href='" + aHtml + "&UserID=" + row.QAPIC + "'   style='margin-left:5px; color:#003399' target='_blank'>" + row.QAPIC + "</li></a> "
                    return strHtml;

                }


            }, {
                field: 'DVTDay',
                title: 'DVTDay',


            }, {
                field: 'PVTDay',
                title: 'PVTDay',


            }, {
                field: 'MPDay',
                title: 'MPDay',


            }, {
                field: 'UserID',
                title: '操作人',


            }, {
                field: 'TransDateTime',
                title: '操作时间',


            }
            //{

            //    field: "Action", title: "编辑", align: "center",
            //    formatter: function (value, row, index) {
            //        var strHtml = "<a href='javascript:void(0);' onclick='ShowModal(2,"+row+")'  style='margin-left:5px; color:#003399'><li class='glyphicon glyphicon-pencil'></li></a> "
            //        return strHtml;

            //    }, edit: false
            //}
        ],
        onDblClickRow: onDblClickRow

    });
    function onDblClickRow(selectRow)
    {
        //$ddCompanyCode.val(selectRow.CompanyCode);
        //$ddDepartMent.val(selectRow.DepartMent);
        //$ddLeader.val(selectRow.Leader);
        $txtModel.val(selectRow.Model);
        $txtCustModel.val(selectRow.CustModel);
        $ddCompanyCode.val(selectRow.CompanyCode);
        $txtDVTDay.val(selectRow.DVTDay);
        $txtPVTDay.val(selectRow.PVTDay);
        $txtMPDay.val(selectRow.MPDay);
        $ddQAPIC.val(selectRow.QAPIC);
        
 
        $('select').comboSelect();
 
        ShowModal(2);
    }

    function ShowModal(type) //type = 1 表示新增，2表示编辑
    {
         
        if (type == 1) {
            $txtModel.attr("disabled", false);
        } else {
            $txtModel.attr("disabled", "disabled");
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


