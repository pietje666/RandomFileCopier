using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RandomFileCopier.Validation;
using GalaSoft.MvvmLight;
using System.Linq;

namespace RandomFileCopier.Models.Base
{
    internal class ValidatableModelBase : ObservableObject, INotifyDataErrorInfo
    {

        protected void Validate<T>(T entity, IValidator<T> validator, [CallerMemberName] string propertyName = null)
        {
            Validate(entity, new IValidator<T>[1] { validator }, propertyName);
        }

        protected void Validate<T>(T entity, IValidator<T>[] validators, [CallerMemberName] string propertyName = null)
        {
            foreach (var validator in validators)
            {
                if (!validator.IsValid(entity))
                {
                    AddError(propertyName, validator.ErrorMessage);
                }
                else
                {
                    RemoveError(propertyName, validator.ErrorMessage);
                }
            }
        }



        private readonly Dictionary<string, List<string>> errors;

        public ValidatableModelBase()
        {
            errors = new Dictionary<string, List<string>>();
        }

        // Adds the specified error to the errors collection if it is not 
        // already present, inserting it in the first position if isWarning is 
        // false. Raises the ErrorsChanged event if the collection changes. 
        public void AddError(string propertyName, string error, bool isWarning = false)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
            {
                if (isWarning) errors[propertyName].Add(error);
                else errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);
            }
        }

        // Removes the specified error from the errors collection if it is
        // present. Raises the ErrorsChanged event if the collection changes.
        public void RemoveError(string propertyName,  string error)
        {
            if (errors.ContainsKey(propertyName) && errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);
                if (errors[propertyName].Count == 0) errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) ||
                !errors.ContainsKey(propertyName)) return null;
            return errors[propertyName];
        }

        public bool HasPropertyErrors(string propertyName)
        {
            List<string> errorList;
            errors.TryGetValue(propertyName, out errorList);
            return errorList != null &&  errorList.Any();
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }
    }
}
