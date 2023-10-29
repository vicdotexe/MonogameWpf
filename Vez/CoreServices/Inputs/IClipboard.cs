namespace Vez.CoreServices.Inputs
{
	public interface IClipboard
	{
		string GetContents();
		void SetContents(string text);
	}
}