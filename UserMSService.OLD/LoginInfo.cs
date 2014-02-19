using System;
using System.Runtime.Serialization;
using Model;

namespace UserMSService
{
    [DataContract]
    public class LoginInfo
    {
        [DataMember]
        public Exception ex;
        [DataMember]
        public int ReturnCode;
        [DataMember]
        public UserInfo Userinfo;

         
    }
}