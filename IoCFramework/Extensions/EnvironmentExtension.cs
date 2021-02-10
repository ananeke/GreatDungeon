using System.Diagnostics;
using System.Reflection;

namespace PatternsLibrary.Extensions
{
    public class EnvironmentExtension
    {
        #region Public Properties
        /// <summary>
        /// True if we are in a development (specifically, debuggable) environment
        /// </summary>
        public static bool IsDevelopment { get; private set; }

        /// <summary>
        /// The configuration of the environment, either Development or Production
        /// </summary>
        public string Configuration { get; private set; }

        #endregion

        #region Constructor
        static EnvironmentExtension()
        {
            IsDevelopment = Assembly.GetEntryAssembly()?.GetCustomAttribute<DebuggableAttribute>()?.IsJITTrackingEnabled == true;
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public EnvironmentExtension()
        {
            Configuration = IsDevelopment ? "Development" : "Production";
        }

        #endregion
    }
}
