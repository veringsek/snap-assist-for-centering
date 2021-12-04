#ifdef _UNICODE
#ifndef UNICODE
#define UNICODE
#endif
#endif

#include <Windows.h>
#include <iostream>
using namespace std;

// Globals
HWINEVENTHOOK hook;

// Header
void terminates();
void Hook();
void Unhook();

void CALLBACK WinEventProc(HWINEVENTHOOK hook, DWORD event, HWND hwnd, LONG idObject, LONG idChild, DWORD idEventThread, DWORD eventTime)
{
    RECT windowTarget;
    GetWindowRect(hwnd, &windowTarget);
    if (event == EVENT_SYSTEM_MOVESIZEEND) {
        POINT cursor;
        GetCursorPos(&cursor);
        RECT workarea;
        SystemParametersInfoA(SPI_GETWORKAREA, 0, &workarea, 0);
        POINT center;
        center.x = (workarea.left + workarea.right) / 2;
        center.y = (workarea.top + workarea.bottom) / 2;
        int dx = cursor.x - center.x;
        int dy = cursor.y - center.y;
        if ((-100 <= dx && dx <= 100) && (-100 <= dy && dy <= 100))
        {
            POINT start;
            start.x = windowTarget.left;
            start.y = windowTarget.top;
            int width = windowTarget.right - windowTarget.left;
            int height = windowTarget.bottom - windowTarget.top;
            POINT end;
            end.x = center.x - width / 2;
            end.y = center.y - height / 2;
            SetWindowPos(hwnd, NULL, end.x, end.y, width, height, SWP_SHOWWINDOW);
        }
    }
}

BOOL WINAPI consoleControlHandler(DWORD sig)
{
    terminates();
    return true;
}

int main()
{
    FreeConsole();
    SetConsoleCtrlHandler(consoleControlHandler, TRUE);
    Hook();

    MSG messages;
    while (GetMessage(&messages, NULL, 0, 0) > 0)
    {
        TranslateMessage(&messages);
        DispatchMessage(&messages);
    }
    return messages.wParam;
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