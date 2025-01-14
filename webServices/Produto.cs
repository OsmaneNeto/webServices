namespace webServices
{
    public class Produto
    {
        public int codigo { get; set; }
        public string nome { get; set; }
        public double preco { get; set; }
        public int quantidade { get; set; }

        public Produto(int codigo, string nome, double preco, int quantidade)
        {
            this.codigo = codigo;
            this.nome = nome;
            this.preco = preco;
            this.quantidade = quantidade;


        }
        //A declaração do construtor vazio impede a geração automática de um construtor sem parâmetro. 
        public Produto() { }
        //Eu posso gerar uma classe  com o contrutor ou uma classe vazia
        //já que não tem condições ou parametros ele servira para por exemplo chamar todo o banco, listar todo o banco
    }
        
}
    

