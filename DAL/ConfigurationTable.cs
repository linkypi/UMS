using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class ConfigurationTable
    {
        private int MenthodID;
	    public ConfigurationTable()
	    {
		    this.MenthodID = 0;
	    }

        public ConfigurationTable(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }
        /// <summary>
        /// 修改配置表
        /// </summary>
        /// <param name="hallModel"></param>
        /// <param name="areaModel"></param>
        /// <param name="levelModel"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user,Model.Pro_HallInfo hallModel,Model.Pro_AreaInfo areaModel,Model.Pro_LevelInfo levelModel,
            Model.Pro_TypeInfo typeModel,Model.Pro_ClassInfo classModel,Model.Pro_ProInfo proModel)
        {
            
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //修改门店
                    if (hallModel != null)
                    {
                        var query = from b in lqh.Umsdb.Pro_HallInfo
                                    where b.HallID == hallModel.HallID
                                    select b;
                        if (query != null&&query.Count()!=0)
                        {
                            Model.Pro_HallInfo NewhallModel=query.First();
                            NewhallModel.HallName = hallModel.HallName;
                            NewhallModel.LevelID = hallModel.LevelID;
                            NewhallModel.Note = hallModel.Note;
                            NewhallModel.Order = hallModel.Order;
                            NewhallModel.AreaID = hallModel.AreaID;
                            NewhallModel.CanBack = hallModel.CanBack;
                            NewhallModel.CanIn = hallModel.CanIn;
                            NewhallModel.Flag = hallModel.Flag;
                            lqh.Umsdb.SubmitChanges();
                            return new Model.WebReturn() { Obj = NewhallModel, ReturnValue=true, Message="已更新" };
                        }
                    }
                    //修改区域
                    if (areaModel != null)
                    {
                        var query = from b in lqh.Umsdb.Pro_AreaInfo
                                    where b.AreaID == areaModel.AreaID
                                    select b;
                        if (query != null && query.Count() != 0)
                        {
                            Model.Pro_AreaInfo NewAreaModel = query.First();
                            NewAreaModel.AreaName = areaModel.AreaName;
                            NewAreaModel.Flag= areaModel.Flag;
                            NewAreaModel.Note = areaModel.Note;
                            NewAreaModel.Order = areaModel.Order;
                            lqh.Umsdb.SubmitChanges();
                            return new Model.WebReturn() { Obj = NewAreaModel, ReturnValue = true, Message = "已更新" };
                        }
                    }
                    //修改等级
                    if (levelModel != null)
                    {
                        var query = from b in lqh.Umsdb.Pro_LevelInfo
                                    where b.LevelID ==levelModel.LevelID
                                    select b;
                        if (query != null && query.Count() != 0)
                        {
                            Model.Pro_LevelInfo NewlevelModel = query.First();
                            NewlevelModel.LevelName = levelModel.LevelName;
                            NewlevelModel.Flag = levelModel.Flag;
                            NewlevelModel.Note = levelModel.Note;
                            NewlevelModel.Order = levelModel.Order;
                            lqh.Umsdb.SubmitChanges();
                            return new Model.WebReturn() { Obj = NewlevelModel, ReturnValue = true, Message = "已更新" };
                        }
                    }
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "更新失败" };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "更新失败" };
                }
            }
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="hallModel"></param>
        /// <param name="areaModel"></param>
        /// <param name="levelModel"></param>
        /// <param name="typeModel"></param>
        /// <param name="classModel"></param>
        /// <param name="proModel"></param>
        /// <returns></returns>
        public Model.WebReturn Add(Model.Pro_HallInfo hallModel, Model.Pro_AreaInfo areaModel, Model.Pro_LevelInfo levelModel,
      Model.Pro_TypeInfo typeModel, Model.Pro_ClassInfo classModel, Model.Pro_ProInfo proModel)
        {

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    //新增门店
                    if (hallModel != null)
                    {
                        var query = from b in lqh.Umsdb.Pro_HallInfo
                                    where b.HallName== hallModel.HallName
                                    select b;
                        if (query.Count()>0)
                        {     
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "已存在" };
                        }
                        lqh.Umsdb.Pro_HallInfo.InsertOnSubmit(hallModel);
                        lqh.Umsdb.SubmitChanges();
                        return new Model.WebReturn() { Obj = hallModel, ReturnValue = false, Message = "添加成功" };
                    }
                    //新增区域
                    if (areaModel != null)
                    {
                        var query = from b in lqh.Umsdb.Pro_AreaInfo
                                    where b.AreaName == areaModel.AreaName
                                    select b;
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "已存在" };
                        }
                        lqh.Umsdb.Pro_AreaInfo.InsertOnSubmit(areaModel);
                        lqh.Umsdb.SubmitChanges();
                        return new Model.WebReturn() { Obj = areaModel, ReturnValue = false, Message = "添加成功" };
                    }
                    //新增等级
                    if (levelModel != null)
                    {
                        var query = from b in lqh.Umsdb.Pro_LevelInfo
                                    where b.LevelName == levelModel.LevelName
                                    select b;
                        if (query.Count() > 0)
                        {
                            return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "已存在" };
                        }
                        lqh.Umsdb.Pro_LevelInfo.InsertOnSubmit(levelModel);
                        lqh.Umsdb.SubmitChanges();
                        return new Model.WebReturn() { Obj = levelModel, ReturnValue = false, Message = "添加成功" };
                    }
                 
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "添加失败" };
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = "添加失败" };
                }
            }
        }

    }
}
