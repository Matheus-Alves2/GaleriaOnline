import "./Galeria.css"; // Importando o CSS específico para a galeria
import icon from "../../assets/images/upload.svg"; // Importando o ícone para o upload de imagens
import Botao from "../../components/botao/Botao";
import Card from "../../components/card/Card";
import { useEffect, useState } from "react";
import api from '../../services/Services'

export const Galeria = () => {
  
  const [cards, setCards] = useState([]);
  const [nomeImagem, setNomeImagem] = useState("");
  const [imagem, setImagem] = useState(null); // <--- ADICIONADO

  async function cadastrarCard(e){
    e.preventDefault();

    if(imagem && nomeImagem){
      try {
        const formData = new FormData();
        formData.append("Nome", nomeImagem);
        formData.append("Arquivo", imagem);

        await api.post("Imagem/upload", formData, {
          headers: {
            "Content-Type": "multipart/form-data"
          }
        });

        alert("Imagem cadastrada com sucesso!");
        listarCards(); // <--- pra atualizar a tela logo após cadastrar

      } catch (error) {
        alert("Não foi possível realizar o cadastro!");
        console.error(error);
      }
    } else {
      alert("Preencha os campos de Nome e Imagem");
    }
  }

  async function listarCards(){
    try {
      const resposta = await api.get("Imagem");
      console.log(resposta);
      setCards(resposta.data);
    } catch (error) {
      console.error("Erro ao listar");
      alert("Erro ao listar!!");
    }
  }

  async function editarCard(id,nomeAntigo){
    const novoNome = prompt("Digite o novo nome da imagem",nomeAntigo);
    const inputArquivo = document.createElement("input");
    inputArquivo.type = "file";
    inputArquivo.accept = "image/*";

    inputArquivo.onchange = async (e) => {
      const novoArquivo = e.target.files[0];
      const formData = new FormData();
      formData.append("Nome", novoNome);
      formData.append("Arquivo", novoArquivo);

      if(formData){
        try {
          await api.put(`Imagem/${id}`, formData,{
            headers: {
              "Content-Type": "multipart/form-data"
            }
          })
          alert("Ebaaa deu certo")
          listarCards();
        } catch (error) {
          alert("Nao foi possivel alterar o card");
          console.log(error);
        }
      }
    };
    inputArquivo.click();
  }

  async function excluirCard(id){
    try {
      await api.delete(`Imagem/${id}`)
      alert("Excluiu👍!!!!");
      listarCards();
    } catch (error) {
      alert("Erro ao excluir o card");
      console.log(error);
    }
  }
  
  useEffect(() => {
    listarCards();
  }, []); 
  
  return (
    <>
      <h1 className="tituloGaleria">Galeria Online</h1>

      <form className="formulario" onSubmit={cadastrarCard}>
        <div className="campoNome">
          <label>Nome:</label>
          <input
            type="text"
            className="inputNome"
            placeholder="Digite Seu Nome..."
            onChange={(e) => setNomeImagem(e.target.value)}
            value={nomeImagem}
          />
        </div>

        <div className="campoImagem">
          <label className="arquivoLabel">
            <i>
              <img src={icon} alt="Icone de upload de imagem" />
            </i>
            <input 
              type="file" 
              className="arquivoInput" 
              onChange={(e) => setImagem(e.target.files[0])} 
            />
          </label>
        </div>

        <Botao nomeBotao="Cadastrar" type="submit" />
      </form>

      <div className='campoCards'>
        {cards.length > 0 ? (
          cards.map((e) => (
            <Card 
              key={e.id}
              tituloCard={e.nome}
              imgCard={`https://localhost:7188/${e.caminho.replace("wwwroot/", "")}`} 
              funcaoExcluir={() =>excluirCard(e.id)}
              funcaoEditar={() =>editarCard(e.id,e.nome)}
            />
          ))
        ) : (
          <p>Nenhum Card Cadastrado.</p>
        )}
      </div>
    </>
  )
}
      
export default Galeria;
