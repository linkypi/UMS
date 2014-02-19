using System.Runtime.Serialization;
using System;
namespace UserMSService
{
    [DataContract]
    public class InitData
    {
        [DataMember] public Exception ex;
        [DataMember] public int ReturnCode;
        //[DataMember] object
    }
}