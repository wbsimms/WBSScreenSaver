#region copyright
// Copyright (c) 2015 Wm. Barrett Simms wbsimms.com
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using WaveSim;
using Application = System.Windows.Application;

namespace WBSScreenSaver
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private HwndSource winWPFContent;

		private void ApplicationStartup(object sender, StartupEventArgs e)
		{
			if (e.Args.Length == 0 || e.Args[0].ToLower().StartsWith("/s"))
			{
				foreach (Screen s in Screen.AllScreens)
				{
					if (s != Screen.PrimaryScreen)
					{
						Blackout window = new Blackout();
						window.Left = s.WorkingArea.Left;
						window.Top = s.WorkingArea.Top;
						window.Width = s.WorkingArea.Width;
						window.Height = s.WorkingArea.Height;
						window.Show();
					}
					else
					{
						MainWindow window = new MainWindow();
						window.Left = s.WorkingArea.Left;
						window.Top = s.WorkingArea.Top;
						window.Width = s.WorkingArea.Width;
						window.Height = s.WorkingArea.Height;
						window.Show();
					}
				}
			}
			else if (e.Args[0].ToLower().StartsWith("/p"))
			{
				MainWindow window = new MainWindow();
				Int32 previewHandle = Convert.ToInt32(e.Args[1]);
				IntPtr pPreviewHnd = new IntPtr(previewHandle);
				RECT lpRect = new RECT();
				bool bGetRect = Win32API.GetClientRect(pPreviewHnd, ref lpRect);

				HwndSourceParameters sourceParams = new HwndSourceParameters("sourceParams");

				sourceParams.PositionX = 0;
				sourceParams.PositionY = 0;
				sourceParams.Height = lpRect.Bottom - lpRect.Top;
				sourceParams.Width = lpRect.Right - lpRect.Left;
				sourceParams.ParentWindow = pPreviewHnd;
				sourceParams.WindowStyle = (int)(WindowStyles.WS_VISIBLE | WindowStyles.WS_CHILD | WindowStyles.WS_CLIPCHILDREN);

				winWPFContent = new HwndSource(sourceParams);
				winWPFContent.Disposed += (o, args) => window.Close();
				winWPFContent.RootVisual = window.MainGrid;

			} else if (e.Args[0].ToLower().StartsWith("/c"))
			{
			}
		}
	}
}
