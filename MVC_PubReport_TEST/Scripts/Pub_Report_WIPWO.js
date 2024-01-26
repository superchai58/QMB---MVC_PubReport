

$(document).ready(function () {
     
    var $txtFilePath = $('#UploadExcelFilePath');

    var $lblMessageError = $('#message-error');
    var $lblMessageSuccess = $('#message-success'); 
    var $UserLogin = $('#UserLogin');
    var $UserLogOut = $('#UserLogOut');
     
 
    var $btnUploadExcel = $('#UploadExcel'); 
    var tableDataRows = 0; 
 
    var   uploadOK
    $btnUploadExcel.click(function () {
        var name = $txtFilePath.val();
        var formaData = new FormData();

        uploadOK = 0;
        formaData.append("file", $txtFilePath[0].files[0])
        formaData.append("name", name);


        if (name == "" || name == undefined) {
            ErrorMsg("请选择需要上传的Excel文件");
            return;
        }
        $btnUploadExcel.val("资料正在上传...");
        //DisableBtns(); 
        var url = baseUrl + "/Query/QueryWIPWO_UploadExcel";
        ajaxJsonRequestFile(url, 'post', formaData, funQueryWIPWO, AjaxFailResult);


    });
    function funQueryWIPWO(data) {
        if (data.result != "OK") { 
            ErrorMsg(data.result);  
            return;

        } else { 
            uploadOK = 1; 
            OKMsg(data.rowMsg); 
            return;
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
                $UserLogin.html('<a class="thickbox" title="登录PUB Report"  href=' + baseUrl + '/User/Login?keepThis=True&TB_iframe=True&height=430&width=350     > 请重新登录 </a>')


                tb_init('a.thickbox, area.thickbox, input.thickbox');
                imgLoader = new Image();// preload image
                imgLoader.src = tb_pathToImage;
            }

        }
        $lblMessageError.html(msg);
        $lblMessageSuccess.html("");
    }
})
