using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Automation.Peers;

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

        [RequiresUnreferencedCode("Chama JsonSerializer.Deserialize")]
        public static T ReadJson<T>(string filePath)
        {
            string text = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<T>(json: text);
        }
    }

    public class SQLBuilder
    {
        public bool encrypt { get; set; }
        public string dataSource { get; set; }
        public string user { get; set; }
        public string password { get; set; }     
    }
}
