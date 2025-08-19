using GaleriaOnline.WebApi.DbContextImagem;
using GaleriaOnline.WebApi.Interfaces;
using GaleriaOnline.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GaleriaOnline.WebApi.Repositories
{
    public class ImagemRepository : IImagemRepository
    {
        private readonly GaleriaOnlineDbContext _context;

        public ImagemRepository(GaleriaOnlineDbContext context)
        {
            _context = context;
        }
        public async Task<Imagem> CreateAsync(Imagem imagem)
        {
            await  _context.SaveChangesAsync();
            _context.Imagens.Add(imagem);
            return imagem;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Imagem = await _context.Imagens.FindAsync(id);
            if (Imagem == null)
            {
                return false;
            }
            _context.Imagens.Remove(Imagem);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Imagem>> GetAllAsync()
        {
            return await _context.Imagens.ToListAsync();
        }

        public async Task<Imagem?> GetByIdAsync(int id)
        {
            return await _context.Imagens.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Imagem imagem)
        {
            _context.Imagens.Update(imagem);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
