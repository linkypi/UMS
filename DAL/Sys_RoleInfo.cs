using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Collections;
using Model;
using System.Data.Linq;

namespace DAL
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Sys_RoleInfo : Sys_InitParentInfo
    {

        private List<Model.ReportSqlParams> _paramList = new List<Model.ReportSqlParams>() { 
       
            new Model.ReportSqlParams_String(){ParamName="RoleName"}, 
            new Model.ReportSqlParams_String(){ParamName="Note"}
        };
        public List<Model.ReportSqlParams> ParamList
        {
            get { return _paramList; }
            set { _paramList = value; }
        }

        private int MenthodID;

	    public Sys_RoleInfo()
	    {
		    this.MenthodID = 0;
	    }

        public Sys_RoleInfo(int MenthodID)
	    {
		    this.MenthodID = MenthodID;
	    }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
    
        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Sys_RoleInfo model,List< Model.Sys_RoleMethod> role_menu_method)
        {
          

            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List
       
            //插入Sys_RoleInfo Sys_RoleMethod 
            //
            //返回
            if (model == null) return new Model.WebReturn();
            string Msg = "";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                      
                        var query = from b in lqh.Umsdb.Sys_RoleInfo
                                    where b.RoleName == model.RoleName
                                    select b;
                        if (query != null && query.Count() > 0)
                        {
                            Msg = "角色名 " + model.RoleName + " 已经存在";
                            return new Model.WebReturn() { ReturnValue = false, Message = Msg };
                        }
                     

                        List<Model.Sys_MenuInfo> menuinfo = new List<Model.Sys_MenuInfo>();
                       List<int> parent_list=new List<int>();
                        model.Menu_ID_List = "/";
                        model.Method_ID_List = "/";
                        model.MenuXML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<root>\n";                                       
                        //获取方法 生成 Method_ID_List 
                       
                        if (role_menu_method != null && role_menu_method.Count > 0)
                        {
                            foreach (var m in role_menu_method)
                            {
                                //m.RoleID = model.RoleID;
                                var query_menu = from b in lqh.Umsdb.Sys_MenuInfo
                                                 where b.MenuID== m.MenuID
                                                 select b;
                                if (query_menu == null && query_menu.Count() == 0)
                                {
                                    Msg = "菜单 " + m.MethodID + " 不存在";
                                    return new Model.WebReturn() { ReturnValue = false, Message = Msg };
                                }
              
                                menuinfo.AddRange(query_menu);//保存查询到的菜单列表
                               
                                model.Menu_ID_List += m.MenuID + "/";
                                var query_method = from b in lqh.Umsdb.Sys_MethodInfo
                                                   where b.MethodID == m.MethodID
                                                   select b;
                                if (query_method == null && query_method.Count() == 0)
                                {
                                    Msg = "方法" + m.MethodID + "不存在";
                                    return new Model.WebReturn() { ReturnValue = false, Message = Msg };

                                }
                                model.Method_ID_List += m.MethodID + "/";
                            }                                               
                           
                        }
                        //从菜单列表中搜索具有相同父节点的子节点，保存在 arraylist 中                        
                        //model.MenuXML += GetNode(0, menuinfo);
                        model.MenuXML += "</root>";
                        model.Flag = true;
                        lqh.Umsdb.Sys_RoleInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges();
                        lqh.Umsdb.Sys_RoleMethod.InsertAllOnSubmit(role_menu_method);
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "新增成功" };

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                        throw ex;
                    }

                }

            }
           
        }

        public Model.WebReturn Add(Model.Sys_UserInfo user, Model.Sys_RoleInfo model)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List

            //插入Sys_RoleInfo Sys_RoleMethod 
            //
            //返回
            Model.WebReturn r = null;
            if (model == null) return new Model.WebReturn();
            string Msg = "";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        DataLoadOptions dataload = new DataLoadOptions();
                        dataload.LoadWith<Model.MenuInfo>(c => c.Sys_MethodInfo); 
                        lqh.Umsdb.LoadOptions = dataload;

                        #region 验证操作权限
                        r= ValidClassInfo.GetHall_ProIDFromRole(user,this.MenthodID,new List<string>(), new List<string>(),lqh);
                        if (r.ReturnValue == false) return r;
                        #endregion

                        #region 验证用户
                        var query_user = from b in lqh.Umsdb.Sys_UserInfo
                                         where b.UserID == user.UserID
                                         select b;
                        if (query_user.Count() == 0)
                        {
                            Msg = "用户 " + model.Updater + " 不存在";
                            return new Model.WebReturn() { ReturnValue = false, Message = Msg };
                        }
                        model.Updater = user.UserID;
                        #endregion


                        var query = from b in lqh.Umsdb.Sys_RoleInfo
                                    where b.RoleName == model.RoleName
                                    select b;

                        if (query != null && query.Count() > 0)
                        {
                            Msg = "角色名 " + model.RoleName + " 已经存在";
                            return new Model.WebReturn() { ReturnValue = false, Message = Msg };
                        }
                        #region 未选任何菜单
                        if (model.Sys_Role_MenuInfo == null || model.Sys_Role_MenuInfo.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未选任何菜单" };
                        }
                        #endregion

                        List<Model.Sys_MenuInfo> menuinfo = new List<Model.Sys_MenuInfo>();
                        List<int> parent_list = new List<int>();
                        model.Menu_ID_List = "/";
                        model.Method_ID_List = "/";
                        model.MenuXML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<root>\r\n";
                        //获取方法 生成 Method_ID_List 
                        #region 全部商品 全部仓库 菜单
                        //var all_pro = (from b in lqh.Umsdb.Pro_ProInfo
                        //              select b).ToList();
                        //var all_hall = (from b in lqh.Umsdb.Pro_HallInfo
                        //               select b).ToList();
                        //var all_menu = (from b in lqh.Umsdb.Sys_MenuInfo
                        //                select b).ToList();
                        List<int?> menuStr = (from b in model.Sys_Role_MenuInfo select b.MenuID).ToList();

                        var menu_list = (from b in lqh.Umsdb.Sys_MenuInfo
                                         where menuStr.Contains(b.MenuID)
                                         select b).ToList();

                        #endregion
                        #region 左连接

                        var join_list = from b in model.Sys_Role_MenuInfo
                                        join c in menu_list
                                        on b.MenuID equals c.MenuID
                                        into temp1
                                        from c1 in temp1.DefaultIfEmpty()
                                        select new { 
                                           Sys_Role_MenuInfo= b,
                                           Sys_MenuInfo=c1

                                        };

                        #endregion
                        bool noError = true;

                            foreach (var m in join_list)
                            {
                                
                                if (m.Sys_MenuInfo==null)
                                {
                                    noError = false;
                                    Msg = "菜单 " + m.Sys_MenuInfo.MenuID + " 不存在";
                                    m.Sys_MenuInfo.Note = Msg;
                                    continue;
                                }

                                menuinfo.Add(m.Sys_MenuInfo);//保存查询到的菜单列表 
                                
                                model.Menu_ID_List += m.Sys_MenuInfo.MenuID + "/";

                                string[] s = (from b in m.Sys_MenuInfo.Sys_MethodInfo
                                                  select b.MethodID+"").ToArray();


                                model.Method_ID_List += string.Join("/", s)+"/";
                            }
                            if (!noError)
                            {
                                return new WebReturn() {  ReturnValue=false, Obj=model};
                            }
                        
                        //从菜单列表中搜索具有相同父节点的子节点，保存在 arraylist 中        
                        List<APIModel.MenuList> MenuForJson = new List<APIModel.MenuList>();

                        model.MenuXML += GetNode(0, menuinfo.Distinct().ToList(), MenuForJson);
                        model.MenuXML += "</root>";
                        model.Flag = true;
                        model.Updater = model.Updater;
                        model.UpDateTime = DateTime.Now;
                        JsonFx.Json.JsonWriter write=new JsonFx.Json.JsonWriter();
                        if (MenuForJson.Count() > 0)
                        {
                            model.MobileMenuJson = write.Write(MenuForJson.First().child);

                        }
                        else
                            model.MobileMenuJson = "[]";
                        lqh.Umsdb.Sys_RoleInfo.InsertOnSubmit(model);
                        lqh.Umsdb.SubmitChanges(); 
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "新增成功" };

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false };
                        throw ex;
                    }

                }

            }
        }
        public static string GetNode(int parent, List<Model.Sys_MenuInfo> dtLV ,List<APIModel.MenuList> MenuForJson)
        {
            string xml = "";
            //System.Data.DataRow[] drs = dtLV.Select("pmenulv=" + menuLV);
            //System.Data.DataRow[] drs_menu;
            List<Model.Sys_MenuInfo> drs = (dtLV.Where<Model.Sys_MenuInfo>(p => p!=null && p.Parent == parent)).ToList();
         //   List<Model.Sys_MenuInfo> drs_menu = null;
            //带子节点的节点
            
            foreach (Model.Sys_MenuInfo dr in drs)
            {
                
                APIModel.MenuList MenuForJsonOne = new APIModel.MenuList() { 
                
                image=dr.MenuImg,
                menuid=dr.MenuID+"",
                menuname=dr.MenuValue,
                title=dr.MenuText
                };
                if (!dr.DisplayMobile)
                {
                    if (string.IsNullOrEmpty(dr.MenuValue))
                    {
                        xml += "<node  name='" + dr.MenuText + "' address=''>\r\n";
                    }
                    else
                    {
                        if ((dr.MenuValue + "").IndexOf("?") >= 0)
                            xml += "<node  name='" + dr.MenuText + "' address='" + dr.MenuValue + "&amp;MenuID=" + dr.MenuID + "'>\r\n";
                        else
                            xml += "<node  name='" + dr.MenuText + "' address='" + dr.MenuValue + "?&amp;MenuID=" + dr.MenuID + "'>\r\n";
                    }
                    xml += GetNode(dr.MenuID, dtLV, MenuForJsonOne.child);
                    xml += "</node>\r\n";
                }
                else
                {
                    GetNode(dr.MenuID, dtLV, MenuForJsonOne.child);
                    MenuForJson.Add(MenuForJsonOne);
                }
                
                
                

            }
            //不带子节点的页面
            //drs_menu = dtMenu.Where(p => p.Parent ==parent).ToList();
            //foreach (Model.Sys_MenuInfo dr_menu in drs_menu)
            //{
            //    xml += "<node id='" + dr_menu.MenuID + "' name='" + dr_menu.MenuText + "' address='" + (dr_menu.MenuValue + "").Replace("&", "&amp;") + "&amp;menuid=" + dr_menu.MenuID;

            //    xml += "'/>\n";

             
            //}

            return xml;
        }
        //public static string GetAllMenuMethodXML()
        //{
        //    string MenuXML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<Tree>\n";
        //    //SqlDAL.Sys_MenuLVInfo m = new SqlDAL.Sys_MenuLVInfo();
        //    //SqlDAL.DALReturn r = m.GetList(true);
        //    List<Model.Sys_MenuInfo> lv_list = new List<Model.Sys_MenuInfo>();
        //    if (r.Ex == null && r.Obj != null)
        //    {
        //        lv_list = ((IQueryable<Model.Sys_MenuInfo>)r.Obj).ToList();
        //    }
        //   Model.Sys_MenuInfo m2 = new Model.Sys_MenuInfo();
        //    r = m2.GetList(true);
        //    List<Model.Sys_MenuInfo> menu_list = new List<Model.Sys_MenuInfo>();
        //    if (r.Ex == null && r.Obj != null)
        //    {
        //        menu_list = ((IQueryable<Model.Sys_MenuInfo>)r.Obj).ToList();
        //    }
        //    Model.Sys_MethodInfo m3 = new Model.Sys_MethodInfo();
        //    r = m3.GetList();
        //    List<Model.Sys_MethodInfo> method_list = new List<Model.Sys_MethodInfo>();
        //    if (r.Ex == null && r.Obj != null)
        //    {
        //        method_list = ((IQueryable<Model.Sys_MethodInfo>)r.Obj).ToList();
        //    }
        //    MenuXML += GetAllMenuMethodXML(0, lv_list, menu_list, method_list);
        //    MenuXML += "</Tree>";

        //    return MenuXML;
        //}

        //public static string GetAllMenuMethodXML(int menuLV, List<Model.Sys_MenuInfo> dtLV, List<Model.Sys_MenuInfo> dtMenu, List<Model.Sys_MethodInfo> dtMethod)
        //{
        //    string xml = "";
        //    List<Model.Sys_MenuInfo> drs = (dtLV.Where<Model.Sys_MenuInfo>(p => p.PMenuLv == menuLV)).ToList();
        //    List<Model.Sys_MenuInfo> drs_menu = null;
        //    //带子节点的节点

        //    foreach (Model.Sys_MenuInfo dr in drs)
        //    {
        //        xml += "<Node Value='' Text='" + dr.MenuLVText + "' ToolTip='' Expanded='true'>\n";
        //        xml += GetAllMenuMethodXML(dr.MenuLV, dtLV, dtMenu, dtMethod);
        //        xml += "</Node>\n";

        //    }
        //    //不带子节点的页面
        //    drs_menu = (dtMenu.Where(p => p.MenuLv == menuLV)).ToList();
        //    foreach (Model.Sys_MenuInfo dr_menu in drs_menu)
        //    {
        //        if (dr_menu.MenuID == 70)
        //        {

        //        }
        //        xml += "<Node Value='" + dr_menu.MenuID + "' Text='" + dr_menu.MenuText + "' ToolTip='0'";
        //        xml += "  Expanded='false'>\n";
        //        List<Maticsoft.Model.Sys_MethodInfo> temp = dtMethod.FindAll(p => (dr_menu.Method_ID_List + "").IndexOf("/" + p.MethodID + "/") >= 0);
        //        foreach (Maticsoft.Model.Sys_MethodInfo method in temp)
        //        {
        //            xml += "<Node Text='" + method.Name + "'  Expanded='false' Value='" + method.MethodID + "' ToolTip='1'/>";
        //        }
        //        xml += "</Node>\n";

        //    }
        //    return xml;
        //}

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Model.WebReturn Update(Model.Sys_UserInfo user, Model.Sys_RoleInfo model)
        {
            //验证权限是否重复 菜单id 方法id 门店id 类别id
            //获取菜单生成XML  MenuXML  Menu_ID_List

            //插入Sys_RoleInfo Sys_RoleMethod 
            //
            //返回
            Model.WebReturn r = null;
            if (model == null) return new Model.WebReturn();
            string Msg = "";
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        //DataLoadOptions dataload = new DataLoadOptions();
                        //dataload.LoadWith<Model.MenuInfo>(c => c.Sys_MethodInfo);
                        //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_MenuInfo);
                        //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_Menu_ProInfo);
                        //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_Menu_HallInfo);
                        //lqh.Umsdb.LoadOptions = dataload;

                        #region 验证操作权限
                        r = ValidClassInfo.GetHall_ProIDFromRole(user, this.MenthodID, new List<string>(), new List<string>(),lqh);
                        if (r.ReturnValue == false) return r;
                        #endregion
                        #region 验证用户

                        var query_user = from b in lqh.Umsdb.Sys_UserInfo
                                         where b.UserID == user.UserID
                                         select b;
                        if (query_user.Count() == 0)
                        {
                            Msg = "用户 " + model.Updater + " 不存在";
                            return new Model.WebReturn() { ReturnValue = false, Message = Msg };
                        }
                        model.Updater = user.UserID;
                        #endregion

                        var query = (from b in lqh.Umsdb.Sys_RoleInfo
                                    where b.RoleName == model.RoleName || b.RoleID==model.RoleID
                                    select b).ToList();

                        if(query.Where(p=>p.RoleID==model.RoleID).Count()==0)
                        {
                            Msg = "角色名 " + model.RoleID + " 不存在";
                            return new Model.WebReturn() { ReturnValue = false, Message = Msg };
                        }
                        Model.Sys_RoleInfo role_First = query.Where(p => p.RoleID == model.RoleID).First();

                        if (query.Where(p => p.RoleID != model.RoleID && (p.RoleName+"").ToLower() ==( model.RoleName+"").ToLower()).Count() > 0)
                        {
                            Msg = "角色名 " + model.RoleName + " 已经存在";
                            return new Model.WebReturn() { ReturnValue = false, Message = Msg };
                        }
                        #region 未选任何菜单
                        if (model.Sys_Role_MenuInfo == null || model.Sys_Role_MenuInfo.Count() == 0)
                        {
                            return new Model.WebReturn() { ReturnValue = false, Message = "未选任何菜单" };
                        }
                        #endregion

                        #region 保存日志
                        

                        Model.Sys_RoleInfo_back role_bak = new Sys_RoleInfo_back() { 
                            Flag=role_First.Flag,
                            Menu_ID_List=role_First.Menu_ID_List,
                            MenuXML=role_First.MenuXML,
                            Method_ID_List=role_First.Method_ID_List,
                            Note=role_First.Note,
                            RoleID=role_First.RoleID,
                            RoleName=role_First.RoleName,
                            Updater=role_First.Updater,
                            UpdateTime=role_First.UpDateTime
                        };
                        //lqh.Umsdb.Sys_RoleInfo_back.InsertOnSubmit(role_bak);
                        #region Sys_Role_MenuInfo_bak

                        var Role_menuInfo = (from b in lqh.Umsdb.Sys_Role_MenuInfo
                                             where b.RoleID == role_First.RoleID
                                             select b).ToList();

                        var Role_MenuInfo_bak = from b in Role_menuInfo
                                                   select new Model.Sys_Role_MenuInfo_bak{
                                                   MenuID=b.MenuID,
                                                   Note=b.Note,
                                                   RoleID=b.RoleID,
                                                   Sys_RoleInfo_back=role_bak
                                                   };
                        
                        lqh.Umsdb.Sys_Role_MenuInfo.DeleteAllOnSubmit(Role_menuInfo);
                        //lqh.Umsdb.ExecuteCommand("delete from Sys_Role_MenuInfo where roleid=" + role_First.RoleID, new object[] { });
                        
                        #endregion

                        #region dbo.Sys_Role_Menu_HallInfo_bak
                        var role_menu_hall = (from b in lqh.Umsdb.Sys_Role_Menu_HallInfo
                                              where b.RoleID == role_First.RoleID
                                              select b).ToList(); 
                        
                        var Role_Menu_HallInfo_bak = from b in role_menu_hall
                                                     select new Model.Sys_Role_Menu_HallInfo_bak
                                                {
                                                    MenuID = b.MenuID,
                                                    Note = b.Note,
                                                    RoleID = b.RoleID,
                                                    HallID=b.HallID,
                                                    Sys_RoleInfo_back = role_bak
                                                }; 
                        lqh.Umsdb.Sys_Role_Menu_HallInfo.DeleteAllOnSubmit(role_menu_hall);
                        //lqh.Umsdb.ExecuteCommand("delete from Sys_Role_Menu_HallInfo where roleid="+ role_First.RoleID , new object[]{});
                        #endregion

                        #region dbo.Sys_Role_Menu_ProInfo_bak
                        var role_menu_pro =( from b in lqh.Umsdb.Sys_Role_Menu_ProInfo
                                             where b.RoleID == role_First.RoleID
                                             select b).ToList();
                        var Role_Menu_ProInfo_bak = from b in role_menu_pro
                                                    select new Model.Sys_Role_Menu_ProInfo_bak
                                                     {
                                                         MenuID = b.MenuID,
                                                         Note = b.Note,
                                                         RoleID = b.RoleID,
                                                         ClassID = b.ClassID,
                                                         Sys_RoleInfo_back = role_bak
                                                     }; 
                        lqh.Umsdb.Sys_Role_Menu_ProInfo.DeleteAllOnSubmit(role_menu_pro);
                        //lqh.Umsdb.ExecuteCommand("delete from Sys_Role_Menu_ProInfo where roleid=" + role_First.RoleID, new object[] { });
                        
                        #endregion
                        


                        lqh.Umsdb.Sys_RoleInfo_back.InsertOnSubmit(role_bak);
                        lqh.Umsdb.SubmitChanges();
                        #endregion

                        
                        

                        List<Model.Sys_MenuInfo> menuinfo = new List<Model.Sys_MenuInfo>();
                        List<int> parent_list = new List<int>();
                        role_First.RoleName = model.RoleName;
                        role_First.Note = model.Note;
                        role_First.Updater = user.UserID;
                        role_First.UpDateTime = DateTime.Now;
                        role_First.Menu_ID_List = "/";
                        role_First.Method_ID_List = "/";
                        role_First.MenuXML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<root>\r\n";
                        //获取方法 生成 Method_ID_List 
                        #region 全部商品 全部仓库 菜单
                        //var all_pro = (from b in lqh.Umsdb.Pro_ProInfo
                        //              select b).ToList();
                        //var all_hall = (from b in lqh.Umsdb.Pro_HallInfo
                        //               select b).ToList();
                        //var all_menu = (from b in lqh.Umsdb.Sys_MenuInfo
                        //                select b).ToList();
                        List<int?> menuStr = (from b in model.Sys_Role_MenuInfo select b.MenuID).ToList();

                        var menu_list = (from b in lqh.Umsdb.Sys_MenuInfo
                                         where menuStr.Contains(b.MenuID)
                                         select b).ToList();

                        #endregion
                        #region 左连接

                        var join_list = from b in model.Sys_Role_MenuInfo
                                        join c in menu_list
                                        on b.MenuID equals c.MenuID
                                        into temp1
                                        from c1 in temp1.DefaultIfEmpty()
                                        select new
                                        {
                                            Sys_Role_MenuInfo = b,
                                            Sys_MenuInfo = c1

                                        };

                        #endregion
                        bool noError = true;

                        foreach (var m in join_list)
                        {

                            if (m.Sys_MenuInfo == null)
                            {
                                noError = false;
                                Msg = "菜单 " + m.Sys_MenuInfo.MenuID + " 不存在";
                                m.Sys_MenuInfo.Note = Msg;
                                continue;
                            }

                            menuinfo.Add(m.Sys_MenuInfo);//保存查询到的菜单列表 
                            role_First.Menu_ID_List += m.Sys_MenuInfo.MenuID + "/";

                            string[] s = (from b in m.Sys_MenuInfo.Sys_MethodInfo
                                          select b.MethodID + "").ToArray();


                            role_First.Method_ID_List += string.Join("/", s) + "/";
                        }
                        if (!noError)
                        {
                            return new WebReturn() { ReturnValue = false, Obj = model };
                        }

                        //从菜单列表中搜索具有相同父节点的子节点，保存在 arraylist 中        
                        List<APIModel.MenuList> MenuForJson = new List<APIModel.MenuList>();
                        role_First.MenuXML += GetNode(0, menuinfo.Distinct().ToList(), MenuForJson);
                        role_First.MenuXML += "</root>";
                        JsonFx.Json.JsonWriter write = new JsonFx.Json.JsonWriter();
                        if (MenuForJson.Count() > 0)
                        {
                            role_First.MobileMenuJson = write.Write(MenuForJson.First().child);

                        }
                        else
                            role_First.MobileMenuJson = "[]";
                        role_First.Sys_Role_Menu_HallInfo = model.Sys_Role_Menu_HallInfo; 
                        role_First.Sys_Role_Menu_ProInfo=model.Sys_Role_Menu_ProInfo;
                        role_First.Sys_Role_MenuInfo = model.Sys_Role_MenuInfo;
                          
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new Model.WebReturn() { ReturnValue = true, Message = "修改成功" };

                    }
                    catch (Exception ex)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message= "服务器出错，"+ex.Message };
                        throw ex;
                    }

                }

            }
                           
        }
        public Model.WebReturn GetModel(Model.Sys_UserInfo user)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                try
                {
                    var query=(from b in lqh.Umsdb.Sys_RoleInfo
                              select b).ToList();
                    return new Model.WebReturn() { Obj = query, ReturnValue = true };            
                }
                catch (Exception ex)
                {
                    return new Model.WebReturn() {  ReturnValue = false };
                }

              }
        }
        public  List<Model.Sys_RoleInfo> GetList(Model.Sys_UserInfo user, DateTime dt)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
              
                    try
                    {
                        DataLoadOptions dataload = new DataLoadOptions();
                        dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_MenuInfo);
                        //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_Menu_ProInfo);
                        //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_Menu_HallInfo);
                        dataload.LoadWith<Model.Sys_Role_MenuInfo>(c => c.Sys_MenuInfo);
                        dataload.LoadWith<Model.Sys_MenuInfo>(c => c.Sys_MethodInfo);
                        
                        lqh.Umsdb.LoadOptions = dataload;
                        lqh.Umsdb.ObjectTrackingEnabled = false;

                        var query = from b in lqh.Umsdb.Sys_RoleInfo
                                     where b.Flag == true && b.RoleID==user.RoleID
                                     select b;
                        //if (query == null || query.Count() == 0)
                        //{
                        //    return null;
                        //}
                    //    System.Collections.ArrayList arr = new System.Collections.ArrayList();
                        //arr.AddRange(query);
                       // arr.Add(query);

                      
                        return query.ToList();
                    }
                    catch (Exception ex)
                    {
                        return new List<Model.Sys_RoleInfo>();
                    }
                }
            }

        //public  List<Model.Sys_RoleInfo> GetList(Sys_UserInfo user, DateTime dt)
        //{
        //    using (LinQSqlHelper lqh = new LinQSqlHelper())
        //    {

        //        try
        //        {
        //            var query = (from b in lqh.Umsdb.GetTable<Model.Sys_RoleInfo>()
        //                         where b.Flag == true
        //                         select b).ToList();
        //            if (query == null || query.Count() == 0)
        //            {
        //                return null;
        //            }
                   
                   

        //            return query.ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        public Model.WebReturn GetList(Model.Sys_UserInfo user, Model.ReportPagingParam pageParam)
        {
            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                //using (TransactionScope ts = new TransactionScope())
                //{
                try
                {
                    DataLoadOptions dataload = new DataLoadOptions();
                    dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_MenuInfo);
                    //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_Menu_ProInfo);
                    //dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_Menu_HallInfo);
                    dataload.LoadWith<Model.Sys_Role_MenuInfo>(c => c.Sys_MenuInfo);
                    //dataload.LoadWith<Model.Sys_MenuInfo>(c => c.Sys_MethodInfo);

                    lqh.Umsdb.LoadOptions = dataload;
                    #region 权限
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    Model.WebReturn ret = ValidClassInfo.GetHall_ProIDFromRole(user, this.MenthodID, ValidHallIDS, ValidProIDS,lqh);

                    if (ret.ReturnValue != true)
                    { return ret; }

                    #endregion

                    if (pageParam == null || pageParam.PageIndex < 0 || pageParam.PageSize != 20)
                    {
                        return new Model.WebReturn() { ReturnValue = false, Message = "参数错误" };
                    }
                    if (pageParam.ParamList == null) pageParam.ParamList = new List<Model.ReportSqlParams>();
                    #region 将传入的参数与指定的参数了左连接

                    var param_join = from b in pageParam.ParamList
                                     join c in this.ParamList
                                     on new { b.ParamName, t = b.GetType() }
                                     equals
                                     new { c.ParamName, t = c.GetType() }
                                     into temp1
                                     from c1 in temp1.DefaultIfEmpty()
                                     select new
                                     {
                                         ParamFront = b,
                                         ParamBehind = c1
                                     };

                    #endregion

                    #region 获取数据

                    var inorder_query = from b in lqh.Umsdb.Sys_RoleInfo
                                        where b.Flag==true 
                                        select b;
                    foreach (var m in param_join)
                    {
                        if (m.ParamBehind == null)//不存在字段
                        {
                            continue;
                        }
                        //new Model.ReportSqlParams(){ParamName="InOrderID" },
                        //new Model.ReportSqlParams(){ParamName="Pro_HallID"},
                        //new Model.ReportSqlParams(){ParamName="OldID"}, 
                        //new Model.ReportSqlParams(){ParamName="UserName"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_start"},
                        //new Model.ReportSqlParams(){ParamName="SysDate_end"},
                        //new Model.ReportSqlParams(){ParamName="Note"}

                        switch (m.ParamFront.ParamName)
                        {
                            
                            case "RoleName":
                                Model.ReportSqlParams_String mm4 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm4.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.RoleName.Contains(mm4.ParamValues)
                                                    select b;
                                    break;
                                }
                            
                            case "Note":
                                Model.ReportSqlParams_String mm7 = (Model.ReportSqlParams_String)m.ParamFront;
                                if (string.IsNullOrEmpty(mm7.ParamValues))
                                    break;
                                else
                                {
                                    inorder_query = from b in inorder_query
                                                    where b.Note.Contains(mm7.ParamValues)
                                                    select b;
                                    break;
                                }
                            default: break;
                        }
                    }

                    #endregion

                    #region 过滤仓库
                    //if (ValidHallIDS.Count() > 0)
                    //    inorder_query = from b in inorder_query
                    //                    where ValidHallIDS.Contains(b.Pro_HallID)
                    //                    orderby b.SysDate descending
                    //                    select b;

                    //else
                    //    inorder_query = from b in inorder_query
                    //                    orderby b.SysDate descending
                    //                    select b;
                    #endregion
                    pageParam.RecordCount = inorder_query.Count();

                    #region 判断是否超过总页数

                    int pagecount = pageParam.RecordCount / pageParam.PageSize;

                    if (pageParam.PageIndex > pagecount)
                    {
                        pageParam.PageIndex = 0;
                        List<Model.Sys_RoleInfo> list = inorder_query.Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }

                    else
                    {
                        //pageParam.PageIndex = 0;
                        List<Model.Sys_RoleInfo> list = inorder_query.Skip(pageParam.PageSize * pageParam.PageIndex).Take(pageParam.PageSize).ToList();
                        pageParam.Obj = list;
                        return new Model.WebReturn() { ReturnValue = true, Message = "获取成功", Obj = pageParam };
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    return new Model.WebReturn() { Obj = null, ReturnValue = false, Message = ex.Message };

                }
                //}
            }
        }

        /// <summary>
        /// 批量修改角色XML
        /// </summary>
        /// <returns></returns>
        public Model.WebReturn UpdateRoleXML()
        {

            using (LinQSqlHelper lqh = new LinQSqlHelper())
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        DataLoadOptions dataload = new DataLoadOptions();
                        dataload.LoadWith<Model.Sys_RoleInfo>(c => c.Sys_Role_MenuInfo);
                        lqh.Umsdb.LoadOptions = dataload;

                        var RoleList = from b in lqh.Umsdb.Sys_RoleInfo
                                       select b;
                        var menuList = from b in lqh.Umsdb.Sys_MenuInfo
                                       where b.Flag == true orderby b.Order
                                       select b;
                        //var menu_list=
                        foreach (Model.Sys_RoleInfo model in RoleList)
                        {
                            
                            var join_list =( from b in model.Sys_Role_MenuInfo
                                             orderby b.Sys_MenuInfo.Order
                                            select b.Sys_MenuInfo).ToList();



                            if (model.RoleID == 1)
                                join_list = (from b in lqh.Umsdb.Sys_MenuInfo
                                            orderby b.Order
                                            select b).ToList();
                            //else
                            //{
                            //    var templist = join_list.ToList();
                            //    templist.Remove(null);
                            //    foreach (var m in join_list)
                            //    {
                            //        if (m == null || m.Parent==null || m.Parent==0) continue;
                            //        if (templist.Where(p => p.MenuID == m.Parent).Count() > 0) continue;
                            //        else {
                            //            int? i=m.Parent;
                            //            while(m.Parent!=null && m.Parent!=0)
                            //            {
                            //                var t= menuList.Where(p => p.MenuID == i);
                            //                if (t.Count() == 0) break;
                            //                else {
                            //                    templist.Add(t.First());
                            //                    i = t.First().Parent;
                            //                }
                            //            }
                            //        }
                            //    }
                            //}
                            model.MenuXML = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<root>\r\n";
                       
                        
                        //从菜单列表中搜索具有相同父节点的子节点，保存在 arraylist 中        
                            List<APIModel.MenuList> MenuForJson = new List<APIModel.MenuList>();
                            model.MenuXML += GetNode(0, join_list.Distinct().ToList(), MenuForJson);
                            model.MenuXML += "</root>";
                            JsonFx.Json.JsonWriter write = new JsonFx.Json.JsonWriter();
                            if (MenuForJson.Count() > 0)
                            {
                                model.MobileMenuJson = write.Write(MenuForJson.First().child);

                            }
                            else
                                model.MobileMenuJson = "[]";
                        }
                        lqh.Umsdb.SubmitChanges();
                        ts.Complete();
                        return new WebReturn() { ReturnValue=true, Message="更新成功"};
                    }
                    catch (Exception ex)
                    {
                        return new WebReturn() { ReturnValue = false, Message = "更新失败" + ex.Message };
                    }
                }
            }
        }
    }
                      
    
}
               