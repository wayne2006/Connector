using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coldairarrow.DotNettySocket.Protocol
{
    /// <summary>
    /// String type request information
    /// </summary>
    public class StringRequestInfo : RequestInfo<string>
    {
        private static readonly string m_Spliter = " ";
        private static readonly string[] m_ParameterSpliters = new string[] { " " };

        /// <summary>
        /// Initializes a new instance of the <see cref="StringRequestInfo"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="body">The body.</param>
        /// <param name="parameters">The parameters.</param>
        public StringRequestInfo(string key, string body, string[] parameters)
            : base(key, body)
        {
            Parameters = parameters;

            //m_Spliter = " ";
            //m_ParameterSpliters = new string[] { " " };
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public string[] Parameters { get; private set; }

        /// <summary>
        /// Gets the first param.
        /// </summary>
        /// <returns></returns>
        public string GetFirstParam()
        {
            if(Parameters.Length > 0)
                return Parameters[0];

            return string.Empty;
        }

        /// <summary>
        /// Gets the <see cref="System.String"/> at the specified index.
        /// </summary>
        public string this[int index]
        {
            get { return Parameters[index]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        public static StringRequestInfo FromBytes(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);
            return ParseRequestInfo(str);
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return Key+" "+Body;
        }

        /// <summary>
        /// Parses the request info.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static StringRequestInfo ParseRequestInfo(string source)
        {
            int pos = source.IndexOf(m_Spliter);

            string name = string.Empty;
            string param = string.Empty;

            if (pos > 0)
            {
                name = source.Substring(0, pos);
                param = source.Substring(pos + m_Spliter.Length);
            }
            else
            {
                name = source;
            }

            return new StringRequestInfo(name, param,
                param.Split(m_ParameterSpliters, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
