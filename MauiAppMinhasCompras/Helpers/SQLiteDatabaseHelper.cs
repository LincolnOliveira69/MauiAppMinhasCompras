// Os "using" que não são usados ficam com uma coloração cinza, ao término da classe podemos removê-los.
// Para tanto basta clicar com o botão direito do mouse e clicar em "Remover e Classificar Usos".
// Os "using" que não foram usados serão removidos.
using MauiAppMinhasCompras.Models; // Importa o namespace onde está definida a classe Produto.
using SQLite; // Importa a biblioteca SQLite para manipulação do banco de dados.

namespace MauiAppMinhasCompras.Helpers // Define o namespace para organizar a classe helper.
{
    public class SQLiteDatabaseHelper // Classe responsável por interagir com o banco SQLite.
    {
        readonly SQLiteAsyncConnection _conn; // Declara a conexão assíncrona com o banco.

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path); // Inicializa a conexão com o arquivo de banco no caminho especificado em path.
            _conn.CreateTableAsync<Produto>().Wait(); // Cria a tabela Produto se não existir na primeira vez que é acessado (aguarda execução síncrona).
        }
        public Task<int> Insert(Produto p)
// Quando concluída, essa tarefa fornecerá um número inteiro representando o resultado da operação.
// No caso do InsertAsync, esse inteiro indica quantas linhas foram inseridas no banco de dados.
        {
            return _conn.InsertAsync(p); // Insere um objeto Produto na tabela e retorna o número de linhas afetadas.
        }

        public Task<List<Produto>> Update(Produto p)
// Define um método assíncrono que recebe um objeto "Produto" como parâmetro.
// O retorno é um Task<List<Produto>>, ou seja, uma tarefa que será executada em segundo plano.
// Quando concluída, essa tarefa fornecerá uma lista de objetos "Produto" resultante da execução da operação.
// O uso de Task permite que o programa continue rodando sem bloquear a aplicação enquanto a atualização é processada.
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
//Define uma instrução SQL parametrizada para atualizar os campos Descricao, Quantidade e Preco de um registro na tabela Produto.
// O uso de "?" indica placeholders que serão substituídos pelos valores fornecidos em tempo de execução.
// Isso visa evitar concatenação direta e torna a query mais segura e reutilizável.
            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
                );
// Executa a instrução SQL definida, substituindo cada placeholder (?) pelos valores do objeto "p".
// O retorno é uma tarefa que, quando concluída, fornece uma lista de objetos Produto resultante da execução.
        }

        public Task<int> Delete(int id)
// Quando concluída, essa tarefa fornecerá um número inteiro representando o resultado da operação.
// No caso do DeleteAsync, esse inteiro indica quantas linhas foram removidas da tabela Produto.
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id); // Exclui o produto onde (=>) a coluna Id seja igual ao parâmetro fornecido.

        }

        public Task<List<Produto>> GetAll()
// Define um método assíncrono que não recebe parâmetros.
// Quando concluída, essa tarefa fornecerá uma lista contendo todos os objetos "Produto" armazenados na tabela.
// O uso de Task permite que a aplicação continue rodando sem bloqueio enquanto os dados são recuperados.
        {
            return _conn.Table<Produto>().ToListAsync(); // Retorna todos os registros da tabela Produto em forma de lista.
        }

        public Task<List<Produto>> Search(string q)
 // Quando concluída, essa tarefa fornecerá uma lista de objetos "Produto" que atendem ao critério de pesquisa.
 // O uso de Task permite que a aplicação continue rodando sem bloqueio enquanto a consulta é processada.
        {
            string sql = "SELECT * Produto WHERE descricao LIKE '%" + q + "%'";
// Monta um comando SQL para buscar todos os registros da tabela Produto cuja coluna descricao contenha o texto informado em "q".
// O uso de % é um coringa, ou seja, não importa o que tem antes ou depois.

            return _conn.QueryAsync<Produto>(sql); // Executa a consulta e retorna lista de produtos encontrados.
        }
    }
}