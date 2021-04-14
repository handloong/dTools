using dTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dTools
{
    /// <summary>
    /// DataTable扩展类
    /// </summary>
    public static class ExtensionDataTable
    {
        #region DataTable扩展方法

        /// <summary>
        ///  转换成Html Table
        /// </summary>
        /// <param name="table"></param>
        /// <param name="headerTitle"></param>
        /// <param name="contentBackGroundcolor">表头名称映射</param>
        /// <param name="titleBackgroundColor">标题背景颜色</param>
        /// <param name="borderColor">线的颜色</param>
        /// <returns></returns>
        public static string ToHtmlTable(this DataTable table,
                                         Dictionary<string, string> headerTitle = null,
                                         string contentBackGroundcolor = "#ffffff",
                                         string titleBackgroundColor = "#4b7cb2",
                                         string borderColor = "#4b7cb2")
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            System.Text.StringBuilder sb = new System.Text.StringBuilder($@"
<style type='text/css'>
table.gridtable_{guid} {{
    font-family: verdana,arial,sans-serif;
	font-size:11px;
	color:#fff;
	border-width: 1px;
	border-collapse: collapse;
}}
table.gridtable_{guid} th {{
    border-width: 1px;
	padding: 8px;
	border-style: solid;
	border-color: {borderColor};
	background-color: {titleBackgroundColor};
}}
table.gridtable_{guid} td {{
    border-width: 1px;
	padding: 8px;
	color:#000;
	border-style: solid;
	border-color: {borderColor};
	background-color: {contentBackGroundcolor};
}}
</style>

<table class='gridtable_{guid}'>
<tr>");
            //渲染列头
            var ingorList = new List<int>();
            //列多少行
            var dcCount = table.Columns.Count;
            for (int i = 0; i < dcCount; i++)
            {
                var dc = table.Columns[i];
                if (headerTitle != null)
                {
                    if (headerTitle.ContainsKey(dc.ColumnName))
                    {
                        sb.Append($@"<th>{headerTitle[dc.ColumnName]}</th>");
                    }
                    else
                    {
                        //此列忽略
                        ingorList.Add(i);
                    }
                }
                else
                {
                    sb.Append($@"<th>{dc.ColumnName}</th>");
                }
            }

            sb.Append($@"</tr>");



            //渲染 <tr>    <tr><td>admin</td><td>admin</td></tr>
            for (int i = 0; i < table.Rows.Count; i++)
            {
                sb.Append($@"<tr>");
                //渲染 <td>admin</td><td>admin</td>
                for (int j = 0; j < dcCount; j++)
                {
                    if (!ingorList.Contains(j))
                    {
                        sb.Append($@"<td>{Convert.ToString(table.Rows[i][j])}</td>");
                    }
                }
                sb.Append($@"</tr>");
            }

            sb.Append($@"
</table>");
            return sb.ToString();
        }

        /// <summary>
        /// DataTable转换为IEnumerable
        /// </summary>
        /// <param name="table">DataTable数据源</param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<dynamic> ToIEnumerable(this DataTable table)
        {
            return table?.Rows.Count > 0 ? table.AsEnumerable().Select(row => new DynamicRow(row)) : null;
        }

        /// <summary>
        /// DataTable转换强类型List集合
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="table">DataTable数据源</param>
        /// <returns>IList</returns>
        public static IList<T> ToList<T>(this DataTable table)
        {
            List<T> list = null;
            if (table?.Rows.Count > 0)
            {
                list = new List<T>();
                if (typeof(T).Name != "Object")
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var instance = Activator.CreateInstance<T>();
                        var props = instance.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
                        foreach (var p in props)
                        {
                            if (!p.CanWrite) continue;
                            if (table.Columns.Contains(p.Name) && !row[p.Name].IsNull())
                            {
                                p.SetValue(instance, row[p.Name].ToSafeValue(p.PropertyType), null);
                            }
                        }
                        list.Add(instance);
                    }
                }
                else
                {
                    list = table.ToIEnumerable()?.ToList() as List<T>;
                }
            }
            return list;
        }

        /// <summary>
        /// 去除重复数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataTable RemoveNullValue(this DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool IsNull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {
                        IsNull = false;
                    }
                }
                if (IsNull)
                {
                    removelist.Add(dt.Rows[i]);
                }
            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
            return dt;
        }
        /// <summary>
        /// DataTable转Hashtable
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <returns>Hashtable</returns>
        public static Hashtable ToHashtable(this DataTable dt)
        {
            Hashtable ht = null;
            if (dt?.Rows.Count > 0)
            {
                ht = new Hashtable();
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        var key = dt.Columns[i].ColumnName;
                        ht[key] = dr[key];
                    }
                }
            }
            return ht;
        }

        /// <summary>
        /// DataRow转HashTable
        /// </summary>
        /// <param name="dr">DataRow数据源</param>
        /// <returns>Hashtable</returns>
        public static Hashtable ToHashTable(this DataRow dr)
        {
            Hashtable ht = null;
            if (dr != null)
            {
                ht = new Hashtable(dr.ItemArray.Length);
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    ht.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
            }
            return ht;
        }

        /// <summary>
        /// 根据条件过滤表的内容
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="condition">过滤条件</param>
        /// <returns>DataTable</returns>
        public static DataTable Filter(this DataTable dt, string condition)
        {
            if (dt?.Rows.Count > 0 && !string.IsNullOrEmpty(condition))
            {
                var newdt = dt.Clone();
                var dr = dt.Select(condition);
                for (var i = 0; i < dr.Length; i++)
                {
                    newdt.ImportRow(dr[i]);
                }
                dt = newdt;
            }
            return dt;
        }

        /// <summary>
        /// 根据条件过滤表的内容
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <param name="condition">过滤条件</param>
        /// <param name="sort">排序字段</param>
        /// <returns>DataTable</returns>
        public static DataTable Filter(this DataTable dt, string condition, string sort)
        {
            if (dt?.Rows.Count > 0 && !string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(sort))
            {
                var newdt = dt.Clone();
                var dr = dt.Select(condition, sort);
                for (var i = 0; i < dr.Length; i++)
                {
                    newdt.ImportRow(dr[i]);
                }
                dt = newdt;
            }
            return dt;
        }

        /// <summary>
        /// DataTable转Xml
        /// </summary>
        /// <param name="dt">DataTable数据源</param>
        /// <returns>string</returns>
        public static string ToXml(this DataTable dt)
        {
            var result = string.Empty;
            if (dt?.Rows.Count > 0)
            {
                using (var writer = new StringWriter())
                {
                    dt.WriteXml(writer);
                    result = writer.ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// DataSet转Xml
        /// </summary>
        /// <param name="ds">DataSet数据源</param>
        /// <returns>string</returns>
        public static string ToXml(this DataSet ds)
        {
            var result = string.Empty;
            if (ds?.Tables.Count > 0)
            {
                using (var writer = new StringWriter())
                {
                    ds.WriteXml(writer);
                    result = writer.ToString();
                }
            }
            return result;
        }

        /// <summary>
        /// 将dt2合并到dt1中
        /// </summary>
        /// <param name="dt1">dt1</param>
        /// <param name="dt2">dt2</param>
        /// <returns>返回dt1</returns>
        public static DataTable Append(this DataTable dt1, DataTable dt2)
        {
            var obj = new object[dt1.Columns.Count];
            foreach (DataRow dr in dt2.Rows)
            {
                dr.ItemArray.CopyTo(obj, 0);
                dt1.Rows.Add(obj);
            }
            return dt1;
        }

        #endregion DataTable扩展方法

        #region DynamicRow

        /// <summary>
        /// 自定义动态类
        /// </summary>
        [Serializable]
        private sealed class DynamicRow : DynamicObject, IDictionary<string, object>
        {
            /// <summary>
            /// 私有字段
            /// </summary>
            private DataRow _row;

            /// <summary>
            /// 含参构造函数
            /// </summary>
            /// <param name="row"></param>
            public DynamicRow(DataRow row) => _row = row;

            #region 实现DynamicObject

            public override IEnumerable<string> GetDynamicMemberNames() => base.GetDynamicMemberNames();

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var retVal = _row.Table.Columns.Contains(binder.Name);
                result = null;
                if (retVal && !(_row[binder.Name] is DBNull)) result = _row[binder.Name];
                return retVal;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                if (!_row.Table.Columns.Contains(binder.Name))
                {
                    var dc = new DataColumn(binder.Name, value.GetType());
                    _row.Table.Columns.Add(dc);
                }
                _row[binder.Name] = value;
                return true;
            }

            public override bool TryInvoke(InvokeBinder binder, object[] args, out object result) => base.TryInvoke(binder, args, out result);

            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) => base.TryInvokeMember(binder, args, out result);

            #endregion 实现DynamicObject

            #region 实现IDictionary<string, object>

            ICollection<string> IDictionary<string, object>.Keys => _row.Table.Columns.OfType<DataColumn>().Select(o => o.ColumnName).ToArray();
            ICollection<object> IDictionary<string, object>.Values => _row.ItemArray.ToList();
            int ICollection<KeyValuePair<string, object>>.Count => _row.Table.Columns.Count;
            bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;

            object IDictionary<string, object>.this[string key]
            {
                get { return _row[key]; }
                set { _row[key] = value; }
            }

            bool IDictionary<string, object>.ContainsKey(string key) => _row.Table.Columns.Contains(key);

            void IDictionary<string, object>.Add(string key, object value)
            {
                if (!_row.Table.Columns.Contains(key))
                {
                    var dc = new DataColumn(key, value.GetType());
                    _row.Table.Columns.Add(dc);
                }
                _row[key] = value;
            }

            bool IDictionary<string, object>.Remove(string key)
            {
                var r = false;
                if (_row.Table.Columns.Contains(key))
                {
                    _row.Table.Columns.Remove(key);
                    r = true;
                }
                return r;
            }

            bool IDictionary<string, object>.TryGetValue(string key, out object value)
            {
                value = null;
                var b = _row.Table.Columns.Contains(key);
                if (b)
                {
                    if (!(_row[key] is DBNull)) value = _row[key];
                }
                return b;
            }

            void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
            {
                if (!_row.Table.Columns.Contains(item.Key))
                {
                    var dc = new DataColumn(item.Key, item.Value.GetType());
                    _row.Table.Columns.Add(item.Key);
                }
                _row[item.Key] = item.Value;
            }

            void ICollection<KeyValuePair<string, object>>.Clear() => _row.Table.Columns.Clear();

            bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item) => _row.Table.Columns.Contains(item.Key);

            void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                foreach (var kv in this)
                {
                    array[arrayIndex++] = kv;
                }
            }

            bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
            {
                var r = false;
                if (_row.Table.Columns.Contains(item.Key))
                {
                    _row.Table.Columns.Remove(item.Key);
                    r = true;
                }
                return r;
            }

            IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                var columns = _row.Table.Columns;
                for (var i = 0; i < columns.Count; i++)
                {
                    var key = i < columns.Count ? _row.Table.Columns[i].ColumnName : null;
                    var value = i < columns.Count ? _row[i] : null;
                    yield return new KeyValuePair<string, object>(key, value);
                }
            }

            #endregion 实现IDictionary<string, object>
        }

        #endregion DynamicRow

        #region 转换成JgridHtml
        /// <summary>
        /// 转换成JgridHtml
        /// </summary>
        /// <param name="this"></param>
        /// <param name="width"></param>
        /// <param name="align"></param>
        /// <param name="sortable"></param>
        /// <returns></returns>
        public static string ToJgridHtml(this DataTable @this, int width = 150, string align = "left", bool sortable = false)
        {
            var sb = new StringBuilder();
            foreach (DataColumn col in @this.Columns)
            {
                var c = col.ColumnName;
                sb.AppendLine($@"{{ label: '{c}', name: '{c}', index: '{c}', width: {width}, align: '{align}', sortable: {sortable.ToString().ToLower()} }},");
            }
            return sb.ToString();
        }
        #endregion

        #region ToCSC
        /// <summary>
        /// 转换成CSV
        /// </summary>
        /// <param name="this"></param>
        /// <param name="csvFileFullName"></param>
        /// <param name="writeHeader"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static bool ToCSV(this DataTable @this, string csvFileFullName, bool writeHeader,
          string delimeter)
        {
            if ((null != @this) && (@this.Rows.Count > 0))
            {
                File.Delete(csvFileFullName);

                var tmpLineText = new StringBuilder();

                //Write header
                if (writeHeader)
                {
                    tmpLineText.Clear();
                    for (int i = 0; i < @this.Columns.Count; i++)
                    {
                        string tmpColumnValue = @this.Columns[i].ColumnName;
                        if (tmpColumnValue.Contains(delimeter))
                        {
                            tmpColumnValue = "\"" + tmpColumnValue + "\"";
                        }

                        if (i == @this.Columns.Count - 1)
                        {
                            tmpLineText.Append(tmpColumnValue);
                        }
                        else
                        {
                            tmpLineText.Append(tmpColumnValue + delimeter);
                        }
                    }
                    WriteFile(csvFileFullName, tmpLineText.ToString());
                }

                //Write content
                for (int j = 0; j < @this.Rows.Count; j++)
                {
                    tmpLineText.Clear();
                    for (int k = 0; k < @this.Columns.Count; k++)
                    {
                        string tmpRowValue = @this.Rows[j][k].ToString();
                        if (tmpRowValue.Contains(delimeter))
                        {
                            tmpRowValue = "\"" + tmpRowValue + "\"";
                        }

                        if (k == @this.Columns.Count - 1)
                        {
                            tmpLineText.Append(tmpRowValue);
                        }
                        else
                        {
                            tmpLineText.Append(tmpRowValue + delimeter);
                        }
                    }
                    WriteFile(csvFileFullName, tmpLineText.ToString());
                }
            }
            return true;
        }

        private static void WriteFile(string fileFullName, string message)
        {
            using (StreamWriter sw = new StreamWriter(fileFullName, true, Encoding.UTF8))
            {
                sw.WriteLine(message);
            }
        }
        #endregion

    }
}