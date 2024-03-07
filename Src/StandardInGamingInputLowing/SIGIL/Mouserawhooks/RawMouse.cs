using System;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RawInput_dll
{
	public sealed class RawMouse
	{
		private readonly Dictionary<IntPtr,ButtonPressEvent> _deviceList = new Dictionary<IntPtr,ButtonPressEvent>();
		public delegate void DeviceEventHandler(object sender, RawInputEventArg e);
		public event DeviceEventHandler ButtonPressed;
		readonly object _padLock = new object();
		public int NumberOfMouses { get; private set; }
		static InputData _rawBuffer;

		public RawMouse(IntPtr hwnd, bool captureOnlyInForeground)
		{
			var rid = new RawInputDevice[1];

			rid[0].UsagePage = HidUsagePage.GENERIC;       
			rid[0].Usage = HidUsage.Mouse;              
            rid[0].Flags = (captureOnlyInForeground ? RawInputDeviceFlags.NONE : RawInputDeviceFlags.INPUTSINK) | RawInputDeviceFlags.DEVNOTIFY;
			rid[0].Target = hwnd;

			if(!Win32.RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])))
			{
				throw new ApplicationException("Failed to register raw input device(s).");
			}
		}

		public void EnumerateDevices()
		{
			lock (_padLock)
			{
				_deviceList.Clear();

				var mouseNumber = 0;

				var globalDevice = new ButtonPressEvent
				{
					DeviceName = "Global Mouse",
					DeviceHandle = IntPtr.Zero,
					DeviceType = Win32.GetDeviceType(DeviceType.RimTypemouse),
					Name = "Fake Mouse. Some buttons are sent to rawinput with a handle of zero.",
					Source = mouseNumber++.ToString(CultureInfo.InvariantCulture)
				};

				_deviceList.Add(globalDevice.DeviceHandle, globalDevice);
				
				var numberOfDevices = 0;
				uint deviceCount = 0;
				var dwSize = (Marshal.SizeOf(typeof(Rawinputdevicelist)));

				if (Win32.GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)dwSize) == 0)
				{
					var pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
					Win32.GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint)dwSize);

					for (var i = 0; i < deviceCount; i++)
					{
						uint pcbSize = 0;

						// On Window 8 64bit when compiling against .Net > 3.5 using .ToInt32 you will generate an arithmetic overflow. Leave as it is for 32bit/64bit applications
						var rid = (Rawinputdevicelist)Marshal.PtrToStructure(new IntPtr((pRawInputDeviceList.ToInt64() + (dwSize * i))), typeof(Rawinputdevicelist));

						Win32.GetRawInputDeviceInfo(rid.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);

						if (pcbSize <= 0) continue;

						var pData = Marshal.AllocHGlobal((int)pcbSize);
						Win32.GetRawInputDeviceInfo(rid.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, pData, ref pcbSize);
						var deviceName = Marshal.PtrToStringAnsi(pData);

                        if (rid.dwType == DeviceType.RimTypemouse || rid.dwType == DeviceType.RimTypeHid)
						{
							var deviceDesc = Win32.GetDeviceDescription(deviceName);

							var dInfo = new ButtonPressEvent
							{
								DeviceName = Marshal.PtrToStringAnsi(pData),
								DeviceHandle = rid.hDevice,
								DeviceType = Win32.GetDeviceType(rid.dwType),
								Name = deviceDesc,
								Source = mouseNumber++.ToString(CultureInfo.InvariantCulture)
							};
						   
							if (!_deviceList.ContainsKey(rid.hDevice))
							{
								numberOfDevices++;
								_deviceList.Add(rid.hDevice, dInfo);
							}
						}

						Marshal.FreeHGlobal(pData);
					}

					Marshal.FreeHGlobal(pRawInputDeviceList);

					NumberOfMouses = numberOfDevices;
					Debug.WriteLine("EnumerateDevices() found {0} Mouse(s)", NumberOfMouses);
					return;
				}
			}
			
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}
	   
		public void ProcessRawInput(IntPtr hdevice)
		{
			//Debug.WriteLine(_rawBuffer.data.mouse.ToString());
			//Debug.WriteLine(_rawBuffer.data.hid.ToString());
			//Debug.WriteLine(_rawBuffer.header.ToString());

			if (_deviceList.Count == 0) return;

			var dwSize = 0;
			Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, IntPtr.Zero, ref dwSize, Marshal.SizeOf(typeof(Rawinputheader)));

			if (dwSize != Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, out _rawBuffer, ref dwSize, Marshal.SizeOf(typeof (Rawinputheader))))
			{
				Debug.WriteLine("Error getting the rawinput buffer");
				return;
			}

			ButtonPressEvent buttonPressEvent;

			if (_deviceList.ContainsKey(_rawBuffer.header.hDevice))
			{
				lock (_padLock)
				{
					buttonPressEvent = _deviceList[_rawBuffer.header.hDevice];
				}
			}
			else
			{
				Debug.WriteLine("Handle: {0} was not in the device list.", _rawBuffer.header.hDevice);
				return;
			}
			
			buttonPressEvent.lLastX = _rawBuffer.data.mouse.lLastX;
            buttonPressEvent.lLastY = _rawBuffer.data.mouse.lLastY;
            buttonPressEvent.ulButtons = _rawBuffer.data.mouse.ulButtons;
			buttonPressEvent.ulExtraInformation = _rawBuffer.data.mouse.ulExtraInformation;
            buttonPressEvent.usButtonData = _rawBuffer.data.mouse.usButtonData;
            buttonPressEvent.usButtonFlags = _rawBuffer.data.mouse.usButtonFlags;
            buttonPressEvent.usFlags = _rawBuffer.data.mouse.usFlags;

            if (ButtonPressed != null)
			{
				ButtonPressed(this, new RawInputEventArg(buttonPressEvent));
			}
		}
	}
}