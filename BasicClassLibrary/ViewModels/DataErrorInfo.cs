using PatternsLibrary.BaseClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BaseClassesLibrary.ViewModels
{
    /// <summary>
    /// Provides functionality to provide errors for the object if it is in an invalid state.  NET 3.5 or above
    /// </summary>
    public abstract class DataErrorInfoDelegateRule<T> : DataErrorInfoBaseClass, IDataErrorInfo where T : DataErrorInfoDelegateRule<T> // where T it means that T class need to inherit this class 
    {
        public string Error => string.Empty;
        public string this[string columnName] => OnValidate(columnName);
        private static List<DelegateRule<T>> rules;
        #region Properties and events
        /// <summary>
        /// Gets the rules which provide the errors.
        /// </summary>
        /// <value>The rules this instance must satisfy.</value>
        protected static List<DelegateRule<T>> Rules
        {
            get { return rules; }
        }
        #endregion

        #region Constructors
        static DataErrorInfoDelegateRule()
        {
            rules = new List<DelegateRule<T>>();
        }

        public DataErrorInfoDelegateRule() : base()
        {
            ApplyRules();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (string.IsNullOrEmpty(propertyName)) ApplyRules();
            else ApplyRules(propertyName);
        }

        /// <summary>
        /// Check validation of property.
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <returns></returns>
        protected virtual string OnValidate(string propertyName)
        {
            List<string> result;
            if (string.IsNullOrEmpty(propertyName))
            {
                result = new List<string>();

                foreach (var errorMessages in this.errorMessages.Values)
                {
                    result.AddRange(errorMessages);
                }
            }
            else
            {
                errorMessages.TryGetValue(propertyName, out result);
            }
            return string.Join(Environment.NewLine, result);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Applies all rules to this instance.
        /// </summary>
        private void ApplyRules()
        {
            lock (_classlock)
            {
                foreach (string propertyName in rules.Select(x => x.PropertyName))
                {
                    this.ApplyRules(propertyName);
                }
            }
        }
        /// <summary>
        /// Applies the rules to this instance for the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void ApplyRules(string propertyName)
        {
            List<string> propertyErrors = Apply((T)this, propertyName);

            if (propertyErrors.Count > 0)
            {
                if (errorMessages.ContainsKey(propertyName))
                {
                    errorMessages[propertyName].Clear();
                }
                else
                {
                    errorMessages[propertyName] = new List<string>();
                }

                errorMessages[propertyName].AddRange(propertyErrors);
            }
            else if (errorMessages.ContainsKey(propertyName))
            {
                errorMessages[propertyName].Clear();
            }
        }

        /// <summary>
        /// Applies the <see cref="Rule{T}"/>'s contained in this instance to <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to apply the rules to.</param>
        /// <param name="propertyName">Name of the property we want to apply rules for. <c>null</c>
        /// to apply all rules.</param>
        /// <returns>A collection of errors.</returns>
        private List<string> Apply(T obj, string propertyName)
        {
            List<string> errors = new List<string>();

            foreach (Rule<T> rule in rules)
            {
                if (string.IsNullOrEmpty(propertyName) || rule.PropertyName.Equals(propertyName))
                {
                    if (!rule.Apply(obj))
                    {
                        errors.Add(rule.Error);
                    }
                }
            }

            return errors;
        }
        #endregion
    }

    /// <summary>
    /// Provides functionality to provide errors for the object if it is in an invalid state.  NET 3.5 or above
    /// </summary>
    public abstract class DataErrorInfoDataAnnotations : DataErrorInfoBaseClass, IDataErrorInfo
    {
        public string Error => string.Empty;
        public string this[string columnName] => OnValidate(columnName);

        #region Protected Methods
        /// <summary>
		/// Notifies listeners that a property value has changed.
		/// </summary>
		/// <param name="propertyName">Name of the property used to notify listeners. This
		/// value is optional and can be provided automatically when invoked from compilers
		/// that support <see cref="CallerMemberNameAttribute"/>.</param>
		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            OnValidate(propertyName);
        }

        /// <summary>
        /// Check validation of property.
        /// </summary>
        /// <param name="propertyName">Name of property</param>
        /// <returns></returns>
        protected virtual string OnValidate(string propertyName)
        {
            ValidationContext context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            Collection<ValidationResult> validationResults = new Collection<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this, context, validationResults, true);
            if (!isValid)
            {
                if (validationResults.All(result => result.MemberNames.All(memberName => !memberName.Equals(propertyName))))
                {
                    errorMessages[propertyName].Clear();
                }
                else
                {
                    errorMessages[propertyName].Clear();
                    var errors = validationResults.Where(result => result.MemberNames.Any(memberName => memberName.Equals(propertyName))).Select(x => x.ErrorMessage);
                    errorMessages[propertyName].AddRange(errors);
                }
            }
            else
            {
                errorMessages[propertyName].Clear();
            }
            return string.Join(Environment.NewLine, errorMessages[propertyName]);
        }
        #endregion
    }
}
