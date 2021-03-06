﻿using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Sriracha.Data.Impersonation
{
	public class ImpersonationContext : IDisposable
	{
		private readonly DeployCredentialsMasked _credentials;
		private readonly SafeTokenHandle _handle;
		private readonly WindowsImpersonationContext _context;

		public ImpersonationContext(DeployCredentialsMasked credentials, SafeTokenHandle handle, WindowsImpersonationContext context)
		{
			_credentials = credentials;
			_handle = handle;
			_context = context;
		}

		public SafeTokenHandle TokenHandle 
		{ 
			get
			{
				return _handle;
			}
		}

		public DeployCredentialsMasked Credentials
		{
			get 
			{
				return _credentials;
			}
		}

		public void Dispose()
		{
			this._context.Dispose();
			this._handle.Dispose();
		}

		public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			private SafeTokenHandle()
				: base(true) { }

			[DllImport("kernel32.dll")]
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool CloseHandle(IntPtr handle);

			protected override bool ReleaseHandle()
			{
				return CloseHandle(handle);
			}
		}
    }
}
