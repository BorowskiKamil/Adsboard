using System;

namespace Adsboard.Common.Types
{
    public class AdsboardException : Exception
    {
        public string Code { get; }

        public AdsboardException()
        {
        }

        public AdsboardException(string code)
        {
            Code = code;
        }

        public AdsboardException(string message, params object[] args) 
            : this(string.Empty, message, args)
        {
        }

        public AdsboardException(string code, string message, params object[] args) 
            : this(null, code, message, args)
        {
        }

        public AdsboardException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        public AdsboardException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}