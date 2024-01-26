

$(document).ready(function () {
    var $tbResultTable = $('#ResultTable');
    
    var $btnSave = $('#save');
    var $btnDelete = $('#delete');
    var $btnRest = $('#rest');
    var $btnToExcel = $('#ToExcel');
     
    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
    var $prog_in = $('#prog_in');
    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');
    var $ModalDoing = $('#doing');
    

    var $ddServerIP = $('#ServerIP');
    var $ddType = $('#Type');
    var $ddMailGroup = $('#MailGroup');
    var $txtJOBName = $('#Jobname');
    var $txtMax_allowed_Duration = $('#Max_allowed_Duration');
    var $txtOwner = $('#Owner');


    $ddServerIP.change(function () {
        var ServerIP, Jobname;

        Jobname = "";
        ServerIP = $ddServerIP.val();
        if (ServerIP != "")
        {
            var url = baseUrl + "/DBA/QMS_DefineCheckSQLjobs_Detail_View";

            jsonData = { "ServerIP": ServerIP, "Jobname": Jobname }
            ajaxJsonRequestDerived(url, 'post', jsonData, ShowData, AjaxFailResult);
        }
    })
    
    $btnRest.click(function () {
        $ddServerIP.val("");
        $ddType.val("");
        $ddMailGroup.val("");
        $txtJOBName.val("");
        $txtMax_allowed_Duration.val("");
        $txtOwner.val("");
    })
     

    $btnSave.click(function () {
        var ServerIP;
        var Type;
        var MailGroup;
        var JOBName;
        var Max_allowed_Duration;
        var Owner;
        var Msg;

        ServerIP = $ddServerIP.val();
        Type = $ddType.val();
        MailGroup = $ddMailGroup.val();
        JOBName = $txtJOBName.val();
        Max_allowed_Duration = $txtMax_allowed_Duration.val();
        Owner = $txtOwner.val();


        Msg = "";
        if (ServerIP == "") {
            Msg = Msg + "[ServerIP] 不能为空 || ";
        }
        if (Type == "") {
            Msg = Msg + "[Type] 不能为空 || ";
        }
        if (MailGroup == "") {
            Msg = Msg + "[MailGroup] 不能为空 || ";
        }
        if (JOBName == "") {
            Msg = Msg + "[JOBName] 不能为空 || ";
        }
        if (Max_allowed_Duration == "") {
            Msg = Msg + "[Max_allowed_Duration] 不能为空 || ";
        }

        if (checkNumberInt(Max_allowed_Duration) == false)
        {
            Msg = Msg + "[执行时间] 必须是整数 || ";
        }
        if (Owner == "") {
            Msg = Msg + "[Owner] 不能为空 || ";
        }
        if (Msg != "") {
            ErrorMsg(Msg);
            return;
        }

        DisableBtns();
        var url = baseUrl + "/DBA/QMS_DefineCheckSQLjobs_Detail_Save";
       
        					
        jsonData = { "ServerIP": ServerIP, "Type": Type, "MailGroup": MailGroup, "Owner": Owner, "Jobname": JOBName, "Max_allowed_Duration": Max_allowed_Duration }
        ajaxJsonRequestDerived(url, 'post', jsonData, ShowData, AjaxFailResult);

    });

    function ShowData(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);
        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            OKMsg("一共(" + data.tableData.length + ")行资料");
        }
        EnableBtns();
    }


    function DisableBtns() {
        $btnSave.attr("disabled", "disabled");
        $btnDelete.attr("disabled", "disabled");
        $btnToExcel.attr("disabled", "disabled");
 
    }
    function EnableBtns() {
        $btnSave.val("添加/更新");
        $btnSave.attr("disabled", false);


        $btnDelete.val("删除");
        $btnDelete.attr("disabled", false);


        $btnToExcel.val("导出数据Excel");
        $btnToExcel.attr("disabled", false);

  



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

    $btnToExcel.click(function () {

        $tbResultTable.tableExport({
            type: 'excel',
            escape: 'false',
            fileName: 'QMS_DefineCheckSQLjobs',
            tableName: 'QMS_DefineCheckSQLjobs',
            ignoreColumn: [0],
            worksheetName: ['QMS_DefineCheckSQLjobs']
        })
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
            var url = baseUrl + "/DBA/QMS_DefineCheckSQLjobs_Detail_Delete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, QMS_DefineCheckSQLjobs_Detail_Delete, AjaxFailResult);
        }



    });
    function QMS_DefineCheckSQLjobs_Detail_Delete(data) {
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
                field: 'ServerIP',
 
                title: 'ServerIP'
            }, {
                field: 'Type',
                title: 'Type'
            }, {
                field: 'MailGroup',
                title: 'MailGroup',
                
            }, {
                field: 'Jobname',
                title: 'Jobname',
          

            }, {
                field: 'Max_allowed_Duration',
                title: '执行时间',
            }, {
                field: 'Owner',
                title: 'Owner',
            } ],

        onClickRow: function (row, $element) {
            $ddServerIP.val(row.ServerIP);
            $ddType.val(row.Type);
            $ddMailGroup.val(row.MailGroup);
            $txtJOBName.val(row.Jobname);
            $txtMax_allowed_Duration.val(row.Max_allowed_Duration);
            $txtOwner.val(row.Owner);
 




        },
    });
})