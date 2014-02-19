using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DAL
{
    public class Package_SalesNameInfo : Sys_InitParentInfo
    {
        private int MethodID;

        public Package_SalesNameInfo()
        {
            this.MethodID = 0;
        }

        public Package_SalesNameInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

       /// <summary>
        /// 获取套餐分类菜单  246
       /// </summary>
       /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user)
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               try
               {
                   var list = from a in lqh.Umsdb.View_PackageSalesNameInfo
                              select a;

                   return new Model.WebReturn() { ReturnValue = true, Obj = list.ToList() };
               }
               catch (Exception ex)
               {
                   return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
               }
           }
       }

        public List<Model.Package_SalesNameInfo> GetList(Model.Sys_UserInfo user,DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var list = from a in lqh.Umsdb.Package_SalesNameInfo
                               select a;

                    return  list.ToList();
                }
                catch (Exception ex)
                {
                    return new List<Model.Package_SalesNameInfo>();
                }
            }
        }

        #region 套餐分类菜单
        public Model.WebReturn GetModel(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    DataLoadOptions dl=new DataLoadOptions();
                    dl.LoadWith<Model.Package_SalesNameInfo>(p=>p.VIP_OffList);
                    dl.LoadWith<Model.VIP_OffList>(list => list.Package_GroupInfo);
                    dl.LoadWith<Model.Package_GroupInfo>(list=>list.Package_ProInfo);

                    lqh.Umsdb.LoadOptions = dl;

                    var query = (from b in lqh.Umsdb.GetTable<Model.Package_SalesNameInfo>()
                                 where b.ID >=4
                                 select b).ToList();
                    if (query == null || query.Count() == 0)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "获取失败" };
                    }
                    return new Model.WebReturn() { Obj = query, ReturnValue = true, Message = "获取成功" };
                }
                catch
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "获取失败" };
                }
            }
        }
        #endregion
       /// <summary>
       /// 更新
       /// </summary>
       /// <param name="user"></param>
       /// <param name="pac"></param>
       /// <returns></returns>
       public Model.WebReturn Update(Model.Sys_UserInfo user,Model.View_PackageSalesNameInfo pac) //249
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               using (TransactionScope ts = new TransactionScope())
               {
                   try
                   {
                       #region 权限验证

                       //用户权限
                       Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                       if (!result.ReturnValue)
                       { return result; }


                       //有权限的仓库
                       List<string> ValidHallIDS = new List<string>();
                       //有权限的商品
                       List<string> ValidProIDS = new List<string>();

                       Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                       if (ret.ReturnValue != true)
                       { return ret; }

                       #endregion 

                       var package = from a in lqh.Umsdb.Package_SalesNameInfo
                                     where a.ID == pac.ID
                                     select a;
                       if (package.Count() == 0)
                       {
                           return new Model.WebReturn() { ReturnValue = false,Message="没有找到当前套餐信息，保存失败！"};
                       }

                       //判断是否已经存在同名套餐分类
                       var pacx = from a in lqh.Umsdb.Package_SalesNameInfo
                                  where a.ID != pac.ID && a.SalesName == pac.SalesName
                                  select a;
                       if (pacx.Count() > 0)
                       {
                           return new Model.WebReturn() { ReturnValue = false,Message="已存在相同名称的套餐，保存失败！"};
                       }
                       Model.Package_SalesNameInfo model = package.First();
                       model.Note = pac.Note;
                       model.SalesName = pac.SalesName;
                       pac.Parent = model.Parent;
                       lqh.Umsdb.SubmitChanges();
                       ts.Complete();
                       
                       return new Model.WebReturn() { ReturnValue = true, Message = "保存成功！",Obj = pac };
                   }
                   catch (Exception ex)
                   {
                       return new Model.WebReturn() { ReturnValue=false,Message=ex.Message};
                   }
               }
           }
       }

      /// <summary>
      /// 添加
      /// </summary>
      /// <returns></returns>
       public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Package_SalesNameInfo model)//247
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               using (TransactionScope ts = new TransactionScope())
               {
                   try
                   {
                       #region 权限验证

                       //用户权限
                       Model.WebReturn result = ValidClassInfo.ValidateUser(user, lqh);

                       if (!result.ReturnValue)
                       { return result; }


                       //有权限的仓库
                       List<string> ValidHallIDS = new List<string>();
                       //有权限的商品
                       List<string> ValidProIDS = new List<string>();

                       Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                       if (ret.ReturnValue != true)
                       { return ret; }

                       #endregion 

                       //判断是否已经存在同名套餐分类
                       var pacx = from a in lqh.Umsdb.Package_SalesNameInfo
                                  where a.SalesName == model.SalesName
                                  select a;
                       if (pacx.Count() > 0)
                       {
                           return new Model.WebReturn() { ReturnValue = false, Message = "已存在相同名称的套餐，保存失败！" };
                       }

                       lqh.Umsdb.Package_SalesNameInfo.InsertOnSubmit(model);
                       lqh.Umsdb.SubmitChanges();
                       ts.Complete();
                       return new Model.WebReturn() { ReturnValue = true, Message = "添加成功！",Obj=model };
                   }
                   catch (Exception ex)
                   {
                       return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                   }
               }
           }
       }

       public Model.WebReturn Delete(Model.Sys_UserInfo user, List<int> list) //248
       {
           using (LinQSqlHelper lqh = new LinQSqlHelper())
           {
               using (TransactionScope ts = new TransactionScope())
               {
                   try
                   {
                         #region 权限验证
                       
                         //用户权限
                          Model.WebReturn result = ValidClassInfo.ValidateUser(user,lqh);

                          if (!result.ReturnValue)
                          { return result; }

                          
                          //有权限的仓库
                          List<string> ValidHallIDS = new List<string>();
                          //有权限的商品
                          List<string> ValidProIDS = new List<string>();

                         Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, MethodID, ValidHallIDS, ValidProIDS, lqh);

                        if (ret.ReturnValue != true)
                        { return ret; }

                        #endregion 

                       var package = from a in lqh.Umsdb.Package_SalesNameInfo
                                     where list.Contains( a.ID ) 
                                     select a;
                       if (package.Count() == 0)
                       {
                           return new Model.WebReturn() { ReturnValue =false,Message="未能找到指定套餐，删除失败！"};
                       }

                       //使用中的套餐分类无法删除
                       var usemodel = from a in lqh.Umsdb.VIP_OffList
                                      where list.Contains(a.Type ) &&
                                      a.Flag==true && a.EndDate > DateTime.Now
                                      select a;
                       if (usemodel.Count() != 0)
                       {
                           return new Model.WebReturn() {ReturnValue= false,Message="该套餐分类使用中无法删除，删除失败！" };
                       }

                       //Model.Package_SalesNameInfo pac = new Model.Package_SalesNameInfo();
                       //pac.ID = model.ID;
                       //pac.Note = model.Note;
                       //pac.SalesName = model.SalesName;
                       //pac.Parent = model.Parent;
                       //Model.Package_SalesNameInfo mm = package.First();
                       lqh.Umsdb.Package_SalesNameInfo.DeleteAllOnSubmit(package);
                       lqh.Umsdb.SubmitChanges();

                       ts.Complete();
                       return new Model.WebReturn() { ReturnValue = true, Message = "删除成功！",Obj = null };
                   }
                   catch (Exception ex)
                   {
                       return new Model.WebReturn() { ReturnValue = false, Message = ex.Message };
                   }
               }
           }
       }
    }
}
