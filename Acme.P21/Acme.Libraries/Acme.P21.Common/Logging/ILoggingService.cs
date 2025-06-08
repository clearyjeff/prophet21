using System;
using Serilog.Events;

namespace Acme.P21.Common.Logging
{
    public interface ILoggingService
    {
        void Debug(string messageTemplate);
        void Debug<T1>(string messageTemplate, T1 propertyValue);
        void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Debug(string messageTemplate, params object[] propertyValues);
        void Debug(Exception exception, string messageTemplate);
        void Debug<T1>(Exception exception, string messageTemplate, T1 propertyValue);
        void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2);

        void Debug(Exception exception, string messageTemplate, params object[] propertyValues);
        void Error(string messageTemplate);
        void Error<T1>(string messageTemplate, T1 propertyValue);
        void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Error(string messageTemplate, params object[] propertyValues);
        void Error(Exception exception, string messageTemplate);
        void Error<T1>(Exception exception, string messageTemplate, T1 propertyValue);
        void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2);

        void Error(Exception exception, string messageTemplate, params object[] propertyValues);
        void Fatal(string messageTemplate);
        void Fatal<T1>(string messageTemplate, T1 propertyValue);
        void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);

        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);

        void Information(string messageTemplate);
        void Information<T1>(string messageTemplate, T1 propertyValue);
        void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Information(string messageTemplate, params object[] propertyValues);
        void Information(Exception exception, string messageTemplate);
        void Information<T1>(Exception exception, string messageTemplate, T1 propertyValue);
        void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2);

        void Information(Exception exception, string messageTemplate, params object[] propertyValues);

        bool IsEnabled(LogEventLevel level);

        void Verbose(string messageTemplate);
        void Verbose<T1>(string messageTemplate, T1 propertyValue);
        void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Verbose(string messageTemplate, params object[] propertyValues);
        void Verbose(Exception exception, string messageTemplate);
        void Verbose<T1>(Exception exception, string messageTemplate, T1 propertyValue);
        void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2);

        void Verbose(Exception exception, string messageTemplate, params object[] propertyValues);

        void Warning(string messageTemplate);
        void Warning<T1>(string messageTemplate, T1 propertyValue);
        void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1);
        void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2);
        void Warning(string messageTemplate, params object[] propertyValues);
        void Warning(Exception exception, string messageTemplate);
        void Warning<T1>(Exception exception, string messageTemplate, T1 propertyValue);
        void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1);

        void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
            T2 propertyValue2);

        void Warning(Exception exception, string messageTemplate, params object[] propertyValues);
    }
}