import "./Botao.css";

const Botao = ({ nomeBotao, onClick, type = "button" }) => {
    return (
        <button 
            className='botao' 
            onClick={onClick} 
            type={type}
        >
           {nomeBotao}
        </button>
    )
}

export default Botao;