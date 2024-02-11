#include "pch.h"
#include <stdio.h>
#include <stdlib.h>
#include <malloc.h>
#include <windows.h>
#include <mmsystem.h>
#include <winioctl.h>
#pragma comment(lib, "winmm.lib")
#pragma comment(lib, "ntdll.lib")
INPUT down[1], up[1], downa[1], upa[1], MiceW3[1], Micek[1], Micel[1], Micelf[1], Micerc[1], Micercf[1], Micemc[1], Micemcf[1], Micewd[1], Micewu[1];
int size = sizeof(INPUT);

extern "C"
{
	__declspec(dllexport) void MoveMouseTo(int x, int y)
	{
		mouse_event(0x8001, x, y, 0, 0);
	}
	__declspec(dllexport) void MoveMouseBy(int x, int y)
	{
		mouse_event(0x0001, x, y, 0, 0);
	}
	__declspec(dllexport) void MouseMW3(int x, int y)
	{
		MiceW3[0].type = 0;
		MiceW3[0].mi.dwFlags = 0x8001;
		MiceW3[0].mi.dx = x;
		MiceW3[0].mi.dy = y;
		SendInput(1, MiceW3, size);
	}
	__declspec(dllexport) void MouseBrink(int x, int y)
	{
		Micek[0].type = 0;
		Micek[0].mi.dwFlags = 0x0001;
		Micek[0].mi.dx = x;
		Micek[0].mi.dy = y;
		SendInput(1, Micek, size);
	}
	__declspec(dllexport) void SendKey(UINT bVk, UINT bScan)
	{
		keybd_event(bVk, bScan, 0, 0);
	}
	__declspec(dllexport) void SendKeyF(UINT bVk, UINT bScan)
	{
		keybd_event(bVk, bScan, 0x0002, 0);
	}
	__declspec(dllexport) void SendKeyArrows(UINT bVk, UINT bScan)
	{
		keybd_event(bVk, bScan, 0x0001 | 0x0008, 0);
		keybd_event(bVk, bScan, 0, 0);
	}
	__declspec(dllexport) void SendKeyArrowsF(UINT bVk, UINT bScan)
	{
		keybd_event(bVk, bScan, 0x0002 | 0x0001 | 0x0008, 0);
		keybd_event(bVk, bScan, 0x0002, 0);
	}
	__declspec(dllexport) void SendMouseEventButtonLeft()
	{
		mouse_event(0x0002, 0, 0, 0, 0);
	}
	__declspec(dllexport) void SendMouseEventButtonLeftF()
	{
		mouse_event(0x0004, 0, 0, 0, 0);
	}
	__declspec(dllexport) void SendMouseEventButtonRight()
	{
		mouse_event(0x0008, 0, 0, 0, 0);
	}
	__declspec(dllexport) void SendMouseEventButtonRightF()
	{
		mouse_event(0x0010, 0, 0, 0, 0);
	}
	__declspec(dllexport) void SendMouseEventButtonMiddle()
	{
		mouse_event(0x0020, 0, 0, 0, 0);
	}
	__declspec(dllexport) void SendMouseEventButtonMiddleF()
	{
		mouse_event(0x0040, 0, 0, 0, 0);
	}
	__declspec(dllexport) void SendMouseEventButtonWheelUp()
	{
		mouse_event(0x0800, 0, 0, 120, 0);
	}
	__declspec(dllexport) void SendMouseEventButtonWheelDown()
	{
		mouse_event(0x0800, 0, 0, -120, 0);
	}
	__declspec(dllexport) void SimulateKeyDown(UINT keyCode, UINT bScan)
	{
		down[0].type = 1;
		down[0].ki.dwFlags = 0;
		down[0].ki.wVk = keyCode;
		down[0].ki.wScan = bScan;
		SendInput(1, down, size);
	}
	__declspec(dllexport) void SimulateKeyUp(UINT keyCode, UINT bScan)
	{
		up[0].type = 1;
		up[0].ki.dwFlags = 0x0002;
		up[0].ki.wVk = keyCode;
		up[0].ki.wScan = bScan;
		SendInput(1, up, size);
	}
	__declspec(dllexport) void SimulateKeyDownArrows(UINT keyCode, UINT bScan)
	{
		downa[0].type = 1;
		downa[0].ki.wVk = keyCode;
		downa[0].ki.wScan = bScan;
		downa[0].ki.dwFlags = 0x0001 | 0x0008;
		SendInput(1, downa, size);
		downa[0].ki.dwFlags = 0;
		SendInput(1, downa, size);
	}
	__declspec(dllexport) void SimulateKeyUpArrows(UINT keyCode, UINT bScan)
	{
		upa[0].type = 1;
		upa[0].ki.wVk = keyCode;
		upa[0].ki.wScan = bScan;
		upa[0].ki.dwFlags = 0x0002 | 0x0001 | 0x0008;
		SendInput(1, upa, size);
		upa[0].ki.dwFlags = 0x0002;
		SendInput(1, upa, size);
	}
	__declspec(dllexport) void LeftClick()
	{
		Micel[0].type = 0;
		Micel[0].mi.dwFlags = 0x0002;
		SendInput(1, Micel, size);
	}
	__declspec(dllexport) void LeftClickF()
	{
		Micelf[0].type = 0;
		Micelf[0].mi.dwFlags = 0x0004;
		SendInput(1, Micelf, size);
	}
	__declspec(dllexport) void RightClick()
	{
		Micerc[0].type = 0;
		Micerc[0].mi.dwFlags = 0x0008;
		SendInput(1, Micerc, size);
	}
	__declspec(dllexport) void RightClickF()
	{
		Micercf[0].type = 0;
		Micercf[0].mi.dwFlags = 0x0010;
		SendInput(1, Micercf, size);
	}
	__declspec(dllexport) void MiddleClick()
	{
		Micemc[0].type = 0;
		Micemc[0].mi.dwFlags = 0x0020;
		SendInput(1, Micemc, size);
	}
	__declspec(dllexport) void MiddleClickF()
	{
		Micemcf[0].type = 0;
		Micemcf[0].mi.dwFlags = 0x0040;
		SendInput(1, Micemcf, size);
	}
	__declspec(dllexport) void WheelDownF()
	{
		Micewd[0].type = 0;
		Micewd[0].mi.mouseData = -120;
		Micewd[0].mi.dwFlags = 0x0800;
		SendInput(1, Micewd, size);
	}
	__declspec(dllexport) void WheelUpF()
	{
		Micewu[0].type = 0;
		Micewu[0].mi.mouseData = 120;
		Micewu[0].mi.dwFlags = 0x0800;
		SendInput(1, Micewu, size);
	}
}