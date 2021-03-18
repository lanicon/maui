using System.Collections.Generic;
using Android.Text;
using AndroidX.AppCompat.Widget;

namespace Microsoft.Maui
{
	public static class EditorExtensions
	{
		public static void UpdateText(this AppCompatEditText editText, IEditor editor)
		{
			string? newText = editor.Text;

			if (editText.Text == newText)
				return;

			int maxLength = editor.MaxLength;
			newText = TrimToMaxLength(newText, maxLength);

			editText.Text = newText;

			if (string.IsNullOrEmpty(newText))
				return;

			editText.SetSelection(newText!.Length);
		}

		public static void UpdateMaxLength(this AppCompatEditText editText, IEditor editor)
		{
			var currentFilters = new List<IInputFilter>(editText?.GetFilters() ?? new IInputFilter[0]);

			for (var i = 0; i < currentFilters.Count; i++)
			{
				if (currentFilters[i] is InputFilterLengthFilter)
				{
					currentFilters.RemoveAt(i);
					break;
				}
			}

			currentFilters.Add(new InputFilterLengthFilter(editor.MaxLength));

			if (editText == null)
				return;

			editText.SetFilters(currentFilters.ToArray());
			editText.Text = TrimToMaxLength(editor.Text, editor.MaxLength);
		}

		internal static string? TrimToMaxLength(string currentText, int maxLength)
		{
			if (currentText == null || currentText.Length <= maxLength)
				return currentText;

			return currentText.Substring(0, maxLength);
		}
	}
}