using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace liemei.Common.Models
{
    public static class DispatcherHelper
    {
        public static Dispatcher UIDispatcher
        {
            get;
            private set;
        }

        public static void CheckBeginInvokeOnUI(Action action)
        {
            if (UIDispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                UIDispatcher.BeginInvoke(action);
            }
            //Dispatcher.BeginInvoke(action);
        }

        public static void Initialize()
        {
            if (UIDispatcher != null)
            {
                return;
            }
            UIDispatcher = Dispatcher.CurrentDispatcher;
        }
    }
}
