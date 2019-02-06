using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bantam.Classes
{
    static class Config
    {
        static Config()
        {
            //todo this should be in our global xml file
            enableLogging = true;
            enableGlobalMessageBoxes = true;
            logLevel = 1;

            maxExecutionTime = false;
            disableErrorLogs = true;

            maxPostSizeKiB = 8192;

            injectRandomComments = true;
            commentFrequency = 2;
            commentMaxLength = 24;

            phpVaribleNameMaxLength = 16;
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool enableLogging;
        public static bool EnableLogging
        {
            get { return enableLogging; }
            set { enableLogging = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool enableGlobalMessageBoxes;
        public static bool EnableGlobalMessageBoxes
        {
            get { return enableGlobalMessageBoxes; }
            set { enableGlobalMessageBoxes = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int logLevel;
        public static int LogLevel
        {
            get { return logLevel; }
            set { logLevel = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool maxExecutionTime;
        public static bool MaxExecutionTime
        {
            get { return maxExecutionTime; }
            set { maxExecutionTime = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool disableErrorLogs;
        public static bool DisableErrorLogs
        {
            get { return disableErrorLogs; }
            set { disableErrorLogs = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int maxPostSizeKiB;
        public static int MaxPostSizeKib
        {
            get { return maxPostSizeKiB;  }
            set { maxPostSizeKiB = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private const int maxCookieSizeB = 8192;
        public static int MaxCookieSizeB
        {
            get { return maxCookieSizeB; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool injectRandomComments;
        public static bool InjectRandomComments
        {
            get { return injectRandomComments; }
            set { injectRandomComments = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int commentFrequency;
        public static int CommentFrequency
        {
            get { return commentFrequency; }
            set { commentFrequency = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int commentMaxLength;
        public static int CommentMaxLength
        {
            get { return commentMaxLength; }
            set { commentMaxLength = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private const bool randomizePhpVariableNames = true;
        public static bool RandomizePhpVariableNames
        {
            get { return randomizePhpVariableNames; }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int phpVaribleNameMaxLength;
        public static int PhpVariableNameMaxLength
        {
            get { return phpVaribleNameMaxLength; }
        }
    }
}
