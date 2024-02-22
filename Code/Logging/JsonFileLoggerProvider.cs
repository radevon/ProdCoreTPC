using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdCoreTPC.Code.Logging
{
    public class JsonFileLoggerProvider : ILoggerProvider
    {
        string path;
        public JsonFileLoggerProvider(string path)
        {
            this.path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new JsonFileLogger(path);
        }
        public void Dispose() { }
    }
}
