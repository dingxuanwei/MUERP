/*************************************************************************
 * 文件名称 ：Column.cs                          
 * 描述说明 ：定义题头
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System.Collections.Generic;

namespace MUSystem.Core
{
    public class Column
    {
        public Column()
        {
            rowspan = 1;
            colspan = 1;
        }
        public string field { get; set; }
        public string title { get; set; }
        public int rowspan { get; set; }
        public int colspan { get; set; }
        public string hidden { get; set; }

        //此处被借来当是否导出该字段用
        public bool sortable { get; set; }

        //此处被借来当是否导出该字段用
        public string noprint { get; set; }


    }

    //二维尺寸表头列
    public class TwoDimensionalColumn
    {
        /// <summary>
        /// sheet名称
        /// </summary>
        public List<string> SheetName { get; set; }
        /// <summary>
        /// 尺码表头
        /// </summary>
        public List<string> SizeTitle { get; set; }
        /// <summary>
        /// 公共固定表头
        /// </summary>
        public List<Column> CommonTitle { get; set; }
    }
}
