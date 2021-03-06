﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loggly.Config;
using NLog;

namespace Demo
{
    public class Vehicle
    {
        public string Name { get; set; }
        public int Wheels { get; set; }

        public int EngineSize { get; set; }
    }

    public class Truck : Vehicle
    {
        public Truck()
        {
            Wheels = 10;
            EngineSize = 12;
            Name = "Mack Truck";
        }
    }

    class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        
        static void Main(string[] args)
        {
            StandardLogging();
            StandardLogException();

            LogWithObjectMetadata();
            LogExceptionWithObjectMetadata();

            LogToTargetsButSuppressToLoggly();

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
        
        private static void StandardLogging()
        {
            var nLogVersion = typeof(Logger).Assembly.GetName().Version;
            Log.Info("NLog.Targarts.Loggly demo starting up using loggly transport={0}, nlog version={1}", LogglyConfig.Instance.Transport, nLogVersion);
        }
        private static void StandardLogException()
        {
            Log.Error(new Exception("Cable unplugged"), "Did someone unplug the cable?");
        }
        private static void LogWithObjectMetadata()
        {
            var eventInfo = new LogEventInfo(LogLevel.Info, "MyLogName", "event info test message");
            eventInfo.Properties.Add("VehicleParameter", new Truck());
            Log.Log(eventInfo);
        }
        private static void LogExceptionWithObjectMetadata()
        {
            var eventInfo2 = new LogEventInfo(LogLevel.Error, "MyLogName", "something broke");
            eventInfo2.Exception = new Exception("This is a really bad exception");
            eventInfo2.Properties.Add("CustomParameter", 987654);
            Log.Log(eventInfo2);
        }

        private static void LogToTargetsButSuppressToLoggly()
        {
            var eventInfo2 = new LogEventInfo(LogLevel.Info, "MyLogName", "weeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee!");
            eventInfo2.Properties.Add("syslog-suppress", true);
            Log.Log(eventInfo2);
        }
    }
}
