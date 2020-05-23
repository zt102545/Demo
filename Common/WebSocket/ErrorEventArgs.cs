using System;

namespace Common
{
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(Exception exception, string message = null)
        {
            Exception = exception;
            Message = message;
        }
        public Exception Exception { get; private set; }
        public string Message { get; private set; }
    }
}
