using MauiAppMinhasCompras.Models;   // Importa a classe Produto, que representa os dados do item

namespace MauiAppMinhasCompras.Views
{
    public partial class Relatorio : ContentPage
    {
        public Relatorio()
        {
            InitializeComponent();

            // Carrega todos os produtos inicialmente usando o banco já criado (App.Db)
            // Como o método é assíncrono, aqui poderíamos usar await, mas em construtor não é permitido.
            // Então o carregamento inicial pode ser feito no OnAppearing.
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Carrega todos os produtos do banco sem aplicar filtro
            // Aguarda a tarefa concluir e obtém a lista
            var produtos = await App.Db.GetAll();

            // Atualiza a lista exibida na tela
            lst_relatorio.ItemsSource = produtos;
        }

        // Método chamado quando o botão "Filtrar" é clicado
        private async void OnFiltrarClicked(object sender, EventArgs e)
        {
            var inicio = dtInicio.Date; // Pega a data inicial escolhida no DatePicker
            var fim = dtFim.Date.AddDays(1).AddTicks(-1); // Pega a data final escolhida no DatePicker até o fim do dia

            // Consulta no banco: retorna todos os produtos e aplica filtro em memória
            var produtos = await App.Db.GetAll();
            var filtrados = produtos
                .Where(p => p.DataCadastro >= inicio && p.DataCadastro <= fim)
                .ToList();

            // Atualiza a lista exibida na tela com os resultados filtrados
            lst_relatorio.ItemsSource = filtrados;
        }
    }
}