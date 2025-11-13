using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Logging;

namespace LoggingCS.LoggingConfiguration
{
   public class LoggerConfiguration
    {
        public LoggerType LoggerType { get; set; }
        //public DatabaseLoggerConfiguration DatabaseLoggerConfiguration{ get; set; }
        public TextLoggerConfiguration? TextLoggerConfiguration { get; set; }
    }

     public class TextLoggerConfiguration
    {
        public string? Directory { get; set; }
        public string? Filename { get; set; }
        public string? FileExtension { get; set; }

    }
}
