using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace Model
{
    
//    [global::System.Runtime.Serialization.DataContractAttribute()]
    public class UserInfo:Sys_UserInfo
    {
        private ArrayList _MenuList;
        private ArrayList _MethodList;
        private Sys_RoleInfo _Role;
        private List<RoleMenuInfo> roleMenuInfo;

        public List<RoleMenuInfo> RoleMenuInfo
        {
            get { return roleMenuInfo; }
            set { roleMenuInfo = value; }
        }

//        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public Sys_RoleInfo RoleInfo
        {
            get { return _Role; }
            set { _Role = value; }
        }
//         [global::System.Runtime.Serialization.DataMemberAttribute()]
        public ArrayList MethodList
        {
            get { return _MethodList; }
            set { _MethodList = value; }
        }
//         [global::System.Runtime.Serialization.DataMemberAttribute()]
        public ArrayList MenuList
        {
            get { return _MenuList; }
            set { _MenuList = value; }
        }
        public UserInfo()
        {
            
        }
        //public UserInfo(Sys_UserInfo userInfo)
        //{
        //    this.roleMenuInfo = new List<Model.RoleMenuInfo>();
        //    //RoleMenuInfo rmi = null;
        //    //foreach(var item in  userInfo.Sys_RoleInfo.Sys_Role_MenuInfo)
        //    //{
        //    //    rmi = new RoleMenuInfo(item);
        //    //    this.roleMenuInfo.Add(rmi);
        //    //}
        //    this.UserID = userInfo.UserID;
        //    this.UserName = userInfo.UserName;
        //    this.UserPwd = userInfo.UserPwd;
        //    this.RealName = userInfo.RealName;
        //    this.UserIP = userInfo.UserIP;
        //    this.DtpID = userInfo.DtpID;
        //    this.UpdUserID = userInfo.UpdUserID;
        //    this.RoleID = userInfo.RoleID;
        //    this.Note = userInfo.Note;
        //    this.CanLogin = userInfo.CanLogin;
        //    this.SysDate = userInfo.SysDate;
        //    this.Flag = userInfo.Flag;
        //    this.CancelLimit = userInfo.CancelLimit;
        //    this.AduitLimit = userInfo.AduitLimit;
        //    this.Pro_BackInfo = userInfo.Pro_BackInfo;
        //    this.Pro_BorowAduit = userInfo.Pro_BorowAduit;
        //    this.User = userInfo.User;
        //    this.Pro_BorowInfo = userInfo.Pro_BorowInfo;
        //    this.Pro_InOrder = userInfo.Pro_InOrder;
        //    this.Pro_OutInfo = userInfo.Pro_OutInfo;
        //    this.PRO_OUTI_REFERENCE_SYS_USER2 = userInfo.PRO_OUTI_REFERENCE_SYS_USER2;
        //    this.PRO_OUTI_REFERENCE_SYS_USER3 = userInfo.PRO_OUTI_REFERENCE_SYS_USER3;
        //    this.Pro_PriceChange = userInfo.Pro_PriceChange;
        //    this.Pro_RepairInfo = userInfo.Pro_RepairInfo;
        //    this.Pro_ReturnInfo = userInfo.Pro_ReturnInfo;
        //    this.Pro_SellAduit = userInfo.Pro_SellAduit;
        //    this.PRO_SELL_REFERENCE_SYS_USER5 = userInfo.PRO_SELL_REFERENCE_SYS_USER5;
        //    this.Pro_SellBack = userInfo.Pro_SellBack;
        //    this.PRO_SELL_REFERENCE_SYS_USER8 = userInfo.PRO_SELL_REFERENCE_SYS_USER8;
        //    this.Pro_SellBackAduit = userInfo.Pro_SellBackAduit;
        //    this.PRO_SELL_REFERENCE_SYS_USER7 = userInfo.PRO_SELL_REFERENCE_SYS_USER7; 
        //    this.Sys_UserOPList = userInfo.Sys_UserOPList;
        //    this.SYS_USER_REFERENCE_SYS_USER4 = userInfo.SYS_USER_REFERENCE_SYS_USER4;
        //    this.Sys_UserRemindList = userInfo.Sys_UserRemindList;
        //    this.VIP_CardChange = userInfo.VIP_CardChange;
        //    this.VIP_OffList = userInfo.VIP_OffList;
        //    this.VIP_RenewBackAduit = userInfo.VIP_RenewBackAduit;
        //    this.VIP_RENE_REFERENCE_SYS_USER2 = userInfo.VIP_RENE_REFERENCE_SYS_USER2;
        //    this.VIP_VIPBack = userInfo.VIP_VIPBack;
        //    this.VIP_VIPBackAduit = userInfo.VIP_VIPBackAduit;
        //    this.VIP_VIPB_REFERENCE_SYS_USER3 = userInfo.VIP_VIPB_REFERENCE_SYS_USER3;
        //    this.VIP_VIPInfo = userInfo.VIP_VIPInfo;
        //    this.VIP_VIPInfo_BAK = userInfo.VIP_VIPInfo_BAK;
        //    this.VIP_VIPType = userInfo.VIP_VIPType;
        //    this.VIP_VIPType_Bak = userInfo.VIP_VIPType_Bak;
        //    this.VIP_VIPTypeService_BAK = userInfo.VIP_VIPTypeService_BAK;

        //}

    }
}
