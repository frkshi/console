using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DataEntity
{
    [Serializable]
    public abstract class EntityBase
    {
        protected List<string> _changedProperties = new List<string>();

        /// <summary>
        /// 更改属性
        /// </summary>
        protected List<string> ChangedProperties
        {
            get { return _changedProperties; }
        }

        /// <summary>
        /// 记录更改
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            this.ChangedProperties.Add(propertyName);
        }
        /// <summary>
        /// 校验更改
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns>bool型</returns>
        public bool CheckPropertyChanged(string propertyName)
        {
            return ChangedProperties.Contains(propertyName);
        }

        static public byte[] Serialize(EntityBase entity)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            formatter.Serialize(memoryStream, entity);
            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[memoryStream.Length];
            memoryStream.Read(buffer, 0, buffer.Length);
            
            memoryStream.Close();
            return buffer;
        }

        static public object Deserialize(byte[] buffer)
        {
            if (buffer == null)
                return null;

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(buffer);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(memoryStream);
        }
    }
}
