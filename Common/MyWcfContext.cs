using System.Collections.Generic;
using System.ServiceModel;

namespace Common
{
    public class MyWcfContext : IExtension<InstanceContext>
    {
          private readonly IDictionary<string, object> items;

          private MyWcfContext()
    {
        items = new Dictionary<string, object>();
    }

    public IDictionary<string, object> Items
    {
        get { return items; }
    }

    public static MyWcfContext Current
    {
        get
        {
            MyWcfContext context = OperationContext.Current.InstanceContext.Extensions.Find<MyWcfContext>();
            if (context == null)
            {
        context = new MyWcfContext();
                OperationContext.Current.InstanceContext.Extensions.Add(context);
            }
            return context;
        }
    }

    public void Attach(InstanceContext owner) { }
    public void Detach(InstanceContext owner) { }
    }
}