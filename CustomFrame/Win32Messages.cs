using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomFrame
{
  internal static class Win32Messages
  {
    public const int WM_CREATE = 0x0001;

    public const int WM_ACTIVATE = 0x0006;

    public const int WM_PAINT = 0x000F;

    public const int WM_NCCALCSIZE = 0x0083;
    public const int WM_NCHITTEST = 0x0084;
    public const int WM_NCPAINT = 0x0085;
  }
}
