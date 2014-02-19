using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Linq;
using System.Data.Linq;


namespace DAL
{
    /// <summary>
    /// 会员卡类别
    /// </summary>
    public class VIP_VIPType : Sys_InitParentInfo
    {
        private int MenthodID;

        public VIP_VIPType()
        {
            this.MenthodID = 0;
        }

        public VIP_VIPType(int MenthodID)
        {
            this.MenthodID = MenthodID;
        }
        #region 新增卡类型
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.VIP_VIPType model)
        {

            //插入类别信息
            //插入类别指定的服务信息 VIP_VIPTypeService   
            //返回
            //备份 VIP_VIPType_Bak ，VIP_VIPTypeService_BAK
            //更新类别信息
            //更新类别指定的服务信息 VIP_VIPTypeService
            //返回
            string Msg = "";
            if (model == null)
                return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "无参数" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {

                        #region  判断卡类别的名称是否存在
                        int query_Exist = (from b in lqh.Umsdb.VIP_VIPType
                                           where b.Name == model.Name && b.Flag == true
                                           select b).Count();
                        if (query_Exist > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "卡类别名称已存在！" };
                        }
                        #endregion
                        #region 判断服务
                        //存在服务
                        if (model.VIP_VIPTypeService != null)
                        {

                            #region 判断服务是否在商品列表中
                            List<string> ProIDGH = (from b in model.VIP_VIPTypeService
                                                    select b.ProID).ToList();

                            if (ProIDGH.Count() != model.VIP_VIPTypeService.Count())
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "获取商品ID出错！" };
                            }

                            var query_Pro = from b in lqh.Umsdb.Pro_ProInfo
                                            where ProIDGH.Contains(b.ProID) && b.IsService == true
                                            select b;
                            if (query_Pro == null || query_Pro.Count() != model.VIP_VIPTypeService.Count())
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "部分服务不存在！" };
                            }
                            #endregion
                            #region 验证数据有效性
                            var UnProCount = (from b in model.VIP_VIPTypeService
                                              where b.SCount == 0
                                              select b.SCount).ToList();
                            if (UnProCount.Count() > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "服务的数量不能为空！" };
                            }

                            #endregion
                            //不存在服务
                        }
                        #endregion
                        #region 添加新的卡类型和服务
                        model.UpdUser = user.UserID;
                        model.SysDate = DateTime.Now;
                        model.Flag = true;
                        lqh.Umsdb.VIP_VIPType.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "新增成功！" };
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "新增失败！" };
                        throw ex;
                    }

                }

            }
        }
        #endregion
        #region 修改卡类别资料
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.VIP_VIPType model)
        {
            //备份 VIP_VIPType_Bak ，VIP_VIPTypeService_BAK
            //更新类别信息
            //更新类别指定的服务信息 VIP_VIPTypeService
            //返回
            string Msg = "";
            if (model == null)
                return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "无参数" };
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        #region  判断卡类别的名称是否存在
                        int query_Exist = (from b in lqh.Umsdb.VIP_VIPType
                                           where b.Name == model.Name && b.Flag == true
                                           select b).Count();
                        if (query_Exist > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "卡类别名称已存在！" };
                        }
                        #endregion
                        #region 备份卡类型

                        Model.VIP_VIPType VIPtype = new Model.VIP_VIPType();
                        var query = (from b in lqh.Umsdb.VIP_VIPType
                                     from c in b.VIP_VIPTypeService
                                     where b.ID == model.ID
                                     select b).ToList();
                        if (query.Count() == 0)
                        {
                            var query_NoService = (from b in lqh.Umsdb.VIP_VIPType
                                                   where b.ID == model.ID
                                                   select b).ToList();
                            if (query_NoService.Count() == 0)
                            {
                                Msg = "卡类别" + model.Name + " 不存在";
                                return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = Msg };
                            }
                            VIPtype = query_NoService.First();
                        }
                        else
                            VIPtype = query.First();
                        Model.VIP_VIPType_Bak Type_Bak = new Model.VIP_VIPType_Bak();
                        Type_Bak.Cost_production = VIPtype.Cost_production;
                        Type_Bak.Flag = VIPtype.Flag;
                        Type_Bak.OldID = VIPtype.ID;
                        Type_Bak.Name = VIPtype.Name;
                        Type_Bak.Note = VIPtype.Note;
                        Type_Bak.SBalance = VIPtype.SBalance;
                        Type_Bak.SPoint = VIPtype.SPoint;
                        Type_Bak.SysDate = DateTime.Now;
                        Type_Bak.UpdUser = VIPtype.UpdUser;
                        Type_Bak.Validity = VIPtype.Validity;
                        #endregion
                        #region 备份卡类型服务
                        if (VIPtype.VIP_VIPTypeService != null)
                        {
                            List<Model.VIP_VIPTypeService_BAK> Service_Bak = new List<Model.VIP_VIPTypeService_BAK>();
                            foreach (var ServiceItem in VIPtype.VIP_VIPTypeService)
                            {
                                Model.VIP_VIPTypeService_BAK bak = new Model.VIP_VIPTypeService_BAK();
                                bak.OldID = ServiceItem.ID;
                                bak.ProID = ServiceItem.ProID;
                                bak.SCount = ServiceItem.SCount;
                                bak.SysDate = DateTime.Now;
                                bak.TypeID = ServiceItem.TypeID;
                                bak.UpdUser = user.UserID;
                                Service_Bak.Add(bak);
                            }
                            lqh.Umsdb.VIP_VIPTypeService_BAK.InsertAllOnSubmit(Service_Bak);
                        }
                        #endregion
                        #region  修改卡类型
                        VIPtype.Cost_production = model.Cost_production;
                        if (model.Name != null)
                            VIPtype.Name = model.Name;
                        VIPtype.Note = model.Note;
                        VIPtype.SBalance = model.SBalance;
                        VIPtype.SPoint = model.SPoint;
                        VIPtype.SysDate = DateTime.Now;
                        VIPtype.UpdUser = user.UserID;
                        VIPtype.Validity = model.Validity;
                        #endregion
                        #region 更新旧服务
                        VIPtype.VIP_VIPTypeService = model.VIP_VIPTypeService;
                        #endregion
                        #region 判断服务
                        //存在服务
                        if (model.VIP_VIPTypeService != null)
                        {

                            #region 判断服务是否在商品列表中
                            List<string> ProIDGH = (from b in model.VIP_VIPTypeService
                                                    select b.ProID).ToList();

                            if (ProIDGH.Count() != model.VIP_VIPTypeService.Count())
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "获取商品ID出错！" };
                            }

                            var query_Pro = from b in lqh.Umsdb.Pro_ProInfo
                                            where ProIDGH.Contains(b.ProID) && b.IsService == true
                                            select b;
                            if (query_Pro == null || query_Pro.Count() != model.VIP_VIPTypeService.Count())
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "部分服务不存在！" };
                            }
                            #endregion
                            #region 验证数据有效性
                            var UnProCount = (from b in model.VIP_VIPTypeService
                                              where b.SCount == 0
                                              select b.SCount).ToList();
                            if (UnProCount.Count() > 0)
                            {
                                return new Model.WebReturn() { ReturnValue = false, Message = "服务的数量不能为空！" };
                            }

                            #endregion
                            //不存在服务
                        }
                        #endregion
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "更新成功！" };

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "更新失败！" };
                        throw ex;
                    }

                }

            }
        }
        #endregion
        #region 获取所有类别
        public Model.WebReturn GetModel(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = (from b in lqh.Umsdb.GetTable<Model.VIP_VIPType>()
                                 where b.Flag == true
                                 select b).ToList();
                    if (query == null || query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "获取失败" };
                    }
                    return new Model.WebReturn() { Obj = query, ReturnValue = true, Message = "获取成功" };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取失败" };
                }
            }
        }
        #endregion
        #region 初始化数据
        public List<Model.VIP_VIPType> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query = from b in lqh.Umsdb.GetTable<Model.VIP_VIPType>()
                                where b.Flag == true
                                select b;
                    //if (query == null || query.Count() == 0)
                    //{
                    //    return null;
                    //}
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.VIP_VIPType>();
                }
            }
        }
        #endregion
        #region 删除卡类别
        public Model.WebReturn Delete(Model.Sys_UserInfo user, Model.VIP_VIPType model)
        {
            string Msg = "";
            if (model == null)
                return new Model.WebReturn() {  ReturnValue = false, Message = "无参数" };
            if (user.RoleID != 1)
            {
                return new Model.WebReturn() { ReturnValue = false, Message = "需要系统管理员身份才能删除" };
            }
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        DataLoadOptions dataload = new DataLoadOptions();
                        dataload.LoadWith<Model.VIP_VIPType>(p => p.VIP_VIPTypeService);
                        lqh.Umsdb.LoadOptions = dataload;
                        #region  判断卡类别是否已经使用
                        int query_Exist = (from b in lqh.Umsdb.Pro_ProInfo
                                           join c in lqh.Umsdb.VIP_VIPType on b.VIP_TypeID equals c.ID
                                           where c.ID == model.ID
                                           select b).Count();
                        if (query_Exist > 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "已使用的卡类别不能删除！" };
                        }
                        #endregion
                        #region 删除卡类别
                        var query = from b in lqh.Umsdb.VIP_VIPType
                                  //  from c in b.VIP_VIPTypeService
                                    where b.ID == model.ID
                                    select b;
                        #region 验证数据有效性
                        if (query.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "不存在卡类别！" };
                        }
                        #endregion
                        #region 添加卡类型备份
                        Model.VIP_VIPType OldType = query.First();
                        Model.VIP_VIPType_Bak Bak = new Model.VIP_VIPType_Bak();
                        Bak.Cost_production = OldType.Cost_production;
                        Bak.Flag = false;
                        Bak.Name = OldType.Name;
                        Bak.Note = OldType.Note;
                        Bak.OldID = OldType.ID;
                        Bak.SBalance = OldType.SBalance;
                        Bak.SPoint = OldType.SPoint;
                        Bak.SysDate = DateTime.Now;
                        Bak.UpdUser = user.UserID;
                        Bak.Validity = OldType.Validity;
                        #endregion
                        #region 添加卡类型服务备份
                        if (OldType.VIP_VIPTypeService != null)
                        {
                            foreach (var Item in OldType.VIP_VIPTypeService)
                            {
                                Model.VIP_VIPTypeService_BAK Service_Bak = new Model.VIP_VIPTypeService_BAK();
                                Service_Bak.OldID = Item.ID;
                                Service_Bak.ProID = Item.ProID;
                                Service_Bak.SCount = Item.SCount;
                                Service_Bak.SysDate = DateTime.Now;
                                Service_Bak.TypeID = Item.TypeID;
                                Service_Bak.UpdUser = user.UserID;
                                if (Bak.VIP_VIPTypeService_BAK == null)
                                    Bak.VIP_VIPTypeService_BAK = new System.Data.Linq.EntitySet<Model.VIP_VIPTypeService_BAK>();
                                Bak.VIP_VIPTypeService_BAK.Add(Service_Bak);
                            }
                        }
                        #endregion
                        #region 完成操作
                        lqh.Umsdb.VIP_VIPType.DeleteOnSubmit(OldType);
                        lqh.Umsdb.VIP_VIPType_Bak.InsertOnSubmit(Bak);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "删除成功！" };
                        #endregion
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "删除失败！" };
                        throw ex;
                    }

                }
            }
        }
        #endregion
    }
}
