

$(document).ready(function () {
    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');
    var $ModalDoing = $('#doing');
    var $tbLinkSever = $('#ResultTableLinkServer');
    var $tbSVR_Define = $('#ResultTableSVR_Define');

    var $btnDelLinkServer = $('#DelLinkServer');
    var $btnCreateLinkServer = $('#CreateLinkServer');
    var $btnResetLinkServer = $('#ResetLinkServer');
    var $btnShowLinkServer = $('#ShowLinkServer');

    var $btnReseSVR_Define = $('#ReseSVR_Define');
    var $btnCreateSVR_Define = $('#CreateSVR_Define');
    var $ddShowSVR_Define = $('#ShowSVR_Define');
    var $btnDelSVR_Define = $('#DelSVR_Define');

    var $txtLocalIP = $('#LocalIP');
    var $txtRemoteIP = $('#RemoteIP');
    var $txtLoginUser = $('#LoginUser');
    var $txtPW = $('#PW');
    var $txtServerDesc = $('#ServerDesc');

    var $txtBU = $('#BU');
    var $txtBUType = $('#BUType');
    var $txtCustomer = $('#Customer');
    var $txtCName = $('#CName');
    var $txtItem = $('#Item');
    var $txtContext = $('#Context');
    var $txtIP = $('#IP');
    var $txtDBName = $('#DBName');
    var $txtDescription = $('#Description');

    $btnResetLinkServer.click(function () {
        $txtRemoteIP.val("");
        $txtLoginUser.val("");
        $txtPW.val("");
        $txtServerDesc.val("");
    });
    $btnReseSVR_Define.click(function () {
        $txtBU.val("");
        $txtBUType.val("");
        $txtCustomer.val("");
        $txtCName.val("");
       
        $txtContext.val("");
        $txtIP.val("");
        $txtDBName.val("");
        $txtDescription.val("");
    });
    //初始化table
   
    $btnShowLinkServer.click(function () {
        var url = baseUrl + "/DBA/LinkedServer_View";
        ajaxJsonRequestDerived(url, 'post', "", Show_Data, AjaxFailResult);

    });
    //$btnShowSVR_Define.click(function () {
    //    var url = baseUrl + "/DBA/SVR_Define_View";
    //    ajaxJsonRequestDerived(url, 'post', "", Show_Data_SVR_Define, AjaxFailResult);
    //})
    $ddShowSVR_Define.change(function () {
        var item;
        item = $ddShowSVR_Define.val();

        if (item == "")
        {
            ErrorMsg("请选择查询的类别");
            return;
        }
        var url = baseUrl + "/DBA/SVR_Define_View";
        jsonData = { "Item": item };
        ajaxJsonRequestDerived(url, 'post', jsonData, Show_Data_SVR_Define, AjaxFailResult);
    })
    //初始化table end
    $btnCreateSVR_Define.click(function () {
        var BU, BUType, Customer, CName, Item, Context, IP, DBName, Description;

        BU = $txtBU.val();
        BUType = $txtBUType.val();
        Customer = $txtCustomer.val();
        CName = $txtCName.val();
        Item = $txtItem.val();
        Context = $txtContext.val();
        IP = $txtIP.val();
        DBName = $txtDBName.val();
        Description = $txtDescription.val();

        var MSG = "";
        if (BU == "")
        {
            MSG = MSG + "[BU] 不能为空 ||";
        }
        if (BUType == "") {
            MSG = MSG + "[BUType] 不能为空 ||";
        }

        if (Customer == "") {
            MSG = MSG + "[Customer] 不能为空 ||";
        }

        if (CName == "") {
            MSG = MSG + "[CName] 不能为空 ||";
        }

        if (Item == "") {
            MSG = MSG + "[Item] 不能为空 ||";
        }

        if (Context == "") {
            MSG = MSG + "[Context] 不能为空 ||";
        }

        if (IP == "") {
            MSG = MSG + "[IP] 不能为空 ||";
        }

        if (DBName == "") {
            MSG = MSG + "[DBName] 不能为空 ||";
        }
        if (Item == "") {
            MSG = MSG + "[Item] 不能为空 ||";
        }

        if (MSG != "")
        {
            ErrorMsg(MSG);
            return;
        }
        var url = baseUrl + "/DBA/SVR_Define_Save";
        jsonData = { "BU": BU, "BUType": BUType, "Customer": Customer, "CName": CName, "Item": Item, "Context": Context, "IP": IP, "DBName": DBName, "Description": Description };
        ajaxJsonRequestDerived(url, 'post', jsonData, Show_Data_SVR_Define, AjaxFailResult);
        
    });
    $btnCreateLinkServer.click(function () {
        var LocalIP;
        var RemoteIP;
        var LoginUser;
        var PW;
        var ServerDesc;

        LocalIP = $txtLocalIP.val();
        RemoteIP = $txtRemoteIP.val();
        LoginUser = $txtLoginUser.val();
        PW = $txtPW.val();
        ServerDesc = $txtServerDesc.val();
        var MSG = "";
        if (LocalIP == "")
        {
            MSG = MSG + '[本地IP] 不能为空 || ';
        }
        if (RemoteIP == "") {
            MSG = MSG + '[远程IP] 不能为空 || ';
        }
        if (LoginUser == "") {
            MSG = MSG + '[登录账号] 不能为空 || ';
        }
        if (PW == "") {
            MSG = MSG + '[密码] 不能为空 || ';
        }
        if (MSG != "") {
            ErrorMsg(MSG);

            return;
        }

        var url = baseUrl + "/DBA/LinkedServer_Save";
        jsonData = { "LocalIP": LocalIP, "RemoteIP": RemoteIP, "LoginUser": LoginUser, "PW": PW, "ServerDesc": ServerDesc };
        ajaxJsonRequestDerived(url, 'post', jsonData, Show_Data, AjaxFailResult);

    });

    function Show_Data(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbLinkSever.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            OKMsg("一共(" + dataLen + ")行");
        }

    }
 
    function Show_Data_SVR_Define(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbSVR_Define.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            OKMsg("一共(" + dataLen + ")行");
        }

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
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href=' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350   > 请重新登录 </a>')


                tb_init('a.thickbox, area.thickbox, input.thickbox');
                imgLoader = new Image();// preload image
                imgLoader.src = tb_pathToImage;
            }

        }
        $lblMessageError.html(msg);
        $lblMessageSuccess.html("");
    }

    $btnDelSVR_Define.click(function () {
        var selectRows = $tbSVR_Define.bootstrapTable('getSelections');


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
            var url = baseUrl + "/DBA/SVR_Define_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, SVR_Define_Delete, AjaxFailResult);
        }
    });

    function SVR_Define_Delete(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            $ModalDoing.modal('hide');

        } else {
            //移除选中的行
            var ids = $.map($tbSVR_Define.bootstrapTable('getSelections'), function (row) {
                return row.ID;
            });
            $tbSVR_Define.bootstrapTable('remove', {
                field: 'ID',
                values: ids
            });
            //$lblMessageError.html("");
            //$lblMessageSuccess.html("(" + ids.length + ")行资料已删除");
            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');
        }
    }

    $btnDelLinkServer.click(function () {
        var selectRows = $tbLinkSever.bootstrapTable('getSelections');

 
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
            var url = baseUrl + "/DBA/LinkedServer_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, Delete_Data, AjaxFailResult);
        }



    });
    function Delete_Data(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            $ModalDoing.modal('hide');

        } else {
            //移除选中的行
            var ids = $.map($tbLinkSever.bootstrapTable('getSelections'), function (row) {
                return row.IP;
            });
            $tbLinkSever.bootstrapTable('remove', {
                field: 'IP',
                values: ids
            });
            //$lblMessageError.html("");
            //$lblMessageSuccess.html("(" + ids.length + ")行资料已删除");
            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');
        }
    }
    
    $tbSVR_Define.bootstrapTable({
        editable: true, //开启编辑模式 
        clickToSelect: true,
        search: true,
        datatype: "json",
        toolbar: '#toolbarSVR_Define',//工具按钮用哪个容器
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
                //visible: false,
            }, {
                field: 'BU',

                title: 'BU'
            }, {
                field: 'BUType',
                title: 'BUType',
           
            }, {
                field: 'Customer',
                title: 'Customer',

            }, {
                field: 'CName',
                title: 'CName',


            }, {
                field: 'Item',
                title: 'Item',
            }, {
                field: 'Context',
                title: 'Context',
            }
            , {
                field: 'IP',
                title: 'IP',
            }
            , {
                field: 'DBName',
                title: 'DBName',
            }, {
                field: 'ISNew',
                title: 'ISNew',
            }, {
                field: 'Description',
                title: 'Description',
            }
        ],

        onClickRow: function (row, $element) {
            $txtBU.val(row.BU);
            $txtBUType.val(row.BUType);
            $txtCustomer.val(row.Customer);
            $txtCName.val(row.CName);
            $txtItem.val(row.Item);
            $txtContext.val(row.Context);
            $txtIP.val(row.IP);
            $txtDBName.val(row.DBName);
            $txtDescription.val(row.Description);

        },
    });


    $tbLinkSever.bootstrapTable({
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
            },   {
                field: 'LocalIP',
          
                title: 'LocalIP'
            }, {
                field: 'IP',
                title: 'IP',
                sortable:true,
            }, {
                field: 'UserName',
                title: 'UserName',
     
            }, {
                field: 'PWD',
                title: 'PWD',
        

            }, {
                field: 'Desc1',
                title: 'Desc1',
            } ],

        onClickRow: function (row, $element) {
            $txtLocalIP.val(row.LocalIP);
            $txtRemoteIP.val(row.IP);
            $txtLoginUser.val(row.UserName);
            $txtPW.val(row.PWD);
            $txtServerDesc.val(row.Desc1);

        },
    });



    //MainDB_SysConfig
    var $tbrsMainDB_SysConfig = $('#rsMainDB_SysConfig');

    var $btnReseShowMainDB_SysConfig = $('#ReseShowMainDB_SysConfig');
    var $btnCreateMainDB_SysConfig = $('#CreateMainDB_SysConfig');
    var $btnShowMainDB_SysConfig = $('#ShowMainDB_SysConfig');
    var $btnDelMainDB_SysConfig = $('#DelMainDB_SysConfig');

    var $txtMainDB_SysConfigBU = $('#MainDB_SysConfig_BU');
    var $txtMainDB_SysConfigType = $('#MainDB_SysConfig_Type');
    var $txtMainDB_SysConfigServerIP = $('#ServerIP');
    var $txtMainDB_SysConfigServerName = $('#ServerName');
    var $ddMainDB_SysConfigServerType = $('#ServerType');
    var $txtMainDB_SysConfigClusterIP = $('#ClusterIP');
    var $txtMainDB_SysConfigListenIP = $('#ListenIP');
    var $txtMainDB_SysConfigDescription = $("#MainDB_SysConfig_Description");

    $btnShowMainDB_SysConfig.click(function () {
        var url = baseUrl + "/DBA/MainDB_SysConfig_View";       
        ajaxJsonRequestDerived(url, 'post', "", Show_Data_MainDB_SysConfig, AjaxFailResult);
    });
    $btnCreateMainDB_SysConfig.click(function () {
        var BU, BUType, ServerName, ServerType, ServerIP, ClusterIP, ListenIP, Description;
        var Msg = "";
        BU = $txtMainDB_SysConfigBU.val().trim();
        BUType = $txtMainDB_SysConfigType.val().trim();
        ServerName = $txtMainDB_SysConfigServerName.val().trim();
        ServerType = $ddMainDB_SysConfigServerType.val().trim();
        ServerIP = $txtMainDB_SysConfigServerIP.val().trim();
        ClusterIP = $txtMainDB_SysConfigClusterIP.val().trim();
        ListenIP = $txtMainDB_SysConfigListenIP.val().trim();
        Description = $txtMainDB_SysConfigDescription.val().trim();

        
        if (BU == "")
        {
            Msg = Msg + "[BU] 不能为空 || ";
        }
        if (BUType == "") {
            Msg = Msg + "[BUType] 不能为空 || ";
        }
        if (ServerName == "") {
            Msg = Msg + "[ServerName] 不能为空 || ";
        }
        if (ServerType == "") {
            Msg = Msg + "[ServerType] 不能为空 || ";
        }
        if (ServerIP == "") {
            Msg = Msg + "[ServerIP] 不能为空 || ";
        }
        if (ClusterIP == "") {
            Msg = Msg + "[ClusterIP] 不能为空 || ";
        }
        if (Msg != "") {
            ErrorMsg(Msg);
            return;
        }

        var url = baseUrl + "/DBA/MainDB_SysConfig_Save";
        jsonData = { "BU": BU, "BUType": BUType, "ServerName": ServerName, "ServerType": ServerType, "ServerIP": ServerIP, "ClusterIP": ClusterIP, "ListenIP": ListenIP,   "Description": Description };
        ajaxJsonRequestDerived(url, 'post', jsonData, Show_Data_MainDB_SysConfig, AjaxFailResult);

    });
    $btnDelMainDB_SysConfig.click(function () {
        var selectRows = $tbrsMainDB_SysConfig.bootstrapTable('getSelections');


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
            var url = baseUrl + "/DBA/MainDB_SysConfig_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, MainDB_SysConfig_Delete, AjaxFailResult);
        }
    });

    function MainDB_SysConfig_Delete(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            $ModalDoing.modal('hide');

        } else {
            //移除选中的行
            var ids = $.map($tbrsMainDB_SysConfig.bootstrapTable('getSelections'), function (row) {
                return row.ServerIP;
            });
            $tbrsMainDB_SysConfig.bootstrapTable('remove', {
                field: 'ServerIP',
                values: ids
            });
            //$lblMessageError.html("");
            //$lblMessageSuccess.html("(" + ids.length + ")行资料已删除");
            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');
        }
    }

    $btnReseShowMainDB_SysConfig.click(function () {
        $txtMainDB_SysConfigBU.val("");
        $txtMainDB_SysConfigType.val("");
        $txtMainDB_SysConfigServerName.val("");
        $ddMainDB_SysConfigServerType.val("");
        $txtMainDB_SysConfigServerIP.val("");
        $txtMainDB_SysConfigClusterIP.val("");
        $txtMainDB_SysConfigListenIP.val("");
        $txtMainDB_SysConfigDescription.val("");
    });
    function Show_Data_MainDB_SysConfig(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbrsMainDB_SysConfig.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            OKMsg("一共(" + dataLen + ")行");
        }

    }
    $tbrsMainDB_SysConfig.bootstrapTable({
        editable: true, //开启编辑模式 
        clickToSelect: true,
        search: true,
        datatype: "json",
        toolbar: '#toolbarMainDB_SysConfig',//工具按钮用哪个容器
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
                field: 'BU',

                title: 'BU'
            }, {
                field: 'Type',
                title: 'Type',
 
            }, {
                field: 'ServerType',
                title: 'ServerType',


            }, {
                field: 'DBName',
                title: 'ServerName',
            }, {
                field: 'ServerIP',
                title: 'ServerIP',

            }, {
                field: 'ClusterIP',
                title: 'ClusterIP',
            }, {
                field: 'ListenIP',
                title: 'ListenIP',
            }, {
                field: 'Description',
                title: '描述',
            }
        , {
            field: 'Transdatetime',
            title: '日期',
        }],

        onClickRow: function (row, $element) {
            $txtMainDB_SysConfigBU.val(row.BU);
            $txtMainDB_SysConfigType.val(row.Type);
            $txtMainDB_SysConfigServerIP.val(row.ServerIP);
            $txtMainDB_SysConfigServerName.val(row.DBName);
            $ddMainDB_SysConfigServerType.val(row.ServerType);
            $txtMainDB_SysConfigClusterIP.val(row.ClusterIP);
            $txtMainDB_SysConfigListenIP.val(row.ListenIP);
            $txtMainDB_SysConfigDescription.val(row.Description);

           
            

        },
    });
    //MainDB_SysConfig


    //FileServer
    var $tbrsFileServer = $('#rsFileServer');

    var $btnReseFileServer = $('#ReseFileServer');
    var $btnCreateFileServer = $('#CreateFileServer');
    var $btnShowFileServer = $('#ShowFileServer');
    var $btnDelFileServer = $('#DelFileServer');

    var $txtFileServerClusterIP = $('#FileServerClusterIP');
    var $txtIP1 = $('#IP1');
    var $txtIP2 = $('#IP2');
    var $txtActualIP = $('#ActualIP');
    var $txtFileServerDesc = $('#FileServerDesc');

    $btnReseFileServer.click(function () {
        $txtFileServerClusterIP.val("");
        $txtIP1.val("");
        $txtIP2.val("");
        $txtActualIP.val("");
        $txtFileServerDesc.val("");
    });
    $btnShowFileServer.click(function () {
        var url = baseUrl + "/DBA/FileServer_View";
        ajaxJsonRequestDerived(url, 'post', "", Show_Data_FileServer, AjaxFailResult);
    });
 
    $btnDelFileServer.click(function () {
        var selectRows = $tbrsFileServer.bootstrapTable('getSelections');
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
            var url = baseUrl + "/DBA/FileServer_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, DelFileServer, AjaxFailResult);
        }
    });
    function DelFileServer(data)
    {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            $ModalDoing.modal('hide');
        } else {
            //移除选中的行
            var ids = $.map($tbrsFileServer.bootstrapTable('getSelections'), function (row) {
                return row.ClusterIP;
            });
            $tbrsFileServer.bootstrapTable('remove', {
                field: 'ClusterIP',
                values: ids
            });
            //$lblMessageError.html("");
            //$lblMessageSuccess.html("(" + ids.length + ")行资料已删除");
            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');
        }
    }
    $btnCreateFileServer.click(function () {
        var ClusterIP, IP1, IP2, ActualIP, Description;

        ClusterIP = $txtFileServerClusterIP.val().trim();
        IP1 = $txtIP1.val().trim();
        IP2 = $txtIP2.val().trim();
        ActualIP = $txtActualIP.val().trim();
        Description = $txtFileServerDesc.val().trim();

        var MSG = "";
        if (ClusterIP == "")
        {
            MSG = MSG + "[ClusterIP] 不能为空 || ";
        }
        if (IP1 == "") {
            MSG = MSG + "[IP1] 不能为空 || ";
        }
        if (IP2 == "") {
            MSG = MSG + "[IP2] 不能为空 || ";
        }
        if (ActualIP == "") {
            MSG = MSG + "[ActualIP] 不能为空 || ";
        }
        if (MSG != "")
        {
            ErrorMsg(MSG);
            return;
        }
        var url = baseUrl + "/DBA/FileServer_Save";
        jsonData = { "ClusterIP": ClusterIP, "IP1": IP1, "IP2": IP2, "ActualIP": ActualIP,   "Description": Description };
        ajaxJsonRequestDerived(url, 'post', jsonData, Show_Data_FileServer, AjaxFailResult);



    });


    function Show_Data_FileServer(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbrsFileServer.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            OKMsg("一共(" + dataLen + ")行");
        }

    }
    $tbrsFileServer.bootstrapTable({
        editable: true, //开启编辑模式 
        clickToSelect: true,
        search: true,
        datatype: "json",
        toolbar: '#toolbarFileServer',//工具按钮用哪个容器
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

            {  title: 'Item<br />',
                formatter: function (value, row, index)
                { return index + 1 }
            }, {
                field: 'ClusterIP',
                title: 'ClusterIP'
            }, {
                field: 'IP1',
                title: 'IP1',

            }, {
                field: 'IP2',
                title: 'IP2',
            }, {
                field: 'ActualIP',
                title: 'ActualIP',
            }, {
                field: 'ServerDesc',
                title: '描述',

            }, {
                field: 'UserID',
                title: '操作人',
            }, {
                field: 'TransDatetime',
                title: '日期',
            } ],

        onClickRow: function (row, $element) {
            $txtFileServerClusterIP.val(row.ClusterIP);
            $txtIP1.val(row.IP1);
            $txtIP2.val(row.IP2);
            $txtActualIP.val(row.ActualIP);
            $txtFileServerDesc.val(row.ServerDesc);
        },
    });
    //FileServer

    //DBShareFolderList

    var $tbrsDBShareFolderList = $('#rsDBShareFolderList');

    var $btnDelDBShareFolderList = $('#DelDBShareFolderList');
    var $btnShowDBShareFolderList = $('#ShowDBShareFolderList');
    var $btnCreateDBShareFolderList = $('#CreateDBShareFolderList');
    var $btnResetDBShareFolderList = $('#ResetDBShareFolderList');

    var $txtDBShareFolderListPU = $('#DBShareFolderListPU');
    var $txtDBShareFolderListType = $('#DBShareFolderListType');
    var $txtDBShareFolderListServerIP = $('#DBShareFolderListServerIP');
    var $txtBackUpServerIP = $('#BackUpServerIP');
    var $txtPath = $('#Path');
    var $txtUserName = $('#UserName');
    var $txtLimitedFileNum = $('#LimitedFileNum');
    var $txtLimitedFileSize = $('#LimitedFileSize');
    var $txtPassWord = $('#PassWord');

    $btnResetDBShareFolderList.click(function () {
        $txtDBShareFolderListPU.val("");
        $txtDBShareFolderListType.val("");
        $txtDBShareFolderListServerIP.val("");
        $txtBackUpServerIP.val("");
        $txtPath.val("");
        $txtUserName.val("");
        $txtPassWord.val("");
        $txtLimitedFileNum.val("");
        $txtLimitedFileSize.val("");
    });
    $btnShowDBShareFolderList.click(function () {
        var url = baseUrl + "/DBA/DBShareFolderList_View";
        ajaxJsonRequestDerived(url, 'post', "", Show_Data_DBShareFolderList, AjaxFailResult);
    });

    $btnDelDBShareFolderList.click(function () {
        var selectRows = $tbrsDBShareFolderList.bootstrapTable('getSelections');
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
            var url = baseUrl + "/DBA/DBShareFolderList_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, DBShareFolderList_Delete, AjaxFailResult);
        }
    });
    function DBShareFolderList_Delete(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            $ModalDoing.modal('hide');
        } else {
            //移除选中的行
            var ids = $.map($tbrsDBShareFolderList.bootstrapTable('getSelections'), function (row) {
                return row.ID;
            });
            $tbrsDBShareFolderList.bootstrapTable('remove', {
                field: 'ID',
                values: ids
            });
            //$lblMessageError.html("");
            //$lblMessageSuccess.html("(" + ids.length + ")行资料已删除");
            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');
        }
    }
    $btnCreateDBShareFolderList.click(function () {
        var PU,Type,ServerIP,Path,UserName	;
        var Password,LimitedFileNum,LimitedFileSize,BackUpServerIP;

        PU = $txtDBShareFolderListPU.val();
        Type = $txtDBShareFolderListType.val();
        ServerIP = $txtDBShareFolderListServerIP.val();
        BackUpServerIP = $txtBackUpServerIP.val();
        Path = $txtPath.val();
        UserName = $txtUserName.val();
        Password = $txtPassWord.val();
        LimitedFileNum = $txtLimitedFileNum.val();
        LimitedFileSize = $txtLimitedFileSize.val();

        var MSG = "";
        if (PU == "") {
            MSG = MSG + "[PU] 不能为空 || ";
        }
        if (Type == "") {
            MSG = MSG + "[Type] 不能为空 || ";
        }
        if (ServerIP == "") {
            MSG = MSG + "[ServerIP] 不能为空 || ";
        }
        if (BackUpServerIP == "") {
            MSG = MSG + "[BackUpServerIP] 不能为空 || ";
        }

        if (Path == "") {
            MSG = MSG + "[Path] 不能为空 || ";
        }
        if (UserName == "") {
            MSG = MSG + "[UserName] 不能为空 || ";
        }
        if (Password == "") {
            MSG = MSG + "[Password] 不能为空 || ";
        }
        if (LimitedFileNum == "") {
            MSG = MSG + "[LimitedFileNum] 不能为空 || ";
        }
        if (LimitedFileSize == "") {
            MSG = MSG + "[LimitedFileSize] 不能为空 || ";
        }

        if (checkNumberInt(LimitedFileNum) == false)
        {
            MSG = MSG + "[LimitedFileNum] 必须是整数 || ";
        }
        if (checkNumberInt(LimitedFileSize) == false) {
            MSG = MSG + "[LimitedFileSize] 必须是整数 || ";
        }

        if (MSG != "") {
            ErrorMsg(MSG);
            return;
        }
        var url = baseUrl + "/DBA/DBShareFolderList_Save";
        jsonData = { "PU": PU, "Type": Type, "ServerIP": ServerIP, "BackUpServerIP": BackUpServerIP, "Path": Path,
                    "UserName": UserName, "Password": Password, "LimitedFileNum": LimitedFileNum, "LimitedFileSize": LimitedFileSize 
                    };
        ajaxJsonRequestDerived(url, 'post', jsonData, Show_Data_DBShareFolderList, AjaxFailResult);

       

    });


    function Show_Data_DBShareFolderList(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbrsDBShareFolderList.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            OKMsg("一共(" + dataLen + ")行");
        }

    }
    $tbrsDBShareFolderList.bootstrapTable({
        editable: true, //开启编辑模式 
        clickToSelect: true,
        search: true,
        datatype: "json",
        toolbar: '#toolbarDBShareFolderList',//工具按钮用哪个容器
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
             },{
                title: 'Item<br />',
                formatter: function (value, row, index)
                { return index + 1 }
             } , {
                field: 'ID',
                title: 'ID',
                visible:false,
            }, {
                field: 'PU',
                title: 'PU'
            }, {
                field: 'Type',
                title: 'Type',

            }, {
                field: 'ServerIP',
                title: 'ServerIP',
            }, {
                field: 'BackUpServerIP',
                title: 'BackUpServerIP',
            }, {
                field: 'Path',
                title: 'Path',
            }, {
                field: 'UserName',
                title: 'UserName',

            }, {
                field: 'Password',
                title: 'Password',
            }, {
                field: 'LimitedFileNum',
                title: 'LimitedFileNum',
            }, {
                field: 'LimitedFileSize',
                title: 'LimitedFileSize',
            }],

        onClickRow: function (row, $element) {
            $txtDBShareFolderListPU.val(row.PU);
            $txtDBShareFolderListType.val(row.Type);
            $txtDBShareFolderListServerIP.val(row.ServerIP);
            $txtBackUpServerIP.val(row.BackUpServerIP);
            $txtPath.val(row.Path);
            $txtUserName.val(row.UserName);
            $txtLimitedFileNum.val(row.LimitedFileNum);
            $txtLimitedFileSize.val(row.LimitedFileSize);
            $txtPassWord.val(row.Password);
        },
    });
    //DBShareFolderList


    //DBServer_LIST
    var $txtDBServer_LISTBU = $('#DBServer_LISTBU');
    var $txtDBServer_LISTCustomer = $('#DBServer_LISTCustomer');
    var $txtDBServer_LISTServerName = $('#DBServer_LISTServerName');
    var $ddDBServer_LISTTYPE = $('#DBServer_LISTTYPE');

    var $txtDBServer_LISTIP = $('#DBServer_LISTIP');
    var $txtDBServer_LISTPWD = $('#DBServer_LISTPWD');
    var $txtDBServer_LISTDBName = $('#DBServer_LISTDBName');
    var $ddIS_MAIN_DB = $('#IS_MAIN_DB');
    var $txtMirrorDBIP = $('#MirrorDBIP');
    var $txtOwner_Name = $('#Owner_Name');
    var $ddIS_QFMS_DB = $('#IS_QFMS_DB');
    var $ddIS_AlwaysOn_DB = $('#IS_AlwaysOn_DB');
    var $ddIsNeed_AdjustWOQty = $('#IsNeed_AdjustWOQty');
    var $ddIsNeed_sDayOfYear = $('#IsNeed_sDayOfYear');

    var $btnResetDBServer_LIST = $('#ResetDBServer_LIST');
    var $btnCreateDBServer_LIST = $('#CreateDBServer_LIST');
    var $btnShowDBServer_LIST = $('#ShowDBServer_LIST');
    var $btnDelDBServer_LIST = $('#DelDBServer_LIST');

    var $tbrsDBServer_LIST = $('#rsDBServer_LIST');

 
    $btnResetDBServer_LIST.click(function () {
        $ddDBServer_LISTTYPE.val("");
        $txtDBServer_LISTServerName.val("");
        $txtDBServer_LISTCustomer.val("");
        $txtDBServer_LISTBU.val("");
        $txtDBServer_LISTIP.val("");
        $txtDBServer_LISTPWD.val("");
        $txtDBServer_LISTDBName.val("");
        $ddIS_MAIN_DB.val("");
        $txtMirrorDBIP.val("");
        $txtOwner_Name.val("");
        $ddIS_QFMS_DB.val("");
        $ddIS_AlwaysOn_DB.val("");
        $ddIsNeed_AdjustWOQty.val("");
        $ddIsNeed_sDayOfYear.val("");
    });
    $btnShowDBServer_LIST.click(function () {
        var url = baseUrl + "/DBA/DBServer_List_View";
        ajaxJsonRequestDerived(url, 'post', "", Show_Data_DBServer_List, AjaxFailResult);
    });

    $btnDelDBServer_LIST.click(function () {
        var selectRows = $tbrsDBServer_LIST.bootstrapTable('getSelections');
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
            var url = baseUrl + "/DBA/DBServer_List_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, DBServer_List_Delete, AjaxFailResult);
        }
    });
    function DBServer_List_Delete(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
            $ModalDoing.modal('hide');
        } else {
     
            var ids = $.map($tbrsDBServer_LIST.bootstrapTable('getSelections'), function (row) {
                return row.ID;
            });
            $tbrsDBServer_LIST.bootstrapTable('remove', {
                field: 'ID',
                values: ids
            });
           
            OKMsg("(" + ids.length + ")行资料已删除");
            $ModalDoing.modal('hide');
        }
    }
    $btnCreateDBServer_LIST.click(function () {
        var BU,TYPE,IP,ServerName,DBName,PWD,Customer,IS_MAIN_DB;
        var IS_QFMS_DB,	MirrorDBIP,IS_AlwaysOn_DB,FileShare_Witness;
        var IsNeed_AdjustWOQty, IsNeed_sDayOfYear, Owner_Name;
 
        TYPE = $ddDBServer_LISTTYPE.val();
        ServerName = $txtDBServer_LISTServerName.val();
        Customer = $txtDBServer_LISTCustomer.val();
        BU = $txtDBServer_LISTBU.val();
        IP = $txtDBServer_LISTIP.val();
        PWD= $txtDBServer_LISTPWD.val();
        DBName = $txtDBServer_LISTDBName.val();
        IS_MAIN_DB = $ddIS_MAIN_DB.val();
        MirrorDBIP = $txtMirrorDBIP.val();
        Owner_Name = $txtOwner_Name.val();
        IS_QFMS_DB= $ddIS_QFMS_DB.val();
        IS_AlwaysOn_DB= $ddIS_AlwaysOn_DB.val();
        IsNeed_AdjustWOQty = $ddIsNeed_AdjustWOQty.val();
        IsNeed_sDayOfYear = $ddIsNeed_sDayOfYear.val();

        var MSG = "";
        if (TYPE == "") {
            MSG = MSG + "[TYPE] 不能为空 || ";
        }

        if (ServerName == "") {
            MSG = MSG + "[ServerName] 不能为空 || ";
        }

        if (Customer == "") {
            MSG = MSG + "[Customer] 不能为空 || ";
        }

        if (BU == "") {
            MSG = MSG + "[BU] 不能为空 || ";
        }

        if (IP == "") {
            MSG = MSG + "[IP] 不能为空 || ";
        }

        if (PWD == "") {
            MSG = MSG + "[PWD] 不能为空 || ";
        }

        if (DBName == "") {
            MSG = MSG + "[DBName] 不能为空 || ";
        }

        if (MirrorDBIP == "") {
            MSG = MSG + "[MirrorDBIP] 不能为空 || ";
        }

        if (Owner_Name == "") {
            MSG = MSG + "[Owner_Name] 不能为空 || ";
        } 

        if (MSG != "") {
            ErrorMsg(MSG);
            return;
        }
        var url = baseUrl + "/DBA/DBServer_List_Save";
         									 						

        jsonData = {
            "BU": BU, "TYPE": TYPE, "IP": IP, "ServerName": ServerName, "DBName": DBName,
            "PWD": PWD, "Customer": Customer, "IS_MAIN_DB": IS_MAIN_DB, "IS_QFMS_DB": IS_QFMS_DB,

            "MirrorDBIP": MirrorDBIP, "IS_AlwaysOn_DB": IS_AlwaysOn_DB, "IsNeed_AdjustWOQty": IsNeed_AdjustWOQty, "IsNeed_sDayOfYear": IsNeed_sDayOfYear, "Owner_Name": Owner_Name 
 
        };
        ajaxJsonRequestDerived(url, 'post', jsonData, Show_Data_DBServer_List, AjaxFailResult);



    });


    function Show_Data_DBServer_List(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbrsDBServer_LIST.bootstrapTable('load', data.tableData);
            var dataLen = data.tableData.length;
            OKMsg("一共(" + dataLen + ")行");
        }

    }
    $tbrsDBServer_LIST.bootstrapTable({
        editable: true, //开启编辑模式 
        clickToSelect: true,
        search: true,
        datatype: "json",
        toolbar: '#toolbarDBServer_LIST',//工具按钮用哪个容器
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
             }, {
                 title: 'Item<br />',
                 formatter: function (value, row, index)
                 { return index + 1 }
             }, {
                 field: 'ID',
                 title: 'ID',
                 visible: false,
             }, {
                 field: 'BU',
                 title: 'BU'
             }, {
                 field: 'Customer',
                 title: 'Customer',
             }, {
                 field: 'ServerName',
                 title: 'ServerName',
             }, {
                 field: 'TYPE',
                 title: 'TYPE',

             }, {
                 field: 'IP',
                 title: '主服务器IP',
             }, {
                 field: 'PWD',
                 title: 'DB密码',

             }, {
                 field: 'DBName',
                 title: 'DBName',
             }, {
                 field: 'IS_AlwaysOn_DB',
                 title: '加入可用性组',
             }, {
                 field: 'MirrorDBIP',
                 title: '备份服务器IP',
             }, {
                 field: 'Owner_Name',
                 title: '负责人',
             }, {
                 field: 'IS_QFMS_DB',
                 title: '是否是QFMS:',
             }, {
                 field: 'IS_MAIN_DB',
                 title: '是否是主DB',
             } , {
                 field: 'IsNeed_AdjustWOQty',
                 title: '是否自动砍单',
             }, {
                 field: 'IsNeed_sDayOfYear',
                 title: '是否有周别表',
             }

        ],

        onClickRow: function (row, $element) {
            $ddDBServer_LISTTYPE.val(row.TYPE);
            $txtDBServer_LISTServerName.val(row.ServerName);
            $txtDBServer_LISTCustomer.val(row.Customer);
            $txtDBServer_LISTBU.val(row.BU);
            $txtDBServer_LISTIP.val(row.IP);
            $txtDBServer_LISTPWD.val(row.PWD);
            $txtDBServer_LISTDBName.val(row.DBName);
            $ddIS_MAIN_DB.val(row.IS_MAIN_DB);
            $txtMirrorDBIP.val(row.MirrorDBIP);
            $txtOwner_Name.val(row.Owner_Name);
            $ddIS_QFMS_DB.val(row.IS_QFMS_DB);
            $ddIS_AlwaysOn_DB.val(row.IS_AlwaysOn_DB);
            $ddIsNeed_AdjustWOQty.val(row.IsNeed_AdjustWOQty);
            $ddIsNeed_sDayOfYear.val(row.IsNeed_sDayOfYear);
        },
    });
    //DBServer_LIST
})