

$(document).ready(function () {
  
    
    var $txtParentID = $("#ParentID");
    var $txtParentName = $("#ParentName");
    var $txtMenuName = $("#MenuName");
    var $txtLinkPage = $("#LinkPage");
    var $ddIsPublic = $("#IsPublic");

    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');

    var $btnSave = $("#Save");
    var $btnDelete = $("#Delete");
    //绑定菜单栏的每个链接，以便点击显示他们的资料
    var oli = $(".addMenu");
    for (var i = 0; i < oli.length; i++) {
        oli[i].onclick = function () {
           
            var ParentID;
            var ParentMenuName;

            ParentID = $(this).attr("id")            
            ParentMenuName = $(this).html();

            $txtParentName.val(ParentMenuName);
            $txtParentID.val(ParentID);

            $btnSave.attr("disabled", false);
            $btnDelete.attr("disabled", false);

             
        };
    }

    $btnDelete.click(function () {
        var ParentID;
        var Msg;

        ParentID = $txtParentID.val();       
        Msg = "";
        if (ParentID == "") {
            Msg = "[父节点]不能为空 ||";
        }
        if (ParentID == 1) {
            Msg = Msg + "该节点不能删";
        }

        if (Msg != "") {
            ErrorMsg(Msg);
            return;
        }

        var url = baseUrl + "/User/FunctionListDelete";
        jsonData = { "ParentID": ParentID };
        ajaxJsonRequestDerived(url, 'post', jsonData, FunctionListDelete, AjaxFailResult);
    });

    function FunctionListDelete(data)
    {
        if (data.result != "") {
            ErrorMsg(data.result);
        } else {
            document.location.reload();
            OKMsg("恭喜，删除成功");
        }
    }

    $btnSave.click(function () {
        var ParentID;
        var ParentMenuName;
        var MenuName;
        var LinkPage;
        var IsPublic;
        var Msg;

        ParentID = $txtParentID.val().trim();
        ParentMenuName = $txtParentName.val().trim();
        MenuName = $txtMenuName.val().trim();
        LinkPage = $txtLinkPage.val().trim();
        IsPublic = $ddIsPublic.val();

        Msg = "";
        if (ParentID == "") {
            Msg = Msg+"[父节点]不能为空 || ";
        }
        if (MenuName == "")
        {
            Msg = Msg + "[节点]不能为空 || ";
        }

        if (IsPublic == "")
        {
            Msg = Msg + "请选择[是否公开] || ";
        }

        if (MenuName.length > 20)
        {
            Msg = Msg + "[节点]长度不能大于20 || ";
        }
        if (LinkPage.length > 50) {
            Msg = Msg + "[链接页面]长度不能大于50 || ";
        }
        if (ParentMenuName.substr(ParentMenuName.length-1, 1) == 3)
        {
            Msg = Msg + "第三层叶子节点不能再增加子节点 || ";
        }

        if (Msg != "")
        {
            ErrorMsg(Msg);
            return;
        }

        var url = baseUrl + "/User/FunctionListSave";
        jsonData = {"ParentID":ParentID,"ParentMenuName":ParentMenuName,"MenuName":MenuName,"LinkPage": LinkPage, "IsPublic":IsPublic};
        ajaxJsonRequestDerived(url, 'post', jsonData, FunctionListSave, AjaxFailResult);

    });
    function FunctionListSave(data)
    {
        if (data.result != "") {
            ErrorMsg(data.result);
        } else {
            document.location.reload();
            OKMsg("恭喜，添加成功");
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
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href=' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350  > 请重新登录 </a>')


                tb_init('a.thickbox, area.thickbox, input.thickbox');
                imgLoader = new Image();// preload image
                imgLoader.src = tb_pathToImage;
            }

        }
        $lblMessageError.html(msg);
        $lblMessageSuccess.html("");
    }
})