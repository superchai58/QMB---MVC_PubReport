$(document).ready(function () {
    var $btnChangePW = $('#ChangePW');
    var $btnSavePassWord = $('#SavePassWord');
    var $btnRegister = $('#register');

    var $txtPU = $('#PU');
    var $txtType = $('#Type');
    var $txtUserID = $('#UserID');
    var $txtNewPassWord = $('#NewPassWord');
    var $txtNewPassWordVerify = $('#NewPassWordVerify');
    var $txtPassWord = $('#PassWord');
    var $txtPassWordVerify = $('#PassWordVerify');
     
    var $ddPU = $('#PU');
    var $ddType = $('#Type');

    var $frmPassWordChange = $('#PassWordChange');
    var $frmUserInfo = $('#UserInfo');
    var $ModalDoing = $('#doing');

    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success');
 


    var $spanTimer = $('#Timer');
    var Timer;  //用于 3秒后跳转到主页的计时器


    //20180416


    //20180416 end  合并代码
    



    $btnChangePW.click(function () {

        $txtNewPassWord.val("");
        $txtNewPassWordVerify.val("");
        OKMsg("");
        $frmPassWordChange.modal('show').css({
            width: 'auto',
            'margin-left': function () {
                return ($frmUserInfo.width() / 10);
            },
            'margin-top': function () {
                return ($frmUserInfo.height() / 2);
            },

        });
    
    });


    $btnSavePassWord.click(function () {
        SavePassWord();
    });

    $txtNewPassWordVerify.blur(function () {
        PassWordMatch();
    });
    $txtNewPassWordVerify.keyup(function (event) {
        if (event.which != 13)
        {
            PassWordMatch();
        }
           
    });
    $txtNewPassWordVerify.keypress(function (e) {
        if (e.which == 13)
        {
            SavePassWord();
        }
        
    });



    $txtPassWordVerify.blur(function () {
        PassWordMatchRegister();
    });
    $txtPassWordVerify.keyup(function (event) {
        if (event.which != 13) {
            PassWordMatchRegister();
        }

    });
    $txtPassWordVerify.keypress(function (e) {
        if (e.which == 13) {
            RegisterUser();
        }

    });


    $txtUserID.blur(function () {

        var PU, Type,UserId;

        PU = $ddPU.val();
        Type = $ddType.val();
        UserId = $txtUserID.val();

        if (PU == "")
        {
            ErrMsg("请选择PU");
            return;
        }
        if (Type == "") {
            ErrMsg("请选择Type");
            return;
        }
        if (UserId == "") {
            ErrMsg("工号不能为空");
            return;
        }
        ChkUserID(UserId);
         
    });
    $btnRegister.click(function () {
        RegisterUser();
    });
    function RegisterUser()
    {
        var PU, Type, UserId,PassWord;

        PU = $ddPU.val();
        Type = $ddType.val();
        UserId = $txtUserID.val();
        PassWord = $txtPassWord.val();

        if (PU == "") {
            ErrMsg("请选择PU");
            return;
        }
        if (Type == "") {
            ErrMsg("请选择Type");
            return;
        }
        if (UserId == "") {
            ErrMsg("工号不能为空");
            return;
        }

        var url = baseUrl + "/User/SaveUser";


        jsonData = { "UserID": UserId, "Type": Type, "PU": PU, "PassWord": PassWord };
        ajaxJsonRequestDerived(url, 'post', jsonData, RegisterUserSuccess, AjaxFailResult);
    }
    function RegisterUserSuccess(data)
    {
        if (data.result != "OK") {
            ErrMsg(data.result);
            return;
        } else {
            OKMsg("注册成功,3秒后自动跳转到主页");
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
            return;
        }
    }
    Timer = 3
    function go()
    {
        Timer = Timer - 1;
        if (Timer > 0) {
            $spanTimer.html(Timer);
        }
        else {
            var url = baseUrl ;
            
            window.location.href = url;
        }
    }
    function ChkUserID(UserId)
    {
        var url = baseUrl + "/User/CheckUser";
         
        
        jsonData = { "UserID": UserId };
        ajaxJsonRequestDerived(url, 'post', jsonData, ChkUserIDSuccess, AjaxFailResult);
    }
    function ChkUserIDSuccess(data)
    {
        if (data.result != "OK") {
            ErrMsg(data.result);
            return;
        } else {
            ErrMsg("");
            return;
        }


    }
    function SavePassWord()
    {
        if (PassWordMatch() == false) {
            $btnSavePassWord.attr("disabled", "disabled");
        } else {
            var url = baseUrl + "/User/ChangePW";
            var UserId = $txtUserID.val();
            var PassWord = $txtNewPassWord.val();
            jsonData = { "UserID": UserId, "PassWord": PassWord };
            ajaxJsonRequestDerived(url, 'post', jsonData, ChangePWSuccess, AjaxFailResult);
           
        }
    }
    function ChangePWSuccess(data)
    {
        if (data.result != "") {
            ErrMsg(data.result);
        } else {
            OKMsg("密码修改成功！");
            $frmPassWordChange.modal('hide')
        }
    }
    function  PassWordMatch()
    {
 
        var NewPassWord; var NewPassWordVerify;

        NewPassWord = $txtNewPassWord.val();
        NewPassWordVerify = $txtNewPassWordVerify.val();
        
        if (NewPassWord == "")
        {
            ErrMsg("新密码不能为空");
            $btnSavePassWord.attr("disabled", "disabled");
            
            return false;
        }
 
        if (NewPassWord != NewPassWordVerify) {
            ErrMsg("两次输入的密码不一致");
            $btnSavePassWord.attr("disabled", "disabled");
            
            return false;
        }
        ErrMsg("");
        $btnSavePassWord.attr("disabled", false);
       
        return true;
    }


    function PassWordMatchRegister() {

        var PassWord; var PassWordVerify;

        PassWord = $txtPassWord.val();
        PassWordVerify = $txtPassWordVerify.val();

        if (PassWord == "") {
            ErrMsg("新密码不能为空");
            $btnRegister.attr("disabled", "disabled");

            return false;
        }

        if (PassWord != PassWordVerify) {
            ErrMsg("两次输入的密码不一致");
            $btnRegister.attr("disabled", "disabled");

            return false;
        }
        ErrMsg("");
        $btnRegister.attr("disabled", false);

        return true;
    }


    function ErrMsg(msg)
    {
        $lblMessageError.html(msg);
        $lblMessageSuccess.html("");
    }
    function OKMsg(msg)
    {
        $lblMessageError.html("");
        $lblMessageSuccess.html(msg)
    }

});
 