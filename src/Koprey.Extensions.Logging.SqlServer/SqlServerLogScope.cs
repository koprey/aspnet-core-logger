using System;
using System.Threading;

namespace Koprey.Extensions.Logging.SqlServer
{
    public class SqlServerLogScope
    {
        private readonly string _name;
        private readonly object _state;

        internal SqlServerLogScope(string name, object state)
        {
            _name = name;
            _state = state;
        }

        public SqlServerLogScope Parent { get; private set; }

        private static AsyncLocal<SqlServerLogScope> _value = new AsyncLocal<SqlServerLogScope>();
        public static SqlServerLogScope Current
        {
            set
            {
                _value.Value = value;
            }
            get
            {
                return _value.Value;
            }
        }

        public static IDisposable Push(string name, object state)
        {
            var temp = Current;
            Current = new SqlServerLogScope(name, state);
            Current.Parent = temp;

            return new DisposableScope();
        }

        public override string ToString()
        {
            return _state?.ToString();
        }

        private class DisposableScope : IDisposable
        {
            public void Dispose()
            {
                Current = Current.Parent;
            }
        }
    }
}
