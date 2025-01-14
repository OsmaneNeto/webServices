using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using webServices;


[Route("api/[controller]")]
//chama a https:/ localHost: porta /api/[controller]
//api ta referenciando o controler
//[controller] ta definindo dentro da sua pasta o que você está  cadastrando e depois para o banco de dados
//[controller] ele puxa todo o codigo
[ApiController]
public class ProdutoController : ControllerBase
{


    //O método que será montado abaixo será chamado via POST
    [HttpPost]
    //Como será utilizado o protocolo HTTP, devemos retornar nos padrões
    //do protocolo, ou seja, através dos códigos existentes. Para isso
    //vamos usar o IActionResult como retorno do método
    //A marcação [FromBody] indica que os dados do Prod uto irão vir
    //dentro do corpo/Body da requisição HTTP
    public IActionResult cadastrar([FromBody] Produto p)
    {
        //Configurar a conexão com o MySQL
        MySqlConnection conexao = new MySqlConnection(
            "server=ESN509VMYSQL; database=apimysqlandroid; uid=aluno; pwd=Senai1234");
        MySqlCommand sql = new MySqlCommand(
            "INSERT INTO produto (nome, preco, quantidade) VALUES (@n, @p, @q)", conexao);
        //Passar os valores de cada @
        sql.Parameters.AddWithValue("@n", p.nome);
        sql.Parameters.AddWithValue("@p", p.preco);
        sql.Parameters.AddWithValue("@q", p.quantidade);
        conexao.Open(); //Abrir a conexão
        //Testar e executar o resultado do comando sql
        if (sql.ExecuteNonQuery() != 0)
        { //Se retornar zero então não alterou nada
            conexao.Close(); //Fecha a conexão e libera os recursos
            return Ok(p); //Retorna código 200 - Sucesso e exibe o produto "p" cadastrado
        }
        else
        {
            conexao.Close();
            return NoContent(); //Retorno em branco
        }
    }

    //Método para realizar a remoção de um produto no banco de dados
    //Ao remover um produto precisamos do ID, que será passado na URL
    [HttpDelete("{id}")] //Indicação que o método irá receber um valor do ID
    public IActionResult remover(int id)
    { //Valor que será recebido
        MySqlConnection conexao = new MySqlConnection(
        "server=ESN509VMYSQL; database=apimysqlandroid; uid=aluno; pwd=Senai1234");
        MySqlCommand sql = new MySqlCommand(
            "DELETE FROM produto WHERE codigo = @id", conexao);
        sql.Parameters.AddWithValue("@id", id); //Passar o valor do @id
        conexao.Open(); //Abre a conexão
        //Executar o comando no banco de dados e testar o seu retorno
        if (sql.ExecuteNonQuery() != 0)
        {
            conexao.Close(); //Fecha a conexão e libera os recursos
            //Retorna o código 200 (Sucesso) com uma mensagem personalizada
            return Ok(new { resposta = "sucesso" });
        }
        else
        {
            conexao.Close(); return NoContent();
        }
    }

    //Método para atualizar um Produto. Os dados do produto serão enviados
    //no corpo (Body) igual no método de cadastrar
    [HttpPut]
    public IActionResult atualizar([FromBody] Produto p)
    {
        MySqlConnection conexao = new MySqlConnection(
        "server=ESN509VMYSQL; database=apimysqlandroid; uid=aluno; pwd=Senai1234");
        MySqlCommand sql = new MySqlCommand(
        "UPDATE produto SET nome=@n, preco=@p, quantidade=@q WHERE codigo=@c", conexao);
        sql.Parameters.AddWithValue("@n", p.nome);
        sql.Parameters.AddWithValue("@p", p.preco);
        sql.Parameters.AddWithValue("@q", p.quantidade);
        sql.Parameters.AddWithValue("@c", p.codigo);
        conexao.Open();
        if (sql.ExecuteNonQuery() != 0)
        {
            conexao.Close();
            return Ok(new { result = "sucesso" });
        }
        else
        {
            conexao.Close();
            return NoContent();
        }
    }

    //Método para buscar todos os produtos
    [HttpGet]
    public IActionResult buscarTodos()
    {
        MySqlConnection conexao = new MySqlConnection(
        "server=ESN509VMYSQL; database=apimysqlandroid; uid=aluno; pwd=Senai1234");
        MySqlCommand sql = new MySqlCommand("SELECT * FROM produto", conexao);
        //Criar uma lista para receber os produtos cadastrados no banco de dados
        List<Produto> lista = new List<Produto>();
        conexao.Open();
        //Criar um objeto do MySqlDataReader para receber os dados
        MySqlDataReader reader = sql.ExecuteReader();
        //Percorrer cada resultado do select
        while (reader.Read())
        {
            Produto p = new Produto(reader.GetInt32("codigo"),
                reader.GetString("nome"), reader.GetDouble("preco"),
                reader.GetInt32("quantidade"));
            lista.Add(p); //Adicionar o produto na lista
        }
        conexao.Close();
        //Utilizando operador ternário para verificar se a lista está vazia ou não
        //e retornar NoContent (se estiver vazia) ou OK (se não estiver vazia)
        return lista.Count == 0 ? NoContent() : Ok(lista);
    }

    //Como existem dois métodos usando HttpGet, um deles precisa estar com um
    //caminho/link diferente. Para configurar o link utilizamos o comando Route
    //e atribuímos um nome para a rota. Nesse caso o link ficará:
    //http://localhost:XXXX/api/Produto/buscarCodigo
    [Route("buscarCodigo")]
    //Método para buscar todos os produtos
    [HttpGet]
    public IActionResult buscarTodos(int codigo)
    {
        MySqlConnection conexao = new MySqlConnection(
        "server=ESN509VMYSQL; database=apimysqlandroid; uid=aluno; pwd=Senai1234");
        MySqlCommand sql = new MySqlCommand(
                    "SELECT * FROM produto WHERE codigo = @c", conexao);
        sql.Parameters.AddWithValue("@c", codigo);
        //Criar um objeto Produto para retornar no final
        Produto p = null;
        conexao.Open();
        //Criar um objeto do MySqlDataReader para receber os dados
        MySqlDataReader reader = sql.ExecuteReader();
        //Percorrer cada resultado do select
        while (reader.Read())
        {
            p = new Produto(reader.GetInt32("codigo"),
                reader.GetString("nome"), reader.GetDouble("preco"),
                reader.GetInt32("quantidade"));
        }
        conexao.Close();
        //Utilizando operador ternário para verificar se a lista está vazia ou não
        //e retornar NoContent (se estiver vazia) ou OK (se não estiver vazia)
        return p == null ? NoContent() : Ok(p);
    }

}