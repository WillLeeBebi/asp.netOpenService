using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace liemei.Common.Models
{
    /// <summary>
    /// 此类是所有ViewMode的基类
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        private EventHandler<TipArgs> _ehshowTip;

        /// <summary>
        /// 页面提示事件
        /// </summary>
        public event EventHandler<TipArgs> OnShowTip
        {
            add
            {
                if (_ehshowTip == null)
                    _ehshowTip = value;
            }
            remove
            {
                _ehshowTip -= value;
            }
        }

        private EventHandler<MessageBoxArgs> _ehshowmsgbox;
        /// <summary>
        /// 显示MessageBox事件
        /// </summary>
        public event EventHandler<MessageBoxArgs> OnShowMessageBox
        {
            add
            {
                if (_ehshowmsgbox == null)
                    _ehshowmsgbox = value;
            }
            remove
            {
                _ehshowmsgbox -= value;
            }
        }

        /// <summary>
        /// 显示提示MessageBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowMessageBox(object sender, MessageBoxArgs e)
        {
            if (_ehshowmsgbox != null)
            {
                _ehshowmsgbox(sender, e);
            }
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowTop(object sender, TipArgs e)
        {
            if (_ehshowTip != null)
            {
                _ehshowTip(sender, e);
            }
        }

        #region 构造方法

        protected ViewModelBase()
        {
        }

        #endregion // Constructor

        #region 显示名字


        public virtual string DisplayName { get; set; }

        #endregion // DisplayName

        #region 调试

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }


        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {
        }

#if DEBUG

        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
            System.Diagnostics.Debug.WriteLine(msg);
        }
#endif

        #endregion
    }

    public class TipArgs : EventArgs
    {
        public TipArgs(bool r, string msg, int h)
        {
            Res = r;
            Meg = msg;
            Height = h;
        }
        public TipArgs()
        { }
        public bool Res;
        public string Meg;
        public int Height;
    }

    public class MessageBoxArgs : EventArgs
    {
        public MessageBoxArgs(string msg, string tit, CustomBoxButtons cb, CustomBoxIcon ic)
        {
            Message = msg;
            Title = tit;
            button = cb;
            icon = ic;
        }

        public string Message;
        public string Title;
        public CustomBoxButtons button;
        public CustomBoxIcon icon;
    }

    /// <summary>
    /// 显示图片
    /// </summary>
    public enum CustomBoxIcon
    {
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 无图标
        /// </summary>
        None,
        /// <summary>
        /// 成功
        /// </summary>
        Success
    }
    /// <summary>
    /// 显示按钮
    /// </summary>
    public enum CustomBoxButtons
    {
        /// <summary>
        /// 确定
        /// </summary>
        OK,
        /// <summary>
        /// 确定于取消
        /// </summary>
        OKCancel
    }
}
