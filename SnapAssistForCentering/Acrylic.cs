using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SnapAssistForCentering
{
    internal static class Acrylic
    {
        public const int WS_SYSMENU = 0x80000;
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_TOPMOST = 0x00000008;

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        internal enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_GRADIENT = 1,
            ACCENT_TRANSPARENT_GRADIENT = 2,
            ACCENT_BLUR_BEHIND = 3,
            ACCENT_ACRYLIC_BLUR_BEHIND = 4,
            ACCENT_INVALID_STATE = 5
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public uint AccentFlags;
            public uint GradientColor;
            public uint AnimationId;
        }

        internal enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public int SizeOfData;
            public IntPtr Data;
        }
        public static void Enable(IntPtr HWnd, byte alpha = 0x60, byte red = 0x30, byte green = 0x30, byte blue = 0x30, bool hasFrame = true)
        {
            AccentPolicy accent = new AccentPolicy
            {
                AccentState = AccentState.ACCENT_ACRYLIC_BLUR_BEHIND,
                AccentFlags = (uint)(hasFrame ? 0x20 | 0x40 | 0x80 | 0x100 : 0x0),
                GradientColor = ((uint)alpha << 24) | ((uint)blue << 16) | ((uint)green << 8) | red
            };
            int accentStructSize = Marshal.SizeOf(accent);
            IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);
            WindowCompositionAttributeData data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };
            SetWindowCompositionAttribute(HWnd, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }
    }
}
