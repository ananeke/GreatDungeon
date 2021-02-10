using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseClassesLibrary.Extensions
{
    public interface CopyInterface<T> where T : class
    {
        void ShallowCopy(T obj);
        void DeepCopy(T obj);
    }

    public abstract class BaseCopy<T> : CopyInterface<T> where T : BaseCopy<T>
    {
        public void DeepCopy(T obj)
        {
            ValueCopy(obj);
            ReferenceDeepCopy(obj);
        }

        public void ShallowCopy(T obj)
        {
            ValueCopy(obj);
            ReferenceShallowCopy(obj);
        }

        protected abstract void ValueCopy(T obj);
        protected virtual void ReferenceShallowCopy(T obj) { }
        protected virtual void ReferenceDeepCopy(T obj) { }
    }

    public interface CloneInterface<T> : CopyInterface<T> where T : class
    {
        T ShallowClone();
        T DeepClone();
    }

    public abstract class BaseClone<T> : BaseCopy<T>, CloneInterface<T> where T : BaseClone<T>, new()
    {
        public T DeepClone()
        {
            var newObject = new T();
            newObject.DeepCopy(this as T);
            return newObject;
        }

        public T ShallowClone()
        {
            var newObject = new T();
            newObject.ShallowCopy(this as T);
            return newObject;
        }
    }
}
