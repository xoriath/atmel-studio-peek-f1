using System;
using System.Runtime.InteropServices;

namespace Atmel.Studio.Help.Peek.Protocol
{
    public class ProtocolFactory : IClassFactory
    {
        private static IInternetSession GetSession()
        {
            IInternetSession session;
            int res = NativeMethods.CoInternetGetSession(0, out session, 0);

            if (res != NativeConstants.S_OK || session == null)
                throw new InvalidOperationException("CoInternetGetSession failed.");

            return session;
        }

        private Func<IProtocol> _factory;

        private ProtocolFactory(Func<IProtocol> factory)
        {
            _factory = factory;
        }

        public static void Register(string name, Func<IProtocol> factory)
        {
            string emptyStr = null;

            IInternetSession session = GetSession();
            try
            {
                Guid handlerGuid = typeof(Protocol).GUID;
                session.RegisterNameSpace(
                    new ProtocolFactory(factory),
                    ref handlerGuid,
                    name,
                    0,
                    ref emptyStr,
                    0);
            }
            finally
            {
                Marshal.ReleaseComObject(session);
                session = null;
            }
        }

        public void LockServer(bool Lock)
        {
        }

        public int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
        {
            ppvObject = IntPtr.Zero;

            if (pUnkOuter != IntPtr.Zero)
                return NativeConstants.CLASS_E_NOAGGREGATION;

            if (typeof(IInternetProtocol).GUID.Equals(riid)
                || typeof(IInternetProtocolRoot).GUID.Equals(riid)
                || typeof(IInternetProtocolInfo).GUID.Equals(riid))
            {
                object obj = new Protocol(_factory());
                IntPtr objPtr = Marshal.GetIUnknownForObject(obj);
                IntPtr resultPtr;
                Guid refIid = riid;
                Marshal.QueryInterface(objPtr, ref refIid, out resultPtr);
                ppvObject = resultPtr;
                return NativeConstants.S_OK;
            }

            return NativeConstants.E_NOINTERFACE;
        }
    }
}