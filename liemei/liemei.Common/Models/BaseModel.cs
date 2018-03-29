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
    /// 此类是WPF项目中Model的抽象基类
    /// </summary>
    public abstract class BaseModel : IDataErrorInfo, INotifyPropertyChanged
    {
        #region IDataErrorInfo 成员

        public string Error
        {
            get
            {
                foreach (string property in this.ValidatedProperties())
                {
                    string validationError = GetValidationError(property);
                    if (!string.IsNullOrEmpty(validationError))
                        return validationError;
                }
                return null;
            }
        }

        public string this[string propertyName]
        {
            get { return this.GetValidationError(propertyName); }
        }

        #endregion
        public bool IsValid
        {
            get { return Error == null; }
        }

        /// <summary>
        /// 指示对象哪些属性需要验证
        /// </summary>
        /// <returns></returns>
        protected abstract string[] ValidatedProperties();

        /// <summary>
        /// 实现每个属性的验证逻辑
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性验证不通过的错误信息</returns>
        protected abstract string GetValidationError(string propertyName);


        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool IsStringMissing(string value)
        {
            return
                String.IsNullOrEmpty(value) ||
                value.Trim() == String.Empty;
        }


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
    }
}
