using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Model;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.SessionState;
using Common;

namespace UserMSService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“UserMsService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 UserMsService.svc 或 UserMsService.svc.cs，然后开始调试。
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceContract(SessionMode=SessionMode.Allowed)]
    public class UserMsService
    {
       public WebReturn webR;
       public UserInfo user;
        [OperationContract]
        public WebReturn Main(int MethodId, object[] args)
        {
            //全局验证
            webR = MainCheck(MethodId);
            if (!webR.ReturnValue)
            {
                return webR;
            }
            //调用方法
            return webR;
        }
        [OperationContract]
        public WebReturn Login(string username, string password,string sign)
        {
            //解密时间戳

            //登陆
                //验证信息
                //Sys_roleMethod List 菜单 方法 仓库
                //菜单 xml
                //
                //InitdataState 需要初始化的信息
            //返回 userinfo InitdataState
            return new WebReturn() {  };
        }
        [OperationContract]
        public WebReturn InitData()
        {

            //是否登陆
            //解析需要初始化清单
            //返回清单实体类

            return new WebReturn() {  };
        }

        [OperationContract]
        public WebReturn UpdatePwd(string Newpassword,string sign)
        {
            ///
            return new WebReturn() { };
        }

        public WebReturn MainCheck(int MethodId)
        {

            WebReturn r = new WebReturn();
            ////验证是否已经登录
            if (!IsLogin())
            {
                r.Message = "用户尚未登录";
                r.ReturnValue = false;
                return r;
            }
            r = RoleCheckHelp.MainCheck((UserInfo)(MySession["User"]), MethodId);
            return webR;
        } 

        public bool IsLogin()
        {
            return MySession["User"] != null;
        }
        

        public HttpSessionState MySession
        {
            get { return HttpContext.Current.Session; }
          
        }



    }
}
