using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;

namespace UserMS.Common
{
    public class GetTypeService
    {
        public GetTypeService(List<API.VIP_VIPTypeService> TypeServices, ref RadGridView DGservice)
        {

            var query = from b in TypeServices
                        join c in Store.ProInfo on b.ProID equals c.ProID
                        join d in Store.ProClassInfo on c.Pro_ClassID equals d.ClassID
                        join e in Store.ProTypeInfo on c.Pro_TypeID equals e.TypeID
                        select new
                        {
                            b.SCount,
                            c.ProName,
                            c.ProFormat,
                            d.ClassName,
                            e.TypeName
                        };
            if(query.Count()==0)
                return;
            List<SlModel.ServiceModel> ServiceModel = new List<SlModel.ServiceModel>();
            foreach (var Item in query)
            {
                SlModel.ServiceModel model = new SlModel.ServiceModel();
                model.ProClassNane = Item.ClassName;
                model.ProTypeName = Item.TypeName;
                model.ProName = Item.ProName;
                model.SCount = Item.SCount.ToString();
                ServiceModel.Add(model);
            }
            DGservice.ItemsSource = ServiceModel;
            DGservice.Rebind();

                       
        }
    }
}
