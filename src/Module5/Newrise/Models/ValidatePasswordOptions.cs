namespace Newrise.Models {
	public enum ValidatePasswordOptions {
		None = 0,
		HasUpperCase = 1,
		HasLowerCase = 2,
		HasDigit = 4,
		HasSymbol = 8,
		All =
			HasUpperCase |
			HasLowerCase |
			HasDigit |
			HasSymbol
	}
}
