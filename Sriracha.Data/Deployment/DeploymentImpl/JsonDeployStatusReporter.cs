using Common.Logging;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Deployment.DeploymentImpl
{
    public class JsonDeployStatusReporter : IDeployStatusReporter, IDisposable
    {
        private Stream _outputStream;
        private StreamWriter _streamWriter;
        
        public class JsonMessage
        {
            public DateTime DateTimeUtc { get; set; }
            public LogLevel LogLevel { get; set; }
            public string Message { get; set; }
            public string Detail { get; set; }
        }

        public JsonDeployStatusReporter(Stream outputStream)
        {
            _outputStream = outputStream;
            _streamWriter = new StreamWriter(outputStream);
        }

        public void Dispose()
        {
            //if(_outputStream != null)
            //{
            //    using(_outputStream)
            //    {
            //        _outputStream = null;
            //    }
            //}
            if(_streamWriter != null)
            {
                using(_streamWriter)
                {
                    _streamWriter = null;
                }
            }
        }

        private void InternalLog(string message, string detail, LogLevel logLevel, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                message = string.Format(message, args);
            }
            var item = new JsonMessage()
            {
                DateTimeUtc = DateTime.UtcNow,
                LogLevel = logLevel,
                Message = message, 
                Detail = detail
            };
            _streamWriter.WriteLine(item.ToJson(false));
            _streamWriter.Flush();
        }

        public void Info(string message, params object[] args)
        {
            this.InternalLog(message, null, LogLevel.Info, args);
        }

        public void Debug(string message, params object[] args)
        {
            this.InternalLog(message, null, LogLevel.Debug, args);
        }

        public void Error(string message, object[] args)
        {
            this.InternalLog(message, null, LogLevel.Error, args);
        }


        public void ErrorException(Exception err)
        {
            this.InternalLog(err.Message, err.ToString(), LogLevel.Error);
        }

        public void ErrorException(Exception err, string message, params object[] args)
        {
            this.InternalLog(message, err.ToString(), LogLevel.Error, args);
        }
    }
}
