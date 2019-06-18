using System.Collections.Generic;
using System;
using System.Xml;
using System.Reflection;
using UnityEngine;

public abstract class IXMLConfigParser
{
    public abstract Dictionary<I, T> LoadConfig<I, T>(string tablename, string nodePath, string identify);
    public abstract void WriteConfig<I, T>(Dictionary<I, T> dic, string tablename, string nodePath);
    protected T GreateAndSetValue<T>(XmlElement node)
    {
        // 通过类型创建一个对象实例
        T obj = Activator.CreateInstance<T>();

        // 获取一个类的所有字段
        FieldInfo[] fields = typeof(T).GetFields();

        for (int i = 0; i < fields.Length; i++)
        {
            string name = fields[i].Name;
            if (string.IsNullOrEmpty(name)) continue;

            string fieldValue = node.GetAttribute(name);
            if (string.IsNullOrEmpty(fieldValue)) continue;

            try
            {
                ParsePropertyValue<T>(obj, fields[i], fieldValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("XML读取错误：对象类型({2}) => 属性名({0}) => 属性类型({3}) => 属性值({1})",
                    fields[i].Name, fieldValue, typeof(T).ToString(), fields[i].FieldType.ToString()));
            }
        }
        return obj;
    }

    private void ParsePropertyValue<T>(T obj, FieldInfo fieldInfo, string valueStr)
    {
        object value = valueStr;

        // 将字符串解析为类中定义的类型
        if (fieldInfo.FieldType.IsEnum)//是枚举吗 
            value = Enum.Parse(fieldInfo.FieldType, valueStr);//转成枚举类型
        else
        {
            if (fieldInfo.FieldType == typeof(int))
                value = int.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(byte))
                value = byte.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(bool))
                value = bool.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(float))
                value = float.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(double))
                value = double.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(uint))
                value = uint.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(ulong))
                value = ulong.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(Vector3))
                value = Ve3Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(string))
                value = valueStr;

        }

        if (value == null)
            return;
        //重置类型
        fieldInfo.SetValue(obj, value);
    }

    /// <summary>
    /// string 转换为Ve3
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Vector3 Ve3Parse(string name)
    {
        name = name.Replace(" ", "").Replace("(", "").Replace(")", "");
        string[] s = name.Split(',');
        return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
    }
    /// <summary>
    /// 判断节点是否存在
    /// </summary>
    /// <param name="xmlPath"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public bool IsSelectNode(string xmlPath, string node)
    {
        using (XmlReader reader = XmlReader.Create(xmlPath))
        {
            while (reader.Read())
            {
                if (reader.Name == node && reader.NodeType == XmlNodeType.Element)
                    return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 创建最里层复数个数据
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="dic"></param>
    /// <param name="xmlDoc"></param>
    /// <param name="parentXmlE"></param>
    /// <param name="childXmlC"></param>
    public void CreateInsideData<I, T>(Dictionary<I, T> dic, XmlDocument xmlDoc, XmlElement parentXmlE, XMLClass childXmlC)
    {
        XmlElement xmlChild;//最底层的子节点
        FieldInfo[] fields = typeof(T).GetFields();//类里的属性
        string val;//枚举类型转换临时储存
        foreach (var item in dic)
        {
            //创建最里层
            xmlChild = xmlDoc.CreateElement(childXmlC.className);
            //反射出改类的类型 和属性                                       
            for (int k = 0; k < fields.Length; k++)
            {
                //判断是否是枚举
                if (fields[k].FieldType.IsEnum)
                {
                    val = ((int)fields[k].GetValue(item.Value)).ToString();
                }
                else
                {
                    val = fields[k].GetValue(item.Value).ToString();
                }
                xmlChild.SetAttribute(fields[k].Name, val);
            }
            parentXmlE.AppendChild(xmlChild);
        }
    }
    /// <summary>
    /// 获取限定范围内的字符串
    /// </summary>
    /// <param name="str"></param>
    /// <param name="star"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public string GetLimitStr(string str, string star, string end)
    {
        if (star.IndexOf(end, 0) != -1)
            return "";
        //转换为在字符串第几位
        int i = str.IndexOf(star);
        //第二个与第一个index差值
        int j = str.IndexOf(end);
        if (i == -1 || j == -1)
            return "";
        //截取的位置就是第一个位置加上他的长度 到 第二个位置与第一个位置的差值
        return str.Substring(i + star.Length, j - i - star.Length);
    }
}