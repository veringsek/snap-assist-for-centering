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
HWND hwndWindow;

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
        RECT windowIndicator;
        GetWindowRect(hwndWindow, &windowIndicator);
        // int width = windowTarget.right - windowTarget.left;
        // int height = windowTarget.bottom - windowTarget.top;
        int width = 100 * 2; 
        int height = 100 * 2;
        SetWindowPos(hwndWindow, HWND_TOP, center.x - width / 2, center.y - height / 2, width, height, SWP_SHOWWINDOW);
        // ShowWindow(hwndWindow, SW_RESTORE);
        SetForegroundWindow(hwndWindow);
        SetFocus(hwndWindow);
        SetWindowPos(hwnd, HWND_TOP, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
        SetFocus(hwnd);
    }
    else
    {
        ShowWindow(hwndWindow, SW_HIDE);
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

const TCHAR CLASS_NAME[] = TEXT("WindowsSnapCenterClass");
LRESULT CALLBACK WinProc(HWND hwnd, UINT wm, WPARAM wp, LPARAM lp);

LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wparam, LPARAM lparam);

int main()
{
    // FreeConsole();
    WNDCLASS windowClass = {0};
    windowClass.hbrBackground = (HBRUSH)GetStockObject(GRAY_BRUSH);
    windowClass.hCursor = LoadCursor(NULL, IDC_ARROW);
    windowClass.hInstance = NULL;
    windowClass.lpfnWndProc = WndProc;
    windowClass.lpszClassName = TEXT("Window in Console");
    windowClass.style = CS_HREDRAW | CS_VREDRAW;
    if (!RegisterClass(&windowClass))
        MessageBox(NULL, TEXT("Could not register class"), TEXT("Error"), MB_OK);
    hwndWindow = CreateWindow(TEXT("Window in Console"),
                              NULL,
                              WS_POPUP,
                              0,
                              0,
                              100,
                              100,
                              NULL,
                              NULL,
                              NULL,
                              NULL);
    SetConsoleCtrlHandler(consoleControlHandler, TRUE);
    Hook();

    MSG messages;
    while (GetMessage(&messages, NULL, 0, 0) > 0)
    {
        TranslateMessage(&messages);
        DispatchMessage(&messages);
    }
    DeleteObject(hwndWindow); //doing it just in case
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