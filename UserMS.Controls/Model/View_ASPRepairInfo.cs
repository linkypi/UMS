using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace UserMS.Model
{
    public  class View_ASPRepairInfo : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private string _backnote; 

        public string BackNote
        {
            get { return this._backnote; }
            set { _backnote = value; OnPropertyChanged("BackNote"); }
        }

        public System.Collections.Generic.List<UserMS.API.ASP_Factory> Factorys { get; set; }

   
        public System.Nullable<int> OrderID{ get; set; }

   
        public string OldID{ get; set; }

   
        public string RepairNote{ get; set; }
        public string RpState { get; set; }
   
        public string Chk_InOut{ get; set; }

   
        public string Repairer{ get; set; }
   
        public string RepairHallName{ get; set; }

   
        public System.Nullable<bool> IsToFact{ get; set; }

   
        public string IMEI{ get; set; }

   
        public System.Nullable<System.DateTime> SysDate{ get; set; }

   
        public System.Nullable<bool> HasBJ{ get; set; }

   
        public int ID{ get; set; }

   
        public System.Nullable<bool> ZJPassed{ get; set; }

   
        public string ZJNote{ get; set; }

   
        public System.Nullable<System.DateTime> ZJDate{ get; set; }

   
        public string ZJUserID{ get; set; }

   
        public System.Nullable<bool> HasRepaired{ get; set; }

   
        public string ChkNote{ get; set; }

   
        public System.Nullable<bool> IsPassed{ get; set; }

   
        public decimal ProMoney{ get; set; }

   
        public decimal WorkMoney{ get; set; }

   
        public decimal BJ_Money{ get; set; }

   
        public string BJDate{ get; set; }

   
        public string BJUser{ get; set; }

   
        public string RepairerHallID{ get; set; }

   
        public string BJHallID{ get; set; }

   
        public string BJHallName{ get; set; }

   
        public decimal ShouldPay{ get; set; }

   
        public decimal RealPay{ get; set; }

   
        public string FetchNote{ get; set; }

   
        public System.Nullable<bool> IsFetchAduit{ get; set; }

   
        public string FetchAduitUserID{ get; set; }

   
        public System.Nullable<System.DateTime> FetchAduitDate{ get; set; }

   
        public System.Nullable<bool> FetchNeedAudit{ get; set; }

   
        public string AuditUserID{ get; set; }

   
        public System.Nullable<bool> HasAudited{ get; set; }

   
        public System.Nullable<System.DateTime> AuditDate{ get; set; }

   
        public System.Nullable<decimal> AuditMoney{ get; set; }

   
        public System.Nullable<decimal> AuditLowMoney{ get; set; }

   
        public string AuditNote{ get; set; }

   
        public System.Nullable<bool> HasFetch{ get; set; }

   
        public System.Nullable<bool> HasCallBack{ get; set; }

   
        public System.Nullable<bool> IsLack{ get; set; }

   
        public System.Nullable<bool> IsBack{ get; set; }

   
        public System.Nullable<bool> NeedToFact{ get; set; }

   
        public string FetchAuditNote{ get; set; }

   
        public System.Nullable<bool> FetchAuditPassed{ get; set; }

   
        public System.Nullable<int> RepairCount{ get; set; }

   
        public string FetchAuditPassedStr{ get; set; }

   
        public string IsFetchAduitStr{ get; set; }

   
        public string Pro_HeaderIMEI{ get; set; }

   
        public string Sender{ get; set; }

   
        public string SenderPhone{ get; set; }

   
        public string Cus_CPC{ get; set; }

   
        public string Cus_Email{ get; set; }

   
        public string Cus_Add{ get; set; }

   
        public string Cus_Phone2{ get; set; }

   
        public string Cus_Phone{ get; set; }

   
        public string Cus_Name{ get; set; }

   
        public string Pro_Other{ get; set; }

   
        public string Pro_Seq{ get; set; }

   
        public string Pro_Type{ get; set; }

   
        public string Pro_Color{ get; set; }

   
        public string Pro_SN{ get; set; }

   
        public string Pro_Name{ get; set; }

   
        public string Pro_IMEI{ get; set; }

   
        public string ServiceID{ get; set; }

   
        public string Receiver{ get; set; }

   
        public string Cus_CusType{ get; set; }

   
        public string RecHallName{ get; set; }

   
        public string HallID{ get; set; }

   
        public string GzType{ get; set; }

   
        public System.Nullable<decimal> GzMoney{ get; set; }

   
        public System.Nullable<int> Chk_FID{ get; set; }

   
        public System.Nullable<decimal> Chk_Price{ get; set; }

   
        public System.Nullable<bool> IsDelete{ get; set; }

   
        public string RepairID{ get; set; }

   
        public string BackInListID{ get; set; }

   
        public string NewIMEI{ get; set; }

   
        public string NewSN{ get; set; }

   
        public string FacInListID{ get; set; }

   
        public string FacName{ get; set; }

   
        public string BuyDate{ get; set; }

   
        public string Errors{ get; set; }

        public string ToFacNote{ get; set; }

            public string   RepKind{get;set;}
         public string Dispatcher{get;set;}
         public bool? HasDispatch{get;set;}
         public DateTime? 维修完成时间{get;set;}
          public DateTime? 接机日期{get;set;}
       public DateTime? 质检日期{get;set;}
       public DateTime? 审核日期{get;set;}
       public DateTime? 取机日期{get;set;}
      public  DateTime?  回访日期{get;set;}
       public DateTime? 审计日期{get;set;}
      public string  Position     {get;set;}
       public string 撤销人       {get;set;}
       public string 撤销备注     {get;set;}
      public DateTime?  撤销时间   {get;set;}
       public string 质检人       {get;set;}
      public string  需送厂       {get;set;}
      public DateTime?  送厂时间   {get;set;}
      public string  送厂人       {get;set;}
      public string  送厂批次     {get;set;}
     public string   厂家名称     {get;set;}
      public DateTime?  返厂日期  {get;set;}
      public string  返厂批次    {get;set;}
      public string  返厂备注    {get;set;}
      public string  返厂人      {get;set;}
      public string  质检备注    {get;set;}
      public decimal  劳务费    {get;set;}
      public decimal  配件费    {get;set;}
      public decimal  应收      {get;set;}
      public decimal  实收      {get;set;}
      public decimal  备机押金  {get;set;}
        public string  审核人    {get;set;}
        public string  审核备注  {get;set;}
        public string  取机人    {get;set;}
        public string  取机备注  {get;set;}
        public decimal  挂账金额 {get;set;}
        public string  挂账类型     {get;set;}
       public string   回访人       {get;set;}
       public string   审计人       {get;set;}
           public decimal   审计金额{get;set;}
           public decimal  结算金额 {get;set;}
       public string 审计备注     {get;set;}
      public string  ChangPros    {get;set;}
     public string   Chk_RType    {get;set;}
    }                            
}                                
