using UIKit;

namespace Microsoft.Maui
{
	public static class EditorExtensions
	{
		public static void UpdateText(this UITextView textView, IEditor editor)
		{
			string text = editor.Text;

			if (textView.Text != text)
			{
				textView.Text = text;
			}
		}

		public static void UpdateMaxLength(this UITextView textView, IEditor editor)
		{
			var currentControlText = textView.Text;

			if (currentControlText?.Length > editor.MaxLength)
				textView.Text = currentControlText.Substring(0, editor.MaxLength);
		}
	}
}