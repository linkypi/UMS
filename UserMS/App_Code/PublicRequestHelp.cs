using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using UserMS.API;

namespace UserMS
{
    public class PublicRequestHelp
    {
        dynamic busy;

        UserMsServiceClient client;

        public static void UpdateInitData(dynamic busy)
        {
            var a = new ThreadStart(() =>
                {
                    busy.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                                                                          delegate()
                                                                          {
                                                                              busy.IsBusy = true;

                                                                          }));
                    try
                    {
                        var webreturn = Store.wsclient.InitData(new DateTime());
                        if (webreturn.ReturnValue)
                        {
                            InitStore(webreturn.ArrList);
                            busy.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                                                                                  delegate()
                                                                                  {
                                                                                      MessageBox.Show(
                                                                                          Application.Current
                                                                                                     .MainWindow,
                                                                                          "更新基础数据成功");

                                                                                  }));

                        }
                        else
                        {
                            busy.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                                                                                  delegate()
                                                                                  {
                                                                                      MessageBox.Show(
                                                                                          Application.Current
                                                                                                     .MainWindow,
                                                                                          "更新基础数据失败");

                                                                                  }));
                        }
                    }
                    catch
                    {
                        busy.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                                                                              delegate()
                                                                              {
                                                                                  MessageBox.Show(
                                                                                      Application.Current.MainWindow,
                                                                                      "更新基础数据失败");

                                                                              }));
                    }
                    busy.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                                                                              delegate()
                                                                              {
                                                                                  busy.IsBusy = false;

                                                                              }));


                });
            var b = new Thread(a);
            b.Start();



        }


        /// <summary>
        /// 请求辅助类 Main
        /// </summary>
        /// <param name="busy">加载状态</param>
        /// <param name="methodid">方法</param>
        /// <param name="objs">参数</param>
        /// <param name="MainEvent">回调</param>
        public PublicRequestHelp(dynamic busy, int methodid, object[] objs, EventHandler<MainCompletedEventArgs> MainEvent)
        {

            this.busy = busy;
            if (this.busy != null && this.busy.IsBusy) return;
            if (this.busy != null) this.busy.IsBusy = true;
            client = Store.wsclient;
            if (!IsLogin()) return;
            var c1Type = client.GetType();


            var field = c1Type.GetField("MainCompleted", BindingFlags.NonPublic | BindingFlags.Instance);

            MulticastDelegate multicastDelegate = field.GetValue(client) as MulticastDelegate;
            if (multicastDelegate != null)
                foreach (var listener in multicastDelegate.GetInvocationList())
                {
                    client.MainCompleted -= listener as EventHandler<MainCompletedEventArgs>;
                    //Do what you got to do to ensure the listener is not a zombie
                }


            client.MainCompleted += MainEvent;
            client.MainAsync(methodid, objs.ToList());

        }

        /// <summary>
        /// 请求辅助类 Report
        /// </summary>
        /// <param name="busy">加载状态</param>
        /// <param name="methodid">方法</param>
        /// <param name="objs">参数</param>
        /// <param name="MainEvent">回调</param>
        public PublicRequestHelp(dynamic busy, int methodid, object[] objs, EventHandler<MainReportCompletedEventArgs> MainEvent)
        {
            this.busy = busy;
            if (this.busy != null && this.busy.IsBusy) return;
            if (this.busy != null) this.busy.IsBusy = true;
            client = Store.wsclient;
            if (!IsLogin()) return;
            var c1Type = client.GetType();


            var field = c1Type.GetField("MainReportCompleted", BindingFlags.NonPublic | BindingFlags.Instance);

            MulticastDelegate multicastDelegate = field.GetValue(client) as MulticastDelegate;
            if (multicastDelegate != null)
                foreach (var listener in multicastDelegate.GetInvocationList())
                {
                    client.MainReportCompleted -= listener as EventHandler<MainReportCompletedEventArgs>;
                    //Do what you got to do to ensure the listener is not a zombie
                }
            client.MainReportCompleted += MainEvent;
            client.MainReportAsync(methodid, Store.LoginUserInfo.UserName, Store.LoginUserInfo.UserPwd, "", objs.ToList());

        }
        /// <summary>
        /// 请求辅助类 获取报表信息
        /// </summary>
        /// <param name="busy">加载状态</param>
        /// <param name="methodid">方法</param>
        /// <param name="objs">参数</param>
        /// <param name="MainEvent">回调</param>
        public PublicRequestHelp(string reportName, EventHandler<MainReportViewInfoCompletedEventArgs> MainEvent)
        {
            //this.busy = busy;
            //if (this.busy != null && this.busy.IsBusy) return;
            //if (this.busy != null) this.busy.IsBusy = true;
            client = Store.wsclient;
            if (!IsLogin()) return;
            var c1Type = client.GetType();


            var field = c1Type.GetField("MainReportViewInfoCompleted", BindingFlags.NonPublic | BindingFlags.Instance);

            MulticastDelegate multicastDelegate = field.GetValue(client) as MulticastDelegate;
            if (multicastDelegate != null)
                foreach (var listener in multicastDelegate.GetInvocationList())
                {
                    client.MainReportViewInfoCompleted -= listener as EventHandler<MainReportViewInfoCompletedEventArgs>;
                    //Do what you got to do to ensure the listener is not a zombie
                }
            client.MainReportViewInfoCompleted += MainEvent;
            client.MainReportViewInfoAsync(reportName);
        }
        /// <summary>
        /// 如果未登录则登陆，如果已掉线则重新登录
        /// </summary>
        public static bool IsLogin()
        {
            if (Store.LoginUserInfo == null)
            {
                Application.Current.MainWindow.Content = new Login();
                return false;
            }
            if (Store.wsclient == null || Store.wsclient.State != CommunicationState.Opened)
            {
                Application.Current.MainWindow.Content = new Login();
                return false;
            }
            return true;
        }
        //        /// <summary>
        //        /// 请求辅助类
        //        /// </summary>
        //        /// <param name="busy">加载状态</param>
        //        /// <param name="methodid">方法</param>
        //        /// <param name="objs">参数</param>
        //        /// <param name="MainEvent">回调</param>
        //        public PublicRequestHelp(System.Windows.Controls.BusyIndicator busy, int methodid, object[] objs, EventHandler<API.GetRoleCompletedEventArgs> MainEvent)
        //        {
        //            this.busy = busy;
        //            if (this.busy != null && this.busy.IsBusy) return;
        //            if (this.busy != null) this.busy.IsBusy = true;
        //            client = new API.UserMsServiceClient();
        //            client.GetRoleCompleted += MainEvent;
        //            client.GetRoleAsync(null);
        //
        //        }
        //        /// <summary>
        //        /// 请求辅助类
        //        /// </summary>
        //        /// <param name="busy">加载状态</param>
        //        /// <param name="methodid">方法</param>
        //        /// <param name="objs">参数</param>
        //        /// <param name="MainEvent">回调</param>
        //        public PublicRequestHelp(System.Windows.Controls.BusyIndicator busy, int methodid, object[] objs, EventHandler<API.mmmmCompletedEventArgs> MainEvent)
        //        {
        //            this.busy = busy;
        //            if (this.busy != null && this.busy.IsBusy) return;
        //            if (this.busy != null) this.busy.IsBusy = true;
        //            client = new API.UserMsServiceClient();
        //            client.mmmmCompleted += MainEvent;
        //            client.mmmmAsync(null);
        //
        //        }
        public static void InitStore(List<object> arrList)
        {
            Store.RoleInfo = (arrList[0] == null
                ? (List<Sys_RoleInfo>) Store.GetClientStore("RoleInfo")
                : (List<Sys_RoleInfo>) arrList[0]) ?? new List<Sys_RoleInfo>();

            Store.ProInfo = (arrList[1] == null
                ? (List<Pro_ProInfo>) Store.GetClientStore("ProInfo")
                : (List<Pro_ProInfo>) arrList[1]) ?? new List<Pro_ProInfo>();
            Store.ProTypeInfo = (arrList[2] == null
                ? (List<Pro_TypeInfo>) Store.GetClientStore("ProTypeInfo")
                : (List<Pro_TypeInfo>) arrList[2]) ?? new List<Pro_TypeInfo>();
            Store.ProHallInfo = (arrList[3] == null
                ? (List<Pro_HallInfo>) Store.GetClientStore("ProHallInfo")
                : (List<Pro_HallInfo>) arrList[3]) ?? new List<Pro_HallInfo>();
            Store.VIPType = (arrList[4] == null
                ? (List<VIP_VIPType>) Store.GetClientStore("VIPType")
                : (List<VIP_VIPType>) arrList[4]) ?? new List<VIP_VIPType>();

            Store.ProClassInfo = (arrList[5] == null
                ? (List<Pro_ClassInfo>) Store.GetClientStore("ProClassInfo")
                : (List<Pro_ClassInfo>) arrList[5]) ?? new List<Pro_ClassInfo>();

            Store.CardType = (arrList[6] == null
                ? (List<VIP_IDCardType>) Store.GetClientStore("CardType")
                : (List<VIP_IDCardType>) arrList[6]) ?? new List<VIP_IDCardType>();

            Store.AreaInfo = (arrList[7] == null
                ? (List<Pro_AreaInfo>) Store.GetClientStore("AreaInfo")
                : (List<Pro_AreaInfo>) arrList[7]) ?? new List<Pro_AreaInfo>();
            Store.UserOp = (arrList[8] == null
                ? (List<Sys_UserOp>) Store.GetClientStore("UserOp")
                : (List<Sys_UserOp>) arrList[8]) ?? new List<Sys_UserOp>();
            Store.UserOpList = (arrList[9] == null
                ? (List<Sys_UserOPList>) Store.GetClientStore("UserOpList")
                : (List<Sys_UserOPList>) arrList[9]) ?? new List<Sys_UserOPList>();

            Store.UserInfos = (arrList[10] == null
                ? (List<Sys_UserInfo>) Store.GetClientStore("UserInfos")
                : (List<Sys_UserInfo>) arrList[10]) ?? new List<Sys_UserInfo>();

            Store.Options = (arrList[11] == null
                ? (List<Sys_Option>) Store.GetClientStore("Options")
                : (List<Sys_Option>) arrList[11]) ?? new List<Sys_Option>();

            Store.MenuInfos = (arrList[12] == null
                ? (List<Sys_MenuInfo>) Store.GetClientStore("MenuInfos")
                : (List<Sys_MenuInfo>) arrList[12]) ?? new List<Sys_MenuInfo>();


            Store.SellTypes = (arrList[15] == null
                ? (List<Pro_SellType>) Store.GetClientStore("SellTypes")
                : (List<Pro_SellType>) arrList[15]) ?? new List<Pro_SellType>();


            Store.DeptInfo = (arrList[16] == null
                ? (List<Sys_DeptInfo>) Store.GetClientStore("DeptInfo")
                : (List<Sys_DeptInfo>) arrList[16]) ?? new List<Sys_DeptInfo>();

            Store.Level = (arrList[17] == null
                ? (List<Pro_LevelInfo>) Store.GetClientStore("Level")
                : (List<Pro_LevelInfo>) arrList[17]) ?? new List<Pro_LevelInfo>();

            Store.ProMainInfo = (arrList[18] == null
                ? (List<Pro_ProMainInfo>) Store.GetClientStore("ProMainInfo")
                : (List<Pro_ProMainInfo>) arrList[18]) ?? new List<Pro_ProMainInfo>();


            Store.YanbaoPriceStep = (arrList[19] == null
                ? (List<Pro_YanbaoPriceStepInfo>) Store.GetClientStore("YanboPrice")
                : (List<Pro_YanbaoPriceStepInfo>) arrList[19]) ??
                                    new List<Pro_YanbaoPriceStepInfo>();

            Store.ProNameInfo = (arrList[20] == null
                ? (List<Pro_ProNameInfo>) Store.GetClientStore("ProNameInfo")
                : (List<Pro_ProNameInfo>) arrList[20]) ??
                                new List<Pro_ProNameInfo>();

            Store.RulesTypeInfo = (arrList[21] == null
                ? (List<Rules_TypeInfo>)Store.GetClientStore("RulesTypeInfo")
                : (List<Rules_TypeInfo>)arrList[21]) ??
                                  new List<Rules_TypeInfo>();

            Store.PacSalesNameInfo = (arrList[22] == null
              ? (List<Package_SalesNameInfo>)Store.GetClientStore("PacSalesNameInfo")
              : (List<Package_SalesNameInfo>)arrList[22]) ??
                                new List<Package_SalesNameInfo>();


            Store.BigAreaInfo = (arrList[23] == null
              ? (List<Pro_BigAreaInfo>)Store.GetClientStore("BigAreaInfo")
              : (List<Pro_BigAreaInfo>)arrList[23]) ??
                                new List<Pro_BigAreaInfo>(); 

            Store.CheckInfo = (arrList[24] == null
                  ? (List<ASP_CheckInfo>)Store.GetClientStore("CheckInfo")
                  : (List<ASP_CheckInfo>)arrList[24]) ??
                                    new List<ASP_CheckInfo>();
            Store.ErrorInfo = (arrList[25] == null
                  ? (List<ASP_ErrorInfo>)Store.GetClientStore("ErrorInfo")
                  : (List<ASP_ErrorInfo>)arrList[25]) ??
                                    new List<ASP_ErrorInfo>(); 
            Store.BillFields=(arrList[26]==null)?
                (List<Pro_BillFieldInfo>)Store.GetClientStore("BillFields"):
            (List<Pro_BillFieldInfo>) arrList[26] ?? new List<Pro_BillFieldInfo>();
            
            Store.ErrorTypes = (arrList[27]==null)?
                (List<API.ASP_ErrType>)Store.GetClientStore("ErrorTypes") :
            (List<API.ASP_ErrType>)arrList[27] ?? new List<API.ASP_ErrType>();


            Store.ProOthers = (arrList[28] == null) ?
                (List<API.ASP_ProOther>)Store.GetClientStore("ProOthers") :
            (List<API.ASP_ProOther>)arrList[28] ?? new List<API.ASP_ProOther>();

            List<API.Sys_Role_Menu_ProInfo> roleMenuProInfos = (List<Sys_Role_Menu_ProInfo>) arrList[13] ??
                                                               new List<Sys_Role_Menu_ProInfo>();
            List<API.Sys_Role_Menu_HallInfo> roleMenuHallInfos = (List<Sys_Role_Menu_HallInfo>) arrList[14] ??
                                                                 new List<Sys_Role_Menu_HallInfo>();

            Store.PriceStep = (arrList[29] == null) ?
                (List<API.Sys_SalaryPriceStep>)Store.GetClientStore("PriceStep") :
            (List<API.Sys_SalaryPriceStep>)arrList[29] ?? new List<API.Sys_SalaryPriceStep>();


            foreach (var sysRoleInfo in Store.RoleInfo)
            {
                sysRoleInfo.Sys_Role_Menu_ProInfo =
                    roleMenuProInfos.Where(p => p.RoleID == sysRoleInfo.RoleID).ToList();
                sysRoleInfo.Sys_Role_Menu_HallInfo =
                    roleMenuHallInfos.Where(p => p.RoleID == sysRoleInfo.RoleID).ToList();
            }

            Store.SetClientStore("RoleInfo", Store.RoleInfo);
            Store.SetClientStore("ProInfo", Store.ProInfo);
            Store.SetClientStore("ProTypeInfo", Store.ProTypeInfo);
            Store.SetClientStore("ProHallInfo", Store.ProHallInfo);
            Store.SetClientStore("lastinitdata", DateTime.Now);
            Store.SetClientStore("VIPType", Store.VIPType);
            Store.SetClientStore("ProClassInfo", Store.ProClassInfo);
            Store.SetClientStore("CardType", Store.CardType);
            Store.SetClientStore("AreaInfo", Store.AreaInfo);
            Store.SetClientStore("UserOp", Store.UserOp);
            Store.SetClientStore("UserOpList", Store.UserOpList);
            Store.SetClientStore("UserInfos", Store.UserInfos);
            Store.SetClientStore("Options", Store.Options);
            Store.SetClientStore("MenuInfos", Store.MenuInfos);
            Store.SetClientStore("SellTypes", Store.SellTypes);
            Store.SetClientStore("DeptInfo", Store.DeptInfo);
            Store.SetClientStore("Level", Store.Level);
            Store.SetClientStore("ProMainInfo", Store.ProMainInfo);
            Store.SetClientStore("YanboPrice", Store.YanbaoPriceStep);
            Store.SetClientStore("ProNameInfo", Store.ProNameInfo);
            Store.SetClientStore("RulesTypeInfo", Store.RulesTypeInfo);
            Store.SetClientStore("CheckInfo", Store.CheckInfo);
            Store.SetClientStore("ErrorInfo", Store.ErrorInfo);
            Store.SetClientStore("BillFields", Store.BillFields);
            Store.SetClientStore("PriceStep", Store.PriceStep);
        }
    }
}

