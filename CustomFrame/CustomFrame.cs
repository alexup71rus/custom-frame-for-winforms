using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CustomFrame
{
  public partial class CustomFrame : Form
  {
    protected int BorderTop {get; private set;}
    protected int BorderLeft { get; private set; }
    protected int BorderRight {get; private set;}
    protected int BorderBottom {get; private set;}

    public CustomFrame()
    {
      InitializeComponent();
      SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
    }


    protected override void WndProc(ref Message m)
    {
      IntPtr result = IntPtr.Zero;

      bool callDWP = !Win32Interop.DwmDefWindowProc(m.HWnd, m.Msg, m.WParam, m.LParam, out result);
      
      switch (m.Msg)
      {
        case Win32Messages.WM_CREATE:
          {
            RECT rc;
            int style = Win32Interop.GetWindowLong(m.HWnd, Win32Constants.GWL_STYLE);
            int styleEx = Win32Interop.GetWindowLong(m.HWnd, Win32Constants.GWL_EXSTYLE);
            Win32Interop.AdjustWindowRectEx(out rc, style, false, styleEx);

            //BorderTop = Math.Abs(rc.top);
            //BorderLeft = Math.Abs(rc.left);
            BorderRight = Math.Abs(rc.right);
            BorderBottom = Math.Abs(rc.bottom);
          }
          break;

        case Win32Messages.WM_ACTIVATE:
          {
            MARGINS margins = new MARGINS();

            margins.cxLeftWidth = Math.Abs(BorderLeft);
            margins.cxRightWidth = Math.Abs(BorderRight);
            margins.cyBottomHeight = Math.Abs(BorderBottom);
            margins.cyTopHeight = Math.Abs(BorderTop);

            int hr = Win32Interop.DwmExtendFrameIntoClientArea(m.HWnd, ref margins);

            result = IntPtr.Zero;
          }
          break;

        case Win32Messages.WM_NCCALCSIZE:
          {
            if (m.WParam != IntPtr.Zero)
            {
              result = IntPtr.Zero;
              callDWP = false;
            }
          }
          break;

        case Win32Messages.WM_NCHITTEST:
          {
            int ht = NCHitText(m);

            callDWP = (ht == Win32Constants.HTNOWHERE);            
            result = new IntPtr(ht);            
          }
          break;
      }

      m.Result = result;
      if (callDWP)
      {
        base.WndProc(ref m);
      }
    }

    private int NCHitText(Message m)
    {
      int lParam = (int)m.LParam;
      Point pt = new Point(lParam & 0xffff, lParam >> 16);

      Rectangle rc = new Rectangle(this.Location, ClientRectangle.Size);      
      
      int row = 1;
      int col = 1;
      bool onResizeBorder = false;
      
      // Determine if we are on the top or bottom border
      if (pt.Y >= rc.Top && pt.Y < rc.Top + BorderTop)
      {
        onResizeBorder = pt.Y < (rc.Top + BorderBottom);
        row = 0;        
      }
      else if (pt.Y < rc.Bottom && pt.Y > rc.Bottom - BorderBottom)
      {
        row = 2;
      }

      // Determine if we are on the left border or the right border
      if (pt.X >= rc.Left && pt.X < rc.Left + BorderLeft)
      {
        col = 0;
      }
      else if (pt.X < rc.Right && pt.X >= rc.Right - BorderRight)
      {
        col = 2;
      }

      int[,] hitTests = new int[,]
      {
        {Win32Constants.HTTOPLEFT, onResizeBorder ? Win32Constants.HTTOP : Win32Constants.HTCAPTION, Win32Constants.HTTOPRIGHT},
        {Win32Constants.HTLEFT, Win32Constants.HTNOWHERE, Win32Constants.HTRIGHT},
        {Win32Constants.HTBOTTOMLEFT, Win32Constants.HTBOTTOM, Win32Constants.HTBOTTOMRIGHT}
      };
      
      return hitTests[row, col];
    }
  }
}
