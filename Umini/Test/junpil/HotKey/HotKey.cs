using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Umini.Test.junpil
{
	/// <summary>
	/// Class allow to register hotkeys in system 
	/// </summary>
	public sealed class HotKey : IDisposable
	{
		public event Action<HotKey> HotKeyPressed;

		private readonly int _id;
		private bool _isKeyRegistered;
		private readonly IntPtr _handle;

		private bool _disposed;

		public HotKey(ModifierKeys modifierKeys, Keys key, Window window)
			: this(modifierKeys, key, new WindowInteropHelper(window))
		{
			Contract.Requires(window != null);
		}

		public HotKey(ModifierKeys modifierKeys, Keys key, WindowInteropHelper window)
			: this(modifierKeys, key, window.Handle)
		{
			Contract.Requires(window != null);
		}

		public HotKey(ModifierKeys modifierKeys, Keys key, IntPtr windowHandle)
		{
			Contract.Requires(modifierKeys != ModifierKeys.None || key != Keys.None);
			Contract.Requires(windowHandle != IntPtr.Zero);

			Key = key;
			KeyModifier = modifierKeys;
			_id = GetHashCode();
			_handle = windowHandle;
			RegisterHotKey();
			ComponentDispatcher.ThreadPreprocessMessage += ThreadPreprocessMessageMethod;
		}

		~HotKey()
		{
			Dispose(false);
		}

		public Keys Key { get; private set; }

		public ModifierKeys KeyModifier { get; private set; }

		public void RegisterHotKey()
        {
            try
            {
                if (Key == Keys.None)
                    return;
                if (_isKeyRegistered)
                    UnregisterHotKey();
                _isKeyRegistered = HotKeyWinApi.RegisterHotKey(_handle, _id, KeyModifier, Key);
                if (!_isKeyRegistered)
                    throw new ApplicationException("Hotkey already in use");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
		}

		public void UnregisterHotKey()
		{
			_isKeyRegistered = !HotKeyWinApi.UnregisterHotKey(_handle, _id);
		}

		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					ComponentDispatcher.ThreadPreprocessMessage -= ThreadPreprocessMessageMethod;
				}

				UnregisterHotKey();
				_disposed = true;
			}
		}

		private void ThreadPreprocessMessageMethod(ref MSG msg, ref bool handled)
		{
			if (!handled)
			{
				if (msg.message == HotKeyWinApi.WmHotKey
				    && (int) (msg.wParam) == _id)
				{
					OnHotKeyPressed();
					handled = true;
				}
			}
		}

		private void OnHotKeyPressed()
		{
			if (HotKeyPressed != null)
				HotKeyPressed(this);
		}
	}
}