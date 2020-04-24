namespace bantam.Classes
{
    /// <summary>
    /// Config Class, Holds default and global config / option variables
    /// </summary>
    static class Config
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        static Config()
        {
            //todo this should be in our global xml file

            logLevel = LogHelper.LOG_LEVEL.ERROR;
            commentFrequency = 50;
            commentMaxLength = 24;
            maxPostSizeKiB = 8192;
            phpVaribleNameMaxLength = 16;
            timeoutMS = 20000; //20 sec
            phpShellCodeExectionValue = 0;

            enableLogging = true;
            enableGlobalMessageBoxes = true;
            maxExecutionTime = false;
            disableErrorLogs = true;
            injectRandomComments = true;

            defaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:64.0) Gecko/20100101 Firefox/64.0";
        }

        /// <summary>
        /// 
        /// </summary>
        private static string defaultUserAgent;
        public static string DefaultUserAgent {
            get {
                return defaultUserAgent;
            }
            set {
                defaultUserAgent = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool enableLogging;
        public static bool EnableLogging {
            get {
                return enableLogging;
            }
            set {
                enableLogging = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool enableGlobalMessageBoxes;
        public static bool EnableGlobalMessageBoxes {
            get {
                return enableGlobalMessageBoxes;
            }
            set {
                enableGlobalMessageBoxes = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static LogHelper.LOG_LEVEL logLevel;
        public static LogHelper.LOG_LEVEL LogLevel {
            get {
                return logLevel;
            }
            set {
                logLevel = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool maxExecutionTime;
        public static bool MaxExecutionTime {
            get {
                return maxExecutionTime;
            }
            set {
                maxExecutionTime = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool disableErrorLogs;
        public static bool DisableErrorLogs {
            get {
                return disableErrorLogs;
            }
            set {
                disableErrorLogs = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int maxPostSizeKiB;
        public static int MaxPostSizeKib {
            get {
                return maxPostSizeKiB;
            }
            set {
                maxPostSizeKiB = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private const int maxCookieSizeB = 8192;
        public static int MaxCookieSizeB {
            get {
                return maxCookieSizeB;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool injectRandomComments;
        public static bool InjectRandomComments {
            get {
                return injectRandomComments;
            }
            set {
                injectRandomComments = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int commentFrequency;
        public static int CommentFrequency {
            get {
                return commentFrequency;
            }
            set {
                commentFrequency = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int commentMaxLength;
        public static int CommentMaxLength {
            get {
                return commentMaxLength;
            }
            set {
                commentMaxLength = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private const bool randomizePhpVariableNames = true;
        public static bool RandomizePhpVariableNames {
            get {
                return randomizePhpVariableNames;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int phpVaribleNameMaxLength;
        public static int PhpVariableNameMaxLength {
            get {
                return phpVaribleNameMaxLength;
            }
            set {
                phpVaribleNameMaxLength = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int timeoutMS;
        public static int TimeoutMS {
            get {
                return timeoutMS;
            }
            set {
                timeoutMS = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static int phpShellCodeExectionValue;
        public static int PhpShellCodeExectionVectorValue {
            get {
                return phpShellCodeExectionValue;
            }
            set {
                phpShellCodeExectionValue = value;
            }
        }
    }
}
