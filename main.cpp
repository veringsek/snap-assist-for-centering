﻿#ifdef _UNICODE
#ifndef UNICODE
#define UNICODE
#endif
#endif

#include <Windows.h>
#include <iostream>
using namespace std;

// Constants
int SENSOR_WIDTH = 100;
int SENSOR_HEIGHT = 100;

// Globals
HWINEVENTHOOK hook;
HWND hwndAssist;

// Header
void terminates();
void Hook();
void Unhook();

void CALLBACK WinEventProc(HWINEVENTHOOK hook, DWORD event, HWND hwnd, LONG idObject, LONG idChild, DWORD idEventThread, DWORD eventTime)
{
    RECT windowTarget;
    GetWindowRect(hwnd, &windowTarget);

    if (event == EVENT_SYSTEM_MOVESIZESTART)
    {
        RECT workarea;
        SystemParametersInfoA(SPI_GETWORKAREA, 0, &workarea, 0);
        POINT center;
        center.x = (workarea.left + workarea.right) / 2;
        center.y = (workarea.top + workarea.bottom) / 2;

        int width = windowTarget.right - windowTarget.left;
        int height = windowTarget.bottom - windowTarget.top;
        POINT end;
        end.x = center.x - width / 2;
        end.y = center.y - height / 2;
        SetWindowPos(hwndAssist, HWND_NOTOPMOST, end.x, end.y, width, height, SWP_NOACTIVATE);
        // SetWindowPos(hwndAssist, HWND_NOTOPMOST, center.x - SENSOR_WIDTH / 2, center.y - SENSOR_HEIGHT / 2, SENSOR_WIDTH, SENSOR_HEIGHT, SWP_NOACTIVATE);
        ShowWindow(hwndAssist, SW_SHOWNOACTIVATE);
    }
    else if (event == EVENT_SYSTEM_MOVESIZEEND)
    {
        // ShowWindow(hwndAssist, SW_HIDE);
        POINT cursor;
        GetCursorPos(&cursor);
        RECT workarea;
        SystemParametersInfoA(SPI_GETWORKAREA, 0, &workarea, 0);
        POINT center;
        center.x = (workarea.left + workarea.right) / 2;
        center.y = (workarea.top + workarea.bottom) / 2;
        int dx = cursor.x - center.x;
        int dy = cursor.y - center.y;
        if ((-SENSOR_WIDTH <= dx && dx <= SENSOR_WIDTH) && (-SENSOR_HEIGHT <= dy && dy <= SENSOR_HEIGHT))
        {
            int width = windowTarget.right - windowTarget.left;
            int height = windowTarget.bottom - windowTarget.top;
            POINT end;
            end.x = center.x - width / 2;
            end.y = center.y - height / 2;
            SetWindowPos(hwnd, NULL, end.x, end.y, width, height, SWP_SHOWWINDOW | SWP_NOSIZE);
        }
    }
}

BOOL WINAPI consoleControlHandler(DWORD sig)
{
    terminates();
    return true;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wparam, LPARAM lparam);

int main()
{
    // FreeConsole();
    SetConsoleCtrlHandler(consoleControlHandler, TRUE);
    Hook();

    WNDCLASS windowClass = {0};
    windowClass.hbrBackground = (HBRUSH)GetStockObject(GRAY_BRUSH);
    windowClass.hCursor = LoadCursor(NULL, IDC_ARROW);
    windowClass.hInstance = NULL;
    windowClass.lpfnWndProc = WndProc;
    windowClass.lpszClassName = TEXT("Window in Console");
    windowClass.style = CS_HREDRAW | CS_VREDRAW;
    if (!RegisterClass(&windowClass))
    {
        MessageBox(NULL, TEXT("Could not register class"), TEXT("Error"), MB_OK);
    }
    hwndAssist = CreateWindow(
        TEXT("Window in Console"),
        NULL,
        WS_POPUP,
        0, 0,
        SENSOR_WIDTH, SENSOR_HEIGHT,
        NULL, NULL, NULL, NULL);

    MSG messages;
    while (GetMessage(&messages, NULL, 0, 0) > 0)
    {
        TranslateMessage(&messages);
        DispatchMessage(&messages);
    }
    return messages.wParam;
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wparam, LPARAM lparam)
{
    switch (message)
    {
    case WM_CHAR: //this is just for a program exit besides window's borders/task bar
        if (wparam == VK_ESCAPE)
        {
            DestroyWindow(hwnd);
        }
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    case WM_MOUSEMOVE:
        POINT cursor;
        GetCursorPos(&cursor);
        // cout << cursor.x << ", " << cursor.y << endl;
        MessageBox(NULL, TEXT("CCC"), TEXT("Error"), MB_OK);
        break;
    default:
        return DefWindowProc(hwnd, message, wparam, lparam);
    }
    return 0;
}

void terminates()
{
    Unhook();
    exit(1);
}

void Hook()
{
    hook = SetWinEventHook(EVENT_SYSTEM_MOVESIZESTART, EVENT_SYSTEM_MOVESIZEEND, NULL, WinEventProc, 0, 0, WINEVENT_OUTOFCONTEXT);
    cout << "Hooked. " << endl;
}

void Unhook()
{
    UnhookWinEvent(hook);
    cout << "Unhooked. " << endl;
}