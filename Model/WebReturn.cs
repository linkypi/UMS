using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace Model
{ 
    public class WebReturn
    {
        private string _Message;
        private ArrayList _arrList=new ArrayList();
        private object _Obj;
        private bool _ReturnValue; 

        public bool ReturnValue
        {
            get { return _ReturnValue; }
            set { _ReturnValue = value; }
        } 
        public object Obj
        {
            get { return _Obj; }
            set { _Obj = value; }
        } 
        public ArrayList ArrList
        {
            get { return _arrList; }
            set { _arrList = value; }
        } 
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        
    }
}
