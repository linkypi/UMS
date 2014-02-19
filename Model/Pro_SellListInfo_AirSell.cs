using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 空充
    /// </summary>
    public class Pro_SellListInfo_AirSell : Pro_SellListInfo
    {
        public override bool SellListInfoCheck(Model.Sys_UserInfo sysUser)
        {
            this.MenuID = 102;//单卖

            Model.UMSDB m = new UMSDB();
            var q = from b in m.Sys_Role_Menu_ProInfo
                    where b.RoleID==sysUser.RoleID && b.MenuID == this.MenuID && b.Pro_ClassInfo!=null 
                    && b.Pro_ClassInfo.Pro_ProInfo.Any(p => p.ProID == this.ProID)
                    select b;
            if (q.Count() == 0)
            {
                return false;
            }
            return true;
        }

        
    }
}
