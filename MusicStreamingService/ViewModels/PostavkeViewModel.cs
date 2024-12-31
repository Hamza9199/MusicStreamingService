using MusicStreamingService.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


public class PostavkeViewModel : INotifyPropertyChanged
{
	public PostavkeViewModel()
	{
	}

	public ICommand PrebaciDetalje => new Command(async () =>
	{
		await Application.Current.MainPage.Navigation.PushAsync(new DetaljiRacuna());
	});

	public ICommand PrebaciInfo => new Command(async () =>
	{
		await Application.Current.MainPage.Navigation.PushAsync(new InfoAplikacije());
	});

	public event PropertyChangedEventHandler? PropertyChanged;
}



