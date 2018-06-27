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
        public string host { get; set; }

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
        /// <param name="Host"></param>
        /// <param name="Code"></param>
        /// <param name="Callback"></param>
        /// <param name="CallbackArgs"></param>
        public DynamicThreadArgs(string Host, string Code, Action<object> Callback = null, object[] CallbackArgs = null)
        {
            host = Host;
            code = Code;
            callback = Callback;

            //if we have ben supplied a callback without callback args, we default the callback args to include the "target"/"host" for gui manipulation/etc
            if (Callback != null && CallbackArgs == null)
            {
                callbackArgs = new object[] { Host };
            }
            else
            {
                callbackArgs = CallbackArgs;
            }
        }
    }
}
