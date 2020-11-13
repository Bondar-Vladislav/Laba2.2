using System;
using System.Runtime.InteropServices;

namespace Lab2._2
{
    public class NativeMethods
    {
        private enum ErrorModes : uint
        {
            SYSTEM_DEFAULT = 0x0,
            SEM_FAILCRITICALERRORS = 0x0001,
            SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,
            SEM_NOGPFAULTERRORBOX = 0x0002,
            SEM_NOOPENFILEERRORBOX = 0x8000
        }

        [DllImport("kernel32.dll")]
        private static extern ErrorModes SetErrorMode(ErrorModes modes);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hLibrary);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hLibrary, string name);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate double _TheFuncDelegate(double x);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr _FuncNameDelegate();

        private IntPtr GetLibrary(string path)
        {
            var oldMode = SetErrorMode(ErrorModes.SEM_FAILCRITICALERRORS);
            try
            {
                return LoadLibrary(path);
            }
            finally
            {
                SetErrorMode(oldMode);
            }
        }

        private T GetFunction<T>(string functionName) where T : class
        {
            var address = GetProcAddress(hLibrary, functionName);

            if (address == IntPtr.Zero)
                throw new Exception($"GetFunction<{typeof(T).Name}>({functionName})");

            return Marshal.GetDelegateForFunctionPointer(address, typeof(T)) as T;
        }

        private readonly IntPtr hLibrary;
        private readonly _TheFuncDelegate _TheFunc;
        private readonly _FuncNameDelegate _FuncName;

        public double TheFunc(double x) => this._TheFunc(x);
        public string FuncName() => Marshal.PtrToStringAnsi(this._FuncName());

        public NativeMethods(string path)
        {
            this.hLibrary = GetLibrary(path);

            if (this.hLibrary == IntPtr.Zero)
                throw new Exception($"NativeMethods({path})");

            this._TheFunc = GetFunction<_TheFuncDelegate>("TheFunc");
            this._FuncName = GetFunction<_FuncNameDelegate>("FuncName");
        }

        ~NativeMethods()
        {
            if (this.hLibrary == IntPtr.Zero)
                return;

            FreeLibrary(this.hLibrary);
        }
    }
}
