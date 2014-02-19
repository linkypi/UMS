using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using UserMS.API;

namespace UserMS
{
    public partial class Login 
    {
        public API.UserMsServiceClient webservice;
        //public API.UserMsServiceClient webservice =  new UserMsServiceClient();
        public Login()
        {
            //            StyleManager.ApplicationTheme = new Windows8Theme();
            InitializeComponent();


            DateTime dt = new DateTime(2000, 1, 1);
            Assembly assembly = Assembly.GetExecutingAssembly();
            String version = assembly.FullName.Split(',')[1];

            String fullversion = version.Split('=')[1];
            int dates = int.Parse(fullversion.Split('.')[2]);

            int seconds = int.Parse(fullversion.Split('.')[3]); 
            dt = dt.AddDays(dates);
            dt = dt.AddSeconds(seconds * 2);
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                this.Foot.Text += "   版本号: " +
                System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CheckForUpdateCompleted += CurrentDeployment_CheckForUpdateCompleted;
                System.Deployment.Application.ApplicationDeployment.CurrentDeployment.UpdateCompleted += CurrentDeployment_UpdateCompleted;
                System.Deployment.Application.ApplicationDeployment.CurrentDeployment.UpdateProgressChanged += CurrentDeployment_UpdateProgressChanged;

                this.LoginButton.Content = "正在检查更新...";
                this.LoginButton.IsEnabled = false;
                System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CheckForUpdateAsync();
            }
            else
            {
                this.Foot.Text += "   *非对外版本*";
            }
            this.Foot.Text += "   编译时间: " + dt.ToString();
            
           
//            if (Application.Current.IsRunningOutOfBrowser)
//            {
//                Application.Current.CheckAndDownloadUpdateCompleted += Current_CheckAndDownloadUpdateCompleted;
//                Application.Current.CheckAndDownloadUpdateAsync();
//                this.LoginButton.Content = "正在检查更新...";
//                this.LoginButton.IsEnabled = false;
//            }

        }

        void CurrentDeployment_CheckForUpdateCompleted(object sender, System.Deployment.Application.CheckForUpdateCompletedEventArgs e)
        {
            System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CheckForUpdateCompleted -=
                CurrentDeployment_CheckForUpdateCompleted;

            if (e.Error == null)
            {
                if (e.UpdateAvailable)
                {
                    this.LoginButton.Content = "正在更新...";
                    this.progrssbar.Visibility = Visibility.Visible;
                    //MessageBox.Show(System.Windows.Application.Current.MainWindow, "1.");
                    System.Deployment.Application.ApplicationDeployment.CurrentDeployment.UpdateAsync();
                    
                }
                else
                {
                    this.LoginButton.IsEnabled = true;
                    this.LoginButton.Content = "登录";
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "检查更新失败.");
                this.LoginButton.IsEnabled = true;
                this.LoginButton.Content = "登录";
            }
        }

        void CurrentDeployment_UpdateProgressChanged(object sender, System.Deployment.Application.DeploymentProgressChangedEventArgs e)
        {
            
            this.progrssbar.Maximum = e.BytesTotal;
            this.progrssbar.Value = e.BytesCompleted;

        }

        void CurrentDeployment_UpdateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            System.Deployment.Application.ApplicationDeployment.CurrentDeployment.UpdateCompleted -=
                CurrentDeployment_UpdateCompleted;
            System.Deployment.Application.ApplicationDeployment.CurrentDeployment.UpdateProgressChanged -=
                CurrentDeployment_UpdateProgressChanged;

            if (e.Error == null)
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "系统已成功更新, 请重启系统");
                
//                System.Windows.Forms.Application.Restart();
//                Application.Current.Shutdown();
                
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow, "系统更新失败, 请重新启动再试");
//                Application.Current.Shutdown();
            }
            
            
        }

//        void Current_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
//        {
//            if (e.Error == null)
//            {
//                if (e.UpdateAvailable)
//                {
//                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"系统已成功更新, 请重新启动本系统.");
//                    //App.Current.MainWindow.Close();
//                    this.LoginButton.Content = "请先重启本系统";
//
//                }
//                else
//                {
//                    this.LoginButton.IsEnabled = true;
//                    this.LoginButton.Content = "登录";
//                }
//
//            }
//            else
//            {
//                MessageBox.Show(System.Windows.Application.Current.MainWindow,"检查更新失败.");
//                this.LoginButton.IsEnabled = true;
//                this.LoginButton.Content = "登录";
//            }
//        }

        private void LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            this.busy.IsBusy = false;

            if (e.Error == null)
            {
                if (e.Result.ReturnValue == false)
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"登录失败: " + e.Result.Message);
                    return;

                }

                Store.LoginUserInfo = (Sys_UserInfo)e.Result.Obj;
                
                //this.Content = new Index();
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"登录失败: 服务器错误\n" + e.Error.Message);
                return;
            }

            DateTime lastfetch = (DateTime)(Store.GetClientStore("lastinitdata") ?? new DateTime());


            //this.webservice = new UserMsServiceClient();
            this.webservice.InitDataAsync(lastfetch);
            Store.LoginUserName = UserName.Text;
            Store.LoginUserPassword = this.UserPwd.Password;
            this.busy.BusyContent = "登陆成功, 正在获取基础数据";
            this.busy.IsBusy = true;
            
            //Store.userinfo = (NPS_Webservice.Webservice.Sys_UserInfo)e.Result.ReturnObj;

        }

        void webservice_InitDataCompleted(object sender, InitDataCompletedEventArgs e)
        {
            this.webservice.InitDataCompleted -= webservice_InitDataCompleted;

            if (e.Error == null)
            {
                if (e.Result.ReturnValue)
                {
                    try
                    {
                        //this.busy.IsBusy = false;

                        this.busy.IsIndeterminate = false;
                        this.busy.ProgressValue = 0;
                        //this.busy.IsBusy = true;
                        List<object> arrList = e.Result.ArrList;
                        PublicRequestHelp.InitStore(arrList);
                        try
                        {
                            //Store.clientSettings.Save();
                        }
                        catch (IsolatedStorageException ex)
                        {
                            //Store.clientSettings.Clear();
                            //Store.SetClientStore("NeedMoreSpace", true);
                            //.clientSettings.Save();
                        }
                        Store.wsclient = webservice;
                        Application.Current.MainWindow.Content = new Index();
                        //this.Content = new Index();
                        //this.Content = new Views.StockMS.EnteringStock.Mainaddcmgoods();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(System.Windows.Application.Current.MainWindow,"获得初始数据失败: " + ex.Message);

                    }

                }
                else
                {
                    MessageBox.Show(System.Windows.Application.Current.MainWindow,"获得初始数据失败: " + e.Result.Message);
                }
            }
            else
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"获得初始数据失败: 服务器错误\n" + e.Error.Message);
            }
            this.busy.IsBusy = false;
            this.busy.IsIndeterminate = true;

        }


        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(System.Windows.Application.Current.MainWindow,"haha");


            UserName.Focus();

        }

        private void RadButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.testversion.IsChecked == true)
            {
                Store.IsTesting = true;
                Store.ReportServiceURL = "http://www.zs96000.com:23457/WcfReportService.svc";
//                this.webservice=new UserMsServiceClient("TSET",new EndpointAddress(
//                        "http://192.168.0.8:23457/UserMSService.svc"));
//         ;
//                this.webservice = new UserMsServiceClient(new WSHttpBinding()
//                {
//                    
//                    ReaderQuotas =
//                        new XmlDictionaryReaderQuotas()
//                        {
//                            MaxArrayLength = 2147483646,
//                            MaxBytesPerRead = 2147483646,
//                            MaxDepth = 2147483646,
//                            MaxNameTableCharCount = 2147483646,
//                            MaxStringContentLength = 2147483646
//                        },
//                    AllowCookies = true,
//                    MessageEncoding = WSMessageEncoding.Mtom,
//                    MaxReceivedMessageSize = 2147483647
//                    ,
//                    MaxBufferPoolSize = 2147483647
//                    ,
//                    ReliableSession =
//                        new OptionalReliableSession() {Enabled = true, InactivityTimeout = new TimeSpan(8, 0, 0)},
//                    Security = new WSHttpSecurity() {Mode = SecurityMode.None}
//                    
//                },
//                    new EndpointAddress(
//                        "http://192.168.0.8:23457/UserMSService.svc"));
//                this.webservice.Endpoint.Behaviors.Add(new DataContractSerializerOperationBehavior(
//                    )
//                {
//                    MaxItemsInObjectGraph
//                        =
//                        2147483647
//                });
                this.webservice=new UserMsServiceClient();
                this.webservice.Endpoint.Address=new EndpointAddress( "http://www.zs96000.com:23457/UserMSService.svc");
                ;
            }
            else
            {
                Store.IsTesting = false;
                Store.ReportServiceURL = ConfigurationManager.AppSettings["ReportServiceUrl"];
                this.webservice = new UserMsServiceClient();
            }

            this.webservice.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(LoginCompleted);
            this.webservice.InitDataCompleted += webservice_InitDataCompleted;
            if (string.IsNullOrEmpty(this.UserName.Text) || string.IsNullOrEmpty(this.UserPwd.Password))
            {
                MessageBox.Show(System.Windows.Application.Current.MainWindow,"用户名或密码不能为空！");
                return;
            }
            this.busy.BusyContent = "正在登陆";
            this.busy.IsBusy = true;
            this.webservice.LoginAsync(this.UserName.Text, this.UserPwd.Password, null,DateTime.Now);
            if (sender!=null && Store.GetClientStore("NeedMoreSpace") != null)
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                store.IncreaseQuotaTo(store.Quota+ 50*1024*1024);
                Store.SetClientStore("NeedMoreSpace",null);
               // Store.clientSettings.Save();
            }
        }

        private void Foot2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           // Store.clientSettings.Clear();
            MessageBox.Show(System.Windows.Application.Current.MainWindow,"清空成功");
        }

        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            MultSelecter a = new MultSelecter();
            a.Show();

        }

        private void Login_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Store.GetClientStore("NeedMoreSpace") != null)
                {
                    var store = IsolatedStorageFile.GetUserStoreForApplication();
                    store.IncreaseQuotaTo(store.Quota + 50 * 1024 * 1024);
                    Store.SetClientStore("NeedMoreSpace", null);
                    //Store.clientSettings.Save();
                }
                RadButton_Click_1(null, null);
            }
        }
    }
}
