import "./Card.css";

//import imgCard from "../../assets/images/nagiAura.jpg"; // Importando uma imagem de exemplo
import imgPen from "../../assets/images/pen.svg"; // Importando o ícone de caneta
import imgTrash from "../../assets/images/trash.svg"; // Importando o ícone de lixeira


const Card = ({tituloCard, imgCard, funcaoEditar, funcaoExcluir}) => {
  return (
    <>
      <div className="cardDaImagem">
        <p>{tituloCard}</p>
        <img className="imgDoCard" src={imgCard} alt="Imagem de exemplo" />
        <div className="icons">
            <img src={imgPen}  onClick={funcaoEditar} alt="Ícone de editar" />
            <img src={imgTrash}  onClick={funcaoExcluir}alt="Ícone de deletar" />
        </div>
      </div>
    </>
  );
};
export default Card;
