<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >

<head>
    <title>广东广信通信服务有限公司中山分公司 终端销售综合管理系统</title>
    <style type="text/css">
    html, body {
	    height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
    }
    .container p {
    color: #2F2F2F;
    font: 16px Arial,Helvetica,sans-serif;
    padding-bottom: 10px;
}
    .container p.font_size_18 {
    font-size: 18px;
}
.container p.font_size_18 span {
    display: block;
}
.container p.font_size_18 span.span_tx1 {
    color: #8E8D8D;
    font: 16px Arial,Helvetica,sans-serif;
    padding: 25px 0;
}
.container p.font_size_18 span.span_tx2, .container p.font_size_18 span.span_tx2 a {
    font: 12px/18px Arial,Helvetica,sans-serif;
}
.container a {
    background: url("../img/link_line.png") repeat-x scroll 0 bottom transparent;
    color: #1B93D5;
    font: 16px Arial,Helvetica,sans-serif;
    padding-bottom: 2px;
    text-decoration: none;
}
.container a:hover {
    background: none repeat scroll 0 0 transparent;
}
.container {
    padding-top: 130px;
    text-align: center;
}
    </style>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript" src="SplashScreen.js" ></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Silverlight 应用程序中未处理的错误 " + appSource + "\n";

            errMsg += "代码: " + iErrorCode + "    \n";
            errMsg += "类别: " + errorType + "       \n";
            errMsg += "消息: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "文件: " + args.xamlFile + "     \n";
                errMsg += "行: " + args.lineNumber + "     \n";
                errMsg += "位置: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "行: " + args.lineNumber + "     \n";
                    errMsg += "位置: " + args.charPosition + "     \n";
                }
                errMsg += "方法名称: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="height:100%">
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
            
            
            
  <%     
      string orgSourceValue = @"ClientBin/UserMS.xap";     
    string param;

    if (System.Diagnostics.Debugger.IsAttached)     
    {
        param = "<param name=\"source\" value=\"" + orgSourceValue + "\" />";
    }
    else     
    {     
      string xappath = HttpContext.Current.Server.MapPath(@"") + @"\" + orgSourceValue;

      DateTime xapCreationDate = System.IO.File.GetLastWriteTime(xappath);      

      param = "<param name=\"source\" value=\"" + orgSourceValue + "?ts=" +
                xapCreationDate.ToFileTimeUtc().ToString() + "\" />";     
    }

    Response.Write(param);     
  %>

		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="5.0.61118.0" />
            <param name="autoUpgrade" value="true" />
            <param name="splashscreensource" value="/SplashScreen.xaml" />
            <param name="onSourceDownloadProgressChanged" value="onSourceDownloadProgressChanged" />
		 <div style="margin: 0px; padding: 0px;" class="background">
                <div class="container">
                    
                    <div class="info_box">
                        
                        <p class="font_size_18">
                            若要使用本系统, <strong>请先安装 Microsoft Silverlight 插件</strong>. 
                            <br/>
                            <br/>
                            <a href="/plugins/sl_setup.exe">
                                    下载地址</a>
                        </p>
                    </div>
                    <!-- end info -->
                </div>
            </div>
	    </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe></div>
    </form>
</body>
</html>
