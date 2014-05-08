using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public enum RepairState:uint
    {
        WFDispatch = 0,
        WFRepaire = 1,  //"待维修",
        WFToFac = 2,    //"待送厂",
        WFBack =3,      // "待返厂",
        WFZhiJian =4,   // "待质检",
        WFShenHe = 5,   //"待审核",
        WFGetMobile = 6,//"待取机",
        WFBJAudit = 7,  //"待备机返库审批",
        WFAudit = 9,    //"待审计",
        WFCallBack = 8, //"待回访",
        Finished = 10   //"完成"
    }
}
