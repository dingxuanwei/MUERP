using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace MUSystem.Utils
{
    public class EachHelper
    {
        /// <summary>
        /// 原始导出excel函数有三个参数
        /// </summary>
        /// <param name="list"></param>
        /// <param name="handle"></param>
        public static void EachListHeader(object list, Action<int, string, Type> handle)
        {
            var index = 0;
            var dict = ZGeneric.GetListProperties(list);
            foreach (var item in dict)
                handle(index++, item.Key, item.Value);
        }

        /// <summary>
        /// 现在添加一个字段sortable，用于判断该字段是否导出
        /// </summary>
        /// <param name="list"></param>
        /// <param name="handle"></param>
        //public static void EachListHeaderExport(object list, Action<int, string,bool, Type> handle)
        //{
        //    var index = 0;
        //    var dict = ZGeneric.GetListProperties(list);
        //    foreach (var item in dict)
        //        handle(index++, item.Key,item.Key, item.Value);
        //}

        public static void EachListRow(object list, Action<int, object> handle)
        {
            var index = 0;
            IEnumerator enumerator = ((dynamic)list).GetEnumerator();
            while (enumerator.MoveNext())
                handle(index++, enumerator.Current);
        }

        public static void EachObjectProperty(object row, Action<int, string, object> handle)
        {
            var index = 0;
            var dict = ZGeneric.GetDictionaryValues(row);
            foreach (var item in dict)
                handle(index++, item.Key, item.Value);
        }
    }
}
