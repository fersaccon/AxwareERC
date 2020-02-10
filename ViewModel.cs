using AxwareERC.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxwareERC
{
	public class ViewModel : INotifyPropertyChanged
	{
		private string _appTitle = "ERC Axware - file not loaded";
		private List<Competitor> _competitorsResultList = new List<Competitor>();

		public string AppTitle
		{
			get { return _appTitle; }
			set
			{
				if (_appTitle == value) return;

				_appTitle = value;
				this.OnPropertyChanged("AppTitle");
			}
		}

		public List<Competitor> CompetitorsResultList
		{
			get { return _competitorsResultList; }
			set
			{
				if (_competitorsResultList == value) return;

				_competitorsResultList = value;
				this.OnPropertyChanged("CompetitorsResults");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}






	}
}
