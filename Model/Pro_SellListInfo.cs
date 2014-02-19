using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text; 

namespace Model
{
    [InheritanceMapping(Code = "A", Type = typeof(Pro_SellListInfo_AirSell))]
    [InheritanceMapping(Code = "C", Type = typeof(Pro_SellListInfo_Child))]
    [InheritanceMapping(Code = "M", Type = typeof(Pro_SellListInfo_MemberSell))]
    [InheritanceMapping(Code = "S", Type = typeof(Pro_SellListInfo_SingleSell))]
    [InheritanceMapping(Code = "P", Type = typeof(Pro_SellListInfo_PhoneNumSell))]
    [InheritanceMapping(Code = "T", Type = typeof(Pro_SellListInfo_TicketSell))]
    [InheritanceMapping(Code = "Y", Type = typeof(Pro_SellListInfo_YanBaoSell))]
    [InheritanceMapping(Code = "I", Type = typeof(Pro_SellListInfo), IsDefault = true)]
    public partial class Pro_SellListInfo
    {
        //private Model.Sys_RoleInfo _Role;
        ///// <summary>
        ///// 角色 用于权限验证
        ///// </summary>
        //public Model.Sys_RoleInfo Role
        //{
        //    get { return _Role; }
        //    set { _Role = value; }
        //}

        private int _MenuID;
        /// <summary>
        /// 菜单ID 用于权限验证
        /// </summary>
        public int MenuID
        {
            get { return _MenuID; }
            set { _MenuID = value; }
        }
        //private List<Model.Pro_ProInfo> _ProS;
        ///// <summary>
        ///// 本次提交销售明细中所有的商品
        ///// </summary>
        //public List<Model.Pro_ProInfo> ProS
        //{
        //    get { return _ProS; }
        //    set { _ProS = value; }
        //}

        /// <summary>
        /// 验证单卖的有效性
        /// </summary>
        /// <param name="_Role"></param>
        /// <param name="_ProS"></param>
        /// <returns></returns>
        public virtual bool SellListInfoCheck(Model.Sys_UserInfo sysUser)
        {
            return true;
        }
        /// <summary>
        /// 用于LINQ识别该行是哪个继承类, 但不保存数据库
        /// </summary>
        [Column(IsDiscriminator = true, IsDbGenerated = true, AutoSync = AutoSync.Never, Name = "ClassType")]
        public  string ClassType_;
    }
}
