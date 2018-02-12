//*********************************************************
//
//    Copyright © Organized Chaos Technologies Inc. 2015 All rights reserved.
//	  Technical Contact: Rahim Bhatia, rahim@organizedchaostech.com
//	  http://www.organizedchaostech.com
//
//*********************************************************

using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;
using _logger = Microsoft.Practices.EnterpriseLibrary.Logging.Logger;

namespace eCMS.ExceptionLoging
{
	public class Logger
    {
        public Logger()
        {           
            _logger.SetLogWriter(new LogWriterFactory().Create(),false);
        }

        #region Methods

        [DebuggerStepThrough()]
        public void LogInfo(string message)
        {
            _logger.SetLogWriter(new LogWriterFactory().Create());
            _logger.Write(CreateEntry(message, TraceEventType.Information));
        }

        [DebuggerStepThrough()]
        public void LogWarning(string message)
        {
            _logger.SetLogWriter(new LogWriterFactory().Create());
            _logger.Write(CreateEntry(message, TraceEventType.Warning));
        }

        [DebuggerStepThrough()]
        public void LogError(string message)
        {
            _logger.Write(CreateEntry(message, TraceEventType.Error));
        }

        [DebuggerStepThrough()]
        private LogEntry CreateEntry(string message, TraceEventType severity)
        {
            return new LogEntry(message, "General", 0, 100, severity, string.Empty, null);
        }

        [DebuggerStepThrough()]
        private LogEntry CreateEntry(string message, CustomExceptionType exceptionType, TraceEventType severity)
        {
            return new LogEntry(message, "General", 0, 100, severity, string.Empty, null);
        }


		#endregion
	}
}