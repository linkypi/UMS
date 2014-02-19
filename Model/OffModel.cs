using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class OffModel
    {
        public List<Model.ProModel> ProModel { get; set; }
        public List<Model.HallModel> HallModel {get;set;}
        public List<Model.VIPModel> VIPModel{get;set;}
        public List<Model.VIPTypeModel> VIPTypeModel { get; set; }
        public List<Model.GrounpInfo> GrounpInfo { get; set; }
        public string Name {get;set;}
        public DateTime StartDate {get;set;}
        public DateTime EndDate {get;set;}
        public decimal ArriveMoney {get;set;}
        public string Note { get; set; }
        public int VIPTicketMaxCount { get; set; }
        public string CreatName { get; set; }
        public string SalesName { get; set; }
        public string ApplyNote { get; set; }
        public int ID { get;set;}
    }
}
