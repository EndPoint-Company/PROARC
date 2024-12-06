using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PROARC.src.Control.Database
{
    public static class DatabaseUtil
    {
        public static SecureString? ToSecureString(this string? sender)
        {
            if (sender == null) return null;

            return sender.Aggregate(new SecureString(), (item, c) => { item.AppendChar(c); return item; },
                (item) => { item.MakeReadOnly(); return item; });
        }
        public static string? ToUnSecureString(this SecureString? sender)
        {
            if (sender == null) return null;

            var unmanagedString = IntPtr.Zero;

            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(sender);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
