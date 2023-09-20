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

namespace SipayTestCase.Application.Cqrs.Commands.User;

public class UserRegisterCommand:  IRequest<IDataResult<UserDto>>
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    
    public class UserRegisterCommandHandler: IRequestHandler<UserRegisterCommand,IDataResult<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public UserRegisterCommandHandler(IUserRepository userRepository, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public async Task<IDataResult<UserDto>> Handle(UserRegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(d => d.Email == request.Email);
            if (user != null)
                return new ErrorDataResult<UserDto>(
                    "Kullanıcı daha önce kayıt edilmiş. Şifrenizi hatırlamıyorsanız lütfen şifre hatırlatma hizmetini kullanınız.",
                    null);
            
            byte[] passwordHash, passwordSalt;
            Helpers.CreatePasswordHash(request.Password!, out passwordHash, out passwordSalt);
            
            user ??= new Domain.Entities.User()
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = request.Email,
                FullName = request.FullName,
                CreatedDate = DateTime.UtcNow,
            };
            
            await _userRepository.InsertAsync(user);
            
            var userDto = _mapper.Map<UserDto>(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email!),
                }),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            userDto.AuthToken = tokenString;
            
            return new SuccessDataResult<UserDto>("Kullanıcı başarılı olarak oluşturuldu.",userDto);
        }
    }
}