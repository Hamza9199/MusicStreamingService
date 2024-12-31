using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStreamingService.ViewModels
{
    public class AlbumViewModel : INotifyPropertyChanged
	{
		public AlbumViewModel()
		{
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
