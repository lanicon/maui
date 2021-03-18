namespace Microsoft.Maui
{
	/// <summary>
	/// Represents a View used to accept multi-line input.
	/// </summary>
	public interface IEditor : IView, IText
	{
		/// <summary>
		/// Gets the maximum allowed length of input.
		/// </summary>
		int MaxLength { get; }
	}
}