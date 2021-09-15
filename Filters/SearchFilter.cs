using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace WebApplication1.Filters
{
    public class SearchFilter<T>
    {
        public SearchFilter()
        {
        }
        public SearchFilter(T data,IQueryable<T> filterresponse)
        {
            Data = data;
            Filterresponse = filterresponse;
        }

        public T Data { get; set; }
        public IQueryable<T> Filterresponse { get; set; }

        public IQueryable<T> filter()
        {
            int i = 0;
            foreach (PropertyInfo prop in Data.GetType().GetProperties())
            {
                //System.Diagnostics.Debug.WriteLine(prop.ToString());
                //System.Diagnostics.Debug.WriteLine(prop.GetValue(Data,null).ToString());
                if (true&&i<1/*!Convert.ToBoolean(prop.GetValue(prop,null))*/)
                {
                    System.Diagnostics.Debug.WriteLine(prop.GetValue(Data, null).ToString());
                    Filterresponse = Filterresponse.Where(x => x.GetType().GetProperty(prop.Name)==prop);
                }
                i++;
            }
            return this.Filterresponse;
        }
    }
}
