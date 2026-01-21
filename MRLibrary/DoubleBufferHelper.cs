using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MRLibrary
{
    public static class DoubleBufferHelper
    {
        public static bool EnableDoubleBuffering(Control control,bool enable = true)
        {
            if (control == null) return false;
            try
            {
                PropertyInfo doubleBufferPropertyInfo= control.GetType().GetProperty("DoubleBuffered",BindingFlags.Instance |BindingFlags.NonPublic);
                if(doubleBufferPropertyInfo != null)
                {
                    doubleBufferPropertyInfo.SetValue(control, enable, null);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool EnableOptimizedDoubleBuffering (Control control, bool enable = true)
        {
            if (control == null) return false;
            try
            {
                PropertyInfo doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBufferd", BindingFlags.Instance | BindingFlags.NonPublic);
                doubleBufferPropertyInfo?.SetValue(control, enable, null);
                MethodInfo setStyleMethod = control.GetType().GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
                if (setStyleMethod != null)
                {
                    setStyleMethod.Invoke(control, new object[]
                    {
                        ControlStyles.UserPaint|
                        ControlStyles.AllPaintingInWmPaint|
                        ControlStyles.OptimizedDoubleBuffer,
                        enable
                    });
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static int EnableDoubleBufferingForMultiple(Control[] controls, bool enable= true)
        {
            if (controls == null || controls.Length == 0) return 0;
            int successCount = 0;
            foreach(var control in controls)
            {
                if (EnableOptimizedDoubleBuffering(control, enable)) successCount++;
            }
            return successCount;
        }
        public static int EnableDoubleBufferingForChildren(Control parentControl, bool enable=true, bool recursive= false)
        {
            if (parentControl == null || parentControl.Controls.Count == 0) return 0;
            int successCount = 0;
            foreach(Control child in parentControl.Controls)
            {
                if (EnableOptimizedDoubleBuffering(child, enable)) successCount++;
                if (recursive && child.Controls.Count > 0) successCount += EnableDoubleBufferingForChildren(child, enable, recursive);
            }
            return successCount;
        }
        public static int EnableDoubleBufferingForType<T>(Control parentControl, bool enable = true, bool recursive = true) where T : Control
        {
            if (parentControl == null) return 0;
            int successCount = 0;
            foreach(Control child in parentControl.Controls)
            {
                if(child is T)
                {
                    if (EnableOptimizedDoubleBuffering(child, enable)) successCount++;
                }
                if (recursive && child.Controls.Count > 0) successCount += EnableDoubleBufferingForType<T>(child, enable, recursive);
            }
            return successCount;
        }
        public static bool IsDoubleBufferingEnabled(Control control)
        {
            if (control == null) return false;
            try
            {
                PropertyInfo doubleBufferPropertyInfo = control.GetType().GetProperty(
                    "DoubleBuffered",BindingFlags.Instance| BindingFlags.NonPublic);
                if (doubleBufferPropertyInfo != null)
                    return (bool)doubleBufferPropertyInfo.GetValue(control, null);
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
