using Newrise.Models;
using System.ComponentModel.DataAnnotations;

namespace Newrise.Validators {
	public class PasswordAttribute : ValidationAttribute {

		public readonly ValidatePasswordOptions _options;
		public ValidatePasswordOptions _result;
		public string _allowedSymbols = "~`!@#$%^&*-_+=|\\:;\"',.?/(){}[]<>";
		public bool _allowSpace = false;
		public int _minLength = 8;
		public string _error = string.Empty;

		public string AllowedSymbols {
			get => _allowedSymbols;
			set => _allowedSymbols = value;
		}
		public bool AllowSpace {
			get => _allowSpace;
			set => _allowSpace = value;
		}
		public int MinLength {
			get => _minLength;
			set => _minLength = value;
		}

		public ValidatePasswordOptions Options { get => _options; }
		public ValidatePasswordOptions Result { get => _result; }
		public string Error { get => _error; }

		public PasswordAttribute() {
			_options = ValidatePasswordOptions.All;
		}
		public PasswordAttribute(ValidatePasswordOptions options) {
			_options = options;
		}

		public override bool IsValid(object value) {
			var password = value.ToString();
			_result = ValidatePasswordOptions.None;
			if (password.Length < _minLength) {
				_error = "{0} must have at least {1} characters.";
				return false;
			}
			if (!_allowSpace && password.Contains(' ')) {
				_error = "{0} cannot contain spaces.";
				return false;
			}
			foreach (var code in password) {
				if (Char.IsAsciiLetterLower(code))
					_result |= ValidatePasswordOptions.HasLowerCase;
				else
				if (Char.IsAsciiLetterUpper(code))
					_result |= ValidatePasswordOptions.HasUpperCase;
				else
				if (Char.IsAsciiDigit(code))
					_result |= ValidatePasswordOptions.HasDigit;
				else
				if (_allowedSymbols.Contains(code))
					_result |= ValidatePasswordOptions.HasSymbol;
			}

			if (_options.HasFlag(ValidatePasswordOptions.HasLowerCase) &&
			!_result.HasFlag(ValidatePasswordOptions.HasLowerCase)) {
				_error = "{0} must have at least one lowercase character.";
				return false;
			}
			if (_options.HasFlag(ValidatePasswordOptions.HasUpperCase) &&
			!_result.HasFlag(ValidatePasswordOptions.HasUpperCase)) {
				_error = "{0} must have at least one uppercase character.";
				return false;
			}
			if (_options.HasFlag(ValidatePasswordOptions.HasDigit) &&
			!_result.HasFlag(ValidatePasswordOptions.HasDigit)) {
				_error = "{0} must have at least one digit.";
				return false;
			}
			if (_options.HasFlag(ValidatePasswordOptions.HasSymbol) &&
			!_result.HasFlag(ValidatePasswordOptions.HasSymbol)) {
				_error = "{0} must have at least one symbol.";
				return false;
			}
			_error = string.Empty;
			return true;
		}

		public override string FormatErrorMessage(string name) {
			return string.Format(_error, name, _minLength);
		}
	}
}
