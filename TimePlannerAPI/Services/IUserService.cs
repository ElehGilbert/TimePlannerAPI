using TimePlannerAPI.DTOs;

namespace TimePlannerAPI.Services
{
    public interface IUserService
    {
        Task<UserDTO> GetByIdAsync(Guid id);
        Task<UserDTO> UpdateAsync(Guid id, UpdateUserDto updateDto);
    }
}