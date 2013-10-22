using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("MTU.DataAccess")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("3h")]
[assembly: AssemblyProduct("MTU.DataAccess")]
[assembly: AssemblyCopyright("Copyright © 3h 2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("300b172d-7680-4a75-ab21-0dcda7ffccaf")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("3.0.16.0")]
//[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: InternalsVisibleTo("MTU.DataAccess.Test")]

//modify: 2010-2-2 修改CommunicationSetting出错，主键Id未传入 版本号：0.1.0.6
//modify: 2010-2-5 添加配置同步的逻辑，specialManager.   版本号： 0.1.0.7
//modify: 2010-2-22 添加AlertData表RTUId字段 版本号： 0.1.0.8
//modify: 2010-4-7  添加对Collection文件的读取逻辑  版本号 0.1.0.9
//modify: 2010-4-8  添加MeasureData保存时，计算NewData的逻辑  版本号 0.1.0.9
//modify: 2010-4-12 修正数据库连接断开时，异常数据恢复时的Bug。 版本号 0.1.0.10
//                  添加AlertData保存时，计算NewData的逻辑
//modify: 2010-5-21 添加操作失败的异常信息捕获，检测连接三次后，检测时间间隔变改为30分钟，成功后还原为默认5分钟。版本号 0.1.0.11
//modify: 2010-5-26 添加在目标存储为sqlite的方式的情况下，获取指定条件的MeasureData. 版本号0.1.0.12
//modify: 2010-06-09 检测时间间隔读取全局变量 连接错误检测间隔1分钟，数据库级错误间隔前三次1分钟，以后是15分钟。 版本号0.1.0.13
//modify: 2011-1-19 检测数据库连接时，取消记入系统日志。增加日志信息等。版本号： 0.1.0.14