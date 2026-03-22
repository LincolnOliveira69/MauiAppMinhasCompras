using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Obtém o produto que está vinculado ao contexto da página
            Produto produto_anexado = BindingContext as Produto;

            // Cria um novo objeto Produto com os dados atualizados do formulário
            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text)
            };

            // Atualiza o registro no banco de dados
            await App.Db.Update(p);

            // Exibe mensagem de sucesso após a atualização
            await DisplayAlert("Sucesso!", "Registo Atualizado", "OK");

            // Retorna para a página anterior na navegação
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