#ifdef _UNICODE
#ifndef UNICODE
#define UNICODE
#endif
#endif

#include <Windows.h>
#include <iostream>
using namespace std;

// Constants
int SENSOR_WIDTH = 50;
int SENSOR_HEIGHT = 50;

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
        SetWindowPos(hwndAssist, HWND_TOPMOST, end.x, end.y, width, height, SWP_NOACTIVATE);
        ShowWindow(hwndAssist, SW_SHOWNOACTIVATE);

        HDC hdc;
        hdc = GetDC(hwndAssist);
        RECT sensor;
        SetRect(&sensor, width / 2 - SENSOR_WIDTH, height / 2 - SENSOR_HEIGHT, width / 2 + SENSOR_WIDTH, height / 2 + SENSOR_HEIGHT);
        FillRect(hdc, &sensor, (HBRUSH)GetStockObject(BLACK_BRUSH));
        ReleaseDC(hwndAssist, hdc);
    }
    else if (event == EVENT_SYSTEM_MOVESIZEEND)
    {
        ShowWindow(hwndAssist, SW_HIDE);
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
            WINDOWPLACEMENT placement;
            GetWindowPlacement(hwnd, &placement);
            placement.rcNormalPosition.left = end.x;
            placement.rcNormalPosition.right = end.x + width;
            placement.rcNormalPosition.top = end.y;
            placement.rcNormalPosition.bottom = end.y + height;
            placement.showCmd = SW_SHOW;
            SetWindowPlacement(hwnd, &placement);
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
    windowClass.hbrBackground = (HBRUSH)GetStockObject(NULL_BRUSH);
    windowClass.hCursor = LoadCursor(NULL, IDC_ARROW);
    windowClass.hInstance = NULL;
    windowClass.lpfnWndProc = WndProc;
    windowClass.lpszClassName = TEXT("SnapAssistForCentering");
    windowClass.style = CS_HREDRAW | CS_VREDRAW;
    if (!RegisterClass(&windowClass))
    {
        MessageBox(NULL, TEXT("Could not register class"), TEXT("Error"), MB_OK);
    }
    hwndAssist = CreateWindow(
        TEXT("SnapAssistForCentering"),
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