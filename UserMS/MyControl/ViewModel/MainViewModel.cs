using System.Collections.ObjectModel;

namespace UserMS.ViewModel
{
	public class MainViewModel
	{
		public MainViewModel()
		{
			this.Tabs = new ObservableCollection<TabViewModel>();
			this.AddItem(null);
		}

		/// <summary>
		/// Gets the collection of tabs.
		/// </summary>
		public ObservableCollection<TabViewModel> Tabs
		{
			get;
			private set;
		}

		/// <summary>
		/// Adds new tab item to the Tabs collection.
		/// </summary>
		public void AddItem(TabViewModel sender)
		{
			TabViewModel newTabItem = new TabViewModel(this);
			newTabItem.Header = "New Tab";
			newTabItem.IsSelected = true;
			if (sender != null)
			{
				int insertIndex = this.Tabs.IndexOf(sender) + 1;
				this.Tabs.Insert(insertIndex, newTabItem);
			}
			else
			{
				this.Tabs.Add(newTabItem);
			}
		}

		/// <summary>
		/// Removes an item from the Tabs collection.
		/// </summary>
		/// <param name="tabItem">The tab item.</param>
		public void RemoveItem(TabViewModel tabItem)
		{
			this.Tabs.Remove(tabItem);
			tabItem.Dispose();
		}
	}
}
