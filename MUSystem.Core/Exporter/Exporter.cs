/*************************************************************************
 * 文件名称 ：Exporter.cs                          
 * 描述说明 ：文件导出处理
 * 
 * 创建信息 : create by liuhuisheng.xm@gmail.com on 2012-11-10
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2013 厦门纵云信息科技有限公司 www.zoewin.com
**************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using MUSystem.Utils;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MUSystem.Core
{
    public class Exporter
    {
        const string DEFAULT_EXPORT = "xls";
        const string DEFAULT_DATAGETTER = "api";
        const string DEFAULT_COMPRESS = "none";

        private Dictionary<string, Type> _compress = new Dictionary<string, Type>() { 
            { "zip", typeof(ZipCompress)},
            {"none",typeof(NoneCompress)}
        };
        private Dictionary<string, Type> _dataGetter = new Dictionary<string, Type>() { 
            { "api", typeof(ApiData) } 
        };
        private Dictionary<string, Type> _export = new Dictionary<string, Type>() { 
            { "xls", typeof(XlsExport) }, 
            { "xlsx", typeof(XlsxExport) } ,
            { "doc", typeof(HtmlDocExport) },
            { "pdf", typeof(PdfExport) }
        };

        private Dictionary<string,IFormatter> _fieldFormatter = new Dictionary<string,IFormatter>();

        private object _data;
        private List<List<Column>> _title;
        private Stream _fileStream = null;
        private string _fileName = string.Empty;
        private string _suffix = string.Empty;

        public static Exporter Instance()
        {
            var export = new Exporter();
            var context = HttpContext.Current;

            if (context.Request.Form["titles"] != null)
                export.Title(JsonConvert.DeserializeObject<List<List<Column>>>(context.Request.Form["titles"]));
          
            if (context.Request.Form["dataGetter"] != null)
                export.Data(context.Request.Form["dataGetter"]);
            //}
            if (context.Request.Form["fileType"] != null)
                export.Export(context.Request.Form["fileType"]);

            if (context.Request.Form["compressType"] != null)
                export.Compress(context.Request.Form["compressType"]);


            return export;
        }
 
        public Exporter Data(IDataGetter data)
        {
            _data = data.GetData(HttpContext.Current);
            return this;
        }
 
        public Exporter Data(string type)
        {
            var dataGetter = GetActor<IDataGetter>(_dataGetter, DEFAULT_DATAGETTER,type);
            return Data(dataGetter);
        }

        public Exporter Data(object data)
        {
            _data = data;
            return this;
        }

        public Exporter AddFormatter(string field,IFormatter formatter)
        {
            _fieldFormatter[field] = formatter;
            return this;
        }

        public Exporter Title(List<List<Column>> title)
        {
            _title = title;
            return this;
        }

        public Exporter FileName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public Exporter Export(string type)
        {
            var export = GetActor<IExport>(_export, DEFAULT_EXPORT, type);
            return Export(export);
        }

        public Exporter Export(IExport export)
        {
            if (_title == null)
            {
                _title = new List<List<Column>>();
                _title.Add(new List<Column>());
                EachHelper.EachListHeader(_data, (i, field, type) => _title[0].Add(new Column() { title = field, field = field, rowspan = 1, colspan = 1 }));
            }
 
            Dictionary<int, int> currentHeadRow = new Dictionary<int, int>();
            Dictionary<string, List<int>> fieldIndex = new Dictionary<string, List<int>>();
            Func<int, int> GetCurrentHeadRow = cell => currentHeadRow.ContainsKey(cell) ? currentHeadRow[cell] : 0;
            var currentRow = 0;
            var currentCell = 0;

            export.Init(_data);

            //生成多行题头
            for (var i = 0; i < _title.Count; i++)
            {
                currentCell = 0;

                for (var j = 0; j < _title[i].Count; j++)
                {
                    var item = _title[i][j];
                    //此处如果前台datagrid列为隐藏，则此处不会作为某字段进行导出
                    if ((item.hidden == "true" || item.hidden == "hidden"))
                    {
                        if(!item.sortable)
                        {
                            continue;
                        }
                    } 


                    while (currentRow < GetCurrentHeadRow(currentCell)) 
                        currentCell++;

                    export.FillData(currentCell, currentRow, "title_" + item.field, item.title);

                    if (item.rowspan + item.colspan > 2)
                        export.MergeCell(currentCell, currentRow, currentCell + item.colspan - 1, currentRow + item.rowspan - 1);

                    if (!string.IsNullOrEmpty(item.field))
                    {
                        if (!fieldIndex.ContainsKey(item.field))
                            fieldIndex[item.field] = new List<int>();
                        fieldIndex[item.field].Add(currentCell);
                    }

                    for (var k = 0; k < item.colspan; k++)
                        currentHeadRow[currentCell] = GetCurrentHeadRow(currentCell++) + item.rowspan;
                }
                currentRow++;
            }

            //设置题头样式
            export.SetHeadStyle(0, 0, currentCell - 1, currentRow - 1);

            //设置数据样式
            var dataCount = 0;
            EachHelper.EachListRow(_data, (i, r) => dataCount++);
            export.SetRowsStyle(0, currentRow, currentCell - 1, currentRow + dataCount - 1);

            //填充数据
            EachHelper.EachListRow(_data, (rowIndex, rowData) =>
            {
                EachHelper.EachObjectProperty(rowData, (i, name, value) =>
                {
                    if (fieldIndex.ContainsKey(name))
                        foreach (int cellIndex in fieldIndex[name])
                        {
                            if (_fieldFormatter.ContainsKey(name))
                                value = _fieldFormatter[name].Format(value);
                            //增加HTML元素标签过滤 modify by zq 2016 06 13
                            string strText = string.Empty;
                            if (value != null)
                            {
                                strText = System.Text.RegularExpressions.Regex.Replace(value.ToString(), "<[^>]+>", "");
                                strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
                                //strText = value.ToString();
                            }
                           
                            export.FillData(cellIndex, currentRow, name, strText);
                        }
                });
                currentRow++;
            });
           
            _fileStream = export.SaveAsStream();

            _suffix = export.suffix;
            if (string.IsNullOrEmpty(_fileName))
                _fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            
            return this;
        }

        public Exporter Compress(string type)
        {
            var compress = GetActor<ICompress>(_compress, DEFAULT_COMPRESS, type);
            return Compress(compress);
        }

        public Exporter Compress(ICompress compress)
        {
            _fileStream = compress.Compress(_fileStream, string.Format("{0}.{1}", _fileName, _suffix));
            _suffix = compress.Suffix(_suffix);
            return this;
        }

        public void Download()
        {
            if (_fileStream != null && _fileStream.Length > 0)
                ZFiles.DownloadFile(HttpContext.Current, _fileStream, string.Format("{0}.{1}",_fileName,_suffix), 1024 * 1024 * 10);
        }
 
        private T GetActor<T>(Dictionary<string, Type> dict, string defaultKey, string key)
        {
            if (!dict.ContainsKey(key))
                key = defaultKey;

            return (T)Activator.CreateInstance(dict[key]);
        }

       

        public static bool DownloadTwoSheet()
        {
            var export = new Exporter();
            var context = HttpContext.Current;
            var titles=JsonConvert.DeserializeObject<List<List<Column>>>(context.Request.Form["titles"]);
            var titlesSum =JsonConvert.DeserializeObject<List<List<Column>>>(context.Request.Form["titlesSum"]);

            var param = JsonConvert.DeserializeObject<dynamic>(context.Request.Form["dataParams"]);
            var paramSum = JsonConvert.DeserializeObject<dynamic>(context.Request.Form["dataParamsSum"]);
            var gridData = GetDataTwoSheet(context, param, context.Request.Form["dataAction"]);
            var gridSumData = GetDataTwoSheet(context, paramSum, context.Request.Form["dataActionSum"]);
            var type = context.Request.Form["fileType"];
            ImportToExcel(titles,titlesSum, gridData, gridSumData, type);
            return true;
        }
        public static dynamic GetDataTwoSheet(HttpContext context,dynamic param,string url)
        {
            dynamic data = null;
            var route = url.Replace("/api/", "").Split('/'); // route[0]=mms,route[1]=send,route[2]=get
            var type = Type.GetType(String.Format("MUSystem.Areas.{0}.Controllers.{1}ApiController,MUSystem.Web", route), false, true);
            if (type != null)
            {
                var instance = Activator.CreateInstance(type);

                var action = route.Length > 2 ? route[2] : "Get";
                var methodInfo = type.GetMethod(action);
                var parameters = new object[] { param };
                data = methodInfo.Invoke(instance, parameters);
                

                if (data.GetType() == typeof(ExpandoObject))
                {
                    if ((data as ExpandoObject).Where(x => x.Key == "rows").Count() > 0)
                        data = data.rows;
                }
            }

            return data;
        }
        /// <summary>
        /// 根据路由url获取某路由默认首页数据
        /// </summary>
        /// <param name="param"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static dynamic GetDataByRoute(dynamic param, string url)
        {
            dynamic data = null;
            var route = url.Replace("/api/", "").Split('/'); // route[0]=mms,route[1]=send,route[2]=get
            var type = Type.GetType(String.Format("MUSystem.Areas.{0}.Controllers.{1}ApiController,MUSystem.Web", route), false, true);
            if (type != null)
            {
                var instance = Activator.CreateInstance(type);

                var action = route.Length > 2 ? route[2] : "Get";
                var methodInfo = type.GetMethod(action);
                var parameters = new object[] { new RequestWrapper().SetRequestData(param) };
                data = methodInfo.Invoke(instance, parameters);

                //if (data.GetType() == typeof(ExpandoObject))
                //{
                //    if ((data as ExpandoObject).Where(x => x.Key == "rows").Count() > 0)
                //        data = data.rows;
                //}
            }

            return data;
        }

        public static bool ImportToExcel(List<List<Column>> titles, List<List<Column>> titlesSum,dynamic grid,dynamic gridSum,string type)
        {
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.BufferOutput = true;
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.AppendHeader("Content-Disposition", "attachment;filename="+Server.UrlEncode("孟宪会Excel表格测试")+".xls");  
            // 采用下面的格式，将兼容 Excel 2003,Excel 2007, Excel 2010。  

            String FileName = DateTime.Now.ToString("yyyyMMddHHmmss"); ;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName +"."+ type);
            HttpContext.Current.Response.Write("<?xml version='1.0'?><?mso-application progid='Excel.Sheet'?>");
            HttpContext.Current.Response.Write(@"<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'  
            xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel'  
            xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet' xmlns:html='http://www.w3.org/TR/REC-html40'>");
            HttpContext.Current.Response.Write(@"<DocumentProperties xmlns='urn:schemas-microsoft-com:office:office'>");
            HttpContext.Current.Response.Write(@"<Author></Author><LastAuthor></LastAuthor>  
            <Created>2010-09-08T14:07:11Z</Created><Company>mxh</Company><Version>1990</Version>");
            HttpContext.Current.Response.Write("</DocumentProperties>");
            HttpContext.Current.Response.Write(@"<Styles><Style ss:ID='Default' ss:Name='Normal'><Alignment ss:Vertical='Center'/>  
            <Borders/><Font ss:FontName='宋体' x:CharSet='134' ss:Size='12'/><Interior/><NumberFormat/><Protection/></Style>");
            //定义标题样式      
            HttpContext.Current.Response.Write(@"<Style ss:ID='Header' ><Borders><Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/></Borders>  
            <Font ss:FontName='宋体' x:CharSet='134' ss:Size='14' ss:Color='#000000' ss:Bold='1' /></Style>");
            //定义边框  
            HttpContext.Current.Response.Write(@"<Style ss:ID='border'><NumberFormat ss:Format='@'/><Borders>  
            <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/></Borders></Style>");
            HttpContext.Current.Response.Write("</Styles>");

            #region sheet1
            HttpContext.Current.Response.Write("<Worksheet ss:Name='Sheet1'>");
            HttpContext.Current.Response.Write("<Table x:FullColumns='1' x:FullRows='1'>");

            //输出标题  
            foreach (var items in titles)
            {
                HttpContext.Current.Response.Write("\r\n<Row ss:AutoFitHeight='1'>");
                for (var i = 0; i < items.Count; i++)
                {
                    var flag = items[i].hidden;
                    var title = items[i].title;
                    if (!(flag == "true" || flag == "hidden"))
                    {
                        HttpContext.Current.Response.Write("<Cell ss:StyleID='Header'><Data ss:Type='String'>" + title + "</Data></Cell>");
                    }
                }
                HttpContext.Current.Response.Write("\r\n</Row>");
            }
            //数据行
            for (var j=0;j< grid.Count;j++)
            {
                ExpandoObject data = grid[j];
                HttpContext.Current.Response.Write("<Row>");
                foreach (var items in titles)
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        var flag = items[i].hidden;
                        var title = items[i].title;
                        var filed = items[i].field;
                        if (!(flag == "true" || flag == "hidden"))
                        {
                            var value = data.SingleOrDefault(x=>x.Key==filed);
                            HttpContext.Current.Response.Write("<Cell ss:StyleID='border'><Data ss:Type='String'>" + value.Value + "</Data></Cell>");
                            
                           
                        }
                    }
                }
                HttpContext.Current.Response.Write("</Row>");

            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</Worksheet>");
            HttpContext.Current.Response.Flush();
            #endregion
            #region sheet2
            HttpContext.Current.Response.Write("<Worksheet ss:Name='Sheet2'>");
            HttpContext.Current.Response.Write("<Table x:FullColumns='1' x:FullRows='1'>");

            //输出标题  
            foreach (var items in titlesSum)
            {
                HttpContext.Current.Response.Write("\r\n<Row ss:AutoFitHeight='1'>");
                for (var i = 0; i < items.Count; i++)
                {
                    var flag = items[i].hidden;
                    var title = items[i].title;
                    if (!(flag == "true" || flag == "hidden"))
                    {
                        HttpContext.Current.Response.Write("<Cell ss:StyleID='Header'><Data ss:Type='String'>" + title + "</Data></Cell>");
                    }
                }
                HttpContext.Current.Response.Write("\r\n</Row>");
            }
            //数据行
            for (var j = 0; j < gridSum.Count; j++)
            {
                ExpandoObject data = gridSum[j];
                HttpContext.Current.Response.Write("<Row>");
                foreach (var items in titlesSum)
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        var flag = items[i].hidden;
                        var title = items[i].title;
                        var filed = items[i].field;
                        if (!(flag == "true" || flag == "hidden"))
                        {
                            var value = data.SingleOrDefault(x => x.Key == filed);
                            HttpContext.Current.Response.Write("<Cell ss:StyleID='border'><Data ss:Type='String'>" + value.Value + "</Data></Cell>");
                        }
                    }
                }
                HttpContext.Current.Response.Write("</Row>");

            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</Worksheet>");
            HttpContext.Current.Response.Flush();
            #endregion
            HttpContext.Current.Response.Write("</Workbook>");
            HttpContext.Current.Response.End();
            return true;
        }

        /// <summary>
        /// 导出二维尺寸表
        /// </summary>
        ///创建人：马灵
        /// 创建时间:2018-05-08
        public void DownloadTwoDimensionSize()
        {
            if (_fileStream != null && _fileStream.Length > 0)
                ImportTwoDimensionSizeToExcel(_fileName,_data,_title);
        }

        /// <summary>
        /// 获取所有的表头以及sheet名称
        /// </summary>
        /// 创建人：马灵
        /// 创建时间:2018-05-08
        /// <param name="_title"></param>
        /// <returns></returns>
        private static TwoDimensionalColumn GetTitles(List<List<Column>> _title)
        {
            var sheetNameList = new List<string>();
            var sizeList = new List<string>();
            var commonTitle = new List<Column>();
            //生成多行题头
            for (var i = 0; i < _title.Count; i++)
            {
                for (var j = 0; j < _title[i].Count; j++)
                {
                    var item = _title[i][j];
                    //此处如果前台datagrid列为隐藏，则此处不会作为某字段进行导出
                    if ((item.hidden == "true" || item.hidden == "hidden"))
                    {
                        if (!item.sortable)
                        {
                            continue;
                        }
                    }
                    var field=item.field;
                    var title=item.title;
                    //获取所有sheetName
                    if (string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(title) && field != "PartType" && field != "UpperDeviation" && field != "LowerDeviation" && field != "Description")
                    {
                        if (!title.Contains('_'))
                        {
                            sheetNameList.Add(title);
                        }
                    }
                    //获取尺码表头
                    if (!string.IsNullOrEmpty(field) && !string.IsNullOrEmpty(title) && field != "PartType" && field != "UpperDeviation" && field != "LowerDeviation" && field != "Description")
                    {
                       
                            var isExist = sizeList.Count(x => x == title);
                            if (isExist == 0)
                                sizeList.Add(title);
                    }
                    //获取固定表头
                    if ( field == "PartType" ||  field == "Description")
                    {

                        commonTitle.Add(item);
                    }
                }
               
            }
            var result=new TwoDimensionalColumn();
            result.SheetName = sheetNameList;
            result.SizeTitle = sizeList;
            result.CommonTitle = commonTitle;
            return result;
        }
        /// <summary>
        /// 二维尺寸表导出多个sheet数据处理
        /// </summary>
        /// 创建人：马灵
        /// 创建时间:2018-05-08
        /// <param name="_fileName"></param>
        /// <param name="_data"></param>
        /// <param name="_title"></param>
        /// <returns></returns>
        public static bool ImportTwoDimensionSizeToExcel(string _fileName,object _data, List<List<Column>> _title)
        {
            //获取所有的表头以及sheet名称
            var Columns= GetTitles(_title);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.BufferOutput = true;
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.AppendHeader("Content-Disposition", "attachment;filename="+Server.UrlEncode("孟宪会Excel表格测试")+".xls");  
            // 采用下面的格式，将兼容 Excel 2003,Excel 2007, Excel 2010。  

            String FileName = DateTime.Now.ToString("yyyyMMddHHmmss"); ;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + _fileName + ".xls" );
            HttpContext.Current.Response.Write("<?xml version='1.0'?><?mso-application progid='Excel.Sheet'?>");
            HttpContext.Current.Response.Write(@"<Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet'  
            xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel'  
            xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet' xmlns:html='http://www.w3.org/TR/REC-html40'>");
            HttpContext.Current.Response.Write(@"<DocumentProperties xmlns='urn:schemas-microsoft-com:office:office'>");
            HttpContext.Current.Response.Write(@"<Author></Author><LastAuthor></LastAuthor>  
            <Created>2010-09-08T14:07:11Z</Created><Company>mxh</Company><Version>1990</Version>");
            HttpContext.Current.Response.Write("</DocumentProperties>");
            HttpContext.Current.Response.Write(@"<Styles><Style ss:ID='Default' ss:Name='Normal'><Alignment ss:Vertical='Center'/>  
            <Borders/><Font ss:FontName='宋体' x:CharSet='134' ss:Size='12'/><Interior/><NumberFormat/><Protection/></Style>");
            //定义标题样式      
            HttpContext.Current.Response.Write(@"<Style ss:ID='Header' ><Borders><Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/></Borders>  
            <Font ss:FontName='宋体' x:CharSet='134' ss:Size='14' ss:Color='#000000' ss:Bold='1' /></Style>");
            //定义边框  
            HttpContext.Current.Response.Write(@"<Style ss:ID='border'><NumberFormat ss:Format='@'/><Borders>  
            <Border ss:Position='Bottom' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Left' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Right' ss:LineStyle='Continuous' ss:Weight='1'/>  
            <Border ss:Position='Top' ss:LineStyle='Continuous' ss:Weight='1'/></Borders></Style>");
            HttpContext.Current.Response.Write("</Styles>");

            #region 处理数据导出
            var sheetNames = Columns.SheetName;
            var commonTitle = Columns.CommonTitle;
            var sizeTitle = Columns.SizeTitle;
            for (var i = 0; i < sheetNames.Count; i++)
            {
                HttpContext.Current.Response.Write("<Worksheet ss:Name='"+ sheetNames [i]+ "'>");
                HttpContext.Current.Response.Write("<Table x:FullColumns='1' x:FullRows='1'>");
                HttpContext.Current.Response.Write("\r\n<Row ss:AutoFitHeight='1'>");
                //输出标题  
                for (var j = 0; j < commonTitle.Count;j++)//固定标题  
                {
                    var title = commonTitle[j].title;
                    if (i > 0 && commonTitle[j].field == "Description")//只有第一个sheet需要这个度量描述字段
                        continue;
                    HttpContext.Current.Response.Write("<Cell ss:StyleID='Header'><Data ss:Type='String'>" + title + "</Data></Cell>");
                }
                for (var j = 0; j < sizeTitle.Count; j++)//固定尺码 
                {
                    var title = sizeTitle[j];
                    HttpContext.Current.Response.Write("<Cell ss:StyleID='Header'><Data ss:Type='String'>" + title + "</Data></Cell>");
                }
                HttpContext.Current.Response.Write("\r\n</Row>");

                //数据行
                var grid = (dynamic)_data;
                for(var g=0;g< grid.Count;g++)
                {
                    ExpandoObject data = grid[g];
                    HttpContext.Current.Response.Write("<Row>");
                   //固定列数据
                    for (var k = 0; k < commonTitle.Count;k++)
                    {
                       var flag = commonTitle[k].hidden;
                       var title = commonTitle[k].title;
                       var filed = commonTitle[k].field;
                       if (i > 0 && filed == "Description")//只有第一个sheet需要这个度量描述字段
                            continue;
                       if (!(flag == "true" || flag == "hidden"))
                       {
                           var value = data.SingleOrDefault(x => x.Key == filed);
                           HttpContext.Current.Response.Write("<Cell ss:StyleID='border'><Data ss:Type='String'>" + value.Value + "</Data></Cell>");
                       }
                    }
                    //尺码数据
                    for (var k = 0; k < sizeTitle.Count; k++)
                    {
                            var filed = sheetNames[i]+"_"+ sizeTitle[k];
                            var value = data.SingleOrDefault(x => x.Key == filed);
                            HttpContext.Current.Response.Write("<Cell ss:StyleID='border'><Data ss:Type='String'>" + value.Value + "</Data></Cell>");
                       
                    }
                    HttpContext.Current.Response.Write("</Row>");
                }

                HttpContext.Current.Response.Write("</Table>");
                HttpContext.Current.Response.Write("</Worksheet>");
                HttpContext.Current.Response.Flush();

            }
            #endregion
            
            HttpContext.Current.Response.Write("</Workbook>");
            HttpContext.Current.Response.End();
            return true;
        }
    }
}
