using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{ 
    [Serializable]    
    public class CollectionFile
    {
        public string FileName { get; set; }
        public int DecodeNum { get; set; }
        public int UnDecodeNum { get; set; }
    }
}
