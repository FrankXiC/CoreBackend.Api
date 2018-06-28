using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using log4net.Repository;

namespace CoreBackend.Api.Models
{

    public class SDKProperties {
        public static ILoggerRepository LogRepository { get; set; }
    }
    public class Base {
        private static readonly ILog log = LogManager.GetLogger(SDKProperties.LogRepository.Name, typeof(Base));

        public static void LogTest() {
            log.Info("Info test");
            log.Warn("Warn test");
            log.Error("Error test");
        }
    }
}
