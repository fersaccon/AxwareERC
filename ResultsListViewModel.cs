using AxwareERC.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxwareERC
{
	public class ResultsViewModel : INotifyPropertyChanged
	{
		private List<Competitor> _competitorsResultList = new List<Competitor>();

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
