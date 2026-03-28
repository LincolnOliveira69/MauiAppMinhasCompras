using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Cria um novo objeto Produto com os dados informados pelo usuário
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),

                // Calcula o total automaticamente
                //Total = Convert.ToDouble(txt_preco.Text) * Convert.ToDouble(txt_quantidade.Text),

                // Define a data de cadastro como a data/hora atual
                DataCadastro = DateTime.Now

            };

            // Insere o produto no banco de dados
            await App.Db.Insert(p);

            // Exibe mensagem de sucesso após a inserção
            await DisplayAlert("Sucesso!", "Registo Inserido", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            // Caso ocorra algum erro (ex: conversão inválida ou falha no banco),
            // exibe uma mensagem de alerta para o usuário
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}