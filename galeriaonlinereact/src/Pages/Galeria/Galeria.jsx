import './Galeria.css'

import dwloadzin from "../../assets/img/dwloadzin.svg"
import { Botao }  from '../../components/botao/Botao'
import { Card }   from '../../components/card/Card'


export const Galeria = () => {
    return(
        <>
            <h1 className='tituloGaleria'>Galeria Online</h1>
            <form className="formulario" onSubmit="">
                <div className="campoNome">
                    <label>Nome</label>
                    <input type="text" className='inputNome'/>
                </div>
                <div className="campoImagem">
                    <label className="arquivoLabel">
                        <i><img src={dwloadzin} alt="Icone de upload de imagem"/></i>
                        <input type="file" className="arquivoInput"/>
                    </label>
                </div>
                <Botao nomeBotao="Cadastrar"/>
            </form>

            <div className='campoCards'>
                <Card tituloCard="nagi Aura"/>
            </div>
        </>
    )
}