/*************************************************************************
 * 文件名称 ：ApiData.cs                          
 * 描述说明 ：取得API数据
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;
using System.Dynamic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MUSystem.Core
{
    public class ApiData : IDataGetter
    {
        public object GetData(HttpContext context)
        {
            dynamic data = null;
            var url = context.Request.Form["dataAction"];
            var param = JsonConvert.DeserializeObject<dynamic>(context.Request.Form["dataParams"]);

            var route = url.Replace("/api/", "").Split('/'); // route[0]=mms,route[1]=send,route[2]=get
            var type = Type.GetType(String.Format("MUSystem.Areas.{0}.Controllers.{1}ApiController,MUSystem.Web", route), false, true);
            if (type != null)
            {
                var instance = Activator.CreateInstance(type);

                //action注意方法名的大小写在反射的时候
                var action = route.Length > 2 ? route[2] : "Get";
                //var action = "GetDetail";
                var methodInfo = type.GetMethod(action);

                var parameters = new object[] { new RequestWrapper().SetRequestData(param) };
                //此时说明要打印明细了
                if (route.Length>3)
                {
                    parameters = new object[] { route[3] };
                }
            
                data = methodInfo.Invoke(instance, parameters);

                if (data.GetType() == typeof(ExpandoObject))
                {
                    if ((data as ExpandoObject).Where(x => x.Key == "rows").Count() > 0)
                        data = data.rows;
                }
            }

            return data;
        }
    }
}
