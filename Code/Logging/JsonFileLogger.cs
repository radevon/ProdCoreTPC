using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace ProdCoreTPC.Code.Logging
{
    public class JsonFileLogger : ILogger, IDisposable
    {
        object _lock = new object();
        string _filePath;
        public JsonFileLogger(string filePath)
        {
            _filePath = filePath;
        }
        public IDisposable BeginScope<TState>(TState state) => this;
       
        public void Dispose() { }
        public bool IsEnabled(LogLevel logLevel) => true;
       
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var jsonLine = JsonConvert.SerializeObject(new
            {
                logLevel,
                time=DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                eventId,
                parameters = (state as IEnumerable<KeyValuePair<string, object>>)?.ToDictionary(i => i.Key, i => i.Value),
                message = formatter(state, exception),
                exception = exception?.GetType().Name,
                stackTrace = exception?.StackTrace
            }, Formatting.Indented,new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
            lock (_lock)
            {
                try
                {
                    if (!Directory.Exists(_filePath))
                        Directory.CreateDirectory(_filePath.TrimEnd('/'));

                    File.AppendAllText(_filePath.TrimEnd('/') + "/" + DateTime.Now.ToString("yyyy_MM_dd") + "__log.json", jsonLine + Environment.NewLine + Environment.NewLine);
                
                }catch(Exception)
                {

                }
                
            }
        }
    }
}
