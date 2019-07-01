using System.Collections.ObjectModel;
namespace Osca.Models.Misc
{
	public class Group<TTitle, TType> : ObservableCollection<TType>
	{
		public TTitle Title { get; set; }
	}
}
