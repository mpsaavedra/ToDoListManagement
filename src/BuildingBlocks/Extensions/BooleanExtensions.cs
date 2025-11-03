using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootler;

public static class BooleanExtensions
{
    public static void IsFalseThrow(this bool source, string message) =>
        source.IsFalseThrow<ApplicationException>(message);

    public static void IsFalseThrow<TException>(this bool source, string message)
        where TException : Exception, new()
    {
        if (source) return;

        throw (TException)Activator.CreateInstance(typeof(TException), message)!;
    }
}
