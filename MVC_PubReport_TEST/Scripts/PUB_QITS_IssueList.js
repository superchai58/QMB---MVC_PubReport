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

    var $ddModel = $('#Model');
    var $ddIssueCategory = $('#IssueCategory')
    var $ddIssueType = $('#IssueType');
    var $ddStatus = $('#Status');
    var $ddISHoldSN = $('#ISHoldSN');

    var $txtHoldSNQty = $('#HoldSNQty');
    var $txtLine = $('#Line');

    var $txtIssueDate = $('#IssueDate');
    var $txtScheduleDate = $('#ScheduleDate');
    var $txtResponsibilityCategory = $('#ResponsibilityCategory');
    var $txtYield = $('#Yield');
    var $txtLineOwner = $('#LineOwner');
    var $txtPEOwner = $('#PEOwner');
    var $txtQMOwner = $('#QMOwner');
    var $txtIssueDate = $('#IssueDate');
    var $txtScheduleDate = $('#ScheduleDate');
    var $txtRootCasuse = $('#RootCasuse');
    var $txtActionDesc = $('#ActionDesc');

    var IssueNO = "";
        
    //格式化下拉框

    $(function () {
        $('select').comboSelect();
    });

    //绑定日历
    var $IssueDateCalendar = $('#ddIssueDate');
    $IssueDateCalendar.calendar({
        trigger: '#IssueDate',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected');

        },
        onClose: function (view, date, data) {
            var Date;
            Date = $txtIssueDate.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            }

        }
    });


    var $ScheduleDateCalendar = $('#ddScheduleDate');
    $ScheduleDateCalendar.calendar({
        trigger: '#ScheduleDate',
        zIndex: 999,
        format: 'yyyymmdd',
        onSelected: function (view, date, data) {
            console.log('event: onSelected');

        },
        onClose: function (view, date, data) {
            var Date;
            Date = $txtScheduleDate.val().trim();
            if (Date == "") {
                ErrorMsg("请选择日期");
                return;
            }

        }
    });

 
    //if (UserRole == "0" || UserRole == "1") //若是助理
    var url = baseUrl + "/QA/QITS_IssueListGetDataByRole";

   

    ajaxJsonRequestDerived(url, 'post', "", Show_Data, AjaxFailResult);




    $ddModel.change(function () {
        var Model = $ddModel.val();
        $txtQMOwner.val("");
        if (Model == "")
        {
            ErrorMsg("请选择Model");
            return;
        }
        var url = baseUrl + "/QA/QITS_IssueListGetDataByModel";
         
        var jsondata = { "Model": Model };

        ajaxJsonRequestDerived(url, 'post', jsondata, Show_Data, AjaxFailResult);
        
    })

    function Show_Data(data) {
        if (data.result != "OK") {
            ErrorMsg(data.result);

        } else {
            $tbResultTable.bootstrapTable('load', data.tableData);
            $txtQMOwner.val( data.QMOwer);
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
        IssueNO = "";
        $ddModel.val("");
        $ddIssueCategory.val("");
        $ddIssueType.val("");
        $ddStatus.val("");
        $ddISHoldSN.val("");
        $txtHoldSNQty.val("");
        $txtLine.val("");
        $txtResponsibilityCategory.val("");
        $txtYield.val("");
        $txtLineOwner.val("");
        $txtPEOwner.val("");
        $txtQMOwner.val("");
        $txtIssueDate.val("");
        $txtScheduleDate.val("");

        $lblMessageErrorModal.html("");
        $lblMessageSuccessModal.html("");

        $('select').comboSelect();
    }
    $btnClose.click(function () {
        ResetTxt();
        $ModalAddInfo.modal('hide');
    });
    $btnSave.click(function () {

        var  PU, Type, Model, Line, IssueCategory;
        var IssueType, Yield, RootCasuse, ActionDesc;
        var ResponsibilityCategory, ISHoldSN, HoldSNQty;
        var IssueDate, ScheduleDate, LineOwner, QMOwner;
        var PEOwner, Status, UserID, TransDateTime, CreateDateTime;
        var HoldSNQty1

        Model = $ddModel.val();
        Line = $txtLine.val();
        IssueCategory = $ddIssueCategory.val();
        IssueType = $ddIssueType.val();
        Status = $ddStatus.val( );
        ISHoldSN = $ddISHoldSN.val();
        HoldSNQty = $txtHoldSNQty.val();
        Line = $txtLine.val();
        ResponsibilityCategory = $txtResponsibilityCategory.val();
        Yield= $txtYield.val();
        LineOwner= $txtLineOwner.val();
        PEOwner = $txtPEOwner.val();
        QMOwner = $txtQMOwner.val();
        IssueDate = $txtIssueDate.val();
        ScheduleDate = $txtScheduleDate.val();
        RootCasuse = $txtRootCasuse.val();
        ActionDesc = $txtActionDesc.val();

        if (HoldSNQty == "") {
            HoldSNQty1 = 0;
        }
        else {
            if (checkNumberInt(HoldSNQty) == false) {
                ErrorMsgModal("Hold 机器数量 不是数字");
                return;
            }
            HoldSNQty1 = parseInt(HoldSNQty)
        }
       

        if (IssueType == "") {
            ErrorMsgModal("IssueType 不能为空");
            return;
        }


        var url = baseUrl + "/QA/QITS_IssueListSave";
        var jasondata = {
            "Model": Model, "Line": Line, "IssueCategory": IssueCategory, "IssueType": IssueType, "Status": Status,
            "ISHoldSN": ISHoldSN, "HoldSNQty": HoldSNQty1, "Line": Line, "ResponsibilityCategory": ResponsibilityCategory,
            "Yield": Yield, "LineOwner": LineOwner, "PEOwner": PEOwner, "QMOwner": QMOwner, "IssueDate": IssueDate,
            "ScheduleDate": ScheduleDate, "RootCasuse": RootCasuse, "ActionDesc": ActionDesc, "IssueNO": IssueNO
        }
        //alert(IssueNO);

        var sendJasonData = { "jsonData": "[" + JSON.stringify(jasondata) + "]" };
        ajaxJsonRequestDerived(url, 'post', sendJasonData, Show_DataModal, AjaxFailResult);

    });

    function Show_DataModal(data) {

       
        if (data.result != "OK") {
            ErrorMsgModal(data.result);
            //ErrorMsg(data.result);

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
            var url = baseUrl + "/QA/QITS_IssueListDelete";
            jsonData = { "jsonData": JSON.stringify(selectRows) };
            ajaxJsonRequestDerived(url, 'post', jsonData, DeleteData, AjaxFailResult);
        }
    });
    function DeleteData(data) {
        if (data.result != "OK") {

            ErrorMsg(data.result);
            $ModalDoing.modal('hide');
        } else {
            //移除选中的行
            var ids = $.map($tbResultTable.bootstrapTable('getSelections'), function (row) {
                return row.IssueNO;
            });
            $tbResultTable.bootstrapTable('remove', {
                field: 'IssueNO',
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
            fileName: 'QITS_IssueList',
            tableName: 'QITS_IssueList',
            ignoreColumn: [0],
            worksheetName: ['QITS_IssueList']
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
            else {

                EnableBtns();
            }
        }
        else {

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
                field: 'IssueNO',
                align: 'center',
                title: 'IssueNO',
                visible: true,
            }, {
                field: 'PU',
                title: 'PU',

            }, {
                field: 'Type',
                title: 'Type',


            }, {
                field: 'Model',
                title: '广达机种',
                formatter: function (value, row, index) {
                    var aHtml = baseUrl + "/QA/QITS_ModelSetting?Flag=ModelSetting_ListView";
                    var strHtml = "<a href='" + aHtml + "&Model=" + row.Model + "'   style='margin-left:5px; color:#003399' target='_blank'>" + row.Model + "</li></a> "
                    return strHtml;

                }

            }, {
                field: 'Line',
                title: '线别',


            }, {
                field: 'IssueCategory',
                title: 'IssueCategory',


            }, {
                field: 'IssueType',
                title: 'IssueType',


            }, {
                field: 'Yield',
                title: '不良率',


            }, {
                field: 'RootCasuse',
                title: 'RootCasuse',


            }, {
                field: 'ActionDesc',
                title: 'ActionDesc',


            }, {
                field: 'ResponsibilityCategory',
                title: '责任类别',


            }, {
                field: 'ISHoldSN',
                title: '是否Hold货',


            }, {
                field: 'HoldSNQty',
                title: 'Hold货数量',


            }, {
                field: 'IssueDate',
                title: 'Issue发生时间',


            }, {
                field: 'ScheduleDate',
                title: '预计完成时间',


            }
            , {
                field: 'LineOwner',
                title: '线别组长',


            }
             , {
                 field: 'QMOwner',
                 title: 'QM负责人',
                 formatter: function (value, row, index) {
                     var aHtml = baseUrl + "/QA/User_List?Flag=User_ListView";
                     var strHtml = "<a href='" + aHtml + "&UserID=" + row.QMOwner + "'   style='margin-left:5px; color:#003399' target='_blank'>" + row.QMOwner + "</li></a> "
                     return strHtml;

                 }


             }
              , {
                  field: 'PEOwner',
                  title: 'PE负责人',


              }
               , {
                   field: 'Status',
                   title: '状态',


               }, {
                field: 'UserID',
                title: '操作人',


            },

        ],
        onDblClickRow: onDblClickRow

    });
    function onDblClickRow(selectRow) {
        //$txtIssueType.attr("disabled", "disabled");

        //$txtIssueType.val(selectRow.IssueType);

        //alert(selectRow.IssueNO);

        IssueNO = selectRow.IssueNO;
        $ddModel.val(selectRow.Model);
          $txtLine.val(selectRow.Line);
         $ddIssueCategory.val(selectRow.IssueCategory);
         $ddIssueType.val(selectRow.IssueType);
        $ddStatus.val(selectRow.Status);
        $ddISHoldSN.val(selectRow.ISHoldSN);
        $txtHoldSNQty.val(selectRow.HoldSNQty);
        $txtLine.val(selectRow.Line);
        $txtResponsibilityCategory.val(selectRow.ResponsibilityCategory);
         $txtYield.val(selectRow.Yield);
         $txtLineOwner.val(selectRow.LineOwner);
         $txtPEOwner.val(selectRow.PEOwner);
        $txtQMOwner.val(selectRow.QMOwer);
        $txtIssueDate.val(selectRow.IssueDate);
         $txtScheduleDate.val(selectRow.ScheduleDate);
         $txtRootCasuse.val(selectRow.RootCasuse);
         $txtActionDesc.val(selectRow.ActionDesc);

         $('select').comboSelect();
        ShowModal(2);
    }

    function ShowModal(type) //type = 1 表示新增，2表示编辑
    {


        if (type == 1) {
            //$txtIssueType.attr("disabled", false);
        } else {
            //$txtIssueType.attr("disabled", "disabled");
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


