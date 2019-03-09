using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace CustomFrame
{
    public partial class Form1 : CustomFrame
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.Style |= (int)0x00040000L;//(int)0x00C00000L ; //WS_SIZEBOX;
                return cp;
            }
        }
        public Form1()
        {
            InitializeComponent();
            panel1.MouseDown += delegate
            {
                panel1.Capture = false;
                var msg = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            };
            label1.MouseDown += delegate
            {
                label1.Capture = false;
                var msg = Message.Create(this.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref msg);
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Normal)
            {
                x2 = x;
                y2 = y;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                Size = new Size(x2, y2);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        bool ChangeSize;
        int x = 323;
        int y = 408;
        int x2 = 323;
        int y2 = 408;
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (ChangeSize == true)
            {
                try
                {
                    x = CustomFrame.ActiveForm.Size.Width;
                    y = CustomFrame.ActiveForm.Size.Height;
                    Size = new Size(CustomFrame.ActiveForm.Size.Width, CustomFrame.ActiveForm.Size.Height);
                }
                catch
                {
                    Size = new Size(x, y);
                }
            }
            else 
            {
                ChangeSize = true;
                Size = new Size(x, y);
            }
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            ChangeSize = false;
            x = CustomFrame.ActiveForm.Size.Width;
            y = CustomFrame.ActiveForm.Size.Height;
            Size = new Size(CustomFrame.ActiveForm.Size.Width, CustomFrame.ActiveForm.Size.Height);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ChangeSize = false;
            x = CustomFrame.ActiveForm.Size.Width;
            y = CustomFrame.ActiveForm.Size.Height;
            Size = new Size(CustomFrame.ActiveForm.Size.Width, CustomFrame.ActiveForm.Size.Height);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            ChangeSize = false;
        }

        private void Form1_MaximizedBoundsChanged(object sender, EventArgs e)
        {
            ChangeSize = false;
            x = CustomFrame.ActiveForm.Size.Width;
            y = CustomFrame.ActiveForm.Size.Height;
            Size = new Size(CustomFrame.ActiveForm.Size.Width, CustomFrame.ActiveForm.Size.Height);
        }
    }
}
