using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;

namespace ShapeDatasetGeneratorWPF.ViewModel
{
    public class ValidatableViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        #region Constructor

        public ValidatableViewModelBase()
        {
            _errors = new Dictionary<string, ObservableCollection<string>>();
            _allErrorsUnsorted = new ObservableCollection<string>();
        }

        #endregion

        #region Properties

        public bool HasErrors => _errors.Any();

        #endregion

        #region Eventhandler

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region Fields

        protected readonly Dictionary<string, ObservableCollection<string>> _errors;
        protected readonly ObservableCollection<string> _allErrorsUnsorted;

        #endregion

        #region Methods

        public virtual IEnumerable GetErrors(string propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName)) return _errors.Values;
            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }

        public virtual IEnumerable GetAllErrorsUnsorted(string propertyName = "")
        {
            _allErrorsUnsorted.Clear();
            if (string.IsNullOrEmpty(propertyName))
            {
                foreach (var errors in _errors.Values)
                    foreach (var error in errors)
                        _allErrorsUnsorted.Add(error);
                return _allErrorsUnsorted;
            }

            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }

        protected void OnErrorsChanged([CallerMemberName] string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void AddError(string propertyName, string errorMessage)
        {
            // get error message via WPFLocalizationExtension using a normal resource key
            //var errorMsg = LocalizationProvider.GetLocalizedValue<string>(errorMessage);
            AddErrors(propertyName, new ObservableCollection<string> { errorMessage });
        }

        protected void AddErrors(string propertyName, IList<string> errors)
        {
            // Added empty error => do nothing
            if (errors?.Count == 0) return;

            var changed = false;
            //  check if error-dictionary already has error(s) for the property. If not => add it to the dictionary
            if (!_errors.ContainsKey(propertyName))
            {
                _errors.Add(propertyName, new ObservableCollection<string>());
                changed = true;
            }

            // add errors of the property if they are not already in the error-list
            foreach (var err in errors)
            {
                if (_errors[propertyName].Contains(err)) continue;
                _errors[propertyName].Add(err);
                changed = true;
            }

            if (changed)
                // Fire event sothat the UI updates
                OnErrorsChanged(propertyName);
        }

        protected void ClearAllErrors()
        {
            _errors.Clear();
            OnErrorsChanged("");
        }

        protected void ClearAllErrorsOfSingleProperty(string propertyName)
        {
            _errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        protected void ClearSingleError(string propertyName, string errorMessage)
        {
            if (_errors.ContainsKey(propertyName))
            {
                // delete the single error message from the list of errors of the property
                foreach (var errorMsg in _errors[propertyName])
                {
                    _errors[propertyName].Remove(errorMessage);
                    break;
                }

                // if property has no more errors => clear the property from the error list
                // otherwise you will end up with an empty observablecollection => this.HasErrors will see this ObservableCollection and think there are still errors
                if (_errors[propertyName].Count == 0) _errors.Remove(propertyName);
                // Fire event sothat the UI updates
                OnErrorsChanged(propertyName);
            }
        }

        // only needed if property attributes are used to get ValidationResults and ErrorMessages
        protected string[] GetErrorsFromAnnotations<T>(string propertyName, T value)
        {
            var results = new List<ValidationResult>();
            var vc = new ValidationContext(this, null, null) { MemberName = propertyName };
            var isValid = Validator.TryValidateProperty(value, vc, results);
            return isValid ? null : Array.ConvertAll(results.ToArray(), o => o.ErrorMessage);
        }

        #endregion
    }
}
