
var jsonData;
var opType = 2;



//获取前N天或者后N天的日期

//window.alert("今天是：" + showdate(0));
//window.alert("昨天是：" + showdate(-1));
//window.alert("明天是：" + showdate(1));
//window.alert("10天前是：" + showdate(-10));
//window.alert("5天后是：" + showdate(5));
function showdate(n) {
    var uom = new Date();
    var month;
    var day;

    uom.setDate(uom.getDate() + n);
    month = (uom.getMonth() + 1);
    day = uom.getDate();
    if (month < 10)
    {
        month = "0" + month;
    }
    if (day < 10) {
        day = "0" + day;
    }

    //uom = uom.getFullYear() + "-" + month + "-" + day;
    uom = uom.getFullYear() +""  + month +""  + day;
 
    return uom;
}

//两个时间相差天数 兼容firefox chrome
function datedifference(sDate1, sDate2) {    //sDate1和sDate2是2006-12-18格式  
    var dateSpan,
        tempDate,
        iDays;
    sDate1 = Date.parse(sDate1);
    sDate2 = Date.parse(sDate2);
    dateSpan = sDate2 - sDate1;
    dateSpan = Math.abs(dateSpan);
    iDays = Math.floor(dateSpan / (24 * 3600 * 1000));
    return iDays
};

//去掉空格方法
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.ltrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.rtrim = function () {
    return this.replace(/(\s*$)/g, "");
}


function AjaxFailResult(xhr, status, err) {
    if (xhr.status == "400") {
        self.location = baseUrl;
    }

    if (status != "success") {
         
        alert("Ajax 出错");
        
    }

}

function closeWindows() {
    var userAgent = navigator.userAgent;
    if (userAgent.indexOf("Firefox") != -1
    || userAgent.indexOf("Chrome") != -1) {
        close();//直接调用JQUERY close方法关闭
    } else {
        window.opener = null;
        window.open("", "_self");
        window.close();
    }
};

function ResetMessage()
{
    $('#IE_ModelStageForm #message-error').html("");
    $('#IE_ModelStageForm #message-Success').html("");
}

//验证字符串是否是数字
function checkNumber(theObj) {
    var reg = /^[0-9]+.?[0-9]*$/;
    if (reg.test(theObj)) {
        return true;
    }
    return false;
}
function checkNumberInt(theObj) {
    var reg = /^[0-9]+?[0-9]*$/;
    if (reg.test(theObj)) {
        return true;
    }
    return false;
}
//获取url变量名称
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}
$(document).ready(function () {
    
    //Login 页面
    var $UserLogin = $('#UserLogin');
    var $txtUserId = $('#Login #UserId');
    var $txtPassword = $('#Login #PassWord');
    var $lblMsg = $('#lblMsg'); 

    var $btnLogin = $('#Login #LoginButton');
    
    var $ARegisterLink = $('#RegisterLink');
    var $chAutologin = $('#autologin')[0];


    $chAutologin.checked = $.cookie('PUB_Report_autologin');
    if ($.cookie('PUB_Report_autologin') == "true") {
        $chAutologin.checked = true;
    } else {
        $chAutologin.checked = false;
    }
    $txtUserId.val($.cookie('PUB_Report_UserId'));
    $txtPassword.val($.cookie('PUB_Report_Password'));

    

    if ($chAutologin.checked )
    {
        //自动登录
        Login();
    }

    $btnLogin.click(function () {
        Login();
      
    });
    $txtPassword.keypress(function (event) {
        if (event.keyCode == "13")
        {
            Login();
           
        }
    });
    
    function Login() {
        var UserID, PassWord;

        UserID = $txtUserId.val();
        PassWord = $txtPassword.val();


        if (UserID == "" || UserID == undefined) {
            $lblMsg.html("用户名 不能为空，请填写!");
            return false;
        }
        if (PassWord == "" || PassWord == undefined) {
            $lblMsg.html("密码 不能为空，请填写!");
            return false;
        }
        $lblMsg.html("");
        jsonData = { "UserID": UserID, "PassWord": PassWord };
         
        var url = baseUrl + "/User/LoginCheckAuth";
        var htmlLoging = " <div><span style ='margin-left:40%'><img src='../CSS/Images/Logining.gif'  /></span></div>";
        var htmlLoginBtn = "<input type='button' id='btnLogin' value='登录' class='btn btn-block btn-primary btn-small'    />";
        $btnLogin.html();
        $btnLogin.html(htmlLoging);
        ajaxJsonRequestDerived(url, 'post', jsonData, LoginSuccess, AjaxFailResult);
    }
    function LoginSuccess(data) {
        if (data.result == "OK") {


            $lblMsg.html("登录成功");
            self.parent.tb_remove_reload();    //close the thickbox
            var chk = $chAutologin.checked;
            if (chk)
            {
                $.cookie('PUB_Report_autologin', chk, { expires: 7 });
                $.cookie('PUB_Report_UserId', $txtUserId.val(), { expires: 7 });
                $.cookie('PUB_Report_Password', $txtPassword.val(), { expires: 7 });
            }
            else {
                $.cookie('PUB_Report_autologin', chk);
                $.cookie('PUB_Report_UserId', $txtUserId.val());
                $.cookie('PUB_Report_Password', $txtPassword.val());
            }

        } else {
            $lblMsg.html("用户名或密码不正确");
            $txtPassword.focus();
            $txtPassword.val("");
            var htmlLoginBtn = "<input type='button' id='btnLogin' onclick = 'Login()'  value='登录' class='btn btn-block btn-primary btn-small'    />";
            $btnLogin.html();
            $btnLogin.html(htmlLoginBtn);

        }
    }
 

    $ARegisterLink.click(function () {
        var url = baseUrl + "/User/RegisterUser";
        self.parent.location.href = url;
        

    });
    //Login 页面End

 

     
});