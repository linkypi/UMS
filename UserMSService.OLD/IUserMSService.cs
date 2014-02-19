using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Model;
namespace UserMSService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IUserMSService”。
    [ServiceContract]
    public interface IUserMSService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        LoginInfo Login(string username, string password);

        [OperationContract]
        InitData InitData(UserInfo userInfo);

        [OperationContract]
        WebReturn Main(int Method, object[] args, UserInfo userInfo);






        // TODO: 在此添加您的服务操作
    }

    
}
