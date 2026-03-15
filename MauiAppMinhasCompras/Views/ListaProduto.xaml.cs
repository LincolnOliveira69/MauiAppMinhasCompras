using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

    protected async override void OnAppearing()
    {
		List<Produto> tmp = await App.Db.GetAll();

		tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		}catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
	}

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		string q = e.NewTextValue;

		lista.Clear();

        List<Produto> tmp = await App.Db.Search(q);

        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");
    }

    // Remover pelo MenuItem (ação de contexto)
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        var mi = (MenuItem)sender;
        var produto = (Produto)mi.BindingContext;

        bool confirm = await DisplayAlert("Remover", $"Deseja remover {produto.Descricao}?", "Sim", "Não");
        if (confirm)
        {
            await App.Db.Delete(produto.Id);
            lista.Remove(produto);
        }
    }

    // Remover pelo botão da Toolbar (item selecionado)
    private async void ToolbarItem_Clicked_Remover(object sender, EventArgs e)
    {
        if (lst_produtos.SelectedItem is Produto produto)
        {
            bool confirm = await DisplayAlert("Remover",
                $"Deseja remover o produto \"{produto.Descricao}\"?",
                "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(produto.Id);
                lista.Remove(produto);
            }
        }
        else
        {
            await DisplayAlert("Aviso", "Selecione um produto na lista para remover.", "OK");
        }
    }
}