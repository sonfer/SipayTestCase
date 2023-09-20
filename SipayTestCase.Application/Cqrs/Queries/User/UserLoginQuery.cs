using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SipayTestCase.Application.Interfaces;
using SipayTestCase.Contract.Commons;
using SipayTestCase.Contract.Dtos;
using SipayTestCase.Shared.Helpers;
using SipayTestCase.Shared.Interfaces;
using SipayTestCase.Shared.Responses;

namespace SipayTestCase.Application.Cqrs.Queries.User;

public class UserLoginQuery: IRequest<IDataResult<UserDto>>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    
    public class UserLoginQueryHandler:IRequestHandler<UserLoginQuery, IDataResult<UserDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;

        public UserLoginQueryHandler(IMapper mapper, IUserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<IDataResult<UserDto>> Handle(UserLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(d => d!.Email == request.Email);
            if (user is null)
                return new ErrorDataResult<UserDto>(
                    "Kullanıcı bulunamadı. Lütfen eposta adresinizi kontrol ederek yeniden deneyiniz.", null!);
            
            if (!Helpers.VerifyPasswordHash(request.Password!, user.PasswordHash!, user.PasswordSalt!))
                return new ErrorDataResult<UserDto>("Şifrenizi hatalı girdiniz!", null!);
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email!)
                }),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.AuthToken = tokenString;

            return new SuccessDataResult<UserDto>("Giriş başarılı", userDto);
        }
    }
}