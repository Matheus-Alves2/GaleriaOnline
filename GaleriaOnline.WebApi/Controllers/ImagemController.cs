using GaleriaOnline.WebApi.DTO;
using GaleriaOnline.WebApi.Models;
using GaleriaOnline.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GaleriaOnline.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagemController : ControllerBase
    {
        private readonly ImagemRepository _repository;
        private readonly IWebHostEnvironment _envi;

        public ImagemController(ImagemRepository repository, IWebHostEnvironment envi)
        {
            _repository = repository;
            _envi = envi;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImagemPorID(int id)
        {
            var imagem = await _repository.GetByIdAsync(id);
            if (imagem == null)
            {
                return NotFound();
            }

            return Ok(imagem);
        }
        [HttpGet]
        public async Task<IActionResult> GetTodasAsImagens()
        {
            var images = await _repository.GetAllAsync();
            return Ok(images);
        }
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImagem([FromForm] ImagemDto dto)
        {
            if (dto.Arquivo == null || dto.Arquivo.Length == 0 || String.IsNullOrWhiteSpace(dto.Nome))
            {
                return BadRequest("Deve ser enviado um Nome e uma Imagem.");
            }

            var extensao = Path.GetExtension(dto.Arquivo.FileName);
            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
            var pastaRelativa = "wwwroot/imagens";
            var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), pastaRelativa);

            if (!Directory.Exists(caminhoPasta))
            {
                Directory.CreateDirectory(caminhoPasta);

            }
            var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await dto.Arquivo.CopyToAsync(stream);
            }
            var imagem = new Imagem
            {
                Nome = dto.Nome,
                Caminho = Path.Combine(pastaRelativa, nomeArquivo).Replace("\\", "/")
            };

            await _repository.CreateAsync(imagem);
            return CreatedAtAction(nameof(GetImagemPorID), new { id = imagem.Id }, imagem);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarImagem(int id, PutImagemDto ImagemAtualizada)
        {
            var imagem = await _repository.GetByIdAsync(id);
            if (imagem == null)
            {
                return NotFound("Imagem nao encontrada");

            }
            if (ImagemAtualizada.Arquivo == null && string.IsNullOrWhiteSpace(ImagemAtualizada.Nome))
            {
                return BadRequest();
            }
            if (!string.IsNullOrWhiteSpace(ImagemAtualizada.Nome))
            {
                imagem.Nome = ImagemAtualizada.Nome;
            }

            var caminhoAntigo = Path.Combine(Directory.GetCurrentDirectory(), imagem.Caminho.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (ImagemAtualizada.Arquivo != null && ImagemAtualizada.
                Arquivo.Length > 0)
            {
                if (System.IO.File.Exists(caminhoAntigo))
                {
                    System.IO.File.Delete(caminhoAntigo);
                }

                var extensao = Path.GetExtension(ImagemAtualizada.Arquivo.FileName);
                var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
                var pastaRelativa = "wwwroot/imagens";
                var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), pastaRelativa);

                if (!Directory.Exists(caminhoPasta))
                {
                    Directory.CreateDirectory(caminhoPasta);

                }
                var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await ImagemAtualizada.Arquivo.CopyToAsync(stream);
                }
                imagem.Caminho = Path.Combine(pastaRelativa, nomeArquivo).Replace("\\", "/");
            }

            var atualizado = await _repository.UpdateAsync(imagem);
            if (!atualizado)
            {
                return StatusCode(500, "Ocorreu um erro ao atualizar a imagem.");
            }
            return Ok(imagem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarImagem(int id)
        {
            var imagem = await _repository.GetByIdAsync(id);
            if (imagem == null)
            {
                return NotFound("Imagem nao encontrada");
            }
            var caminhoFisico = Path.Combine(Directory.GetCurrentDirectory(), imagem.Caminho.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (System.IO.File.Exists(caminhoFisico))
            {
                try
                {
                    System.IO.File.Delete(caminhoFisico);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Erro ao excluir o Arquivo: {ex.Message}");
                }

                var deletado = await _repository.DeleteAsync(id);
                if (!deletado)
                {
                    return StatusCode(500, "Erro ao excluir imagem do banco");
                }
                return NoContent();
            }
        }
    }
}
