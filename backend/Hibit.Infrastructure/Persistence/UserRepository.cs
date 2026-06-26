using Hibit.Application.Common.Interfaces;
using Hibit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hibit.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Usuario?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default) =>
        _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public async Task AddAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        await _context.Usuarios.AddAsync(usuario, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        _context.Usuarios.AnyAsync(cancellationToken);
}
