import "./Card.css"
import nagiAura from '../../assets/img/nagiAura.jpg'

import imglapis from '../../assets/img/lapis.svg'
import imglixeirinha from '../../assets/img/lixeirinha.svg'

export const Card = ({tituloCard}) => {
    return(
        <>
            <div className="cardDaImagem">
                <p>{tituloCard}</p>
                <img className="imgDoCard" src={nagiAura} alt="Imagem relacionada ao card." />
                <div className="icons">
                    <img src={imglapis} alt="Ícone de caneta para realizar uma alteração." />
                    <img src={imglixeirinha} alt="ícone de uma lixeira para realizar a exclusão." />
                </div>
            </div>
        </>
    )
}