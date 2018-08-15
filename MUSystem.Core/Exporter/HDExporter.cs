/*************************************************************************
 * 文件名称 ：HDExporter.cs                          
 * 描述说明 ：文件导出处理(主表和明细表) 只限exsl
 * 
 * 创建信息 : create by PepeLee on 2016-1-25
 * 修订信息 : modify by (person) on (date) for (reason)
 * 
 * 版权信息 : Copyright (c) 2016 秒优信息科技有限公司 
**************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using MUSystem.Utils;
using System.Dynamic;

namespace MUSystem.Core
{
    public class HDExporter
    {
        const string DEFAULT_EXPORT = "xls";
        const string DEFAULT_DATAGETTER = "api";
        const string DEFAULT_COMPRESS = "none";

        private Dictionary<string, Type> _dataGetter = new Dictionary<string, Type>() { 
            { "api", typeof(ApiData) } 
        };

        private Dictionary<string, Type> _export = new Dictionary<string, Type>() { 
            { "xls", typeof(XlsExport) }, 
            { "xlsx", typeof(XlsxExport) }
        };

        private Dictionary<string, IFormatter> _fieldFormatter = new Dictionary<string, IFormatter>();

        private object _data;//主表数据
        //private object _data_D;//子表信息
        private List<List<Column>> _title_H;//主表标题
        private List<List<Column>> _title_D;//子表标题
        private Stream _fileStream = null;
        private string _fileName = string.Empty;
        private string _suffix = string.Empty;

        public static HDExporter Instance()
        {
            var hdexport = new HDExporter();
            var context = HttpContext.Current;
            //明细表标题
            if (context.Request.Form["titles_d"] != null)
                hdexport.DetailesTitle(JsonConvert.DeserializeObject<List<List<Column>>>(context.Request.Form["titles_d"]));
            //主表标题
            if (context.Request.Form["titles_h"] != null)
                hdexport.MasterTitle(JsonConvert.DeserializeObject<List<List<Column>>>(context.Request.Form["titles_h"]));

            //获取数据
            if (context.Request.Form["dataGetter"] != null)
                hdexport.HeaderAndDetailsData(context.Request.Form["dataGetter"]);

            if (context.Request.Form["fileType"] != null)
                hdexport.Export(context.Request.Form["fileType"]);

            if (context.Request.Form["dataGetter"] != null)
                hdexport.HeaderAndDetailsData(context.Request.Form["dataGetter"]);

            if (context.Request.Form["fileType"] != null)
                hdexport.Export(context.Request.Form["fileType"]);


            return hdexport;
        }

        public HDExporter MasterTitle(List<List<Column>> title_h)
        {
            _title_H = title_h;
            return this;
        }

        public HDExporter DetailesTitle(List<List<Column>> title_d) {
            _title_D = title_d;
            return this;
        }


        public HDExporter HeaderAndDetailsData(IDataGetter data)
        {
            _data = data.GetData(HttpContext.Current);
            return this;
        }
        public HDExporter HeaderAndDetailsData(string type)
        {
            var dataGetter = GetActor<IDataGetter>(_dataGetter, DEFAULT_DATAGETTER, type);
            return HeaderAndDetailsData(dataGetter);
        }

        public HDExporter Export(string type)
        {
            var export = GetActor<IExport>(_export, DEFAULT_EXPORT, type);
            return Export(export);
        }
        public HDExporter Export(IExport export)
        {
            
            Dictionary<int, int> currentHeadRow = new Dictionary<int, int>();
            Dictionary<string, List<int>> fieldIndex = new Dictionary<string, List<int>>();
            Func<int, int> GetCurrentHeadRow = cell => currentHeadRow.ContainsKey(cell) ? currentHeadRow[cell] : 0;
            var currentRow = 0;
            var currentCell = 0;

            export.Init(_data);

            for (var i = 0; i < _title_H.Count; i++)
            {
                currentCell = 0;

                for (var j = 0; j < _title_H[i].Count; j++)
                {
                    var item = _title_H[i][j];
                    if (item.hidden == "true" || item.hidden == "hidden") continue;

                    while (currentRow < GetCurrentHeadRow(currentCell))
                        currentCell++;

                    export.FillData(currentCell, currentRow, "title_" + item.field, item.title);


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
            //设置主表题头样式
            export.SetHeadStyle(0, 0, currentCell - 1, currentRow - 1);

            //设置主表数据样式
            var dataCount = 0;
            EachHelper.EachListRow((object)((dynamic)_data).hData, (i, r) => dataCount++);
            export.SetRowsStyle(0, currentRow, currentCell - 1, currentRow + dataCount - 1);

            //填充主表数据
            EachHelper.EachListRow((object)((dynamic)_data).hData, (rowIndex, rowData) =>
            {
                EachHelper.EachObjectProperty(rowData, (i, name, value) =>
                {
                    if (fieldIndex.ContainsKey(name))
                        foreach (int cellIndex in fieldIndex[name])
                        {
                            if (_fieldFormatter.ContainsKey(name))
                                value = _fieldFormatter[name].Format(value);
                            export.FillData(cellIndex, currentRow, name, value);
                        }
                });
                currentRow++;
            });
            //明细表
            for (var i = 0; i < _title_D.Count; i++)
            {
                currentCell = 0;

                for (var j = 0; j < _title_D[i].Count; j++)
                {
                    var item = _title_D[i][j];
                    if (item.hidden == "true" || item.hidden == "hidden") continue;

                    while (currentRow < GetCurrentHeadRow(currentCell))
                        currentCell++;

                    export.FillData(currentCell, currentRow, "title_" + item.field, item.title);


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
            //设置明细表题头样式
            export.SetHeadStyle(0, currentRow - 1, currentCell - 1, currentRow - 1);

            //设置明细表数据样式
            var dataCount_d = 0;
            EachHelper.EachListRow((object)((dynamic)_data).dData, (i, r) => dataCount_d++);
            export.SetRowsStyle(0, currentRow, currentCell - 1, currentRow + dataCount_d - 1);

            //填充明细表数据
            EachHelper.EachListRow((object)((dynamic)_data).dData, (rowIndex, rowData) =>
            {
                EachHelper.EachObjectProperty(rowData, (i, name, value) =>
                {
                    if (fieldIndex.ContainsKey(name))
                        foreach (int cellIndex in fieldIndex[name])
                        {
                            if (_fieldFormatter.ContainsKey(name))
                                value = _fieldFormatter[name].Format(value);
                            export.FillData(cellIndex, currentRow, name, value);
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
        public HDExporter FileName(string fileName)
        {
            _fileName = fileName;
            return this;
        }
        public void Download()
        {
            if (_fileStream != null && _fileStream.Length > 0)
                ZFiles.DownloadFile(HttpContext.Current, _fileStream, string.Format("{0}.{1}", _fileName, _suffix), 1024 * 1024 * 10);
        }
        private T GetActor<T>(Dictionary<string, Type> dict, string defaultKey, string key)
        {
            if (!dict.ContainsKey(key))
                key = defaultKey;

            return (T)Activator.CreateInstance(dict[key]);
        }
    }
}
