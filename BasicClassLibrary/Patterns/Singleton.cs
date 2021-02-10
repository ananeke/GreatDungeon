using BaseClassesLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClassesLibrary.Patterns
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        #region Protected properties, events
        protected static object classLock;
        protected static T singletonObject;
        #endregion

        #region Constructors
        protected Singleton()
        {
            classLock = typeof(T);
        }
        #endregion

        #region Public properties, events
        public static T SingletonObject
        {
            get
            {
                lock (classLock)
                {
                    if (singletonObject == null)
                    {
                        singletonObject = new T();
                    }
                    return singletonObject;
                }
            }
        }
        #endregion
    }

    public abstract class BaseViewModelSingleton<T> : BaseViewModel
        where T : BaseViewModelSingleton<T>, new()
    {
        #region Protected properties, events
        protected static object classLock;
        protected static T singletonObject;
        #endregion

        #region Constructors
        protected BaseViewModelSingleton()
        {
            classLock = typeof(T);
        }
        #endregion

        #region Public properties, events
        public static T SingletonObject
        {
            get
            {
                lock (classLock)
                {
                    if (singletonObject == null)
                    {
                        singletonObject = new T();
                    }
                    return singletonObject;
                }
            }
        }
        #endregion
    }


    public abstract class NotifyDataErrorInfoDataAnnotationsSingleton<T> : NotifyDataErrorInfoDataAnnotations<T>
        where T : NotifyDataErrorInfoDataAnnotationsSingleton<T>, new()
    {
        #region Private properties, events
        protected static object classLock;
        #endregion
        #region Protected properties, events
        protected static T singletonObject;
        #endregion

        #region Constructors
        static NotifyDataErrorInfoDataAnnotationsSingleton()
        {
            classLock = typeof(T);
        }
        #endregion

        #region Public properties, events
        public static T SingletonObject
        {
            get
            {
                lock (classLock)
                {
                    if (singletonObject == null)
                    {
                        singletonObject = new T();
                    }
                    return singletonObject;
                }
            }
        }
        #endregion
    }
}
