namespace UserMS.Common
{
    public class MethodIDStore
    {
        #region 调拨方法ID
        public static int MethodID = 16;
        public static int Method_List = 69;
        public static int Method_CM = 70;
        public static int Method_Cancel = 26;
        public static int Method_Accept = 19;
        #endregion
        #region 库存方法ID
        public static int Method_AllStore = 64;
        #endregion
        #region 会员方法ID
        /// <summary>
        /// 获取会员方法ID
        /// </summary>
        public static int Method_GetVIP = 3;
        #endregion
        #region 获取取消续期
        public static int Method_GetRenewID = 89;
        #endregion
        #region 添加商品使用的方法ID
        public static  int PerytMethodID = 166;//获取商品属性
        public static int GetRole = 167;//获取角色
        public static int AddProMethodID = 173;//添加商品
        #endregion
        #region 员工管理方法ID
        public static int GetStaffMethodID = 158;
        public static int AddSatffMethodID = 156;
        public static int UpdateSatffMethodID = 157;
        #endregion 
        #region 添加类别
        public static int AddClassMethodID = 125;
        #endregion
        #region 商品转换类别提交ID
        public static int AddOfChangedID = 185;
        #endregion
        #region 商品操作
        public static int UpdatePro = 174;
        public static int GetProModel = 5;
        public static int DelProInfo = 204;
        #endregion
        #region 职位表操作
        public static int AddUserOp = 192;
        public static int DelUserOp = 193;
        public static int UpdateUserOp = 194;
        public static int GetModelUserOp = 195;
        #endregion 
        #region 新增仓库或修改仓库
        public static int AddHall = 198;
        public static int UpdateHall = 199;
        public static int SelectHall = 200;
        #endregion 

        #region 添加总商品
        public static int AddProMainInfo = 201;
        public static int UpdateProMainInfo = 202;
        public static int GetProMainInfo = 203;
        #endregion 

        #region 空冲转类别 
        public static int AddAirOut = 216;
        public static int GetAirAcceptModel = 223;
        public static int AcceptAirDB = 224;
        public static int GetAirDBModel = 225;

        public static int GetAirCancelModel = 226;
        public static int CancelAirDB = 227;
        public static int GetAirPro = 228;
        #endregion 

        #region 门店优惠
        public static int AddHallOff = 233;
        public static int HallOffSearch = 234;
        #endregion 

        #region 部门操作
        public static int GetDeptInfo = 241;
        public static int AddDeptInfo = 243;
        public static int DelDeptInfo = 244;
        public static int UpdateDeptInfo = 245;
        #endregion

        #region 套餐配置
        public static int GetVIPMethod = 3;
        public static int GetSalesName = 258;
        public static int AddPackage = 148;
        public static int GetPackageSource = 149;
        public static int DeletePackage = 152;
        #endregion

        
    }
}