using UnityEngine;

namespace Gear.Core
{
    /// <summary>
    /// Centralized logging system that can be stripped from Release builds.
    /// Wraps all Debug.Log calls for performance optimization.
    /// </summary>
    public static class LogManager
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        private const bool ENABLE_LOGS = true; // Set to false for Release builds

        /// <summary>
        /// Logs a message with the specified log level.
        /// Called by any system that needs to log information.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="level">The severity level of the log</param>
        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            if (!ENABLE_LOGS) return;

            switch (level)
            {
                case LogLevel.Info:
                    Debug.Log($"[INFO] {message}");
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning($"[WARNING] {message}");
                    break;
                case LogLevel.Error:
                    Debug.LogError($"[ERROR] {message}");
                    break;
            }
        }

        /// <summary>
        /// Logs a message with context object for easier debugging.
        /// Called by MonoBehaviours to provide GameObject context.
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="context">The Unity object context</param>
        /// <param name="level">The severity level of the log</param>
        public static void Log(string message, Object context, LogLevel level = LogLevel.Info)
        {
            if (!ENABLE_LOGS) return;

            switch (level)
            {
                case LogLevel.Info:
                    Debug.Log($"[INFO] {message}", context);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning($"[WARNING] {message}", context);
                    break;
                case LogLevel.Error:
                    Debug.LogError($"[ERROR] {message}", context);
                    break;
            }
        }
    }
}
