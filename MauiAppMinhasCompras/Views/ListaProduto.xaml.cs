using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    // Lista observável que será usada como fonte de dados para a interface
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();

        // Define que a lista de produtos será exibida no componente lst_produtos
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        try
        {
            // Limpa a lista atual para evitar duplicações
            lista.Clear();

            // Busca todos os produtos no banco de dados
            List<Produto> tmp = await App.Db.GetAll();

            // Adiciona cada produto retornado à lista observável
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            // Caso ocorra algum erro (ex: falha no banco),
            // exibe uma mensagem de alerta para o usuário
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Navega para a página de cadastro de um novo produto
            Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {
            // Se houver erro na navegação, mostra alerta
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            // Captura o texto digitado na busca
            string q = e.NewTextValue;

            // Limpa a lista atual
            lista.Clear();

            // Busca produtos que correspondem ao texto digitado
            List<Produto> tmp = await App.Db.Search(q);

            // Adiciona os resultados à lista observável
            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            // Se ocorrer erro na busca, exibe alerta
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        // Calcula o total dos produtos sem necessidade de try-catch,
        // pois não há operações assíncronas ou acesso ao banco
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        // Exibe o valor total em um alerta
        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Obtém o item selecionado no menu de contexto
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            // Pede confirmação ao usuário antes de excluir
            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                // Remove do banco e da lista observável
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            // Se houver erro na exclusão, mostra alerta
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            // Obtém o produto selecionado na lista
            Produto p = e.SelectedItem as Produto;

            // Navega para a tela de edição, passando o produto como contexto
            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            // Se houver erro na navegação, mostra alerta
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
//Realizado na agenda 4 como "desafio" proposto no fim da aula do professor Tiago Silva.

/*
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
    */