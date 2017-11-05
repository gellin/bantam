using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bantam_php
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicThreadArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string target { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<object> callback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object[] callbackArgs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Code"></param>
        /// <param name="Callback"></param>
        /// <param name="CallbackArgs"></param>
        public DynamicThreadArgs(string Target, string Code, Action<object> Callback, object[] CallbackArgs)
        {
            target = Target;
            code = Code;
            callback = Callback;
            callbackArgs = CallbackArgs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Target"></param>
        /// <param name="Code"></param>
        public DynamicThreadArgs(string Target, string Code)
        {
            target = Target;
            code = Code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetHost"></param>
        /// <param name="code"></param>
        /// <param name="callbackArgs"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static DynamicThreadArgs GetThreadArgs(string targetHost, string code, object[] callbackArgs, Action<object> callback)
        {
            return new DynamicThreadArgs(targetHost, code, callback, callbackArgs);
        }
    }
}
