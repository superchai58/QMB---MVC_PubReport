

$(document).ready(function () {
    var $multiSelect = $('#multiAuth');
    var $txtUserId = $('#txtUserID');
    var $btnSave = $('#Save');
    var $tbResultTable = $('#ResultTable');
    var Timer;
    var $spanTimer = $('#Timer');

    var $btnDelete = $('#delete'); 
    var $btnToExcel = $('#ToExcel');

    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');

    var $ModalDoing = $('#doing');

    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');

    $multiSelect.multiSelect({
        selectionHeader: '已选区',
        selectableHeader: '待选区',
        keepOrder: true,
        afterSelect: function (values) {
            //alert(values);
           
        }
    });

    $txtUserId.blur( function(){
         
     

        var UserId = $txtUserId.val();
        var url = baseUrl + "/User/AuthManageGetData";
        jsonData = { "UserID": UserId };
        ajaxJsonRequestDerived(url, 'post', jsonData, AuthManageGetDataSucc, AjaxFailResult);
       

    });

    function AuthManageGetDataSucc(data)
    {
        if (data.result != "OK")
        {
            ErrMsg(data.result);
            $btnSave.attr("disabled", true);
            return;
        }
        var tableData = data.tableData;
        var hasAuth=[];
        var UserId = $txtUserId.val();
        var index = 0;
        for (var i = 0; i < tableData.length;i++)
        {
            if (tableData[i].IsPublic == 1)
            {                
           
                var json = { UserID: UserId, MenuName: tableData[i].MenuName };
                hasAuth.push(json);
            }
            else
            {               
                $multiSelect.multiSelect('addOption', { value: tableData[i].MenuName, text: tableData[i].MenuName, index: index });
                index = index + 1;
            }
            
        }
        $btnSave.attr("disabled", false);

        $tbResultTable.bootstrapTable('load', hasAuth);
    }

    $btnSave.click(function () {

        var UserId = $txtUserId.val().trim();
        var url = baseUrl + "/User/AuthManageSaveData";
        var AuthItem = $multiSelect.val();
        
        jsonData = { "UserID": UserId, AuthItem: AuthItem };
        ajaxJsonRequestDerived(url, 'post', jsonData, AuthManageSaveDataSucc, AjaxFailResult);
    });

    function AuthManageSaveDataSucc(data)
    {
        //OKMsg("注册成功,3秒后自动跳转到主页");
        setInterval(go, 1000);
        $ModalDoing.modal('show').css({
            width: 'auto',
            'margin-left': function () {
                return ($(this).width() / 10);
            },
            'margin-top': function () {
                return ($(this).height() / 2);
            },

        });
    }

    Timer = 3
    function go() {
        Timer = Timer - 1;
        if (Timer > 0) {
            $spanTimer.html(Timer);
        }
        else {
            var url = baseUrl;

            window.location.href = url;
        }
    }

    function OKMsg(msg) {
        $lblMessageError.html("");
        $lblMessageSuccess.html(msg)
    }
    function ErrMsg(msg) {
        $lblMessageError.html(msg);
        $lblMessageSuccess.html("");
    }
    $btnToExcel.click(function () {

        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: '权限管理',
            tableName: '权限管理',
            ignoreColumn: [0],
            worksheetName: ['权限管理']
        })
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
            var url = baseUrl + "/User/AuthManageDelete";
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
                return row.MenuName;
            });
            $tbResultTable.bootstrapTable('remove', {
                field: 'MenuName',
                values: ids
            });
 
            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');
            
            var UserId = $txtUserId.val();
            var url = baseUrl + "/User/AuthManageGetData";
            jsonData = { "UserID": UserId };
            ajaxJsonRequestDerived(url, 'post', jsonData, AuthManageGetDataSucc, AjaxFailResult);
        }
    }
    $tbResultTable.bootstrapTable({
        editable: true, //开启编辑模式
        clickToSelect: true,
        search: true,
        datatype: "json",
        showHeader:true,
        toolbar: '#toolbar',//工具按钮用哪个容器
        striped:true,
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
                field: 'Item',
                title: 'Item',
                formatter: function (value, row, index)
                { return index + 1 }
            },  {
                field: 'UserID',
                title: 'UserID'
            }, {
                field: 'MenuName',
                title: 'MenuName'
            } 

        ],
 
    });
 
})