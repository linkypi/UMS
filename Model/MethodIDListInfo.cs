using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class MethodIDListInfo
    {
        #region 借贷审批方法编号
        /// <summary>
        /// 一级审批方法
        /// </summary>
        public static int BorowAduit_Aduit = 21;

        /// <summary>
        /// 二级审批方法
        /// </summary>
        public static int BorowAduit2_Aduit = 207;

        /// <summary>
        /// 三级审批方法
        /// </summary>
        public static int BorowAduit3_Aduit = 208;

        /// <summary>
        /// 四级审批方法
        /// </summary>
        public static int BorowAduit4_Aduit = 286;  
        #endregion

        #region 批发审批方法编号
        /// <summary>
        /// 一级审批方法
        /// </summary>
        public static int Pro_SellAduit_Aduit = 27;

        
        /// <summary>
        /// 二级审批方法
        /// </summary>
        public static int Pro_SellAduit2_Aduit = 217;
      
        /// <summary>
        /// 三级审批方法
        /// </summary>
        public static int Pro_SellAduit3_Aduit = 218;
        #endregion

        #region 退货审批方法
        /// <summary>
        /// 退货审批方法
        /// </summary>
        public static int Pro_SellBack_Aduit = 32;
 
        #endregion
        #region 套餐审批方法
        /// <summary>
        /// 套餐一级审批方法
        /// </summary>
        public static int Pro_SellOff_Aduit1 = 300;
        /// <summary>
        /// 套餐二级审批方法
        /// </summary>
        public static int Pro_SellOff_Aduit2 = 297;
        /// <summary>
        /// 套餐三级审批方法
        /// </summary>
        public static int Pro_SellOff_Aduit3 = 308;
        #endregion
    }
}
