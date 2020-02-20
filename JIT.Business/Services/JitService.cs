using AutoMapper;
using JIT.Business.Interfaces;
using JIT.Core.DTOs;
using JIT.Core.Entities;
using JIT.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JIT.Business.Services
{
    public class JitService : IJitService
    {

        private readonly IMapper _mapper;
        private readonly IJitRepository _jitRepository;

        public JitService(IMapper mapper, IJitRepository jitRepository)
        {
            _mapper = mapper;
            _jitRepository = jitRepository;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(UserDto user)
        {
            return await _jitRepository.UserExists(user.Username);
        }

        public void Delete(UserDto user)
        {
            var userFromDb = _mapper.Map<UserDto, User>(user);
            _jitRepository.Delete(userFromDb);
        }

        public async Task<ICollection<UserDto>> GetAllUsers()
        {
            var usersFromDb = await _jitRepository.GetAllUsers();
            return _mapper.Map<ICollection<User>, ICollection<UserDto>>(usersFromDb);
        }

        public async Task<UserDto> GetUserById(int id, bool includeWorkingHours = false)
        {
            var userFromDb = await _jitRepository.GetUserById(id, includeWorkingHours);

            if (includeWorkingHours)
            {
                return _mapper.Map<User, UserDto>(userFromDb);
            }

            return _mapper.Map<User, UserDto>(userFromDb);
        }

        public async Task<UserDto> Login(UserDto user)
        {
            var checkIfUserExist = await _jitRepository.UserExists(user.Username);

            if (!checkIfUserExist)
                return null;
            var userPassword = user.Password;

            var userFromDb = await GetUserByUsername(user);

            if (!VerifyPasswordHash(userPassword, userFromDb.PasswordHash, userFromDb.PasswordSalt)) return null;

            user.Id = userFromDb.Id;

            return user;
        }

        public async Task<UserDto> Register(UserDto user)
        {
            byte[] passwordHash, passwordSalt = { 0 };

            //saljemo referencu (pokazivac)
            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

            var userToDb = _mapper.Map<UserDto, User>(user);

            userToDb.PasswordHash = passwordHash;
            userToDb.PasswordSalt = passwordSalt;

            await _jitRepository.Register(userToDb);

            return user;
        }

        public void Update(UserDto user)
        {
            var userFromDb = _mapper.Map<UserDto, User>(user);
            _jitRepository.Update(userFromDb);
        }

        public async Task<User> GetUserByUsername(UserDto user)
        {
            return await _jitRepository.GetUserByUsername(user.Username);
        }

        public async Task<Project> SaveNewProject(ProjectDto project)
        {
            return await _jitRepository.SaveNewProject(_mapper.Map<ProjectDto, Project>(project));
        }

        public async Task<ICollection<ProjectDto>> GetAllProjectsByUserId(int userId)
        {
            var projectsFromDb = await _jitRepository.GetAllProjectsByUserId(userId);

            return _mapper.Map<ICollection<Project>, ICollection<ProjectDto>>(projectsFromDb);
        }

        public async Task<ICollection<ProjectDto>> GetAllProjectsBetweenDates(int userId, DateTime startDate, DateTime endDate)
        {
            var projectsFromDb = await _jitRepository.GetAllProjectsBetweenDates(userId, startDate, endDate);

            return _mapper.Map<ICollection<Project>, ICollection<ProjectDto>>(projectsFromDb);
        }

        public async Task<bool> AuthenticateUser(int id)
        {
            var userFromDb = await _jitRepository.GetUserById(id);
            if (userFromDb == null) return false;

            userFromDb.isAuthenticated = true;

            _jitRepository.Update(userFromDb);

            return true;
        }
    }
}
