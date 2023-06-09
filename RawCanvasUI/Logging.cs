﻿using Rage;
using System;

namespace RawCanvasUI
{
    /// <summary>
    /// Used to determine whether a logging call should actually be used.
    /// </summary>
    public enum LoggingLevel
    {
        /// <summary>The most verbose level, logs everything.</summary>
        DEBUG,

        /// <summary>A standard logging level.</summary>
        INFO,

        /// <summary>Not quite errors but also not normal.</summary>
        WARNING,

        /// <summary>Used for exception handling.</summary>
        ERROR,
    }

    internal static class Logging
    {
        /// <summary>
        /// Send a DEBUG level log messsage.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public static void Debug(string message)
        {
            Log(LoggingLevel.DEBUG, message);
        }

        /// <summary>
        /// Send an ERROR level log message. Use for Exception handling.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="ex">The Exception raised.</param>
        public static void Error(string message, Exception ex)
        {
            Log(LoggingLevel.ERROR, message);
            Log(LoggingLevel.ERROR, ex.Message);
            Log(LoggingLevel.DEBUG, ex.StackTrace);
        }

        /// <summary>
        /// Send an INFO level log message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public static void Info(string message)
        {
            Log(LoggingLevel.INFO, message);
        }

        /// <summary>
        /// Send a WARNING level log message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        public static void Warning(string message)
        {
            Log(LoggingLevel.WARNING, message);
        }

        private static void Log(LoggingLevel level, string message)
        {
            Game.LogTrivial($"RawCanvasUI [{Enum.GetName(typeof(LoggingLevel), level)}] {message}");
        }
    }
}
