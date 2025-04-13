using TimePlannerAPI.DTOs;

namespace TimePlannerAPI.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateDto);
    }
}