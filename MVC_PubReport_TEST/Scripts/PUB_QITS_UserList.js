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

    var $ddCompanyCode = $('#CompanyCode');
    var $ddDepartMent = $('#DepartMent');
    var $ddLeader = $('#Leader');
    var $ddUserRole = $('#UserRole');
    var $txtUserID = $('#UserID');
    var $txtUserName = $('#UserName');
    var $txtTel = $('#Tel');
    var $txtEmail = $('#Email');




    $ddCompanyCode.change(function () {
        var CompanyCode;
        var option;
        var optionStr;

        CompanyCode = $ddCompanyCode.val();
        $ddDepartMent.empty();
        optionStr = "<option value='' >--Please Select--</option>";

        if (CompanyCode == "QTA") {
            $txtUserID.attr("disabled", false);
           

            
            optionStr = optionStr + '<option value ="QM">QM</option>';
            optionStr = optionStr + '<option value ="PE-EE">PE-EE</option>';
            optionStr = optionStr + '<option value ="PE-SW">PE-SW</option>';
            optionStr = optionStr + '<option value ="PE-ME">PE-ME</option>';
            optionStr = optionStr + '<option value ="Other">其他</option>';                                 
            

            

        } else {
            $txtUserID.attr("disabled", "disabled");
            optionStr = optionStr + '<option value ="RD">RD</option>';
            optionStr = optionStr + '<option value ="PE">PE</option>';
            optionStr = optionStr + '<option value ="SALES">SALES</option>';
            optionStr = optionStr + '<option value ="QA">QA</option>';
            optionStr = optionStr + '<option value ="Other">其他</option>';
        }
        option = $(optionStr);
        $ddDepartMent.append(option);
        $ddDepartMent.comboSelect();

    });

    if (QueryStr == null || QueryStr == "") {

        var url = baseUrl + "/QA/User_ListViewAll";
      
        ajaxJsonRequestDerived(url, 'post', "", Show_Data, AjaxFailResult);
    } else {

        var url = baseUrl + "/QA/User_ListView";
        var UserID = getQueryString("UserID")
        var jsondata = { "UserID": UserID };
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
            url = baseUrl + "/QA/User_ListViewGetLeader"
            ajaxJsonRequestDerived(url, 'post', "", Show_Leader, AjaxFailResult);
        }
    }

    function Show_Leader(data) {
        //alert(data.tableData);

        var LeaderJason = data.tableData
        var rowLength = LeaderJason.length;
        //alert(rowLength);
        $ddLeader.empty();
        var optionStr = "<option value='' >--Please Select--</option>";
        for (var i = 0; i < rowLength; i++) {
            optionStr = optionStr + "<option value='" + LeaderJason[i]["UserID"] + "' >" + LeaderJason[i]["UserID"] + "(" + LeaderJason[i]["UserName"] + ")</option>";
        }
        var option = $(optionStr);
        $ddLeader.append(option);
        $ddLeader.comboSelect();
    }

    $btnAdd.click(function () {
        ShowModal(1);

    })
    $btnReset.click(function () {

        ResetTxt();
    });

    function ResetTxt() {
        $ddCompanyCode.val("");
        $ddDepartMent.val("");
        $ddLeader.val("");
        $ddUserRole.val("");
        $txtUserID.val("");
        $txtUserName.val("");
        $txtTel.val("");
        $txtEmail.val("");

        $lblMessageErrorModal.html("");
        $lblMessageSuccessModal.html("");
    }
    $btnClose.click(function () {
        ResetTxt();
        $ModalAddInfo.modal('hide');
    });
    $btnSave.click(function () {

        var CompanyCode, DepartMent, Leader, UserRole, UserID, UserName, Tel, Email

        CompanyCode = $ddCompanyCode.val();
        DepartMent = $ddDepartMent.val();
        Leader = $ddLeader.val();
        UserRole = $ddUserRole.val();
        UserID = $txtUserID.val();
        UserName = $txtUserName.val();
        Tel = $txtTel.val();
        Email = $txtEmail.val();

        if (CompanyCode == "") {
            ErrorMsgModal("公司代码 不能为空");
            return;
        }
        if (DepartMent == "") {
            ErrorMsgModal("DepartMent 不能为空");
            return;
        }
        if (CompanyCode == "") {
            ErrorMsgModal("单位 不能为空");
            return;
        }
        if (UserName == "") {
            ErrorMsgModal("姓名 不能为空");
            return;
        }

        var url = baseUrl + "/QA/User_ListSave";
        var jsonSend = {
            "CompanyCode": CompanyCode, "DepartMent": DepartMent, "Leader": Leader,
            "UserRole": UserRole, "UserID": UserID, "UserName": UserName,
            "Tel": Tel, "Email": Email
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
            var url = baseUrl + "/QA/User_List_Delete";
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
                return row.UserID;
            });
            $tbResultTable.bootstrapTable('remove', {
                field: 'UserID',
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
            fileName: 'User_List',
            tableName: 'User_List',
            ignoreColumn: [0, 1],
            worksheetName: ['User_List']
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
            }, {
                field: 'UserID',
                title: '员工号',



            }, {
                field: 'UserName',
                title: '姓名',


            }, {
                field: 'UserRole',
                title: 'UserRole',


            }, {
                field: 'CompanyCode',
                title: '公司代码',
                formatter: function (value, row, index) {
                    var aHtml = baseUrl + "/QA/QITS_CompanyCode?Flag=viewQITS_CompanyCode";
                    var strHtml = "<a href='" + aHtml + "&CompanyCode=" + row.CompanyCode + "'   style='margin-left:5px; color:#003399' target='_blank'>" + row.CompanyCode + "</li></a> "
                    return strHtml;

                }

            }, {
                field: 'PU',
                title: 'PU',


            }, {
                field: 'Type',
                title: 'Type',


            }, {
                field: 'DepartMent',
                title: 'DepartMent',


            }, {
                field: 'Tel',
                title: 'Tel',


            }, {
                field: 'Email',
                title: 'Email',


            }, {
                field: 'Leader',
                title: 'Leader',


            },
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
        $ddCompanyCode.val(selectRow.CompanyCode);
        $ddDepartMent.val(selectRow.DepartMent);
        $ddLeader.val(selectRow.Leader);

        
        switch (selectRow.UserRole)
        {
            case "Leader": $ddUserRole.val("0"); break;
            case "助理": $ddUserRole.val("1"); break;
            case "工程师": $ddUserRole.val("2"); break;
            default: $ddUserRole.val("");

        }
        $('select').comboSelect();
        $txtUserID.val(selectRow.UserID);
        $txtUserName.val(selectRow.UserName);
        $txtTel.val(selectRow.Tel);
        $txtEmail.val(selectRow.Email);
        ShowModal(2);
    }

    function ShowModal(type) //type = 1 表示新增，2表示编辑
    {
 

        if (type == 1) {
            $txtUserID.attr("disabled", false);
        } else {
            $txtUserID.attr("disabled", "disabled");
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


