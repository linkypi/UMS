using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Common;
using Model;

namespace DAL
{
    public class Asset_UseInfo
    {
        private int MethodID;

        public Asset_UseInfo()
        {
            this.MethodID = 0;
        }

        public Asset_UseInfo(int MenthodID)
        {
            this.MethodID = MenthodID;
        }

        public WebReturn Add(Model.Sys_UserInfo sysUser, Model.Asset_UseInfo model)
        {
            try
            {
               
                model.SysDate = DateTime.Now;
                model.SysUser = sysUser.UserID;
                model.ProCount = 1;
                model.Flag = false;



               

                    #region 验证仓库 、商品权限

                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    var r = ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Any() && !ValidHallIDS.Contains(model.HallID))
                        return new Model.WebReturn() {ReturnValue = false, Message = "仓库无权操作"};

                    #endregion

                    using (LinQSqlHelper lqh = new LinQSqlHelper())
                    {

                    #region  拣货

                    var imeiList = lqh.Umsdb.Pro_IMEI.Where(b => b.IMEI == model.IMEI && b.HallID == model.HallID);

                    #endregion

                    if (imeiList.Any())
                    {
                        var imeimodel = imeiList.First();
                        r = Utils.CheckIMEI(imeimodel);
                        if (r.ReturnValue != true)
                            return r;

                        imeimodel.Pro_StoreInfo.ProCount -= 1;

                        imeimodel.Asset_UseInfo = model;
                       
                        lqh.Umsdb.Asset_UseInfo.InsertOnSubmit(model);
                        
                        lqh.Umsdb.SubmitChanges();
                        return new WebReturn()
                        {
                            ReturnValue = true
                        };
                    }
                    else
                    {
                        return new WebReturn()
                        {
                            ReturnValue = false,
                            Message = "串码不存在"

                        };
                    }



                }
            }
            catch (Exception ex)
            {
                return new WebReturn()
                {
                    ReturnValue = false,
                    Message = ex.Message
                };

            }

        }

        public WebReturn Del(Model.Sys_UserInfo sysUser, Model.Asset_UseInfo model)
        {


            try
            {




              


                    #region 验证仓库 、商品权限

                    //有权限的仓库
                    List<string> ValidHallIDS = new List<string>();
                    //有权限的商品
                    List<string> ValidProIDS = new List<string>();

                    var r = ValidClassInfo.GetHall_ProIDFromRole(sysUser, this.MethodID, ValidHallIDS, ValidProIDS);

                    if (r.ReturnValue != true)
                        return r;
                    //有仓库限制，而且仓库不在权限范围内
                    if (ValidHallIDS.Any() && !ValidHallIDS.Contains(model.HallID))
                        return new Model.WebReturn() {ReturnValue = false, Message = "仓库无权操作"};

                    #endregion
                    using (LinQSqlHelper lqh = new LinQSqlHelper())
                    {
                    var Model = lqh.Umsdb.Asset_UseInfo.First(p => p.ID == model.ID);


                    var imeiList = lqh.Umsdb.Pro_IMEI.Where(b => b.IMEI == Model.IMEI && b.HallID == Model.HallID);



                    if (imeiList.Any())
                    {
                        var imeimodel = imeiList.First();


                        imeimodel.Pro_StoreInfo.ProCount += 1;
                        imeimodel.Asset_UseInfo = null;
                        Model.Flag = true;

                        Model.EndDate = DateTime.Now;
                        lqh.Umsdb.SubmitChanges();
                        return new WebReturn()
                        {
                            ReturnValue = true
                        };
                    }
                    else
                    {
                        return new WebReturn()
                        {
                            ReturnValue = false,
                            Message = "串码不存在"

                        };
                    }



                }



            }
            catch (Exception ex)
            {
                return new WebReturn()
                {
                    ReturnValue = false,
                    Message = ex.Message
                };

            }
        }


    }
}